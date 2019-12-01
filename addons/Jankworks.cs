using System.Runtime.InteropServices;

namespace Jankworks {
    [System.Security.SuppressUnmanagedCodeSecurity()]
    internal static class JankClient {
        internal const string NativeLibraryName = "CSteamworks";
        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void StartVoiceRecording();

		[DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void StopVoiceRecording();

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong GetSteamID();
    }
}