using System;
using System.Runtime.InteropServices;

namespace ETW
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TRACE_LOGFILE_HEADER
    {
        public uint BufferSize;         // Logger buffer size in Kbytes
        public uint Version;            // Logger version
        public uint ProviderVersion;    // defaults to NT version
        public uint NumberOfProcessors; // Number of Processors
        public ulong EndTime;           // Time when logger stops
        public uint TimerResolution;    // assumes timer is constant!!!
        public uint MaximumFileSize;    // Maximum in Mbytes
        public LogFileMode LogFileMode; // specify logfile mode
        public uint BuffersWritten;     // used to file start of Circular File

        public uint StartBuffers;       // Count of buffers written at start.
        public uint PointerSize;        // Size of pointer type in bits
        public uint EventsLost;         // Events losts during log session
        public uint CpuSpeedInMHz;      // Cpu Speed in MHz

        private IntPtr LoggerName;       // Do not use
        private IntPtr LogFileName;      // Do not use
        public TIME_ZONE_INFORMATION TimeZone;

        public ulong BootTime;
        public ulong PerfFreq;     
        public ulong StartTime;    
        public Clock ReservedFlags;  // ClockType
        public uint BuffersLost;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct EVENT_TRACE_HEADER // overlays WNODE_HEADER
    {
        public ushort Size; // Size of entire record
        public ushort FieldTypeFlags; // Indicates valid fields

        public byte Type; // event type
        public byte Level; // trace instrumentation level
        public ushort Version; // version of trace record

        public uint ThreadId; // Thread Id
        public uint ProcessId; // Process Id
        public ulong TimeStamp; // time when event happens
        public Guid Guid; // Guid that identifies event

        public uint KernelTime; // Kernel Mode CPU ticks
        public uint UserTime; // User mode CPU ticks
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct EVENT_TRACE
    {
        public EVENT_TRACE_HEADER Header; // Event trace header
        public uint InstanceId; // Instance Id of this event
        public uint ParentInstanceId; // Parent Instance Id.
        public Guid ParentGuid; // Parent Guid;
        public IntPtr MofData; // Pointer to Variable Data
        public uint MofLength; // Variable Datablock Length

        public byte ProcessorNumber;
        public byte Alignment;
        public ushort LoggerId;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct EVENT_TRACE_LOGFILEW
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string LogFileName;

        [MarshalAs(UnmanagedType.LPWStr)]
        public string LoggerName;

        public ulong CurrentTime; // timestamp of last event
        public uint BuffersRead; // buffers read to date

        public ProcessTraceMode ProcessTraceMode;

        public EVENT_TRACE CurrentEvent; // Current Event from this stream.
        public TRACE_LOGFILE_HEADER LogfileHeader;
        public PEVENT_TRACE_BUFFER_CALLBACKW BufferCallback; // callback before each buffer is read
        //
        // following variables are filled for BufferCallback.
        //
        public uint BufferSize;
        public uint Filled;
        public uint EventsLost;

        public PEVENT_RECORD_CALLBACK EventRecordCallback;

        public bool IsKernelTrace;

        public IntPtr Context;
    }

    [StructLayout(LayoutKind.Sequential, Size = 0xAC, CharSet = CharSet.Unicode)]
    public struct TIME_ZONE_INFORMATION
    {
        public uint Bias;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string StandardName;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U2, SizeConst = 8)]
        public ushort[] StandardDate;

        public uint StandardBias;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string DaylightName;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U2, SizeConst = 8)]
        public ushort[] DaylightDate;

        public uint DaylightBias;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct EVENT_DESCRIPTOR
    {
        public UInt16 Id;
        public byte Version;
        public byte Channel;
        public byte Level;
        public byte Opcode;
        public UInt16 Task;
        public UInt64 Keyword;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct EVENT_HEADER
    {
        public UInt16 Size;
        public UInt16 HeaderType;
        public EventHeaderFlag Flags;
        public UInt16 EventProperty;
        public UInt32 ThreadId;
        public UInt32 ProcessId;
        public UInt64 TimeStamp;
        public Guid ProviderId;
        public EVENT_DESCRIPTOR EventDescriptor;
        public UInt64 ProcessorTime;
        public Guid ActivityId;
    }

    [StructLayout(LayoutKind.Sequential)]
    public ref struct EVENT_RECORD
    {
        public EVENT_HEADER EventHeader;
        public ETW_BUFFER_CONTEXT BufferContext;
        public UInt16 ExtendedDataCount;
        public UInt16 UserDataLength;
        public IntPtr ExtendedData;
        public IntPtr UserData;
        public IntPtr UserContext;

        [StructLayout(LayoutKind.Explicit)]
        public struct ETW_BUFFER_CONTEXT
        {
            [FieldOffset(0)] public byte ProcessorNumber;
            [FieldOffset(1)] public byte Alignment;
            [FieldOffset(0)] public UInt16 ProcessorIndex;
            [FieldOffset(2)] public UInt16 LoggerId;
        }
    }

    public delegate bool PEVENT_TRACE_BUFFER_CALLBACKW([In] IntPtr logfile);

    public delegate void PEVENT_RECORD_CALLBACK(in EVENT_RECORD eventRecord);

    [Flags]
    public enum EventHeaderFlag : ushort
    {
        EXTENDED_INFO = 0x0001,
        PRIVATE_SESSION = 0x0002,
        STRING_ONLY = 0x0004,
        TRACE_MESSAGE = 0x0008,
        NO_CPUTIME = 0x0010,
        HEADER_32_BIT = 0x0020,
        HEADER_64_BIT = 0x0040,
        DECODE_GUID = 0x0080,
        CLASSIC_HEADER = 0x0100,
        PROCESSOR_INDEX = 0x0200,
    }

    [Flags]
    public enum ProcessTraceMode : uint
    {
        EVENT_RECORD = 0x10000000,
        REAL_TIME = 0x00000100,
        RAW_TIMESTAMP = 0x00001000,
    }

    public static partial class Native
    {
        [DllImport(AdvApi32, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern long OpenTraceW([In,Out] ref EVENT_TRACE_LOGFILEW Logfile);

        [DllImport(AdvApi32, CharSet = CharSet.Unicode, SetLastError = true)]
        public extern static int ProcessTrace([In] long[] HandleArray, [In] uint HandleCount, in long StartTime, in long EndTime);

        [DllImport(AdvApi32, CharSet = CharSet.Unicode, SetLastError = true)]
        public extern static int CloseTrace([In] long TraceHandle);
    }
}