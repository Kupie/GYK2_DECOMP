using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000A2C RID: 2604
public class UISaveSlotsWindow : LazyWindow<LazyWidgetDataBase>
{
	// Token: 0x06004641 RID: 17985 RVA: 0x0014CB34 File Offset: 0x0014AD34
	public override void Init()
	{
		this.uiSaveSlotPrefab.gameObject.SetActive(false);
		this.newSaveSlot = this.GetUISaveSlotElement();
		this.newSaveSlot.Show(null, true, false);
		UISaveSlot.OnSaveSlotSelected += this.OnSaveSlotSelectedHandler;
		UISaveSlot.OnSaveSlotDelete += this.OnSaveSlotDeletedHandler;
		SmoothMouseWheelScroll.EnsureForItem(this.scroll, this.uiSaveSlotPrefab, 80f);
		base.Init();
		base.GamepadNavigationController.loopVerticalNavigation = true;
	}

	// Token: 0x06004642 RID: 17986 RVA: 0x0014CBB8 File Offset: 0x0014ADB8
	public override void Open(LazyWidgetDataBase data)
	{
		base.Open(data);
		this.ReadSlotsAndDisplay();
		this.pickedSaveSlot = null;
		this.scroll.verticalNormalizedPosition = 1f;
		this.newSaveSlot.gameObject.SetActive(true);
		((RectTransform)base.transform).RefreshContentFitter();
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
		}
	}

	// Token: 0x06004643 RID: 17987 RVA: 0x0014CC20 File Offset: 0x0014AE20
	private void ReadSlotsAndDisplay()
	{
		this.saveSlotDataList = SaveSystem.SaveSlotDataList;
		this.newSaveSlot.gameObject.SetActive(true);
		foreach (SaveSlotData saveSlotData in this.saveSlotDataList)
		{
			if (!saveSlotData.IsDemoSlotDeleted)
			{
				UISaveSlot uisaveSlotElement = this.GetUISaveSlotElement();
				this.showingSlotsElements.Add(uisaveSlotElement);
				uisaveSlotElement.Show(saveSlotData, true, false);
			}
		}
		this.SortSlotsByDateTime();
	}

	// Token: 0x06004644 RID: 17988 RVA: 0x0014CCB4 File Offset: 0x0014AEB4
	private void OnSaveSlotSelectedHandler(SaveSlotData slotData)
	{
		if (!base.IsShownAndTop)
		{
			return;
		}
		if (slotData == null)
		{
			this.pickedSaveSlot = null;
			this.Close();
			MainGame.Instance.StartNewGame(false);
			return;
		}
		this.Close();
		LazyTimer.AddTimer(0f, delegate
		{
			UILoadingOverlay overlay = LazyUI.Get<UILoadingOverlay>();
			Action<GameSave> <>9__2;
			overlay.Draw(new LoadingWindowData(MainGame.EntrySceneToLoad, delegate
			{
				SaveSlotData slotData2 = slotData;
				Action<GameSave> action;
				if ((action = <>9__2) == null)
				{
					action = (<>9__2 = delegate(GameSave s)
					{
						if (s != null)
						{
							this.pickedSaveSlot = slotData;
							MainGame.Instance.ContinueGame(this.pickedSaveSlot, s);
							return;
						}
						overlay.Hide();
						this.Open(null);
					});
				}
				SaveSystem.Load(slotData2, action);
			}, false));
		}, null);
	}

	// Token: 0x06004645 RID: 17989 RVA: 0x0014CD20 File Offset: 0x0014AF20
	private void OnSaveSlotDeletedHandler(UISaveSlot slot, SaveSlotData slotData)
	{
		UISaveSlotsWindow.<>c__DisplayClass11_0 CS$<>8__locals1 = new UISaveSlotsWindow.<>c__DisplayClass11_0();
		CS$<>8__locals1.slot = slot;
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.slotData = slotData;
		if (!base.IsShownAndTop)
		{
			return;
		}
		LazyUI.GetWindow<UIDialogWindow>().Open(new UIDialogWindowData(LLBase.L("ui_save_remove_header"), LLBase.L("ui_save_remove_info"), new Action(CS$<>8__locals1.<OnSaveSlotDeletedHandler>g__Yes|0), new Action(LazyUI.GetWindow<UIDialogWindow>().Close), false)
		{
			ShowCloseButton = false
		});
	}

	// Token: 0x06004646 RID: 17990 RVA: 0x0014CD9C File Offset: 0x0014AF9C
	private void SortSlotsByDateTime()
	{
		this.showingSlotsElements.Sort((UISaveSlot x, UISaveSlot y) => DateTime.Compare(y.LinkedSaveSlot.GetSaveDateTime(), x.LinkedSaveSlot.GetSaveDateTime()));
		for (int i = 0; i < this.showingSlotsElements.Count; i++)
		{
			this.showingSlotsElements[i].transform.SetSiblingIndex(i);
		}
		this.newSaveSlot.transform.SetAsFirstSibling();
	}

	// Token: 0x06004647 RID: 17991 RVA: 0x0014CE10 File Offset: 0x0014B010
	private UISaveSlot GetUISaveSlotElement()
	{
		if (this.uiSaveSlotsElementPool.Count == 0)
		{
			this.uiSaveSlotsElementPool.Push(this.uiSaveSlotPrefab.Copy(null, false, ""));
		}
		return this.uiSaveSlotsElementPool.Pop();
	}

	// Token: 0x06004648 RID: 17992 RVA: 0x0014CE48 File Offset: 0x0014B048
	protected override void PrintTips(GamepadNavigationItem gamepadNavigationItem)
	{
		UISaveSlot uisaveSlot;
		if (!gamepadNavigationItem.TryGetComponent<UISaveSlot>(out uisaveSlot))
		{
			base.PrintTips(gamepadNavigationItem);
			return;
		}
		if (uisaveSlot.CanDeleteSaveSlot())
		{
			this.lazyButtonTips.Print(new LazyGameKeyTip[]
			{
				LazyGameKeyTip.Select(true, true, true),
				LazyGameKeyTip.Back(true, true, true),
				new LazyGameKeyTip(GameKey.SaveDelete, "ui_save_slot_delete", true, true, true)
			});
			return;
		}
		this.lazyButtonTips.Print(new LazyGameKeyTip[]
		{
			LazyGameKeyTip.Select(true, true, true),
			LazyGameKeyTip.Back(true, true, true)
		});
	}

	// Token: 0x06004649 RID: 17993 RVA: 0x0014CED3 File Offset: 0x0014B0D3
	protected override void InitCloseButton(LazyButton button)
	{
		base.InitCloseButton(button);
		button.onClick.AddListener(new UnityAction(this.ReturnToPreviousWindow));
	}

	// Token: 0x0600464A RID: 17994 RVA: 0x0014CEF3 File Offset: 0x0014B0F3
	protected override bool OnPressedBack()
	{
		bool flag = base.OnPressedBack();
		this.ReturnToPreviousWindow();
		return flag;
	}

	// Token: 0x0600464B RID: 17995 RVA: 0x0014CF01 File Offset: 0x0014B101
	private void ReturnToPreviousWindow()
	{
		LazyUI.GetWindow<UIMainMenuWindow>().Open(null);
	}

	// Token: 0x0600464C RID: 17996 RVA: 0x0014CF10 File Offset: 0x0014B110
	public override void Hide()
	{
		base.Hide();
		for (int i = this.showingSlotsElements.Count - 1; i >= 0; i--)
		{
			this.showingSlotsElements[i].gameObject.SetActive(false);
			this.uiSaveSlotsElementPool.Push(this.showingSlotsElements[i]);
		}
		this.showingSlotsElements.Clear();
		this.newSaveSlot.gameObject.SetActive(false);
	}

	// Token: 0x0600464D RID: 17997 RVA: 0x0013AAB0 File Offset: 0x00138CB0
	protected override void TestDraw()
	{
		this.Open(null);
	}

	// Token: 0x04003704 RID: 14084
	[SerializeField]
	private ScrollRect scroll;

	// Token: 0x04003705 RID: 14085
	[SerializeField]
	private UISaveSlot uiSaveSlotPrefab;

	// Token: 0x04003706 RID: 14086
	private Stack<UISaveSlot> uiSaveSlotsElementPool = new Stack<UISaveSlot>();

	// Token: 0x04003707 RID: 14087
	private List<UISaveSlot> showingSlotsElements = new List<UISaveSlot>();

	// Token: 0x04003708 RID: 14088
	private UISaveSlot newSaveSlot;

	// Token: 0x04003709 RID: 14089
	private List<SaveSlotData> saveSlotDataList;

	// Token: 0x0400370A RID: 14090
	private SaveSlotData pickedSaveSlot;
}
