using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000827 RID: 2087
public interface IBubbleDrawable
{
	// Token: 0x170007FB RID: 2043
	// (get) Token: 0x06003570 RID: 13680
	SGuid BubbleDrawableUniqueId { get; }

	// Token: 0x170007FC RID: 2044
	// (get) Token: 0x06003571 RID: 13681
	List<LazyWidgetDataBase> BubbleDrawableWidgets { get; }

	// Token: 0x170007FD RID: 2045
	// (get) Token: 0x06003572 RID: 13682
	Vector3 BubbleDrawablePosition { get; }
}
