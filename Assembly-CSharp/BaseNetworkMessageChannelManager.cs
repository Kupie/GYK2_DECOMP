using System;
using System.Collections.Generic;

// Token: 0x0200075B RID: 1883
public abstract class BaseNetworkMessageChannelManager
{
	// Token: 0x060030F9 RID: 12537
	public abstract void Init();

	// Token: 0x060030FA RID: 12538
	public abstract void DeInit();

	// Token: 0x060030FB RID: 12539 RVA: 0x000E8E30 File Offset: 0x000E7030
	public void RegisterMessageChannels()
	{
		foreach (NetworkMessageChannelBase networkMessageChannelBase in this.messageChannels.Values)
		{
			networkMessageChannelBase.Register();
		}
	}

	// Token: 0x060030FC RID: 12540 RVA: 0x000E8E88 File Offset: 0x000E7088
	public void UnregisterMessageChannels()
	{
		foreach (NetworkMessageChannelBase networkMessageChannelBase in this.messageChannels.Values)
		{
			networkMessageChannelBase.Unregister();
		}
	}

	// Token: 0x060030FD RID: 12541 RVA: 0x000E8EE0 File Offset: 0x000E70E0
	public void Publish<T>(T data, ulong customClientId = 0UL)
	{
		NetworkMessageChannelBase networkMessageChannelBase;
		if (this.messageChannels.TryGetValue(typeof(T), out networkMessageChannelBase))
		{
			NetworkMessageChannel<T> networkMessageChannel = networkMessageChannelBase as NetworkMessageChannel<T>;
			if (networkMessageChannel != null)
			{
				networkMessageChannel.Publish(data, customClientId);
			}
		}
	}

	// Token: 0x060030FE RID: 12542 RVA: 0x000E8F18 File Offset: 0x000E7118
	public void AddListener<T>(Action<T, ulong> callback)
	{
		NetworkMessageChannelBase networkMessageChannelBase;
		if (this.messageChannels.TryGetValue(typeof(T), out networkMessageChannelBase))
		{
			NetworkMessageChannel<T> networkMessageChannel = networkMessageChannelBase as NetworkMessageChannel<T>;
			if (networkMessageChannel != null)
			{
				networkMessageChannel.OnMessageReceive += callback;
			}
		}
	}

	// Token: 0x060030FF RID: 12543 RVA: 0x000E8F50 File Offset: 0x000E7150
	public void RemoveListener<T>(Action<T, ulong> callback)
	{
		NetworkMessageChannelBase networkMessageChannelBase;
		if (this.messageChannels.TryGetValue(typeof(T), out networkMessageChannelBase))
		{
			NetworkMessageChannel<T> networkMessageChannel = networkMessageChannelBase as NetworkMessageChannel<T>;
			if (networkMessageChannel != null)
			{
				networkMessageChannel.OnMessageReceive -= callback;
			}
		}
	}

	// Token: 0x06003100 RID: 12544 RVA: 0x000E8F88 File Offset: 0x000E7188
	public NetworkMessageChannelBase GetNetworkMessageChannel<T>()
	{
		NetworkMessageChannelBase networkMessageChannelBase;
		if (this.messageChannels.TryGetValue(typeof(T), out networkMessageChannelBase))
		{
			NetworkMessageChannel<T> networkMessageChannel = networkMessageChannelBase as NetworkMessageChannel<T>;
			if (networkMessageChannel != null)
			{
				return networkMessageChannel;
			}
		}
		return null;
	}

	// Token: 0x04002766 RID: 10086
	protected Dictionary<Type, NetworkMessageChannelBase> messageChannels = new Dictionary<Type, NetworkMessageChannelBase>();
}
