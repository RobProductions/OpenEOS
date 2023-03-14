using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Epic.OnlineServices.Connect;
using Epic.OnlineServices;

namespace RobProductions.OpenEOS
{
	/// <summary>
	/// Representation of a Connect System Login configuration for EOSCore.LoginConnect().
	/// </summary>
	public class EOSLoginConnectSet
	{
		/// <summary>
		/// The token for the desired user.
		/// Will vary based on credentialsType.
		/// </summary>
		public string credentialsToken;

		/// <summary>
		/// Type of credential used by the Connect system.
		/// </summary>
		public ExternalCredentialType credentialsType;
		
		/// <summary>
		/// Additional information about the user only used
		/// in external auth providers.
		/// </summary>
		public UserLoginInfo? additionalLoginInfo;

	}
}