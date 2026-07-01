using Hexa.NET.ImGui;
using Hexa.NET.OpenGL;
using Hexa.NET.SDL3;
using System;
using System.Diagnostics;
using System.Threading;

public partial class ImGuiCore
{
	// Shared fields
	public GL Gl;
	public SDLWindowPtr window;
	public ImGuiIOPtr io;
	public IntPtr hwnd;
	public uint windowId;
	public Action DrawMenu;

	public bool menuVisible;
	public Thread imguiThread;
	public bool imguiRunning;

	/// <summary>
	/// Start the ImGui thread. Set DrawMenu before calling.
	/// </summary>
	public void StartImGuiThread()
	{
		if (DrawMenu == null)
		{
			Debug.WriteLine("Set ImGuiCore.DrawMenu to your custom 'public void DrawMenu()' before starting the ImGui thread!");

			return;
		}

		imguiRunning = true;

		imguiThread = new Thread(ImGuiLoop)
		{
			Name = "ImGui Render",
			IsBackground = true
		};

		imguiThread.Start();
	}

	private void ImGuiLoop()
	{
		try
		{
			CreateImGuiWindow();

			while (imguiRunning)
			{
				BeginFrame();

				CheckWindowToggle();
				UpdateZOrder();
				SetMouseCapture();
				ProcessEvents();

				if (!menuVisible)
				{
					CheckDisplaySize();
					IdleFrame();
					continue;
				}

				SetWindowSize();
				RenderFrame(DrawMenu);
				EndFrame();
			}
		}
		catch (Exception ex)
		{
			Debug.WriteLine(ex);
		}
		finally
		{
			ShutdownImGui();
		}
	}
}
