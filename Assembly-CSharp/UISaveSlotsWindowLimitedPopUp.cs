using System;
using System.Collections;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000A37 RID: 2615
public class UISaveSlotsWindowLimitedPopUp : LazyWindow<UISaveSlotsWindowLimitedPopUpData>
{
	// Token: 0x06004679 RID: 18041 RVA: 0x00002318 File Offset: 0x00000518
	private static void SaveImportLog(string message)
	{
	}

	// Token: 0x0600467A RID: 18042 RVA: 0x0014D9FC File Offset: 0x0014BBFC
	public override void Init()
	{
		this.uiSaveSlotPrefab.gameObject.SetActive(false);
		UISaveSlot.OnSaveSlotSelectedWithSlot += this.OnSaveSlotSelectedHandler;
		SmoothMouseWheelScroll.EnsureForItem(this.scroll, this.uiSaveSlotPrefab, 80f);
		base.GamepadNavigationController.loopVerticalNavigation = true;
		base.Init();
	}

	// Token: 0x0600467B RID: 18043 RVA: 0x0014DA54 File Offset: 0x0014BC54
	public override void Open(UISaveSlotsWindowLimitedPopUpData data)
	{
		base.Open(data);
		UISaveSlotsWindowLimitedPopUp.SaveImportLog(string.Format("Open targetSlotIndex:[{0}]", data.TargetSlotIndex));
		this.ReadSlotsAndDisplay();
		if (this.scroll != null)
		{
			this.scroll.verticalNormalizedPosition = 1f;
		}
		((RectTransform)base.transform).RefreshContentFitter();
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
		}
	}

	// Token: 0x0600467C RID: 18044 RVA: 0x0014DACC File Offset: 0x0014BCCC
	private void ReadSlotsAndDisplay()
	{
		UISaveSlotsWindowLimitedPopUp.SaveImportLog("ReadSlotsAndDisplay started");
		this.importableSaveSlots = SaveSystem.GetImportableSaveSlotsForLimitedSlots();
		UISaveSlotsWindowLimitedPopUp.SaveImportLog(string.Format("ReadSlotsAndDisplay importableCount:[{0}]", this.importableSaveSlots.Count));
		foreach (SaveSlotData saveSlotData in this.importableSaveSlots)
		{
			UISaveSlotsWindowLimitedPopUp.SaveImportLog(string.Format("ReadSlotsAndDisplay show importable slotName:[{0}] platform:[{1}] isDemoSave:[{2}] canLoad:[{3}] sourceLabel:[{4}]", new object[]
			{
				(saveSlotData != null) ? saveSlotData.slotName : null,
				(saveSlotData != null) ? saveSlotData.platform : null,
				(saveSlotData != null) ? new bool?(saveSlotData.isDemoSave) : null,
				SaveSystem.CanLoadSaveSlot(saveSlotData),
				SaveSystem.GetLimitedSaveSlotSourceLabel(saveSlotData)
			}));
			UISaveSlot uisaveSlotElement = this.GetUISaveSlotElement();
			this.showingSlotsElements.Add(uisaveSlotElement);
			uisaveSlotElement.Show(saveSlotData, false, false);
			uisaveSlotElement.SetSourceLabel(SaveSystem.GetLimitedSaveSlotSourceLabel(saveSlotData));
		}
	}

	// Token: 0x0600467D RID: 18045 RVA: 0x0014DBE8 File Offset: 0x0014BDE8
	private void OnSaveSlotSelectedHandler(UISaveSlot slot, SaveSlotData slotData)
	{
		UISaveSlotsWindowLimitedPopUp.<>c__DisplayClass9_0 CS$<>8__locals1 = new UISaveSlotsWindowLimitedPopUp.<>c__DisplayClass9_0();
		CS$<>8__locals1.slotData = slotData;
		CS$<>8__locals1.<>4__this = this;
		if (!base.IsShownAndTop || !this.showingSlotsElements.Contains(slot) || CS$<>8__locals1.slotData == null)
		{
			UISaveSlotsWindowLimitedPopUp.SaveImportLog(string.Format("OnSaveSlotSelectedHandler ignored IsShownAndTop:[{0}] containsSlot:[{1}] slotDataNull:[{2}]", base.IsShownAndTop, this.showingSlotsElements.Contains(slot), CS$<>8__locals1.slotData == null));
			return;
		}
		UISaveSlotsWindowLimitedPopUp.SaveImportLog(string.Format("OnSaveSlotSelectedHandler selected slotName:[{0}] platform:[{1}] isDemoSave:[{2}] targetSlotIndex:[{3}]", new object[]
		{
			CS$<>8__locals1.slotData.slotName,
			CS$<>8__locals1.slotData.platform,
			CS$<>8__locals1.slotData.isDemoSave,
			this.data.TargetSlotIndex
		}));
		UIDialogWindowData uidialogWindowData = new UIDialogWindowData(LLBase.L("save_import_confirm"), LLBase.L("save_import_confirm_txt"), new Action(CS$<>8__locals1.<OnSaveSlotSelectedHandler>g__Yes|0), new Action(LazyUI.GetWindow<UIDialogWindow>().Close), false);
		uidialogWindowData.ShowCloseButton = false;
		LazyUI.GetWindow<UIDialogWindow>().Open(uidialogWindowData);
	}

	// Token: 0x0600467E RID: 18046 RVA: 0x0014DD02 File Offset: 0x0014BF02
	private IEnumerator ImportAfterOverlayShown(SaveSlotData slotData)
	{
		string text = "ImportAfterOverlayShown started slotName:[{0}] targetSlotIndex:[{1}]";
		SaveSlotData slotData2 = slotData;
		UISaveSlotsWindowLimitedPopUp.SaveImportLog(string.Format(text, (slotData2 != null) ? slotData2.slotName : null, this.data.TargetSlotIndex));
		SaveSystem.TriggerOnSaveStartEvent(true);
		yield return null;
		SaveSystem.ImportSaveToLimitedSlot(slotData, this.data.TargetSlotIndex, delegate(bool success)
		{
			string text2 = "ImportSaveToLimitedSlot callback slotName:[{0}] targetSlotIndex:[{1}] success:[{2}]";
			SaveSlotData slotData3 = slotData;
			UISaveSlotsWindowLimitedPopUp.SaveImportLog(string.Format(text2, (slotData3 != null) ? slotData3.slotName : null, this.data.TargetSlotIndex, success));
			if (!success)
			{
				return;
			}
			this.Close();
			Action onImported = this.data.OnImported;
			if (onImported == null)
			{
				return;
			}
			onImported();
		}, false);
		yield break;
	}

	// Token: 0x0600467F RID: 18047 RVA: 0x0014DD18 File Offset: 0x0014BF18
	private UISaveSlot GetUISaveSlotElement()
	{
		if (this.uiSaveSlotsElementPool.Count == 0)
		{
			this.uiSaveSlotsElementPool.Push(this.uiSaveSlotPrefab.Copy(null, false, ""));
		}
		return this.uiSaveSlotsElementPool.Pop();
	}

	// Token: 0x06004680 RID: 18048 RVA: 0x0014DD4F File Offset: 0x0014BF4F
	protected override void PrintTips(GamepadNavigationItem gamepadNavigationItem)
	{
		this.lazyButtonTips.Print(new LazyGameKeyTip[]
		{
			LazyGameKeyTip.Select(true, true, true),
			LazyGameKeyTip.Back(true, true, true)
		});
	}

	// Token: 0x06004681 RID: 18049 RVA: 0x0014DD78 File Offset: 0x0014BF78
	protected override void InitCloseButton(LazyButton button)
	{
		base.InitCloseButton(button);
		button.onClick.AddListener(new UnityAction(this.Close));
	}

	// Token: 0x06004682 RID: 18050 RVA: 0x0014DD99 File Offset: 0x0014BF99
	protected override bool OnPressedBack()
	{
		this.Close();
		return true;
	}

	// Token: 0x06004683 RID: 18051 RVA: 0x0014DDA4 File Offset: 0x0014BFA4
	public override void Hide()
	{
		for (int i = this.showingSlotsElements.Count - 1; i >= 0; i--)
		{
			this.showingSlotsElements[i].gameObject.SetActive(false);
			this.uiSaveSlotsElementPool.Push(this.showingSlotsElements[i]);
		}
		this.showingSlotsElements.Clear();
		base.Hide();
	}

	// Token: 0x06004684 RID: 18052 RVA: 0x0014DE08 File Offset: 0x0014C008
	protected override void TestDraw()
	{
		this.Open(new UISaveSlotsWindowLimitedPopUpData(1, null));
	}

	// Token: 0x04003726 RID: 14118
	[SerializeField]
	private ScrollRect scroll;

	// Token: 0x04003727 RID: 14119
	[SerializeField]
	private UISaveSlot uiSaveSlotPrefab;

	// Token: 0x04003728 RID: 14120
	private Stack<UISaveSlot> uiSaveSlotsElementPool = new Stack<UISaveSlot>();

	// Token: 0x04003729 RID: 14121
	private List<UISaveSlot> showingSlotsElements = new List<UISaveSlot>();

	// Token: 0x0400372A RID: 14122
	private List<SaveSlotData> importableSaveSlots = new List<SaveSlotData>();
}
