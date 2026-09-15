using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020007D5 RID: 2005
public class CinematicsTextWidgetData : LazyWidgetDataBase
{
	// Token: 0x170007C0 RID: 1984
	// (get) Token: 0x06003397 RID: 13207 RVA: 0x000F92DD File Offset: 0x000F74DD
	// (set) Token: 0x06003398 RID: 13208 RVA: 0x000F92E5 File Offset: 0x000F74E5
	public Action<string> OnTextChanged { get; set; }

	// Token: 0x170007C1 RID: 1985
	// (get) Token: 0x06003399 RID: 13209 RVA: 0x000F92EE File Offset: 0x000F74EE
	// (set) Token: 0x0600339A RID: 13210 RVA: 0x000F92F6 File Offset: 0x000F74F6
	public Action<bool> OnVisibleChanged { get; set; }

	// Token: 0x170007C2 RID: 1986
	// (get) Token: 0x0600339B RID: 13211 RVA: 0x000F92FF File Offset: 0x000F74FF
	// (set) Token: 0x0600339C RID: 13212 RVA: 0x000F9307 File Offset: 0x000F7507
	public Action<Color> OnColorChanged { get; set; }

	// Token: 0x170007C3 RID: 1987
	// (get) Token: 0x0600339D RID: 13213 RVA: 0x000F9310 File Offset: 0x000F7510
	// (set) Token: 0x0600339E RID: 13214 RVA: 0x000F9318 File Offset: 0x000F7518
	public Vector2 Position { get; set; }

	// Token: 0x170007C4 RID: 1988
	// (get) Token: 0x0600339F RID: 13215 RVA: 0x000F9321 File Offset: 0x000F7521
	// (set) Token: 0x060033A0 RID: 13216 RVA: 0x000F9329 File Offset: 0x000F7529
	public Vector2 Size { get; set; }

	// Token: 0x170007C5 RID: 1989
	// (get) Token: 0x060033A1 RID: 13217 RVA: 0x000F9332 File Offset: 0x000F7532
	// (set) Token: 0x060033A2 RID: 13218 RVA: 0x000F933A File Offset: 0x000F753A
	public Color Color { get; set; } = Color.white;

	// Token: 0x170007C6 RID: 1990
	// (get) Token: 0x060033A3 RID: 13219 RVA: 0x000F9343 File Offset: 0x000F7543
	// (set) Token: 0x060033A4 RID: 13220 RVA: 0x000F934B File Offset: 0x000F754B
	public Vector2 Pivot { get; set; } = new Vector2(0.5f, 0.5f);

	// Token: 0x170007C7 RID: 1991
	// (get) Token: 0x060033A5 RID: 13221 RVA: 0x000F9354 File Offset: 0x000F7554
	// (set) Token: 0x060033A6 RID: 13222 RVA: 0x000F935C File Offset: 0x000F755C
	public bool ShowBackground { get; set; }

	// Token: 0x060033A7 RID: 13223 RVA: 0x000F9365 File Offset: 0x000F7565
	public CinematicsTextWidgetData(Vector2 position, Vector2 size)
	{
		this.Position = position;
		this.Size = size;
	}
}
