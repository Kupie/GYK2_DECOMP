using System;
using UnityEngine;

// Token: 0x02000475 RID: 1141
[Serializable]
public class ModifyWgoCustomTagOperation : SaveFixWgoOperation
{
	// Token: 0x17000518 RID: 1304
	// (get) Token: 0x06001E0B RID: 7691 RVA: 0x0008D385 File Offset: 0x0008B585
	public SGuid WgoUniqueId
	{
		get
		{
			return this.wgoUniqueId;
		}
	}

	// Token: 0x17000519 RID: 1305
	// (get) Token: 0x06001E0C RID: 7692 RVA: 0x0008D38D File Offset: 0x0008B58D
	public string WgoId
	{
		get
		{
			return this.wgoId;
		}
	}

	// Token: 0x1700051A RID: 1306
	// (get) Token: 0x06001E0D RID: 7693 RVA: 0x0008D395 File Offset: 0x0008B595
	public string CustomTag
	{
		get
		{
			return this.customTag;
		}
	}

	// Token: 0x1700051B RID: 1307
	// (get) Token: 0x06001E0E RID: 7694 RVA: 0x00028294 File Offset: 0x00026494
	public override bool OccupiesUniqueId
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06001E0F RID: 7695 RVA: 0x0008D39D File Offset: 0x0008B59D
	public override string Info()
	{
		return "wgo custom tag";
	}

	// Token: 0x1700051C RID: 1308
	// (get) Token: 0x06001E10 RID: 7696 RVA: 0x0008D3A4 File Offset: 0x0008B5A4
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
			return text + "  tag=[" + this.customTag + "]";
		}
	}

	// Token: 0x06001E11 RID: 7697 RVA: 0x0008D3E2 File Offset: 0x0008B5E2
	public override bool TryGetTargetUniqueId(out SGuid uniqueId)
	{
		uniqueId = this.wgoUniqueId;
		return !SGuid.IsNullOrEmpty(this.wgoUniqueId);
	}

	// Token: 0x06001E12 RID: 7698 RVA: 0x0008D3FA File Offset: 0x0008B5FA
	public void SetTarget(SGuid uniqueId, string id, string tag)
	{
		this.wgoUniqueId = uniqueId;
		this.wgoId = id;
		this.customTag = tag;
	}

	// Token: 0x06001E13 RID: 7699 RVA: 0x0008D414 File Offset: 0x0008B614
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
		string text = wgoData.CustomTag;
		wgoData.CustomTag = this.customTag;
		ctx.Log(string.Concat(new string[] { this.Summary, ": customTag [", text, "] -> [", this.customTag, "]" }));
	}

	// Token: 0x04001B92 RID: 7058
	[SerializeField]
	private string wgoId;

	// Token: 0x04001B93 RID: 7059
	[SerializeField]
	private string customTag;

	// Token: 0x04001B94 RID: 7060
	[SerializeField]
	private SGuid wgoUniqueId;
}
