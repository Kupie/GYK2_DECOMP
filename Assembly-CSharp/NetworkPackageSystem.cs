using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000766 RID: 1894
public class NetworkPackageSystem
{
	// Token: 0x140000A9 RID: 169
	// (add) Token: 0x0600312F RID: 12591 RVA: 0x000E974C File Offset: 0x000E794C
	// (remove) Token: 0x06003130 RID: 12592 RVA: 0x000E9784 File Offset: 0x000E7984
	public event Action<ulong> OnPackageRetrievalFailed;

	// Token: 0x1700078C RID: 1932
	// (get) Token: 0x06003131 RID: 12593 RVA: 0x000E97B9 File Offset: 0x000E79B9
	// (set) Token: 0x06003132 RID: 12594 RVA: 0x000E97C1 File Offset: 0x000E79C1
	public int PackagesCountSentThisTick { get; set; }

	// Token: 0x1700078D RID: 1933
	// (get) Token: 0x06003133 RID: 12595 RVA: 0x000E97CA File Offset: 0x000E79CA
	// (set) Token: 0x06003134 RID: 12596 RVA: 0x000E97D2 File Offset: 0x000E79D2
	public int PackagesCountReceivedThisTick { get; set; }

	// Token: 0x06003135 RID: 12597 RVA: 0x000E97DB File Offset: 0x000E79DB
	public NetworkPackageSystem(int packageQueueSize)
	{
		this.packages = new PackageList<CommandPackage>(packageQueueSize);
		this.lastCreatedPackageId = 0UL;
		this.lastReceivedPackageId = 0UL;
	}

	// Token: 0x06003136 RID: 12598 RVA: 0x000E9800 File Offset: 0x000E7A00
	public void CreateCommandPackage(List<Command> commands)
	{
		Queue<Command> queue = new Queue<Command>();
		for (int i = 0; i < commands.Count; i++)
		{
			queue.Enqueue(commands[i]);
		}
		while (queue.Count > 0)
		{
			CommandPackage newCommandPackage = this.GetNewCommandPackage();
			while (queue.Count > 0 && newCommandPackage.TryAddCommand(queue.Peek()))
			{
				queue.Dequeue();
			}
			if (!this.packages.TryAdd(newCommandPackage))
			{
				if (LazyNetwork.NetworkManager.IsHost)
				{
					throw new Exception("Network package queue is maxed out. Consider increasing queue size in [UNetworkManager] or optimize payloads");
				}
				break;
			}
		}
	}

	// Token: 0x06003137 RID: 12599 RVA: 0x000E9888 File Offset: 0x000E7A88
	public void TrySendRegularPackage()
	{
		CommandPackage commandPackage;
		if (this.packages.TryGetForSending(out commandPackage))
		{
			LazyNetwork.NetworkMessageChannelManager.Publish<CommandPackage>(commandPackage, 0UL);
			int packagesCountSentThisTick = this.PackagesCountSentThisTick;
			this.PackagesCountSentThisTick = packagesCountSentThisTick + 1;
		}
	}

	// Token: 0x06003138 RID: 12600 RVA: 0x000E98C4 File Offset: 0x000E7AC4
	public void SendMissingPackages(ulong clientId, ulong lastReceivedPackageId)
	{
		ulong num = lastReceivedPackageId + 1UL;
		ulong packageId = this.packages.Last<CommandPackage>().packageId;
		while (num <= packageId)
		{
			CommandPackage commandPackage;
			if (!this.packages.TryGetById(num, out commandPackage))
			{
				Action<ulong> onPackageRetrievalFailed = this.OnPackageRetrievalFailed;
				if (onPackageRetrievalFailed == null)
				{
					return;
				}
				onPackageRetrievalFailed(clientId);
				return;
			}
			else
			{
				LazyNetwork.NetworkMessageChannelManager.Publish<CommandPackage>(commandPackage, 0UL);
				int packagesCountSentThisTick = this.PackagesCountSentThisTick;
				this.PackagesCountSentThisTick = packagesCountSentThisTick + 1;
				num += 1UL;
				Debug.Log(string.Format("Missing Package #{0} was sent to client [{1}]", commandPackage.packageId, clientId));
			}
		}
	}

	// Token: 0x06003139 RID: 12601 RVA: 0x000E9954 File Offset: 0x000E7B54
	public void ExecutePackageForHost(CommandPackage package, ulong senderClientId, StartedHostState currentState)
	{
		for (int i = 0; i < package.serializedCommands.Count; i++)
		{
			ICommand command = CommandFactory.DeserializeCommand(package.serializedCommands[i]);
			currentState.ReceiveNetworkData<ICommand>(command, senderClientId);
		}
		int packagesCountReceivedThisTick = this.PackagesCountReceivedThisTick;
		this.PackagesCountReceivedThisTick = packagesCountReceivedThisTick + 1;
	}

	// Token: 0x0600313A RID: 12602 RVA: 0x000E99A4 File Offset: 0x000E7BA4
	public void ExecutePackageForClient(CommandPackage package, ulong senderClientId, ConnectedToHostState currentState)
	{
		if (!this.IsPackageQueueConsistent(package.packageId))
		{
			OrderMessage orderMessage = new OrderMessage(OrderMessageType.RequestPackages)
			{
				lastPackageIdReceived = (int)this.lastCreatedPackageId
			};
			LazyNetwork.NetworkMessageChannelManager.Publish<OrderMessage>(orderMessage, 0UL);
			Debug.LogWarning(string.Format("Client missed one or more packages from server. Last received = Package #{0}.", this.lastReceivedPackageId) + "Requesting missing packages from server...");
			return;
		}
		this.lastReceivedPackageId = package.packageId;
		foreach (byte[] array in package.serializedCommands)
		{
			ICommand command = CommandFactory.DeserializeCommand(array);
			currentState.ReceiveNetworkData<ICommand>(command, senderClientId);
		}
		int packagesCountReceivedThisTick = this.PackagesCountReceivedThisTick;
		this.PackagesCountReceivedThisTick = packagesCountReceivedThisTick + 1;
	}

	// Token: 0x0600313B RID: 12603 RVA: 0x000E9A70 File Offset: 0x000E7C70
	public void Debug_ChangeLastReceivedPackageId(ulong value)
	{
		this.lastReceivedPackageId = value;
	}

	// Token: 0x0600313C RID: 12604 RVA: 0x000E9A79 File Offset: 0x000E7C79
	private bool IsPackageQueueConsistent(ulong packageId)
	{
		return (this.lastReceivedPackageId < packageId && packageId - this.lastReceivedPackageId == 1UL) || this.lastReceivedPackageId >= packageId || this.lastReceivedPackageId == 0UL;
	}

	// Token: 0x0600313D RID: 12605 RVA: 0x000E9AA8 File Offset: 0x000E7CA8
	private CommandPackage GetNewCommandPackage()
	{
		ulong num = this.lastCreatedPackageId + 1UL;
		this.lastCreatedPackageId = num;
		return new CommandPackage(num);
	}

	// Token: 0x0400277B RID: 10107
	private PackageList<CommandPackage> packages;

	// Token: 0x0400277C RID: 10108
	private ulong lastCreatedPackageId;

	// Token: 0x0400277D RID: 10109
	private ulong lastReceivedPackageId;
}
