# OpenEOS for Unity  
[![openupm](https://img.shields.io/npm/v/com.robproductions.openeos?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.robproductions.openeos/) ![GitHub release (latest by date)](https://img.shields.io/github/v/release/RobProductions/OpenEOS?logo=github)

An open-source port of the [Epic Online Services (EOS) SDK](https://dev.epicgames.com/docs/epic-online-services) in Unity Package format, with minor enhancements to provide a clean integration between EGS and Unity.

<img width="800" src="Documentation~/DocAssets/OpenEOSLogoBanner.jpg">

## Overview

OpenEOS offers two main services to your Unity project:  

1. A port of the relevant EOS SDK files, including the C# codebase and DLLs configured to work within the Editor and builds. This allows you to import the EOS SDK as a package instead of managing the SDK on a project-by-project basis in your Assets folder.  
2. A few helper extensions and shortcuts for commonly used Epic Online Services features, such as SDK initialization and account management.

### OpenEOS... Why?

When researching the EOS SDK for use in my own projects, I was surprised to see only one or two usable plugins for integration with Unity (at the time of writing). Looking into those, they are robust in their own right. However, they offer full "manager" solutions that don't allow much customization and essentially sit on top of the regular Online Services library.

My goal for OpenEOS was to provide a more lightweight solution that can easily be deployed to all of my projects—something more organized than Epic's recommended "drag and drop into Assets" workflow but more customizable than the existing EOS Manager packages. Overall, I set out to create a package that lets you jumpstart *your own* EOS Manager, which can be customized per project while also eliminating redundant work with helpful extensions. As such, this is not a complete solution, and you’ll need to write your own EOS Manager that interfaces with the OpenEOS layer or the EOS SDK directly.

If you're looking for a more robust solution that includes its own manager, I highly recommend [PlayEveryWhere's EOS Plugin for Unity](https://github.com/PlayEveryWare/eos_plugin_for_unity). The package is officially recommended by Epic and will quickly get your project set up if you don't care about fine-tuning your SDK implementation. I also want to thank [Dylan Hunt's EOS-Unity project](https://gitlab.com/dylanh724/eos-unity), which helped me understand the common initialization steps for EOS. I believe the project is deprecated now, but it still serves as a useful reference for setting up the SDK.

### How should I use OpenEOS?

You can use OpenEOS in either or both of the following ways:  

1. A neatly packaged container for the EOS SDK that you can interface with directly by importing the `Epic.OnlineServices` namespace into your custom setup code.  
2. A set of extensions that interact with the packaged SDK, providing helper functions to simplify SDK usage. Simply import the `RobProductions.OpenEOS` namespace to use the *EOSCore*, *EOSAuth*, and *EOSEnv* static libraries to quickly implement common SDK steps: initialization, shutdown, authentication system login, connect system login, translation of `EpicAccountId` to `ProductUserId`, and acquisition of command-line arguments from the EGS Launcher.

## Versioning

Since this package is both a port of the EOS SDK and an extension, versioning is somewhat unique. The package code builds on top of the included EOS SDK version, and users will likely care about which version of the SDK they are integrating. The release number follows this format:

*X.X.X-Y.Y.Y*, where X.X.X is the package code version, and Y.Y.Y is the Epic Online Services SDK version.  
Example: **1.3.0-1.15.2** means package version 1.3.0 targeting EOS SDK version 1.15.2.  

Unfortunately, since Unity [enforces the SemVer system in the `package.json`](https://docs.unity3d.com/Manual/upm-semver.html), this versioning scheme will only be viewable on GitHub. Use the release tags to map code versions to SDK versions, as only X.X.X will appear in the Unity Package Manager.

**Current EOS SDK Target:** 1.16.1

### Disclaimer

Updates to the EOS SDK will result in a new package version so that `package.json` reflects the change. However, the package will not revert to previous SDK versions. In other words, package updates will only target newer versions of the SDK.  
*Example: If you need version 1.3.0 targeting SDK 1.14, but version 1.2.0 already targets SDK 1.15.2, a specific branch or release would need to be created.* Exceptions can be made if there is sufficient demand.

## Usage

You can use this package to interface with the SDK directly or take advantage of wrapper shortcuts provided by OpenEOS. Let's start with the first use case.

### Accessing Epic Online Services SDK

After installing the package, import the relevant EOS namespace to begin working with the SDK inside your custom EOS Manager. You might need namespaces like `Epic.OnlineServices`, `Epic.OnlineServices.Auth`, or `Epic.OnlineServices.Connect`. Refer to [Epic's EOS documentation](https://dev.epicgames.com/docs/epic-online-services) for detailed usage of these services. The [C# getting started page](https://dev.epicgames.com/docs/epic-online-services/eos-get-started/eossdkc-sharp-getting-started) also provides useful setup steps for working with Unity. If you install this package, you can skip the initial steps of importing the SDK and configuring the DLLs.

Refer to the [EOS SDK API](https://dev.epicgames.com/docs/api-ref) for information about the SDK interfaces and their expected parameters. If you are new to the EOS SDK, it's essential to understand the complete workflow Epic expects from your project. Start with [this introduction](https://dev.epicgames.com/docs/epic-online-services/eos-get-started/introduction-resources) to learn more, though examples are often more informative. Here's a brief summary of the expected flow within your EOS Manager:

1. **Initialize the SDK.** (In Editor mode, this involves [dynamically loading and unloading](https://dev.epicgames.com/docs/epic-online-services/eos-get-started/eossdkc-sharp-getting-started) the EOS libraries.)  
2. **Acquire the `PlatformInterface`,** which acts as your EOS session. Ensure you call `.Tick()` periodically.  
3. **Log in an Epic User** using the Auth system (if needed).  
4. **Log in a generic user** through a provider using the Connect system, or convert the Epic User to a generic Product User.  
5. **Use the `PlatformInterface` and user data** to interact with the SDK, such as unlocking achievements or uploading statistics.  
6. **Shut down the SDK** on app exit and release the memory used by the `PlatformInterface`.

### EOSCore Layer

If you'd like an easier time performing common core SDK steps within your EOS Manager, you can import the `RobProductions.OpenEOS` namespace and use the helper functions provided by **EOSCore** to initialize and shut down the SDK. All the code contains summary comments so you can read about what each function does in your IDE.

**EOSCore.Init()** will automatically initialize the SDK. It uses the custom `EOSInitSet` class, allowing you to provide your initialization configuration, mainly the five secret IDs that associate your app with the EOS service. You can find the required secret IDs in your developer portal. Read more about the developer portal [here](https://dev.epicgames.com/docs/dev-portal/product-management). Additionally, `Init()` requires the project path to the OpenEOS installation. With the new *"InstallationPathType"* enum, you can have `Init()` automatically find your installation path by specifying how you installed OpenEOS. 

Alternatively, you can use the custom path type and provide the *customPathToOpenEOS* to manually specify your installation location. For this method, assume the search starts at the root of the project. For example, if you installed via the Package Manager’s remote method, provide something like `"Library/AssetCache/com.robproductions.openeos@someuniqueid"`, as shown in your file browser. 

The installation path parameters are used only in **Editor mode**, allowing EOSCore to locate the Plugins folder within the `EOSSDK` folder and dynamically load EOS DLLs before running the main initialization steps. This method is recommended on the [C# Getting Started page](https://dev.epicgames.com/docs/epic-online-services/eos-get-started/eossdkc-sharp-getting-started) because it allows you to shut down the SDK in Editor mode without being constrained by the Unity Editor’s lifecycle. As of version 1.15, this is the required method for properly initializing the SDK in Unity Editor.

With just 2-3 parameters as input into **EOSCore.Init()**, you can quickly start the SDK and retrieve the **PlatformInterface** for use in all future SDK requests. If the returned platform is not `null`, you have successfully started the SDK. Here’s an example:

```cs
var initSet = new EOSInitSet()
{
    productName = "Test Game",
    productVersion = "V1.1",
    clientID = "------------------",
    clientSecret = "-----------------",
    productID = "--------------------",
    sandboxID = "--------------------",
    deploymentID = "----------------",
};

platform = EOSCore.Init(initSet, EOSCore.InstallationPathType.CustomPath, "Assets/MyPackages/OpenEOS/");

if (platform != null)
{
    initialized = true;
}
```

Remember to call `.Tick()` at least a few times per second on the provided **PlatformInterface** to keep EOS running.

To determine the appropriate `SandboxID` and `DeploymentID`, you can use **EOSEnv.GetSandboxID()** to retrieve the command-line parameter passed by the Epic Games Launcher. As per [Epic's testing guide](https://dev.epicgames.com/docs/en-US/epic-games-store/testing-guide), this method ensures your builds work seamlessly across different sandbox environments. However, it's up to you to test this and add fallback logic if needed—for example, when users run the app outside the EGS Launcher.

**EOSCore.Shutdown()** performs all the necessary cleanup steps to shut down the SDK. It requires the **PlatformInterface** generated from `Init()` to release memory allocated for the session. Additionally, it dynamically unloads the libraries linked during initialization using `Bindings.Unhook()`. If you used `Init()`, ensure you call `Shutdown()` at the end of your app’s lifecycle, typically in `OnApplicationQuit` or `OnDestroy`.

---

### EOSAuth Layer

**EOSAuth** provides shortcuts for working with EOS accounts and user systems. EOS offers two main login systems: **Auth** and **Connect**.

**EOSAuth.LoginAuth()** allows you to log in a user via the Auth system, which handles Epic accounts. Provide the custom `EOSLoginAuthSet` class with the credentials filled out, and `LoginAuth()` will run the asynchronous operation. The `OnLoginCallback` will be invoked when the operation completes, and based on the `ResultCode`, you’ll know whether the login was successful. Epic recommends trying multiple login types: 

1. **Persistent Auth** – Checks if a session already exists.  
2. **Exchange Code** – Uses a code from the launcher to log in automatically.  
3. **Account Portal** – Opens a web browser as a fallback option.

Once logged in, you’ll receive an **EpicAccountId** to reference the user. For Exchange Code login, use **EOSEnv.GetExchangeCodeToken()** to retrieve the token from the command line. Note that this token will not be available in the Editor, so your EOS Manager should detect when to use this login type.

To test your login system with the "Developer" login type, you’ll need to use the **DevAuthTool** provided by the SDK. You can download it from the [developer portal](https://dev.epicgames.com/docs/epic-online-services/eos-get-started/services-quick-start) and find it in the "tools" folder of the SDK archive. 

<img width="800" src="Documentation~/DocAssets/DevAuthToolScreen.jpg">

After obtaining an **EpicAccountId**, some SDK features require a **ProductUserId**. You can either translate an Epic user or use a provider service (Google, Steam, etc.) via the [Connect Interface](https://dev.epicgames.com/docs/game-services/eos-connect-interface).

**EOSAuth.LoginConnect()** is a wrapper for the Connect process. It takes user credentials and optional data in the custom `EOSLoginConnectSet` container and provides a callback with the `ResultCode`. You can use **EOSAuth.LoginEpicAccountToProductUser()** to simplify the conversion from `EpicAccountId` to `ProductUserId`. If the connection fails, use **EOSAuth.LoginEpicAccountToProductUserWithCreate()** to create the user if not already linked.

Your complete login flow might look like this:

1. Use `LoginAuth()` with **Persistent Auth** and provide a callback to check the result.
2. If it fails, retry with **Exchange Code** login.
3. If that also fails, use **Account Portal** login as a fallback.
4. On success, call `LoginEpicAccountToProductUserWithCreate()` and store the `ProductUserId`.

---

### EOSEnv Layer

**EOSEnv** provides utilities for retrieving values from the command line used by the EGS Launcher. 

- **EOSEnv.GetAllCommandLineArgs()** – Helps debug all parameters passed to the app.  
- **EOSEnv.GetCommandLineArgValue()** – Returns the value of a specific parameter by name.  
- **GetExchangeCodeToken()** – Retrieves the Exchange Code token for login.  
- **GetSandboxID()** – Identifies the current sandbox environment (e.g., Dev, Stage, Live). 

If the user runs the app outside the EGS Launcher, these parameters may not be available, and OpenEOS will print a warning.

---

### Disabling at Compile Time

You can use the `EOS_DISABLE` scripting define symbol to exclude the SDK and the OpenEOS wrapper library from the build. Navigate to **Project Settings** → **Scripting Define Symbols** to add the symbol, or set it via [custom scripting methods](https://docs.unity3d.com/Manual/CustomScriptingSymbols.html). 

Example usage:

```cs
#if !EOS_DISABLE
using RobProductions.OpenEOS;
using Epic.OnlineServices;
using Epic.OnlineServices.Auth;
using Epic.OnlineServices.Achievements;
using Epic.OnlineServices.Connect;
#endif
```
## Installation

### Recommended Installation

If you're looking for a specific release of OpenEOS, you can specify a release tag with a hashtag like this:  
`https://github.com/RobProductions/OpenEOS.git#ReleaseNumber`

1. Open the [Package Manager](https://docs.unity3d.com/2020.3/Documentation/Manual/upm-ui.html) in Unity.
2. Copy the GitHub "HTTPS Clone" URL for OpenEOS: [https://github.com/RobProductions/OpenEOS.git](https://github.com/RobProductions/OpenEOS.git).
3. Click the '+' icon and select *"Add package from git URL."*
4. Paste the HTTPS Clone URL into the popup and (optionally) append *#YourChosenReleaseNumber* at the end, then press Enter.
5. Wait for the download to complete.
6. You can now import the `RobProductions.OpenEOS` namespace and `Epic.OnlineServices` namespace into your EOS Manager to start using the SDK and its quick service functions.

**Note:**  
If you don't specify a release number, clicking the "update" button in the Unity Package Manager might not only update the package but also upgrade the underlying EOS SDK version. This can lead to potential incompatibilities with your existing EOS code, so proceed with caution.

---

### Optional Installations

#### **OpenUPM Installation**  
Follow the [recommended steps](https://openupm.com/docs/getting-started.html#installing-a-upm-package) to install through OpenUPM.

#### **Local Package Installation**  
You can download the project as a .zip file and place it on your local drive. Use the *"Add package from disk"* option in the Package Manager to add the package from your local installation. Ensure the resulting folder in your project's `Packages` directory is named either `com.robproductions.openeos` or `com.robproductions.openeos-main` so that `Init()` can locate the installation directory properly.

#### **Handling Installation Issues or Unsupported Unity Versions**  
If the installation fails due to Unity version requirements, you can download the project as a .zip, edit the `package.json` file to lower the Unity version requirement, and add it via *"Add package from disk."*

#### **Installing OpenEOS in the Assets Folder**  
If you prefer to customize OpenEOS without using the package system, you can place it directly in your `Assets/` directory. Download the project as a .zip and place its contents into a folder within `Assets`. Ensure the files are self-contained to avoid confusion with other Assembly Definitions. When specifying the OpenEOS path in `EOSCore.Init()`, use the following format: `Assets/path_to_openEOS`.

---

### Want More API Details?

If you installed OpenEOS via Git, make sure that .csproj files for "Git Packages" are enabled. Go to:  
*Edit -> Preferences -> External Tools*.

<img width="500" src="Documentation~/DocAssets/GitPackagesSetting.jpg">

With this enabled, you can view summary comments, return values, input parameters, and function descriptions directly in your IDE. This also allows you to explore Epic’s SDK comments alongside OpenEOS enhancements, giving you better insight into the code.

---

## Updates & Contributions

OpenEOS updates to the EOS SDK will happen manually and infrequently, as I usually work with stable versions for my projects. In the future, I hope to automate the process of updating SDK files and renaming DLL folders for easier integration.

### How to Contribute  

1. Fork the repository to your GitHub account.
2. Clone the code into the `Assets` or `Packages` folder of a test Unity project.
3. Create a new branch or work off of your own working branch.
4. After making changes, submit a pull request to merge your code. Ideally, target the "working-branch" used for testing before merging into the main branch.
5. Upon approval, your changes will be merged into the main branch, and a new release tag will be created.

---

## Credits & Details

Created by [RobProductions](https://twitter.com/RobProductions). Check out RobProductions' games on [Steam](https://store.steampowered.com/developer/robproductions).

### Requirements  

- Tested with Unity 2020.3.26f1 and .NET 4.x, though it may work with earlier versions. If confirmed, the `package.json` will be updated for better compatibility.

### Limitations  

- **Platform Support:** This package was developed mainly for PC and Mac, with SDK DLLs configured for Windows (32-bit and 64-bit), macOS, and Linux. Mobile platforms like iOS and Android are currently not supported due to missing modules and potential DLL conflicts. Future support may be added if compatibility is confirmed.  
- **Simplified Initialization:** Some SDK setup steps are simplified to match my use cases. If advanced configurations are needed, feel free to contribute to the project or contact me.  
- **Focus on Core Features:** Current enhancements primarily focus on SDK initialization, shutdown, and account authorization, as these are my immediate priorities. Other features like achievements are straightforward to implement, needing only a `ProductUserId` and a string ID. Additional wrappers for other SDK features may be added in the future based on new use cases.

---

## License

The bundled Epic Online Services SDK is subject to the [Epic Developer Agreement](https://dev.epicgames.com/en-US/services/terms/agreements). 

The rest of the code in this package is licensed under the MIT License. While attribution is not required, it would be greatly appreciated. The software is open source, and contributors will be listed in the README.md or documentation files.  

**Disclaimer:** This code is provided "as is," without any warranties. 
