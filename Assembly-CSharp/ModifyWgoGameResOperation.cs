using System;
using System.Collections.Generic;
using System.Text;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000476 RID: 1142
[Serializable]
public class ModifyWgoGameResOperation : SaveFixWgoOperation
{
	// Token: 0x1700051D RID: 1309
	// (get) Token: 0x06001E15 RID: 7701 RVA: 0x0008D4CC File Offset: 0x0008B6CC
	public SGuid WgoUniqueId
	{
		get
		{
			return this.wgoUniqueId;
		}
	}

	// Token: 0x1700051E RID: 1310
	// (get) Token: 0x06001E16 RID: 7702 RVA: 0x0008D4D4 File Offset: 0x0008B6D4
	public string WgoId
	{
		get
		{
			return this.wgoId;
		}
	}

	// Token: 0x1700051F RID: 1311
	// (get) Token: 0x06001E17 RID: 7703 RVA: 0x0008D4DC File Offset: 0x0008B6DC
	public GameRes GameRes
	{
		get
		{
			return this.gameRes;
		}
	}

	// Token: 0x17000520 RID: 1312
	// (get) Token: 0x06001E18 RID: 7704 RVA: 0x00028294 File Offset: 0x00026494
	public override bool OccupiesUniqueId
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06001E19 RID: 7705 RVA: 0x0008D4E4 File Offset: 0x0008B6E4
	public override string Info()
	{
		return "wgo gameres";
	}

	// Token: 0x17000521 RID: 1313
	// (get) Token: 0x06001E1A RID: 7706 RVA: 0x0008D4EC File Offset: 0x0008B6EC
	public override int InfoCount
	{
		get
		{
			GameRes gameRes = this.gameRes;
			int? num;
			if (gameRes == null)
			{
				num = null;
			}
			else
			{
				List<GameResAtom> list = gameRes.List;
				num = ((list != null) ? new int?(list.Count) : null);
			}
			int? num2 = num;
			return num2.GetValueOrDefault();
		}
	}

	// Token: 0x17000522 RID: 1314
	// (get) Token: 0x06001E1B RID: 7707 RVA: 0x0008D534 File Offset: 0x0008B734
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
			GameRes gameRes = this.gameRes;
			int? num;
			if (gameRes == null)
			{
				num = null;
			}
			else
			{
				List<GameResAtom> list = gameRes.List;
				num = ((list != null) ? new int?(list.Count) : null);
			}
			int? num2 = num;
			int valueOrDefault = num2.GetValueOrDefault();
			if (valueOrDefault == 0)
			{
				return text2 + "  (empty)";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(text2);
			stringBuilder.Append("  ");
			int num3 = Mathf.Min(valueOrDefault, 3);
			for (int i = 0; i < num3; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append(", ");
				}
				GameResAtom gameResAtom = this.gameRes.List[i];
				stringBuilder.Append(string.IsNullOrEmpty(gameResAtom.type) ? "?" : gameResAtom.type);
				stringBuilder.Append('=');
				stringBuilder.Append(gameResAtom.value);
			}
			if (valueOrDefault > num3)
			{
				stringBuilder.Append(", …");
			}
			return stringBuilder.ToString();
		}
	}

	// Token: 0x06001E1C RID: 7708 RVA: 0x0008D65E File Offset: 0x0008B85E
	public override bool TryGetTargetUniqueId(out SGuid uniqueId)
	{
		uniqueId = this.wgoUniqueId;
		return !SGuid.IsNullOrEmpty(this.wgoUniqueId);
	}

	// Token: 0x06001E1D RID: 7709 RVA: 0x0008D676 File Offset: 0x0008B876
	public void SetTarget(SGuid uniqueId, string id)
	{
		this.wgoUniqueId = uniqueId;
		this.wgoId = id;
		if (this.gameRes == null)
		{
			this.gameRes = new GameRes();
		}
	}

	// Token: 0x06001E1E RID: 7710 RVA: 0x0008D69C File Offset: 0x0008B89C
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
		if (this.gameRes == null || this.gameRes.List == null || this.gameRes.List.Count == 0)
		{
			ctx.LogWarning(this.Summary + ": GameRes is empty");
			return;
		}
		for (int i = 0; i < this.gameRes.List.Count; i++)
		{
			GameResAtom gameResAtom = this.gameRes.List[i];
			if (gameResAtom == null || string.IsNullOrEmpty(gameResAtom.type))
			{
				ctx.LogWarning(string.Format("{0}: empty atom type at index {1}, skip", this.Summary, i));
			}
			else
			{
				float num = wgoData.GetGameRes(gameResAtom.type);
				wgoData.SetGameRes(gameResAtom.type, gameResAtom.value);
				ctx.Log(string.Format("{0}: set [{1}] {2} -> {3}", new object[] { this.Summary, gameResAtom.type, num, gameResAtom.value }));
			}
		}
	}

	// Token: 0x04001B95 RID: 7061
	[SerializeField]
	private string wgoId;

	// Token: 0x04001B96 RID: 7062
	[SerializeField]
	private GameRes gameRes = new GameRes();

	// Token: 0x04001B97 RID: 7063
	[SerializeField]
	private SGuid wgoUniqueId;
}
