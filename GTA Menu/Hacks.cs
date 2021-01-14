using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace GTA_Menu {
    class Hacks {
        [DllImport("kernel32.dll")]
        static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
        [DllImport("kernel32.dll")]
        static extern uint SuspendThread(IntPtr hThread);
        [DllImport("kernel32.dll")]
        static extern int ResumeThread(IntPtr hThread);
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool CloseHandle(IntPtr handle);

        public enum ThreadAccess : int {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002)
        }

        public static void SuspendTask(Process process) {
            foreach (ProcessThread thread in process.Threads) {
                IntPtr openThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)thread.Id);
                if (openThread == IntPtr.Zero) continue;

                SuspendThread(openThread);
                CloseHandle(openThread);
            }

            System.Threading.Thread.Sleep(9000);

            foreach (ProcessThread thread in process.Threads) {
                IntPtr openThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)thread.Id);
                if (openThread == IntPtr.Zero) continue;

                var suspendCount = 0;
                do {
                    suspendCount = ResumeThread(openThread);
                } while (suspendCount > 0);

                CloseHandle(openThread);
            }
        }

        public static void ForceEndTask(Process process) {
            process.Kill();
        }
    }
}
