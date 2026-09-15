using System;
using System.Collections.Generic;

// Token: 0x02000750 RID: 1872
public abstract class OnlineState : ConnectionState
{
	// Token: 0x060030A4 RID: 12452 RVA: 0x000E85A3 File Offset: 0x000E67A3
	public OnlineState(ConnectionManager connectionManager, NetworkPackageSystem networkPackageSystem)
		: base(connectionManager)
	{
		this.networkPackageSystem = networkPackageSystem;
	}

	// Token: 0x060030A5 RID: 12453
	protected abstract void ExecuteCommand(ICommand command, ulong senderClientId);

	// Token: 0x060030A6 RID: 12454
	protected abstract void ExecutePackage(CommandPackage package, ulong senderClientId);

	// Token: 0x060030A7 RID: 12455 RVA: 0x000E85C9 File Offset: 0x000E67C9
	public override void Enter()
	{
		LazyNetwork.NetworkMessageChannelManager.RegisterMessageChannels();
		LazyNetwork.NetworkMessageChannelManager.AddListener<ICommand>(new Action<ICommand, ulong>(this.ExecuteCommand));
		LazyNetwork.NetworkMessageChannelManager.AddListener<CommandPackage>(new Action<CommandPackage, ulong>(this.ExecutePackage));
	}

	// Token: 0x060030A8 RID: 12456 RVA: 0x000E8603 File Offset: 0x000E6803
	public override void Exit()
	{
		LazyNetwork.NetworkMessageChannelManager.RemoveListener<ICommand>(new Action<ICommand, ulong>(this.ExecuteCommand));
		LazyNetwork.NetworkMessageChannelManager.RemoveListener<CommandPackage>(new Action<CommandPackage, ulong>(this.ExecutePackage));
		LazyNetwork.NetworkMessageChannelManager.UnregisterMessageChannels();
	}

	// Token: 0x060030A9 RID: 12457 RVA: 0x000E8640 File Offset: 0x000E6840
	public override void SendNetworkData<T>(T data)
	{
		Command command = data as Command;
		if (command != null)
		{
			this.AddCommand(command);
			return;
		}
		UniqueCommandHolder uniqueCommandHolder = data as UniqueCommandHolder;
		if (uniqueCommandHolder != null)
		{
			this.AddNotRepeatableCommand(uniqueCommandHolder);
		}
	}

	// Token: 0x060030AA RID: 12458 RVA: 0x000E867C File Offset: 0x000E687C
	public override void ReceiveNetworkData<T>(T data, ulong senderClientId)
	{
		ICommand command = data as ICommand;
		if (command != null)
		{
			this.ExecuteCommand(command, senderClientId);
			return;
		}
		CommandPackage commandPackage = data as CommandPackage;
		if (commandPackage != null)
		{
			this.ExecutePackage(commandPackage, senderClientId);
		}
	}

	// Token: 0x060030AB RID: 12459 RVA: 0x000E86B8 File Offset: 0x000E68B8
	public override void Update()
	{
		foreach (UniqueCommandHolder uniqueCommandHolder in this.notRepeatableCommands)
		{
			LazyNetwork.NetworkMessageChannelManager.Publish<ICommand>(uniqueCommandHolder.GetCommand(), 0UL);
			uniqueCommandHolder.isQueued = false;
		}
		this.notRepeatableCommands.Clear();
		if (this.commands.Count > 0)
		{
			this.networkPackageSystem.CreateCommandPackage(this.commands);
			this.commands.Clear();
			this.networkPackageSystem.TrySendRegularPackage();
		}
	}

	// Token: 0x060030AC RID: 12460 RVA: 0x000E8760 File Offset: 0x000E6960
	private void AddCommand(Command command)
	{
		if (command == null)
		{
			return;
		}
		if (LazyNetwork.NetworkManager.IsHost && LazyNetwork.NetworkManager.OtherClients.Count == 0)
		{
			return;
		}
		this.commands.Add(command);
	}

	// Token: 0x060030AD RID: 12461 RVA: 0x000E8790 File Offset: 0x000E6990
	private void AddNotRepeatableCommand(UniqueCommandHolder commandHolder)
	{
		if (commandHolder == null)
		{
			return;
		}
		if (LazyNetwork.NetworkManager.IsHost && LazyNetwork.NetworkManager.OtherClients.Count == 0)
		{
			return;
		}
		if (commandHolder.isQueued)
		{
			return;
		}
		this.notRepeatableCommands.Add(commandHolder);
		commandHolder.isQueued = true;
	}

	// Token: 0x04002754 RID: 10068
	protected NetworkPackageSystem networkPackageSystem;

	// Token: 0x04002755 RID: 10069
	protected List<Command> commands = new List<Command>();

	// Token: 0x04002756 RID: 10070
	protected List<UniqueCommandHolder> notRepeatableCommands = new List<UniqueCommandHolder>();
}
