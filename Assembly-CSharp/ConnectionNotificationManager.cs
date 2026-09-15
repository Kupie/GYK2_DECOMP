using System;
using Unity.Netcode;
using UnityEngine;

// Token: 0x02000749 RID: 1865
public class ConnectionNotificationManager : MonoBehaviour
{
	// Token: 0x1700076F RID: 1903
	// (get) Token: 0x06003072 RID: 12402 RVA: 0x000E8115 File Offset: 0x000E6315
	// (set) Token: 0x06003073 RID: 12403 RVA: 0x000E811C File Offset: 0x000E631C
	public static ConnectionNotificationManager Singleton { get; internal set; }

	// Token: 0x140000A1 RID: 161
	// (add) Token: 0x06003074 RID: 12404 RVA: 0x000E8124 File Offset: 0x000E6324
	// (remove) Token: 0x06003075 RID: 12405 RVA: 0x000E815C File Offset: 0x000E635C
	public event Action<ulong, ConnectionNotificationManager.ConnectionStatus> OnClientConnectionNotification;

	// Token: 0x06003076 RID: 12406 RVA: 0x000E8191 File Offset: 0x000E6391
	private void Awake()
	{
		if (ConnectionNotificationManager.Singleton != null)
		{
			throw new Exception("Detected more than one instance of ConnectionNotificationManager! Do you have more than one component attached to a GameObject");
		}
		ConnectionNotificationManager.Singleton = this;
	}

	// Token: 0x06003077 RID: 12407 RVA: 0x000E81B4 File Offset: 0x000E63B4
	private void Start()
	{
		if (ConnectionNotificationManager.Singleton != this)
		{
			return;
		}
		if (NetworkManager.Singleton == null)
		{
			throw new Exception("There is no NetworkManager for the ConnectionNotificationManager to do stuff with! Please add a NetworkManager to the scene.");
		}
		NetworkManager.Singleton.OnClientConnectedCallback += this.OnClientConnectedCallback;
		NetworkManager.Singleton.OnClientDisconnectCallback += this.OnClientDisconnectCallback;
	}

	// Token: 0x06003078 RID: 12408 RVA: 0x000E8213 File Offset: 0x000E6413
	private void OnDestroy()
	{
		if (NetworkManager.Singleton != null)
		{
			NetworkManager.Singleton.OnClientConnectedCallback -= this.OnClientConnectedCallback;
			NetworkManager.Singleton.OnClientDisconnectCallback -= this.OnClientDisconnectCallback;
		}
	}

	// Token: 0x06003079 RID: 12409 RVA: 0x000E824E File Offset: 0x000E644E
	private void OnClientConnectedCallback(ulong clientId)
	{
		Action<ulong, ConnectionNotificationManager.ConnectionStatus> onClientConnectionNotification = this.OnClientConnectionNotification;
		if (onClientConnectionNotification == null)
		{
			return;
		}
		onClientConnectionNotification(clientId, ConnectionNotificationManager.ConnectionStatus.Connected);
	}

	// Token: 0x0600307A RID: 12410 RVA: 0x000E8262 File Offset: 0x000E6462
	private void OnClientDisconnectCallback(ulong clientId)
	{
		Action<ulong, ConnectionNotificationManager.ConnectionStatus> onClientConnectionNotification = this.OnClientConnectionNotification;
		if (onClientConnectionNotification == null)
		{
			return;
		}
		onClientConnectionNotification(clientId, ConnectionNotificationManager.ConnectionStatus.Disconnected);
	}

	// Token: 0x0200074A RID: 1866
	public enum ConnectionStatus
	{
		// Token: 0x0400273F RID: 10047
		Connected,
		// Token: 0x04002740 RID: 10048
		Disconnected
	}
}
