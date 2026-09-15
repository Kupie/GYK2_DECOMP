using System;
using System.Collections.Generic;

// Token: 0x020005B2 RID: 1458
[Serializable]
public class UnlockedCustomizationPartData
{
	// Token: 0x060025A4 RID: 9636 RVA: 0x000B0630 File Offset: 0x000AE830
	public UnlockedCustomizationPartData()
	{
	}

	// Token: 0x060025A5 RID: 9637 RVA: 0x000B0643 File Offset: 0x000AE843
	public UnlockedCustomizationPartData(CustomizablePartType type, List<string> unlockedIds)
	{
		this.type = type;
		this.unlockedIds = unlockedIds;
	}

	// Token: 0x040020E0 RID: 8416
	public CustomizablePartType type;

	// Token: 0x040020E1 RID: 8417
	public List<string> unlockedIds = new List<string>();
}
