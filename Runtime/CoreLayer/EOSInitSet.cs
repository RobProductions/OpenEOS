using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Epic.OnlineServices.Platform;


namespace RobProductions.OpenEOS
{
	/// <summary>
	/// Representation of a setup configuration for the EOSCore.
	/// Create this data set in your code and pass it in to the EOSCore.Init for a 
	/// Quick Initialization process!
	/// <br></br><br></br>
	/// It is recommended that you insert your own setup layer within your manager
	/// that is then passed into this set to work with cleaner data. i.e. you can hide
	/// your secrets, ignore unneeded parameters, and swap sandboxID based
	/// on an enum instead of a string. This class is left intentionally
	/// generic to handle all cases in the future.
	/// </summary>
	public class EOSInitSet
	{
		//SDK Init Options

		/// <summary>
		/// Initialize option - Human readable name of your EOS product.
		/// This should match your Product display name in EGS but you do you.
		/// </summary>
		public string productName = "Example Name";
		/// <summary>
		/// Initialize option - Version identifier string.
		/// There may not be any restrictions on what you put here,
		/// but it's helpful to provide your real app version from Application.version.
		/// </summary>
		public string productVersion = "1.0.0";

		//Platform Interface Options

		/// <summary>
		/// The ID of the intended Sandbox environment (Dev, Stage, Live).
		/// Find this in the "Product Settings" page in EOS dev portal.
		/// </summary>
		public string sandboxID = "";
		/// <summary>
		/// The ID of the intended deployment environment (Dev, Stage, Live).
		/// Find this in the "Product Settings" page in EOS dev portal.
		/// </summary>
		public string deploymentID = "";

		/// <summary>
		/// Build Patch Tool Credential - Client identifier.
		/// Find this in Product Settings in EOS dev portal under "General".
		/// </summary>
		public string clientID = "";
		/// <summary>
		/// Build Patch Tool Credential - Secret password.
		/// Find this in Product Settings in EOS dev portal under "General".
		/// </summary>
		public string clientSecret = "";

		/// <summary>
		/// EOS Product ID.
		/// Find this in the Product Settings page in EOS dev portal.
		/// </summary>
		public string productID = "";

		/// <summary>
		/// Additional flags to be passed into the Platform generator.
		/// This lets you more finely control the enabled SDK features.
		/// </summary>
		public PlatformFlags platformFlags = PlatformFlags.None;

		//Optional Interface Options

		/// <summary>
		/// Optional: Encryption key used by player data storage & title storage.
		/// Will be treated as null if empty string.
		/// </summary>
		public string encryptionKey = "";

		/// <summary>
		/// Optional: If true, EOS will launch assuming the client is a dedicated game server.
		/// Use false for local client, which is most likely what you want.
		/// </summary>
		public bool isServerFlag = false;
	}
}

