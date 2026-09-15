using System;
using LazyBearTechnology;

// Token: 0x02000A07 RID: 2567
public class UINotesWindowData : LazyWidgetDataBase
{
	// Token: 0x17000A8A RID: 2698
	// (get) Token: 0x0600451C RID: 17692 RVA: 0x00146F0A File Offset: 0x0014510A
	// (set) Token: 0x0600451D RID: 17693 RVA: 0x00146F12 File Offset: 0x00145112
	public ItemDef NoteItem { get; private set; }

	// Token: 0x0600451E RID: 17694 RVA: 0x00146F1B File Offset: 0x0014511B
	public UINotesWindowData(ItemDef itemDef)
	{
		this.NoteItem = itemDef;
	}
}
