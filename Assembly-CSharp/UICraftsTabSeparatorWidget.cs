using System;
using LazyBearTechnology;

// Token: 0x02000992 RID: 2450
public class UICraftsTabSeparatorWidget : LazyWidget<UICraftsTabSeparatorWidgetData>
{
	// Token: 0x0600415B RID: 16731 RVA: 0x0013762E File Offset: 0x0013582E
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Draw(new UICraftsTabSeparatorWidgetData());
	}
}
