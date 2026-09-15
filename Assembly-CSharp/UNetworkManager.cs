using System;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

// Token: 0x0200076E RID: 1902
public class UNetworkManager : MonoBehaviour, INetworkManager
{
	// Token: 0x140000AA RID: 170
	// (add) Token: 0x0600315D RID: 12637 RVA: 0x000EA2E4 File Offset: 0x000E84E4
	// (remove) Token: 0x0600315E RID: 12638 RVA: 0x000EA31C File Offset: 0x000E851C
	public event Action OnServerStarted;

	// Token: 0x140000AB RID: 171
	// (add) Token: 0x0600315F RID: 12639 RVA: 0x000EA354 File Offset: 0x000E8554
	// (remove) Token: 0x06003160 RID: 12640 RVA: 0x000EA38C File Offset: 0x000E858C
	public event Action OnServerStopped;

	// Token: 0x140000AC RID: 172
	// (add) Token: 0x06003161 RID: 12641 RVA: 0x000EA3C4 File Offset: 0x000E85C4
	// (remove) Token: 0x06003162 RID: 12642 RVA: 0x000EA3FC File Offset: 0x000E85FC
	public event Action OnClientStarted;

	// Token: 0x140000AD RID: 173
	// (add) Token: 0x06003163 RID: 12643 RVA: 0x000EA434 File Offset: 0x000E8634
	// (remove) Token: 0x06003164 RID: 12644 RVA: 0x000EA46C File Offset: 0x000E866C
	public event Action OnClientStopped;

	// Token: 0x140000AE RID: 174
	// (add) Token: 0x06003165 RID: 12645 RVA: 0x000EA4A4 File Offset: 0x000E86A4
	// (remove) Token: 0x06003166 RID: 12646 RVA: 0x000EA4DC File Offset: 0x000E86DC
	public event Action<ulong> OnClientConnected;

	// Token: 0x140000AF RID: 175
	// (add) Token: 0x06003167 RID: 12647 RVA: 0x000EA514 File Offset: 0x000E8714
	// (remove) Token: 0x06003168 RID: 12648 RVA: 0x000EA54C File Offset: 0x000E874C
	public event Action<ulong> OnClientDisconnected;

	// Token: 0x1700078F RID: 1935
	// (get) Token: 0x06003169 RID: 12649 RVA: 0x000EA581 File Offset: 0x000E8781
	public bool IsHost
	{
		get
		{
			return !this.IsCoopGame || NetworkManager.Singleton.IsHost;
		}
	}

	// Token: 0x17000790 RID: 1936
	// (get) Token: 0x0600316A RID: 12650 RVA: 0x000EA597 File Offset: 0x000E8797
	public bool IsClient
	{
		get
		{
			return this.IsCoopGame && NetworkManager.Singleton.IsClient;
		}
	}

	// Token: 0x17000791 RID: 1937
	// (get) Token: 0x0600316B RID: 12651 RVA: 0x000EA5AD File Offset: 0x000E87AD
	public ulong MyId
	{
		get
		{
			return NetworkManager.Singleton.LocalClientId;
		}
	}

	// Token: 0x17000792 RID: 1938
	// (get) Token: 0x0600316C RID: 12652 RVA: 0x000EA5B9 File Offset: 0x000E87B9
	public bool IsCoopGame
	{
		get
		{
			return this.isCoopGame;
		}
	}

	// Token: 0x17000793 RID: 1939
	// (get) Token: 0x0600316D RID: 12653 RVA: 0x000EA5C1 File Offset: 0x000E87C1
	public int MaxCommandPackageQueue
	{
		get
		{
			return this.maxCommandPackageQueue;
		}
	}

	// Token: 0x17000794 RID: 1940
	// (get) Token: 0x0600316E RID: 12654 RVA: 0x000EA5C9 File Offset: 0x000E87C9
	public int MaxPayloadSize
	{
		get
		{
			return this.networkTransport.MaxPayloadSize;
		}
	}

	// Token: 0x17000795 RID: 1941
	// (get) Token: 0x0600316F RID: 12655 RVA: 0x000EA5D6 File Offset: 0x000E87D6
	public ulong ServerClientId
	{
		get
		{
			return this.serverClientId;
		}
	}

	// Token: 0x17000796 RID: 1942
	// (get) Token: 0x06003170 RID: 12656 RVA: 0x000EA5DE File Offset: 0x000E87DE
	public List<ulong> OtherClients
	{
		get
		{
			return this.otherClients;
		}
	}

	// Token: 0x06003171 RID: 12657 RVA: 0x000EA5E8 File Offset: 0x000E87E8
	public void Init()
	{
		if (NetworkManager.Singleton == null)
		{
			Debug.LogError("NetworkManager was not initialized.");
			return;
		}
		if (this.networkTransport == null)
		{
			Debug.LogError("Transport protocol was not set.");
			return;
		}
		ConnectionNotificationManager.Singleton.OnClientConnectionNotification += this.NotifyConnectionStatus;
		this.isManagerInitedCorrectly = true;
		NetworkManager.Singleton.OnServerStarted += this.OnServerStarted_Internal;
		NetworkManager.Singleton.OnServerStopped += this.OnServerStopped_Internal;
		NetworkManager.Singleton.OnClientConnectedCallback += this.OnClientStarted_Internal;
		NetworkManager.Singleton.OnClientStopped += this.OnClientStoppedInternal;
		NetworkManager.Singleton.OnClientDisconnectCallback += this.OnClientDisconnectedInternal;
	}

	// Token: 0x06003172 RID: 12658 RVA: 0x000EA6B4 File Offset: 0x000E88B4
	public void DeInit()
	{
		if (NetworkManager.Singleton == null)
		{
			return;
		}
		NetworkManager.Singleton.OnServerStarted -= this.OnServerStarted_Internal;
		NetworkManager.Singleton.OnServerStopped -= this.OnServerStopped_Internal;
		NetworkManager.Singleton.OnClientConnectedCallback -= this.OnClientStarted_Internal;
		NetworkManager.Singleton.OnClientStopped -= this.OnClientStoppedInternal;
		NetworkManager.Singleton.OnClientDisconnectCallback -= this.OnClientDisconnectedInternal;
	}

	// Token: 0x06003173 RID: 12659 RVA: 0x000EA740 File Offset: 0x000E8940
	private void NotifyConnectionStatus(ulong clientId, ConnectionNotificationManager.ConnectionStatus status)
	{
		if (clientId != NetworkManager.Singleton.LocalClient.ClientId)
		{
			if (NetworkManager.Singleton.IsHost)
			{
				if (status != ConnectionNotificationManager.ConnectionStatus.Connected)
				{
					if (status == ConnectionNotificationManager.ConnectionStatus.Disconnected)
					{
						this.otherClients.Remove(clientId);
					}
				}
				else
				{
					this.otherClients.Add(clientId);
				}
			}
			else
			{
				this.serverClientId = clientId;
			}
		}
		Debug.Log(string.Format("Client {0} has {1}", clientId, status));
	}

	// Token: 0x06003174 RID: 12660 RVA: 0x000EA7B3 File Offset: 0x000E89B3
	private void OnServerStarted_Internal()
	{
		Action onServerStarted = this.OnServerStarted;
		if (onServerStarted == null)
		{
			return;
		}
		onServerStarted();
	}

	// Token: 0x06003175 RID: 12661 RVA: 0x000EA7C5 File Offset: 0x000E89C5
	private void OnServerStopped_Internal(bool value)
	{
		Action onServerStopped = this.OnServerStopped;
		if (onServerStopped == null)
		{
			return;
		}
		onServerStopped();
	}

	// Token: 0x06003176 RID: 12662 RVA: 0x000EA7D7 File Offset: 0x000E89D7
	private void OnClientStarted_Internal(ulong id)
	{
		if (!this.IsHost)
		{
			Action onClientStarted = this.OnClientStarted;
			if (onClientStarted == null)
			{
				return;
			}
			onClientStarted();
			return;
		}
		else
		{
			if (id == LazyNetwork.NetworkManager.MyId)
			{
				return;
			}
			Action<ulong> onClientConnected = this.OnClientConnected;
			if (onClientConnected == null)
			{
				return;
			}
			onClientConnected(id);
			return;
		}
	}

	// Token: 0x06003177 RID: 12663 RVA: 0x000EA811 File Offset: 0x000E8A11
	private void OnClientDisconnectedInternal(ulong id)
	{
		if (this.IsHost && id != LazyNetwork.NetworkManager.MyId)
		{
			Action<ulong> onClientDisconnected = this.OnClientDisconnected;
			if (onClientDisconnected == null)
			{
				return;
			}
			onClientDisconnected(id);
		}
	}

	// Token: 0x06003178 RID: 12664 RVA: 0x000EA839 File Offset: 0x000E8A39
	private void OnClientStoppedInternal(bool isHost)
	{
		if (!isHost)
		{
			Action onClientStopped = this.OnClientStopped;
			if (onClientStopped == null)
			{
				return;
			}
			onClientStopped();
		}
	}

	// Token: 0x06003179 RID: 12665 RVA: 0x000EA84E File Offset: 0x000E8A4E
	public bool StartHostGame(string ip, ushort port)
	{
		if (!this.isManagerInitedCorrectly)
		{
			return false;
		}
		this.networkTransport.SetConnectionData(ip, port, null);
		this.isCoopGame = NetworkManager.Singleton.StartHost();
		return this.isCoopGame;
	}

	// Token: 0x0600317A RID: 12666 RVA: 0x00002318 File Offset: 0x00000518
	public void DisconnectClient()
	{
	}

	// Token: 0x0600317B RID: 12667 RVA: 0x000E8CD9 File Offset: 0x000E6ED9
	public void StartClient()
	{
		NetworkManager.Singleton.StartClient();
	}

	// Token: 0x0600317C RID: 12668 RVA: 0x000EA87E File Offset: 0x000E8A7E
	public bool ConnectToHost(string ip, ushort port)
	{
		this.networkTransport.SetConnectionData(ip, port, null);
		this.isCoopGame = NetworkManager.Singleton.StartClient();
		if (!this.isCoopGame)
		{
			Debug.LogError("NetworkManager StartClient failed");
			return false;
		}
		return true;
	}

	// Token: 0x0600317D RID: 12669 RVA: 0x000E8D0F File Offset: 0x000E6F0F
	public void DisconnectFromHost()
	{
		NetworkManager.Singleton.Shutdown(false);
	}

	// Token: 0x0600317E RID: 12670 RVA: 0x000EA8B3 File Offset: 0x000E8AB3
	public void Editor_ConnectToHost(string ip = "127.0.0.1", ushort port = 8889)
	{
		this.ConnectToHost(ip, port);
	}

	// Token: 0x0600317F RID: 12671 RVA: 0x000EA8BE File Offset: 0x000E8ABE
	public void Editor_DisconnectFromHost()
	{
		this.DisconnectFromHost();
	}

	// Token: 0x06003180 RID: 12672 RVA: 0x000E8D2F File Offset: 0x000E6F2F
	public void Editor_SendDataToServer(int value)
	{
		NetworkDataSync.Instance.RecvTestDataServerRpc(NetworkManager.Singleton.LocalClient.ClientId, value);
	}

	// Token: 0x06003181 RID: 12673 RVA: 0x000E8D4B File Offset: 0x000E6F4B
	public void Editor_SendDataToClient(int value)
	{
		NetworkDataSync.Instance.RecvTestDataClientRpc(value);
	}

	// Token: 0x04002787 RID: 10119
	[SerializeField]
	private UnityTransport networkTransport;

	// Token: 0x04002788 RID: 10120
	[SerializeField]
	private int maxCommandPackageQueue = 15;

	// Token: 0x0400278F RID: 10127
	private bool isManagerInitedCorrectly;

	// Token: 0x04002790 RID: 10128
	private ulong serverClientId;

	// Token: 0x04002791 RID: 10129
	private List<ulong> otherClients = new List<ulong>();

	// Token: 0x04002792 RID: 10130
	private bool isCoopGame;
}
