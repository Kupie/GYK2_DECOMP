using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000970 RID: 2416
public class UIContextMenuWindow : LazyWindow<UIContextMenuWindowData>
{
	// Token: 0x06003FA9 RID: 16297 RVA: 0x00130EA8 File Offset: 0x0012F0A8
	public override void Init()
	{
		base.Init();
		this.pool = LazyPooler.CreatePool<UIContextMenuWindowWidget>(this.contextMenuWindowWidgetPrefab, 0, Pool.PoolType.ImmediateActivation, false, false, null);
		this.contextMenuWindowWidgetPrefab.gameObject.SetActive(false);
		this.backgroundBtn.onClick.AddListener(new UnityAction(this.Close));
		base.GamepadNavigationController.loopVerticalNavigation = true;
	}

	// Token: 0x06003FAA RID: 16298 RVA: 0x00130F0C File Offset: 0x0012F10C
	public override void Redraw()
	{
		base.Redraw();
		foreach (UIContextMenuWindowWidget uicontextMenuWindowWidget in this.widgets)
		{
			this.pool.ReleaseObject<UIContextMenuWindowWidget>(uicontextMenuWindowWidget);
		}
		this.widgets.Clear();
		for (int i = 0; i < this.data.Options.Count; i++)
		{
			UIContextMenuWindowWidget orCreateObject = this.pool.GetOrCreateObject<UIContextMenuWindowWidget>();
			orCreateObject.Draw(this.data.Options[i]);
			orCreateObject.transform.SetParent(this.content);
			orCreateObject.gameObject.SetActive(true);
			orCreateObject.transform.SetSiblingIndex(i);
			this.widgets.Add(orCreateObject);
		}
		this.content.RefreshContentFitter();
		this.content.position = this.data.Position;
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
		}
	}

	// Token: 0x06003FAB RID: 16299 RVA: 0x00131024 File Offset: 0x0012F224
	protected override void PrintTips()
	{
		this.lazyButtonTips.Print(new LazyGameKeyTip[]
		{
			LazyGameKeyTip.Select(true, true, true),
			LazyGameKeyTip.Back(true, true, true)
		});
	}

	// Token: 0x06003FAC RID: 16300 RVA: 0x0013104D File Offset: 0x0012F24D
	public override void Close()
	{
		if (base.IsShown)
		{
			base.Close();
		}
	}

	// Token: 0x06003FAD RID: 16301 RVA: 0x0013105D File Offset: 0x0012F25D
	protected override bool OnPressedBack()
	{
		this.Close();
		return true;
	}

	// Token: 0x06003FAE RID: 16302 RVA: 0x00131068 File Offset: 0x0012F268
	protected override void TestDraw()
	{
		UIContextMenuWindowData uicontextMenuWindowData = new UIContextMenuWindowData();
		uicontextMenuWindowData.Options = new List<UIContextMenuWindowWidgetData>();
		uicontextMenuWindowData.Options.Add(new UIContextMenuWindowWidgetData("test1", delegate
		{
			Debug.Log("test1 pressed");
		}, true));
		uicontextMenuWindowData.Options.Add(new UIContextMenuWindowWidgetData("test2", delegate
		{
			Debug.Log("test2 pressed");
		}, true));
		this.Open(uicontextMenuWindowData);
	}

	// Token: 0x0400321E RID: 12830
	[SerializeField]
	private RectTransform content;

	// Token: 0x0400321F RID: 12831
	[SerializeField]
	private LazyButton backgroundBtn;

	// Token: 0x04003220 RID: 12832
	[SerializeField]
	private UIContextMenuWindowWidget contextMenuWindowWidgetPrefab;

	// Token: 0x04003221 RID: 12833
	private Pool pool;

	// Token: 0x04003222 RID: 12834
	private List<UIContextMenuWindowWidget> widgets = new List<UIContextMenuWindowWidget>();
}
