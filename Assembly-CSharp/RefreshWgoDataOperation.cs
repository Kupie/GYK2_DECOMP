using System;
using UnityEngine;

// Token: 0x02000478 RID: 1144
[Serializable]
public class RefreshWgoDataOperation : SaveFixWgoOperation
{
	// Token: 0x1700052B RID: 1323
	// (get) Token: 0x06001E31 RID: 7729 RVA: 0x0008DA7A File Offset: 0x0008BC7A
	public SGuid WgoUniqueId
	{
		get
		{
			return this.wgoUniqueId;
		}
	}

	// Token: 0x1700052C RID: 1324
	// (get) Token: 0x06001E32 RID: 7730 RVA: 0x0008DA82 File Offset: 0x0008BC82
	public string WgoId
	{
		get
		{
			return this.wgoId;
		}
	}

	// Token: 0x1700052D RID: 1325
	// (get) Token: 0x06001E33 RID: 7731 RVA: 0x0008DA8A File Offset: 0x0008BC8A
	public string CustomTag
	{
		get
		{
			return this.customTag;
		}
	}

	// Token: 0x1700052E RID: 1326
	// (get) Token: 0x06001E34 RID: 7732 RVA: 0x0008DA92 File Offset: 0x0008BC92
	public bool IsHidden
	{
		get
		{
			return this.isHidden;
		}
	}

	// Token: 0x06001E35 RID: 7733 RVA: 0x0008DA9A File Offset: 0x0008BC9A
	public override string Info()
	{
		return "refresh wgo";
	}

	// Token: 0x1700052F RID: 1327
	// (get) Token: 0x06001E36 RID: 7734 RVA: 0x0008DAA4 File Offset: 0x0008BCA4
	protected override string SummaryBody
	{
		get
		{
			string text;
			if (!string.IsNullOrEmpty(this.wgoId))
			{
				text = this.wgoId;
			}
			else
			{
				SGuid sguid = this.wgoUniqueId;
				text = ((sguid != null) ? sguid.ToString() : null);
			}
			string text2 = text;
			return string.Format("{0}  tag=[{1}]  hidden={2}", text2, this.customTag, this.isHidden);
		}
	}

	// Token: 0x06001E37 RID: 7735 RVA: 0x0008DAF5 File Offset: 0x0008BCF5
	public override bool TryGetTargetUniqueId(out SGuid uniqueId)
	{
		uniqueId = this.wgoUniqueId;
		return !SGuid.IsNullOrEmpty(this.wgoUniqueId);
	}

	// Token: 0x06001E38 RID: 7736 RVA: 0x0008DB0D File Offset: 0x0008BD0D
	public void SetTarget(SGuid uniqueId, string id, string tag, bool hidden)
	{
		this.wgoUniqueId = uniqueId;
		this.wgoId = id;
		this.customTag = tag;
		this.isHidden = hidden;
	}

	// Token: 0x06001E39 RID: 7737 RVA: 0x0008DB2C File Offset: 0x0008BD2C
	public override void Apply(SaveFixContext ctx)
	{
		if (SGuid.IsNullOrEmpty(this.wgoUniqueId))
		{
			ctx.LogError(this.Summary + ": wgoUniqueId is empty");
			return;
		}
		WgoData wgoData;
		GameSceneData gameSceneData;
		if (!ctx.TryGetWgo(this.wgoUniqueId, out wgoData, out gameSceneData))
		{
			ctx.LogWarning(string.Format("{0}: uniqueId [{1}] id [{2}] not found in save, skip", this.Summary, this.wgoUniqueId, this.wgoId));
			return;
		}
		wgoData.CustomTag = this.customTag;
		wgoData.IsHidden = this.isHidden;
		ctx.Log(this.Summary + ": updated customTag/isHidden");
	}

	// Token: 0x04001B9F RID: 7071
	[SerializeField]
	private string wgoId;

	// Token: 0x04001BA0 RID: 7072
	[SerializeField]
	private string customTag;

	// Token: 0x04001BA1 RID: 7073
	[SerializeField]
	private bool isHidden;

	// Token: 0x04001BA2 RID: 7074
	[SerializeField]
	private SGuid wgoUniqueId;
}
