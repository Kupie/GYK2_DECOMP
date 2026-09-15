using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009E6 RID: 2534
public class UIHotBarSelectionWindow : LazyWindow<UIHotBarSelectionWindowData>
{
	// Token: 0x0600440A RID: 17418 RVA: 0x00143825 File Offset: 0x00141A25
	protected override void ShowWindow()
	{
		base.ShowWindow();
		LazyInput.OnInputChanged += this.Close;
	}

	// Token: 0x0600440B RID: 17419 RVA: 0x00143840 File Offset: 0x00141A40
	public override void Redraw()
	{
		base.Redraw();
		this.itemKeyboard.Draw(this.data.Item, false, -1, false, 1, false, 0, true, false, false, ItemRelatedWidgetState.NotSet, false);
		this.itemGamepad.Draw(this.data.Item, false, -1, false, 1, false, 0, true, false, false, ItemRelatedWidgetState.NotSet, false);
		this.itemKeyboard.ClearCallbacks();
		this.itemGamepad.ClearCallbacks();
		this.hotBarWidget.Draw(this.data.HotBarWidgetData);
		this.UpdateHotBarPos();
		((RectTransform)base.transform).RefreshContentFitter();
	}

	// Token: 0x0600440C RID: 17420 RVA: 0x001438D7 File Offset: 0x00141AD7
	public override void Close()
	{
		base.Close();
		LazyInput.OnInputChanged -= this.Close;
		this.hotBarWidget.Hide();
	}

	// Token: 0x0600440D RID: 17421 RVA: 0x001438FC File Offset: 0x00141AFC
	protected override void Update()
	{
		base.Update();
		if (!LazyInput.IsGamepadActive)
		{
			if (LazyInput.GetKeyDown(GameKey.UseHotBarItem1))
			{
				this.SetSelectItemToHotBarAtIndex(0);
				this.Close();
				return;
			}
			if (LazyInput.GetKeyDown(GameKey.UseHotBarItem2))
			{
				this.SetSelectItemToHotBarAtIndex(1);
				this.Close();
				return;
			}
			if (LazyInput.GetKeyDown(GameKey.UseHotBarItem3))
			{
				this.SetSelectItemToHotBarAtIndex(2);
				this.Close();
				return;
			}
			if (LazyInput.GetKeyDown(GameKey.UseHotBarItem4))
			{
				this.SetSelectItemToHotBarAtIndex(3);
				this.Close();
				return;
			}
		}
	}

	// Token: 0x0600440E RID: 17422 RVA: 0x00143982 File Offset: 0x00141B82
	protected override bool OnPressedBack()
	{
		this.Close();
		return true;
	}

	// Token: 0x0600440F RID: 17423 RVA: 0x0014398C File Offset: 0x00141B8C
	private void UpdateHotBarPos()
	{
		if (LazyInput.IsGamepadActive)
		{
			this.hotBarWidget.transform.SetParent(this.hotBarGamepadPos);
			this.hotBarGamepadPos.gameObject.SetActive(true);
			this.hotBarKeyboardPos.gameObject.SetActive(false);
		}
		else
		{
			this.hotBarWidget.transform.SetParent(this.hotBarKeyboardPos);
			this.hotBarGamepadPos.gameObject.SetActive(false);
			this.hotBarKeyboardPos.gameObject.SetActive(true);
		}
		((RectTransform)this.hotBarWidget.transform).anchoredPosition = Vector3.zero;
	}

	// Token: 0x06004410 RID: 17424 RVA: 0x00143A31 File Offset: 0x00141C31
	private bool SetSelectItemToHotBarAtIndex(int index)
	{
		MainGame.PlayerData.SetHotBarItemAtIndex(this.data.Item.id, index);
		this.Close();
		return true;
	}

	// Token: 0x06004411 RID: 17425 RVA: 0x00143A58 File Offset: 0x00141C58
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.UseHotBarItem1, () => this.SetSelectItemToHotBarAtIndex(0));
		gameKeyDelegates.Add(GameKey.UseHotBarItem2, () => this.SetSelectItemToHotBarAtIndex(1));
		gameKeyDelegates.Add(GameKey.UseHotBarItem3, () => this.SetSelectItemToHotBarAtIndex(2));
		gameKeyDelegates.Add(GameKey.UseHotBarItem4, () => this.SetSelectItemToHotBarAtIndex(3));
		return gameKeyDelegates;
	}

	// Token: 0x06004412 RID: 17426 RVA: 0x00143AC7 File Offset: 0x00141CC7
	protected override void PrintTips()
	{
		this.lazyButtonTips.Clear();
	}

	// Token: 0x06004413 RID: 17427 RVA: 0x00143AD4 File Offset: 0x00141CD4
	protected override void TestDraw()
	{
		this.Open(new UIHotBarSelectionWindowData(MainGame.Instance.GameSave, new Item("beer", 1)));
	}

	// Token: 0x04003516 RID: 13590
	[SerializeField]
	private UIHotBarWidget hotBarWidget;

	// Token: 0x04003517 RID: 13591
	[SerializeField]
	private Transform hotBarKeyboardPos;

	// Token: 0x04003518 RID: 13592
	[SerializeField]
	private Transform hotBarGamepadPos;

	// Token: 0x04003519 RID: 13593
	[SerializeField]
	private UIItemCell itemKeyboard;

	// Token: 0x0400351A RID: 13594
	[SerializeField]
	private UIItemCell itemGamepad;
}
