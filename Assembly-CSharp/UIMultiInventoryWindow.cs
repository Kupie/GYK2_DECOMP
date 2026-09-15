using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A00 RID: 2560
public class UIMultiInventoryWindow : LazyWindow<UIMultiInventoryWindowData>
{
	// Token: 0x060044FF RID: 17663 RVA: 0x001468F8 File Offset: 0x00144AF8
	public override void Redraw()
	{
		base.Redraw();
		this.multiInventoryWidget.Draw(this.data.MultiInventoryWidgetData);
		((RectTransform)base.transform).RefreshContentFitter();
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
		}
	}

	// Token: 0x06004500 RID: 17664 RVA: 0x00146946 File Offset: 0x00144B46
	public override void Hide()
	{
		base.Hide();
		this.multiInventoryWidget.Hide();
	}

	// Token: 0x06004501 RID: 17665 RVA: 0x00146959 File Offset: 0x00144B59
	protected override void TestDraw()
	{
		this.Open(new UIMultiInventoryWindowData(MainGame.PlayerData, null, (Item _) => false, true, null, null));
	}

	// Token: 0x040035D8 RID: 13784
	[SerializeField]
	private MultiInventoryWidget multiInventoryWidget;
}
