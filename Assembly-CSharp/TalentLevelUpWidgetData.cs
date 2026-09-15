using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200092A RID: 2346
public class TalentLevelUpWidgetData : LazyWidgetDataBase
{
	// Token: 0x17000950 RID: 2384
	// (get) Token: 0x06003DD7 RID: 15831 RVA: 0x00127DBE File Offset: 0x00125FBE
	// (set) Token: 0x06003DD8 RID: 15832 RVA: 0x00127DC6 File Offset: 0x00125FC6
	public TalentLevelUpDef Def { get; private set; }

	// Token: 0x17000951 RID: 2385
	// (get) Token: 0x06003DD9 RID: 15833 RVA: 0x00127DCF File Offset: 0x00125FCF
	// (set) Token: 0x06003DDA RID: 15834 RVA: 0x00127DD7 File Offset: 0x00125FD7
	public TalentLevelUpDef.State State { get; private set; }

	// Token: 0x17000952 RID: 2386
	// (get) Token: 0x06003DDB RID: 15835 RVA: 0x00127DE0 File Offset: 0x00125FE0
	// (set) Token: 0x06003DDC RID: 15836 RVA: 0x00127DE8 File Offset: 0x00125FE8
	public Action OnPress { get; private set; }

	// Token: 0x17000953 RID: 2387
	// (get) Token: 0x06003DDD RID: 15837 RVA: 0x00127DF1 File Offset: 0x00125FF1
	// (set) Token: 0x06003DDE RID: 15838 RVA: 0x00127DF9 File Offset: 0x00125FF9
	public Action OnOver { get; private set; }

	// Token: 0x17000954 RID: 2388
	// (get) Token: 0x06003DDF RID: 15839 RVA: 0x00127E02 File Offset: 0x00126002
	// (set) Token: 0x06003DE0 RID: 15840 RVA: 0x00127E0A File Offset: 0x0012600A
	public Action OnOut { get; private set; }

	// Token: 0x17000955 RID: 2389
	// (get) Token: 0x06003DE1 RID: 15841 RVA: 0x00127E13 File Offset: 0x00126013
	// (set) Token: 0x06003DE2 RID: 15842 RVA: 0x00127E1B File Offset: 0x0012601B
	public ZombieWgoData ZombieWgoData { get; private set; }

	// Token: 0x17000956 RID: 2390
	// (get) Token: 0x06003DE3 RID: 15843 RVA: 0x00127E24 File Offset: 0x00126024
	// (set) Token: 0x06003DE4 RID: 15844 RVA: 0x00127E2C File Offset: 0x0012602C
	public string PriceIconId { get; private set; }

	// Token: 0x06003DE5 RID: 15845 RVA: 0x00127E38 File Offset: 0x00126038
	public TalentLevelUpWidgetData(TalentLevelUpDef def, TalentLevelUpDef.State state, string priceIconId)
	{
		TalentLevelUpWidgetData.<>c__DisplayClass33_0 CS$<>8__locals1 = new TalentLevelUpWidgetData.<>c__DisplayClass33_0();
		CS$<>8__locals1.def = def;
		base..ctor();
		this.Def = CS$<>8__locals1.def;
		this.State = state;
		this.PriceIconId = priceIconId;
		this.OnPress = new Action(CS$<>8__locals1.<.ctor>g__OnPress|0);
	}

	// Token: 0x06003DE6 RID: 15846 RVA: 0x00127E84 File Offset: 0x00126084
	public TalentLevelUpWidgetData(TalentLevelUpDef def, TalentLevelUpDef.State state, ZombieWgoData zombieWgoData, Action onQueueChanged)
	{
		TalentLevelUpWidgetData.<>c__DisplayClass34_0 CS$<>8__locals1 = new TalentLevelUpWidgetData.<>c__DisplayClass34_0();
		CS$<>8__locals1.zombieWgoData = zombieWgoData;
		CS$<>8__locals1.def = def;
		CS$<>8__locals1.onQueueChanged = onQueueChanged;
		base..ctor();
		CS$<>8__locals1.<>4__this = this;
		this.ZombieWgoData = CS$<>8__locals1.zombieWgoData;
		this.Def = CS$<>8__locals1.def;
		this.State = state;
		this.OnPress = new Action(CS$<>8__locals1.<.ctor>g__OnPress|0);
	}

	// Token: 0x040030B7 RID: 12471
	public Vector2 localPosition;

	// Token: 0x040030B8 RID: 12472
	public Vector2 upConnectorPos;

	// Token: 0x040030B9 RID: 12473
	public Vector2 downConnectorPos;

	// Token: 0x040030BA RID: 12474
	public Vector2 leftConnectorPos;

	// Token: 0x040030BB RID: 12475
	public Vector2 rightConnectorPos;
}
