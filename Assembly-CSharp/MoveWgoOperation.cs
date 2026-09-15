using System;
using UnityEngine;

// Token: 0x02000477 RID: 1143
[Serializable]
public class MoveWgoOperation : SaveFixWgoOperation
{
	// Token: 0x17000523 RID: 1315
	// (get) Token: 0x06001E20 RID: 7712 RVA: 0x0008D81A File Offset: 0x0008BA1A
	public SGuid WgoUniqueId
	{
		get
		{
			return this.wgoUniqueId;
		}
	}

	// Token: 0x17000524 RID: 1316
	// (get) Token: 0x06001E21 RID: 7713 RVA: 0x0008D822 File Offset: 0x0008BA22
	public string WgoId
	{
		get
		{
			return this.wgoId;
		}
	}

	// Token: 0x17000525 RID: 1317
	// (get) Token: 0x06001E22 RID: 7714 RVA: 0x0008D82A File Offset: 0x0008BA2A
	public Vector3 FromPosition
	{
		get
		{
			return this.fromPosition;
		}
	}

	// Token: 0x17000526 RID: 1318
	// (get) Token: 0x06001E23 RID: 7715 RVA: 0x0008D832 File Offset: 0x0008BA32
	public bool HasFromPosition
	{
		get
		{
			return this.hasFromPosition;
		}
	}

	// Token: 0x17000527 RID: 1319
	// (get) Token: 0x06001E24 RID: 7716 RVA: 0x0008D83A File Offset: 0x0008BA3A
	public Vector3 NewPosition
	{
		get
		{
			return this.newPosition;
		}
	}

	// Token: 0x17000528 RID: 1320
	// (get) Token: 0x06001E25 RID: 7717 RVA: 0x0008D842 File Offset: 0x0008BA42
	public float SearchRadius
	{
		get
		{
			return this.searchRadius;
		}
	}

	// Token: 0x17000529 RID: 1321
	// (get) Token: 0x06001E26 RID: 7718 RVA: 0x0008D822 File Offset: 0x0008BA22
	public string DebugWgoId
	{
		get
		{
			return this.wgoId;
		}
	}

	// Token: 0x06001E27 RID: 7719 RVA: 0x0008D84A File Offset: 0x0008BA4A
	public override string Info()
	{
		return "move wgo";
	}

	// Token: 0x1700052A RID: 1322
	// (get) Token: 0x06001E28 RID: 7720 RVA: 0x0008D854 File Offset: 0x0008BA54
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
			if (this.hasFromPosition)
			{
				return string.Format("{0}  {1}  ->  {2}", text2, this.fromPosition, this.newPosition);
			}
			return string.Format("{0}  ->  {1}", text2, this.newPosition);
		}
	}

	// Token: 0x06001E29 RID: 7721 RVA: 0x0008D8C9 File Offset: 0x0008BAC9
	public override bool TryGetTargetUniqueId(out SGuid uniqueId)
	{
		uniqueId = this.wgoUniqueId;
		return !SGuid.IsNullOrEmpty(this.wgoUniqueId);
	}

	// Token: 0x06001E2A RID: 7722 RVA: 0x0008D8E1 File Offset: 0x0008BAE1
	public void SetTarget(SGuid uniqueId, Vector3 toPosition, string id)
	{
		this.wgoUniqueId = uniqueId;
		this.newPosition = toPosition;
		this.wgoId = id;
	}

	// Token: 0x06001E2B RID: 7723 RVA: 0x0008D8F8 File Offset: 0x0008BAF8
	public void SetFromPosition(Vector3 position)
	{
		this.fromPosition = position;
		this.hasFromPosition = true;
	}

	// Token: 0x06001E2C RID: 7724 RVA: 0x0008D908 File Offset: 0x0008BB08
	public override void Apply(SaveFixContext ctx)
	{
		if (this.TryMoveByUniqueId(ctx))
		{
			return;
		}
		if (this.TryMoveRelatedNearFromPosition(ctx))
		{
			return;
		}
		ctx.LogWarning(string.Format("{0}: uniqueId [{1}] id [{2}] not found in save, skip", this.Summary, this.wgoUniqueId, this.wgoId));
	}

	// Token: 0x06001E2D RID: 7725 RVA: 0x0008D940 File Offset: 0x0008BB40
	private bool TryMoveByUniqueId(SaveFixContext ctx)
	{
		if (SGuid.IsNullOrEmpty(this.wgoUniqueId))
		{
			return false;
		}
		if (!ctx.MoveWgo(this.wgoUniqueId, this.newPosition))
		{
			return false;
		}
		ctx.Log(this.Summary + ": moved by uniqueId");
		return true;
	}

	// Token: 0x06001E2E RID: 7726 RVA: 0x0008D980 File Offset: 0x0008BB80
	private bool TryMoveRelatedNearFromPosition(SaveFixContext ctx)
	{
		if (!this.hasFromPosition || string.IsNullOrEmpty(this.wgoId))
		{
			return false;
		}
		WgoData wgoData;
		GameSceneData gameSceneData;
		if (MoveWgoOperation.IsTreeId(this.wgoId))
		{
			if (!ctx.TryFindWgoByGroupNear("trees", this.fromPosition, this.searchRadius, out wgoData, out gameSceneData))
			{
				return false;
			}
		}
		else if (!ctx.TryFindWgoByIdNear(this.wgoId, this.fromPosition, this.searchRadius, out wgoData, out gameSceneData))
		{
			return false;
		}
		if (!ctx.MoveWgo(wgoData.UniqueId, this.newPosition))
		{
			return false;
		}
		ctx.Log(string.Format("{0}: moved related [{1}] [{2}] near {3}", new object[] { this.Summary, wgoData.id, wgoData.UniqueId, this.fromPosition }));
		return true;
	}

	// Token: 0x06001E2F RID: 7727 RVA: 0x0008DA46 File Offset: 0x0008BC46
	private static bool IsTreeId(string id)
	{
		return GameBalance.Me != null && GameBalance.Me.HasWgoIdByGroup("trees", id);
	}

	// Token: 0x04001B98 RID: 7064
	private const float DefaultSearchRadius = 0.01f;

	// Token: 0x04001B99 RID: 7065
	[SerializeField]
	private string wgoId;

	// Token: 0x04001B9A RID: 7066
	[SerializeField]
	private Vector3 fromPosition;

	// Token: 0x04001B9B RID: 7067
	[SerializeField]
	private bool hasFromPosition;

	// Token: 0x04001B9C RID: 7068
	[SerializeField]
	private Vector3 newPosition;

	// Token: 0x04001B9D RID: 7069
	[SerializeField]
	[Tooltip("Used when uniqueId is missing. Search within this XZ radius of fromPosition.")]
	private float searchRadius = 0.01f;

	// Token: 0x04001B9E RID: 7070
	[SerializeField]
	private SGuid wgoUniqueId;
}
