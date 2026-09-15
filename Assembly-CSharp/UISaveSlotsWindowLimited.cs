using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000A32 RID: 2610
public class UISaveSlotsWindowLimited : LazyWindow<LazyWidgetDataBase>
{
	// Token: 0x0600465A RID: 18010 RVA: 0x00002318 File Offset: 0x00000518
	private static void SaveImportLog(string message)
	{
	}

	// Token: 0x0600465B RID: 18011 RVA: 0x0014D1A0 File Offset: 0x0014B3A0
	public override void Init()
	{
		UISaveSlot.OnSaveSlotSelectedWithSlot += this.OnSaveSlotSelectedHandler;
		UISaveSlot.OnSaveSlotDelete += this.OnSaveSlotDeletedHandler;
		UISaveSlot.OnSaveSlotImport += this.OnSaveSlotImportHandler;
		UISaveSlot.OnSaveSlotEntered += this.OnSaveSlotEnteredHandler;
		base.Init();
		base.GamepadNavigationController.loopVerticalNavigation = true;
	}

	// Token: 0x0600465C RID: 18012 RVA: 0x0014D204 File Offset: 0x0014B404
	public override void Open(LazyWidgetDataBase data)
	{
		base.Open(data);
		UISaveSlotsWindowLimited.SaveImportLog("Open");
		this.ReadSlotsAndDisplay();
		this.focusedSaveSlot = this.GetFirstAvailableSaveSlot();
		UISaveSlotsWindowLimited.SaveImportLog(string.Format("Open focusedSaveSlotNull:[{0}]", this.focusedSaveSlot == null));
		((RectTransform)base.transform).RefreshContentFitter();
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
		}
	}

	// Token: 0x0600465D RID: 18013 RVA: 0x0014D27C File Offset: 0x0014B47C
	private void ReadSlotsAndDisplay()
	{
		UISaveSlotsWindowLimited.SaveImportLog("ReadSlotsAndDisplay started");
		this.saveSlotDataList = SaveSystem.GetLimitedSaveSlotsData();
		this.hasImportableSaveSlots = SaveSystem.HasImportableSaveSlotsForLimitedSlots();
		string text = "ReadSlotsAndDisplay limitedSlotsCount:[{0}] hasImportableSaveSlots:[{1}]";
		List<SaveSlotData> list = this.saveSlotDataList;
		UISaveSlotsWindowLimited.SaveImportLog(string.Format(text, (list != null) ? list.Count : 0, this.hasImportableSaveSlots));
		int num = 0;
		while (num < this.saveSlots.Length && num < 3)
		{
			if (this.saveSlots[num] == null)
			{
				UISaveSlotsWindowLimited.SaveImportLog(string.Format("ReadSlotsAndDisplay ui slot[{0}] is null", num));
			}
			else
			{
				SaveSlotData saveSlotData = this.saveSlotDataList[num];
				UISaveSlotsWindowLimited.SaveImportLog(string.Format("ReadSlotsAndDisplay show ui slot[{0}] saveSlotName:[{1}] platform:[{2}] isDemoSave:[{3}] canImport:[{4}]", new object[]
				{
					num,
					(saveSlotData != null) ? saveSlotData.slotName : null,
					(saveSlotData != null) ? saveSlotData.platform : null,
					(saveSlotData != null) ? new bool?(saveSlotData.isDemoSave) : null,
					this.hasImportableSaveSlots
				}));
				this.saveSlots[num].Show(this.saveSlotDataList[num], true, this.hasImportableSaveSlots);
			}
			num++;
		}
	}

	// Token: 0x0600465E RID: 18014 RVA: 0x0014D3BC File Offset: 0x0014B5BC
	private void OnSaveSlotSelectedHandler(UISaveSlot slot, SaveSlotData slotData)
	{
		if (!base.IsShownAndTop || !this.IsLimitedWindowSlot(slot))
		{
			return;
		}
		this.focusedSaveSlot = slot;
		int slotIndex = this.GetSlotIndex(slot);
		if (slotIndex < 1)
		{
			return;
		}
		if (slotData == null)
		{
			this.Close();
			MainGame.Instance.StartNewGameInLimitedSaveSlot(slotIndex, false);
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
					action = (<>9__2 = delegate(GameSave save)
					{
						if (save != null)
						{
							MainGame.Instance.ContinueGame(slotData, save);
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

	// Token: 0x0600465F RID: 18015 RVA: 0x0014D43C File Offset: 0x0014B63C
	private void OnSaveSlotDeletedHandler(UISaveSlot slot, SaveSlotData slotData)
	{
		UISaveSlotsWindowLimited.<>c__DisplayClass11_0 CS$<>8__locals1 = new UISaveSlotsWindowLimited.<>c__DisplayClass11_0();
		CS$<>8__locals1.slotData = slotData;
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.slot = slot;
		if (!base.IsShownAndTop || !this.IsLimitedWindowSlot(CS$<>8__locals1.slot) || CS$<>8__locals1.slotData == null)
		{
			return;
		}
		UIDialogWindowData uidialogWindowData = new UIDialogWindowData(LLBase.L("ui_save_remove_header"), LLBase.L("ui_save_remove_info"), new Action(CS$<>8__locals1.<OnSaveSlotDeletedHandler>g__Yes|0), new Action(LazyUI.GetWindow<UIDialogWindow>().Close), false);
		uidialogWindowData.ShowCloseButton = false;
		LazyUI.GetWindow<UIDialogWindow>().Open(uidialogWindowData);
	}

	// Token: 0x06004660 RID: 18016 RVA: 0x0014D4CD File Offset: 0x0014B6CD
	private void OnSaveSlotEnteredHandler(UISaveSlot slot)
	{
		if (!base.IsShown || !this.IsLimitedWindowSlot(slot))
		{
			return;
		}
		this.focusedSaveSlot = slot;
	}

	// Token: 0x06004661 RID: 18017 RVA: 0x0014D4E8 File Offset: 0x0014B6E8
	private void OnSaveSlotImportHandler(UISaveSlot slot)
	{
		if (!base.IsShownAndTop || !this.IsLimitedWindowSlot(slot))
		{
			UISaveSlotsWindowLimited.SaveImportLog(string.Format("OnSaveSlotImportHandler ignored IsShownAndTop:[{0}] isLimitedWindowSlot:[{1}]", base.IsShownAndTop, this.IsLimitedWindowSlot(slot)));
			return;
		}
		this.focusedSaveSlot = slot;
		UISaveSlotsWindowLimited.SaveImportLog(string.Format("OnSaveSlotImportHandler accepted targetSlotIndex:[{0}]", this.GetSlotIndex(slot)));
		this.OnPressedImportSave();
	}

	// Token: 0x06004662 RID: 18018 RVA: 0x0014D558 File Offset: 0x0014B758
	private bool OnPressedImportSave()
	{
		if (!base.IsShownAndTop || !this.hasImportableSaveSlots)
		{
			UISaveSlotsWindowLimited.SaveImportLog(string.Format("OnPressedImportSave rejected IsShownAndTop:[{0}] hasImportableSaveSlots:[{1}]", base.IsShownAndTop, this.hasImportableSaveSlots));
			return false;
		}
		UISaveSlot uisaveSlot = ((this.focusedSaveSlot != null) ? this.focusedSaveSlot : this.GetFirstAvailableSaveSlot());
		if (uisaveSlot == null || !uisaveSlot.CanImportSaveSlot())
		{
			UISaveSlotsWindowLimited.SaveImportLog(string.Format("OnPressedImportSave rejected targetSlotNull:[{0}] targetCanImport:[{1}]", uisaveSlot == null, uisaveSlot != null && uisaveSlot.CanImportSaveSlot()));
			return false;
		}
		int slotIndex = this.GetSlotIndex(uisaveSlot);
		if (slotIndex < 1)
		{
			UISaveSlotsWindowLimited.SaveImportLog(string.Format("OnPressedImportSave rejected invalid targetSlotIndex:[{0}]", slotIndex));
			return false;
		}
		UISaveSlotsWindowLimited.SaveImportLog(string.Format("OnPressedImportSave opening popup targetSlotIndex:[{0}]", slotIndex));
		LazyUI.GetWindow<UISaveSlotsWindowLimitedPopUp>().Open(new UISaveSlotsWindowLimitedPopUpData(slotIndex, new Action(this.RefreshAfterImport)));
		return true;
	}

	// Token: 0x06004663 RID: 18019 RVA: 0x0014D653 File Offset: 0x0014B853
	private void RefreshAfterImport()
	{
		UISaveSlotsWindowLimited.SaveImportLog("RefreshAfterImport");
		this.ReadSlotsAndDisplay();
		((RectTransform)base.transform).RefreshContentFitter();
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
		}
	}

	// Token: 0x06004664 RID: 18020 RVA: 0x0014D68C File Offset: 0x0014B88C
	private UISaveSlot GetFirstAvailableSaveSlot()
	{
		for (int i = 0; i < this.saveSlots.Length; i++)
		{
			if (this.saveSlots[i] != null)
			{
				return this.saveSlots[i];
			}
		}
		return null;
	}

	// Token: 0x06004665 RID: 18021 RVA: 0x0014D6C8 File Offset: 0x0014B8C8
	private int GetSlotIndex(UISaveSlot slot)
	{
		int num = 0;
		while (num < this.saveSlots.Length && num < 3)
		{
			if (this.saveSlots[num] == slot)
			{
				return num + 1;
			}
			num++;
		}
		return -1;
	}

	// Token: 0x06004666 RID: 18022 RVA: 0x0014D701 File Offset: 0x0014B901
	private bool IsLimitedWindowSlot(UISaveSlot slot)
	{
		return this.GetSlotIndex(slot) > 0;
	}

	// Token: 0x06004667 RID: 18023 RVA: 0x0014D710 File Offset: 0x0014B910
	protected override void PrintTips(GamepadNavigationItem gamepadNavigationItem)
	{
		base.PrintTips(gamepadNavigationItem);
		UISaveSlot uisaveSlot;
		if (gamepadNavigationItem != null && gamepadNavigationItem.TryGetComponent<UISaveSlot>(out uisaveSlot) && this.IsLimitedWindowSlot(uisaveSlot))
		{
			this.focusedSaveSlot = uisaveSlot;
			List<LazyGameKeyTip> list = new List<LazyGameKeyTip>
			{
				LazyGameKeyTip.Select(true, true, true),
				LazyGameKeyTip.Back(true, true, true)
			};
			if (uisaveSlot.CanDeleteSaveSlot())
			{
				list.Add(new LazyGameKeyTip(GameKey.SaveDelete, "ui_save_slot_delete", true, true, true));
			}
			if (uisaveSlot.CanImportSaveSlot())
			{
				list.Add(new LazyGameKeyTip(this.importSaveGameKey, this.importSaveTipLocale, true, true, true));
			}
			this.lazyButtonTips.Print(list, "  ");
		}
	}

	// Token: 0x06004668 RID: 18024 RVA: 0x0014D7C4 File Offset: 0x0014B9C4
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		if (this.importSaveGameKey != null)
		{
			gameKeyDelegates.TryAdd(this.importSaveGameKey, new Func<bool>(this.OnPressedImportSave));
		}
		return gameKeyDelegates;
	}

	// Token: 0x06004669 RID: 18025 RVA: 0x0014D800 File Offset: 0x0014BA00
	protected override void InitCloseButton(LazyButton button)
	{
		base.InitCloseButton(button);
		button.onClick.AddListener(new UnityAction(this.ReturnToPreviousWindow));
	}

	// Token: 0x0600466A RID: 18026 RVA: 0x0014D820 File Offset: 0x0014BA20
	protected override bool OnPressedBack()
	{
		bool flag = base.OnPressedBack();
		this.ReturnToPreviousWindow();
		return flag;
	}

	// Token: 0x0600466B RID: 18027 RVA: 0x0014CF01 File Offset: 0x0014B101
	private void ReturnToPreviousWindow()
	{
		LazyUI.GetWindow<UIMainMenuWindow>().Open(null);
	}

	// Token: 0x0600466C RID: 18028 RVA: 0x0014D82E File Offset: 0x0014BA2E
	public override void Hide()
	{
		base.Hide();
	}

	// Token: 0x0600466D RID: 18029 RVA: 0x0013AAB0 File Offset: 0x00138CB0
	protected override void TestDraw()
	{
		this.Open(null);
	}

	// Token: 0x04003716 RID: 14102
	[SerializeField]
	private UISaveSlot[] saveSlots = new UISaveSlot[3];

	// Token: 0x04003717 RID: 14103
	[SerializeField]
	private GameKey importSaveGameKey = GameKey.SaveImport;

	// Token: 0x04003718 RID: 14104
	[SerializeField]
	private string importSaveTipLocale = "ui_save_import";

	// Token: 0x04003719 RID: 14105
	private List<SaveSlotData> saveSlotDataList = new List<SaveSlotData>();

	// Token: 0x0400371A RID: 14106
	private UISaveSlot focusedSaveSlot;

	// Token: 0x0400371B RID: 14107
	private bool hasImportableSaveSlots;
}
