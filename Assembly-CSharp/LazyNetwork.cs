using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200075A RID: 1882
public class LazyNetwork : LazySingleton<LazyNetwork>
{
	// Token: 0x17000784 RID: 1924
	// (get) Token: 0x060030EF RID: 12527 RVA: 0x000E8D58 File Offset: 0x000E6F58
	public static INetworkManager NetworkManager
	{
		get
		{
			return LazySingleton<LazyNetwork>.Instance.networkManager;
		}
	}

	// Token: 0x17000785 RID: 1925
	// (get) Token: 0x060030F0 RID: 12528 RVA: 0x000E8D64 File Offset: 0x000E6F64
	public static BaseNetworkMessageChannelManager NetworkMessageChannelManager
	{
		get
		{
			return LazySingleton<LazyNetwork>.Instance.networkMessageChannelManager;
		}
	}

	// Token: 0x17000786 RID: 1926
	// (get) Token: 0x060030F1 RID: 12529 RVA: 0x000E8D70 File Offset: 0x000E6F70
	public static ConnectionManager ConnectionManager
	{
		get
		{
			return LazySingleton<LazyNetwork>.Instance.connectionManager;
		}
	}

	// Token: 0x17000787 RID: 1927
	// (get) Token: 0x060030F2 RID: 12530 RVA: 0x000E8D7C File Offset: 0x000E6F7C
	public static bool IsInitialized
	{
		get
		{
			return Application.isPlaying && LazyNetwork.isInitialized;
		}
	}

	// Token: 0x060030F3 RID: 12531 RVA: 0x000E8D8C File Offset: 0x000E6F8C
	protected override void Awake()
	{
		base.Awake();
		global::UnityEngine.Object.DontDestroyOnLoad(this);
	}

	// Token: 0x060030F4 RID: 12532 RVA: 0x000E8D9A File Offset: 0x000E6F9A
	public void Init(INetworkManager networkManager, BaseNetworkMessageChannelManager networkMessageChannelManager)
	{
		if (LazyNetwork.isInitialized)
		{
			return;
		}
		this.connectionManager = new ConnectionManager();
		this.networkManager = networkManager;
		this.networkMessageChannelManager = networkMessageChannelManager;
		this.InitComponents();
		LazyNetwork.isInitialized = true;
	}

	// Token: 0x060030F5 RID: 12533 RVA: 0x000E8DC9 File Offset: 0x000E6FC9
	private void InitComponents()
	{
		LazyNetwork.NetworkManager.Init();
		LazyNetwork.NetworkMessageChannelManager.Init();
		LazyNetwork.ConnectionManager.Init();
	}

	// Token: 0x060030F6 RID: 12534 RVA: 0x000E8DE9 File Offset: 0x000E6FE9
	private void Update()
	{
		if (!LazyNetwork.isInitialized)
		{
			return;
		}
		LazyNetwork.ConnectionManager.Update();
	}

	// Token: 0x060030F7 RID: 12535 RVA: 0x000E8DFD File Offset: 0x000E6FFD
	private void OnDestroy()
	{
		if (!LazyNetwork.isInitialized)
		{
			return;
		}
		LazyNetwork.ConnectionManager.DeInit();
		LazyNetwork.NetworkMessageChannelManager.DeInit();
		LazyNetwork.NetworkManager.DeInit();
	}

	// Token: 0x04002762 RID: 10082
	private static bool isInitialized;

	// Token: 0x04002763 RID: 10083
	private BaseNetworkMessageChannelManager networkMessageChannelManager;

	// Token: 0x04002764 RID: 10084
	private INetworkManager networkManager;

	// Token: 0x04002765 RID: 10085
	private ConnectionManager connectionManager;
}
