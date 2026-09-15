using System;
using System.Collections.Generic;

// Token: 0x0200076D RID: 1901
public class UNetworkMessageChannelManager : BaseNetworkMessageChannelManager
{
	// Token: 0x06003158 RID: 12632 RVA: 0x000EA231 File Offset: 0x000E8431
	public override void Init()
	{
		this.SetupMessageChannels(new List<NetworkMessageChannelBase>
		{
			new UCommandMessageChannel(),
			new UCommandPackageMessageChannel(),
			new UOrderMessageChannel(),
			new UGameSaveMessageChannel()
		});
	}

	// Token: 0x06003159 RID: 12633 RVA: 0x00002318 File Offset: 0x00000518
	public override void DeInit()
	{
	}

	// Token: 0x0600315A RID: 12634 RVA: 0x000EA26C File Offset: 0x000E846C
	private void SetupMessageChannels(List<NetworkMessageChannelBase> messageChannelsList)
	{
		foreach (NetworkMessageChannelBase networkMessageChannelBase in messageChannelsList)
		{
			Type genericType = this.GetGenericType(networkMessageChannelBase);
			this.messageChannels.Add(genericType, networkMessageChannelBase);
		}
	}

	// Token: 0x0600315B RID: 12635 RVA: 0x000EA2C8 File Offset: 0x000E84C8
	private Type GetGenericType(NetworkMessageChannelBase messageChannel)
	{
		return messageChannel.GetType().BaseType.GetGenericArguments()[0];
	}

	// Token: 0x04002786 RID: 10118
	private List<NetworkMessageChannelBase> messageChannelsInternal;
}
