using System;
using System.Globalization;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000A2B RID: 2603
public class UISaveSlot : MonoBehaviour
{
	// Token: 0x140000C4 RID: 196
	// (add) Token: 0x06004626 RID: 17958 RVA: 0x0014C0A4 File Offset: 0x0014A2A4
	// (remove) Token: 0x06004627 RID: 17959 RVA: 0x0014C0D8 File Offset: 0x0014A2D8
	public static event Action<SaveSlotData> OnSaveSlotSelected;

	// Token: 0x140000C5 RID: 197
	// (add) Token: 0x06004628 RID: 17960 RVA: 0x0014C10C File Offset: 0x0014A30C
	// (remove) Token: 0x06004629 RID: 17961 RVA: 0x0014C140 File Offset: 0x0014A340
	public static event Action<UISaveSlot, SaveSlotData> OnSaveSlotSelectedWithSlot;

	// Token: 0x140000C6 RID: 198
	// (add) Token: 0x0600462A RID: 17962 RVA: 0x0014C174 File Offset: 0x0014A374
	// (remove) Token: 0x0600462B RID: 17963 RVA: 0x0014C1A8 File Offset: 0x0014A3A8
	public static event Action<UISaveSlot, SaveSlotData> OnSaveSlotDelete;

	// Token: 0x140000C7 RID: 199
	// (add) Token: 0x0600462C RID: 17964 RVA: 0x0014C1DC File Offset: 0x0014A3DC
	// (remove) Token: 0x0600462D RID: 17965 RVA: 0x0014C210 File Offset: 0x0014A410
	public static event Action<UISaveSlot> OnSaveSlotImport;

	// Token: 0x140000C8 RID: 200
	// (add) Token: 0x0600462E RID: 17966 RVA: 0x0014C244 File Offset: 0x0014A444
	// (remove) Token: 0x0600462F RID: 17967 RVA: 0x0014C278 File Offset: 0x0014A478
	public static event Action<UISaveSlot> OnSaveSlotEntered;

	// Token: 0x17000ABD RID: 2749
	// (get) Token: 0x06004630 RID: 17968 RVA: 0x0014C2AB File Offset: 0x0014A4AB
	public SaveSlotData LinkedSaveSlot
	{
		get
		{
			return this.linkedSaveSlot;
		}
	}

	// Token: 0x06004631 RID: 17969 RVA: 0x00002318 File Offset: 0x00000518
	private static void SaveImportLog(string message)
	{
	}

	// Token: 0x06004632 RID: 17970 RVA: 0x0014C2B4 File Offset: 0x0014A4B4
	private void Awake()
	{
		this.button.onClick.AddListener(new UnityAction(this.OnClicked));
		this.button.onEnter.AddListener(new UnityAction(this.OnEnter));
		this.button.onExit.AddListener(new UnityAction(this.OnExit));
		this.deleteButton.onClick.AddListener(new UnityAction(this.OnDelete));
		if (this.importButton != null)
		{
			this.importButton.onClick.AddListener(new UnityAction(this.OnImport));
		}
	}

	// Token: 0x06004633 RID: 17971 RVA: 0x0014C35B File Offset: 0x0014A55B
	private void Update()
	{
		if (!this.navigationItem.IsFocused)
		{
			return;
		}
		if (LazyInput.GetKeyDown(GameKey.SaveDelete))
		{
			this.OnDelete();
		}
		if (LazyInput.GetKeyDown(GameKey.SaveImport))
		{
			this.OnImport();
		}
	}

	// Token: 0x06004634 RID: 17972 RVA: 0x0014C38F File Offset: 0x0014A58F
	public bool CanDeleteSaveSlot()
	{
		return this.canDeleteCurrentSlot && this.linkedSaveSlot != null && this.deleteButton.gameObject.activeSelf && this.deleteButton.interactable;
	}

	// Token: 0x06004635 RID: 17973 RVA: 0x0014C3C0 File Offset: 0x0014A5C0
	public bool CanImportSaveSlot()
	{
		bool flag = this.canImportCurrentSlot && this.importButton != null && this.importButton.gameObject.activeSelf && this.importButton.interactable;
		string text = "CanImportSaveSlot result:[{0}] slotName:[{1}] canImportCurrentSlot:[{2}] importButtonNull:[{3}] importButtonActive:[{4}] importButtonInteractable:[{5}]";
		object[] array = new object[6];
		array[0] = flag;
		int num = 1;
		SaveSlotData saveSlotData = this.linkedSaveSlot;
		array[num] = ((saveSlotData != null) ? saveSlotData.slotName : null);
		array[2] = this.canImportCurrentSlot;
		array[3] = this.importButton == null;
		array[4] = this.importButton != null && this.importButton.gameObject.activeSelf;
		array[5] = this.importButton != null && this.importButton.interactable;
		UISaveSlot.SaveImportLog(string.Format(text, array));
		return flag;
	}

	// Token: 0x06004636 RID: 17974 RVA: 0x0014C4A8 File Offset: 0x0014A6A8
	public void Show(SaveSlotData saveSlotDataToDisplay, bool canDelete = true, bool canImport = false)
	{
		this.linkedSaveSlot = saveSlotDataToDisplay;
		this.canDeleteCurrentSlot = canDelete;
		this.canImportCurrentSlot = canImport;
		UISaveSlot.SaveImportLog(string.Format("Show slotName:[{0}] platform:[{1}] isDemoSave:[{2}] canDelete:[{3}] canImport:[{4}] importButtonNull:[{5}]", new object[]
		{
			(saveSlotDataToDisplay != null) ? saveSlotDataToDisplay.slotName : null,
			(saveSlotDataToDisplay != null) ? saveSlotDataToDisplay.platform : null,
			(saveSlotDataToDisplay != null) ? new bool?(saveSlotDataToDisplay.isDemoSave) : null,
			canDelete,
			canImport,
			this.importButton == null
		}));
		this.saveNameLabel.gameObject.SetActive(false);
		this.SetSourceLabel(string.Empty);
		this.SetImportButtonState(this.canImportCurrentSlot);
		if (saveSlotDataToDisplay == null)
		{
			this.deleteButton.gameObject.SetActive(false);
			this.button.interactable = true;
			this.slotBack.gameObject.SetActive(true);
			this.slotBackLocked.gameObject.SetActive(false);
			this.resourcesParent.SetActive(false);
			this.newSaveText.SetActive(true);
			this.dayLabel.gameObject.SetActive(false);
			this.saveDateTimeLabel.gameObject.SetActive(false);
		}
		else
		{
			this.button.interactable = SaveSystem.CanLoadSaveSlot(saveSlotDataToDisplay);
			this.newSaveText.SetActive(false);
			this.saveDateTimeLabel.gameObject.SetActive(true);
			this.dayLabel.gameObject.SetActive(true);
			this.resourcesLabels[0].text = string.Format("{0}", saveSlotDataToDisplay.graveyardQuality);
			this.resourcesLabels[1].text = string.Format("{0}", saveSlotDataToDisplay.churchQuality);
			this.resourcesLabels[2].text = string.Format("{0}", saveSlotDataToDisplay.villageRep);
			this.slotBack.gameObject.SetActive(this.button.interactable);
			this.slotBackLocked.gameObject.SetActive(!this.button.interactable);
			this.resourcesParent.SetActive(true);
			if (this.button.interactable)
			{
				this.deleteButton.gameObject.SetActive(this.canDeleteCurrentSlot);
				TextStyleComponent[] array = this.resourcesStyleComponents;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetTextStyle(this.resourcesStyle);
				}
				Image[] array2 = this.resourcesIcons;
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i].color = this.resourceIconActiveColor;
				}
				this.dayStyleComponent.SetTextStyle(this.daysPart1Style);
				this.dayLabel.text = LLBase.L("save_slot_descr", this.daysPart2Style.ApplyStyleToString(this.linkedSaveSlot.day.ToString(), false, true));
				this.timeStyleComponent.SetTextStyle(this.timePart1Style);
				this.saveDateTimeLabel.text = this.GetStyledSaveDateTime(this.linkedSaveSlot.GetSaveDateTime(), this.timePart2Style) ?? "";
			}
			else
			{
				this.deleteButton.gameObject.SetActive(false);
				TextStyleComponent[] array = this.resourcesStyleComponents;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetTextStyle(this.resourcesStyleInactive);
				}
				Image[] array2 = this.resourcesIcons;
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i].color = this.resourceIconInactiveColor;
				}
				this.dayStyleComponent.SetTextStyle(this.daysStyleInactive);
				this.dayLabel.text = LLBase.L("save_slot_descr", this.linkedSaveSlot.day.ToString());
				this.timeStyleComponent.SetTextStyle(this.timePart1StyleInactive);
				this.saveDateTimeLabel.text = this.GetStyledSaveDateTime(this.linkedSaveSlot.GetSaveDateTime(), this.timePart2StyleInactive) ?? "";
			}
		}
		base.gameObject.SetActive(true);
	}

	// Token: 0x06004637 RID: 17975 RVA: 0x0014C89C File Offset: 0x0014AA9C
	public void SetSourceLabel(string text)
	{
		if (this.sourceLabel == null)
		{
			return;
		}
		bool flag = !string.IsNullOrEmpty(text);
		this.sourceLabel.gameObject.SetActive(flag);
		if (flag)
		{
			this.sourceLabel.text = text;
		}
	}

	// Token: 0x06004638 RID: 17976 RVA: 0x0014C8E4 File Offset: 0x0014AAE4
	private string GetStyledSaveDateTime(DateTime saveDateTime, TextStyle textStyle)
	{
		string text = saveDateTime.ToString(CultureInfo.CurrentCulture);
		string text2 = saveDateTime.ToString("d", CultureInfo.CurrentCulture);
		int num = text.IndexOf(text2, StringComparison.CurrentCulture);
		if (num < 0)
		{
			return text;
		}
		string text3 = text.Substring(0, num);
		string text4 = text.Substring(num + text2.Length);
		return text3 + textStyle.ApplyStyleToString(text2, false, true) + text4;
	}

	// Token: 0x06004639 RID: 17977 RVA: 0x0014C949 File Offset: 0x0014AB49
	private void OnEnter()
	{
		this.selector.gameObject.SetActive(true);
		LazyAudio.PlayAndForget("gui_hover");
		Action<UISaveSlot> onSaveSlotEntered = UISaveSlot.OnSaveSlotEntered;
		if (onSaveSlotEntered == null)
		{
			return;
		}
		onSaveSlotEntered(this);
	}

	// Token: 0x0600463A RID: 17978 RVA: 0x0014C976 File Offset: 0x0014AB76
	private void OnExit()
	{
		this.selector.gameObject.SetActive(false);
	}

	// Token: 0x0600463B RID: 17979 RVA: 0x0014C989 File Offset: 0x0014AB89
	private void OnClicked()
	{
		this.selector.gameObject.SetActive(false);
		Action<UISaveSlot, SaveSlotData> onSaveSlotSelectedWithSlot = UISaveSlot.OnSaveSlotSelectedWithSlot;
		if (onSaveSlotSelectedWithSlot != null)
		{
			onSaveSlotSelectedWithSlot(this, this.linkedSaveSlot);
		}
		Action<SaveSlotData> onSaveSlotSelected = UISaveSlot.OnSaveSlotSelected;
		if (onSaveSlotSelected == null)
		{
			return;
		}
		onSaveSlotSelected(this.linkedSaveSlot);
	}

	// Token: 0x0600463C RID: 17980 RVA: 0x0014C9C8 File Offset: 0x0014ABC8
	private void OnDelete()
	{
		if (!this.CanDeleteSaveSlot())
		{
			return;
		}
		this.selector.gameObject.SetActive(false);
		Action<UISaveSlot, SaveSlotData> onSaveSlotDelete = UISaveSlot.OnSaveSlotDelete;
		if (onSaveSlotDelete == null)
		{
			return;
		}
		onSaveSlotDelete(this, this.linkedSaveSlot);
	}

	// Token: 0x0600463D RID: 17981 RVA: 0x0014C9FC File Offset: 0x0014ABFC
	private void OnImport()
	{
		if (!this.CanImportSaveSlot())
		{
			string text = "OnImport ignored slotName:[";
			SaveSlotData saveSlotData = this.linkedSaveSlot;
			UISaveSlot.SaveImportLog(text + ((saveSlotData != null) ? saveSlotData.slotName : null) + "]");
			return;
		}
		string text2 = "OnImport accepted slotName:[";
		SaveSlotData saveSlotData2 = this.linkedSaveSlot;
		UISaveSlot.SaveImportLog(text2 + ((saveSlotData2 != null) ? saveSlotData2.slotName : null) + "]");
		this.selector.gameObject.SetActive(false);
		Action<UISaveSlot> onSaveSlotImport = UISaveSlot.OnSaveSlotImport;
		if (onSaveSlotImport == null)
		{
			return;
		}
		onSaveSlotImport(this);
	}

	// Token: 0x0600463E RID: 17982 RVA: 0x0014CA80 File Offset: 0x0014AC80
	private void SetImportButtonState(bool isActive)
	{
		if (this.importButton != null)
		{
			this.importButton.gameObject.SetActive(isActive);
			string text = "SetImportButtonState isActive:[{0}] slotName:[{1}]";
			object obj = isActive;
			SaveSlotData saveSlotData = this.linkedSaveSlot;
			UISaveSlot.SaveImportLog(string.Format(text, obj, (saveSlotData != null) ? saveSlotData.slotName : null));
			return;
		}
		string text2 = "SetImportButtonState skipped because importButton is null. Requested isActive:[{0}] slotName:[{1}]";
		object obj2 = isActive;
		SaveSlotData saveSlotData2 = this.linkedSaveSlot;
		UISaveSlot.SaveImportLog(string.Format(text2, obj2, (saveSlotData2 != null) ? saveSlotData2.slotName : null));
	}

	// Token: 0x0600463F RID: 17983 RVA: 0x0014C976 File Offset: 0x0014AB76
	private void OnDisable()
	{
		this.selector.gameObject.SetActive(false);
	}

	// Token: 0x040036E4 RID: 14052
	[SerializeField]
	private LazyButton button;

	// Token: 0x040036E5 RID: 14053
	[SerializeField]
	private GamepadNavigationItem navigationItem;

	// Token: 0x040036E6 RID: 14054
	[SerializeField]
	private LazyButton deleteButton;

	// Token: 0x040036E7 RID: 14055
	[SerializeField]
	private LazyButton importButton;

	// Token: 0x040036E8 RID: 14056
	[SerializeField]
	private GameObject resourcesParent;

	// Token: 0x040036E9 RID: 14057
	[SerializeField]
	private Image slotBack;

	// Token: 0x040036EA RID: 14058
	[SerializeField]
	private Image slotBackLocked;

	// Token: 0x040036EB RID: 14059
	[SerializeField]
	private TextMeshProUGUI dayLabel;

	// Token: 0x040036EC RID: 14060
	[SerializeField]
	private TextMeshProUGUI saveDateTimeLabel;

	// Token: 0x040036ED RID: 14061
	[SerializeField]
	private TextMeshProUGUI saveNameLabel;

	// Token: 0x040036EE RID: 14062
	[SerializeField]
	private TextMeshProUGUI sourceLabel;

	// Token: 0x040036EF RID: 14063
	[SerializeField]
	private TextMeshProUGUI[] resourcesLabels;

	// Token: 0x040036F0 RID: 14064
	[SerializeField]
	private Image[] resourcesIcons;

	// Token: 0x040036F1 RID: 14065
	[SerializeField]
	private GameObject newSaveText;

	// Token: 0x040036F2 RID: 14066
	[SerializeField]
	private GameObject selector;

	// Token: 0x040036F3 RID: 14067
	[SerializeField]
	private TextStyle timePart1Style;

	// Token: 0x040036F4 RID: 14068
	[SerializeField]
	private TextStyle timePart1StyleInactive;

	// Token: 0x040036F5 RID: 14069
	[SerializeField]
	private TextStyle timePart2Style;

	// Token: 0x040036F6 RID: 14070
	[SerializeField]
	private TextStyle timePart2StyleInactive;

	// Token: 0x040036F7 RID: 14071
	[SerializeField]
	private TextStyle daysPart1Style;

	// Token: 0x040036F8 RID: 14072
	[SerializeField]
	private TextStyle daysPart2Style;

	// Token: 0x040036F9 RID: 14073
	[SerializeField]
	private TextStyle daysStyleInactive;

	// Token: 0x040036FA RID: 14074
	[SerializeField]
	private TextStyle resourcesStyle;

	// Token: 0x040036FB RID: 14075
	[SerializeField]
	private TextStyle resourcesStyleInactive;

	// Token: 0x040036FC RID: 14076
	[SerializeField]
	private TextStyleComponent timeStyleComponent;

	// Token: 0x040036FD RID: 14077
	[SerializeField]
	private TextStyleComponent dayStyleComponent;

	// Token: 0x040036FE RID: 14078
	[SerializeField]
	private TextStyleComponent[] resourcesStyleComponents;

	// Token: 0x040036FF RID: 14079
	[SerializeField]
	private Color resourceIconActiveColor = Color.white;

	// Token: 0x04003700 RID: 14080
	[SerializeField]
	private Color resourceIconInactiveColor = new Color(1f, 1f, 1f, 0.7f);

	// Token: 0x04003701 RID: 14081
	private SaveSlotData linkedSaveSlot;

	// Token: 0x04003702 RID: 14082
	private bool canDeleteCurrentSlot = true;

	// Token: 0x04003703 RID: 14083
	private bool canImportCurrentSlot;
}
