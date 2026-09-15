using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009FD RID: 2557
public class UIMapWindow : LazyWindow<MapPageWidgetData>
{
	// Token: 0x17000A82 RID: 2690
	// (get) Token: 0x060044ED RID: 17645 RVA: 0x00146559 File Offset: 0x00144759
	public MapPageWidget MapPageWidget
	{
		get
		{
			return this.mapPageWidget;
		}
	}

	// Token: 0x060044EE RID: 17646 RVA: 0x00146564 File Offset: 0x00144764
	public override void Redraw()
	{
		base.Redraw();
		this.mapPageWidget.Draw(this.data);
		if (GUIElements.Instance.UIWindowSizeType == UIWindowSizeType.Big)
		{
			this.big.gameObject.SetActive(true);
			this.small.gameObject.SetActive(false);
		}
		else
		{
			this.big.gameObject.SetActive(false);
			this.small.gameObject.SetActive(true);
		}
		((RectTransform)base.transform).RefreshContentFitter();
	}

	// Token: 0x060044EF RID: 17647 RVA: 0x001465EA File Offset: 0x001447EA
	public void OnEnterMapMilestone(UIMapMilestone mapMilestone)
	{
		this.mapPageWidget.OnEnterMapMilestone(mapMilestone);
		this.PrintTips();
	}

	// Token: 0x060044F0 RID: 17648 RVA: 0x001465FE File Offset: 0x001447FE
	public void OnExitMapMilestone(UIMapMilestone mapMilestone)
	{
		this.mapPageWidget.OnExitMapMilestone(mapMilestone);
		this.PrintTips();
	}

	// Token: 0x060044F1 RID: 17649 RVA: 0x00146612 File Offset: 0x00144812
	protected override void UpdateGamepadDependentStuff()
	{
		base.UpdateGamepadDependentStuff();
		this.mapPageWidget.UpdateGamepadDependentStuff();
	}

	// Token: 0x060044F2 RID: 17650 RVA: 0x00146625 File Offset: 0x00144825
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.Select, new Func<bool>(this.OnSelectPress));
		return gameKeyDelegates;
	}

	// Token: 0x060044F3 RID: 17651 RVA: 0x00146644 File Offset: 0x00144844
	private bool OnSelectPress()
	{
		if (this.mapPageWidget.CurrentSelected != null)
		{
			this.mapPageWidget.OnPressMapMilestone(this.mapPageWidget.CurrentSelected);
			return true;
		}
		return false;
	}

	// Token: 0x060044F4 RID: 17652 RVA: 0x00146674 File Offset: 0x00144874
	protected override void PrintTips()
	{
		LazyButtonTipsStr lazyButtonTipsStr;
		if (GUIElements.Instance.UIWindowSizeType == UIWindowSizeType.Big)
		{
			this.big.gameObject.SetActive(true);
			this.small.gameObject.SetActive(false);
			lazyButtonTipsStr = this.big;
		}
		else
		{
			this.big.gameObject.SetActive(false);
			this.small.gameObject.SetActive(true);
			lazyButtonTipsStr = this.small;
		}
		if (!LazyInput.IsGamepadActive)
		{
			lazyButtonTipsStr.Clear();
			return;
		}
		if (this.mapPageWidget.CurrentSelected != null)
		{
			lazyButtonTipsStr.Print(new LazyGameKeyTip[]
			{
				LazyGameKeyTip.Select(true, true, true),
				LazyGameKeyTip.Back(true, true, true)
			});
			return;
		}
		lazyButtonTipsStr.Print(LazyGameKeyTip.Back(true, true, true));
	}

	// Token: 0x060044F5 RID: 17653 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x040035CC RID: 13772
	[SerializeField]
	private MapPageWidget mapPageWidget;

	// Token: 0x040035CD RID: 13773
	[SerializeField]
	private LazyButtonTipsStr big;

	// Token: 0x040035CE RID: 13774
	[SerializeField]
	private LazyButtonTipsStr small;
}
