# OpenEOS
![GitHub release (latest by date)](https://img.shields.io/github/v/release/RobProductions/OpenEOS?logo=github)

An open-source port of the Epic Online Services (EOS) SDK in Unity Package format, with minor enhancements to provide a clean integration between EGS and Unity.

<img width = "800" src="Documentation~/DocAssets/OpenEOSLogoBanner.jpg">

## Overview

OpenEOS offers 2 main services to your Unity project:

1. A port of the relevant EOS SDK files, including C# and DLLs configured to work within the Editor and builds. This allows you to import the EOS SDK as a package instead of going about it through the usual drag-and-drop method Epic expects from you 
2. A few extensions and shortcuts to the Epic Online Services 

### Why Use OpenEOS?


### What is 



## Versioning

Since this package is both a port of the EOS SDK and an extension 

1.15.5

## Installation

### Recommended Installation

If you're looking for any specific release of OpenEOS, you can specify a release tag with the hashtag like so: "https://github.com/RobProductions/OpenEOS.git#ReleaseNumber"

1. Open the [Package Manager](https://docs.unity3d.com/2020.3/Documentation/Manual/upm-ui.html) in Unity
2. Copy the GitHub "HTTPS Clone" URL for OpenEOS: [https://github.com/RobProductions/OpenEOS.git](https://github.com/RobProductions/OpenEOS.git)
3. Click the '+' icon and hit *"Add package from git URL"*
4. Paste the HTTPS Clone URL to the popup and (optionally) add on *#YourChosenReleaseNumer* to the end, then hit enter
5. Wait for download to complete
6. You can now import RobProductions.OpenEOS namespace and Epic.OnlineServices namespace into your EOS manager to start using the SDK and quick service functions

Note that if you did not specify a release number, hitting the "update" button from Unity package manager may bump up not only the package version but the underlying EOS SDK version which may result in incompatibilities with your EOS code, so use it wisely and prepare for issues. 

### Optional Installations

**OpenUPM installation**

Check [this link](https://openupm.com/docs/getting-started.html#installing-a-upm-package) for the recommended steps.

**Local package installation**

Feel free to download the project as .zip and place it somewhere on your local drive. Then use the *"Add package from disk"* option in the Package Manager to add this local package instead of the remote installation.

**Installation failed or Unity not supported?**

If installation fails due to version requirements, you may be able force OpenEOS to work by downloading the project as .zip and editing the "package.json" file to lower the Unity requirement. Then use the *"Add package from disk"* option to add your custom version instead. 

**Assets path installation**

OpenEOS should also work as a part of your Assets/ directory if you'd like to customize it for your specific project without having to deal with the package system. Simply download the project as a .zip and place the contents anywhere in your Assets folder, as long as they are self-contained so that the Assembly Definition doesn't confuse itself with your other files. Note: when specifying the OpenEOS installation path in EOSCore.Init, you must now use Assets/path_to_openEOS.

## Updates & Contribution

For now, updates to the EOS SDK will happen manually and likely infrequently since I personally don't always need the latest version for my projects. In the future it would be great to have some automated batch process that places the SDK files correctly and renames the DLL holder to "Plugins" etc. so that can be something to improve in future releases.

### How to Contribute

This open source project is free for all to suggest improvements, and I'm hoping that more contributors could help clean up the code and add further features as suggested by the community. These are the recommended steps to add your contribution:

1. Fork the repository to your GitHub account
2. Clone the code to the Assets folder in any Unity project (as long as it does not include OpenGraphGUI as a package)
3. Create a new branch or work off of your own "working branch"
4. When your changes are complete, submit a pull request to merge your code; ideally to the "working-branch" used to test changes before main
5. If the PR is approved, aggregate changes will eventually be merged into main and a new release tag is created

## Credits & Details

Created by [RobProductions](https://twitter.com/RobProductions). RobProductions' Steam games can be found [here](https://store.steampowered.com/developer/robproductions).

### Requirements

- Tested with Unity 2020.3.26f1 and .NET 4.x, though it will likely work in earlier versions too. If support is confirmed for older versions I will gladly update the package JSON to improve compatibility.

### Limitations

- I mainly focused on developing this package for my own use cases which are geared towards PC and Mac deployment. As such, some steps of the SDK Initializaiton and Login process have been skipped and definitions for more advanced configurations are not yet available. These are relatively easy to update, so if you have need of more options in the EOSCore feel free to let me know.
- Currently the enhancements are mainly limited to SDK initialization, shutdown, and account authorization. This is because EGS Self-publishing only opened up a few days ago at the time of writing, so I haven't had time to look through more of the important SDK features. Also, these are the steps that are most relevant for my use case which will be performed often from my own EOS Managers. Other parts of the SDK I need such as Achievements are relatively simple in comparison, just needing a ProductUserId and string ID, so I felt that no helper functions were necessary. In the future as I discover more use cases for the SDK I may try to branch out and add more wrappers for commonly-used features. 

### License

The Epic Online Services SDK that is bundled in this package is subject to the license agreement found on [Epic's Developer Agreement page](https://dev.epicgames.com/en-US/services/terms/agreements).

This rest of this work is licensed under the MIT License. The intent for this software is to neatly package the Epic Online Services SDK and extend it freely and openly, without requirement of attribution. However, attribution for uses of this package would be much appreciated. The code may be considered "open source" and could include snippets from multiple collaborators. Contributors may be named in the README.md and Documentation files. 

The code is provided "as is" and without warranty. 
