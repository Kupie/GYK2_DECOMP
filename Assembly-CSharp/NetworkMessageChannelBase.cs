using System;

// Token: 0x0200075F RID: 1887
public abstract class NetworkMessageChannelBase
{
	// Token: 0x0600310A RID: 12554
	public abstract void OnReceive(ulong senderClientId, IDisposable messagePayload);

	// Token: 0x0600310B RID: 12555 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void Register()
	{
	}

	// Token: 0x0600310C RID: 12556 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void Unregister()
	{
	}
}
