using System;
using System.Collections.Generic;

// Token: 0x0200097C RID: 2428
public class UICraftSelectionWindowData : UIBaseCraftSelectionWindowData
{
	// Token: 0x170009A8 RID: 2472
	// (get) Token: 0x06004033 RID: 16435 RVA: 0x00133651 File Offset: 0x00131851
	// (set) Token: 0x06004034 RID: 16436 RVA: 0x00133659 File Offset: 0x00131859
	public bool IsGravePartRemove { get; set; }

	// Token: 0x06004035 RID: 16437 RVA: 0x00133662 File Offset: 0x00131862
	public UICraftSelectionWindowData(WgoData wgoData, CraftDef craftDef, Action<CraftDef, List<NeedItemData>, CraftParamsData, int> onAddToQueue, Action<CraftDef, List<NeedItemData>, CraftParamsData, int> onStartCraft)
		: base(wgoData, craftDef, onAddToQueue, onStartCraft)
	{
	}
}
