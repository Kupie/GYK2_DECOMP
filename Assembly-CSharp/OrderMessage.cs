using System;

// Token: 0x0200075D RID: 1885
[Serializable]
public class OrderMessage
{
	// Token: 0x06003102 RID: 12546 RVA: 0x00021B94 File Offset: 0x0001FD94
	public OrderMessage()
	{
	}

	// Token: 0x06003103 RID: 12547 RVA: 0x000E8FCE File Offset: 0x000E71CE
	public OrderMessage(OrderMessageType messageType)
	{
		this.messageType = messageType;
	}

	// Token: 0x06003104 RID: 12548 RVA: 0x000E8FE0 File Offset: 0x000E71E0
	public void Execute(ulong senderClientId)
	{
		switch (this.messageType)
		{
		case OrderMessageType.RequestPackages:
			LazyNetwork.ConnectionManager.NetworkPackageSystem.SendMissingPackages(senderClientId, (ulong)((long)this.lastPackageIdReceived));
			return;
		case OrderMessageType.ForceStartGame:
			LobbyHelper.Client_StartGame();
			return;
		case OrderMessageType.ConfirmGameSave:
			LobbyHelper.Host_UpdateClientSyncStatus(senderClientId);
			return;
		default:
			return;
		}
	}

	// Token: 0x0400276B RID: 10091
	public OrderMessageType messageType;

	// Token: 0x0400276C RID: 10092
	public int lastPackageIdReceived;
}
