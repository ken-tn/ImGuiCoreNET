using Hexa.NET.ImGui;
using Hexa.NET.ImGui.Backends.OpenGL3;
using Hexa.NET.ImGui.Backends.SDL3;
using Hexa.NET.OpenGL;
using Hexa.NET.SDL3;
using System;

public partial class ImGuiCore
{
	private void BeginFrame()
	{
		Gl.ClearColor(0, 0, 0, 0);
		Gl.Clear(GLClearBufferMask.ColorBufferBit);
	}

	public void IdleFrame()
	{
		Gl.SwapBuffers();
		SDL.Delay(20); // Slow down rendering to save resources
	}

	public void EndFrame()
	{
		ImGui.Render();
		ImGuiImplOpenGL3.RenderDrawData(ImGui.GetDrawData());

		if ((io.ConfigFlags & ImGuiConfigFlags.ViewportsEnable) != 0)
		{
			ImGui.UpdatePlatformWindows();
			ImGui.RenderPlatformWindowsDefault();
		}

		Gl.SwapBuffers(); // Refreshes at VSync
	}

	public void RenderFrame(Action DrawMenu)
	{
		ImGuiImplOpenGL3.NewFrame();
		ImGuiImplSDL3.NewFrame();
		ImGui.NewFrame();

		ProcessWindowsEvents();

		if (menuVisible)
		{
			DrawMenu();
		}
	}
}
