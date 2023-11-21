using System;
using System.IO;
using System.Runtime.InteropServices;

namespace RelocEditor
{
    public class PeHeader
    {
        // IMAGE_FILE_HEADER > Machine
        public const UInt16 IMAGE_FILE_MACHINE_I386 = 0x014C;
        public const UInt16 IMAGE_FILE_MACHINE_IA64 = 0x0200;
        public const UInt16 IMAGE_FILE_MACHINE_AMD64 = 0x8664;

        // IMAGE_FILE_HEADER > Characteristics
        public const UInt16 IMAGE_FILE_RELOCS_STRIPPED = 0x0001;
        public const UInt16 IMAGE_FILE_EXECUTABLE_IMAGE = 0x0002;
        public const UInt16 IMAGE_FILE_LINE_NUMS_STRIPPED = 0x0004;
        public const UInt16 IMAGE_FILE_LOCAL_SYMS_STRIPPED = 0x0008;
        public const UInt16 IMAGE_FILE_AGGRESIVE_WS_TRIM = 0x0010;
        public const UInt16 IMAGE_FILE_LARGE_ADDRESS_AWARE = 0x0020;
        public const UInt16 IMAGE_FILE_BYTES_REVERSED_LO = 0x0080;
        public const UInt16 IMAGE_FILE_32BIT_MACHINE = 0x0100;
        public const UInt16 IMAGE_FILE_DEBUG_STRIPPED = 0x0200;
        public const UInt16 IMAGE_FILE_REMOVABLE_RUN_FROM_SWAP = 0x0400;
        public const UInt16 IMAGE_FILE_NET_RUN_FROM_SWAP = 0x0800;
        public const UInt16 IMAGE_FILE_SYSTEM = 0x1000;
        public const UInt16 IMAGE_FILE_DLL = 0x2000;
        public const UInt16 IMAGE_FILE_UP_SYSTEM_ONLY = 0x4000;
        public const UInt16 IMAGE_FILE_BYTES_REVERSED_HI = 0x8000;

        // IMAGE_OPTIONAL_HEADER > Subsystem
        public const UInt16 IMAGE_SUBSYSTEM_UNKNOWN = 0;
        public const UInt16 IMAGE_SUBSYSTEM_NATIVE = 1;
        public const UInt16 IMAGE_SUBSYSTEM_WINDOWS_GUI = 2;
        public const UInt16 IMAGE_SUBSYSTEM_WINDOWS_CUI = 3;
        public const UInt16 IMAGE_SUBSYSTEM_OS2_CUI = 5;
        public const UInt16 IMAGE_SUBSYSTEM_POSIX_CUI = 7;
        public const UInt16 IMAGE_SUBSYSTEM_WINDOWS_CE_GUI = 9;
        public const UInt16 IMAGE_SUBSYSTEM_EFI_APPLICATION = 10;
        public const UInt16 IMAGE_SUBSYSTEM_EFI_BOOT_SERVICE_DRIVER = 11;
        public const UInt16 IMAGE_SUBSYSTEM_EFI_RUNTIME_DRIVER = 12;
        public const UInt16 IMAGE_SUBSYSTEM_EFI_ROM = 13;
        public const UInt16 IMAGE_SUBSYSTEM_XBOX = 14;
        public const UInt16 IMAGE_SUBSYSTEM_WINDOWS_BOOT_APPLICATION = 16;

        // IMAGE_OPTIONAL_HEADER > DllCharacteristics
        public const UInt16 IMAGE_DLLCHARACTERISTICS_DYNAMIC_BASE = 0x0040;
        public const UInt16 IMAGE_DLLCHARACTERISTICS_FORCE_INTEGRITY = 0x0080;
        public const UInt16 IMAGE_DLLCHARACTERISTICS_NX_COMPAT = 0x0100;
        public const UInt16 IMAGE_DLLCHARACTERISTICS_NO_ISOLATION = 0x0200;
        public const UInt16 IMAGE_DLLCHARACTERISTICS_NO_SEH = 0x0400;
        public const UInt16 IMAGE_DLLCHARACTERISTICS_NO_BIND = 0x0800;
        public const UInt16 IMAGE_DLLCHARACTERISTICS_WDM_DRIVER = 0x2000;
        public const UInt16 IMAGE_DLLCHARACTERISTICS_TERMINAL_SERVER_AWARE = 0x8000;

        // IMAGE_DOS_HEADER > e_magic
        public const UInt16 IMAGE_DOS_SIGNATURE = 0x05A4D;

        // IMAGE_NT_HEADERS > Signature
        public const UInt32 IMAGE_NT_SIGNATURE = 0x04550;

        // IMAGE_OPTIONAL_HEADER > Magic
        public const UInt16 IMAGE_NT_OPTIONAL_HDR32_MAGIC = 0x10B;
        public const UInt16 IMAGE_NT_OPTIONAL_HDR64_MAGIC = 0x20B;

        // IMAGE_DATA_DIRECTORY[IMAGE_OPTIONAL_HEADER > NumberOfRvaAndSizes]
        public const UInt16 IMAGE_DIRECTORY_ENTRY_EXPORT = 0;
        public const UInt16 IMAGE_DIRECTORY_ENTRY_IMPORT = 1;
        public const UInt16 IMAGE_DIRECTORY_ENTRY_RESOURCE = 2;
        public const UInt16 IMAGE_DIRECTORY_ENTRY_EXCEPTION = 3;
        public const UInt16 IMAGE_DIRECTORY_ENTRY_SECURITY = 4;
        public const UInt16 IMAGE_DIRECTORY_ENTRY_BASERELOC = 5;
        public const UInt16 IMAGE_DIRECTORY_ENTRY_DEBUG = 6;
        public const UInt16 IMAGE_DIRECTORY_ENTRY_ARCHITECTURE = 7;
        public const UInt16 IMAGE_DIRECTORY_ENTRY_GLOBALPTR = 8;
        public const UInt16 IMAGE_DIRECTORY_ENTRY_TLS = 9;
        public const UInt16 IMAGE_DIRECTORY_ENTRY_LOAD_CONFIG = 10;
        public const UInt16 IMAGE_DIRECTORY_ENTRY_BOUND_IMPORT = 11;
        public const UInt16 IMAGE_DIRECTORY_ENTRY_IAT = 12;
        public const UInt16 IMAGE_DIRECTORY_ENTRY_DELAY_IMPORT = 13;
        public const UInt16 IMAGE_DIRECTORY_ENTRY_COM_DESCRIPTOR = 14;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMAGE_DOS_HEADER
        {
            public UInt16 e_magic;
            public UInt16 e_cblp;
            public UInt16 e_cp;
            public UInt16 e_crlc;
            public UInt16 e_cparhdr;
            public UInt16 e_minalloc;
            public UInt16 e_maxalloc;
            public UInt16 e_ss;
            public UInt16 e_sp;
            public UInt16 e_csum;
            public UInt16 e_ip;
            public UInt16 e_cs;
            public UInt16 e_lfarlc;
            public UInt16 e_ovno;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt16[] e_res;
            public UInt16 e_oemid;
            public UInt16 e_oeminfo;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public UInt16[] e_res2;
            public UInt32 e_lfanew;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMAGE_FILE_HEADER
        {
            public UInt16 Machine;
            public UInt16 NumberOfSections;
            public UInt32 TimeDateStamp;
            public UInt32 PointerToSymbolTable;
            public UInt32 NumberOfSymbols;
            public UInt16 SizeOfOptionalHeader;
            public UInt16 Characteristics;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMAGE_NT_HEADERS64
        {
            public UInt32 Signature;
            public IMAGE_FILE_HEADER FileHeader;
            public IMAGE_OPTIONAL_HEADER64 OptionalHeader;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMAGE_NT_HEADERS32
        {
            public UInt32 Signature;
            public IMAGE_FILE_HEADER FileHeader;
            public IMAGE_OPTIONAL_HEADER32 OptionalHeader;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMAGE_OPTIONAL_HEADER32
        {
            public UInt16 Magic;
            public Byte MajorLinkerVersion;
            public Byte MinorLinkerVersion;
            public UInt32 SizeOfCode;
            public UInt32 SizeOfInitializedData;
            public UInt32 SizeOfUninitializedData;
            public UInt32 AddressOfEntryPoint;
            public UInt32 BaseOfCode;
            public UInt32 BaseOfData;
            public UInt32 ImageBase;
            public UInt32 SectionAlignment;
            public UInt32 FileAlignment;
            public UInt16 MajorOperatingSystemVersion;
            public UInt16 MinorOperatingSystemVersion;
            public UInt16 MajorImageVersion;
            public UInt16 MinorImageVersion;
            public UInt16 MajorSubsystemVersion;
            public UInt16 MinorSubsystemVersion;
            public UInt32 Win32VersionValue;
            public UInt32 SizeOfImage;
            public UInt32 SizeOfHeaders;
            public UInt32 CheckSum;
            public UInt16 Subsystem;
            public UInt16 DllCharacteristics;
            public UInt32 SizeOfStackReserve;
            public UInt32 SizeOfStackCommit;
            public UInt32 SizeOfHeapReserve;
            public UInt32 SizeOfHeapCommit;
            public UInt32 LoaderFlags;
            public UInt32 NumberOfRvaAndSizes;
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            //public IMAGE_DATA_DIRECTORY[] DataDirectory;
            //
            //public IMAGE_DATA_DIRECTORY ExportTableDirectory;
            //public IMAGE_DATA_DIRECTORY ImportTableDirectory;
            //public IMAGE_DATA_DIRECTORY ResourceTableDirectory;
            //public IMAGE_DATA_DIRECTORY ExceptionTableDirectory;
            //public IMAGE_DATA_DIRECTORY CertificateTableDirectory;
            //public IMAGE_DATA_DIRECTORY BaseRelocationTableDirectory;
            //public IMAGE_DATA_DIRECTORY DebugDirectory;
            //public IMAGE_DATA_DIRECTORY ArchitectureDirectory;
            //public IMAGE_DATA_DIRECTORY GlobalPtrDirectory;
            //public IMAGE_DATA_DIRECTORY TLSTableDirectory;
            //public IMAGE_DATA_DIRECTORY LoadConfigTableDirectory;
            //public IMAGE_DATA_DIRECTORY BoundImportDirectory;
            //public IMAGE_DATA_DIRECTORY IATDirectory;
            //public IMAGE_DATA_DIRECTORY DelayImportDescriptorDirectory;
            //public IMAGE_DATA_DIRECTORY CLRRuntimeHeaderDirectory;
            //public IMAGE_DATA_DIRECTORY ReservedDirectory;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMAGE_OPTIONAL_HEADER64
        {
            public UInt16 Magic;
            public Byte MajorLinkerVersion;
            public Byte MinorLinkerVersion;
            public UInt32 SizeOfCode;
            public UInt32 SizeOfInitializedData;
            public UInt32 SizeOfUninitializedData;
            public UInt32 AddressOfEntryPoint;
            public UInt32 BaseOfCode;
            public UInt64 ImageBase;
            public UInt32 SectionAlignment;
            public UInt32 FileAlignment;
            public UInt16 MajorOperatingSystemVersion;
            public UInt16 MinorOperatingSystemVersion;
            public UInt16 MajorImageVersion;
            public UInt16 MinorImageVersion;
            public UInt16 MajorSubsystemVersion;
            public UInt16 MinorSubsystemVersion;
            public UInt32 Win32VersionValue;
            public UInt32 SizeOfImage;
            public UInt32 SizeOfHeaders;
            public UInt32 CheckSum;
            public UInt16 Subsystem;
            public UInt16 DllCharacteristics;
            public UInt64 SizeOfStackReserve;
            public UInt64 SizeOfStackCommit;
            public UInt64 SizeOfHeapReserve;
            public UInt64 SizeOfHeapCommit;
            public UInt32 LoaderFlags;
            public UInt32 NumberOfRvaAndSizes;
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            //public IMAGE_DATA_DIRECTORY[] DataDirectory;
            //
            //public IMAGE_DATA_DIRECTORY ExportTableDirectory;
            //public IMAGE_DATA_DIRECTORY ImportTableDirectory;
            //public IMAGE_DATA_DIRECTORY ResourceTableDirectory;
            //public IMAGE_DATA_DIRECTORY ExceptionTableDirectory;
            //public IMAGE_DATA_DIRECTORY CertificateTableDirectory;
            //public IMAGE_DATA_DIRECTORY BaseRelocationTableDirectory;
            //public IMAGE_DATA_DIRECTORY DebugDirectory;
            //public IMAGE_DATA_DIRECTORY ArchitectureDirectory;
            //public IMAGE_DATA_DIRECTORY GlobalPtrDirectory;
            //public IMAGE_DATA_DIRECTORY TLSTableDirectory;
            //public IMAGE_DATA_DIRECTORY LoadConfigTableDirectory;
            //public IMAGE_DATA_DIRECTORY BoundImportDirectory;
            //public IMAGE_DATA_DIRECTORY IATDirectory;
            //public IMAGE_DATA_DIRECTORY DelayImportDescriptorDirectory;
            //public IMAGE_DATA_DIRECTORY CLRRuntimeHeaderDirectory;
            //public IMAGE_DATA_DIRECTORY ReservedDirectory;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMAGE_DATA_DIRECTORY
        {
            public UInt32 RVA;
            public UInt32 Size;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMAGE_SECTION_HEADER
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public Byte[] Name;
            public UInt32 VirtualSize;
            public UInt32 VirtualAddress;
            public UInt32 SizeOfRawData;
            public UInt32 PointerToRawData;
            public UInt32 PointerToRelocations;
            public UInt32 PointerToLinenumbers;
            public UInt16 NumberOfRelocations;
            public UInt16 NumberOfLinenumbers;
            public DataSectionFlags Characteristics;
        }

        [Flags]
        public enum DataSectionFlags : UInt32
        {
            TypeReg = 0x00000000,
            TypeDsect = 0x00000001,
            TypeNoLoad = 0x00000002,
            TypeGroup = 0x00000004,
            TypeNoPadded = 0x00000008,
            TypeCopy = 0x00000010,
            ContentCode = 0x00000020,
            ContentInitializedData = 0x00000040,
            ContentUninitializedData = 0x00000080,
            LinkOther = 0x00000100,
            LinkInfo = 0x00000200,
            TypeOver = 0x00000400,
            LinkRemove = 0x00000800,
            LinkComDat = 0x00001000,
            NoDeferSpecExceptions = 0x00004000,
            RelativeGP = 0x00008000,
            MemoryPurgeable = 0x00020000,
            MemoryLocked = 0x00040000,
            MemoryPreload = 0x00080000,
            Align1Bytes = 0x00100000,
            Align2Bytes = 0x00200000,
            Align4Bytes = 0x00300000,
            Align8Bytes = 0x00400000,
            Align16Bytes = 0x00500000,
            Align32Bytes = 0x00600000,
            Align64Bytes = 0x00700000,
            Align128Bytes = 0x00800000,
            Align256Bytes = 0x00900000,
            Align512Bytes = 0x00A00000,
            Align1024Bytes = 0x00B00000,
            Align2048Bytes = 0x00C00000,
            Align4096Bytes = 0x00D00000,
            Align8192Bytes = 0x00E00000,
            LinkExtendedRelocationOverflow = 0x01000000,
            MemoryDiscardable = 0x02000000,
            MemoryNotCached = 0x04000000,
            MemoryNotPaged = 0x08000000,
            MemoryShared = 0x10000000,
            MemoryExecute = 0x20000000,
            MemoryRead = 0x40000000,
            MemoryWrite = 0x80000000
        }

        [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMAGE_RELOCATION_BLOCK
        {
            public UInt32 RvaOfPage;
            public UInt32 SizeOfBlock;
        }

        [Flags]
        public enum BASE_RELOCATION_TYPE : Byte
        {
            IMAGE_REL_BASED_ABSOLUTE = 0,
            IMAGE_REL_BASED_HIGH = 1,
            IMAGE_REL_BASED_LOW = 2,
            IMAGE_REL_BASED_HIGHLOW = 3,
            IMAGE_REL_BASED_HIGHADJ = 4,
            IMAGE_REL_BASED_MIPS_JMPADDR = 5,
            IMAGE_REL_BASED_SECTION = 6,
            IMAGE_REL_BASED_REL32 = 7,
            IMAGE_REL_BASED_RESERVED = 8,
            IMAGE_REL_BASED_MIPS_JMPADDR16 = 9,
            IMAGE_REL_BASED_DIR64 = 10
        }

        public IMAGE_DOS_HEADER DosHeader { get; }

        public IMAGE_FILE_HEADER FileHeader { get; }

        public IMAGE_OPTIONAL_HEADER32 OptionalHeader32 { get; }

        public IMAGE_OPTIONAL_HEADER64 OptionalHeader64 { get; }

        public IMAGE_DATA_DIRECTORY[] DataDirectories { get; }

        public IMAGE_SECTION_HEADER[] ImageSectionHeaders { get; }

        public PeHeader(string filePath)
        {
            // Read in the DLL or EXE and get the timestamp
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BinaryReader reader = new BinaryReader(stream);

                // Read dos header
                DosHeader = FromBinaryReader<IMAGE_DOS_HEADER>(reader);

                // Check MZ signature
                if (DosHeader.e_magic != IMAGE_DOS_SIGNATURE)
                {
                    reader.Close();
                    throw new InvalidOperationException("Invalid MZ header signature was found");
                }

                // Add 4 bytes to the offset
                reader.BaseStream.Seek(DosHeader.e_lfanew, SeekOrigin.Begin);

                // Check NT signature
                UInt32 ntHeadersSignature = reader.ReadUInt32();
                if (ntHeadersSignature != IMAGE_NT_SIGNATURE)
                {
                    reader.Close();
                    throw new InvalidOperationException("Invalid NT header signature was found");
                }

                // Read file header
                FileHeader = FromBinaryReader<IMAGE_FILE_HEADER>(reader);
                if (this.Is32BitMachine)
                {
                    // This is 32-bit image
                    OptionalHeader32 = FromBinaryReader<IMAGE_OPTIONAL_HEADER32>(reader);
                    if (OptionalHeader32.Magic != IMAGE_NT_OPTIONAL_HDR32_MAGIC)
                    {
                        reader.Close();
                        throw new InvalidOperationException("Malformed 32-bit image was found");
                    }

                    // Read directories
                    var validRvaAndSize = Math.Min(OptionalHeader32.NumberOfRvaAndSizes, 16);
                    DataDirectories = new IMAGE_DATA_DIRECTORY[validRvaAndSize];
                    for (uint dirNo = 0; dirNo < DataDirectories.Length; dirNo++)
                    {
                        DataDirectories[dirNo] = FromBinaryReader<IMAGE_DATA_DIRECTORY>(reader);
                    }
                }
                else if (this.Is64BitMachine)
                {
                    // This is 64-bit image
                    OptionalHeader64 = FromBinaryReader<IMAGE_OPTIONAL_HEADER64>(reader);
                    if (OptionalHeader64.Magic != IMAGE_NT_OPTIONAL_HDR64_MAGIC)
                    {
                        reader.Close();
                        throw new InvalidOperationException("Malformed 64-bit image was found");
                    }

                    // Read directories
                    var validRvaAndSize = Math.Min(OptionalHeader64.NumberOfRvaAndSizes, 16);
                    DataDirectories = new IMAGE_DATA_DIRECTORY[validRvaAndSize];
                    for (uint dirNo = 0; dirNo < DataDirectories.Length; dirNo++)
                    {
                        DataDirectories[dirNo] = FromBinaryReader<IMAGE_DATA_DIRECTORY>(reader);
                    }
                }
                else
                {
                    // This is unknown image
                    reader.Close();
                    throw new InvalidOperationException("Unsupported image was found");
                }

                // Read sections
                ImageSectionHeaders = new IMAGE_SECTION_HEADER[FileHeader.NumberOfSections];
                for (uint headerNo = 0; headerNo < ImageSectionHeaders.Length; headerNo++)
                {
                    ImageSectionHeaders[headerNo] = FromBinaryReader<IMAGE_SECTION_HEADER>(reader);
                }

                // Finish
                reader.Close();
            }
        }

        public UInt32 GetOffsetFromRVA(UInt32 rva)
        {
            for (uint headerNo = 0; headerNo < ImageSectionHeaders.Length; headerNo++)
            {
                if ((rva >= ImageSectionHeaders[headerNo].VirtualAddress) && (rva < (ImageSectionHeaders[headerNo].VirtualAddress + ((ImageSectionHeaders[headerNo].VirtualSize > 0) ? ImageSectionHeaders[headerNo].VirtualSize : ImageSectionHeaders[headerNo].SizeOfRawData))))
                {
                    return ((rva - ImageSectionHeaders[headerNo].VirtualAddress) + ImageSectionHeaders[headerNo].PointerToRawData);
                }
            }

            return 0;
        }

        public Boolean IsValidDirectory(UInt16 imgDir)
        {
            return (imgDir >= IMAGE_DIRECTORY_ENTRY_EXPORT && imgDir <= DataDirectories.Length);
        }

        public IMAGE_SECTION_HEADER GetSectionInfo(UInt16 imgDir)
        {
            if (IsValidDirectory(imgDir))
            {
                for (uint headerNo = 0; headerNo < ImageSectionHeaders.Length; headerNo++)
                {
                    if ((DataDirectories[imgDir].RVA >= ImageSectionHeaders[headerNo].VirtualAddress) && 
                        (DataDirectories[imgDir].RVA < (ImageSectionHeaders[headerNo].VirtualAddress + 
                        ((ImageSectionHeaders[headerNo].VirtualSize > 0) ? ImageSectionHeaders[headerNo].VirtualSize : ImageSectionHeaders[headerNo].SizeOfRawData))))
                    {
                        return ImageSectionHeaders[headerNo];
                    }
                }
            }

            return new IMAGE_SECTION_HEADER { };
        }

        public UInt32 GetSectionIndex(UInt16 imgDir)
        {
            if (IsValidDirectory(imgDir))
            {
                for (uint headerNo = 0; headerNo < ImageSectionHeaders.Length; headerNo++)
                {
                    if ((DataDirectories[imgDir].RVA >= ImageSectionHeaders[headerNo].VirtualAddress) && 
                        (DataDirectories[imgDir].RVA < (ImageSectionHeaders[headerNo].VirtualAddress + ((ImageSectionHeaders[headerNo].VirtualSize > 0) ? ImageSectionHeaders[headerNo].VirtualSize : ImageSectionHeaders[headerNo].SizeOfRawData))))
                    {
                        return headerNo;
                    }
                }
            }
            return 0;
        }

        public UInt32 GetDirectoryOffset(UInt16 imgDir)
        {
            if (IsValidDirectory(imgDir))
            {
                if (this.Is32BitMachine)
                {
                    return (UInt32)(DosHeader.e_lfanew + Marshal.SizeOf(typeof(IMAGE_NT_HEADERS32)) + Marshal.SizeOf(typeof(IMAGE_DATA_DIRECTORY)) * imgDir);
                }
                else if (this.Is64BitMachine)
                {
                    return (UInt32)(DosHeader.e_lfanew + Marshal.SizeOf(typeof(IMAGE_NT_HEADERS64)) + Marshal.SizeOf(typeof(IMAGE_DATA_DIRECTORY)) * imgDir);
                }
            }
            return 0;
        }

        public IMAGE_DATA_DIRECTORY GetImageDirectory(UInt16 imgDir)
        {
            if (IsValidDirectory(imgDir))
            {
                return DataDirectories[imgDir];
            }

            return new IMAGE_DATA_DIRECTORY { };
        }

        public static T FromBinaryReader<T>(BinaryReader reader)
        {
            // Read in a byte array
            byte[] bytes = reader.ReadBytes(Marshal.SizeOf(typeof(T)));

            // Pin the managed memory while, copy it out the data, then unpin it
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            T theStructure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();

            return theStructure;
        }

        public bool Is32BitMachine
        {
            get
            {
                return FileHeader.Machine == IMAGE_FILE_MACHINE_I386;
            }
        }

        public bool Is64BitMachine
        {
            get
            {
                return FileHeader.Machine == IMAGE_FILE_MACHINE_AMD64;
            }
        }

        public UInt32 GetImageBase32()
        {
            return OptionalHeader32.ImageBase;
        }

        public UInt64 GetImageBase64()
        {
            return OptionalHeader64.ImageBase;
        }

        public UInt64 GetImageBase32Or64()
        {
            if (this.Is32BitMachine)
            {
                return OptionalHeader32.ImageBase;
            }
            else if (this.Is64BitMachine)
            {
                return OptionalHeader64.ImageBase;
            }

            return 0;
        }

        public DateTime TimeStamp
        {
            get
            {
                // Timestamp is a date offset from 1970
                DateTime returnValue = new DateTime(1970, 1, 1, 0, 0, 0);

                // Add in the number of seconds since 1970/1/1
                returnValue = returnValue.AddSeconds(FileHeader.TimeDateStamp);
                // Adjust to local timezone
                returnValue += TimeZone.CurrentTimeZone.GetUtcOffset(returnValue);

                return returnValue;
            }
        }
    }
}
