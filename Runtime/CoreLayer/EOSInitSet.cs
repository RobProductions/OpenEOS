using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Epic.OnlineServices.Platform;


namespace RobProductions.OpenEOS
{
	/// <summary>
	/// Representation of a setup configuration for the EOS Core.
	/// Create this data set in your code and pass it in to the Core for a 
	/// Quick Initialization process!
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
	}
}

