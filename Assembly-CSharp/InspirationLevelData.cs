using System;
using System.Collections.Generic;

// Token: 0x020001EF RID: 495
[Serializable]
public class InspirationLevelData
{
	// Token: 0x06000C4E RID: 3150 RVA: 0x0003E4A0 File Offset: 0x0003C6A0
	public InspirationDef GetDataForLevel(int level)
	{
		int num = level - 1;
		if (num < this.levels.Count)
		{
			return this.levels[num];
		}
		return null;
	}

	// Token: 0x04000E02 RID: 3586
	public string id;

	// Token: 0x04000E03 RID: 3587
	public string talentId;

	// Token: 0x04000E04 RID: 3588
	public List<InspirationDef> levels = new List<InspirationDef>();

	// Token: 0x04000E05 RID: 3589
	public List<string> inspirationLocks = new List<string>();

	// Token: 0x04000E06 RID: 3590
	public List<string> techLocks = new List<string>();

	// Token: 0x04000E07 RID: 3591
	public List<string> questLocks = new List<string>();
}
