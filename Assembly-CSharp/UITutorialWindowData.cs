using System;
using LazyBearTechnology;

// Token: 0x02000A48 RID: 2632
public class UITutorialWindowData : LazyWidgetDataBase
{
	// Token: 0x17000AC7 RID: 2759
	// (get) Token: 0x060046FA RID: 18170 RVA: 0x0014FD13 File Offset: 0x0014DF13
	// (set) Token: 0x060046FB RID: 18171 RVA: 0x0014FD1B File Offset: 0x0014DF1B
	public string Page { get; set; }

	// Token: 0x17000AC8 RID: 2760
	// (get) Token: 0x060046FC RID: 18172 RVA: 0x0014FD24 File Offset: 0x0014DF24
	// (set) Token: 0x060046FD RID: 18173 RVA: 0x0014FD2C File Offset: 0x0014DF2C
	public Action OnCompleteCallback { get; set; }

	// Token: 0x17000AC9 RID: 2761
	// (get) Token: 0x060046FE RID: 18174 RVA: 0x0014FD35 File Offset: 0x0014DF35
	// (set) Token: 0x060046FF RID: 18175 RVA: 0x0014FD3D File Offset: 0x0014DF3D
	public bool CanCloseFromAnyPage { get; set; }

	// Token: 0x06004700 RID: 18176 RVA: 0x0014FD46 File Offset: 0x0014DF46
	public UITutorialWindowData(string pageId, Action onComplete = null, bool canCloseFromAnyPage = false)
	{
		this.Page = pageId;
		this.OnCompleteCallback = onComplete;
		this.CanCloseFromAnyPage = canCloseFromAnyPage;
	}
}
