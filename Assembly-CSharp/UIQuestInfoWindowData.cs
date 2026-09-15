using System;
using LazyBearTechnology;

// Token: 0x02000A23 RID: 2595
public class UIQuestInfoWindowData : LazyWidgetDataBase
{
	// Token: 0x17000AA4 RID: 2724
	// (get) Token: 0x060045B8 RID: 17848 RVA: 0x00149EF0 File Offset: 0x001480F0
	// (set) Token: 0x060045B9 RID: 17849 RVA: 0x00149EF8 File Offset: 0x001480F8
	public QuestData QuestData { get; private set; }

	// Token: 0x060045BA RID: 17850 RVA: 0x00149F01 File Offset: 0x00148101
	public UIQuestInfoWindowData(QuestData questData)
	{
		this.QuestData = questData;
	}
}
