using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x02000967 RID: 2407
public class UITechTreeElementWindowData : LazyWidgetDataBase
{
	// Token: 0x17000988 RID: 2440
	// (get) Token: 0x06003F5F RID: 16223 RVA: 0x0012FEAB File Offset: 0x0012E0AB
	// (set) Token: 0x06003F60 RID: 16224 RVA: 0x0012FEB3 File Offset: 0x0012E0B3
	public string HeaderText { get; private set; }

	// Token: 0x17000989 RID: 2441
	// (get) Token: 0x06003F61 RID: 16225 RVA: 0x0012FEBC File Offset: 0x0012E0BC
	// (set) Token: 0x06003F62 RID: 16226 RVA: 0x0012FEC4 File Offset: 0x0012E0C4
	public string TopText { get; private set; }

	// Token: 0x1700098A RID: 2442
	// (get) Token: 0x06003F63 RID: 16227 RVA: 0x0012FECD File Offset: 0x0012E0CD
	// (set) Token: 0x06003F64 RID: 16228 RVA: 0x0012FED5 File Offset: 0x0012E0D5
	public string BotText { get; private set; }

	// Token: 0x1700098B RID: 2443
	// (get) Token: 0x06003F65 RID: 16229 RVA: 0x0012FEDE File Offset: 0x0012E0DE
	// (set) Token: 0x06003F66 RID: 16230 RVA: 0x0012FEE6 File Offset: 0x0012E0E6
	public TechDef TechDef { get; private set; }

	// Token: 0x1700098C RID: 2444
	// (get) Token: 0x06003F67 RID: 16231 RVA: 0x0012FEEF File Offset: 0x0012E0EF
	// (set) Token: 0x06003F68 RID: 16232 RVA: 0x0012FEF7 File Offset: 0x0012E0F7
	public LinkedEntityWidgetData[] LinkedEntityWidgetDatas { get; private set; }

	// Token: 0x1700098D RID: 2445
	// (get) Token: 0x06003F69 RID: 16233 RVA: 0x0012FF00 File Offset: 0x0012E100
	// (set) Token: 0x06003F6A RID: 16234 RVA: 0x0012FF08 File Offset: 0x0012E108
	public List<UIDialogWindowData.ButtonData> ButtonsData { get; set; }

	// Token: 0x06003F6B RID: 16235 RVA: 0x0012FF14 File Offset: 0x0012E114
	public UITechTreeElementWindowData(TechDef techDef, List<UIDialogWindowData.ButtonData> buttonsData, string botLabelLocale = null, string topLabelLocale = null)
	{
		this.TechDef = techDef;
		this.HeaderText = LLBase.L(techDef.id);
		this.ButtonsData = buttonsData;
		if (!string.IsNullOrEmpty(topLabelLocale))
		{
			this.TopText = LLBase.L(topLabelLocale);
		}
		if (!string.IsNullOrEmpty(botLabelLocale))
		{
			this.BotText = LLBase.L(botLabelLocale);
		}
	}
}
