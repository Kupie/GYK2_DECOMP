using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Cysharp.Threading.Tasks;
using LazyBearTechnology;
using LinqTools;
using Unity.Netcode;

// Token: 0x02000776 RID: 1910
public static class LobbyHelper
{
	// Token: 0x06003191 RID: 12689 RVA: 0x000EAC2F File Offset: 0x000E8E2F
	public static string GetLocalIpAddress()
	{
		IPAddress ipaddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault((IPAddress ip) => ip.AddressFamily == AddressFamily.InterNetwork);
		if (ipaddress == null)
		{
			return null;
		}
		return ipaddress.ToString();
	}

	// Token: 0x06003192 RID: 12690 RVA: 0x000EAC70 File Offset: 0x000E8E70
	public static void Host_Init()
	{
		GameSave gameSave = new GameSave();
		GameSave.SetupNewGameSave(gameSave);
		NetworkPlayer networkPlayer = new NetworkPlayer((int)NetworkManager.Singleton.LocalClient.ClientId, gameSave.playerData);
		MainGame.Instance.PrepareGameForNetwork(gameSave, networkPlayer, true);
		NetworkManager.Singleton.OnClientConnectedCallback += LobbyHelper.Host_OnClientConnected;
	}

	// Token: 0x06003193 RID: 12691 RVA: 0x000EACC8 File Offset: 0x000E8EC8
	public static void Host_UpdateClientSyncStatus(ulong clientId)
	{
		LobbyHelper.connectedClients[clientId] = true;
		Action<ulong> onClientSynced = LobbyHelper.OnClientSynced;
		if (onClientSynced != null)
		{
			onClientSynced(clientId);
		}
		if (LobbyHelper.Host_AreAllClientsSynced())
		{
			Action onAllClientsSynced = LobbyHelper.OnAllClientsSynced;
			if (onAllClientsSynced == null)
			{
				return;
			}
			onAllClientsSynced();
		}
	}

	// Token: 0x06003194 RID: 12692 RVA: 0x000EAD00 File Offset: 0x000E8F00
	public static void Host_StartGame()
	{
		MainGame.Instance.LoadGameScene(false).Forget();
		foreach (ulong num in LobbyHelper.connectedClients.Keys)
		{
			OrderMessage orderMessage = new OrderMessage(OrderMessageType.ForceStartGame);
			LazyNetwork.NetworkMessageChannelManager.Publish<OrderMessage>(orderMessage, num);
		}
	}

	// Token: 0x06003195 RID: 12693 RVA: 0x000EAD74 File Offset: 0x000E8F74
	public static void Host_SyncGameSaves()
	{
		List<ulong> list = new List<ulong>();
		foreach (ulong num in LobbyHelper.connectedClients.Keys)
		{
			list.Add(num);
			LazyNetwork.NetworkMessageChannelManager.Publish<GameSave>(MainGame.Instance.GameSave, num);
		}
		list.ForEach(delegate(ulong index)
		{
			LobbyHelper.connectedClients[index] = false;
		});
	}

	// Token: 0x06003196 RID: 12694 RVA: 0x000EAE0C File Offset: 0x000E900C
	private static void Host_OnClientConnected(ulong clientId)
	{
		NetworkPlayer networkPlayer = MainGame.Instance.GameSave.CreateClient((int)clientId);
		MainGame.Instance.SpawnPlayer(networkPlayer);
		LobbyHelper.connectedClients.Add(clientId, false);
		Action<ulong> onClientAdded = LobbyHelper.OnClientAdded;
		if (onClientAdded == null)
		{
			return;
		}
		onClientAdded(clientId);
	}

	// Token: 0x06003197 RID: 12695 RVA: 0x000EAE54 File Offset: 0x000E9054
	private static bool Host_AreAllClientsSynced()
	{
		foreach (KeyValuePair<ulong, bool> keyValuePair in LobbyHelper.connectedClients)
		{
			if (!keyValuePair.Value)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06003198 RID: 12696 RVA: 0x000EAEB0 File Offset: 0x000E90B0
	public static void Client_InitGameSave(GameSave gameSave)
	{
		MainGame.Instance.SetGameSave(gameSave);
		OrderMessage orderMessage = new OrderMessage(OrderMessageType.ConfirmGameSave);
		LazyNetwork.NetworkMessageChannelManager.Publish<OrderMessage>(orderMessage, 0UL);
	}

	// Token: 0x06003199 RID: 12697 RVA: 0x000EAEDC File Offset: 0x000E90DC
	public static void Client_StartGame()
	{
		LazyUI.GetWindow<UILobbyWindow>().Close();
		int num = (int)NetworkManager.Singleton.LocalClient.ClientId;
		NetworkPlayer networkPlayer;
		if (MainGame.Instance.GameSave.GetClient(num, out networkPlayer, false))
		{
			MainGame.Instance.PrepareGameForNetwork(MainGame.Instance.GameSave, networkPlayer, false);
			MainGame.Instance.LoadGameScene(false).Forget();
		}
	}

	// Token: 0x0400279E RID: 10142
	public static Action OnAllClientsSynced;

	// Token: 0x0400279F RID: 10143
	public static Action<ulong> OnClientAdded;

	// Token: 0x040027A0 RID: 10144
	public static Action<ulong> OnClientSynced;

	// Token: 0x040027A1 RID: 10145
	private static Dictionary<ulong, bool> connectedClients = new Dictionary<ulong, bool>();
}
