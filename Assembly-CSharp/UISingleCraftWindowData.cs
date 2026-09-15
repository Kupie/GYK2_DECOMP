using System;
using System.Collections.Generic;

// Token: 0x02000A3D RID: 2621
public class UISingleCraftWindowData : UIBaseCraftSelectionWindowData
{
	// Token: 0x060046BD RID: 18109 RVA: 0x00133662 File Offset: 0x00131862
	public UISingleCraftWindowData(WgoData wgoData, CraftDef craftDef, Action<CraftDef, List<NeedItemData>, CraftParamsData, int> onAddToQueue, Action<CraftDef, List<NeedItemData>, CraftParamsData, int> onStartCraft)
		: base(wgoData, craftDef, onAddToQueue, onStartCraft)
	{
	}

	// Token: 0x17000AC2 RID: 2754
	// (get) Token: 0x060046BE RID: 18110 RVA: 0x0014EF80 File Offset: 0x0014D180
	protected override int MinCraftsCount
	{
		get
		{
			if (!base.CraftComponent.IsStarted)
			{
				return 0;
			}
			return 1;
		}
	}
}
