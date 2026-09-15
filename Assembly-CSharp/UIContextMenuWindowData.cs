using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000972 RID: 2418
public class UIContextMenuWindowData : LazyWidgetDataBase
{
	// Token: 0x17000993 RID: 2451
	// (get) Token: 0x06003FB4 RID: 16308 RVA: 0x0013112E File Offset: 0x0012F32E
	// (set) Token: 0x06003FB5 RID: 16309 RVA: 0x00131136 File Offset: 0x0012F336
	public List<UIContextMenuWindowWidgetData> Options { get; set; }

	// Token: 0x17000994 RID: 2452
	// (get) Token: 0x06003FB6 RID: 16310 RVA: 0x0013113F File Offset: 0x0012F33F
	// (set) Token: 0x06003FB7 RID: 16311 RVA: 0x00131147 File Offset: 0x0012F347
	public Vector2 Position { get; set; }
}
