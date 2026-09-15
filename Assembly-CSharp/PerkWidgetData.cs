using System;
using LazyBearTechnology;

// Token: 0x02000944 RID: 2372
public class PerkWidgetData : LazyWidgetDataBase
{
	// Token: 0x1700096C RID: 2412
	// (get) Token: 0x06003E84 RID: 16004 RVA: 0x0012A41E File Offset: 0x0012861E
	// (set) Token: 0x06003E85 RID: 16005 RVA: 0x0012A426 File Offset: 0x00128626
	public PerkData PerkData { get; private set; }

	// Token: 0x1700096D RID: 2413
	// (get) Token: 0x06003E86 RID: 16006 RVA: 0x0012A42F File Offset: 0x0012862F
	// (set) Token: 0x06003E87 RID: 16007 RVA: 0x0012A437 File Offset: 0x00128637
	public Action<PerkWidgetData> OnPress { get; private set; }

	// Token: 0x1700096E RID: 2414
	// (get) Token: 0x06003E88 RID: 16008 RVA: 0x0012A440 File Offset: 0x00128640
	// (set) Token: 0x06003E89 RID: 16009 RVA: 0x0012A448 File Offset: 0x00128648
	public Action<PerkWidgetData> OnOver { get; private set; }

	// Token: 0x1700096F RID: 2415
	// (get) Token: 0x06003E8A RID: 16010 RVA: 0x0012A451 File Offset: 0x00128651
	// (set) Token: 0x06003E8B RID: 16011 RVA: 0x0012A459 File Offset: 0x00128659
	public Action<PerkWidgetData> OnOut { get; private set; }

	// Token: 0x17000970 RID: 2416
	// (get) Token: 0x06003E8C RID: 16012 RVA: 0x0012A462 File Offset: 0x00128662
	// (set) Token: 0x06003E8D RID: 16013 RVA: 0x0012A46A File Offset: 0x0012866A
	public bool IsActive { get; set; }

	// Token: 0x06003E8E RID: 16014 RVA: 0x0012A473 File Offset: 0x00128673
	public PerkWidgetData(PerkData perkData, bool isActive, Action<PerkWidgetData> onPress, Action<PerkWidgetData> onOver, Action<PerkWidgetData> onOut)
	{
		this.PerkData = perkData;
		this.OnPress = onPress;
		this.OnOver = onOver;
		this.OnOut = onOut;
		this.IsActive = isActive;
	}
}
