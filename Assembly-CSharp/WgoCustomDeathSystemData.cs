using System;
using System.Collections.Generic;

// Token: 0x02000441 RID: 1089
[Serializable]
public class WgoCustomDeathSystemData
{
	// Token: 0x06001CB8 RID: 7352 RVA: 0x00086677 File Offset: 0x00084877
	public void AddCustomDeath(WgoData wgoData)
	{
		this.wgoCustomDeathData.Add(new WgoCustomDeathSystemData.WgoCustomDeathData
		{
			wgoUniqueId = wgoData.UniqueId,
			remainingTime = wgoData.Definition.customDeathTime
		});
	}

	// Token: 0x04001AC5 RID: 6853
	public List<WgoCustomDeathSystemData.WgoCustomDeathData> wgoCustomDeathData = new List<WgoCustomDeathSystemData.WgoCustomDeathData>();

	// Token: 0x02000442 RID: 1090
	[Serializable]
	public class WgoCustomDeathData
	{
		// Token: 0x04001AC6 RID: 6854
		public SGuid wgoUniqueId;

		// Token: 0x04001AC7 RID: 6855
		public float remainingTime;
	}
}
