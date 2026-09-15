using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000202 RID: 514
[Serializable]
public class QuestDef : BalanceBaseObject
{
	// Token: 0x1700022B RID: 555
	// (get) Token: 0x06000C98 RID: 3224 RVA: 0x0003F6D7 File Offset: 0x0003D8D7
	public Sprite Icon
	{
		get
		{
			return LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(this.iconId, "default_quest_icon");
		}
	}

	// Token: 0x1700022C RID: 556
	// (get) Token: 0x06000C99 RID: 3225 RVA: 0x0003F6EE File Offset: 0x0003D8EE
	public Sprite Portrait
	{
		get
		{
			if (this.LinkedWgo != null)
			{
				return this.LinkedWgo.Portrait;
			}
			return LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("default_quest_icon", null);
		}
	}

	// Token: 0x1700022D RID: 557
	// (get) Token: 0x06000C9A RID: 3226 RVA: 0x0003F714 File Offset: 0x0003D914
	private WGODef LinkedWgo
	{
		get
		{
			return GameBalance.Me.GetData<WGODef>(this.wgoNpcId);
		}
	}

	// Token: 0x1700022E RID: 558
	// (get) Token: 0x06000C9B RID: 3227 RVA: 0x0003F726 File Offset: 0x0003D926
	public Vector2Int TreePos
	{
		get
		{
			return new Vector2Int(this.posX, this.posY);
		}
	}

	// Token: 0x06000C9C RID: 3228 RVA: 0x0003F73C File Offset: 0x0003D93C
	public static void LinkQuests()
	{
		foreach (QuestDef questDef in GameBalance.Me.questDefs)
		{
			questDef.InitParentsAndChildren();
		}
	}

	// Token: 0x06000C9D RID: 3229 RVA: 0x0003F790 File Offset: 0x0003D990
	private void InitParentsAndChildren()
	{
		for (int i = 0; i < this.parents.Count; i++)
		{
			string text = this.parents[i];
			if (!string.IsNullOrEmpty(text))
			{
				QuestDef data = GameBalance.Me.GetData<QuestDef>(text);
				if (data == null)
				{
					Debug.LogError("no quest definition with id: [" + text + "]");
				}
				else
				{
					if (!data.childDefinitionList.Contains(this))
					{
						data.childDefinitionList.Add(this);
					}
					if (!this.parentDefinitionList.Contains(data))
					{
						this.parentDefinitionList.Add(data);
					}
				}
			}
		}
	}

	// Token: 0x04000EB0 RID: 3760
	public bool isHidden;

	// Token: 0x04000EB1 RID: 3761
	[AutoParse("is_unknown")]
	public bool isUnknown;

	// Token: 0x04000EB2 RID: 3762
	[AutoParse("custom_triggers")]
	public List<ExpressionGameRes> customTriggers = new List<ExpressionGameRes>();

	// Token: 0x04000EB3 RID: 3763
	[AutoParse("execute_start")]
	public List<LazyExpression> execExpressionsStart = new List<LazyExpression>();

	// Token: 0x04000EB4 RID: 3764
	[AutoParse("execute_start_fail")]
	public List<LazyExpression> execExpressionsStartFail = new List<LazyExpression>();

	// Token: 0x04000EB5 RID: 3765
	[AutoParse("execute_finish")]
	public List<LazyExpression> execExpressionsFinish = new List<LazyExpression>();

	// Token: 0x04000EB6 RID: 3766
	[AutoParse("parents")]
	public List<string> parents = new List<string>();

	// Token: 0x04000EB7 RID: 3767
	[AutoParse("brother_ids")]
	public List<string> brotherIds = new List<string>();

	// Token: 0x04000EB8 RID: 3768
	[AutoParse("xpos")]
	[SerializeField]
	private int posX;

	// Token: 0x04000EB9 RID: 3769
	[AutoParse("ypos")]
	[SerializeField]
	private int posY;

	// Token: 0x04000EBA RID: 3770
	[AutoParse("phase")]
	public int phase;

	// Token: 0x04000EBB RID: 3771
	public bool hasPosInBalance;

	// Token: 0x04000EBC RID: 3772
	[AutoParse("icon_id")]
	public string iconId;

	// Token: 0x04000EBD RID: 3773
	[AutoParse("wgo_npc_id")]
	public string wgoNpcId;

	// Token: 0x04000EBE RID: 3774
	[AutoParse("rep_visualisation")]
	public GameRes repVisualisationRes;

	// Token: 0x04000EBF RID: 3775
	public QuestCheck startCheck;

	// Token: 0x04000EC0 RID: 3776
	public QuestFinishCheck finishCheck;

	// Token: 0x04000EC1 RID: 3777
	[NonSerialized]
	public List<QuestDef> childDefinitionList = new List<QuestDef>();

	// Token: 0x04000EC2 RID: 3778
	[NonSerialized]
	public List<QuestDef> parentDefinitionList = new List<QuestDef>();
}
