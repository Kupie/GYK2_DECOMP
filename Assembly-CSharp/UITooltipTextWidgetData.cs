using System;
using LazyBearTechnology;
using TMPro;

// Token: 0x02000857 RID: 2135
public class UITooltipTextWidgetData : LazyWidgetDataBase
{
	// Token: 0x17000822 RID: 2082
	// (get) Token: 0x060036B6 RID: 14006 RVA: 0x00109127 File Offset: 0x00107327
	// (set) Token: 0x060036B7 RID: 14007 RVA: 0x0010912F File Offset: 0x0010732F
	public string Text { get; private set; }

	// Token: 0x17000823 RID: 2083
	// (get) Token: 0x060036B8 RID: 14008 RVA: 0x00109138 File Offset: 0x00107338
	// (set) Token: 0x060036B9 RID: 14009 RVA: 0x00109140 File Offset: 0x00107340
	public TextAlignmentOptions TextAlignmentOptions { get; private set; }

	// Token: 0x17000824 RID: 2084
	// (get) Token: 0x060036BA RID: 14010 RVA: 0x00109149 File Offset: 0x00107349
	// (set) Token: 0x060036BB RID: 14011 RVA: 0x00109151 File Offset: 0x00107351
	public TextStyle TextStyle { get; private set; }

	// Token: 0x060036BC RID: 14012 RVA: 0x0010915A File Offset: 0x0010735A
	public UITooltipTextWidgetData(string text, TextAlignmentOptions textAlignmentOptions, TextStyle textStyle)
	{
		this.Text = text;
		this.TextAlignmentOptions = textAlignmentOptions;
		this.TextStyle = textStyle;
	}
}
