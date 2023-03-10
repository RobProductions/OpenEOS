// Copyright Epic Games, Inc. All Rights Reserved.
// This file is automatically generated. Changes to this file may be overwritten.

namespace Epic.OnlineServices
{
	/// <summary>
	/// List of the supported identity providers to authenticate a user.
	/// 
	/// The type of authentication token is specific to each provider.
	/// Tokens in string format should be passed as-is to the function.
	/// Tokens retrieved as raw byte arrays should be converted into a hex-encoded UTF-8 string (e.g. "FA87097A..") before being passed to the function.
	/// <see cref="Common.ToString" /> can be used for this conversion.
	/// <seealso cref="Auth.AuthInterface.Login" />
	/// <seealso cref="Connect.ConnectInterface.Login" />
	/// </summary>
	public enum ExternalCredentialType : int
	{
		/// <summary>
		/// Epic Account Services Token
		/// 
		/// Using ID Token is preferred, retrieved with <see cref="Auth.AuthInterface.CopyIdToken" /> that returns <see cref="Auth.IdToken.JsonWebToken" />.
		/// Using Auth Token is supported for backwards compatibility, retrieved with <see cref="Auth.AuthInterface.CopyUserAuthToken" /> that returns <see cref="Auth.Token.AccessToken" />.
		/// 
		/// Supported with <see cref="Connect.ConnectInterface.Login" />.
		/// <seealso cref="Auth.AuthInterface.CopyIdToken" />
		/// <seealso cref="Auth.AuthInterface.CopyUserAuthToken" />
		/// </summary>
		Epic = 0,
		/// <summary>
		/// Steam Encrypted App Ticket
		/// 
		/// Generated using the ISteamUser::RequestEncryptedAppTicket API of Steamworks SDK.
		/// For ticket generation parameters, use pDataToInclude(<see langword="null" />) and cbDataToInclude(0).
		/// 
		/// The retrieved App Ticket byte buffer needs to be converted into a hex-encoded UTF-8 string (e.g. "FA87097A..") before passing it to the <see cref="Connect.ConnectInterface.Login" /> API.
		/// <see cref="Common.ToString" /> can be used for this conversion.
		/// 
		/// Supported with <see cref="Connect.ConnectInterface.Login" />.
		/// </summary>
		SteamAppTicket = 1,
		/// <summary>
		/// PlayStation(TM)Network ID Token
		/// 
		/// Retrieved from the PlayStation(R) SDK. Please see first-party documentation for additional information.
		/// 
		/// Supported with <see cref="Auth.AuthInterface.Login" />, <see cref="Connect.ConnectInterface.Login" />.
		/// </summary>
		PsnIdToken = 2,
		/// <summary>
		/// Xbox Live XSTS Token
		/// 
		/// Retrieved from the GDK and XDK. Please see first-party documentation for additional information.
		/// 
		/// Supported with <see cref="Auth.AuthInterface.Login" />, <see cref="Connect.ConnectInterface.Login" />.
		/// </summary>
		XblXstsToken = 3,
		/// <summary>
		/// Discord Access Token
		/// 
		/// Retrieved using the ApplicationManager::GetOAuth2Token API of Discord SDK.
		/// 
		/// Supported with <see cref="Connect.ConnectInterface.Login" />.
		/// </summary>
		DiscordAccessToken = 4,
		/// <summary>
		/// GOG Galaxy Encrypted App Ticket
		/// 
		/// Generated using the IUser::RequestEncryptedAppTicket API of GOG Galaxy SDK.
		/// For ticket generation parameters, use data(<see langword="null" />) and dataSize(0).
		/// 
		/// The retrieved App Ticket byte buffer needs to be converted into a hex-encoded UTF-8 string (e.g. "FA87097A..") before passing it to the <see cref="Connect.ConnectInterface.Login" /> API.
		/// For C/C++ API integration, use the <see cref="Common.ToString" /> API for the conversion.
		/// For C# integration, you can use <see cref="Helper.ToHexString" /> for the conversion.
		/// 
		/// Supported with <see cref="Connect.ConnectInterface.Login" />.
		/// </summary>
		GogSessionTicket = 5,
		/// <summary>
		/// Nintendo Account ID Token
		/// 
		/// Identifies a Nintendo user account and is acquired through web flow authentication where the local user logs in using their email address/sign-in ID and password.
		/// This is the common Nintendo account that users login with outside the Nintendo Switch device.
		/// 
		/// Supported with <see cref="Auth.AuthInterface.Login" />, <see cref="Connect.ConnectInterface.Login" />.
		/// </summary>
		NintendoIdToken = 6,
		/// <summary>
		/// Nintendo Service Account ID Token (NSA ID)
		/// 
		/// The NSA ID identifies uniquely the local Nintendo Switch device. The authentication token is acquired locally without explicit user credentials.
		/// As such, it is the primary authentication method for seamless login on Nintendo Switch.
		/// 
		/// The NSA ID is not exposed directly to the user and does not provide any means for login outside the local device.
		/// Because of this, Nintendo Switch users will need to link their Nintendo Account or another external user account
		/// to their Product User ID in order to share their game progression across other platforms. Otherwise, the user will
		/// not be able to login to their existing Product User ID on another platform due to missing login credentials to use.
		/// It is recommended that the game explicitly communicates this restriction to the user so that they will know to add
		/// the first linked external account on the Nintendo Switch device and then proceed with login on another platform.
		/// 
		/// In addition to sharing cross-platform game progression, linking the Nintendo Account or another external account
		/// will allow preserving the game progression permanently. Otherwise, the game progression will be tied only to the
		/// local device. In case the user loses access to their local device, they will not be able to recover the game
		/// progression if it is only associated with this account type.
		/// 
		/// Supported with <see cref="Auth.AuthInterface.Login" />, <see cref="Connect.ConnectInterface.Login" />.
		/// </summary>
		NintendoNsaIdToken = 7,
		/// <summary>
		/// Uplay Access Token
		/// </summary>
		UplayAccessToken = 8,
		/// <summary>
		/// OpenID Provider Access Token
		/// 
		/// Supported with <see cref="Connect.ConnectInterface.Login" />.
		/// </summary>
		OpenidAccessToken = 9,
		/// <summary>
		/// Device ID access token that identifies the current locally logged in user profile on the local device.
		/// The local user profile here refers to the operating system user login, for example the user's Windows Account
		/// or on a mobile device the default active user profile.
		/// 
		/// This credential type is used to automatically login the local user using the EOS Connect Device ID feature.
		/// 
		/// The intended use of the Device ID feature is to allow automatically logging in the user on a mobile device
		/// and to allow playing the game without requiring the user to necessarily login using a real user account at all.
		/// This makes a seamless first-time experience possible and allows linking the local device with a real external
		/// user account at a later time, sharing the same <see cref="ProductUserId" /> that is being used with the Device ID feature.
		/// 
		/// Supported with <see cref="Connect.ConnectInterface.Login" />.
		/// <seealso cref="Connect.ConnectInterface.CreateDeviceId" />
		/// </summary>
		DeviceidAccessToken = 10,
		/// <summary>
		/// Apple ID Token
		/// 
		/// Supported with <see cref="Connect.ConnectInterface.Login" />.
		/// </summary>
		AppleIdToken = 11,
		/// <summary>
		/// Google ID Token
		/// 
		/// Supported with <see cref="Connect.ConnectInterface.Login" />.
		/// </summary>
		GoogleIdToken = 12,
		/// <summary>
		/// Oculus User ID and Nonce
		/// 
		/// Call ovr_User_GetUserProof(), or Platform.User.GetUserProof() if you are using Unity, to retrieve the nonce.
		/// Then pass the local User ID and the Nonce as a "<see cref="UserID" />|<see cref="Nonce" />" formatted string for the <see cref="Connect.ConnectInterface.Login" /> Token parameter.
		/// 
		/// Note that in order to successfully retrieve a valid non-zero id for the local user using ovr_User_GetUser(),
		/// your Oculus App needs to be configured in the Oculus Developer Dashboard to have the User ID feature enabled.
		/// 
		/// Supported with <see cref="Connect.ConnectInterface.Login" />.
		/// </summary>
		OculusUseridNonce = 13,
		/// <summary>
		/// itch.io JWT Access Token
		/// 
		/// Use the itch.io app manifest to receive a JWT access token for the local user via the ITCHIO_API_KEY process environment variable.
		/// The itch.io access token is valid for 7 days after which the game needs to be restarted by the user as otherwise EOS Connect
		/// authentication session can no longer be refreshed.
		/// 
		/// Supported with <see cref="Connect.ConnectInterface.Login" />.
		/// </summary>
		ItchioJwt = 14,
		/// <summary>
		/// itch.io Key Access Token
		/// 
		/// This access token type is retrieved through the OAuth 2.0 authentication flow for the itch.io application.
		/// 
		/// Supported with <see cref="Connect.ConnectInterface.Login" />.
		/// </summary>
		ItchioKey = 15,
		/// <summary>
		/// Epic Games ID Token
		/// 
		/// Acquired using <see cref="Auth.AuthInterface.CopyIdToken" /> that returns <see cref="Auth.IdToken.JsonWebToken" />.
		/// 
		/// Supported with <see cref="Connect.ConnectInterface.Login" />.
		/// </summary>
		EpicIdToken = 16,
		/// <summary>
		/// Amazon Access Token
		/// 
		/// Supported with <see cref="Connect.ConnectInterface.Login" />.
		/// </summary>
		AmazonAccessToken = 17,
		/// <summary>
		/// Steam Auth Session Ticket
		/// 
		/// Generated using the ISteamUser::GetAuthSessionTicket API of Steamworks SDK.
		/// 
		/// The retrieved Auth Session Ticket byte buffer needs to be converted into a hex-encoded UTF-8 string (e.g. "FA87097A..") before passing it to the <see cref="Auth.AuthInterface.Login" /> or <see cref="Connect.ConnectInterface.Login" /> APIs.
		/// <see cref="Common.ToString" /> can be used for this conversion.
		/// 
		/// Supported with <see cref="Auth.AuthInterface.Login" />, <see cref="Connect.ConnectInterface.Login" />.
		/// </summary>
		SteamSessionTicket = 18
	}
}