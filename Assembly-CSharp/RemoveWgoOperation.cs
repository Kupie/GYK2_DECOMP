using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x02000479 RID: 1145
[Serializable]
public class RemoveWgoOperation : SaveFixWgoOperation
{
	// Token: 0x17000530 RID: 1328
	// (get) Token: 0x06001E3B RID: 7739 RVA: 0x0008DBC0 File Offset: 0x0008BDC0
	public IReadOnlyList<SGuid> WgoUniqueIds
	{
		get
		{
			return this.wgoUniqueIds;
		}
	}

	// Token: 0x17000531 RID: 1329
	// (get) Token: 0x06001E3C RID: 7740 RVA: 0x0008DBC8 File Offset: 0x0008BDC8
	public IReadOnlyList<string> DebugWgoIds
	{
		get
		{
			return this.debugWgoIds;
		}
	}

	// Token: 0x06001E3D RID: 7741 RVA: 0x0008DBD0 File Offset: 0x0008BDD0
	public override string Info()
	{
		return "remove wgo";
	}

	// Token: 0x17000532 RID: 1330
	// (get) Token: 0x06001E3E RID: 7742 RVA: 0x0008DBD7 File Offset: 0x0008BDD7
	public override int InfoCount
	{
		get
		{
			List<SGuid> list = this.wgoUniqueIds;
			if (list == null)
			{
				return 0;
			}
			return list.Count;
		}
	}

	// Token: 0x17000533 RID: 1331
	// (get) Token: 0x06001E3F RID: 7743 RVA: 0x0008DBEC File Offset: 0x0008BDEC
	protected override string SummaryBody
	{
		get
		{
			List<SGuid> list = this.wgoUniqueIds;
			int num = ((list != null) ? list.Count : 0);
			if (num == 0)
			{
				return "(empty)";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(num);
			stringBuilder.Append((num == 1) ? " object" : " objects");
			if (this.debugWgoIds != null && this.debugWgoIds.Count > 0)
			{
				stringBuilder.Append("  (");
				int num2 = Mathf.Min(this.debugWgoIds.Count, 3);
				for (int i = 0; i < num2; i++)
				{
					if (i > 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(this.debugWgoIds[i]);
				}
				if (this.debugWgoIds.Count > num2)
				{
					stringBuilder.Append(", …");
				}
				stringBuilder.Append(')');
			}
			return stringBuilder.ToString();
		}
	}

	// Token: 0x06001E40 RID: 7744 RVA: 0x0008DCC6 File Offset: 0x0008BEC6
	public override bool ContainsUniqueId(SGuid uniqueId)
	{
		return this.IndexOf(uniqueId) >= 0;
	}

	// Token: 0x06001E41 RID: 7745 RVA: 0x0008DCD8 File Offset: 0x0008BED8
	public void AddTarget(SGuid uniqueId, string wgoId)
	{
		if (SGuid.IsNullOrEmpty(uniqueId) || this.ContainsUniqueId(uniqueId))
		{
			return;
		}
		if (this.wgoUniqueIds == null)
		{
			this.wgoUniqueIds = new List<SGuid>();
		}
		if (this.debugWgoIds == null)
		{
			this.debugWgoIds = new List<string>();
		}
		this.wgoUniqueIds.Add(uniqueId);
		this.debugWgoIds.Add(string.IsNullOrEmpty(wgoId) ? uniqueId.ToString() : wgoId);
	}

	// Token: 0x06001E42 RID: 7746 RVA: 0x0008DD48 File Offset: 0x0008BF48
	public override void Apply(SaveFixContext ctx)
	{
		if (this.wgoUniqueIds == null || this.wgoUniqueIds.Count == 0)
		{
			ctx.LogWarning(this.Summary + ": list is empty");
			return;
		}
		for (int i = 0; i < this.wgoUniqueIds.Count; i++)
		{
			SGuid sguid = this.wgoUniqueIds[i];
			string text = ((i < this.debugWgoIds.Count) ? this.debugWgoIds[i] : ((sguid != null) ? sguid.ToString() : null));
			if (SGuid.IsNullOrEmpty(sguid))
			{
				ctx.LogWarning(string.Format("{0}: empty uniqueId at index {1}, skip", this.Summary, i));
			}
			else if (!ctx.RemoveWgoData(sguid))
			{
				ctx.LogWarning(string.Format("{0}: uniqueId [{1}] ({2}) not found in save, skip", this.Summary, sguid, text));
			}
			else
			{
				RemoveWgoOperation.RemoveDelayedEventReference(ctx, sguid);
				ctx.WarnAboutDanglingReferences(sguid, text);
				ctx.Log(string.Format("{0}: removed [{1}] [{2}]", this.Summary, text, sguid));
			}
		}
	}

	// Token: 0x06001E43 RID: 7747 RVA: 0x0008DE44 File Offset: 0x0008C044
	private static void RemoveDelayedEventReference(SaveFixContext ctx, SGuid uniqueId)
	{
		WgoDelayedEventSystemData wgoDelayedEventSystemData = ctx.GameSave.wgoDelayedEventSystemData;
		List<SGuid> list = ((wgoDelayedEventSystemData != null) ? wgoDelayedEventSystemData.wgoUniqueIds : null);
		if (list == null)
		{
			return;
		}
		for (int i = list.Count - 1; i >= 0; i--)
		{
			if (list[i] == uniqueId)
			{
				list.RemoveAt(i);
			}
		}
	}

	// Token: 0x06001E44 RID: 7748 RVA: 0x0008DE98 File Offset: 0x0008C098
	private int IndexOf(SGuid uniqueId)
	{
		if (this.wgoUniqueIds == null || SGuid.IsNullOrEmpty(uniqueId))
		{
			return -1;
		}
		for (int i = 0; i < this.wgoUniqueIds.Count; i++)
		{
			if (this.wgoUniqueIds[i] == uniqueId)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x04001BA3 RID: 7075
	[SerializeField]
	private List<string> debugWgoIds = new List<string>();

	// Token: 0x04001BA4 RID: 7076
	[SerializeField]
	private List<SGuid> wgoUniqueIds = new List<SGuid>();
}
