using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Epic.OnlineServices.Auth;
using Epic.OnlineServices;

namespace RobProductions.OpenEOS
{
	/// <summary>
	/// Representation of an Auth System Login configuration for EOSCore.LoginAuth().
	/// Pass this set into EOSCore.LoginAuth() to customize your login configuration.
	/// <br></br><br></br>
	/// It's recommended that you use an extra Auth layer in your EOS Manager
	/// to create this set programmatically, as you may need to attempt
	/// several authentication methods before login is successful.
	/// </summary>
	public class EOSLoginAuthSet
	{
		/// <summary>
		/// The type of login you want to perform.
		/// Epic expects developers to use PersistentAuth in final builds
		/// and then fall back to AccountPortal if it fails.
		/// </summary>
		public LoginCredentialType credentialType = LoginCredentialType.AccountPortal;

		/// <summary>
		/// The ID of the login credential. Will differ based on credentialType,
		/// could be a username or domain of dev user (localhost:port).
		/// Note: Some login types require null here instead of empty string.
		/// </summary>
		public string credentialID = null;

		/// <summary>
		/// The token/secret used to log in this user. Will differ based on credentialType.
		/// Could be dev username, continuance token, or exchange token passed from console, etc.
		/// Note: Some login types require null here instead of empty string.
		/// </summary>
		public string credentialToken = null;

		/// <summary>
		/// Used in the ExternalAuth credential type and ignored for any other type.
		/// Determines the source of the external credential.
		/// </summary>
		public ExternalCredentialType credentialExternalType = ExternalCredentialType.Epic;

		/// <summary>
		/// Permissions requested for this login session.
		/// Match the request to the feature set you enabled in your
		/// client policy your app is linked to in the dev portal. 
		/// If you don't, you'll get an error.
		/// </summary>
		public AuthScopeFlags authScopeFlags = AuthScopeFlags.BasicProfile | AuthScopeFlags.FriendsList | AuthScopeFlags.Presence;
	}
}