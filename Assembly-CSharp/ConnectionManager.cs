using System;
using UnityEngine;

// Token: 0x0200074C RID: 1868
public class ConnectionManager
{
	// Token: 0x17000770 RID: 1904
	// (get) Token: 0x0600307C RID: 12412 RVA: 0x000E8276 File Offset: 0x000E6476
	// (set) Token: 0x0600307D RID: 12413 RVA: 0x000E827E File Offset: 0x000E647E
	public OfflineState OfflineState { get; private set; }

	// Token: 0x17000771 RID: 1905
	// (get) Token: 0x0600307E RID: 12414 RVA: 0x000E8287 File Offset: 0x000E6487
	// (set) Token: 0x0600307F RID: 12415 RVA: 0x000E828F File Offset: 0x000E648F
	public StartedHostState StartedHostState { get; private set; }

	// Token: 0x17000772 RID: 1906
	// (get) Token: 0x06003080 RID: 12416 RVA: 0x000E8298 File Offset: 0x000E6498
	// (set) Token: 0x06003081 RID: 12417 RVA: 0x000E82A0 File Offset: 0x000E64A0
	public ConnectedToHostState ConnectedToHostState { get; private set; }

	// Token: 0x17000773 RID: 1907
	// (get) Token: 0x06003082 RID: 12418 RVA: 0x000E82A9 File Offset: 0x000E64A9
	public ConnectionState CurrentState
	{
		get
		{
			return this.currentState;
		}
	}

	// Token: 0x17000774 RID: 1908
	// (get) Token: 0x06003083 RID: 12419 RVA: 0x000E82B1 File Offset: 0x000E64B1
	public NetworkPackageSystem NetworkPackageSystem
	{
		get
		{
			return this.networkPackageSystem;
		}
	}

	// Token: 0x06003084 RID: 12420 RVA: 0x000E82B9 File Offset: 0x000E64B9
	public void Init()
	{
		this.networkPackageSystem = new NetworkPackageSystem(LazyNetwork.NetworkManager.MaxCommandPackageQueue);
		this.InitStates();
		this.SubscribeToNetworkEvents();
	}

	// Token: 0x06003085 RID: 12421 RVA: 0x000E82DC File Offset: 0x000E64DC
	public void Update()
	{
		this.currentState.Update();
	}

	// Token: 0x06003086 RID: 12422 RVA: 0x000E82E9 File Offset: 0x000E64E9
	public void DeInit()
	{
		this.UnsubscribeFromNetworkEvents();
	}

	// Token: 0x06003087 RID: 12423 RVA: 0x000E82F1 File Offset: 0x000E64F1
	private void InitStates()
	{
		this.OfflineState = new OfflineState(this);
		this.StartedHostState = new StartedHostState(this, this.networkPackageSystem);
		this.ConnectedToHostState = new ConnectedToHostState(this, this.networkPackageSystem);
		this.currentState = this.OfflineState;
	}

	// Token: 0x06003088 RID: 12424 RVA: 0x000E8330 File Offset: 0x000E6530
	private void SubscribeToNetworkEvents()
	{
		LazyNetwork.NetworkManager.OnServerStarted += this.OnServerStarted;
		LazyNetwork.NetworkManager.OnServerStopped += this.OnServerStopped;
		LazyNetwork.NetworkManager.OnClientStarted += this.OnClientStarted;
		LazyNetwork.NetworkManager.OnClientStopped += this.OnClientStopped;
	}

	// Token: 0x06003089 RID: 12425 RVA: 0x000E8398 File Offset: 0x000E6598
	private void UnsubscribeFromNetworkEvents()
	{
		LazyNetwork.NetworkManager.OnServerStarted -= this.OnServerStarted;
		LazyNetwork.NetworkManager.OnServerStopped -= this.OnServerStopped;
		LazyNetwork.NetworkManager.OnClientStarted -= this.OnClientStarted;
		LazyNetwork.NetworkManager.OnClientStopped -= this.OnClientStopped;
	}

	// Token: 0x0600308A RID: 12426 RVA: 0x000E8400 File Offset: 0x000E6600
	private void ChangeState(ConnectionState nextState)
	{
		Debug.Log(string.Concat(new string[]
		{
			"Changed connection state from ",
			this.currentState.GetType().Name,
			" to ",
			nextState.GetType().Name,
			"."
		}));
		ConnectionState connectionState = this.currentState;
		if (connectionState != null)
		{
			connectionState.Exit();
		}
		this.currentState = nextState;
		this.currentState.Enter();
	}

	// Token: 0x0600308B RID: 12427 RVA: 0x000E8479 File Offset: 0x000E6679
	private void OnServerStarted()
	{
		this.ChangeState(this.StartedHostState);
	}

	// Token: 0x0600308C RID: 12428 RVA: 0x000E8487 File Offset: 0x000E6687
	private void OnServerStopped()
	{
		this.ChangeState(this.OfflineState);
	}

	// Token: 0x0600308D RID: 12429 RVA: 0x000E8495 File Offset: 0x000E6695
	private void OnClientStarted()
	{
		this.ChangeState(this.ConnectedToHostState);
	}

	// Token: 0x0600308E RID: 12430 RVA: 0x000E8487 File Offset: 0x000E6687
	private void OnClientStopped()
	{
		this.ChangeState(this.OfflineState);
	}

	// Token: 0x0400274D RID: 10061
	private NetworkPackageSystem networkPackageSystem;

	// Token: 0x0400274E RID: 10062
	private ConnectionState currentState;
}
