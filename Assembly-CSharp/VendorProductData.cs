using System;

// Token: 0x0200021C RID: 540
[Serializable]
public class VendorProductData : IAutoParsable
{
	// Token: 0x17000240 RID: 576
	// (get) Token: 0x06000CDB RID: 3291 RVA: 0x00040C24 File Offset: 0x0003EE24
	public ItemDef Definition
	{
		get
		{
			return GameBalance.Me.GetData<ItemDef>(this.itemId);
		}
	}

	// Token: 0x06000CDC RID: 3292 RVA: 0x00040C38 File Offset: 0x0003EE38
	public override string ToString()
	{
		if (string.IsNullOrEmpty(this.itemId))
		{
			return string.Empty;
		}
		string text = this.itemId;
		if (this.priceMod != 0)
		{
			text = string.Format("[{0}]", this.priceMod) + text;
		}
		if (this.baseCount != 0)
		{
			text += string.Format("={0}", this.baseCount);
		}
		return text;
	}

	// Token: 0x04000F6E RID: 3950
	public string itemId;

	// Token: 0x04000F6F RID: 3951
	public int priceMod;

	// Token: 0x04000F70 RID: 3952
	public int baseCount;
}
