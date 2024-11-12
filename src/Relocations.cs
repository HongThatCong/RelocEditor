using System;
using System.Collections.Generic;
using System.IO;

namespace RelocEditor
{
    public class Relocations
    {
        public struct Page
        {
            public UInt64 address;
            public uint size;
            public uint count;
        }

        public struct Reloc
        {
            public ushort offset;
            public PeHeader.BASE_RELOCATION_TYPE type;
        }

        private readonly SortedDictionary<UInt64, List<Reloc>> pages;
        private string filePath;

        private readonly PeHeader pehr;

        public bool IsNotSaved { get; private set; }

        /// <summary>
        /// Open the file specified into the <paramref name="path"/>, read the relocation section
        /// and load it.
        /// </summary>
        /// <param name="path"></param>
        public Relocations(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException();

            this.filePath = path;
            this.IsNotSaved = false;

            pehr = new PeHeader(path);

            if (!pehr.IsValidDirectory(PeHeader.IMAGE_DIRECTORY_ENTRY_BASERELOC))
            {
                throw new InvalidOperationException("Image has no relocation table");
            }

            // reading relocation section data
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BinaryReader reader = new BinaryReader(stream);

                PeHeader.IMAGE_DATA_DIRECTORY baseRelocPtr = pehr.GetImageDirectory(PeHeader.IMAGE_DIRECTORY_ENTRY_BASERELOC);
                PeHeader.IMAGE_SECTION_HEADER baseRelocSection = pehr.GetSectionInfo(PeHeader.IMAGE_DIRECTORY_ENTRY_BASERELOC);

                reader.BaseStream.Seek(baseRelocSection.PointerToRawData, SeekOrigin.Begin);

                pages = new SortedDictionary<UInt64, List<Reloc>>();

                PeHeader.IMAGE_RELOCATION_BLOCK relBlock;

                while (reader.BaseStream.Position < baseRelocSection.PointerToRawData + baseRelocPtr.Size) // 4K block loop
                {
                    relBlock = PeHeader.FromBinaryReader<PeHeader.IMAGE_RELOCATION_BLOCK>(reader);

                    uint size = relBlock.SizeOfBlock;
                    uint count = (size - 8) / 2;

                    List<Reloc> relocs = new List<Reloc>();

                    for (int i = 0; i < count; i++) // offsets loop
                    {
                        // WORD     0000000000000000b
                        // 0x3041   0011000001000001b
                        // 0X0FFF   0000111111111111b
                        ushort data = reader.ReadUInt16();

                        Reloc r = new Reloc
                        {
                            offset = (ushort)(data & 0x0FFF),
                            type = (PeHeader.BASE_RELOCATION_TYPE)(((data & 0xF000) >> 12) & 0xF)
                        };

                        relocs.Add(r);
                    }

                    pages.Add(relBlock.RvaOfPage, relocs);
                }

                reader.Close();
            }
        }

        /// <summary>
        /// Edit an specific address
        /// </summary>
        /// <param name="address">Old address</param>
        /// <param name="newAddress">New address</param>
        /// <param name="newType">New type</param>
        /// <returns>True if edited with success, else false</returns>
        public bool EditRelocation(UInt64 address, UInt64 newAddress, PeHeader.BASE_RELOCATION_TYPE newType)
        {
            UInt64 oldAddress = (address & 0xFFFFFFFFF000) - pehr.GetImageBase32Or64();
            ushort oldOffset = (ushort)(address & 0x00000FFF);

            PeHeader.BASE_RELOCATION_TYPE oldType = PeHeader.BASE_RELOCATION_TYPE.IMAGE_REL_BASED_ABSOLUTE;

            if (!pages.TryGetValue(oldAddress, out List<Reloc> relocs))
                return false;

            foreach (Reloc reloc in relocs)
            {
                if (reloc.offset == oldOffset)
                {
                    oldType = reloc.type;
                    break;
                }
            }

            if (oldType == PeHeader.BASE_RELOCATION_TYPE.IMAGE_REL_BASED_ABSOLUTE)
                return false;

            // delete old address and add new address. If not success, restore old address
            if (!DeleteRelocation(address))
                return false;

            if (AddRelocation(newAddress, newType) <= 0)
            {
                AddRelocation(address, oldType);
                return false;
            }

            IsNotSaved = true;
            return true;
        }

        /// <summary>
        /// Remove a specific address
        /// </summary>
        /// <param name="address">Address to remove</param>
        /// <returns>True if removed with success, else false</returns>
        public bool DeleteRelocation(UInt64 address)
        {
            PeHeader.IMAGE_DATA_DIRECTORY baseRelocPtr = pehr.GetImageDirectory(PeHeader.IMAGE_DIRECTORY_ENTRY_BASERELOC);

            // search if 4K address exists
            UInt64 page = (address & 0xFFFFF000) - pehr.GetImageBase32Or64();
            if (!pages.TryGetValue(page, out List<Reloc> relocs))
                return false;

            ushort offset = (ushort)(address & 0x0FFF);

            // search if offset exists
            foreach (Reloc reloc in relocs)
            {
                if (reloc.offset == offset && reloc.type != PeHeader.BASE_RELOCATION_TYPE.IMAGE_REL_BASED_ABSOLUTE)
                {
                    relocs.Remove(reloc);
                    baseRelocPtr.Size -= 2;

                    if (relocs.Count % 2 != 0) // align in 32bits
                    {
                        bool isAlignDeleted = false;

                        foreach (Reloc item in relocs) // search if align already exists
                        {
                            if (item.offset == 0 && item.type == PeHeader.BASE_RELOCATION_TYPE.IMAGE_REL_BASED_ABSOLUTE)
                            {
                                relocs.Remove(item);
                                isAlignDeleted = true;
                                baseRelocPtr.Size -= 2;
                                break;
                            }
                        }

                        if (!isAlignDeleted) // if no align reloc found, add it
                        {
                            Reloc item = new Reloc
                            {
                                offset = 0,
                                type = PeHeader.BASE_RELOCATION_TYPE.IMAGE_REL_BASED_ABSOLUTE
                            };
                            relocs.Add(item);
                            baseRelocPtr.Size += 2;
                        }
                    }

                    if (relocs.Count == 0) // remove page if nothing offset
                    {
                        pages.Remove(page);
                        baseRelocPtr.Size -= 8;
                    }

                    IsNotSaved = true;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Add a relocation address
        /// </summary>
        /// <param name="address">Address to add</param>
        /// <param name="type">Type of relocation</param>
        /// <returns>-1 if duplicated, 0 if not added, 1 if added a page and 2 if added only in reloc of page</returns>
        public int AddRelocation(UInt64 address, PeHeader.BASE_RELOCATION_TYPE type)
        {
            PeHeader.IMAGE_DATA_DIRECTORY baseRelocPtr = pehr.GetImageDirectory(PeHeader.IMAGE_DIRECTORY_ENTRY_BASERELOC);
            PeHeader.IMAGE_SECTION_HEADER baseRelocSection = pehr.GetSectionInfo(PeHeader.IMAGE_DIRECTORY_ENTRY_BASERELOC);

            if (address < pehr.GetImageBase32Or64())
                return 0;

            UInt64 page = (address & 0xFFFFF000) - pehr.GetImageBase32Or64();
            ushort offset = (ushort)(address & 0x00000FFF);

            if (!pages.ContainsKey(page)) // create a new page if doesn't exists
            {
                if (baseRelocSection.SizeOfRawData - baseRelocPtr.Size < 12)
                    return 0;

                List<Reloc> relocs = new List<Reloc>();

                Reloc reloc = new Reloc
                {
                    offset = offset,
                    type = type
                };
                relocs.Add(reloc);

                reloc = new Reloc
                {
                    offset = 0,
                    type = PeHeader.BASE_RELOCATION_TYPE.IMAGE_REL_BASED_ABSOLUTE
                };
                relocs.Add(reloc);

                pages.Add(page, relocs);

                baseRelocPtr.Size += 12;

                IsNotSaved = true;
                return 1;
            }
            else // just added the offset
            {
                if (!pages.TryGetValue(page, out List<Reloc> relocs))
                    return 0;

                foreach (Reloc item in relocs) // search if address already present
                {
                    if (item.offset == offset && item.type != PeHeader.BASE_RELOCATION_TYPE.IMAGE_REL_BASED_ABSOLUTE)
                        return -1;
                }

                if (relocs.Count % 2 == 0) // align in 32bits
                {
                    bool isAlignDeleted = false;

                    foreach (Reloc item in relocs) // search if align already exists
                    {
                        if (item.offset == 0 && item.type == PeHeader.BASE_RELOCATION_TYPE.IMAGE_REL_BASED_ABSOLUTE)
                        {
                            relocs.Remove(item);
                            isAlignDeleted = true;
                            baseRelocPtr.Size -= 2;
                            break;
                        }
                    }

                    if (!isAlignDeleted) // if no align reloc found, add it
                    {
                        if (baseRelocSection.SizeOfRawData - baseRelocPtr.Size < 4)
                            return 0;

                        Reloc item = new Reloc
                        {
                            offset = 0,
                            type = PeHeader.BASE_RELOCATION_TYPE.IMAGE_REL_BASED_ABSOLUTE
                        };
                        relocs.Add(item);
                        baseRelocPtr.Size += 2;
                    }
                }

                Reloc reloc = new Reloc
                {
                    offset = offset,
                    type = type
                };
                relocs.Add(reloc);
                baseRelocPtr.Size += 2;

                IsNotSaved = true;
                return 2;
            }
        }

        /// <summary>
        /// Obtain a list of pages available
        /// </summary>
        /// <returns>A list of pages available</returns>
        public List<Page> GetPages()
        {
            List<Page> result = new List<Page>();

            SortedDictionary<UInt64, List<Reloc>>.Enumerator enumerator = pages.GetEnumerator();

            while (enumerator.MoveNext())
            {
                uint count = (uint)enumerator.Current.Value.Count;

                Page p = new Page
                {
                    address = enumerator.Current.Key + pehr.GetImageBase32Or64(),
                    size = (count * 2) + 8,
                    count = count
                };
                result.Add(p);
            }

            return result;
        }

        /// <summary>
        /// Try to obtain a list of relocations for an address given
        /// </summary>
        /// <param name="baseAddress">Base address of relocations</param>
        /// <param name="relocs">List of relocations for this address</param>
        /// <returns>True if relocations given with success, else false</returns>
        public bool TryGetRelocs(UInt64 baseAddress, out List<Reloc> relocs)
        {
            UInt64 page = baseAddress - pehr.GetImageBase32Or64();
            return pages.TryGetValue(page, out relocs);
        }

        /// <summary>
        /// Obtain the Size of Relocation section into the RAM
        /// </summary>
        /// <returns>The virtual size</returns>
        public uint GetVirtualSize()
        {
            PeHeader.IMAGE_DATA_DIRECTORY baseRelocPtr = pehr.GetImageDirectory(PeHeader.IMAGE_DIRECTORY_ENTRY_BASERELOC);
            return baseRelocPtr.Size;
        }

        /// <summary>
        /// Obtain the Size of Relocation section into the disk
        /// </summary>
        /// <returns>The RAW size</returns>
        public uint GetRawSize()
        {
            PeHeader.IMAGE_SECTION_HEADER baseRelocSection = pehr.GetSectionInfo(PeHeader.IMAGE_DIRECTORY_ENTRY_BASERELOC);
            return baseRelocSection.SizeOfRawData;
        }

        /// <summary>
        /// Obtain the file path
        /// </summary>
        /// <returns>The file path</returns>
        public string GetPath()
        {
            return filePath;
        }

        /// <summary>
        /// Obtain VirtualAddress
        /// </summary>
        /// <returns>VirtualAddress</returns>
        public uint GetVirtualAddress()
        {
            PeHeader.IMAGE_DATA_DIRECTORY baseRelocPtr = pehr.GetImageDirectory(PeHeader.IMAGE_DIRECTORY_ENTRY_BASERELOC);
            return baseRelocPtr.RVA;
        }

        /// <summary>
        /// Obtain ImageBase
        /// </summary>
        /// <returns>imageBase</returns>
        public UInt64 GetImageBase()
        {
            return pehr.GetImageBase32Or64();
        }

        /// <summary>
        /// Obtain RawAddress
        /// </summary>
        /// <returns>RawAddress</returns>
        public uint GetRawAddress()
        {
            PeHeader.IMAGE_SECTION_HEADER baseRelocSection = pehr.GetSectionInfo(PeHeader.IMAGE_DIRECTORY_ENTRY_BASERELOC);
            return baseRelocSection.PointerToRawData;
        }

        /// <summary>
        /// Write the relocations section into the file
        /// </summary>
        /// <returns>True if written with success, else false</returns>
        public bool WriteRelocations(string newPath = "")
        {
            PeHeader.IMAGE_DATA_DIRECTORY baseRelocPtr = pehr.GetImageDirectory(PeHeader.IMAGE_DIRECTORY_ENTRY_BASERELOC);
            PeHeader.IMAGE_SECTION_HEADER baseRelocSection = pehr.GetSectionInfo(PeHeader.IMAGE_DIRECTORY_ENTRY_BASERELOC);
            uint offsetVirtualSize = pehr.GetDirectoryOffset(PeHeader.IMAGE_DIRECTORY_ENTRY_BASERELOC) + 4;

            if (!File.Exists(filePath))
                return false;

            if (!string.IsNullOrEmpty(newPath) && newPath != filePath) // save as
            {
                try
                {
                    File.Copy(filePath, newPath, true);
                    filePath = newPath;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            BinaryWriter bw = new BinaryWriter(new FileStream(filePath, FileMode.Open, FileAccess.Write, FileShare.None));

            // write new relocation size
            bw.BaseStream.Seek(offsetVirtualSize, SeekOrigin.Begin);
            bw.Write(baseRelocPtr.Size);

            // go to beginning of relocation section
            bw.BaseStream.Seek(baseRelocSection.PointerToRawData, SeekOrigin.Begin);

            foreach (KeyValuePair<UInt64, List<Reloc>> page in pages)
            {
                // write page
                bw.Write((uint)(page.Key & 0x0FFFFFFFF));
                bw.Write((uint)(page.Value.Count * 2 + 8));

                foreach (Reloc reloc in page.Value)
                {
                    // write reloc
                    ushort temp = (ushort)((((ushort)reloc.type << 12) & 0xF000) | (reloc.offset & 0xFFF));
                    bw.Write(temp);
                }
            }

            // fill the end with null bytes
            while (bw.BaseStream.Position < baseRelocSection.PointerToRawData + baseRelocSection.SizeOfRawData)
                bw.Write((uint)0x00000000);

            bw.Close();

            IsNotSaved = false;
            return true;
        }
    }
}
