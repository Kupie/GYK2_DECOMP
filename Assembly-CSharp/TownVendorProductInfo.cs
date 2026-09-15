using System;

// Token: 0x02000219 RID: 537
[Serializable]
public class TownVendorProductInfo : IAutoParsable
{
	// Token: 0x1700023E RID: 574
	// (get) Token: 0x06000CD5 RID: 3285 RVA: 0x00040B65 File Offset: 0x0003ED65
	public ItemDef Definition
	{
		get
		{
			return GameBalance.Me.GetData<ItemDef>(this.itemId);
		}
	}

	// Token: 0x06000CD6 RID: 3286 RVA: 0x00040B78 File Offset: 0x0003ED78
	public override string ToString()
	{
		if (string.IsNullOrEmpty(this.itemId))
		{
			return string.Empty;
		}
		string text = this.itemId;
		if (this.happinessCount != 0)
		{
			text = string.Format("[{0}]", this.happinessCount) + text;
		}
		if (this.itemCount != 0)
		{
			text += string.Format("={0}", this.itemCount);
		}
		return text;
	}

	// Token: 0x04000F5F RID: 3935
	public string itemId;

	// Token: 0x04000F60 RID: 3936
	public int itemCount;

	// Token: 0x04000F61 RID: 3937
	public int happinessCount;

	// Token: 0x04000F62 RID: 3938
	public float perOne;
}
