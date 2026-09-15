using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000210 RID: 528
[Serializable]
public class TechDef : BalanceBaseObject
{
	// Token: 0x17000236 RID: 566
	// (get) Token: 0x06000CBD RID: 3261 RVA: 0x00040161 File Offset: 0x0003E361
	public Vector2 TreePos
	{
		get
		{
			return new Vector2(this.posX, this.posY);
		}
	}

	// Token: 0x17000237 RID: 567
	// (get) Token: 0x06000CBE RID: 3262 RVA: 0x00040174 File Offset: 0x0003E374
	public TechState TechState
	{
		get
		{
			if (MainGame.Instance.GameSave.knowledgeSystem.IsTechUnlocked(this.id))
			{
				return TechState.Unlocked;
			}
			if (MainGame.Instance.GameSave.knowledgeSystem.IsTechHidden(this.id))
			{
				return TechState.Hidden;
			}
			if (!this.ParentsUnlocked)
			{
				return TechState.Visible;
			}
			return TechState.Available;
		}
	}

	// Token: 0x17000238 RID: 568
	// (get) Token: 0x06000CBF RID: 3263 RVA: 0x000401C8 File Offset: 0x0003E3C8
	public bool EnoughResources
	{
		get
		{
			return MainGame.PlayerData.IsEnoughRes(this.PriceRes) && MainGame.PlayerData.IsEnoughRes(this.LockRes);
		}
	}

	// Token: 0x17000239 RID: 569
	// (get) Token: 0x06000CC0 RID: 3264 RVA: 0x000401F0 File Offset: 0x0003E3F0
	public bool ParentsUnlocked
	{
		get
		{
			bool flag = false;
			TechLockType techLockType = this.techLockType;
			if (techLockType != TechLockType.All)
			{
				if (techLockType != TechLockType.Any)
				{
					goto IL_00B5;
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
						if (!MainGame.Instance.GameSave.knowledgeSystem.unlockedTechs.Contains(text))
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
					if (MainGame.Instance.GameSave.knowledgeSystem.unlockedTechs.Contains(text2))
					{
						flag = true;
					}
				}
				return flag;
			}
			IL_00B5:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x1700023A RID: 570
	// (get) Token: 0x06000CC1 RID: 3265 RVA: 0x000402D8 File Offset: 0x0003E4D8
	public GameRes PriceRes
	{
		get
		{
			GameRes gameRes = new GameRes();
			gameRes.Add("tech_red", (float)this.redSpheresPrice);
			gameRes.Add("tech_green", (float)this.greenSpheresPrice);
			gameRes.Add("tech_blue", (float)this.blueSpheresPrice);
			return gameRes;
		}
	}

	// Token: 0x06000CC2 RID: 3266 RVA: 0x00040318 File Offset: 0x0003E518
	public static void InitTechs()
	{
		if (!TechDef.isInitialized)
		{
			TechDef.isInitialized = true;
			foreach (TechDef techDef in GameBalance.Me.techDefs)
			{
				techDef.InitTechDef();
			}
		}
	}

	// Token: 0x06000CC3 RID: 3267 RVA: 0x0004037C File Offset: 0x0003E57C
	private void InitTechDef()
	{
		for (int i = 0; i < this.parents.Count; i++)
		{
			string text = this.parents[i];
			if (!string.IsNullOrEmpty(text))
			{
				TechDef data = GameBalance.Me.GetData<TechDef>(text);
				if (data == null)
				{
					Debug.LogError("no tech definition with id: [" + text + "]");
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
		this.SetLinkedEntityWidgetDatas();
	}

	// Token: 0x1700023B RID: 571
	// (get) Token: 0x06000CC4 RID: 3268 RVA: 0x00040417 File Offset: 0x0003E617
	private GameRes LockRes
	{
		get
		{
			GameRes gameRes = new GameRes();
			gameRes.Add(this.districtReputationLock);
			gameRes.Add(this.CharReputationLock);
			return gameRes;
		}
	}

	// Token: 0x1700023C RID: 572
	// (get) Token: 0x06000CC5 RID: 3269 RVA: 0x00040438 File Offset: 0x0003E638
	public GameRes CharReputationLock
	{
		get
		{
			if (this.repRes == null)
			{
				this.repRes = new GameRes();
			}
			this.repRes.Clear();
			if (this.wgoRepLock.List.Count <= 0)
			{
				return this.repRes;
			}
			WGODef data = GameBalance.Me.GetData<WGODef>(this.wgoRepLock.List[0].type);
			if (data != null)
			{
				this.repRes.Add(data.repResName, this.wgoRepLock.List[0].value);
			}
			return this.repRes;
		}
	}

	// Token: 0x06000CC6 RID: 3270 RVA: 0x000404D4 File Offset: 0x0003E6D4
	public void Unlock(bool free = false)
	{
		if (!free)
		{
			this.SpendPrice();
		}
		MainGame.Instance.GameSave.knowledgeSystem.UnlockTech(this.id, false);
		foreach (string text in this.craftsAfterUnlock)
		{
			MainGame.Instance.GameSave.knowledgeSystem.UnlockCraft(text);
		}
		foreach (string text2 in this.alchemyFormulasAfterUnlock)
		{
			MainGame.Instance.GameSave.knowledgeSystem.UnlockAlchemyFormula(text2);
		}
		foreach (string text3 in this.buildingsAfterUnlock)
		{
			MainGame.Instance.GameSave.knowledgeSystem.UnlockBuilding(text3);
		}
		foreach (string text4 in this.townBuildingsAfterUnlock)
		{
			MainGame.Instance.GameSave.knowledgeSystem.UnlockTownBuilding(text4);
		}
		foreach (string text5 in this.perksAfterUnlock)
		{
			MainGame.Instance.GameSave.perkSystemData.AddPerk(text5);
		}
		MainGame.Instance.GameSave.knowledgeSystem.TryRevealInspirations();
		if (!this.addGameResAfterUnlock.IsEmpty())
		{
			MainGame.PlayerData.AddRes(this.addGameResAfterUnlock);
		}
		if (!this.setGameResAfterUnlock.IsEmpty())
		{
			MainGame.PlayerData.SetRes(this.setGameResAfterUnlock);
		}
		foreach (LazyExpression lazyExpression in this.expressionsAfterUnlock)
		{
			lazyExpression.Evaluate();
		}
	}

	// Token: 0x06000CC7 RID: 3271 RVA: 0x0004072C File Offset: 0x0003E92C
	private void SpendPrice()
	{
		GameRes priceRes = this.PriceRes;
		if (!priceRes.IsEmpty())
		{
			MainGame.PlayerData.AddResWithoutGlobalChangeEvent(priceRes * -1f);
		}
	}

	// Token: 0x06000CC8 RID: 3272 RVA: 0x0004075D File Offset: 0x0003E95D
	public void RemoveTech()
	{
		MainGame.Instance.GameSave.knowledgeSystem.RemoveTech(this.id);
	}

	// Token: 0x06000CC9 RID: 3273 RVA: 0x0004077C File Offset: 0x0003E97C
	public string GetPriceLabel(TextStyle normal, TextStyle notEnough, GameResIconType gameResIconType)
	{
		string text = string.Empty;
		for (int i = 0; i < this.PriceRes.List.Count; i++)
		{
			string text2 = this.PriceRes.List[i].ToFormattedString(false, (string s, string s1) => s + s1, false, true, gameResIconType);
			if (MainGame.PlayerData.GetRes(this.PriceRes.List[i].type, 0f) >= this.PriceRes.List[i].value)
			{
				text2 = normal.ApplyStyleToString(text2, true, true);
			}
			else
			{
				text2 = notEnough.ApplyStyleToString(text2, true, true);
			}
			text += text2;
			if (i != this.PriceRes.List.Count - 1)
			{
				text += " ";
			}
		}
		return text;
	}

	// Token: 0x06000CCA RID: 3274 RVA: 0x0004086C File Offset: 0x0003EA6C
	private void SetLinkedEntityWidgetDatas()
	{
		for (int i = 0; i < this.craftsAfterUnlock.Count; i++)
		{
			CraftDef craftDef = GameBalance.GetCraftDef(this.craftsAfterUnlock[i]);
			this.linkedEntityWidgetDatas.Add(new LinkedEntityWidgetData(craftDef, null));
		}
		for (int j = 0; j < this.alchemyFormulasAfterUnlock.Count; j++)
		{
			AlchemyFormulaDef data = GameBalance.Me.GetData<AlchemyFormulaDef>(this.alchemyFormulasAfterUnlock[j]);
			this.linkedEntityWidgetDatas.Add(new LinkedEntityWidgetData(data, null));
		}
		for (int k = 0; k < this.buildingsAfterUnlock.Count; k++)
		{
			BuildingDef data2 = GameBalance.Me.GetData<BuildingDef>(this.buildingsAfterUnlock[k] ?? "");
			if (data2 != null)
			{
				this.linkedEntityWidgetDatas.Add(new LinkedEntityWidgetData(data2, null, 1, false));
			}
		}
		for (int l = 0; l < this.townBuildingsAfterUnlock.Count; l++)
		{
			TownBuildingDef dataOrNull = GameBalance.Me.GetDataOrNull<TownBuildingDef>(this.townBuildingsAfterUnlock[l]);
			if (dataOrNull != null)
			{
				this.linkedEntityWidgetDatas.Add(new LinkedEntityWidgetData(dataOrNull, null));
			}
		}
		for (int m = 0; m < this.perksAfterUnlock.Count; m++)
		{
			PerkDef data3 = GameBalance.Me.GetData<PerkDef>(this.perksAfterUnlock[m]);
			if (data3 != null && !data3.isHidden)
			{
				this.linkedEntityWidgetDatas.Add(new LinkedEntityWidgetData(data3, null));
			}
		}
		foreach (LinkedEntityWidgetData linkedEntityWidgetData in this.linkedEntityWidgetDatas)
		{
			linkedEntityWidgetData.NotShowStudyWidgetInItemTooltips = true;
			linkedEntityWidgetData.ShowCraftedAtFromCraftDefInsteadOfItem = true;
		}
	}

	// Token: 0x04000F1A RID: 3866
	private static bool isInitialized;

	// Token: 0x04000F1B RID: 3867
	[AutoParse("custom_icon_id")]
	public string customIconId;

	// Token: 0x04000F1C RID: 3868
	[AutoParse("available_at_start")]
	public bool availableAtStart;

	// Token: 0x04000F1D RID: 3869
	[AutoParse("hidden_at_start")]
	public bool hiddenAtStart;

	// Token: 0x04000F1E RID: 3870
	[AutoParse("is_available_in_demo")]
	public bool isAvailableInDemo;

	// Token: 0x04000F1F RID: 3871
	[AutoParse("tab")]
	public TechTreeTab tab;

	// Token: 0x04000F20 RID: 3872
	[AutoParse("parents")]
	public List<string> parents = new List<string>();

	// Token: 0x04000F21 RID: 3873
	[AutoParse("xpos")]
	[SerializeField]
	private float posX;

	// Token: 0x04000F22 RID: 3874
	[AutoParse("ypos")]
	[SerializeField]
	private float posY;

	// Token: 0x04000F23 RID: 3875
	[AutoParse("red_spheres")]
	[SerializeField]
	private int redSpheresPrice;

	// Token: 0x04000F24 RID: 3876
	[AutoParse("green_spheres")]
	[SerializeField]
	private int greenSpheresPrice;

	// Token: 0x04000F25 RID: 3877
	[AutoParse("blue_spheres")]
	[SerializeField]
	private int blueSpheresPrice;

	// Token: 0x04000F26 RID: 3878
	[AutoParse("char_rep")]
	[SerializeField]
	public GameRes wgoRepLock;

	// Token: 0x04000F27 RID: 3879
	[AutoParse("dis_rep")]
	[SerializeField]
	public GameRes districtReputationLock;

	// Token: 0x04000F28 RID: 3880
	[AutoParse("crafts_on_unlock")]
	public List<string> craftsAfterUnlock = new List<string>();

	// Token: 0x04000F29 RID: 3881
	[AutoParse("alchemy_formulas_on_unlock")]
	public List<string> alchemyFormulasAfterUnlock = new List<string>();

	// Token: 0x04000F2A RID: 3882
	[AutoParse("buildings_on_unlock")]
	public List<string> buildingsAfterUnlock = new List<string>();

	// Token: 0x04000F2B RID: 3883
	[AutoParse("town_buildings_on_unlock")]
	public List<string> townBuildingsAfterUnlock = new List<string>();

	// Token: 0x04000F2C RID: 3884
	[AutoParse("perks_on_unlock")]
	public List<string> perksAfterUnlock = new List<string>();

	// Token: 0x04000F2D RID: 3885
	[AutoParse("add_res_on_unlock")]
	public GameRes addGameResAfterUnlock;

	// Token: 0x04000F2E RID: 3886
	[AutoParse("set_res_on_unlock")]
	public GameRes setGameResAfterUnlock;

	// Token: 0x04000F2F RID: 3887
	[AutoParse("expr_on_unlock")]
	public List<LazyExpression> expressionsAfterUnlock;

	// Token: 0x04000F30 RID: 3888
	public TechDefType techDefType;

	// Token: 0x04000F31 RID: 3889
	public TechLockType techLockType;

	// Token: 0x04000F32 RID: 3890
	public static List<string> FlyingReses = new List<string> { "tech_red", "tech_green", "tech_blue", "happiness" };

	// Token: 0x04000F33 RID: 3891
	[NonSerialized]
	public List<TechDef> childDefinitionList = new List<TechDef>();

	// Token: 0x04000F34 RID: 3892
	[NonSerialized]
	public List<TechDef> parentDefinitionList = new List<TechDef>();

	// Token: 0x04000F35 RID: 3893
	[NonSerialized]
	public List<LinkedEntityWidgetData> linkedEntityWidgetDatas = new List<LinkedEntityWidgetData>();

	// Token: 0x04000F36 RID: 3894
	private GameRes repRes;
}
