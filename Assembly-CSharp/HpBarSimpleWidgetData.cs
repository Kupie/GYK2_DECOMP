using System;

// Token: 0x020007DF RID: 2015
public class HpBarSimpleWidgetData : HpBarWidgetData
{
	// Token: 0x170007D2 RID: 2002
	// (get) Token: 0x060033F3 RID: 13299 RVA: 0x000FAD14 File Offset: 0x000F8F14
	// (set) Token: 0x060033F4 RID: 13300 RVA: 0x000FAD1C File Offset: 0x000F8F1C
	public float CustomWidth { get; set; } = 30f;

	// Token: 0x170007D3 RID: 2003
	// (get) Token: 0x060033F5 RID: 13301 RVA: 0x000FAD25 File Offset: 0x000F8F25
	// (set) Token: 0x060033F6 RID: 13302 RVA: 0x000FAD2D File Offset: 0x000F8F2D
	public float CustomHeight { get; set; } = 5f;

	// Token: 0x170007D4 RID: 2004
	// (get) Token: 0x060033F7 RID: 13303 RVA: 0x000FAD36 File Offset: 0x000F8F36
	// (set) Token: 0x060033F8 RID: 13304 RVA: 0x000FAD3E File Offset: 0x000F8F3E
	public HpBarSimpleWidgetData.SpriteType Sprite { get; set; } = HpBarSimpleWidgetData.SpriteType.Ally;

	// Token: 0x060033F9 RID: 13305 RVA: 0x000FAD47 File Offset: 0x000F8F47
	public HpBarSimpleWidgetData(HPComponent hpComponent, float customWidth = 30f, float customHeight = 5f, HpBarSimpleWidgetData.SpriteType spriteType = HpBarSimpleWidgetData.SpriteType.Ally)
		: base(hpComponent)
	{
		this.CustomWidth = customWidth;
		this.CustomHeight = customHeight;
		this.Sprite = spriteType;
	}

	// Token: 0x0400296B RID: 10603
	public const float DEFAULT_WIDTH = 30f;

	// Token: 0x0400296C RID: 10604
	public const float DEFAULT_HEIGHT = 5f;

	// Token: 0x020007E0 RID: 2016
	public enum SpriteType
	{
		// Token: 0x04002971 RID: 10609
		Ally = 1,
		// Token: 0x04002972 RID: 10610
		Enemy
	}
}
