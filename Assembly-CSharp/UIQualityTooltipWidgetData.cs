using System;
using LazyBearTechnology;

// Token: 0x020007F5 RID: 2037
public class UIQualityTooltipWidgetData : LazyWidgetDataBase
{
	// Token: 0x170007DB RID: 2011
	// (get) Token: 0x0600344A RID: 13386 RVA: 0x000FBD15 File Offset: 0x000F9F15
	// (set) Token: 0x0600344B RID: 13387 RVA: 0x000FBD1D File Offset: 0x000F9F1D
	public string IconName { get; private set; }

	// Token: 0x170007DC RID: 2012
	// (get) Token: 0x0600344C RID: 13388 RVA: 0x000FBD26 File Offset: 0x000F9F26
	// (set) Token: 0x0600344D RID: 13389 RVA: 0x000FBD2E File Offset: 0x000F9F2E
	public WgoData WgoData { get; private set; }

	// Token: 0x170007DD RID: 2013
	// (get) Token: 0x0600344E RID: 13390 RVA: 0x000FBD37 File Offset: 0x000F9F37
	// (set) Token: 0x0600344F RID: 13391 RVA: 0x000FBD3F File Offset: 0x000F9F3F
	public string ValueText { get; private set; }

	// Token: 0x170007DE RID: 2014
	// (get) Token: 0x06003450 RID: 13392 RVA: 0x000FBD48 File Offset: 0x000F9F48
	// (set) Token: 0x06003451 RID: 13393 RVA: 0x000FBD50 File Offset: 0x000F9F50
	public bool HasValueOverride { get; private set; }

	// Token: 0x06003452 RID: 13394 RVA: 0x000FBD59 File Offset: 0x000F9F59
	public UIQualityTooltipWidgetData(string iconName, WgoData wgoData)
	{
		this.IconName = iconName;
		this.WgoData = wgoData;
	}

	// Token: 0x06003453 RID: 13395 RVA: 0x000FBD6F File Offset: 0x000F9F6F
	public UIQualityTooltipWidgetData(string iconName, string valueText)
	{
		this.IconName = iconName;
		this.ValueText = valueText;
		this.HasValueOverride = true;
	}
}
