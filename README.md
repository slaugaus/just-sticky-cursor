# Overview

A reimplementation of the "sticky cursor" part of [Dual Monitor Tools](https://dualmonitortool.sourceforge.net/), made to test my C# abilities and experiment with Windows SDKs.

Currently, it places horizontal and vertical "sticky borders" right down the middle of a 1080p monitor, which can be configured with a couple of edits to `CursorControl.cs`. If your cursor touches one, it will be prevented from going further until it's moved an additional 500 pixels towards it, at which point it will break through.

**Currently, the UI window that pops up must be the active (focused) window if you want this functionality to work. I'm working to diagnose and rectify this...**

According to the build log, you may need to install [the Windows App SDK](http://go.microsoft.com/fwlink/?linkid=2222757) for it to work.

[Software Demo Video](https://youtu.be/U-z3tO8MJGw)

# Development Environment

IDE: [Visual Studio Community 2022](https://visualstudio.microsoft.com/vs/)

Language: C# with [WinUI 3](https://learn.microsoft.com/en-us/windows/apps/winui/winui3/) (part of the Windows App SDK)

# Useful Websites

- [WinUI 3 Gallery](https://github.com/microsoft/WinUI-Gallery)
- [Template Studio for WinUI extension](https://marketplace.visualstudio.com/items?itemName=TemplateStudio.TemplateStudioForWinUICs)
- [`winuser.h` (`user32.dll`) documentation](https://learn.microsoft.com/en-us/windows/win32/api/winuser/)
- [Low-level keyboard/mouse hook library that I'm not currently using](https://github.com/riyasy/Global-Low-Level-Key-Board-And-Mouse-Hook)

# Future Work

Laid out in TODOs all over the code.

- Use a low-level mouse hook instead of directly interfacing with it - needed for global functionality
- Add useful settings to the UI
- "Minimize to tray icon" functionality
- And a whole lot more!
