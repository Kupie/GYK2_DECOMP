using System;
using System.Collections.Generic;
using DG.Tweening;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A43 RID: 2627
public class UITutorialListWindow : LazyWindow<UITutorialListWindowData>
{
	// Token: 0x060046D6 RID: 18134 RVA: 0x0014F284 File Offset: 0x0014D484
	public override void Init()
	{
		base.Init();
		base.GamepadNavigationController.loopVerticalNavigation = true;
		SmoothMouseWheelScroll.EnsureForItem(this.scrollRect, 44f);
	}

	// Token: 0x060046D7 RID: 18135 RVA: 0x0014F2AC File Offset: 0x0014D4AC
	public override void Redraw()
	{
		base.Redraw();
		this.DrawItems();
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
		}
		if (this.scrollRect != null)
		{
			this.scrollRect.DOKill(false);
			this.scrollRect.verticalNormalizedPosition = 1f;
		}
	}

	// Token: 0x060046D8 RID: 18136 RVA: 0x0014F305 File Offset: 0x0014D505
	public override void Close()
	{
		bool flag = ((this.data != null) ? this.data.OpenSource : UITutorialListOpenSource.HUD) == UITutorialListOpenSource.PauseWindow && !this.openingTutorial;
		base.Close();
		if (flag)
		{
			LazyUI.GetWindow<UIGamePauseWindow>().Open(null);
		}
	}

	// Token: 0x060046D9 RID: 18137 RVA: 0x0014F33F File Offset: 0x0014D53F
	public override void Hide()
	{
		this.HideDisplayedItems();
		this.openingTutorial = false;
		base.Hide();
	}

	// Token: 0x060046DA RID: 18138 RVA: 0x0014F354 File Offset: 0x0014D554
	private void DrawItems()
	{
		this.HideDisplayedItems();
		KnowledgeSystem knowledgeSystem = MainGame.Instance.GameSave.knowledgeSystem;
		if (knowledgeSystem.viewedTutorials == null || this.itemsContainer == null || UIPrefabsPooler.Instance == null)
		{
			return;
		}
		foreach (string text in knowledgeSystem.viewedTutorials)
		{
			UITutorialListItemWidget elementFromPool = UIPrefabsPooler.Instance.GetElementFromPool<UITutorialListItemWidget>(this.itemsContainer);
			if (!(elementFromPool == null))
			{
				this.displayedItems.Add(elementFromPool);
				elementFromPool.Init();
				elementFromPool.Draw(new UITutorialListItemWidgetData(text, new Action<string>(this.OpenTutorial)));
			}
		}
	}

	// Token: 0x060046DB RID: 18139 RVA: 0x0014F420 File Offset: 0x0014D620
	private void HideDisplayedItems()
	{
		foreach (UITutorialListItemWidget uitutorialListItemWidget in this.displayedItems)
		{
			uitutorialListItemWidget.DeInit();
			uitutorialListItemWidget.Hide();
			UIPrefabsPooler.Instance.ReleaseElementToPool<UITutorialListItemWidget>(uitutorialListItemWidget);
		}
		this.displayedItems.Clear();
	}

	// Token: 0x060046DC RID: 18140 RVA: 0x0014F490 File Offset: 0x0014D690
	private void OpenTutorial(string tutorialId)
	{
		this.openingTutorial = true;
		this.Close();
		LazyUI.GetWindow<UITutorialWindow>().Open(new UITutorialWindowData(tutorialId, new Action(this.ReturnToTutorialList), true));
	}

	// Token: 0x060046DD RID: 18141 RVA: 0x0014F4BC File Offset: 0x0014D6BC
	private void ReturnToTutorialList()
	{
		LazyUI.GetWindow<UITutorialListWindow>().Open(this.data);
	}

	// Token: 0x060046DE RID: 18142 RVA: 0x0014F4CE File Offset: 0x0014D6CE
	protected override void PrintTips()
	{
		LazyButtonTipsStr lazyButtonTips = this.lazyButtonTips;
		if (lazyButtonTips == null)
		{
			return;
		}
		lazyButtonTips.Print(new LazyGameKeyTip[]
		{
			LazyGameKeyTip.Select(true, true, true),
			LazyGameKeyTip.Back(true, true, true)
		});
	}

	// Token: 0x060046DF RID: 18143 RVA: 0x0014F4FC File Offset: 0x0014D6FC
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Open(new UITutorialListWindowData(UITutorialListOpenSource.HUD));
	}

	// Token: 0x04003745 RID: 14149
	[SerializeField]
	private Transform itemsContainer;

	// Token: 0x04003746 RID: 14150
	[SerializeField]
	private ScrollRect scrollRect;

	// Token: 0x04003747 RID: 14151
	private readonly List<UITutorialListItemWidget> displayedItems = new List<UITutorialListItemWidget>();

	// Token: 0x04003748 RID: 14152
	private bool openingTutorial;
}
