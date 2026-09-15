using System;
using Unity.Netcode;

// Token: 0x0200076B RID: 1899
public abstract class UNetworkMessageChannel<T> : NetworkMessageChannel<T>
{
	// Token: 0x0600314F RID: 12623 RVA: 0x000EA044 File Offset: 0x000E8244
	public UNetworkMessageChannel()
	{
		this.name = base.GetType().Name;
	}

	// Token: 0x06003150 RID: 12624 RVA: 0x000EA064 File Offset: 0x000E8264
	protected void SendMessage(FastBufferWriter writer, ulong customClientId)
	{
		if (!LazyNetwork.NetworkManager.IsHost)
		{
			NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage(this.name, LazyNetwork.ConnectionManager.CurrentState.DestinationClients[0], writer, this.networkDelivery);
			return;
		}
		if (customClientId == 0UL)
		{
			NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage(this.name, LazyNetwork.ConnectionManager.CurrentState.DestinationClients, writer, this.networkDelivery);
			return;
		}
		NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage(this.name, customClientId, writer, this.networkDelivery);
	}

	// Token: 0x06003151 RID: 12625 RVA: 0x000EA0FB File Offset: 0x000E82FB
	public override void Register()
	{
		NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler(this.name, new CustomMessagingManager.HandleNamedMessageDelegate(this.OnReceiveInternal));
	}

	// Token: 0x06003152 RID: 12626 RVA: 0x000EA11E File Offset: 0x000E831E
	public override void Unregister()
	{
		NetworkManager.Singleton.CustomMessagingManager.UnregisterNamedMessageHandler(this.name);
	}

	// Token: 0x06003153 RID: 12627 RVA: 0x000EA135 File Offset: 0x000E8335
	public void SetNetworkDelivery(NetworkDelivery networkDelivery)
	{
		this.networkDelivery = networkDelivery;
	}

	// Token: 0x06003154 RID: 12628 RVA: 0x000EA13E File Offset: 0x000E833E
	private void OnReceiveInternal(ulong senderClientId, FastBufferReader messagePayload)
	{
		this.OnReceive(senderClientId, messagePayload);
	}

	// Token: 0x04002784 RID: 10116
	protected NetworkDelivery networkDelivery = NetworkDelivery.ReliableSequenced;

	// Token: 0x04002785 RID: 10117
	protected string name;
}
