using System;

// Token: 0x0200074D RID: 1869
public class ConnectedToHostState : OnlineState
{
	// Token: 0x06003090 RID: 12432 RVA: 0x000E84A3 File Offset: 0x000E66A3
	public ConnectedToHostState(ConnectionManager connectionManager, NetworkPackageSystem networkPackageSystem)
		: base(connectionManager, networkPackageSystem)
	{
	}

	// Token: 0x06003091 RID: 12433 RVA: 0x000E84AD File Offset: 0x000E66AD
	public override void Enter()
	{
		base.Enter();
		base.AddDestinationClient(LazyNetwork.NetworkManager.ServerClientId);
	}

	// Token: 0x06003092 RID: 12434 RVA: 0x000E84C5 File Offset: 0x000E66C5
	public override void Exit()
	{
		base.Exit();
		base.RemoveDestinationClient(LazyNetwork.NetworkManager.ServerClientId);
	}

	// Token: 0x06003093 RID: 12435 RVA: 0x000E84DD File Offset: 0x000E66DD
	protected override void ExecuteCommand(ICommand command, ulong senderClientId)
	{
		command.Execute(senderClientId, MainGame.Instance.GameSave);
	}

	// Token: 0x06003094 RID: 12436 RVA: 0x000E84F0 File Offset: 0x000E66F0
	protected override void ExecutePackage(CommandPackage package, ulong senderClientId)
	{
		this.networkPackageSystem.ExecutePackageForClient(package, senderClientId, this);
	}
}
