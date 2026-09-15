using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000A67 RID: 2663
public class UIPreorderWindow : LazyWindow<LazyWidgetDataBase>
{
	// Token: 0x06004832 RID: 18482 RVA: 0x001566E4 File Offset: 0x001548E4
	public override void Init()
	{
		base.Init();
		this.nextButton.onClick.AddListener(new UnityAction(this.ShowNextPage));
		this.prevButton.onClick.AddListener(delegate
		{
			this.ShowPrevPage();
		});
		this.nextButton.SetCallbacksIntoGamepadNavigationItem();
		this.prevButton.SetCallbacksIntoGamepadNavigationItem();
	}

	// Token: 0x06004833 RID: 18483 RVA: 0x00156748 File Offset: 0x00154948
	public override void Open(LazyWidgetDataBase data)
	{
		this.nextButton.gameObject.SetActive(false);
		this.prevButton.gameObject.SetActive(false);
		this.isFirstPage = true;
		this.btnData = new UIDialogWindowData.ButtonData(new Action(this.Close), LLBase.L("btn_ok"), () => true, true, GameKey.Select, "");
		this.skinImage.sprite = this.skinPC;
		base.Open(data);
		this.UpdatePage();
		((RectTransform)base.transform).RefreshContentFitter();
	}

	// Token: 0x06004834 RID: 18484 RVA: 0x001567F8 File Offset: 0x001549F8
	private void ShowNextPage()
	{
		if (!this.isFirstPage)
		{
			return;
		}
		this.isFirstPage = false;
		this.UpdatePage();
	}

	// Token: 0x06004835 RID: 18485 RVA: 0x00156810 File Offset: 0x00154A10
	private bool ShowPrevPage()
	{
		if (this.isFirstPage)
		{
			return false;
		}
		this.isFirstPage = true;
		this.UpdatePage();
		return true;
	}

	// Token: 0x06004836 RID: 18486 RVA: 0x0015682A File Offset: 0x00154A2A
	private void UpdatePage()
	{
		this.page1.SetActive(this.isFirstPage);
		this.page2.SetActive(!this.isFirstPage);
		this.UpdateButtons();
		((RectTransform)base.transform).RefreshContentFitter();
	}

	// Token: 0x06004837 RID: 18487 RVA: 0x00156868 File Offset: 0x00154A68
	private void UpdateButtons()
	{
		this.nextButton.gameObject.SetActive(true);
		this.prevButton.gameObject.SetActive(true);
		this.nextButton.interactable = this.isFirstPage;
		this.prevButton.interactable = !this.isFirstPage;
		this.lazyButton.Draw(this.btnData);
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
			this.PrintTips();
		}
	}

	// Token: 0x06004838 RID: 18488 RVA: 0x001568E8 File Offset: 0x00154AE8
	protected override bool OnPressedBack()
	{
		if (!this.prevButton.gameObject.activeSelf || !this.prevButton.interactable)
		{
			return false;
		}
		if (!this.ShowPrevPage())
		{
			base.OnPressedBack();
		}
		return true;
	}

	// Token: 0x06004839 RID: 18489 RVA: 0x0015691B File Offset: 0x00154B1B
	protected bool OnPressedLeft()
	{
		this.ShowPrevPage();
		return true;
	}

	// Token: 0x0600483A RID: 18490 RVA: 0x00156925 File Offset: 0x00154B25
	protected bool OnPressedRight()
	{
		this.ShowNextPage();
		return true;
	}

	// Token: 0x0600483B RID: 18491 RVA: 0x00156930 File Offset: 0x00154B30
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.Select, () => false);
		gameKeyDelegates.Add(GameKey.DpadRight, new Func<bool>(this.OnPressedRight));
		gameKeyDelegates.Add(GameKey.DpadLeft, new Func<bool>(this.OnPressedLeft));
		gameKeyDelegates.Add(GameKey.Right, new Func<bool>(this.OnPressedRight));
		gameKeyDelegates.Add(GameKey.Left, new Func<bool>(this.OnPressedLeft));
		return gameKeyDelegates;
	}

	// Token: 0x0600483C RID: 18492 RVA: 0x001569CC File Offset: 0x00154BCC
	protected override void PrintTips()
	{
		List<LazyGameKeyTip> list = new List<LazyGameKeyTip>();
		if (this.nextButton.gameObject.activeSelf)
		{
			list.Add(new LazyGameKeyTip(GameKey.DpadRight, "tip_next", this.nextButton.interactable, true, true));
		}
		if (this.prevButton.gameObject.activeSelf)
		{
			list.Add(new LazyGameKeyTip(GameKey.DpadLeft, "tip_prev", this.prevButton.interactable, true, true));
			if (this.prevButton.interactable)
			{
				list.Add(LazyGameKeyTip.Back(true, true, true));
			}
		}
		this.lazyButtonTips.Print(list, "  ");
	}

	// Token: 0x0600483D RID: 18493 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04003856 RID: 14422
	[SerializeField]
	private UIDialogWindowButton lazyButton;

	// Token: 0x04003857 RID: 14423
	[SerializeField]
	private LazyButton nextButton;

	// Token: 0x04003858 RID: 14424
	[SerializeField]
	private LazyButton prevButton;

	// Token: 0x04003859 RID: 14425
	[SerializeField]
	private GameObject page1;

	// Token: 0x0400385A RID: 14426
	[SerializeField]
	private GameObject page2;

	// Token: 0x0400385B RID: 14427
	[SerializeField]
	private Image skinImage;

	// Token: 0x0400385C RID: 14428
	[SerializeField]
	private Sprite skinPC;

	// Token: 0x0400385D RID: 14429
	[SerializeField]
	private Sprite skinPlaystation;

	// Token: 0x0400385E RID: 14430
	[SerializeField]
	private Sprite skinXbox;

	// Token: 0x0400385F RID: 14431
	[SerializeField]
	private Sprite skinSwitch;

	// Token: 0x04003860 RID: 14432
	private UIDialogWindowData.ButtonData btnData;

	// Token: 0x04003861 RID: 14433
	private bool isFirstPage;
}
