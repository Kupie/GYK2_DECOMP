using System;

namespace LazyBearTechnology
{
	// Token: 0x02000119 RID: 281
	public class PlayerInfo
	{
		// Token: 0x060005BC RID: 1468 RVA: 0x0001D366 File Offset: 0x0001B566
		public PlayerInfo()
		{
			this.isEmpty = true;
		}

		// Token: 0x040002A8 RID: 680
		public bool isEmpty;

		// Token: 0x040002A9 RID: 681
		public ulong localId;

		// Token: 0x040002AA RID: 682
		public string userName;

		// Token: 0x040002AB RID: 683
		public byte[] imageData;
	}
}
