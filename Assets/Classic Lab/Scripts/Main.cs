using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VGS.VLR
{
    public class Main : MonoBehaviour
    {
        public static bool runOnce = false;

        private const int SW_MAXIMIZE = 3;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(HandleRef handle, out int processId);

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool EnumWindows(EnumWindowsProc callback, IntPtr extraData);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();

        private HandleRef unityWindowHandle;

        private void Awake()
        {
            if (!Application.isEditor && Player.platform == "Desktop" && !runOnce)
            {
                MaximizeWindow();
                runOnce = true;
            }
        }

        private void Update()
        {
            if (!Application.isEditor && Player.platform == "Desktop")
            {
                if (Input.GetKeyDown(KeyCode.F11))
                {
                    if (Screen.fullScreen)
                    {
                        MaximizeWindow();
                    }
                    else
                    {
                        MaximizeWindow();
                        Screen.fullScreen = true;
                    }
                }
            }
        }
        
        private void MaximizeWindow()
        {
            PlayerPrefs.SetInt("Fullscreen mode_h3981298716", 0);
            Screen.fullScreen = false;
            Screen.fullScreenMode = FullScreenMode.Windowed;
            Invoke("MaximizeAppWindow", 0.2f);
        }

        private void MaximizeAppWindow()
        {
            EnumWindows(EnumWindowsCallBack, IntPtr.Zero);
            ShowWindow(unityWindowHandle.Handle, SW_MAXIMIZE);
        }

        private bool EnumWindowsCallBack(IntPtr hWnd, IntPtr lParam)
        {
            int procid;
            GetWindowThreadProcessId(new HandleRef(this, hWnd), out procid);
            int currentPID = System.Diagnostics.Process.GetCurrentProcess().Id;
            new HandleRef(this, System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle);
            if (procid == currentPID)
            {
                unityWindowHandle = new HandleRef(this, hWnd);
                return false;
            }
            return true;
        }
    }
}
