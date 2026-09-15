using System;

// Token: 0x02000751 RID: 1873
public class StartedHostState : OnlineState
{
	// Token: 0x060030AE RID: 12462 RVA: 0x000E84A3 File Offset: 0x000E66A3
	public StartedHostState(ConnectionManager connectionManager, NetworkPackageSystem networkPackageSystem)
		: base(connectionManager, networkPackageSystem)
	{
	}

	// Token: 0x060030AF RID: 12463 RVA: 0x000E87D0 File Offset: 0x000E69D0
	public override void Enter()
	{
		base.Enter();
		LazyNetwork.NetworkManager.OnClientConnected += this.OnClientConnected;
		LazyNetwork.NetworkManager.OnClientDisconnected += this.OnClientDisconnected;
	}

	// Token: 0x060030B0 RID: 12464 RVA: 0x000E8804 File Offset: 0x000E6A04
	public override void Exit()
	{
		base.Exit();
		LazyNetwork.NetworkManager.OnClientConnected -= this.OnClientConnected;
		LazyNetwork.NetworkManager.OnClientDisconnected -= this.OnClientDisconnected;
	}

	// Token: 0x060030B1 RID: 12465 RVA: 0x000E8838 File Offset: 0x000E6A38
	public override void SendNetworkData<T>(T data)
	{
		Command command = data as Command;
		if (command != null)
		{
			command.Execute(LazyNetwork.NetworkManager.MyId, MainGame.Instance.GameSave);
		}
		base.SendNetworkData<T>(data);
	}

	// Token: 0x060030B2 RID: 12466 RVA: 0x000E8875 File Offset: 0x000E6A75
	protected override void ExecuteCommand(ICommand command, ulong senderClientId)
	{
		command.Execute(senderClientId, MainGame.Instance.GameSave);
		this.SendNetworkData<StartedHostState>(this);
	}

	// Token: 0x060030B3 RID: 12467 RVA: 0x000E888F File Offset: 0x000E6A8F
	protected override void ExecutePackage(CommandPackage package, ulong senderClientId)
	{
		this.networkPackageSystem.ExecutePackageForHost(package, senderClientId, this);
	}

	// Token: 0x060030B4 RID: 12468 RVA: 0x000E889F File Offset: 0x000E6A9F
	private void OnClientConnected(ulong id)
	{
		base.AddDestinationClient(id);
	}

	// Token: 0x060030B5 RID: 12469 RVA: 0x000E88A8 File Offset: 0x000E6AA8
	private void OnClientDisconnected(ulong id)
	{
		base.RemoveDestinationClient(id);
	}
}
