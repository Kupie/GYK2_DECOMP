using System;
using System.Collections.Generic;
using System.Text;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001D6 RID: 470
[Serializable]
public class AlchemyFormulaDef : BalanceBaseObject
{
	// Token: 0x17000200 RID: 512
	// (get) Token: 0x06000BFC RID: 3068 RVA: 0x0003C830 File Offset: 0x0003AA30
	public ItemDef ItemDef
	{
		get
		{
			return GameBalance.Me.GetData<ItemDef>(this.id);
		}
	}

	// Token: 0x06000BFD RID: 3069 RVA: 0x0003C842 File Offset: 0x0003AA42
	public Vector3Int GetRunesAsVector3Int()
	{
		return new Vector3Int(this.runesRed, this.runesGreen, this.runesBlue);
	}

	// Token: 0x06000BFE RID: 3070 RVA: 0x0003C85C File Offset: 0x0003AA5C
	public string GetRunesAsString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (this.runesRed > 0)
		{
			stringBuilder.Append(string.Format("{0}{1}", "rune_r".FontIcon(), this.runesRed));
		}
		if (this.runesGreen > 0)
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append(" ");
			}
			stringBuilder.Append(string.Format("{0}{1}", "rune_g".FontIcon(), this.runesGreen));
		}
		if (this.runesBlue > 0)
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append(" ");
			}
			stringBuilder.Append(string.Format("{0}{1}", "rune_b".FontIcon(), this.runesBlue));
		}
		return stringBuilder.ToString();
	}

	// Token: 0x04000D18 RID: 3352
	[AutoParse("runes_r")]
	public int runesRed;

	// Token: 0x04000D19 RID: 3353
	[AutoParse("runes_g")]
	public int runesGreen;

	// Token: 0x04000D1A RID: 3354
	[AutoParse("runes_b")]
	public int runesBlue;

	// Token: 0x04000D1B RID: 3355
	[AutoParse("tab")]
	public AlchemyFormulaTab tab;

	// Token: 0x04000D1C RID: 3356
	[AutoParse("hidden_at_start")]
	public bool hiddenAtStart;

	// Token: 0x04000D1D RID: 3357
	[AutoParse("crafts_in")]
	public List<string> craftsIn = new List<string>();

	// Token: 0x04000D1E RID: 3358
	[AutoParse("on_craft_end_expressions")]
	public List<LazyExpression> onCraftEndExpressions = new List<LazyExpression>();
}
