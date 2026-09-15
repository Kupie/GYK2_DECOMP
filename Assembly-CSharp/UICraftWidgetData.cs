using System;
using System.Collections.Generic;

// Token: 0x02000999 RID: 2457
public class UICraftWidgetData : UIBaseCraftWidgetData
{
	// Token: 0x0600417C RID: 16764 RVA: 0x00137BF9 File Offset: 0x00135DF9
	public UICraftWidgetData(WgoData wgoData, CraftDef craftDef, Action<CraftDef, List<NeedItemData>, CraftParamsData, int> onPress, Action onOver, Action onOut)
		: base(wgoData, craftDef, onPress, onOver, onOut)
	{
	}

	// Token: 0x0600417D RID: 16765 RVA: 0x00137C08 File Offset: 0x00135E08
	public UICraftWidgetData(WgoData wgoData, AlchemyMixDef mixDef, Action<CraftDef, List<NeedItemData>, CraftParamsData, int> onPress, Action onOver, Action onOut)
		: base(wgoData, mixDef, onPress, onOver, onOut)
	{
	}
}
