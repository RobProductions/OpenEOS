using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Epic.OnlineServices.Auth;


namespace RobProductions.OpenEOS
{
	/// <summary>
	/// Representation of a Login configuration for EOSCore.Login().
	/// Pass this set into EOSCore.Login() to customize your login configuration.
	/// <br></br><br></br>
	/// It's recommended that you use an extra Auth layer in your EOS Manager
	/// to create this set programmatically, as you may need to attempt
	/// several authentication methods before login is successful.
	/// </summary>
	public class EOSLoginSet
	{
		/// <summary>
		/// The type of login you want to perform.
		/// Epic expects developers to use PersistentAuth in final builds
		/// and then fall back to AccountPortal if it fails.
		/// </summary>
		public LoginCredentialType credentialType = LoginCredentialType.AccountPortal;

		/// <summary>
		/// The ID of the login credential. Will differ based on credentialType,
		/// could be a username or unique ID for a dev user.
		/// </summary>
		public string credentialID = "";

		public string credentialToken = "";

		/// <summary>
		/// Permissions requested for this login session.
		/// Match the request to the feature set intended with your Product.
		/// </summary>
		public AuthScopeFlags authScopeFlags = AuthScopeFlags.BasicProfile | AuthScopeFlags.FriendsList | AuthScopeFlags.Presence;
	}
}