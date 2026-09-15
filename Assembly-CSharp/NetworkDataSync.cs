using System;
using Unity.Netcode;
using UnityEngine;

// Token: 0x02000760 RID: 1888
public class NetworkDataSync : NetworkBehaviour
{
	// Token: 0x17000788 RID: 1928
	// (get) Token: 0x0600310E RID: 12558 RVA: 0x000E90B5 File Offset: 0x000E72B5
	// (set) Token: 0x0600310F RID: 12559 RVA: 0x000E90BC File Offset: 0x000E72BC
	public static NetworkDataSync Instance { get; private set; }

	// Token: 0x06003110 RID: 12560 RVA: 0x000E90C4 File Offset: 0x000E72C4
	private void Awake()
	{
		NetworkDataSync.Instance = this;
		global::UnityEngine.Object.DontDestroyOnLoad(this);
	}

	// Token: 0x06003111 RID: 12561 RVA: 0x000E90D2 File Offset: 0x000E72D2
	private void Start()
	{
		NetworkManager.Singleton.AddNetworkPrefab(base.gameObject);
	}

	// Token: 0x06003112 RID: 12562 RVA: 0x000E90E4 File Offset: 0x000E72E4
	[ServerRpc(RequireOwnership = false)]
	public void RecvTestDataServerRpc(ulong clientId, int intVal)
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
		{
			ServerRpcParams serverRpcParams;
			FastBufferWriter fastBufferWriter = base.__beginSendServerRpc(2800608343U, serverRpcParams, RpcDelivery.Reliable);
			BytePacker.WriteValueBitPacked(fastBufferWriter, clientId);
			BytePacker.WriteValueBitPacked(fastBufferWriter, intVal);
			base.__endSendServerRpc(ref fastBufferWriter, 2800608343U, serverRpcParams, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute || (!networkManager.IsServer && !networkManager.IsHost))
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		if (NetworkManager.Singleton.LocalClient.ClientId == clientId)
		{
			return;
		}
		Debug.Log(string.Format("clientId: [{0}]; {1}, {2}: [{3}]", new object[] { clientId, "RecvTestDataServerRpc", "intVal", intVal }));
	}

	// Token: 0x06003113 RID: 12563 RVA: 0x000E9220 File Offset: 0x000E7420
	[ClientRpc]
	public void RecvTestDataClientRpc(int intVal)
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute && (networkManager.IsServer || networkManager.IsHost))
		{
			ClientRpcParams clientRpcParams;
			FastBufferWriter fastBufferWriter = base.__beginSendClientRpc(1606100119U, clientRpcParams, RpcDelivery.Reliable);
			BytePacker.WriteValueBitPacked(fastBufferWriter, intVal);
			base.__endSendClientRpc(ref fastBufferWriter, 1606100119U, clientRpcParams, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute || (!networkManager.IsClient && !networkManager.IsHost))
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		if (base.IsOwner)
		{
			return;
		}
		Debug.Log(string.Format("{0}, {1}: [{2}]", "RecvTestDataClientRpc", "intVal", intVal));
	}

	// Token: 0x06003115 RID: 12565 RVA: 0x000E9338 File Offset: 0x000E7538
	protected override void __initializeVariables()
	{
		base.__initializeVariables();
	}

	// Token: 0x06003116 RID: 12566 RVA: 0x000E9350 File Offset: 0x000E7550
	protected override void __initializeRpcs()
	{
		base.__registerRpc(2800608343U, new NetworkBehaviour.RpcReceiveHandler(NetworkDataSync.__rpc_handler_2800608343), "RecvTestDataServerRpc", RpcInvokePermission.Everyone);
		base.__registerRpc(1606100119U, new NetworkBehaviour.RpcReceiveHandler(NetworkDataSync.__rpc_handler_1606100119), "RecvTestDataClientRpc", RpcInvokePermission.Server);
		base.__initializeRpcs();
	}

	// Token: 0x06003117 RID: 12567 RVA: 0x000E93A8 File Offset: 0x000E75A8
	private static void __rpc_handler_2800608343(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		ulong num;
		ByteUnpacker.ReadValueBitPacked(reader, out num);
		int num2;
		ByteUnpacker.ReadValueBitPacked(reader, out num2);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((NetworkDataSync)target).RecvTestDataServerRpc(num, num2);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x06003118 RID: 12568 RVA: 0x000E941C File Offset: 0x000E761C
	private static void __rpc_handler_1606100119(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		int num;
		ByteUnpacker.ReadValueBitPacked(reader, out num);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((NetworkDataSync)target).RecvTestDataClientRpc(num);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x06003119 RID: 12569 RVA: 0x000E947E File Offset: 0x000E767E
	protected internal override string __getTypeName()
	{
		return "NetworkDataSync";
	}
}
