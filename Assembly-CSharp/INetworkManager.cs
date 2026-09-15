using System;
using System.Collections.Generic;

// Token: 0x02000759 RID: 1881
public interface INetworkManager
{
	// Token: 0x140000A2 RID: 162
	// (add) Token: 0x060030D7 RID: 12503
	// (remove) Token: 0x060030D8 RID: 12504
	event Action OnServerStarted;

	// Token: 0x140000A3 RID: 163
	// (add) Token: 0x060030D9 RID: 12505
	// (remove) Token: 0x060030DA RID: 12506
	event Action OnServerStopped;

	// Token: 0x140000A4 RID: 164
	// (add) Token: 0x060030DB RID: 12507
	// (remove) Token: 0x060030DC RID: 12508
	event Action OnClientStarted;

	// Token: 0x140000A5 RID: 165
	// (add) Token: 0x060030DD RID: 12509
	// (remove) Token: 0x060030DE RID: 12510
	event Action OnClientStopped;

	// Token: 0x140000A6 RID: 166
	// (add) Token: 0x060030DF RID: 12511
	// (remove) Token: 0x060030E0 RID: 12512
	event Action<ulong> OnClientConnected;

	// Token: 0x140000A7 RID: 167
	// (add) Token: 0x060030E1 RID: 12513
	// (remove) Token: 0x060030E2 RID: 12514
	event Action<ulong> OnClientDisconnected;

	// Token: 0x1700077C RID: 1916
	// (get) Token: 0x060030E3 RID: 12515
	bool IsHost { get; }

	// Token: 0x1700077D RID: 1917
	// (get) Token: 0x060030E4 RID: 12516
	bool IsClient { get; }

	// Token: 0x1700077E RID: 1918
	// (get) Token: 0x060030E5 RID: 12517
	ulong MyId { get; }

	// Token: 0x1700077F RID: 1919
	// (get) Token: 0x060030E6 RID: 12518
	bool IsCoopGame { get; }

	// Token: 0x17000780 RID: 1920
	// (get) Token: 0x060030E7 RID: 12519
	int MaxCommandPackageQueue { get; }

	// Token: 0x17000781 RID: 1921
	// (get) Token: 0x060030E8 RID: 12520
	int MaxPayloadSize { get; }

	// Token: 0x17000782 RID: 1922
	// (get) Token: 0x060030E9 RID: 12521
	ulong ServerClientId { get; }

	// Token: 0x17000783 RID: 1923
	// (get) Token: 0x060030EA RID: 12522
	List<ulong> OtherClients { get; }

	// Token: 0x060030EB RID: 12523
	void Init();

	// Token: 0x060030EC RID: 12524
	void DeInit();

	// Token: 0x060030ED RID: 12525
	bool StartHostGame(string ip, ushort port);

	// Token: 0x060030EE RID: 12526
	bool ConnectToHost(string ip, ushort port);
}
