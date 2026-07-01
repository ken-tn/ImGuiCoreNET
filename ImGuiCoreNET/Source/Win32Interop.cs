using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

public abstract class Win32Interop
{
	// Constants
	public const int VK_LBUTTON = 0x01;
	public const int VK_RBUTTON = 0x02;
	public const int VK_MBUTTON = 0x04;

	public const int GWL_EXSTYLE = -20;

	public const int WS_EX_TRANSPARENT = 0x00000020;
	public const int WS_EX_LAYERED = 0x00080000;
	public const int WS_EX_TOOLWINDOW = 0x00000080;
	public const int WS_EX_NOACTIVATE = 0x08000000;

	public static readonly IntPtr HWND_TOPMOST = new(-1);
	public static readonly IntPtr HWND_NOTOPMOST = new(-2);

	public const uint SWP_NOSIZE = 0x0001;
	public const uint SWP_NOMOVE = 0x0002;
	public const uint SWP_NOACTIVATE = 0x0010;

	// Functions
	[DllImport("user32.dll")]
	public static extern short GetAsyncKeyState(int vKey);

	[DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", SetLastError = true)]
	public static extern IntPtr GetWindowLongPtr(IntPtr hwnd, int index);

	[DllImport("user32.dll", SetLastError = true)]
	public static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

	[DllImport("user32.dll", SetLastError = true)]
	private static extern bool SetWindowPos(
		IntPtr hWnd,
		IntPtr hWndInsertAfter,
		int X,
		int Y,
		int cx,
		int cy,
		uint uFlags
	);

	[DllImport("user32.dll")]
	public static extern IntPtr GetForegroundWindow();
	[DllImport("user32.dll")]
	public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);
	[DllImport("user32.dll")]

	public static extern bool GetCursorPos(ref Point lpPoint);

	[DllImport("user32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool ScreenToClient(
		IntPtr hWnd,
		ref Point lpPoint
	);

	public static void InitializeOverlayWindow(IntPtr hwnd)
	{
		int ex = (int)GetWindowLongPtr(hwnd, GWL_EXSTYLE);

		ex |= WS_EX_LAYERED;
		ex |= WS_EX_TOOLWINDOW;
		ex |= WS_EX_NOACTIVATE;
		ex |= WS_EX_TRANSPARENT;

		if (SetWindowLongPtr(hwnd, GWL_EXSTYLE, (IntPtr)ex) == IntPtr.Zero)
		{
			Debug.WriteLine(Marshal.GetLastWin32Error());
		}
	}

	public static bool IsProcessForeground(uint processId)
	{
		IntPtr foreground = GetForegroundWindow();
		if (foreground == IntPtr.Zero)
			return false;

		GetWindowThreadProcessId(foreground, out uint pid);
		return pid == processId;
	}

	public static void UpdateTopMost(IntPtr hwnd, bool topMost)
	{
		SetWindowPos(
				hwnd,
				topMost ? HWND_TOPMOST : HWND_NOTOPMOST,
				0, 0, 0, 0,
				SWP_NOMOVE |
				SWP_NOSIZE |
				SWP_NOACTIVATE);
	}

	public static void SetWindowTransparent(IntPtr hwnd, bool transparent)
	{
		int ex = (int)GetWindowLongPtr(hwnd, GWL_EXSTYLE);
		if (!transparent)
		{
			ex &= ~WS_EX_TRANSPARENT;
			ex &= ~WS_EX_NOACTIVATE;
		}
		else
		{
			ex |= WS_EX_TRANSPARENT;
			ex |= WS_EX_NOACTIVATE;
		}

		if (SetWindowLongPtr(hwnd, GWL_EXSTYLE, (IntPtr)ex) == IntPtr.Zero)
		{
			Debug.WriteLine(Marshal.GetLastWin32Error());
		}
	}

	public static Point? GetCursorPosition(IntPtr hwnd)
	{
		Point p = new();
		if (GetCursorPos(ref p) && ScreenToClient(hwnd, ref p))
		{
			return p;
		}

		return null;
	}

	public static bool[] GetMouseStates()
	{
		return [(GetAsyncKeyState(VK_LBUTTON) & 0x8000) != 0, (GetAsyncKeyState(VK_RBUTTON) & 0x8000) != 0, (GetAsyncKeyState(VK_MBUTTON) & 0x8000) != 0];
	}
}
