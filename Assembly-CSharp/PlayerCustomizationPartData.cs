using System;

// Token: 0x020005A2 RID: 1442
[Serializable]
public class PlayerCustomizationPartData
{
	// Token: 0x060024F6 RID: 9462 RVA: 0x00021B94 File Offset: 0x0001FD94
	public PlayerCustomizationPartData()
	{
	}

	// Token: 0x060024F7 RID: 9463 RVA: 0x000AD67E File Offset: 0x000AB87E
	public PlayerCustomizationPartData(string id, CustomizablePartType type)
	{
		this.id = id;
		this.type = type;
	}

	// Token: 0x0400208E RID: 8334
	public string id;

	// Token: 0x0400208F RID: 8335
	public CustomizablePartType type;
}
