using System;

// Token: 0x0200074F RID: 1871
public class OfflineState : ConnectionState
{
	// Token: 0x0600309E RID: 12446 RVA: 0x000E859A File Offset: 0x000E679A
	public OfflineState(ConnectionManager connectionManager)
		: base(connectionManager)
	{
	}

	// Token: 0x0600309F RID: 12447 RVA: 0x00002318 File Offset: 0x00000518
	public override void SendNetworkData<T>(T data)
	{
	}

	// Token: 0x060030A0 RID: 12448 RVA: 0x00002318 File Offset: 0x00000518
	public override void ReceiveNetworkData<T>(T data, ulong senderClientId)
	{
	}

	// Token: 0x060030A1 RID: 12449 RVA: 0x00002318 File Offset: 0x00000518
	public override void Enter()
	{
	}

	// Token: 0x060030A2 RID: 12450 RVA: 0x00002318 File Offset: 0x00000518
	public override void Update()
	{
	}

	// Token: 0x060030A3 RID: 12451 RVA: 0x00002318 File Offset: 0x00000518
	public override void Exit()
	{
	}
}
