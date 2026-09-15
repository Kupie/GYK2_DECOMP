using System;

// Token: 0x02000595 RID: 1429
[Serializable]
public class PlayerColorCustomizationPairData
{
	// Token: 0x060024C3 RID: 9411 RVA: 0x00021B94 File Offset: 0x0001FD94
	public PlayerColorCustomizationPairData()
	{
	}

	// Token: 0x060024C4 RID: 9412 RVA: 0x000AC98F File Offset: 0x000AAB8F
	public PlayerColorCustomizationPairData(int index, PlayerColorCustomizationType type)
	{
		this.index = index;
		this.type = type;
	}

	// Token: 0x04002077 RID: 8311
	public int index;

	// Token: 0x04002078 RID: 8312
	public PlayerColorCustomizationType type;
}
