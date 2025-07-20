using System;
using System.Collections.Generic;
using HeathenEngineering.SteamworksIntegration;
using HeathenEngineering.SteamworksIntegration.API;
using Steamworks;
using UnityEngine;

// Token: 0x02000058 RID: 88
[DefaultExecutionOrder(-1)]
public class Authenticator : Singleton<Authenticator>
{
	// Token: 0x1700003D RID: 61
	// (get) Token: 0x0600046B RID: 1131 RVA: 0x00017612 File Offset: 0x00015812
	public static bool UserAuthenticated
	{
		get
		{
			return Authenticator.userAuthenticated;
		}
	}

	// Token: 0x1700003E RID: 62
	// (get) Token: 0x0600046C RID: 1132 RVA: 0x00017619 File Offset: 0x00015819
	public static bool PiratedCopy
	{
		get
		{
			return Authenticator._piratedCopy;
		}
	}

	// Token: 0x0600046D RID: 1133 RVA: 0x00017620 File Offset: 0x00015820
	public static void Login()
	{
		if (!Authenticator.UserAuthenticated)
		{
			Debug.Log("[STEAM] Sending authentication ticket request");
			try
			{
				Authentication.GetAuthSessionTicket(delegate(AuthenticationTicket ticket, bool IOError)
				{
					if (!IOError)
					{
						Authenticator.AuthenticateUser(ticket, UserData.Me);
						return;
					}
					Debug.Log("[STEAM] Ran into error while authenticating");
					Authenticator.userAuthenticated = false;
					Singleton<AuthEvents>.Instance.onAuthenticationFailed.Invoke("Connection to server timed out");
				});
				return;
			}
			catch (Exception ex)
			{
				Debug.Log("[STEAM] Client ran into error while authenticating.\n\nMessage: " + ex.Message);
				Singleton<AuthEvents>.Instance.onAuthenticationFailed.Invoke(ex.Message);
				return;
			}
		}
		Singleton<AuthEvents>.Instance.onAuthenticationSuccessful.Invoke("Client already authenticated.");
	}

	// Token: 0x0600046E RID: 1134 RVA: 0x000176B8 File Offset: 0x000158B8
	public static void Logout(AuthenticationTicket ticket)
	{
		Authentication.CancelAuthTicket(ticket);
		Authentication.EndAuthSession(UserData.Me);
		Debug.Log("[STEAM] Reset authentication session");
	}

	// Token: 0x0600046F RID: 1135 RVA: 0x000176DC File Offset: 0x000158DC
	private static void AuthenticateUser(AuthenticationTicket ticket, UserData user)
	{
		switch (Authentication.BeginAuthSession(ticket.Data, user, delegate(AuthenticationSession result)
		{
			string text;
			switch (result.Response)
			{
			case EAuthSessionResponse.k_EAuthSessionResponseOK:
				text = "Authentication Successful";
				Debug.Log("[STEAM] " + text);
				Authenticator.userAuthenticated = true;
				break;
			case EAuthSessionResponse.k_EAuthSessionResponseUserNotConnectedToSteam:
				text = "Not connected to Steam, check online status";
				Debug.Log("[STEAM] " + text);
				Authenticator.userAuthenticated = false;
				break;
			case EAuthSessionResponse.k_EAuthSessionResponseNoLicenseOrExpired:
				text = "License not owned";
				Debug.Log("[STEAM] " + text);
				Authenticator.userAuthenticated = false;
				Authenticator._piratedCopy = true;
				break;
			case EAuthSessionResponse.k_EAuthSessionResponseVACBanned:
				text = "VAC banned by developer";
				Debug.Log("[STEAM] " + text);
				Authenticator.userAuthenticated = false;
				break;
			case EAuthSessionResponse.k_EAuthSessionResponseLoggedInElseWhere:
				text = "Duplicate game session, unable to authneticate";
				Debug.Log("[STEAM] " + text);
				Authenticator.userAuthenticated = false;
				break;
			case EAuthSessionResponse.k_EAuthSessionResponseVACCheckTimedOut:
				text = "VAC check timed out, cannot authenticate";
				Debug.Log("[STEAM] " + text);
				Authenticator.userAuthenticated = false;
				break;
			case EAuthSessionResponse.k_EAuthSessionResponseAuthTicketCanceled:
				text = "Duplicate ticket request";
				Debug.Log("[STEAM] " + text);
				Authenticator.userAuthenticated = false;
				break;
			case EAuthSessionResponse.k_EAuthSessionResponseAuthTicketInvalidAlreadyUsed:
				text = "Duplicate ticket returned";
				Debug.Log("[STEAM] " + text);
				Authenticator.userAuthenticated = false;
				break;
			case EAuthSessionResponse.k_EAuthSessionResponseAuthTicketInvalid:
				text = "Ticket returned was invalid";
				Debug.Log("[STEAM] " + text);
				Authenticator.userAuthenticated = false;
				break;
			case EAuthSessionResponse.k_EAuthSessionResponsePublisherIssuedBan:
				text = "VAC banned by publisher";
				Debug.Log("[STEAM] " + text);
				Authenticator.userAuthenticated = false;
				break;
			default:
				text = "Connection to server timed out";
				Debug.Log("[STEAM] " + text);
				Authenticator.userAuthenticated = false;
				break;
			}
			if (Authenticator.userAuthenticated)
			{
				Singleton<AuthEvents>.Instance.onAuthenticationSuccessful.Invoke(text);
			}
			else
			{
				Singleton<AuthEvents>.Instance.onAuthenticationFailed.Invoke(text);
			}
			Authenticator.Logout(ticket);
		}))
		{
		case EBeginAuthSessionResult.k_EBeginAuthSessionResultOK:
			Debug.Log("[STEAM] Authentication sent successfully, awaiting callback.");
			return;
		case EBeginAuthSessionResult.k_EBeginAuthSessionResultInvalidTicket:
			Debug.Log("[STEAM] Invalid ticket sent, no response given.");
			return;
		case EBeginAuthSessionResult.k_EBeginAuthSessionResultDuplicateRequest:
			Debug.Log("[STEAM] Duplicate ticket sent, no response given.");
			return;
		case EBeginAuthSessionResult.k_EBeginAuthSessionResultInvalidVersion:
			Debug.Log("[STEAM] Invalid version number, no response given.");
			return;
		case EBeginAuthSessionResult.k_EBeginAuthSessionResultGameMismatch:
			Debug.Log("[STEAM] Game version mismatch, no response given.");
			return;
		case EBeginAuthSessionResult.k_EBeginAuthSessionResultExpiredTicket:
			Debug.Log("[STEAM] Ticket expired, no response given.");
			return;
		default:
			return;
		}
	}

	// Token: 0x04000253 RID: 595
	public static List<ulong> BANNED_IDS = new List<ulong>();

	// Token: 0x04000254 RID: 596
	private static bool userAuthenticated = false;

	// Token: 0x04000255 RID: 597
	private static bool _piratedCopy = false;
}
