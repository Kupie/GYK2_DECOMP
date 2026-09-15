using System;
using LazyBearTechnology;

// Token: 0x02000A42 RID: 2626
public class UITutorialListItemWidgetData : LazyWidgetDataBase
{
	// Token: 0x17000AC4 RID: 2756
	// (get) Token: 0x060046D3 RID: 18131 RVA: 0x0014F25E File Offset: 0x0014D45E
	public string TutorialId { get; }

	// Token: 0x17000AC5 RID: 2757
	// (get) Token: 0x060046D4 RID: 18132 RVA: 0x0014F266 File Offset: 0x0014D466
	public Action<string> OnPressed { get; }

	// Token: 0x060046D5 RID: 18133 RVA: 0x0014F26E File Offset: 0x0014D46E
	public UITutorialListItemWidgetData(string tutorialId, Action<string> onPressed)
	{
		this.TutorialId = tutorialId;
		this.OnPressed = onPressed;
	}
}
