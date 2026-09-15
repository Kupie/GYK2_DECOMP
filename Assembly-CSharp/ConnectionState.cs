using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200074E RID: 1870
public abstract class ConnectionState
{
	// Token: 0x17000775 RID: 1909
	// (get) Token: 0x06003095 RID: 12437 RVA: 0x000E8500 File Offset: 0x000E6700
	public List<ulong> DestinationClients
	{
		get
		{
			return this.destinationClients;
		}
	}

	// Token: 0x06003096 RID: 12438 RVA: 0x000E8508 File Offset: 0x000E6708
	public ConnectionState(ConnectionManager connectionManager)
	{
		this.connectionManager = connectionManager;
	}

	// Token: 0x06003097 RID: 12439
	public abstract void SendNetworkData<T>(T data);

	// Token: 0x06003098 RID: 12440
	public abstract void ReceiveNetworkData<T>(T data, ulong senderClientId);

	// Token: 0x06003099 RID: 12441
	public abstract void Enter();

	// Token: 0x0600309A RID: 12442
	public abstract void Update();

	// Token: 0x0600309B RID: 12443
	public abstract void Exit();

	// Token: 0x0600309C RID: 12444 RVA: 0x000E8524 File Offset: 0x000E6724
	protected void AddDestinationClient(ulong clientId)
	{
		if (this.destinationClients.Contains(clientId))
		{
			Debug.LogWarning(string.Format("Client [{0}] was already added", clientId));
			return;
		}
		this.destinationClients.Add(clientId);
		Debug.Log(string.Format("Destination client {0} was added", clientId));
	}

	// Token: 0x0600309D RID: 12445 RVA: 0x000E8576 File Offset: 0x000E6776
	protected void RemoveDestinationClient(ulong clientId)
	{
		this.destinationClients.Remove(clientId);
		Debug.Log(string.Format("Destination client {0} was removed", clientId));
	}

	// Token: 0x04002752 RID: 10066
	protected ConnectionManager connectionManager;

	// Token: 0x04002753 RID: 10067
	protected List<ulong> destinationClients = new List<ulong>();
}
