using System;
using LazyBearTechnology;

// Token: 0x02000A0E RID: 2574
public class UIPrayReportWindowData : LazyWidgetDataBase
{
	// Token: 0x17000A8F RID: 2703
	// (get) Token: 0x0600453F RID: 17727 RVA: 0x00147A71 File Offset: 0x00145C71
	// (set) Token: 0x06004540 RID: 17728 RVA: 0x00147A79 File Offset: 0x00145C79
	private Action OnClosed { get; set; }

	// Token: 0x17000A90 RID: 2704
	// (get) Token: 0x06004541 RID: 17729 RVA: 0x00147A82 File Offset: 0x00145C82
	// (set) Token: 0x06004542 RID: 17730 RVA: 0x00147A8A File Offset: 0x00145C8A
	public SermonResultData SermonResultData { get; private set; }

	// Token: 0x06004543 RID: 17731 RVA: 0x00147A93 File Offset: 0x00145C93
	public UIPrayReportWindowData(SermonResultData sermonResultData, Action onClosed)
	{
		this.SermonResultData = sermonResultData;
		this.OnClosed = onClosed;
	}

	// Token: 0x06004544 RID: 17732 RVA: 0x00147AA9 File Offset: 0x00145CA9
	public void HandleClosingWindow()
	{
		Action onClosed = this.OnClosed;
		if (onClosed == null)
		{
			return;
		}
		onClosed();
	}
}
