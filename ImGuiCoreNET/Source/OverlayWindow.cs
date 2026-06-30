using Hexa.NET.SDL3;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

public partial class ImGuiCore
{
	private readonly uint gamePid = (uint)Process.GetCurrentProcess().Id;
	private bool lastTopMost;

	public void UpdateZOrder()
	{
		bool shouldBeTopMost = menuVisible && Win32Interop.IsProcessForeground(gamePid);

		if (shouldBeTopMost == lastTopMost)
			return;

		Win32Interop.UpdateTopMost(hwnd, shouldBeTopMost);

		lastTopMost = shouldBeTopMost;
	}

	public unsafe int[] CheckDisplaySize()
	{
		SDLRect bounds = default;

		if (SDL.GetDisplayUsableBounds(SDL.GetPrimaryDisplay(), &bounds))
		{
			currentWidth = bounds.W;
			currentHeight = bounds.H;
		}
		else
		{
			// Default to 1080p
			currentWidth = 1920;
			currentHeight = 1080;
			Debug.WriteLine(Marshal.PtrToStringUTF8((IntPtr)SDL.GetError()) ?? "Unknown SDL error");
		}

		return [currentWidth, currentHeight];
	}

	// Cached for optimization
	private int lastWidth = 0;
	private int lastHeight = 0;
	private int currentWidth = 1920;
	private int currentHeight = 1080;
	public void SetWindowSize()
	{
		if (lastWidth != currentWidth || lastHeight != currentHeight)
		{
			lastWidth = currentWidth;
			lastHeight = currentHeight;

			float mainScale = SDL.GetDisplayContentScale(SDL.GetPrimaryDisplay());
			SDL.SetWindowSize(window,
				(int)(currentWidth * mainScale),
				(int)(currentHeight * mainScale) - 1 // bypass fullscreen issues, but lose 1 height pixel
			);
			SDL.SetWindowPosition(window, 0, 0);
		}
	}
}
