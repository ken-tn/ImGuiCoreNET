using System;
using Hexa.NET.ImGui;
using Hexa.NET.ImGui.Backends.OpenGL3;
using Hexa.NET.ImGui.Backends.SDL3;
using Hexa.NET.SDL3;
using SDLEvent = Hexa.NET.SDL3.SDLEvent;
using SDLWindow = Hexa.NET.SDL3.SDLWindow;

public unsafe partial class ImGuiCore
{
	private bool sdlBackendInitialized;
	private bool glBackendInitialized;

	private ImGuiContextPtr guiContext;
	private SDLGLContext glContext;

	public void CreateImGuiWindow()
	{
		SDL.SetHint(SDL.SDL_HINT_MOUSE_FOCUS_CLICKTHROUGH, "1");
		SDL.Init((uint)(SDLInitFlags.Events | SDLInitFlags.Video));

		float mainScale = SDL.GetDisplayContentScale(SDL.GetPrimaryDisplay());

		window = SDL.CreateWindow(
			"ImGui Window",
			0, // Set width and height later
			0,
			(ulong)(SDLWindowFlags.Opengl | SDLWindowFlags.HighPixelDensity | SDLWindowFlags.Borderless | SDLWindowFlags.Transparent));
		SDL.SetWindowMouseGrab(window, false);

		hwnd = (IntPtr)SDL.GetPointerProperty(
		SDL.GetWindowProperties(window),
		SDL.SDL_PROP_WINDOW_WIN32_HWND_POINTER,
		IntPtr.Zero);
		SDL.GLSetSwapInterval(1);

		windowId = SDL.GetWindowID(window);

		guiContext = ImGui.CreateContext();
		ImGui.SetCurrentContext(guiContext);

		// ImGui configuration
		io = ImGui.GetIO();
		io.ConfigFlags |= ImGuiConfigFlags.NavEnableKeyboard;
		io.ConfigFlags |= ImGuiConfigFlags.NavEnableGamepad;
		io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;
		// io.ConfigFlags |= ImGuiConfigFlags.ViewportsEnable; // Bad performance
		io.ConfigViewportsNoAutoMerge = false;
		io.ConfigViewportsNoTaskBarIcon = false;
		io.ConfigDpiScaleFonts = true;
		io.ConfigDpiScaleViewports = true;

		var style = ImGui.GetStyle();
		style.ScaleAllSizes(mainScale);
		style.FontScaleDpi = mainScale;

		glContext = SDL.GLCreateContext(window);
		SDL.GLMakeCurrent(window, glContext); // May not be required

		ImGuiImplSDL3.SetCurrentContext(guiContext);
		if (!ImGuiImplSDL3.InitForOpenGL(
			new Hexa.NET.ImGui.Backends.SDL3.SDLWindowPtr(
				(Hexa.NET.ImGui.Backends.SDL3.SDLWindow*)window.Handle),
			(void*)glContext.Handle))
		{
			throw new Exception("Failed to initialize ImGui SDL3 backend.");
		}
		sdlBackendInitialized = true;

		ImGuiImplOpenGL3.SetCurrentContext(guiContext);
		if (!ImGuiImplOpenGL3.Init((byte*)null))
		{
			throw new Exception("Failed to initialize ImGui OpenGL3 backend.");
		}
		glBackendInitialized = true;

		Gl = new(new BindingsContext(window, glContext));

		Win32Interop.InitializeOverlayWindow(hwnd);
	}

	public void ShutdownImGui()
	{
		if (window.Handle != null && glContext.Handle != IntPtr.Zero)
		{
			SDL.GLMakeCurrent(window, glContext);
		}

		if (glBackendInitialized)
			ImGuiImplOpenGL3.Shutdown();

		if (sdlBackendInitialized)
			ImGuiImplSDL3.Shutdown();

		if (glContext.Handle != IntPtr.Zero)
		{
			SDL.GLDestroyContext(glContext);
		}

		if (window.Handle != null)
		{
			SDL.GLMakeCurrent(window, IntPtr.Zero);
			SDL.DestroyWindow(window);
		}

		if (Gl != null)
		{
			Gl.Dispose();
		}

		if (!guiContext.IsNull)
		{
			ImGui.DestroyContext(guiContext);
		}

		SDL.Quit();
	}

	/// <summary>
	/// Call from the main thread
	/// </summary>
	public void Deinitialize()
	{
		SDLEvent quitEvent = default;
		imguiRunning = false;
		SDL.PushEvent(ref quitEvent);
		imguiThread?.Join();
	}
}

internal unsafe class BindingsContext : HexaGen.Runtime.IGLContext
{
	private readonly SDLWindow* window;
	private readonly SDLGLContext context;

	public BindingsContext(SDLWindow* window, SDLGLContext context)
	{
		this.window = window;
		this.context = context;
	}

	public nint Handle => (nint)window;

	public bool IsCurrent => SDL.GLGetCurrentContext() == context;

	public void Dispose()
	{
	}

	public nint GetProcAddress(string procName)
	{
		return (nint)SDL.GLGetProcAddress(procName);
	}

	public bool IsExtensionSupported(string extensionName)
	{
		return SDL.GLExtensionSupported(extensionName);
	}

	public void MakeCurrent()
	{
		SDL.GLMakeCurrent(window, context);
	}

	public void SwapBuffers()
	{
		SDL.GLSwapWindow(window);
	}

	public void SwapInterval(int interval)
	{
		SDL.GLSetSwapInterval(interval);
	}

	public bool TryGetProcAddress(string procName, out nint procAddress)
	{
		procAddress = (nint)SDL.GLGetProcAddress(procName);
		return procAddress != 0;
	}
}
