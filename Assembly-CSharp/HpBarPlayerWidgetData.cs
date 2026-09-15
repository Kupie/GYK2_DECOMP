using System;

// Token: 0x020007DD RID: 2013
public class HpBarPlayerWidgetData : HpBarSimpleWidgetData
{
	// Token: 0x170007D0 RID: 2000
	// (get) Token: 0x060033EA RID: 13290 RVA: 0x000FAB28 File Offset: 0x000F8D28
	// (set) Token: 0x060033EB RID: 13291 RVA: 0x000FAB30 File Offset: 0x000F8D30
	public new float CustomWidth { get; private set; } = -1f;

	// Token: 0x170007D1 RID: 2001
	// (get) Token: 0x060033EC RID: 13292 RVA: 0x000FAB39 File Offset: 0x000F8D39
	// (set) Token: 0x060033ED RID: 13293 RVA: 0x000FAB41 File Offset: 0x000F8D41
	public new float CustomHeight { get; private set; } = -1f;

	// Token: 0x060033EE RID: 13294 RVA: 0x000FAB4A File Offset: 0x000F8D4A
	public HpBarPlayerWidgetData(HPComponent hpComponent, float customWidth = -1f, float customHeight = -1f)
		: base(hpComponent, 30f, 5f, HpBarSimpleWidgetData.SpriteType.Ally)
	{
		this.CustomWidth = customWidth;
		this.CustomHeight = customHeight;
	}
}
