
## TODO, in order:
* Give app a tray icon for persistence
	* Show (default) and Close context menu
	* Notification when closing the window
* The actual sticky cursor behavior
	* Where does this code go?
	* Update rate?
	* **Built with configurability in mind**
		* Move distance
		* Monitor edges to work on
	* If cursor is at edge: Lock its y position until it's moved towards it x pixels (track in var)
	* Detect monitor configuration, edge locations
	* Allow free movement if [key] is held - including mouse keys
* Settings UI
	* Run on startup?
	* Sticky or normal when program starts?
	* Keybinds:
		* KeyDown event when textbox is selected; get and fill box with key name (modifiers?)
		* https://learn.microsoft.com/en-us/uwp/api/windows.ui.xaml.uielement.keydown?view=winrt-22621
		* Enable sticky (Ctrl+Win+PgUp) - plays splort or system "Speech On"
		* Disable sticky (Ctrl+Win+PgDn) - plays trolps or system "Speech Off"
		* Hold on keyboard to ignore sticky (Alt or disabled)
		* Hold on mouse to ignore sticky (LMB)
	* Stick resistance value; slider and NumberBox with SpinButton
	* Sounds off/system/slimy (splort)

## Getting Started

Browse and address `TODO:` comments in `View -> Task List` to learn the codebase and understand next steps for turning the generated code into production code.

Explore the [WinUI Gallery](https://www.microsoft.com/store/productId/9P3JFPWWDZRC) to learn about available controls and design patterns.

Relaunch Template Studio to modify the project by right-clicking on the project in `View -> Solution Explorer` then selecting `Add -> New Item (Template Studio)`.

## Publishing

For projects with MSIX packaging, right-click on the application project and select `Package and Publish -> Create App Packages...` to create an MSIX package.

For projects without MSIX packaging, follow the [deployment guide](https://docs.microsoft.com/windows/apps/windows-app-sdk/deploy-unpackaged-apps) or add the `Self-Contained` Feature to enable xcopy deployment.

## CI Pipelines

See [README.md](https://github.com/microsoft/TemplateStudio/blob/main/docs/WinUI/pipelines/README.md) for guidance on building and testing projects in CI pipelines.

## Changelog

See [releases](https://github.com/microsoft/TemplateStudio/releases) and [milestones](https://github.com/microsoft/TemplateStudio/milestones).

## Feedback

Bugs and feature requests should be filed at https://aka.ms/templatestudio.
