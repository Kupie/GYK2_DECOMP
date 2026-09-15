using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x02000884 RID: 2180
public class HUDData : LazyWidgetDataBase
{
	// Token: 0x1700084C RID: 2124
	// (get) Token: 0x060037F6 RID: 14326 RVA: 0x0010E16B File Offset: 0x0010C36B
	// (set) Token: 0x060037F7 RID: 14327 RVA: 0x0010E173 File Offset: 0x0010C373
	public UIEnergySanityBarData EnergySanityBarData { get; private set; }

	// Token: 0x1700084D RID: 2125
	// (get) Token: 0x060037F8 RID: 14328 RVA: 0x0010E17C File Offset: 0x0010C37C
	// (set) Token: 0x060037F9 RID: 14329 RVA: 0x0010E184 File Offset: 0x0010C384
	public UIGameResNotificatorData GameResNotificatiorData { get; private set; }

	// Token: 0x1700084E RID: 2126
	// (get) Token: 0x060037FA RID: 14330 RVA: 0x0010E18D File Offset: 0x0010C38D
	// (set) Token: 0x060037FB RID: 14331 RVA: 0x0010E195 File Offset: 0x0010C395
	public UIBuffsDisplayData BuffsDisplayData { get; private set; }

	// Token: 0x1700084F RID: 2127
	// (get) Token: 0x060037FC RID: 14332 RVA: 0x0010E19E File Offset: 0x0010C39E
	// (set) Token: 0x060037FD RID: 14333 RVA: 0x0010E1A6 File Offset: 0x0010C3A6
	public UIHotBarWidgetData HotBarWidgetData { get; private set; }

	// Token: 0x060037FE RID: 14334 RVA: 0x0010E1B0 File Offset: 0x0010C3B0
	public HUDData(GameSave gameSave)
	{
		this.EnergySanityBarData = new UIEnergySanityBarData(gameSave);
		this.GameResNotificatiorData = new UIGameResNotificatorData();
		this.BuffsDisplayData = new UIBuffsDisplayData(new List<PerkType> { PerkType.Buff });
		this.HotBarWidgetData = new UIHotBarWidgetData(gameSave, true, null);
	}
}
