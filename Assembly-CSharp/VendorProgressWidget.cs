using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008D5 RID: 2261
public class VendorProgressWidget : LazyWidget<VendorProgressWidgetData>
{
	// Token: 0x06003AF4 RID: 15092 RVA: 0x00119B70 File Offset: 0x00117D70
	public override void Redraw()
	{
		base.Redraw();
		this.progressBar.value = this.data.Progress;
	}

	// Token: 0x06003AF5 RID: 15093 RVA: 0x00119B90 File Offset: 0x00117D90
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Draw(new VendorProgressWidgetData
		{
			Progress = 0.5f
		});
	}

	// Token: 0x04002E9B RID: 11931
	[SerializeField]
	private Slider progressBar;
}
