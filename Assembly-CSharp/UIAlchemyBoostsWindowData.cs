using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x020008DB RID: 2267
public class UIAlchemyBoostsWindowData : LazyWidgetDataBase
{
	// Token: 0x170008E5 RID: 2277
	// (get) Token: 0x06003B13 RID: 15123 RVA: 0x0011A3E0 File Offset: 0x001185E0
	// (set) Token: 0x06003B14 RID: 15124 RVA: 0x0011A3E8 File Offset: 0x001185E8
	public Wgo AssignedWgo { get; private set; }

	// Token: 0x170008E6 RID: 2278
	// (get) Token: 0x06003B15 RID: 15125 RVA: 0x0011A3F1 File Offset: 0x001185F1
	// (set) Token: 0x06003B16 RID: 15126 RVA: 0x0011A3F9 File Offset: 0x001185F9
	public List<CraftElement> CraftsToDisplay { get; private set; }

	// Token: 0x170008E7 RID: 2279
	// (get) Token: 0x06003B17 RID: 15127 RVA: 0x0011A402 File Offset: 0x00118602
	// (set) Token: 0x06003B18 RID: 15128 RVA: 0x0011A40A File Offset: 0x0011860A
	public Action<CraftElement, List<NeedItemData>> OnCraftPressed { get; private set; }

	// Token: 0x170008E8 RID: 2280
	// (get) Token: 0x06003B19 RID: 15129 RVA: 0x0011A413 File Offset: 0x00118613
	// (set) Token: 0x06003B1A RID: 15130 RVA: 0x0011A41B File Offset: 0x0011861B
	public Func<CraftElement, List<NeedItemData>, bool> CanCraft { get; private set; }

	// Token: 0x170008E9 RID: 2281
	// (get) Token: 0x06003B1B RID: 15131 RVA: 0x0011A424 File Offset: 0x00118624
	// (set) Token: 0x06003B1C RID: 15132 RVA: 0x0011A42C File Offset: 0x0011862C
	public PlayerData PlayerData { get; private set; }

	// Token: 0x170008EA RID: 2282
	// (get) Token: 0x06003B1D RID: 15133 RVA: 0x0011A435 File Offset: 0x00118635
	// (set) Token: 0x06003B1E RID: 15134 RVA: 0x0011A43D File Offset: 0x0011863D
	public List<Inventory> AdditionalInventories { get; private set; }

	// Token: 0x06003B1F RID: 15135 RVA: 0x0011A446 File Offset: 0x00118646
	public UIAlchemyBoostsWindowData(Wgo wgo, PlayerData playerData, List<CraftElement> craftsToDisplay, Action<CraftElement, List<NeedItemData>> onCraftPressed, Func<CraftElement, List<NeedItemData>, bool> canCraft, List<Inventory> additionalInventories = null)
	{
		this.AssignedWgo = wgo;
		this.PlayerData = playerData;
		this.CraftsToDisplay = craftsToDisplay;
		this.OnCraftPressed = onCraftPressed;
		this.CanCraft = canCraft;
		this.AdditionalInventories = additionalInventories;
	}
}
