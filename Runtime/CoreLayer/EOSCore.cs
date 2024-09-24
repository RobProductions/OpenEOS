using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Epic.OnlineServices;
using Epic.OnlineServices.Platform;
using System.Runtime.InteropServices;
using System;
using System.IO;
using Epic.OnlineServices.Auth;

namespace RobProductions.OpenEOS
{
	public static class EOSCore
	{
		//DEFS

		/// <summary>
		/// Represents the type of logic used for finding the OpenEOS package
		/// installation path. Use GitRemotePackage if you installed via
		/// Git and Local if you embedded the package.
		/// Use CustomPath to provide your own string.
		/// </summary>
		public enum InstallationPathType
		{
			GitRemotePackage = 0,
			LocalPackge = 1,
			CustomPath = 2
		}

		private const string eosPackageName = "com.robproductions.openeos";

		//TRACK

		/// <summary>
		/// Track whether or not you want EOSCore to log its own warnings.
		/// Use SetLogWarnings to change this.
		/// </summary>
		private static bool logWarnings = true;

		//CORE FUNCTIONS

		/// <summary>
		/// Quick Initialization setup for EOS SDK that automatically
		/// goes through the common init steps. Provide an EOSInitSet with populated values
		/// to determine the initialized EOS configuration (Product ID, Flags, Sandbox).
		/// <br></br>
		/// For EOS to work in Editor, provide pathToOpenEOS (Path to the OpenEOS install directory,
		/// in Assets or Library/PackageCache or Packages) so that Init can determine the location
		/// of the EOS DLLs which are provided with OpenEOS. Init will load these DLLs
		/// and use Epic's recommended steps to call Bindings.Hook with the loaded libraries.
		/// As of SDK version 1.12 it is NO LONGER the case that you forgo full shutdown in Editor
		/// because this loading and unloading of libraries will allow you to recall the SDK init.
		/// This check is skipped in actual builds so you can safely pass the install path regardless
		/// of the configuration. 
		/// <br></br>
		/// Ensure that you use EOSCore.Shutdown to stop the SDK and unload Editor libraries.
		/// </summary>
		/// <param name="initSet">Create after importing RobProductions.OpenEOS namespace and provide
		/// this object to customize the configuration used by EOSInit.</param>
		/// <param name="openEOSPathType">Designate the package installation type so that Init() can
		/// find the path to your package instance. Only needed in Editor mode for loading libraries.</param>
		/// <param name="customPathToOpenEOS">Custom override path to the OpenEOS installation directory, typically
		/// Library/PackageCache/com.robproductions.openeos@someindex if you installed via Package Manager,
		/// Packages/com.robproductions.openeos if you embedded the package,
		/// Assets/OpenEOS if you placed it directly into your project. Only needed if you use
		/// custom installation path type.</param>
		/// <returns>The generated PlatformInterface state if successful and null if
		/// unsuccessful for any reason.</returns>
		public static PlatformInterface Init(EOSInitSet initSet, InstallationPathType openEOSPathType, string customPathToOpenEOS = "")
		{
			//According to Epic's EOS documention,
			//EOS libraries should be dynamically loaded and unloaded in the Editor.
			//So we will bind the DLL before initialization.
			//https://dev.epicgames.com/docs/epic-online-services/eos-get-started/eossdkc-sharp-getting-started

#if UNITY_EDITOR

			var finalPathToOpenEOS = customPathToOpenEOS;

			if (openEOSPathType == InstallationPathType.LocalPackge)
			{
				//This should be a constant path in Packages
				finalPathToOpenEOS = Path.Combine("Packages", eosPackageName);
				var localInfo = new DirectoryInfo(finalPathToOpenEOS);
				if(!localInfo.Exists)
				{
					finalPathToOpenEOS = Path.Combine("Packages", eosPackageName + "-main");
					localInfo = new DirectoryInfo(finalPathToOpenEOS);
					if(!localInfo.Exists)
					{
						LogEOS("Warning: Installation Type set to Local Package but " + finalPathToOpenEOS + " did not exist! "
							+ "Please change installation type or use custom path option.");
					}
				}
			}
			else if (openEOSPathType == InstallationPathType.GitRemotePackage)
			{
				//This is a path to the PackageCache but with some arbitrary index that Unity assigns,
				//so we need to search each directory and trim off the @ sign
				var pathToLibraryCache = "Library/PackageCache/";
				try
				{
					var directoryList = Directory.GetDirectories(pathToLibraryCache);
					
					bool foundPackage = false;
					foreach (string subDir in directoryList)
					{
						var subDirLastFolder = new DirectoryInfo(subDir).Name;
						var subDirSplit = subDirLastFolder.Split(new[] { '@' }, 2);
						if(subDirSplit.Length > 0)
						{
							if (subDirSplit[0] == eosPackageName)
							{
								foundPackage = true;
								finalPathToOpenEOS = subDir;
								break;
							}
						}
					}
					if(!foundPackage)
					{
						LogEOS("Warning: Could not find OpenEOS in Git Remote package location (" + pathToLibraryCache + "). "
							+ "Ensure that OpenEOS is installed there or use a custom path option.");
					}
				}
				catch(UnauthorizedAccessException e)
				{
					LogEOS("Error reading Remote Package directories: " + e.ToString());
				}
			}

			if (finalPathToOpenEOS == "")
			{
				LogEOS("Warning: Editor Mode detected but the path to OpenEOS is empty! Please provide an installation path.");
			}
			else
			{
				var dirInfo = new DirectoryInfo(finalPathToOpenEOS);
				if(!dirInfo.Exists)
				{
					LogEOS("Warning: Final path to OpenEOS (" + finalPathToOpenEOS + ") does not exist! "
						+ "Ensure that it leads to the installed OpenEOS package directory.");
				}
				else
				{
					var finalPathToPlugins = Path.Combine(finalPathToOpenEOS, "Runtime/EOSSDK/SDK/Plugins/");
					var pluginsInfo = new DirectoryInfo(finalPathToPlugins);
					if(!pluginsInfo.Exists)
					{
						LogEOS("Warning: Path to SDK/Plugins does not exist! Ensure that the path to OpenEOS leads to the installed OpenEOS package directory "
							+ "and that Runtime/EOSSDK/SDK exists within the directory.");
					}
					else
					{
						try
						{
							LoadEditorLibraries(finalPathToPlugins);
						}
						catch (Exception e)
						{
							LogEOS(e.Message);
							LogEOS(e.StackTrace);
						}
					}
				}
			}
#endif

			//Initialize SDK
			var initOptions = new InitializeOptions()
			{
				ProductName = initSet.productName,
				ProductVersion = initSet.productVersion,
			};

			Result result = PlatformInterface.Initialize(ref initOptions);

			if(result != Result.Success && result != Result.AlreadyConfigured)
			{
				//SDK Initialization failure
				LogEOS("Error: PlatformInterface.Initialize() failed! ResultCode: " + result.ToString());
				return null;
			}

			//Generate Platform
			var clientCredentials = new ClientCredentials();
			clientCredentials.ClientId = initSet.clientID;
			clientCredentials.ClientSecret = initSet.clientSecret;

			Utf8String encryptionKey = null;
			if(initSet.encryptionKey != null && initSet.encryptionKey != "")
			{
				encryptionKey = initSet.encryptionKey;
			}

			var platformOptions = new Options()
			{
				ClientCredentials = clientCredentials,
				ProductId = initSet.productID,
				SandboxId = initSet.sandboxID,
				DeploymentId = initSet.deploymentID,
				Flags = initSet.platformFlags,
				EncryptionKey = encryptionKey,
				IsServer = initSet.isServerFlag,
				//CacheDirectory = ???,
				//OverrideCountryCode = false,
				//OverrideLocaleCode = false,
				//TickBudgetInMilliseconds = 0            
			};

			var retInterface = PlatformInterface.Create(ref platformOptions);

			return retInterface;
		}

		/// <summary>
		/// Shutdown the EOS SDK cleanly. Provide the session that was given
		/// on EOSInit (PlatformInterface) to close it.
		/// Use completeShutdown in non-editor builds only so that
		/// SDK can shutdown completely. Otherwise, you want the SDK
		/// to remain partially initialized or else EOS will throw errors
		/// until the Unity Editor itself has closed.
		/// This can be achieved through #if UNITY_EDITOR checks in your EOS manager.
		/// Returns true when successful and false if otherwise.
		/// </summary>
		/// <param name="platformSession">PlatformInterface that was generated by EOS
		/// when the SDK was initialized.</param>
		/// <returns>True when shutdown was successful, false if otherwise.</returns>
		public static bool Shutdown(PlatformInterface platformSession)
		{
			if(platformSession != null)
			{
				//Release the memory from this session
				platformSession.Release();
			}

			//Then disable the SDK
			var result = PlatformInterface.Shutdown();
			if (result != Result.Success)
			{
				return false;
			}

			//In an Editor environment, Epic recommends to dynamically load
			//and unload the libraries using Bindings.Hook and Unhook.
			//https://dev.epicgames.com/docs/epic-online-services/eos-get-started/eossdkc-sharp-getting-started

#if UNITY_EDITOR
			try
			{
				UnloadEditorLibraries();
			}
			catch(Exception e)
			{
				LogEOS(e.Message);
				LogEOS(e.StackTrace);
			}
#endif

			return true;
		}

		//EDITOR UTIL

		//Snippet provided by EOS Sample code
		//https://dev.epicgames.com/docs/epic-online-services/eos-get-started/eossdkc-sharp-getting-started
#if UNITY_EDITOR
		[DllImport("Kernel32.dll")]
		private static extern IntPtr LoadLibrary(string lpLibFileName);

		[DllImport("Kernel32.dll")]
		private static extern int FreeLibrary(IntPtr hLibModule);

		[DllImport("Kernel32.dll")]
		private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

		private static IntPtr m_LibraryPointer;
#endif

		/// <summary>
		/// Use EOS SDK Bindings.Hook to load in the path the Shipping DLL.
		/// This DLL is bundled within OpenEOS, however we need an installation path,
		/// because users can place OpenEOS anywhere they want.
		/// In Init, you can pass the path to OpenEOS and then Init will add in
		/// the path to the Plugins folder before calling this function.
		/// Throws an exception when the library fails to load.
		/// </summary>
		/// <param name="libraryPathPrefix">Path to the Plugins folder in OpenEOS</param>
		private static void LoadEditorLibraries(string libraryPathPrefix)
		{
#if UNITY_EDITOR
			var libraryPath = libraryPathPrefix + Config.LibraryName;

			m_LibraryPointer = LoadLibrary(libraryPath);
			if (m_LibraryPointer == IntPtr.Zero)
			{
				throw new Exception("Failed to load library" + libraryPath);
			}

			Bindings.Hook(m_LibraryPointer, GetProcAddress);
#endif
		}

		/// <summary>
		/// Unhook EOS SDK Bindings and use FreeLibrary to remove library references.
		/// </summary>
		private static void UnloadEditorLibraries()
		{
#if UNITY_EDITOR
			if (m_LibraryPointer != IntPtr.Zero)
			{
				Bindings.Unhook();

				//Free up to 128 references that could potentially be using this library
				for (int i = 0; i < 128; i++)
				{
					//FreeLibrary returns the number of references left after freeing
					int freeResult = FreeLibrary(m_LibraryPointer);
					if(freeResult <= 0)
					{
						//No more need to remove references
						break;
					}
					if(i == 127)
					{
						LogEOS("Warning: Editor used up 127 references of EOS library");
					}
				}
				m_LibraryPointer = IntPtr.Zero;
			}
#endif
		}
		
		//LOGGING

		/// <summary>
		/// Use this to set whether or not OpenEOS should log its own warnings.
		/// </summary>
		/// <param name="v"></param>
		public static void SetLogWarnings(bool v)
		{
			logWarnings = v;
		}

		/// <summary>
		/// Use this to query whether or not OpenEOS is logging warnings.
		/// </summary>
		/// <returns></returns>
		public static bool GetLogWarnings()
		{
			return logWarnings;
		}

		/// <summary>
		/// Run a Debug Log with some extra flavor text.
		/// </summary>
		/// <param name="message"></param>
		public static void LogEOS(string message)
		{
			if(logWarnings)
			{
				Debug.Log("OpenEOS: " + message);
			}
		}
	}
}

