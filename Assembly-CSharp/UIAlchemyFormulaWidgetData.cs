using System;
using LazyBearTechnology;

// Token: 0x020008E9 RID: 2281
public class UIAlchemyFormulaWidgetData : LazyWidgetDataBase
{
	// Token: 0x170008FC RID: 2300
	// (get) Token: 0x06003BAE RID: 15278 RVA: 0x0011D373 File Offset: 0x0011B573
	// (set) Token: 0x06003BAF RID: 15279 RVA: 0x0011D37B File Offset: 0x0011B57B
	public AlchemyFormulaDef AlchemyFormulaDef { get; set; }

	// Token: 0x170008FD RID: 2301
	// (get) Token: 0x06003BB0 RID: 15280 RVA: 0x0011D384 File Offset: 0x0011B584
	// (set) Token: 0x06003BB1 RID: 15281 RVA: 0x0011D38C File Offset: 0x0011B58C
	public ItemDef ItemDef { get; set; }

	// Token: 0x06003BB2 RID: 15282 RVA: 0x0011D395 File Offset: 0x0011B595
	public ItemDef GetDisplayItemDef()
	{
		if (this.ItemDef != null)
		{
			return this.ItemDef;
		}
		return this.AlchemyFormulaDef.ItemDef;
	}

	// Token: 0x06003BB3 RID: 15283 RVA: 0x0011D3B1 File Offset: 0x0011B5B1
	public string GetRunesAsString()
	{
		if (this.ItemDef != null)
		{
			return this.ItemDef.GetRunesAsString();
		}
		return this.AlchemyFormulaDef.GetRunesAsString();
	}
}
