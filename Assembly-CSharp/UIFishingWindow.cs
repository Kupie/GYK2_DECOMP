using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009D4 RID: 2516
public class UIFishingWindow : LazyWindow<UIFishingWindowData>
{
	// Token: 0x17000A34 RID: 2612
	// (get) Token: 0x06004325 RID: 17189 RVA: 0x0013EECB File Offset: 0x0013D0CB
	private bool IsFishingMiniGameActive
	{
		get
		{
			return this.fishingComponent != null && this.fishingComponent.IsMiniGameActive;
		}
	}

	// Token: 0x17000A35 RID: 2613
	// (get) Token: 0x06004326 RID: 17190 RVA: 0x0013EEE8 File Offset: 0x0013D0E8
	public bool IsBaitSelectionVisible
	{
		get
		{
			return this.baitSelection != null && this.baitSelection.gameObject.activeInHierarchy;
		}
	}

	// Token: 0x06004327 RID: 17191 RVA: 0x0013EF0A File Offset: 0x0013D10A
	public void OnDestroy()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x06004328 RID: 17192 RVA: 0x0013EF12 File Offset: 0x0013D112
	public override void Init()
	{
		base.Init();
		this.leftButton.onClick.AddListener(delegate
		{
			this.SelectNextItem(true);
		});
		this.rightButton.onClick.AddListener(delegate
		{
			this.SelectNextItem(false);
		});
	}

	// Token: 0x06004329 RID: 17193 RVA: 0x0013EF54 File Offset: 0x0013D154
	public override void Open(UIFishingWindowData data)
	{
		if (GUIElements.Instance.UIWindowSizeType == UIWindowSizeType.Big)
		{
			this.baitSelection.anchoredPosition = this.posBig;
		}
		else
		{
			this.baitSelection.anchoredPosition = this.posSmall;
		}
		this.selectionPointer = 0;
		this.baitSelection.gameObject.SetActive(true);
		this.fishingComponent = MainGame.PlayerController.FishingComponent;
		if (this.fishingComponent != null)
		{
			this.fishingComponent.StartActivity(data.Reservoir);
		}
		else
		{
			Debug.LogError("[UIFishingWindow] Can't start fishing. No PlayerFishingComponent found");
		}
		for (int i = 0; i < data.ReservoirFishings.Count; i++)
		{
			UIPossibleFishCell elementFromPool = UIPrefabsPooler.Instance.GetElementFromPool<UIPossibleFishCell>(this.possibleFishesParent);
			this.possibleFishes.Add(elementFromPool);
		}
		base.Open(data);
		this.SetWindowVisible(true);
		((RectTransform)base.transform).RefreshContentFitter();
	}

	// Token: 0x0600432A RID: 17194 RVA: 0x0013F038 File Offset: 0x0013D238
	public override void Close()
	{
		this.selectionPointer = 0;
		base.Close();
		foreach (UIPossibleFishCell uipossibleFishCell in this.possibleFishes)
		{
			UIPrefabsPooler.Instance.ReleaseElementToPool<UIPossibleFishCell>(uipossibleFishCell);
		}
		this.possibleFishes.Clear();
	}

	// Token: 0x0600432B RID: 17195 RVA: 0x0013F0A8 File Offset: 0x0013D2A8
	public override void Redraw()
	{
		base.Redraw();
		if (!this.IsFishingMiniGameActive && this.baitSelection != null && this.baitSelection.gameObject.activeInHierarchy && this.HasSelectableBait())
		{
			this.RedrawInternal();
		}
	}

	// Token: 0x0600432C RID: 17196 RVA: 0x0013F0E6 File Offset: 0x0013D2E6
	protected override void InitCloseButton(LazyButton button)
	{
		button.onClick.AddListener(delegate
		{
			this.Close();
			PlayerController playerController = MainGame.PlayerController;
			if (playerController == null)
			{
				return;
			}
			playerController.FishingComponent.StopActivity(false, null);
		});
	}

	// Token: 0x0600432D RID: 17197 RVA: 0x0013F100 File Offset: 0x0013D300
	private void RedrawInternal()
	{
		if (!this.HasSelectableBait())
		{
			return;
		}
		Item currentBait = this.data.BaitItems[this.selectionPointer];
		if (currentBait != null && this.data.ReservoirFishings != null)
		{
			Wgo reservoir = this.data.Reservoir;
			if (((reservoir != null) ? reservoir.Data : null) != null)
			{
				this.leftButton.interactable = this.data.BaitItems.Count > 1 && this.selectionPointer > 0;
				this.rightButton.interactable = this.data.BaitItems.Count > 1 && this.selectionPointer + 1 < this.data.BaitItems.Count;
				this.header.text = LLBase.L(currentBait.id);
				int count = currentBait.Count;
				int num = 1;
				if (currentBait.id == "no_bait")
				{
					this.itemCell.Draw(currentBait, false, -1, false, 1, false, 0, true, false, false, ItemRelatedWidgetState.NotSet, false);
				}
				else
				{
					this.itemCell.Draw(new Item(currentBait.id, num), true, count, false, 1, false, 0, true, false, false, ItemRelatedWidgetState.NotSet, false);
				}
				bool anyCatchableFish = false;
				int num2 = Mathf.Min(this.data.ReservoirFishings.Count, this.possibleFishes.Count);
				for (int i = 0; i < num2; i++)
				{
					FishingDef fishingDef = this.data.ReservoirFishings[i];
					UIPossibleFishCell uipossibleFishCell = this.possibleFishes[i];
					bool flag = false;
					for (int j = 0; j < fishingDef.baitMod.List.Count; j++)
					{
						if (fishingDef.baitMod.List[j].type == currentBait.id)
						{
							flag = true;
						}
					}
					if (this.data.Reservoir.Data.GetGameRes(fishingDef.fishId + "_caught") > 0f)
					{
						if (flag && this.data.Reservoir.Data.GetGameRes(fishingDef.fishId) > 0f)
						{
							uipossibleFishCell.Draw(new Item(fishingDef.fishId, 1));
							anyCatchableFish = true;
						}
						else
						{
							uipossibleFishCell.DrawInactive(new Item(fishingDef.fishId, 1));
						}
					}
					else if (flag && this.data.Reservoir.Data.GetGameRes(fishingDef.fishId) > 0f)
					{
						uipossibleFishCell.DrawLocked();
						anyCatchableFish = true;
					}
					else
					{
						uipossibleFishCell.DrawLockedInactive();
					}
				}
				this.submutButton.Draw(new UIDialogWindowData.ButtonData(new Action(this.OnSubmitPressed), LLBase.L("ui_submit_bait"), () => anyCatchableFish && (currentBait.id == "no_bait" || currentBait.Count > 0), true, GameKey.SubmitBait, ""));
				this.UpdateGamepadDependentStuff();
				return;
			}
		}
	}

	// Token: 0x0600432E RID: 17198 RVA: 0x0013F40A File Offset: 0x0013D60A
	public void SetWindowVisible(bool isVisible)
	{
		this.contentParent.SetActive(isVisible);
		if (isVisible && LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
		}
		this.UpdateGamepadDependentStuff();
	}

	// Token: 0x0600432F RID: 17199 RVA: 0x0013F438 File Offset: 0x0013D638
	public void DisplayChoicePanel()
	{
		this.UpdateGamepadDependentStuff();
		string currentBaitId = this.GetCurrentBaitId();
		this.data.UpdateAvailableFishingAndBaits();
		this.selectionPointer = this.FindBaitSelectionIndex(currentBaitId);
		this.baitSelection.gameObject.SetActive(true);
		this.RedrawInternal();
	}

	// Token: 0x06004330 RID: 17200 RVA: 0x0013F484 File Offset: 0x0013D684
	private string GetCurrentBaitId()
	{
		UIFishingWindowData data = this.data;
		if (((data != null) ? data.BaitItems : null) == null || this.selectionPointer < 0 || this.selectionPointer >= this.data.BaitItems.Count)
		{
			return null;
		}
		return this.data.BaitItems[this.selectionPointer].id;
	}

	// Token: 0x06004331 RID: 17201 RVA: 0x0013F4E4 File Offset: 0x0013D6E4
	private int FindBaitSelectionIndex(string baitId)
	{
		if (string.IsNullOrEmpty(baitId) || this.data.BaitItems == null)
		{
			return 0;
		}
		for (int i = 0; i < this.data.BaitItems.Count; i++)
		{
			Item item = this.data.BaitItems[i];
			if (item.id == baitId && (item.id == "no_bait" || item.Count > 0))
			{
				return i;
			}
		}
		return 0;
	}

	// Token: 0x06004332 RID: 17202 RVA: 0x0013F564 File Offset: 0x0013D764
	private bool SelectNextItem(bool reverseDirection = false)
	{
		if (this.baitSelection == null || !this.baitSelection.gameObject.activeInHierarchy)
		{
			return false;
		}
		UIFishingWindowData data = this.data;
		if (((data != null) ? data.BaitItems : null) == null)
		{
			Debug.LogError("[UIFishingWindow]: baitItems is null");
			return false;
		}
		if (!this.CanSelectNextItem(reverseDirection))
		{
			return false;
		}
		if (!reverseDirection)
		{
			this.selectionPointer++;
		}
		else
		{
			this.selectionPointer--;
		}
		this.Redraw();
		this.UpdateGamepadDependentStuff();
		return true;
	}

	// Token: 0x06004333 RID: 17203 RVA: 0x0013F5EC File Offset: 0x0013D7EC
	private bool CanSelectNextItem(bool reverseDirection = false)
	{
		if (!(this.baitSelection == null) && this.baitSelection.gameObject.activeInHierarchy)
		{
			UIFishingWindowData data = this.data;
			if (((data != null) ? data.BaitItems : null) != null && this.data.BaitItems.Count > 1)
			{
				if (!reverseDirection)
				{
					return this.selectionPointer + 1 < this.data.BaitItems.Count;
				}
				return this.selectionPointer > 0;
			}
		}
		return false;
	}

	// Token: 0x06004334 RID: 17204 RVA: 0x0013F669 File Offset: 0x0013D869
	private bool HasSelectableBait()
	{
		UIFishingWindowData data = this.data;
		return ((data != null) ? data.BaitItems : null) != null && this.selectionPointer >= 0 && this.selectionPointer < this.data.BaitItems.Count;
	}

	// Token: 0x06004335 RID: 17205 RVA: 0x0013F6A2 File Offset: 0x0013D8A2
	private void OnSubmitPressed()
	{
		this.OnSubmitBait();
	}

	// Token: 0x06004336 RID: 17206 RVA: 0x0013F6AC File Offset: 0x0013D8AC
	private bool OnSubmitBait()
	{
		if (!(this.fishingComponent == null) && !this.IsFishingMiniGameActive && this.HasSelectableBait())
		{
			UIFishingWindowData data = this.data;
			bool flag;
			if (data == null)
			{
				flag = null != null;
			}
			else
			{
				Wgo reservoir = data.Reservoir;
				flag = ((reservoir != null) ? reservoir.Data : null) != null;
			}
			if (flag)
			{
				Item item = this.data.BaitItems[this.selectionPointer];
				List<FishingDef> list = GameBalance.Me.fishingDefs.FindAll((FishingDef x) => x.reservoirId == this.data.Reservoir.Data.id);
				List<float> list2 = new List<float>();
				if (list.Count == 0)
				{
					Debug.LogError("[UIFishingWindow] Can't start fishing. No FishingDef found for [" + this.data.Reservoir.Data.id + "] wgoData");
					return false;
				}
				float num = 0f;
				for (int i = 0; i < list.Count; i++)
				{
					float num2 = 0f;
					if (item != null && list[i].baitMod.Get(item.id, 0f) != 0f)
					{
						num2 = list[i].baitMod.Get(item.id, 0f);
					}
					else if (item == null)
					{
						num2 = list[i].baitMod.Get("no_bait", 0f);
					}
					float dayTimeMod = list[i].GetDayTimeMod();
					float num3 = (float)this.data.Reservoir.Data.GetGameResInt(list[i].fishId);
					float num4 = (float)list[i].baseWeight * num2 * dayTimeMod * num3;
					list2.Add(num4);
					num += num4;
				}
				float num5 = 0f;
				float num6 = global::UnityEngine.Random.Range(0f, num);
				for (int j = 0; j < list2.Count; j++)
				{
					num5 += list2[j];
					if (num6 <= num5)
					{
						if (item != null && item.id != "no_bait")
						{
							MainGame.PlayerData.inventory.RemoveItemById(item.id, 1, null, null, false);
						}
						this.baitSelection.gameObject.SetActive(false);
						this.fishingComponent.RunMiniGame(list[j], this.data.Reservoir.Data, this.data.FishingRodDef);
						return true;
					}
				}
				Debug.LogError("[UIFishingWindow] Can't start fishing. Can't choose FishingDef");
				return false;
			}
		}
		return false;
	}

	// Token: 0x06004337 RID: 17207 RVA: 0x0013F90D File Offset: 0x0013DB0D
	protected override bool OnPressedBack()
	{
		if (!this.contentParent.activeSelf)
		{
			return false;
		}
		PlayerController playerController = MainGame.PlayerController;
		if (playerController != null)
		{
			playerController.FishingComponent.StopActivity(false, null);
		}
		return true;
	}

	// Token: 0x06004338 RID: 17208 RVA: 0x0013F938 File Offset: 0x0013DB38
	protected override void UpdateGamepadDependentStuff()
	{
		base.UpdateGamepadDependentStuff();
		if (!LazyInput.IsGamepadActive)
		{
			this.lazyButtonTips.Clear();
			bool isFishingMiniGameActive = this.IsFishingMiniGameActive;
			this.mouseAndKeyboardHints.SetActive(isFishingMiniGameActive);
			if (isFishingMiniGameActive)
			{
				this.hintRodLabel.text = LLBase.L("btn_pull_rod");
				this.hintExitLabel.text = LLBase.L("ui_menu_exit");
				((RectTransform)this.mouseAndKeyboardHints.transform).RefreshContentFitter();
			}
			return;
		}
		this.mouseAndKeyboardHints.SetActive(false);
		if (!this.IsFishingMiniGameActive)
		{
			List<LazyGameKeyTip> list = new List<LazyGameKeyTip>();
			if (this.CanSelectNextItem(true))
			{
				list.Add(new LazyGameKeyTip(GameKey.PrevTab, "tip_prev", true, true, true));
			}
			if (this.CanSelectNextItem(false))
			{
				list.Add(new LazyGameKeyTip(GameKey.NextTab, "tip_next", true, true, true));
			}
			list.Add(LazyGameKeyTip.Back(true, true, true));
			this.lazyButtonTips.Print(list, "  ");
			return;
		}
		this.lazyButtonTips.Print(new LazyGameKeyTip[]
		{
			new LazyGameKeyTip(GameKey.Select, "btn_pull_rod", true, true, true),
			new LazyGameKeyTip(GameKey.Back, "ui_menu_exit", true, true, true)
		});
	}

	// Token: 0x06004339 RID: 17209 RVA: 0x00002318 File Offset: 0x00000518
	protected override void PrintTips()
	{
	}

	// Token: 0x0600433A RID: 17210 RVA: 0x0013FA6D File Offset: 0x0013DC6D
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.TryAdd(GameKey.PrevTab, () => this.SelectNextItem(true));
		gameKeyDelegates.TryAdd(GameKey.NextTab, () => this.SelectNextItem(false));
		return gameKeyDelegates;
	}

	// Token: 0x0600433B RID: 17211 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x0400345C RID: 13404
	[SerializeField]
	private FishingSettings fishingSettings;

	// Token: 0x0400345D RID: 13405
	[SerializeField]
	private RectTransform baitSelection;

	// Token: 0x0400345E RID: 13406
	[SerializeField]
	private GameObject contentParent;

	// Token: 0x0400345F RID: 13407
	[SerializeField]
	private TextMeshProUGUI header;

	// Token: 0x04003460 RID: 13408
	[SerializeField]
	private LazyButton leftButton;

	// Token: 0x04003461 RID: 13409
	[SerializeField]
	private LazyButton rightButton;

	// Token: 0x04003462 RID: 13410
	[SerializeField]
	private UIDialogWindowButton submutButton;

	// Token: 0x04003463 RID: 13411
	[SerializeField]
	private UIItemCell itemCell;

	// Token: 0x04003464 RID: 13412
	[SerializeField]
	private RectTransform possibleFishesParent;

	// Token: 0x04003465 RID: 13413
	[SerializeField]
	private Vector2 posBig;

	// Token: 0x04003466 RID: 13414
	[SerializeField]
	private Vector2 posSmall;

	// Token: 0x04003467 RID: 13415
	[SerializeField]
	private GameObject mouseAndKeyboardHints;

	// Token: 0x04003468 RID: 13416
	[SerializeField]
	private TextMeshProUGUI hintRodLabel;

	// Token: 0x04003469 RID: 13417
	[SerializeField]
	private TextMeshProUGUI hintExitLabel;

	// Token: 0x0400346A RID: 13418
	[SerializeField]
	private int selectionPointer;

	// Token: 0x0400346B RID: 13419
	private PlayerFishingComponent fishingComponent;

	// Token: 0x0400346C RID: 13420
	private List<UIPossibleFishCell> possibleFishes = new List<UIPossibleFishCell>();
}
