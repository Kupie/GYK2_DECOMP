using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020009D3 RID: 2515
public class UISquadHudWidgetData : LazyWidgetDataBase
{
	// Token: 0x17000A32 RID: 2610
	// (get) Token: 0x06004320 RID: 17184 RVA: 0x0013EE93 File Offset: 0x0013D093
	// (set) Token: 0x06004321 RID: 17185 RVA: 0x0013EE9B File Offset: 0x0013D09B
	public AlliesSpawn AlliesSpawn { get; private set; }

	// Token: 0x17000A33 RID: 2611
	// (get) Token: 0x06004322 RID: 17186 RVA: 0x0013EEA4 File Offset: 0x0013D0A4
	// (set) Token: 0x06004323 RID: 17187 RVA: 0x0013EEAC File Offset: 0x0013D0AC
	public Sprite Banner { get; private set; }

	// Token: 0x06004324 RID: 17188 RVA: 0x0013EEB5 File Offset: 0x0013D0B5
	public UISquadHudWidgetData(AlliesSpawn alliesSpawn, Sprite banner)
	{
		this.AlliesSpawn = alliesSpawn;
		this.Banner = banner;
	}
}
