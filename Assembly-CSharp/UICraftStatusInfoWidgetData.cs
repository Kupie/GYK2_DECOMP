using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;

// Token: 0x0200084A RID: 2122
public class UICraftStatusInfoWidgetData : LazyWidgetDataBase
{
	// Token: 0x1700080F RID: 2063
	// (get) Token: 0x0600366B RID: 13931 RVA: 0x001081B4 File Offset: 0x001063B4
	// (set) Token: 0x0600366C RID: 13932 RVA: 0x001081BC File Offset: 0x001063BC
	public string Text { get; private set; }

	// Token: 0x17000810 RID: 2064
	// (get) Token: 0x0600366D RID: 13933 RVA: 0x001081C5 File Offset: 0x001063C5
	// (set) Token: 0x0600366E RID: 13934 RVA: 0x001081CD File Offset: 0x001063CD
	public TextAlignmentOptions TextAlignmentOptions { get; private set; }

	// Token: 0x17000811 RID: 2065
	// (get) Token: 0x0600366F RID: 13935 RVA: 0x001081D6 File Offset: 0x001063D6
	// (set) Token: 0x06003670 RID: 13936 RVA: 0x001081DE File Offset: 0x001063DE
	public TextStyle TextStyle { get; private set; }

	// Token: 0x17000812 RID: 2066
	// (get) Token: 0x06003671 RID: 13937 RVA: 0x001081E7 File Offset: 0x001063E7
	// (set) Token: 0x06003672 RID: 13938 RVA: 0x001081EF File Offset: 0x001063EF
	public Sprite StatusIcon { get; private set; }

	// Token: 0x06003673 RID: 13939 RVA: 0x001081F8 File Offset: 0x001063F8
	public UICraftStatusInfoWidgetData(string text, TextAlignmentOptions textAlignmentOptions, TextStyle textStyle, Sprite statusIcon)
	{
		this.Text = text;
		this.TextAlignmentOptions = textAlignmentOptions;
		this.TextStyle = textStyle;
		this.StatusIcon = statusIcon;
	}
}
