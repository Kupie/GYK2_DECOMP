using System;
using LazyBearTechnology;

// Token: 0x020009B6 RID: 2486
public class DialogInputWindowData : LazyWidgetDataBase
{
	// Token: 0x17000A08 RID: 2568
	// (get) Token: 0x06004239 RID: 16953 RVA: 0x0013AB4C File Offset: 0x00138D4C
	// (set) Token: 0x0600423A RID: 16954 RVA: 0x0013AB54 File Offset: 0x00138D54
	public Action<string, float> OnButtonPressed { get; private set; }

	// Token: 0x17000A09 RID: 2569
	// (get) Token: 0x0600423B RID: 16955 RVA: 0x0013AB5D File Offset: 0x00138D5D
	// (set) Token: 0x0600423C RID: 16956 RVA: 0x0013AB65 File Offset: 0x00138D65
	public string HeaderText { get; private set; }

	// Token: 0x17000A0A RID: 2570
	// (get) Token: 0x0600423D RID: 16957 RVA: 0x0013AB6E File Offset: 0x00138D6E
	// (set) Token: 0x0600423E RID: 16958 RVA: 0x0013AB76 File Offset: 0x00138D76
	public string ButtonText { get; private set; }

	// Token: 0x0600423F RID: 16959 RVA: 0x0013AB7F File Offset: 0x00138D7F
	public DialogInputWindowData(string headerText, string buttonText, Action<string, float> onButtonPressed)
	{
		this.HeaderText = headerText;
		this.ButtonText = buttonText;
		this.OnButtonPressed = onButtonPressed;
	}
}
