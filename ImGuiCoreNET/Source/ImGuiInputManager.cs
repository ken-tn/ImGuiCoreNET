using Hexa.NET.ImGui.Backends.SDL3;
using Hexa.NET.SDL3;
using System.Drawing;
using SDLEvent = Hexa.NET.SDL3.SDLEvent;

public unsafe partial class ImGuiCore
{
	private int openVK = 0x74;

	/// <summary>
	/// Set the toggle key to open ImGui
	/// </summary>
	/// <param name="VK_Keycode">Virtual keycode value</param>
	public void SetToggleKey(int VK_Keycode)
	{
		openVK = VK_Keycode;
	}

	public void CheckWindowToggle()
	{
		// Detect an F5 press using the toggle bit
		if ((Win32Interop.GetAsyncKeyState(openVK) & 1) != 0)
		{
			ToggleMenu();
		}
	}

	// Captures events for ImGui to process (i.e. inputs)
	public void ProcessEvents()
	{
		SDLEvent sdlEvent = default;

		SDL.PumpEvents();

		while (SDL.PollEvent(ref sdlEvent))
		{
			// These are handled by Win32
			if (sdlEvent.Type is (uint)SDLEventType.MouseMotion ||
				sdlEvent.Type is (uint)SDLEventType.MouseButtonDown ||
				sdlEvent.Type is (uint)SDLEventType.MouseButtonUp)
			{
				continue;
			}
			ImGuiImplSDL3.ProcessEvent(
			(Hexa.NET.ImGui.Backends.SDL3.SDLEvent*)&sdlEvent);

			switch ((SDLEventType)sdlEvent.Type)
			{
				case SDLEventType.Quit:
				case SDLEventType.Terminating:
					imguiRunning = false;
					break;

				case SDLEventType.WindowCloseRequested:
					if (sdlEvent.Window.WindowID == windowId)
						imguiRunning = false;
					break;
			}
		}
	}

	public void ProcessWindowsEvents()
	{
		if (Win32Interop.GetCursorPosition(hwnd) is Point p)
		{
			float scale = SDL.GetWindowDisplayScale(window);
			io.MousePos = new System.Numerics.Vector2(p.X, p.Y) * scale;
		}

		bool[] mouseStates = Win32Interop.GetMouseStates();
		io.MouseDown[0] = mouseStates[0];
		io.MouseDown[1] = mouseStates[1];
		io.MouseDown[2] = mouseStates[2];
	}

	private bool prevTransparent = true;
	private void SetMouseCapture()
	{
		bool transparent = !((io.WantCaptureMouse) && menuVisible);
		if (prevTransparent == transparent)
		{
			return;
		}
		prevTransparent = transparent;

		Win32Interop.SetWindowTransparent(hwnd, transparent);

		if (!transparent)
		{
			SDL.RaiseWindow(window);
		}
	}
}
