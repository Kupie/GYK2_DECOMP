using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x0200098F RID: 2447
public class UICraftQueueElementWidgetData : LazyWidgetDataBase
{
	// Token: 0x170009E2 RID: 2530
	// (get) Token: 0x06004120 RID: 16672 RVA: 0x00137190 File Offset: 0x00135390
	// (set) Token: 0x06004121 RID: 16673 RVA: 0x00137198 File Offset: 0x00135398
	public Action OnPress { get; private set; }

	// Token: 0x170009E3 RID: 2531
	// (get) Token: 0x06004122 RID: 16674 RVA: 0x001371A1 File Offset: 0x001353A1
	// (set) Token: 0x06004123 RID: 16675 RVA: 0x001371A9 File Offset: 0x001353A9
	public Action OnOver { get; private set; }

	// Token: 0x170009E4 RID: 2532
	// (get) Token: 0x06004124 RID: 16676 RVA: 0x001371B2 File Offset: 0x001353B2
	// (set) Token: 0x06004125 RID: 16677 RVA: 0x001371BA File Offset: 0x001353BA
	public Action OnOut { get; private set; }

	// Token: 0x170009E5 RID: 2533
	// (get) Token: 0x06004126 RID: 16678 RVA: 0x001371C3 File Offset: 0x001353C3
	// (set) Token: 0x06004127 RID: 16679 RVA: 0x001371CB File Offset: 0x001353CB
	public Action OnHide { get; private set; }

	// Token: 0x170009E6 RID: 2534
	// (get) Token: 0x06004128 RID: 16680 RVA: 0x001371D4 File Offset: 0x001353D4
	// (set) Token: 0x06004129 RID: 16681 RVA: 0x001371DC File Offset: 0x001353DC
	public Action OnPressPlusQueue { get; private set; }

	// Token: 0x170009E7 RID: 2535
	// (get) Token: 0x0600412A RID: 16682 RVA: 0x001371E5 File Offset: 0x001353E5
	// (set) Token: 0x0600412B RID: 16683 RVA: 0x001371ED File Offset: 0x001353ED
	public Action OnPressMinusQueue { get; private set; }

	// Token: 0x170009E8 RID: 2536
	// (get) Token: 0x0600412C RID: 16684 RVA: 0x001371F6 File Offset: 0x001353F6
	// (set) Token: 0x0600412D RID: 16685 RVA: 0x001371FE File Offset: 0x001353FE
	public Action OnInfCraftButtonPress { get; private set; }

	// Token: 0x170009E9 RID: 2537
	// (get) Token: 0x0600412E RID: 16686 RVA: 0x00137207 File Offset: 0x00135407
	// (set) Token: 0x0600412F RID: 16687 RVA: 0x0013720F File Offset: 0x0013540F
	public Action<CraftElementBase> OnPressQueueUp { get; private set; }

	// Token: 0x170009EA RID: 2538
	// (get) Token: 0x06004130 RID: 16688 RVA: 0x00137218 File Offset: 0x00135418
	// (set) Token: 0x06004131 RID: 16689 RVA: 0x00137220 File Offset: 0x00135420
	public Action<CraftElementBase> OnPressQueueDown { get; private set; }

	// Token: 0x170009EB RID: 2539
	// (get) Token: 0x06004132 RID: 16690 RVA: 0x00137229 File Offset: 0x00135429
	// (set) Token: 0x06004133 RID: 16691 RVA: 0x00137231 File Offset: 0x00135431
	public Action<CraftElementBase> OnRemoveFromQueuePressed { get; private set; }

	// Token: 0x170009EC RID: 2540
	// (get) Token: 0x06004134 RID: 16692 RVA: 0x0013723A File Offset: 0x0013543A
	// (set) Token: 0x06004135 RID: 16693 RVA: 0x00137242 File Offset: 0x00135442
	public CraftElementBase CraftQueueElement { get; private set; }

	// Token: 0x170009ED RID: 2541
	// (get) Token: 0x06004136 RID: 16694 RVA: 0x0013724B File Offset: 0x0013544B
	// (set) Token: 0x06004137 RID: 16695 RVA: 0x00137253 File Offset: 0x00135453
	public List<CraftElementBase> CraftQueue { get; private set; }

	// Token: 0x170009EE RID: 2542
	// (get) Token: 0x06004138 RID: 16696 RVA: 0x0013725C File Offset: 0x0013545C
	// (set) Token: 0x06004139 RID: 16697 RVA: 0x00137264 File Offset: 0x00135464
	public bool IsMulticraftDisabled { get; private set; }

	// Token: 0x170009EF RID: 2543
	// (get) Token: 0x0600413A RID: 16698 RVA: 0x0013726D File Offset: 0x0013546D
	// (set) Token: 0x0600413B RID: 16699 RVA: 0x00137275 File Offset: 0x00135475
	public WgoData WgoData { get; private set; }

	// Token: 0x0600413C RID: 16700 RVA: 0x00137280 File Offset: 0x00135480
	public UICraftQueueElementWidgetData(WgoData wgoData, CraftElementBase craftQueueElement, List<CraftElementBase> craftQueue, Action onHide, Action onPress, Action onOver, Action onOut, Action<CraftElementBase> onQueueUp, Action<CraftElementBase> onQueueDown, Action<CraftElementBase> onRemoveFromQueuePressed)
	{
		this.OnPressPlusQueue = new Action(this.OnPlus);
		this.OnPressMinusQueue = new Action(this.OnMinus);
		this.OnInfCraftButtonPress = new Action(this.OnInf);
		this.OnPressQueueUp = onQueueUp;
		this.OnPressQueueDown = onQueueDown;
		this.OnRemoveFromQueuePressed = onRemoveFromQueuePressed;
		this.OnPress = onPress;
		this.OnOver = onOver;
		this.OnOut = onOut;
		this.OnHide = onHide;
		this.CraftQueueElement = craftQueueElement;
		this.CraftQueue = craftQueue;
		CraftDef craftDef = craftQueueElement.Def as CraftDef;
		this.IsMulticraftDisabled = craftDef != null && craftDef.IsMultipleCraftsDisabled;
		this.WgoData = wgoData;
	}

	// Token: 0x0600413D RID: 16701 RVA: 0x00137334 File Offset: 0x00135534
	private void OnPlus()
	{
		this.AddCount(1);
	}

	// Token: 0x0600413E RID: 16702 RVA: 0x0013733D File Offset: 0x0013553D
	private void OnMinus()
	{
		this.AddCount(-1);
	}

	// Token: 0x0600413F RID: 16703 RVA: 0x00137348 File Offset: 0x00135548
	public void AddCount(int delta)
	{
		if (delta == 0 || this.CraftQueueElement.IsInfinite)
		{
			return;
		}
		if (delta <= 0)
		{
			this.CraftQueueElement.Count = Math.Max(this.CraftQueueElement.Count + delta, 0);
			return;
		}
		if (this.CraftQueueElement.Count == 999)
		{
			return;
		}
		this.CraftQueueElement.Count = Math.Min(this.CraftQueueElement.Count + delta, 999);
	}

	// Token: 0x06004140 RID: 16704 RVA: 0x001373BE File Offset: 0x001355BE
	private void OnInf()
	{
		this.CraftQueueElement.IsInfinite = !this.CraftQueueElement.IsInfinite;
	}
}
