using System;
using LazyBearTechnology;

// Token: 0x020007EF RID: 2031
public class UIInteractionHintRowWidgetData : LazyWidgetDataBase
{
	// Token: 0x170007D8 RID: 2008
	// (get) Token: 0x06003433 RID: 13363 RVA: 0x000FBADA File Offset: 0x000F9CDA
	// (set) Token: 0x06003434 RID: 13364 RVA: 0x000FBAE2 File Offset: 0x000F9CE2
	public InteractionInfo InteractionInfo { get; private set; }

	// Token: 0x06003435 RID: 13365 RVA: 0x000FBAEB File Offset: 0x000F9CEB
	public UIInteractionHintRowWidgetData(InteractionInfo interactionInfo)
	{
		this.InteractionInfo = interactionInfo;
	}
}
