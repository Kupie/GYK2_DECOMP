using System;

// Token: 0x0200075E RID: 1886
public abstract class NetworkMessageChannel<T> : NetworkMessageChannelBase
{
	// Token: 0x140000A8 RID: 168
	// (add) Token: 0x06003105 RID: 12549 RVA: 0x000E902C File Offset: 0x000E722C
	// (remove) Token: 0x06003106 RID: 12550 RVA: 0x000E9064 File Offset: 0x000E7264
	public event Action<T, ulong> OnMessageReceive;

	// Token: 0x06003107 RID: 12551
	public abstract void Publish(T data, ulong customClientId = 0UL);

	// Token: 0x06003108 RID: 12552 RVA: 0x000E9099 File Offset: 0x000E7299
	protected void NotifyOnMessageReceive(T type, ulong senderClientId)
	{
		Action<T, ulong> onMessageReceive = this.OnMessageReceive;
		if (onMessageReceive == null)
		{
			return;
		}
		onMessageReceive(type, senderClientId);
	}
}
