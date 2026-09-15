using System;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

// Token: 0x02000758 RID: 1880
public class GameNetworkManager : MonoBehaviour
{
	// Token: 0x1700077B RID: 1915
	// (get) Token: 0x060030C8 RID: 12488 RVA: 0x000E8BDE File Offset: 0x000E6DDE
	// (set) Token: 0x060030C9 RID: 12489 RVA: 0x000E8BE5 File Offset: 0x000E6DE5
	public static GameNetworkManager Singleton { get; private set; }

	// Token: 0x060030CA RID: 12490 RVA: 0x000E8BED File Offset: 0x000E6DED
	private void Awake()
	{
		if (GameNetworkManager.Singleton != null)
		{
			Debug.LogError("More than one instance of GameNetworkManager is found.");
			return;
		}
		GameNetworkManager.Singleton = this;
		global::UnityEngine.Object.DontDestroyOnLoad(GameNetworkManager.Singleton);
	}

	// Token: 0x060030CB RID: 12491 RVA: 0x000E8C18 File Offset: 0x000E6E18
	private void Start()
	{
		if (GameNetworkManager.Singleton != this)
		{
			Debug.LogError("It seems it's a duplicate. This shouldn't happen.");
			return;
		}
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
		if (ConnectionNotificationManager.Singleton != null)
		{
			ConnectionNotificationManager.Singleton.OnClientConnectionNotification += this.NotifyConnectionStatus;
		}
		this.isManagerInitedCorrectly = true;
	}

	// Token: 0x060030CC RID: 12492 RVA: 0x000E8C98 File Offset: 0x000E6E98
	private void NotifyConnectionStatus(ulong clientId, ConnectionNotificationManager.ConnectionStatus status)
	{
		Debug.Log(string.Format("Client {0} has {1}", clientId, status));
	}

	// Token: 0x060030CD RID: 12493 RVA: 0x000E8CB5 File Offset: 0x000E6EB5
	public bool StartHostGame(string ip, ushort port)
	{
		if (!this.isManagerInitedCorrectly)
		{
			return false;
		}
		this.networkTransport.SetConnectionData(ip, port, null);
		return NetworkManager.Singleton.StartHost();
	}

	// Token: 0x060030CE RID: 12494 RVA: 0x00002318 File Offset: 0x00000518
	public void DisconnectClient()
	{
	}

	// Token: 0x060030CF RID: 12495 RVA: 0x000E8CD9 File Offset: 0x000E6ED9
	public void StartClient()
	{
		NetworkManager.Singleton.StartClient();
	}

	// Token: 0x060030D0 RID: 12496 RVA: 0x000E8CE6 File Offset: 0x000E6EE6
	public bool ConnectToHost(string ip, ushort port)
	{
		this.networkTransport.SetConnectionData(ip, port, null);
		if (!NetworkManager.Singleton.StartClient())
		{
			Debug.LogError("NetworkManager StartClient failed");
			return false;
		}
		return true;
	}

	// Token: 0x060030D1 RID: 12497 RVA: 0x000E8D0F File Offset: 0x000E6F0F
	public void DisconnectFromHost()
	{
		NetworkManager.Singleton.Shutdown(false);
	}

	// Token: 0x060030D2 RID: 12498 RVA: 0x000E8D1C File Offset: 0x000E6F1C
	public void Editor_ConnectToHost(string ip = "127.0.0.1", ushort port = 8889)
	{
		this.ConnectToHost(ip, port);
	}

	// Token: 0x060030D3 RID: 12499 RVA: 0x000E8D27 File Offset: 0x000E6F27
	public void Editor_DisconnectFromHost()
	{
		this.DisconnectFromHost();
	}

	// Token: 0x060030D4 RID: 12500 RVA: 0x000E8D2F File Offset: 0x000E6F2F
	public void Editor_SendDataToServer(int value)
	{
		NetworkDataSync.Instance.RecvTestDataServerRpc(NetworkManager.Singleton.LocalClient.ClientId, value);
	}

	// Token: 0x060030D5 RID: 12501 RVA: 0x000E8D4B File Offset: 0x000E6F4B
	public void Editor_SendDataToClient(int value)
	{
		NetworkDataSync.Instance.RecvTestDataClientRpc(value);
	}

	// Token: 0x0400275F RID: 10079
	[SerializeField]
	private UnityTransport networkTransport;

	// Token: 0x04002761 RID: 10081
	private bool isManagerInitedCorrectly;
}
