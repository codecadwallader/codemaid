/***************************************************************************

Copyright (c) Microsoft Corporation. All rights reserved.
This code is licensed under the Visual Studio SDK license terms.
THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

***************************************************************************/

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Helpers
{
    /// <summary>
    /// Defines pinvoked utility methods and internal VS Constants
    /// </summary>
    /// <remarks>
    /// Part of VSIX unit testing starter kit.
    /// </remarks>
    internal static class NativeMethods
    {
        internal delegate bool CallBack(IntPtr hwnd, IntPtr lParam);

        // Declare two overloaded SendMessage functions
        [DllImport("user32.dll")]
        internal static extern UInt32 SendMessage(IntPtr hWnd, UInt32 Msg,
            UInt32 wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern bool PeekMessage([In, Out] ref Microsoft.VisualStudio.OLE.Interop.MSG msg, HandleRef hwnd, int msgMin, int msgMax, int remove);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern bool TranslateMessage([In, Out] ref Microsoft.VisualStudio.OLE.Interop.MSG msg);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern int DispatchMessage([In] ref Microsoft.VisualStudio.OLE.Interop.MSG msg);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool attach);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        internal static extern uint GetCurrentThreadId();

        [DllImport("user32")]
        internal static extern int EnumChildWindows(IntPtr hwnd, CallBack x, IntPtr y);

        [DllImport("user32")]
        internal static extern bool IsWindowVisible(IntPtr hDlg);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("user32")]
        internal static extern int GetClassName(IntPtr hWnd,
                                               StringBuilder className,
                                               int stringLength);

        [DllImport("user32")]
        internal static extern int GetWindowText(IntPtr hWnd, StringBuilder className, int stringLength);

        [DllImport("user32")]
        internal static extern bool EndDialog(IntPtr hDlg, int result);

        [DllImport("Kernel32")]
        internal static extern long GetLastError();

        internal const int QS_KEY = 0x0001,
                        QS_MOUSEMOVE = 0x0002,
                        QS_MOUSEBUTTON = 0x0004,
                        QS_POSTMESSAGE = 0x0008,
                        QS_TIMER = 0x0010,
                        QS_PAINT = 0x0020,
                        QS_SENDMESSAGE = 0x0040,
                        QS_HOTKEY = 0x0080,
                        QS_ALLPOSTMESSAGE = 0x0100,
                        QS_MOUSE = QS_MOUSEMOVE | QS_MOUSEBUTTON,
                        QS_INPUT = QS_MOUSE | QS_KEY,
                        QS_ALLEVENTS = QS_INPUT | QS_POSTMESSAGE | QS_TIMER | QS_PAINT | QS_HOTKEY,
                        QS_ALLINPUT = QS_INPUT | QS_POSTMESSAGE | QS_TIMER | QS_PAINT | QS_HOTKEY | QS_SENDMESSAGE;

        internal const int Facility_Win32 = 7;

        internal const int WM_CLOSE = 0x0010;

        internal const int
                       S_FALSE = 0x00000001,
                       S_OK = 0x00000000,

                       IDOK = 1,
                       IDCANCEL = 2,
                       IDABORT = 3,
                       IDRETRY = 4,
                       IDIGNORE = 5,
                       IDYES = 6,
                       IDNO = 7,
                       IDCLOSE = 8,
                       IDHELP = 9,
                       IDTRYAGAIN = 10,
                       IDCONTINUE = 11;

        internal static long HResultFromWin32(long error)
        {
            if (error <= 0)
            {
                return error;
            }

            return ((error & 0x0000FFFF) | (Facility_Win32 << 16) | 0x80000000);
        }

        /// <devdoc>Please use this "approved" method to compare file names.</devdoc>
        public static bool IsSamePath(string file1, string file2)
        {
            if (file1 == null || file1.Length == 0)
            {
                return (file2 == null || file2.Length == 0);
            }

            Uri uri1;
            Uri uri2;

            try
            {
                if (!Uri.TryCreate(file1, UriKind.Absolute, out uri1) || !Uri.TryCreate(file2, UriKind.Absolute, out uri2))
                {
                    return false;
                }

                if (uri1 != null && uri1.IsFile && uri2 != null && uri2.IsFile)
                {
                    return 0 == String.Compare(uri1.LocalPath, uri2.LocalPath, StringComparison.OrdinalIgnoreCase);
                }

                return file1 == file2;
            }
            catch (UriFormatException e)
            {
                System.Diagnostics.Trace.WriteLine("Exception " + e.Message);
            }

            return false;
        }
    }
}