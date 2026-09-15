using System;
using System.Collections.Generic;
using LinqTools;

// Token: 0x020001D9 RID: 473
[Serializable]
public class AlchemyMixDef : CraftDef
{
	// Token: 0x17000201 RID: 513
	// (get) Token: 0x06000C01 RID: 3073 RVA: 0x0003C95A File Offset: 0x0003AB5A
	public ItemDef ResultItem
	{
		get
		{
			return GameBalance.Me.GetData<ItemDef>(this.outputItems.chanceOutputItems[0].id);
		}
	}

	// Token: 0x17000202 RID: 514
	// (get) Token: 0x06000C02 RID: 3074 RVA: 0x0003C97C File Offset: 0x0003AB7C
	public AlchemyFormulaDef Formula
	{
		get
		{
			return GameBalance.Me.GetData<AlchemyFormulaDef>(this.outputItems.chanceOutputItems[0].id ?? "");
		}
	}

	// Token: 0x17000203 RID: 515
	// (get) Token: 0x06000C03 RID: 3075 RVA: 0x0003C9A7 File Offset: 0x0003ABA7
	public CraftDef AlchemyWorkBenchCraft
	{
		get
		{
			return GameBalance.GetAlchemyMixCraftDef(this.id);
		}
	}

	// Token: 0x17000204 RID: 516
	// (get) Token: 0x06000C04 RID: 3076 RVA: 0x0003C9B4 File Offset: 0x0003ABB4
	public CraftDef BoostCraft
	{
		get
		{
			if (!this.id.EndsWith("_boost"))
			{
				return null;
			}
			return GameBalance.GetCraftDef(this.id.Substring(this.id.LastIndexOf(':') + 1));
		}
	}

	// Token: 0x17000205 RID: 517
	// (get) Token: 0x06000C05 RID: 3077 RVA: 0x0003C9EC File Offset: 0x0003ABEC
	public bool IsResultKnown
	{
		get
		{
			MainGame instance = MainGame.Instance;
			KnowledgeSystem knowledgeSystem;
			if (instance == null)
			{
				knowledgeSystem = null;
			}
			else
			{
				GameSave gameSave = instance.GameSave;
				knowledgeSystem = ((gameSave != null) ? gameSave.knowledgeSystem : null);
			}
			KnowledgeSystem knowledgeSystem2 = knowledgeSystem;
			return knowledgeSystem2 == null || knowledgeSystem2.IsAlchemyFormulaKnown(this.Formula);
		}
	}

	// Token: 0x06000C06 RID: 3078 RVA: 0x0003CA28 File Offset: 0x0003AC28
	public static bool IsUnknownMixResult(string craftId)
	{
		if (string.IsNullOrEmpty(craftId) || !craftId.StartsWith("mix"))
		{
			return false;
		}
		AlchemyMixDef alchemyMixDef = GameBalance.GetAlchemyMixDef(craftId);
		return alchemyMixDef != null && !alchemyMixDef.IsResultKnown;
	}

	// Token: 0x06000C07 RID: 3079 RVA: 0x0003CA64 File Offset: 0x0003AC64
	public static string MixId(string[] ingredients, CraftDef boostDef = null)
	{
		string text = "mix";
		List<string> list = ingredients.ToList<string>();
		list.Sort();
		foreach (string text2 in list)
		{
			text = text + ":" + text2;
		}
		if (boostDef != null && boostDef.id.EndsWith("_boost"))
		{
			text = text + ":" + boostDef.id;
		}
		return text;
	}

	// Token: 0x04000D28 RID: 3368
	public string[] ingredients;
}
