using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200089C RID: 2204
public class UIMagnifyingGlassWidgetData : LazyWidgetDataBase
{
	// Token: 0x1700086A RID: 2154
	// (get) Token: 0x060038C4 RID: 14532 RVA: 0x00110D20 File Offset: 0x0010EF20
	// (set) Token: 0x060038C5 RID: 14533 RVA: 0x00110D28 File Offset: 0x0010EF28
	public SGuid UniqueId { get; private set; }

	// Token: 0x1700086B RID: 2155
	// (get) Token: 0x060038C6 RID: 14534 RVA: 0x00110D31 File Offset: 0x0010EF31
	// (set) Token: 0x060038C7 RID: 14535 RVA: 0x00110D39 File Offset: 0x0010EF39
	public Vector2 DirectionToTarget { get; set; }

	// Token: 0x1700086C RID: 2156
	// (get) Token: 0x060038C8 RID: 14536 RVA: 0x00110D42 File Offset: 0x0010EF42
	// (set) Token: 0x060038C9 RID: 14537 RVA: 0x00110D4A File Offset: 0x0010EF4A
	public Vector2 ScreenPosition { get; set; }

	// Token: 0x1700086D RID: 2157
	// (get) Token: 0x060038CA RID: 14538 RVA: 0x00110D53 File Offset: 0x0010EF53
	// (set) Token: 0x060038CB RID: 14539 RVA: 0x00110D5B File Offset: 0x0010EF5B
	public bool IsOutOfScreen { get; set; }

	// Token: 0x060038CC RID: 14540 RVA: 0x00110D64 File Offset: 0x0010EF64
	public UIMagnifyingGlassWidgetData(SGuid uniqueId, Vector2 directionToTarget, Vector2 screenPosition)
	{
		this.UniqueId = uniqueId;
		this.DirectionToTarget = directionToTarget;
		this.ScreenPosition = screenPosition;
	}
}
