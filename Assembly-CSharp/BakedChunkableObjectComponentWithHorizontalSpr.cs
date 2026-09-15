using System;

// Token: 0x020006EB RID: 1771
public class BakedChunkableObjectComponentWithHorizontalSpr : BakedChunkableObjectComponent
{
	// Token: 0x06002ED1 RID: 11985 RVA: 0x000E0188 File Offset: 0x000DE388
	public override BakedChunkableObjectComponentData GetData()
	{
		if (this.data == null)
		{
			this.data = new BakedChunkableObjectComponentData();
		}
		return this.data;
	}

	// Token: 0x06002ED2 RID: 11986 RVA: 0x000E01A3 File Offset: 0x000DE3A3
	public override void SetData(BakedChunkableObjectComponentData data)
	{
		this.data = data;
	}

	// Token: 0x06002ED3 RID: 11987 RVA: 0x000E01AC File Offset: 0x000DE3AC
	public override void BakeData()
	{
		base.BakeData();
		HorizontalSprite componentInChildren = base.GetComponentInChildren<HorizontalSprite>(true);
		if (componentInChildren != null)
		{
			this.data.gndLocalPos = componentInChildren.transform.localPosition;
		}
	}

	// Token: 0x06002ED4 RID: 11988 RVA: 0x000E01E8 File Offset: 0x000DE3E8
	public override void ApplyData()
	{
		base.ApplyData();
		HorizontalSprite componentInChildren = base.GetComponentInChildren<HorizontalSprite>(true);
		if (componentInChildren != null)
		{
			componentInChildren.transform.localPosition = this.data.gndLocalPos;
		}
	}

	// Token: 0x040025D2 RID: 9682
	public BakedChunkableObjectComponentData data;
}
