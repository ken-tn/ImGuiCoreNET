<a id="readme-top"></a>
<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#quick-start">Quick start</a>
      <ul>
        <li><a href="#installation">Installation</a></li>
        <li><a href="#dependencies">Dependencies</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

Dear ImGui using Hexa.NET (OpenGL3 and SDL3). Overlays over a program.


### Built With

* [![Hexa][Hexa.NET]][Hexa-url]
* [![OpenGL][OpenGL]][OpenGL-Url]
* [![SDL][SDL]][SDL-url]

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- GETTING STARTED -->
## Quick start

### Installation

#### Visual Studio
* Right click Dependencies -> Add Project Reference... -> ImGuiCoreNET.dll</p>
* Right click Dependencies -> Manage NuGet Packages... -> Hexa.NET.ImGui -> Install</p>

### Usage

``` csharp
using Hexa.NET.ImGui;

public class Example
{
	ImGuiCore imGuiCore = new();

	// Draw your UI here
	public void DrawMenu()
	{
		ImGui.ShowDemoWindow();
	}

	public void OnInitialize()
	{
		imGuiCore.DrawMenu = DrawMenu;
		imGuiCore.StartImGuiThread();
	}
}
```

[product-screenshot]: images/demo.png
![Screen Shot][product-screenshot]

_For more examples, please refer to the [ImGui Documentation](https://github.com/ocornut/imgui/wiki/Getting-Started)_

### Dependencies

* Make sure these are loaded at runtime, along with ImGuiCoreNET.dll

```
cimgui.dll
Hexa.NET.ImGui.Backends.SDL3.dll
Hexa.NET.SDL3.dll
ImGuiImplSDL3.dll
Hexa.NET.ImGui.Backends.dll
Hexa.NET.Utilities.dll
SDL3.dll
Hexa.NET.ImGui.dll
HexaGen.Runtime.dll
Hexa.NET.OpenGL.Core.dll
HexaUtils.dll
Hexa.NET.OpenGL3.dll
ImGuiImpl.dll
```

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- LICENSE -->
## License

Distributed under the GPL-3.0 license. See `LICENSE.txt` for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- ACKNOWLEDGMENTS -->
## Acknowledgments

* [Img Shields](https://shields.io)

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[Hexa.NET]: https://img.shields.io/badge/Hexa.Net-100050?logoColor=white&logo=data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAACwElEQVR4nKRTTUwrZRQ9M51OWzoFWqDTDmCiYQEsJFFa2Fhk04VWfqJxYWJijD/RsABXRqPRCGtXBBGqC+LKSBTBxI0CUWNpNXkvedC34JHQvymhDNAO/ZuZ72W+PnjhvbfjJjeZOd+9Jzn3nssRQnCT4B4FlqLff1KuVARd05CIx4YIIUwgOPyvheNgt9nUd95+a45l2ScT/Lrx2xvz33w7VyurYFgW7naR4rei0VFiGOAdToii93D8lcjKZQ9zKUFV1abI2ERSluXuZnc7Wtr8sPpCANFRz/+Fs4KMc+UYok9Mb6z90ut0OtUrgouLi6aJycn48cl5v7tDhF0MwCKFwfCtlJxUFejZ31E5+g/K8RHaW4W91dXVoCAIJSomJ8tdqXS2n+d51HUrLG2D4IROWB0empyrC5aOIWjEBivHIZXJ9WVzuafMXkowv7D4lTkYt+8ZWF0S5H9mUbqzBJbUaKq7y8j/PQuLwwuP1APWYsHC4tKXdIg78UToj83t16kezgahfwqunjSU29/hcP1NgAF4wQd/6AsQRzfqu19TWX9ubb8WiydGWEJLHob5w3mehS+8DJf0HARxAP5wFJxn4HqhORtCGHYoMLg1OhL6sYEAhl6HoVdBDB0M3wKGb6bfJma+0SIAL4Ze+Gk4GNikM5j64L3PaL+mwijuQ6+p0KqnIHoNxKhDq55RzCjeA6nT7eHD99/9/MpIfr8/1Sn5k/nMfq+7EoXd+zyIFG4QEB1aMUXXWD36H0ohD8kn3pUkKXXNSKWS6oyMjyfzcr6rYSQRmsaYOmG1Mjg7eWAkr5heX/u5z/TA1RrNEASn+tHMzMe2JgHF0wIyB0mUlAxKShaZgz0UlQJsDiemp6c/vWx+7BbGIi//kJPzT5fLZcHQNSR2YsHGMb0aY81jstvVschLK9e2dtNzvh8AAP//2PE2tkif9msAAAAASUVORK5CYII=
[Hexa-url]: https://github.com/HexaEngine/Hexa.NET.ImGui
[OpenGL]: https://img.shields.io/badge/OpenGl-5487A6?logo=OpenGl&logoColor=fff
[OpenGL-url]: https://www.opengl.org/
[SDL]: https://img.shields.io/badge/SDL-000030?style=for-the-badge&logo=sdl
[SDL-url]: https://www.libsdl.org/
