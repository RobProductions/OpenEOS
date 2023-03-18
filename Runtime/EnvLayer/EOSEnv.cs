using Epic.OnlineServices.Auth;
using Epic.OnlineServices.Platform;
using Epic.OnlineServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RobProductions.OpenEOS
{
	/// <summary>
	/// Environment utilities for checking command line parameters
	/// that the EGS Launcher may pass in
	/// </summary>
	public static class EOSEnv
	{
		//TOKEN RETRIEVAL

		/// <summary>
		/// As per https://dev.epicgames.com/docs/en-US/epic-games-store/testing-guide,
		/// when launched through the Epic Games Launcher,
		/// the sandboxId will be passed into the app as a command line param.
		/// They recommend to use this to determine which SandboxID and DeploymentID
		/// to use when Initializing the EOS SDK on app launch. That way you can upload to Dev/Stage
		/// and use that same build when you go live - just include defs for all sandboxes.
		/// </summary>
		/// <returns>The ID of the running sandbox</returns>
		public static string GetSandboxID()
		{
			var sandboxId = GetCommandLineArgValue("epicsandboxid");
			return sandboxId;
		}

		/// <summary>
		/// As per https://dev.epicgames.com/docs/epic-account-services/auth/auth-interface,
		/// when launched through the Epic Games Launcher,
		/// the app's command line will be given 3 special codes:
		/// AUTH_LOGIN, AUTH_TYPE, and AUTH_PASSWORD,
		/// of which one is useful (password, which acts as the credential token)
		/// but I will check Auth Type just in case. Use this token
		/// to easily log in a user (to Auth service for EpicAccountId, using the ExchangeCode mode)
		/// without requiring the web browser.
		/// </summary>
		/// <returns>The token parsed from the command line</returns>
		public static string GetExchangeCodeToken()
		{
			var authTypeCode = GetCommandLineArgValue("AUTH_TYPE");
			if (authTypeCode != null && authTypeCode == "exchangecode")
			{
				var pass = GetCommandLineArgValue("AUTH_PASSWORD");
				if (pass == null)
				{
					EOSCore.LogEOS("Warning: AUTH_PASSWORD was null!");
				}

				return pass;
			}
			else
			{
				EOSCore.LogEOS("Warning: AUTH_TYPE was not exchangecode");
			}

			return null;
		}

		//ENV FUNCTION

		/// <summary>
		/// Input a command line name.
		/// Split a full command argument into its value and return the value.
		/// Note: The opening dash (-) will be appended if missing.
		/// </summary>
		/// <param name="argName"></param>
		/// <returns>The value after the = sign in a command line argument</returns>
		public static string GetCommandLineArgValue(string argName)
		{
			var fullVal = GetCommandLineArg(argName);
			if (fullVal != null)
			{
				var valSplit = fullVal.Split(new[] { '=' }, 2);
				if (valSplit.Length > 1)
				{
					return valSplit[1];
				}
				else
				{
					EOSCore.LogEOS("Warning: CommandLineArg " + fullVal + " could not be split with seperator");
				}
			}
			else
			{
				EOSCore.LogEOS("Warning: CommandLineArg " + argName + " did not exist in System environment");
			}
			return null;
		}

		/// <summary>
		/// Returns an argument string that starts with the given string.
		/// Epic Launcher formats some arguments like -NAME=VALUE,
		/// so we want to detect the given name and return the full string
		/// to parse out the value.
		/// Note: the opening dash (-) will be appended if missing.
		/// </summary>
		/// <param name="startsWith"></param>
		/// <returns>The full string of the argument</returns>
		public static string GetCommandLineArg(string startsWith)
		{
			var fullStartsWith = startsWith;
			if(!startsWith.StartsWith("-"))
			{
				fullStartsWith = "-" + startsWith;
			}
			var args = GetAllCommandLineArgs();
			for (int i = 0; i < args.Length; i++)
			{
				if (args[i].StartsWith(fullStartsWith))
				{
					return args[i];
				}
			}
			return null;
		}

		/// <summary>
		/// Get all of the command line arguments passed into the program.
		/// </summary>
		/// <returns></returns>
		public static string[] GetAllCommandLineArgs()
		{
			return System.Environment.GetCommandLineArgs();
		}
	}

}