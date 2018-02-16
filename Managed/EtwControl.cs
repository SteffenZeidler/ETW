using System;
using System.Runtime.InteropServices;

namespace ETW
{
    public struct WNODE_HEADER
    {
        public uint BufferSize;
        public uint ProviderId;
        public uint Version;
        public uint Linkage;
        public Int64 TimeStamp;
        public Guid Guid;
        public Clock ClientContext;
        public uint Flags;
    };

    [StructLayout(LayoutKind.Sequential)]
    public class EVENT_TRACE_PROPERTIES_BASE
    {
        public WNODE_HEADER Wnode;
        public uint BufferSize;
        public uint MinimumBuffers;
        public uint MaximumBuffers;
        public uint MaximumFileSize;
        public LogFileMode LogFileMode;
        public uint FlushTimer;
        public uint EnableFlags;
        public int AgeLimit;
        public uint NumberOfBuffers;
        public uint FreeBuffers;
        public uint EventsLost;
        public uint BuffersWritten;
        public uint LogBuffersLost;
        public uint RealTimeBuffersLost;
        public IntPtr LoggerThreadId;
        public uint LogFileNameOffset;
        public uint LoggerNameOffset;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class EVENT_TRACE_PROPERTIES : EVENT_TRACE_PROPERTIES_BASE
    {
        private const int MaxTraceFileNameLen = 1024;
        private const int MaxTraceLoggerNameLen = 1024;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MaxTraceLoggerNameLen)]
        public string LoggerName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MaxTraceFileNameLen)]
        public string LogFileName;

        public EVENT_TRACE_PROPERTIES()
        {
            Wnode.BufferSize = (uint)Marshal.SizeOf<EVENT_TRACE_PROPERTIES>();
            LoggerNameOffset = (uint)Marshal.OffsetOf<EVENT_TRACE_PROPERTIES>(nameof(LoggerName));
            LogFileNameOffset = (uint)Marshal.OffsetOf<EVENT_TRACE_PROPERTIES>(nameof(LogFileName));

            Wnode.ClientContext = Clock.SystemTime;
            BufferSize = 64;
        }
    };

    [Flags]
    public enum LogFileMode : uint
    {
        FILE_MODE_SEQUENTIAL = 0x00000001,
        FILE_MODE_CIRCULAR = 0x00000002,
        FILE_MODE_APPEND = 0x00000004,
        FILE_MODE_NEWFILE = 0x00000008,
        USE_MS_FLUSH_TIMER = 0x00000010,
        FILE_MODE_PREALLOCATE = 0x00000020,
        NONSTOPPABLE_MODE = 0x00000040,
        SECURE_MODE = 0x00000080,
        REAL_TIME_MODE = 0x00000100,
        DELAY_OPEN_FILE_MODE = 0x00000200, //deprecated
        BUFFERING_MODE = 0x00000400,
        PRIVATE_LOGGER_MODE = 0x00000800,
        ADD_HEADER_MODE = 0x00001000,
        USE_KBYTES_FOR_SIZE = 0x00002000,
        USE_GLOBAL_SEQUENCE = 0x00004000,
        USE_LOCAL_SEQUENCE = 0x00008000,
        RELOG_MODE = 0x00010000,
        PRIVATE_IN_PROC = 0x00020000,
        BUFFER_INTERFACE_MODE = 0x00040000,
        KD_FILTER_MODE = 0x00080000,
        REAL_TIME_RELOG_MODE = 0x00100000,
        LOST_EVENTS_DEBUG_MODE = 0x00200000,
        STOP_ON_HYBRID_SHUTDOWN = 0x00400000,
        PERSIST_ON_HYBRID_SHUTDOWN = 0x00800000,
        USE_PAGED_MEMORY = 0x01000000,
        SYSTEM_LOGGER_MODE = 0x02000000,
        COMPRESSED_MODE = 0x04000000,
        INDEPENDENT_SESSION_MODE = 0x08000000,
        NO_PER_PROCESSOR_BUFFERING = 0x10000000,
        BLOCKING_MODE = 0x20000000,
        UnUsed = 0x40000000,
        ADDTO_TRIAGE_DUMP = 0x80000000,
    }

    public enum Clock : uint
    {
        QueryPerformanceCounter = 1,
        SystemTime = 2,
        CpuCycleCounter = 3
    }

    public enum ControlCode : uint
    {
        Query,
        Stop,
        Update,
        Flush
    }

    public enum EnableCode : uint
    {
        DisableProvider,
        EnableProvider,
        CaptureState
    }

    public static partial class Native
    {
        private const String AdvApi32 = "advapi32.dll";

        [DllImport(AdvApi32, CharSet = CharSet.Unicode)]
        public static extern int StartTrace(
            [Out] out long SessionHandle,
            [In] string SessionName,
            [In, Out] EVENT_TRACE_PROPERTIES Properties);

        [DllImport(AdvApi32, CharSet = CharSet.Unicode)]
        public static extern int StopTrace(
            [In] long SessionHandle,
            [In] string SessionName,
            [In, Out] EVENT_TRACE_PROPERTIES Properties);

        [DllImport(AdvApi32, CharSet = CharSet.Unicode)]
        public static extern int EnableTrace(
            [In] uint Enable,
            [In] uint EnableFlag,
            [In] uint EnableLevel,
            [In] in Guid ControlGuid,
            [In] long SessionHandle);

        [DllImport(AdvApi32, CharSet = CharSet.Unicode)]
        public static extern int ControlTraceW(
            [In] long SessionHandle,
            [In] string SessionName,
            [In, Out] EVENT_TRACE_PROPERTIES Properties,
            [In] ControlCode ControlCode
            );
    }
}