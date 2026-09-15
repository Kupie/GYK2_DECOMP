using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200020D RID: 525
[Serializable]
public class TalentLevelUpDef : BalanceBaseObject
{
	// Token: 0x17000231 RID: 561
	// (get) Token: 0x06000CB5 RID: 3253 RVA: 0x0003FE7B File Offset: 0x0003E07B
	public Vector2 TreePos
	{
		get
		{
			return new Vector2(this.posX, this.posY);
		}
	}

	// Token: 0x17000232 RID: 562
	// (get) Token: 0x06000CB6 RID: 3254 RVA: 0x0003FE8E File Offset: 0x0003E08E
	public Sprite Icon
	{
		get
		{
			return LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(this.icon, null);
		}
	}

	// Token: 0x17000233 RID: 563
	// (get) Token: 0x06000CB7 RID: 3255 RVA: 0x0003FEA1 File Offset: 0x0003E0A1
	public int ZombieTechPoints
	{
		get
		{
			if (this.techRed > 0)
			{
				return this.techRed;
			}
			if (this.techBlue > 0)
			{
				return this.techBlue;
			}
			if (this.techGreen > 0)
			{
				return this.techGreen;
			}
			return 1;
		}
	}

	// Token: 0x17000234 RID: 564
	// (get) Token: 0x06000CB8 RID: 3256 RVA: 0x0003FED4 File Offset: 0x0003E0D4
	public string ZombieTechPointsIcon
	{
		get
		{
			if (this.techRed > 0)
			{
				return "tech_red".FontIcon();
			}
			if (this.techBlue > 0)
			{
				return "tech_blue".FontIcon();
			}
			if (this.techGreen > 0)
			{
				return "tech_green".FontIcon();
			}
			return string.Empty;
		}
	}

	// Token: 0x17000235 RID: 565
	// (get) Token: 0x06000CB9 RID: 3257 RVA: 0x0003FF24 File Offset: 0x0003E124
	public bool ParentsUnlocked
	{
		get
		{
			if (this.isZombiePerk)
			{
				Debug.LogError("Do not request ParentsUnlocked for TalentLevelUpDef with id:[" + this.id + "], it is for zombie!!!");
				return false;
			}
			bool flag = false;
			TalentLevelUpDef.LockType lockType = this.lockType;
			if (lockType != TalentLevelUpDef.LockType.All)
			{
				if (lockType != TalentLevelUpDef.LockType.Any)
				{
					goto IL_00EF;
				}
			}
			else
			{
				flag = true;
				using (List<string>.Enumerator enumerator = this.parents.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string text = enumerator.Current;
						if (!MainGame.Instance.GameSave.talentSystemData.GetTalentBranch(this.talentId).studiedLevelUps.Contains(text))
						{
							flag = false;
							break;
						}
					}
					return flag;
				}
			}
			using (List<string>.Enumerator enumerator = this.parents.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string text2 = enumerator.Current;
					if (MainGame.Instance.GameSave.talentSystemData.GetTalentBranch(this.talentId).studiedLevelUps.Contains(text2))
					{
						flag = true;
					}
				}
				return flag;
			}
			IL_00EF:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x06000CBA RID: 3258 RVA: 0x00040044 File Offset: 0x0003E244
	public static void Link()
	{
		foreach (TalentLevelUpDef talentLevelUpDef in GameBalance.Me.talentLevelUpDefs)
		{
			talentLevelUpDef.InitParentsAndChildren();
		}
	}

	// Token: 0x06000CBB RID: 3259 RVA: 0x00040098 File Offset: 0x0003E298
	private void InitParentsAndChildren()
	{
		for (int i = 0; i < this.parents.Count; i++)
		{
			string text = this.parents[i];
			if (!string.IsNullOrEmpty(text))
			{
				TalentLevelUpDef data = GameBalance.Me.GetData<TalentLevelUpDef>(text);
				if (data == null)
				{
					Debug.LogError("no TalentLevelUpDef with id: [" + text + "]");
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

	// Token: 0x04000EFD RID: 3837
	[AutoParse("icon")]
	public string icon;

	// Token: 0x04000EFE RID: 3838
	[AutoParse("talent")]
	public string talentId;

	// Token: 0x04000EFF RID: 3839
	[AutoParse("add_level")]
	public int talentValueAdd;

	// Token: 0x04000F00 RID: 3840
	[AutoParse("talent_exp_points_price")]
	public int talentExpPointsPrice;

	// Token: 0x04000F01 RID: 3841
	[AutoParse("perk")]
	public string linkedPerk;

	// Token: 0x04000F02 RID: 3842
	[AutoParse("is_zombie_perk")]
	public bool isZombiePerk;

	// Token: 0x04000F03 RID: 3843
	[AutoParse("is_unknown")]
	public bool isUnknown;

	// Token: 0x04000F04 RID: 3844
	[AutoParse("is_hidden")]
	public bool isHidden;

	// Token: 0x04000F05 RID: 3845
	[AutoParse("available_at_start")]
	public bool availableAtStart;

	// Token: 0x04000F06 RID: 3846
	[AutoParse("is_free")]
	public bool isFreeCoordinates;

	// Token: 0x04000F07 RID: 3847
	[AutoParse("parents")]
	public List<string> parents = new List<string>();

	// Token: 0x04000F08 RID: 3848
	[AutoParse("tech_r")]
	public int techRed;

	// Token: 0x04000F09 RID: 3849
	[AutoParse("tech_g")]
	public int techGreen;

	// Token: 0x04000F0A RID: 3850
	[AutoParse("tech_b")]
	public int techBlue;

	// Token: 0x04000F0B RID: 3851
	[AutoParse("expression_on_buy")]
	public List<LazyExpression> expressionsOnBuy = new List<LazyExpression>();

	// Token: 0x04000F0C RID: 3852
	[AutoParse("xpos")]
	[SerializeField]
	private float posX;

	// Token: 0x04000F0D RID: 3853
	[AutoParse("ypos")]
	[SerializeField]
	private float posY;

	// Token: 0x04000F0E RID: 3854
	public TalentLevelUpDef.LockType lockType;

	// Token: 0x04000F0F RID: 3855
	[NonSerialized]
	public List<TalentLevelUpDef> childDefinitionList = new List<TalentLevelUpDef>();

	// Token: 0x04000F10 RID: 3856
	[NonSerialized]
	public List<TalentLevelUpDef> parentDefinitionList = new List<TalentLevelUpDef>();

	// Token: 0x0200020E RID: 526
	public enum State
	{
		// Token: 0x04000F12 RID: 3858
		Unknown,
		// Token: 0x04000F13 RID: 3859
		Hidden,
		// Token: 0x04000F14 RID: 3860
		Visible,
		// Token: 0x04000F15 RID: 3861
		Available,
		// Token: 0x04000F16 RID: 3862
		Unlocked
	}

	// Token: 0x0200020F RID: 527
	public enum LockType
	{
		// Token: 0x04000F18 RID: 3864
		All,
		// Token: 0x04000F19 RID: 3865
		Any
	}
}
