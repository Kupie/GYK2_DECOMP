using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000A5B RID: 2651
public class UIDemoEndWindow : LazyWindow<LazyWidgetDataBase>
{
	// Token: 0x06004787 RID: 18311 RVA: 0x0015395C File Offset: 0x00151B5C
	public override void Init()
	{
		base.Init();
		this.leftArrow.onClick.AddListener(new UnityAction(this.OnLeftArrowPressed));
		this.rightArrow.onClick.AddListener(new UnityAction(this.OnRightArrowPressed));
		this.menuButton.onClick.AddListener(new UnityAction(this.OnMenuPressed));
		this.wishList.onClick.AddListener(new UnityAction(this.OnWishListPressed));
		this.menuButton.SetCallbacksIntoGamepadNavigationItem();
		this.wishList.SetCallbacksIntoGamepadNavigationItem();
	}

	// Token: 0x06004788 RID: 18312 RVA: 0x001539F8 File Offset: 0x00151BF8
	public override void Open(LazyWidgetDataBase data)
	{
		base.Open(data);
		foreach (TextMeshProUGUI textMeshProUGUI in this.hints)
		{
			textMeshProUGUI.text = ControllerIconLibrary.GetIconId(GameKey.Action, null, false);
		}
		this.UpdateStoreLabel();
		this.isAutoscrollActive = true;
		this.lastAutoscrollTime = Time.time;
		this.currentLine = 1;
		this.autoScrollDirection = 1;
		this.linePreviewDirection = this.autoScrollDirection;
		this.RefreshLineTargets();
		if (!this.HasLineTargets())
		{
			Debug.LogError("UIDemoEndWindow has no line targets");
			this.UpdateArrowsState(true);
			return;
		}
		this.skinImage.sprite = this.skinPC;
		this.UpdateVisibleLineTargetsForCurrentLine();
		this.movable.anchoredPosition = this.GetCurrentLineTarget().anchoredPosition;
		this.UpdateArrowsState(true);
		if (LazyInput.IsGamepadActive)
		{
			this.PrintTips();
		}
	}

	// Token: 0x06004789 RID: 18313 RVA: 0x00153AF0 File Offset: 0x00151CF0
	private void OnLeftArrowPressed()
	{
		if (!this.leftArrow.interactable)
		{
			return;
		}
		this.isAutoscrollActive = false;
		this.MoveBy(-1);
	}

	// Token: 0x0600478A RID: 18314 RVA: 0x00153B0E File Offset: 0x00151D0E
	private bool OnRightArrowPressedGamepad()
	{
		this.OnRightArrowPressed();
		return true;
	}

	// Token: 0x0600478B RID: 18315 RVA: 0x00153B17 File Offset: 0x00151D17
	private bool OnLeftArrowPressedGamepad()
	{
		this.OnLeftArrowPressed();
		return true;
	}

	// Token: 0x0600478C RID: 18316 RVA: 0x00153B20 File Offset: 0x00151D20
	private void OnRightArrowPressed()
	{
		if (!this.rightArrow.interactable)
		{
			return;
		}
		this.isAutoscrollActive = false;
		this.MoveBy(1);
	}

	// Token: 0x0600478D RID: 18317 RVA: 0x00153B40 File Offset: 0x00151D40
	private void MoveBy(int direction)
	{
		this.RefreshLineTargets();
		if (!this.HasLineTargets())
		{
			return;
		}
		int num = this.currentLine;
		int num2 = Mathf.Clamp(this.currentLine + direction, 1, this.activeLineTargets.Count);
		if (num2 == this.currentLine)
		{
			return;
		}
		this.linePreviewDirection = direction;
		this.SetVisibleLineTargets(num, num2);
		this.currentLine = num2;
		this.MoveTo(this.GetCurrentLineTarget());
	}

	// Token: 0x0600478E RID: 18318 RVA: 0x00153BAC File Offset: 0x00151DAC
	private void MoveTo(RectTransform target)
	{
		this.UpdateArrowsState(false);
		TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = this.movable.DOAnchorPos(target.anchoredPosition, this.moveTime, false);
		tweenerCore.onComplete = (TweenCallback)Delegate.Combine(tweenerCore.onComplete, new TweenCallback(delegate
		{
			this.UpdateVisibleLineTargetsForCurrentLine();
			this.UpdateArrowsState(true);
		}));
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
			this.PrintTips();
		}
	}

	// Token: 0x0600478F RID: 18319 RVA: 0x00153C14 File Offset: 0x00151E14
	private void UpdateArrowsState(bool isInteractable = true)
	{
		this.leftArrow.gameObject.SetActive(true);
		this.rightArrow.gameObject.SetActive(true);
		this.leftArrow.interactable = isInteractable && this.currentLine > 1;
		this.rightArrow.interactable = isInteractable && this.currentLine < this.activeLineTargets.Count;
		if (LazyInput.IsGamepadActive)
		{
			this.PrintTips();
		}
	}

	// Token: 0x06004790 RID: 18320 RVA: 0x00153C8E File Offset: 0x00151E8E
	private void OnMenuPressed()
	{
		this.GoToMenu();
	}

	// Token: 0x06004791 RID: 18321 RVA: 0x00153C96 File Offset: 0x00151E96
	private void GoToMenu()
	{
		this.Close();
		MainGame.Instance.GoToMenu(delegate
		{
			MainGame.PlayerController.MovementComponent.ForceStop();
			MainGame.PlayerController.PlayerLocalAreaMovement.StopMovement(true);
			UICinematic uicinematic = LazyUI.Get<UICinematic>();
			if (uicinematic != null)
			{
				uicinematic.DisableCinematic(null, true);
			}
			UIMultiAnswer.ForceDisableAll();
		}, false, FadeFlag.Common);
	}

	// Token: 0x06004792 RID: 18322 RVA: 0x00153CC9 File Offset: 0x00151EC9
	public void OnWishListPressed()
	{
		UIDemoEndWindow.OpenFullGameInStore();
	}

	// Token: 0x06004793 RID: 18323 RVA: 0x00153CD0 File Offset: 0x00151ED0
	public static void OpenFullGameInStore()
	{
		LazyAPI.Platform.OpenProductInStore(new StoreProductInfo
		{
			steamUrl = "steam://advertise/4358690",
			epicGamesUrl = "",
			gogUrl = "",
			xboxProductId = "9PFZQ5GNM8TJ",
			nintendoApplicationId = "0100005027a18000",
			nintendo2ApplicationId = "0400543027ffc000",
			ps4 = new PlayStationStoreProductInfo
			{
				productLabel = "GRAVEYARDKEEPER2",
				serviceLabel = 1U,
				service = PlayStationStoreService.CommerceCatalogAndEntitlements
			},
			ps5 = new PlayStationStoreProductInfo
			{
				productLabel = "GRAVEYARDKEEPER2",
				serviceLabel = 1U,
				service = PlayStationStoreService.CommerceCatalogAndEntitlements
			}
		});
	}

	// Token: 0x06004794 RID: 18324 RVA: 0x00153D76 File Offset: 0x00151F76
	public void UpdateStoreLabel()
	{
		if (this.storeLabel == null)
		{
			return;
		}
		this.storeLabel.text = LLBase.L(this.GetStoreLocaleId());
	}

	// Token: 0x06004795 RID: 18325 RVA: 0x00153D9D File Offset: 0x00151F9D
	private string GetStoreLocaleId()
	{
		return "store_name_steam";
	}

	// Token: 0x06004796 RID: 18326 RVA: 0x00153DA4 File Offset: 0x00151FA4
	protected override void UpdateGamepadDependentStuff()
	{
		base.UpdateGamepadDependentStuff();
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.SetFocusedItem(this.wishList.GetComponent<GamepadNavigationItem>());
		}
	}

	// Token: 0x06004797 RID: 18327 RVA: 0x00153DCC File Offset: 0x00151FCC
	protected override void PrintTips()
	{
		if (LazyInput.IsGamepadActive)
		{
			List<LazyGameKeyTip> list = new List<LazyGameKeyTip>();
			list.Add(LazyGameKeyTip.Select(true, true, true));
			list.Add(new LazyGameKeyTip(GameKey.DecSlider, "tip_prev", this.leftArrow.interactable, true, true));
			list.Add(new LazyGameKeyTip(GameKey.IncSlider, "tip_next", this.rightArrow.interactable, true, true));
			LazyButtonTipsStr lazyButtonTipsStr;
			if (GUIElements.Instance.UIWindowSizeType == UIWindowSizeType.Big)
			{
				this.bigSizeTips.gameObject.SetActive(true);
				this.smallSizeTips.gameObject.SetActive(false);
				lazyButtonTipsStr = this.bigSizeTips;
			}
			else
			{
				this.bigSizeTips.gameObject.SetActive(false);
				this.smallSizeTips.gameObject.SetActive(true);
				lazyButtonTipsStr = this.smallSizeTips;
			}
			lazyButtonTipsStr.Print(list, (GUIElements.Instance.UIWindowSizeType == UIWindowSizeType.Small) ? "\n" : "  ");
		}
	}

	// Token: 0x06004798 RID: 18328 RVA: 0x00153EBC File Offset: 0x001520BC
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.Right, new Func<bool>(this.OnRightArrowPressedGamepad));
		gameKeyDelegates.Add(GameKey.DpadRight, new Func<bool>(this.OnRightArrowPressedGamepad));
		gameKeyDelegates.Add(GameKey.Left, new Func<bool>(this.OnLeftArrowPressedGamepad));
		gameKeyDelegates.Add(GameKey.DpadLeft, new Func<bool>(this.OnLeftArrowPressedGamepad));
		return gameKeyDelegates;
	}

	// Token: 0x06004799 RID: 18329 RVA: 0x00153F2C File Offset: 0x0015212C
	protected override void Update()
	{
		base.Update();
		if (this.isAutoscrollActive && Time.time - this.timeBetweenAutoscrolls >= this.lastAutoscrollTime)
		{
			this.lastAutoscrollTime = Time.time;
			if (this.autoScrollDirection == 1)
			{
				this.RefreshLineTargets();
				if (this.currentLine < this.activeLineTargets.Count)
				{
					this.MoveBy(1);
					if (this.currentLine == this.activeLineTargets.Count)
					{
						this.autoScrollDirection = -1;
					}
				}
			}
			else if (this.currentLine > 1)
			{
				this.MoveBy(-1);
				if (this.currentLine == 1)
				{
					this.autoScrollDirection = 1;
				}
			}
			if (LazyInput.IsGamepadActive)
			{
				this.PrintTips();
			}
		}
	}

	// Token: 0x0600479A RID: 18330 RVA: 0x00153FDC File Offset: 0x001521DC
	private void RefreshLineTargets()
	{
		this.activeLineTargets.Clear();
		if (this.lineTargets != null && this.lineTargets.Length != 0)
		{
			for (int i = 0; i < this.lineTargets.Length; i++)
			{
				this.AddLineTarget(this.lineTargets[i]);
			}
		}
		this.currentLine = (this.HasLineTargets() ? Mathf.Clamp(this.currentLine, 1, this.activeLineTargets.Count) : 1);
	}

	// Token: 0x0600479B RID: 18331 RVA: 0x0015404E File Offset: 0x0015224E
	private void AddLineTarget(RectTransform target)
	{
		if (target != null)
		{
			this.activeLineTargets.Add(target);
		}
	}

	// Token: 0x0600479C RID: 18332 RVA: 0x00154065 File Offset: 0x00152265
	private RectTransform GetCurrentLineTarget()
	{
		return this.activeLineTargets[this.currentLine - 1];
	}

	// Token: 0x0600479D RID: 18333 RVA: 0x0015407A File Offset: 0x0015227A
	private bool HasLineTargets()
	{
		return this.activeLineTargets.Count > 0;
	}

	// Token: 0x0600479E RID: 18334 RVA: 0x0015408A File Offset: 0x0015228A
	private void UpdateVisibleLineTargetsForCurrentLine()
	{
		if (!this.HasLineTargets())
		{
			return;
		}
		this.SetVisibleLineTargets(this.currentLine, this.GetPreviewLine());
	}

	// Token: 0x0600479F RID: 18335 RVA: 0x001540A8 File Offset: 0x001522A8
	private int GetPreviewLine()
	{
		if (this.activeLineTargets.Count <= 1)
		{
			return -1;
		}
		int num = (this.isAutoscrollActive ? this.autoScrollDirection : this.linePreviewDirection);
		int num2 = this.currentLine + num;
		if (num2 < 1 || num2 > this.activeLineTargets.Count)
		{
			num2 = this.currentLine - num;
		}
		if (num2 != this.currentLine)
		{
			return num2;
		}
		return -1;
	}

	// Token: 0x060047A0 RID: 18336 RVA: 0x0015410C File Offset: 0x0015230C
	private void SetVisibleLineTargets(int firstLine, int secondLine = -1)
	{
		for (int i = 0; i < this.activeLineTargets.Count; i++)
		{
			int num = i + 1;
			this.activeLineTargets[i].gameObject.SetActive(num == firstLine || num == secondLine);
		}
	}

	// Token: 0x060047A1 RID: 18337 RVA: 0x00154154 File Offset: 0x00152354
	[LazyUITest]
	protected override void TestDraw()
	{
		LazyUI.GetWindow<UIDemoEndWindow>().Open(null);
	}

	// Token: 0x040037D4 RID: 14292
	private const string DEMO_COMPLETED_SAVE_ENVIRONMENT_PRESET = "indoor";

	// Token: 0x040037D5 RID: 14293
	[SerializeField]
	private RectTransform movable;

	// Token: 0x040037D6 RID: 14294
	[SerializeField]
	private RectTransform[] lineTargets;

	// Token: 0x040037D7 RID: 14295
	[SerializeField]
	private LazyButton wishList;

	// Token: 0x040037D8 RID: 14296
	[SerializeField]
	private LazyButton menuButton;

	// Token: 0x040037D9 RID: 14297
	[SerializeField]
	private LazyButton leftArrow;

	// Token: 0x040037DA RID: 14298
	[SerializeField]
	private LazyButton rightArrow;

	// Token: 0x040037DB RID: 14299
	[SerializeField]
	private float moveTime = 0.5f;

	// Token: 0x040037DC RID: 14300
	[SerializeField]
	private float timeBetweenAutoscrolls = 3f;

	// Token: 0x040037DD RID: 14301
	[SerializeField]
	private LazyButtonTipsStr bigSizeTips;

	// Token: 0x040037DE RID: 14302
	[SerializeField]
	private LazyButtonTipsStr smallSizeTips;

	// Token: 0x040037DF RID: 14303
	[SerializeField]
	private string demoCompletedSaveGdPointId;

	// Token: 0x040037E0 RID: 14304
	[SerializeField]
	private Image skinImage;

	// Token: 0x040037E1 RID: 14305
	[SerializeField]
	private Sprite skinPC;

	// Token: 0x040037E2 RID: 14306
	[SerializeField]
	private Sprite skinPlaystation;

	// Token: 0x040037E3 RID: 14307
	[SerializeField]
	private Sprite skinXbox;

	// Token: 0x040037E4 RID: 14308
	[SerializeField]
	private Sprite skinSwitch;

	// Token: 0x040037E5 RID: 14309
	[SerializeField]
	private List<TextMeshProUGUI> hints;

	// Token: 0x040037E6 RID: 14310
	[SerializeField]
	private TextMeshProUGUI storeLabel;

	// Token: 0x040037E7 RID: 14311
	private int currentLine;

	// Token: 0x040037E8 RID: 14312
	private bool isAutoscrollActive = true;

	// Token: 0x040037E9 RID: 14313
	private float lastAutoscrollTime;

	// Token: 0x040037EA RID: 14314
	private int autoScrollDirection = 1;

	// Token: 0x040037EB RID: 14315
	private int linePreviewDirection = 1;

	// Token: 0x040037EC RID: 14316
	private readonly List<RectTransform> activeLineTargets = new List<RectTransform>();
}
