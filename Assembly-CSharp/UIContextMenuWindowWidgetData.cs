using System;
using LazyBearTechnology;

// Token: 0x02000974 RID: 2420
public class UIContextMenuWindowWidgetData : LazyWidgetDataBase
{
	// Token: 0x06003FC1 RID: 16321 RVA: 0x0013123C File Offset: 0x0012F43C
	public UIContextMenuWindowWidgetData(string name, Action callback, bool enabled = true)
	{
		this.name = name;
		this.callback = callback;
		this.enabled = enabled;
	}

	// Token: 0x0400322B RID: 12843
	public string name;

	// Token: 0x0400322C RID: 12844
	public Action callback;

	// Token: 0x0400322D RID: 12845
	public bool enabled;
}
