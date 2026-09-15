using System;
using System.Collections.Generic;

// Token: 0x020005B1 RID: 1457
[Serializable]
public class UnlockedColorCustomizationData
{
	// Token: 0x060025A2 RID: 9634 RVA: 0x000B05DF File Offset: 0x000AE7DF
	public UnlockedColorCustomizationData()
	{
	}

	// Token: 0x060025A3 RID: 9635 RVA: 0x000B05FD File Offset: 0x000AE7FD
	public UnlockedColorCustomizationData(PlayerColorCustomizationType type, int partSkinId, List<string> unlockedNames)
	{
		this.type = type;
		this.partSkinId = partSkinId;
		this.unlockedNames = unlockedNames;
	}

	// Token: 0x040020DC RID: 8412
	public PlayerColorCustomizationType type;

	// Token: 0x040020DD RID: 8413
	public int partSkinId;

	// Token: 0x040020DE RID: 8414
	public List<string> unlockedNames = new List<string>();

	// Token: 0x040020DF RID: 8415
	public List<int> unlockedIndices = new List<int>();
}
