using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020007B1 RID: 1969
public class UISteamWorkshopCreatorWindow : LazyWindow<LazyWidgetDataBase>
{
	// Token: 0x06003293 RID: 12947 RVA: 0x000F34B0 File Offset: 0x000F16B0
	public static void Toggle()
	{
		if (!SteamWorkshopCreatorConfig.Enabled)
		{
			return;
		}
		if (!SteamManager.Initialized)
		{
			Debug.Log("[SteamWorkshopCreator] Steam is not initialized.");
			return;
		}
		if (UISteamWorkshopCreatorWindow.instance == null)
		{
			UISteamWorkshopCreatorWindow.instance = UISteamWorkshopCreatorWindow.CreateInstance();
			UISteamWorkshopCreatorWindow.instance.Open(null);
			return;
		}
		if (UISteamWorkshopCreatorWindow.instance.IsShown)
		{
			UISteamWorkshopCreatorWindow.instance.Close();
			return;
		}
		UISteamWorkshopCreatorWindow.instance.Open(null);
	}

	// Token: 0x06003294 RID: 12948 RVA: 0x000F351C File Offset: 0x000F171C
	private static UISteamWorkshopCreatorWindow CreateInstance()
	{
		GameObject gameObject = new GameObject("UISteamWorkshopCreatorWindow");
		Transform transform = UISteamWorkshopCreatorWindow.FindUiRoot();
		if (transform != null)
		{
			gameObject.transform.SetParent(transform, false);
		}
		else
		{
			global::UnityEngine.Object.DontDestroyOnLoad(gameObject);
		}
		Canvas canvas = gameObject.AddComponent<Canvas>();
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		canvas.overrideSorting = true;
		canvas.sortingOrder = 500;
		if (transform == null)
		{
			CanvasScaler canvasScaler = gameObject.AddComponent<CanvasScaler>();
			canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
			canvasScaler.scaleFactor = ResolutionConfig.GetUiScaleFactor();
		}
		gameObject.AddComponent<GraphicRaycaster>();
		gameObject.AddComponent<GamepadNavigationController>();
		UISteamWorkshopCreatorWindow uisteamWorkshopCreatorWindow = gameObject.AddComponent<UISteamWorkshopCreatorWindow>();
		uisteamWorkshopCreatorWindow.Init();
		return uisteamWorkshopCreatorWindow;
	}

	// Token: 0x06003295 RID: 12949 RVA: 0x000F35B0 File Offset: 0x000F17B0
	private static Transform FindUiRoot()
	{
		GUIElements guielements = GUIElements.Instance;
		if (guielements == null)
		{
			return null;
		}
		UIFitter privateField = UISteamWorkshopCreatorWindow.GetPrivateField<UIFitter>(guielements, "uiFitter");
		if (!(privateField != null))
		{
			return guielements.Root;
		}
		return privateField.transform;
	}

	// Token: 0x06003296 RID: 12950 RVA: 0x000F35F0 File Offset: 0x000F17F0
	private void Awake()
	{
		this.service = new SteamWorkshopCreatorService();
		this.service.Logged += this.AppendConsole;
		this.service.ItemsChanged += this.RefreshTiles;
		this.ResolveUiTemplates();
		this.BuildUi();
	}

	// Token: 0x06003297 RID: 12951 RVA: 0x000F3642 File Offset: 0x000F1842
	protected override void Update()
	{
		base.Update();
		SteamWorkshopCreatorService steamWorkshopCreatorService = this.service;
		if (steamWorkshopCreatorService == null)
		{
			return;
		}
		steamWorkshopCreatorService.Tick();
	}

	// Token: 0x06003298 RID: 12952 RVA: 0x000F365A File Offset: 0x000F185A
	public override void Open(LazyWidgetDataBase data)
	{
		base.Open(data);
		this.HideCreatePopup();
		this.RefreshTiles();
		this.AppendConsole("Steam Workshop Creator ready.");
	}

	// Token: 0x06003299 RID: 12953 RVA: 0x000F367A File Offset: 0x000F187A
	public override void Close()
	{
		this.HideCreatePopup();
		base.Close();
	}

	// Token: 0x0600329A RID: 12954 RVA: 0x000F3688 File Offset: 0x000F1888
	protected override bool OnPressedBack()
	{
		if (this.createOverlay != null && this.createOverlay.activeSelf)
		{
			this.HideCreatePopup();
			return true;
		}
		return base.OnPressedBack();
	}

	// Token: 0x0600329B RID: 12955 RVA: 0x000F36B4 File Offset: 0x000F18B4
	private void OnDestroy()
	{
		if (this.service != null)
		{
			this.service.Logged -= this.AppendConsole;
			this.service.ItemsChanged -= this.RefreshTiles;
			this.service.Dispose();
			this.service = null;
		}
		if (UISteamWorkshopCreatorWindow.instance == this)
		{
			UISteamWorkshopCreatorWindow.instance = null;
		}
	}

	// Token: 0x0600329C RID: 12956 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x0600329D RID: 12957 RVA: 0x000F371C File Offset: 0x000F191C
	private void ResolveUiTemplates()
	{
		UIDialogWindow uidialogWindow = UISteamWorkshopCreatorWindow.FindDialogWindow();
		if (uidialogWindow == null)
		{
			return;
		}
		this.headerSource = UISteamWorkshopCreatorWindow.GetPrivateField<TextMeshProUGUI>(uidialogWindow, "header");
		this.bodySource = UISteamWorkshopCreatorWindow.GetPrivateField<TextMeshProUGUI>(uidialogWindow, "informationBotText");
		if (this.bodySource == null)
		{
			this.bodySource = UISteamWorkshopCreatorWindow.GetPrivateField<TextMeshProUGUI>(uidialogWindow, "information");
		}
		UIDialogWindowButton privateField = UISteamWorkshopCreatorWindow.GetPrivateField<UIDialogWindowButton>(uidialogWindow, "buttonPrefab");
		if (privateField != null)
		{
			LazyButton lazyButton = privateField.LazyButton;
			if (lazyButton != null)
			{
				Image image = lazyButton.targetGraphic as Image;
				if (image != null)
				{
					this.buttonGraphicTemplate = image;
				}
			}
			if (this.buttonGraphicTemplate == null)
			{
				this.buttonGraphicTemplate = privateField.GetComponentInChildren<Image>(true);
			}
			if (lazyButton != null)
			{
				List<TextTransition> privateField2 = UISteamWorkshopCreatorWindow.GetPrivateField<List<TextTransition>>(lazyButton, "textTransitions");
				if (privateField2 != null && privateField2.Count > 0)
				{
					this.buttonTextTransition = privateField2[0];
					this.buttonStyle = this.buttonTextTransition.defaultStyle;
					this.buttonDisabledStyle = this.buttonTextTransition.disabledStyle;
				}
			}
			if (this.buttonStyle == null)
			{
				TextMeshProUGUI privateField3 = UISteamWorkshopCreatorWindow.GetPrivateField<TextMeshProUGUI>(privateField, "label");
				if (privateField3 != null)
				{
					TextStyleComponent component = privateField3.GetComponent<TextStyleComponent>();
					if (component != null)
					{
						this.buttonStyle = component.CurrentTextStyle;
					}
				}
			}
		}
		if (this.buttonStyle == null)
		{
			this.buttonStyle = UISteamWorkshopCreatorWindow.FindTextStyleByName("small_font_bold-btn_red_active");
		}
		if (this.headerSource != null)
		{
			TextStyleComponent component2 = this.headerSource.GetComponent<TextStyleComponent>();
			if (component2 != null)
			{
				this.headerStyle = component2.CurrentTextStyle;
			}
		}
		if (this.bodySource != null)
		{
			TextStyleComponent component3 = this.bodySource.GetComponent<TextStyleComponent>();
			if (component3 != null)
			{
				this.bodyStyle = component3.CurrentTextStyle;
			}
		}
	}

	// Token: 0x0600329E RID: 12958 RVA: 0x000F38F0 File Offset: 0x000F1AF0
	private static T GetPrivateField<T>(object obj, string name) where T : class
	{
		Type type = obj.GetType();
		while (type != null)
		{
			FieldInfo field = type.GetField(name, UISteamWorkshopCreatorWindow.PrivateInstance);
			if (field != null)
			{
				return field.GetValue(obj) as T;
			}
			type = type.BaseType;
		}
		return default(T);
	}

	// Token: 0x0600329F RID: 12959 RVA: 0x000F3948 File Offset: 0x000F1B48
	private static UIDialogWindow FindDialogWindow()
	{
		UIDialogWindow uidialogWindow;
		try
		{
			uidialogWindow = LazyUI.GetWindow<UIDialogWindow>();
		}
		catch
		{
			uidialogWindow = null;
		}
		return uidialogWindow;
	}

	// Token: 0x060032A0 RID: 12960 RVA: 0x000F3974 File Offset: 0x000F1B74
	private void BuildUi()
	{
		RectTransform rectTransform = (RectTransform)base.transform;
		rectTransform.anchorMin = Vector2.zero;
		rectTransform.anchorMax = Vector2.one;
		rectTransform.offsetMin = Vector2.zero;
		rectTransform.offsetMax = Vector2.zero;
		Image image = base.gameObject.AddComponent<Image>();
		image.color = new Color(0.05f, 0.03f, 0.02f, 0.92f);
		image.raycastTarget = true;
		RectTransform rectTransform2 = this.CreatePanel(rectTransform, "Frame", new Color(0.16f, 0.11f, 0.08f, 0.98f));
		UISteamWorkshopCreatorWindow.Stretch(rectTransform2, Vector2.zero, Vector2.one, new Vector2(16f, 16f), new Vector2(-16f, -16f));
		RectTransform rectTransform3 = this.CreatePanel(rectTransform2, "Header", new Color(0.22f, 0.14f, 0.1f, 1f));
		UISteamWorkshopCreatorWindow.Stretch(rectTransform3, new Vector2(0f, 1f), Vector2.one, new Vector2(0f, -40f), Vector2.zero);
		UISteamWorkshopCreatorWindow.Stretch(this.CreateLabel(rectTransform3, "Title", "Steam Workshop Creator Interface", TextAlignmentOptions.MidlineLeft, true).rectTransform, Vector2.zero, Vector2.one, new Vector2(16f, 0f), new Vector2(-48f, 0f));
		this.closeButton = this.CreateRedButton(rectTransform3, "CloseButton", "X");
		UISteamWorkshopCreatorWindow.PlaceCloseButton((RectTransform)this.closeButton.transform);
		RectTransform rectTransform4 = this.CreateRect(rectTransform2, "Body");
		UISteamWorkshopCreatorWindow.Stretch(rectTransform4, Vector2.zero, Vector2.one, new Vector2(12f, 12f), new Vector2(-12f, -48f));
		RectTransform rectTransform5 = this.CreatePanel(rectTransform4, "TilesArea", new Color(0.12f, 0.08f, 0.06f, 1f));
		UISteamWorkshopCreatorWindow.Stretch(rectTransform5, new Vector2(0f, 0.3f), Vector2.one, Vector2.zero, Vector2.zero);
		RectTransform rectTransform6;
		UISteamWorkshopCreatorWindow.Stretch((RectTransform)UISteamWorkshopCreatorWindow.CreateScroll(rectTransform5, "TilesScroll", out rectTransform6, out this.tilesContent).transform, Vector2.zero, Vector2.one, new Vector2(8f, 8f), new Vector2(-8f, -8f));
		UISteamWorkshopCreatorWindow.Stretch(rectTransform6, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
		VerticalLayoutGroup verticalLayoutGroup = this.tilesContent.gameObject.AddComponent<VerticalLayoutGroup>();
		verticalLayoutGroup.spacing = 6f;
		verticalLayoutGroup.padding = new RectOffset(8, 8, 8, 8);
		verticalLayoutGroup.childAlignment = TextAnchor.UpperCenter;
		verticalLayoutGroup.childControlHeight = false;
		verticalLayoutGroup.childControlWidth = true;
		verticalLayoutGroup.childForceExpandHeight = false;
		verticalLayoutGroup.childForceExpandWidth = true;
		this.tilesContent.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
		UISteamWorkshopCreatorWindow.PinTopStretch(this.tilesContent);
		RectTransform rectTransform7 = this.CreatePanel(rectTransform4, "ConsoleArea", new Color(0.07f, 0.06f, 0.05f, 1f));
		UISteamWorkshopCreatorWindow.Stretch(rectTransform7, Vector2.zero, new Vector2(1f, 0.3f), Vector2.zero, Vector2.zero);
		UISteamWorkshopCreatorWindow.Stretch(this.CreateLabel(rectTransform7, "ConsoleHeader", "Console", TextAlignmentOptions.MidlineLeft, true).rectTransform, new Vector2(0f, 1f), Vector2.one, new Vector2(12f, -28f), new Vector2(-12f, 0f));
		RectTransform rectTransform8;
		RectTransform rectTransform9;
		this.consoleScroll = UISteamWorkshopCreatorWindow.CreateScroll(rectTransform7, "ConsoleScroll", out rectTransform8, out rectTransform9);
		UISteamWorkshopCreatorWindow.Stretch((RectTransform)this.consoleScroll.transform, Vector2.zero, Vector2.one, new Vector2(8f, 8f), new Vector2(-8f, -32f));
		UISteamWorkshopCreatorWindow.Stretch(rectTransform8, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
		this.consoleText = this.CreateLabel(rectTransform8, "ConsoleText", "", TextAlignmentOptions.TopLeft, false);
		this.consoleText.enableWordWrapping = true;
		this.consoleText.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
		UISteamWorkshopCreatorWindow.PinTopStretch(this.consoleText.rectTransform);
		Vector2 offsetMin = this.consoleText.rectTransform.offsetMin;
		offsetMin.x = 4f;
		this.consoleText.rectTransform.offsetMin = offsetMin;
		this.consoleScroll.content = this.consoleText.rectTransform;
		this.BuildCreateOverlay(rectTransform);
	}

	// Token: 0x060032A1 RID: 12961 RVA: 0x000F3E08 File Offset: 0x000F2008
	private void BuildCreateOverlay(RectTransform root)
	{
		this.createOverlay = this.CreatePanel(root, "CreateOverlay", new Color(0f, 0f, 0f, 0.55f)).gameObject;
		UISteamWorkshopCreatorWindow.Stretch((RectTransform)this.createOverlay.transform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
		RectTransform rectTransform = this.CreatePanel(this.createOverlay.transform, "CreatePopup", new Color(0.18f, 0.12f, 0.09f, 1f));
		rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
		rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
		rectTransform.pivot = new Vector2(0.5f, 0.5f);
		rectTransform.sizeDelta = new Vector2(520f, 360f);
		RectTransform rectTransform2 = this.CreatePanel(rectTransform, "PopupHeader", new Color(0.24f, 0.15f, 0.11f, 1f));
		UISteamWorkshopCreatorWindow.Stretch(rectTransform2, new Vector2(0f, 1f), Vector2.one, new Vector2(0f, -40f), Vector2.zero);
		UISteamWorkshopCreatorWindow.Stretch(this.CreateLabel(rectTransform2, "PopupTitle", "Create workshop item", TextAlignmentOptions.MidlineLeft, true).rectTransform, Vector2.zero, Vector2.one, new Vector2(12f, 0f), new Vector2(-44f, 0f));
		LazyButton lazyButton = this.CreateRedButton(rectTransform2, "PopupClose", "X");
		lazyButton.onClick.AddListener(new UnityAction(this.HideCreatePopup));
		UISteamWorkshopCreatorWindow.PlaceCloseButton((RectTransform)lazyButton.transform);
		UISteamWorkshopCreatorWindow.Stretch(this.CreateLabel(rectTransform, "NameLabel", "Name", TextAlignmentOptions.MidlineLeft, false).rectTransform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(16f, -72f), new Vector2(-16f, -48f));
		this.nameInput = this.CreateInputField(rectTransform, "NameInput");
		UISteamWorkshopCreatorWindow.Stretch((RectTransform)this.nameInput.transform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(16f, -108f), new Vector2(-16f, -76f));
		this.nameInput.onValueChanged.AddListener(delegate(string _)
		{
			this.RefreshCreateButtonState();
		});
		this.tagSwitch = this.CreateTagSwitch(rectTransform);
		if (this.tagSwitch != null)
		{
			UISteamWorkshopCreatorWindow.PlaceTagSwitch((RectTransform)this.tagSwitch.transform);
		}
		else
		{
			UISteamWorkshopCreatorWindow.Stretch(this.CreateLabel(rectTransform, "TagLabel", "Type", TextAlignmentOptions.MidlineLeft, false).rectTransform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(16f, -140f), new Vector2(-16f, -116f));
			LazyButton lazyButton2 = this.CreateRedButton(rectTransform, "TagButton", "Translation");
			lazyButton2.onClick.AddListener(new UnityAction(this.CycleTag));
			UISteamWorkshopCreatorWindow.Stretch((RectTransform)lazyButton2.transform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(16f, -176f), new Vector2(-16f, -144f));
			this.tagValueLabel = UISteamWorkshopCreatorWindow.GetButtonLabel(lazyButton2);
		}
		TextMeshProUGUI textMeshProUGUI = this.CreateLabel(rectTransform, "FolderHelp", "Linked folder: Steam uploads this directory as the workshop item content. Pick the folder that contains your files.", TextAlignmentOptions.TopLeft, false);
		textMeshProUGUI.enableWordWrapping = true;
		UISteamWorkshopCreatorWindow.Stretch(textMeshProUGUI.rectTransform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(16f, -228f), new Vector2(-16f, -176f));
		this.folderPathLabel = this.CreateLabel(rectTransform, "FolderPath", "No folder selected", TextAlignmentOptions.MidlineLeft, false);
		this.folderPathLabel.enableWordWrapping = false;
		this.folderPathLabel.overflowMode = TextOverflowModes.Ellipsis;
		UISteamWorkshopCreatorWindow.Stretch(this.folderPathLabel.rectTransform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(16f, -264f), new Vector2(-56f, -232f));
		LazyButton lazyButton3 = this.CreateRedButton(rectTransform, "BrowseButton", "...");
		lazyButton3.onClick.AddListener(new UnityAction(this.BrowseFolder));
		RectTransform rectTransform3 = (RectTransform)lazyButton3.transform;
		rectTransform3.anchorMin = new Vector2(1f, 1f);
		rectTransform3.anchorMax = new Vector2(1f, 1f);
		rectTransform3.pivot = new Vector2(1f, 1f);
		rectTransform3.anchoredPosition = new Vector2(-16f, -232f);
		UISteamWorkshopCreatorWindow.SizeButtonToText(lazyButton3, 26f);
		this.createSubmitButton = this.CreateRedButton(rectTransform, "CreateButton", "Create");
		this.createSubmitButton.onClick.AddListener(new UnityAction(this.OnCreatePressed));
		RectTransform rectTransform4 = (RectTransform)this.createSubmitButton.transform;
		rectTransform4.anchorMin = new Vector2(0.5f, 0f);
		rectTransform4.anchorMax = new Vector2(0.5f, 0f);
		rectTransform4.pivot = new Vector2(0.5f, 0f);
		rectTransform4.anchoredPosition = new Vector2(0f, 16f);
		rectTransform4.sizeDelta = new Vector2(280f, 26f);
		this.createOverlay.SetActive(false);
	}

	// Token: 0x060032A2 RID: 12962 RVA: 0x000F43E4 File Offset: 0x000F25E4
	private void RefreshTiles()
	{
		if (this.tilesContent == null || this.service == null)
		{
			return;
		}
		for (int i = this.tilesContent.childCount - 1; i >= 0; i--)
		{
			global::UnityEngine.Object.Destroy(this.tilesContent.GetChild(i).gameObject);
		}
		this.CreateCreateTile();
		foreach (SteamWorkshopItemRecord steamWorkshopItemRecord in this.service.Items)
		{
			this.CreateItemTile(steamWorkshopItemRecord);
		}
	}

	// Token: 0x060032A3 RID: 12963 RVA: 0x000F4484 File Offset: 0x000F2684
	private void CreateCreateTile()
	{
		RectTransform rectTransform = this.CreatePanel(this.tilesContent, "CreateTile", new Color(0.28f, 0.18f, 0.12f, 1f));
		rectTransform.sizeDelta = new Vector2(0f, 50f);
		LayoutElement layoutElement = rectTransform.gameObject.AddComponent<LayoutElement>();
		layoutElement.minHeight = 50f;
		layoutElement.preferredHeight = 50f;
		LazyButton lazyButton = this.CreateRedButton(rectTransform, "CreateNewButton", "Create new workshop item");
		lazyButton.onClick.AddListener(new UnityAction(this.ShowCreatePopup));
		UISteamWorkshopCreatorWindow.Stretch((RectTransform)lazyButton.transform, Vector2.zero, Vector2.one, new Vector2(12f, 7f), new Vector2(-12f, -7f));
	}

	// Token: 0x060032A4 RID: 12964 RVA: 0x000F4550 File Offset: 0x000F2750
	private void CreateItemTile(SteamWorkshopItemRecord record)
	{
		RectTransform rectTransform = this.CreatePanel(this.tilesContent, "ItemTile", new Color(0.24f, 0.17f, 0.13f, 1f));
		rectTransform.sizeDelta = new Vector2(0f, 50f);
		LayoutElement layoutElement = rectTransform.gameObject.AddComponent<LayoutElement>();
		layoutElement.minHeight = 50f;
		layoutElement.preferredHeight = 50f;
		UISteamWorkshopCreatorWindow.Stretch(this.CreateLabel(rectTransform, "ItemTitle", record.title, TextAlignmentOptions.MidlineLeft, true).rectTransform, Vector2.zero, Vector2.one, new Vector2(12f, 25f), new Vector2(-180f, -2f));
		TextMeshProUGUI textMeshProUGUI = this.CreateLabel(rectTransform, "ItemMeta", string.Format("{0}  {1}", record.tag, record.publishedFileId), TextAlignmentOptions.MidlineLeft, false);
		textMeshProUGUI.enableWordWrapping = false;
		textMeshProUGUI.overflowMode = TextOverflowModes.Ellipsis;
		UISteamWorkshopCreatorWindow.Stretch(textMeshProUGUI.rectTransform, Vector2.zero, Vector2.one, new Vector2(12f, 2f), new Vector2(-180f, -25f));
		LazyButton lazyButton = this.CreateRedButton(rectTransform, "DeleteButton", "Delete");
		lazyButton.onClick.AddListener(delegate
		{
			this.ConfirmDelete(record);
		});
		UISteamWorkshopCreatorWindow.PlaceRightButton((RectTransform)lazyButton.transform, -8f);
		UISteamWorkshopCreatorWindow.SizeButtonToText(lazyButton, 48f);
		LazyButton lazyButton2 = this.CreateRedButton(rectTransform, "UploadButton", "Upload");
		lazyButton2.onClick.AddListener(delegate
		{
			this.service.Upload(record);
		});
		float x = ((RectTransform)lazyButton.transform).sizeDelta.x;
		UISteamWorkshopCreatorWindow.PlaceRightButton((RectTransform)lazyButton2.transform, -16f - x);
		UISteamWorkshopCreatorWindow.SizeButtonToText(lazyButton2, 48f);
	}

	// Token: 0x060032A5 RID: 12965 RVA: 0x000F4740 File Offset: 0x000F2940
	private void ShowCreatePopup()
	{
		this.selectedFolder = "";
		this.tagIndex = 0;
		if (this.nameInput != null)
		{
			this.nameInput.text = "";
		}
		if (this.tagSwitch != null)
		{
			this.tagSwitch.UpdateField(0, false);
		}
		if (this.tagValueLabel != null)
		{
			this.tagValueLabel.text = UISteamWorkshopCreatorWindow.TagOptions[this.tagIndex];
		}
		if (this.folderPathLabel != null)
		{
			this.folderPathLabel.text = "No folder selected";
		}
		this.RefreshCreateButtonState();
		this.createOverlay.SetActive(true);
		this.createOverlay.transform.SetAsLastSibling();
	}

	// Token: 0x060032A6 RID: 12966 RVA: 0x000F47FD File Offset: 0x000F29FD
	private void HideCreatePopup()
	{
		if (this.createOverlay != null)
		{
			this.createOverlay.SetActive(false);
		}
	}

	// Token: 0x060032A7 RID: 12967 RVA: 0x000F481C File Offset: 0x000F2A1C
	private UISwitchButton CreateTagSwitch(Transform parent)
	{
		UISwitchButton uiswitchButton = null;
		try
		{
			UIGameSettingsWindow window = LazyUI.GetWindow<UIGameSettingsWindow>();
			if (window != null)
			{
				uiswitchButton = UISteamWorkshopCreatorWindow.GetPrivateField<UISwitchButton>(window, "languageButton") ?? UISteamWorkshopCreatorWindow.GetPrivateField<UISwitchButton>(window, "fullscreenButton");
			}
		}
		catch
		{
			uiswitchButton = null;
		}
		if (uiswitchButton == null)
		{
			return null;
		}
		UISwitchButton uiswitchButton2 = global::UnityEngine.Object.Instantiate<UISwitchButton>(uiswitchButton, parent, false);
		uiswitchButton2.name = "TagSwitch";
		uiswitchButton2.gameObject.SetActive(true);
		LocalizedLabel[] componentsInChildren = uiswitchButton2.GetComponentsInChildren<LocalizedLabel>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].IgnoreLocalize = true;
		}
		foreach (TMP_Text tmp_Text in uiswitchButton2.GetComponentsInChildren<TMP_Text>(true))
		{
			if (tmp_Text.GetComponent<StaticFontLabel>() == null)
			{
				tmp_Text.gameObject.AddComponent<StaticFontLabel>();
			}
		}
		uiswitchButton2.Initialize(delegate(int index)
		{
			this.tagIndex = index;
			this.RefreshCreateButtonState();
		}, UISteamWorkshopCreatorWindow.TagOptions, 0, "Type", null, null, true);
		return uiswitchButton2;
	}

	// Token: 0x060032A8 RID: 12968 RVA: 0x000F491C File Offset: 0x000F2B1C
	private static void PlaceTagSwitch(RectTransform rect)
	{
		rect.localScale = Vector3.one;
		rect.anchorMin = new Vector2(0f, 1f);
		rect.anchorMax = new Vector2(0f, 1f);
		rect.pivot = new Vector2(0f, 0.5f);
		rect.sizeDelta = new Vector2(330f, 16f);
		rect.anchoredPosition = new Vector2(16f, -148f);
	}

	// Token: 0x060032A9 RID: 12969 RVA: 0x000F49A0 File Offset: 0x000F2BA0
	private void CycleTag()
	{
		this.tagIndex = (this.tagIndex + 1) % UISteamWorkshopCreatorWindow.TagOptions.Length;
		if (this.tagValueLabel != null)
		{
			this.tagValueLabel.text = UISteamWorkshopCreatorWindow.TagOptions[this.tagIndex];
		}
		this.RefreshCreateButtonState();
	}

	// Token: 0x060032AA RID: 12970 RVA: 0x000F49F0 File Offset: 0x000F2BF0
	private void RefreshCreateButtonState()
	{
		if (this.createSubmitButton == null)
		{
			return;
		}
		bool flag = this.nameInput != null && !string.IsNullOrWhiteSpace(this.nameInput.text);
		bool flag2 = Directory.Exists(this.selectedFolder);
		bool flag3 = !string.IsNullOrWhiteSpace(this.GetSelectedTag());
		this.createSubmitButton.interactable = flag && flag2 && flag3;
	}

	// Token: 0x060032AB RID: 12971 RVA: 0x000F4A5C File Offset: 0x000F2C5C
	private string GetSelectedTag()
	{
		return UISteamWorkshopCreatorWindow.TagOptions[Mathf.Clamp(this.tagIndex, 0, UISteamWorkshopCreatorWindow.TagOptions.Length - 1)];
	}

	// Token: 0x060032AC RID: 12972 RVA: 0x000F4A7C File Offset: 0x000F2C7C
	private void BrowseFolder()
	{
		string text = UISteamWorkshopCreatorWindow.PickFolder(Directory.Exists(this.selectedFolder) ? this.selectedFolder : ModsPaths.Root);
		if (string.IsNullOrEmpty(text) || !Directory.Exists(text))
		{
			return;
		}
		this.selectedFolder = text;
		if (this.folderPathLabel != null)
		{
			this.folderPathLabel.text = this.selectedFolder;
		}
		this.service.EnsureThumbnail(this.selectedFolder);
		this.RefreshCreateButtonState();
	}

	// Token: 0x060032AD RID: 12973 RVA: 0x000F4AF8 File Offset: 0x000F2CF8
	private static string PickFolder(string startDirectory)
	{
		return Win32FolderPicker.PickFolder("Select workshop item folder", startDirectory);
	}

	// Token: 0x060032AE RID: 12974 RVA: 0x000F4B08 File Offset: 0x000F2D08
	private void OnCreatePressed()
	{
		string text = ((this.nameInput != null) ? this.nameInput.text.Trim() : "");
		string selectedTag = this.GetSelectedTag();
		string text2 = this.selectedFolder;
		this.HideCreatePopup();
		this.service.CreateAndUpload(text, selectedTag, text2);
	}

	// Token: 0x060032AF RID: 12975 RVA: 0x000F4B60 File Offset: 0x000F2D60
	private void ConfirmDelete(SteamWorkshopItemRecord record)
	{
		UISteamWorkshopCreatorWindow.<>c__DisplayClass63_0 CS$<>8__locals1 = new UISteamWorkshopCreatorWindow.<>c__DisplayClass63_0();
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.record = record;
		CS$<>8__locals1.dialog = UISteamWorkshopCreatorWindow.FindDialogWindow();
		if (CS$<>8__locals1.dialog == null)
		{
			this.service.Delete(CS$<>8__locals1.record);
			return;
		}
		UIDialogWindowData uidialogWindowData = new UIDialogWindowData("Delete workshop item", "This deletes the item on Steam and removes it from the local creator list. Files in the linked folder are not deleted.", new Action(CS$<>8__locals1.<ConfirmDelete>g__Yes|0), new Action(CS$<>8__locals1.dialog.Close), false);
		uidialogWindowData.ShowCloseButton = true;
		CS$<>8__locals1.dialog.Open(uidialogWindowData);
	}

	// Token: 0x060032B0 RID: 12976 RVA: 0x000F4BF0 File Offset: 0x000F2DF0
	private void AppendConsole(string line)
	{
		if (string.IsNullOrEmpty(line))
		{
			return;
		}
		this.consoleLines.Add(string.Format("[{0:HH:mm:ss}] {1}", DateTime.Now, line));
		while (this.consoleLines.Count > 400)
		{
			this.consoleLines.RemoveAt(0);
		}
		this.consoleBuilder.Clear();
		for (int i = 0; i < this.consoleLines.Count; i++)
		{
			if (i > 0)
			{
				this.consoleBuilder.Append('\n');
			}
			this.consoleBuilder.Append(this.consoleLines[i]);
		}
		if (this.consoleText != null)
		{
			this.consoleText.text = this.consoleBuilder.ToString();
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.consoleText.rectTransform);
		}
		if (this.consoleScroll != null)
		{
			Canvas.ForceUpdateCanvases();
			this.consoleScroll.verticalNormalizedPosition = 0f;
		}
	}

	// Token: 0x060032B1 RID: 12977 RVA: 0x000F4CEC File Offset: 0x000F2EEC
	private RectTransform CreateRect(Transform parent, string name)
	{
		GameObject gameObject = new GameObject(name, new Type[] { typeof(RectTransform) });
		gameObject.transform.SetParent(parent, false);
		gameObject.transform.localScale = Vector3.one;
		return (RectTransform)gameObject.transform;
	}

	// Token: 0x060032B2 RID: 12978 RVA: 0x000F4D39 File Offset: 0x000F2F39
	private RectTransform CreatePanel(Transform parent, string name, Color color)
	{
		RectTransform rectTransform = this.CreateRect(parent, name);
		Image image = rectTransform.gameObject.AddComponent<Image>();
		image.color = color;
		image.raycastTarget = true;
		return rectTransform;
	}

	// Token: 0x060032B3 RID: 12979 RVA: 0x000F4D5C File Offset: 0x000F2F5C
	private TextMeshProUGUI CreateLabel(Transform parent, string name, string text, TextAlignmentOptions alignment, bool header = false)
	{
		TextMeshProUGUI textMeshProUGUI = this.CreateRect(parent, name).gameObject.AddComponent<TextMeshProUGUI>();
		textMeshProUGUI.text = text;
		textMeshProUGUI.alignment = alignment;
		textMeshProUGUI.raycastTarget = false;
		textMeshProUGUI.enableAutoSizing = false;
		if (textMeshProUGUI.GetComponent<StaticFontLabel>() == null)
		{
			textMeshProUGUI.gameObject.AddComponent<StaticFontLabel>();
		}
		this.ApplyGameTextStyle(textMeshProUGUI, header);
		return textMeshProUGUI;
	}

	// Token: 0x060032B4 RID: 12980 RVA: 0x000F4DC0 File Offset: 0x000F2FC0
	private void ApplyGameTextStyle(TextMeshProUGUI label, bool header)
	{
		TextStyle textStyle = (header ? this.headerStyle : this.bodyStyle);
		TextMeshProUGUI textMeshProUGUI = (header ? this.headerSource : this.bodySource);
		if (textStyle == null && !header)
		{
			textStyle = this.headerStyle;
			textMeshProUGUI = this.headerSource;
		}
		if (textStyle != null)
		{
			textStyle.ApplyStyle(label, false, null, null, null);
		}
		else if (textMeshProUGUI != null)
		{
			label.font = textMeshProUGUI.font;
			label.fontSharedMaterial = textMeshProUGUI.fontSharedMaterial;
			label.color = textMeshProUGUI.color;
			label.extraPadding = textMeshProUGUI.extraPadding;
			label.fontSize = textMeshProUGUI.fontSize;
		}
		label.enableAutoSizing = false;
		label.ForceMeshUpdate(false, false);
	}

	// Token: 0x060032B5 RID: 12981 RVA: 0x000F4E90 File Offset: 0x000F3090
	private LazyButton CreateRedButton(Transform parent, string name, string text)
	{
		RectTransform rectTransform = this.CreateRect(parent, name);
		Image image = rectTransform.gameObject.AddComponent<Image>();
		this.ApplyButtonGraphic(image);
		image.raycastTarget = true;
		LazyButton lazyButton = rectTransform.gameObject.AddComponent<LazyButton>();
		lazyButton.targetGraphic = image;
		rectTransform.gameObject.AddComponent<GamepadNavigationItem>();
		TextMeshProUGUI textMeshProUGUI = this.CreateLabel(rectTransform, "Label", text, TextAlignmentOptions.Center, false);
		textMeshProUGUI.enableWordWrapping = false;
		textMeshProUGUI.overflowMode = TextOverflowModes.Overflow;
		UISteamWorkshopCreatorWindow.Stretch(textMeshProUGUI.rectTransform, Vector2.zero, Vector2.one, new Vector2(10f, 0f), new Vector2(-10f, 0f));
		this.BindButtonText(lazyButton, textMeshProUGUI);
		rectTransform.sizeDelta = new Vector2(Mathf.Max(textMeshProUGUI.preferredWidth + 24f, 48f), 26f);
		return lazyButton;
	}

	// Token: 0x060032B6 RID: 12982 RVA: 0x000F4F64 File Offset: 0x000F3164
	private void BindButtonText(LazyButton button, TextMeshProUGUI label)
	{
		TextStyle textStyle = this.buttonStyle ?? UISteamWorkshopCreatorWindow.FindTextStyleByName("small_font_bold-btn_red_active");
		if (textStyle != null)
		{
			textStyle.ApplyStyle(label, false, null, null, null);
		}
		List<TextTransition> privateField = UISteamWorkshopCreatorWindow.GetPrivateField<List<TextTransition>>(button, "textTransitions");
		if (privateField != null)
		{
			privateField.Clear();
			List<TextTransition> list = privateField;
			TextTransition textTransition = new TextTransition();
			textTransition.targetLabel = label;
			TextTransition textTransition2 = this.buttonTextTransition;
			textTransition.defaultStyle = ((textTransition2 != null) ? textTransition2.defaultStyle : null) ?? textStyle;
			TextTransition textTransition3 = this.buttonTextTransition;
			textTransition.highlightedStyle = ((textTransition3 != null) ? textTransition3.highlightedStyle : null) ?? textStyle;
			TextTransition textTransition4 = this.buttonTextTransition;
			textTransition.pressedStyle = ((textTransition4 != null) ? textTransition4.pressedStyle : null) ?? textStyle;
			TextTransition textTransition5 = this.buttonTextTransition;
			textTransition.selectedStyle = ((textTransition5 != null) ? textTransition5.selectedStyle : null) ?? textStyle;
			TextTransition textTransition6 = this.buttonTextTransition;
			TextStyle textStyle2;
			if ((textStyle2 = ((textTransition6 != null) ? textTransition6.disabledStyle : null)) == null)
			{
				textStyle2 = this.buttonDisabledStyle ?? textStyle;
			}
			textTransition.disabledStyle = textStyle2;
			list.Add(textTransition);
			button.RefreshTextTransitions();
		}
		label.enableAutoSizing = false;
		label.ForceMeshUpdate(false, false);
	}

	// Token: 0x060032B7 RID: 12983 RVA: 0x000F508C File Offset: 0x000F328C
	private static TextStyle FindTextStyleByName(string name)
	{
		TextStyle[] array = Resources.FindObjectsOfTypeAll<TextStyle>();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != null && array[i].name == name)
			{
				return array[i];
			}
		}
		return null;
	}

	// Token: 0x060032B8 RID: 12984 RVA: 0x000F50D0 File Offset: 0x000F32D0
	private void ApplyButtonGraphic(Image image)
	{
		if (this.buttonGraphicTemplate == null)
		{
			image.color = new Color(0.72f, 0.16f, 0.12f, 1f);
			return;
		}
		image.sprite = this.buttonGraphicTemplate.sprite;
		image.type = this.buttonGraphicTemplate.type;
		image.fillCenter = this.buttonGraphicTemplate.fillCenter;
		image.pixelsPerUnitMultiplier = this.buttonGraphicTemplate.pixelsPerUnitMultiplier;
		image.material = this.buttonGraphicTemplate.material;
		image.color = this.buttonGraphicTemplate.color;
		image.preserveAspect = false;
	}

	// Token: 0x060032B9 RID: 12985 RVA: 0x000F5178 File Offset: 0x000F3378
	private static TextMeshProUGUI GetButtonLabel(LazyButton button)
	{
		return button.GetComponentInChildren<TextMeshProUGUI>(true);
	}

	// Token: 0x060032BA RID: 12986 RVA: 0x000F5184 File Offset: 0x000F3384
	private static void PlaceCloseButton(RectTransform rect)
	{
		rect.anchorMin = new Vector2(1f, 0.5f);
		rect.anchorMax = new Vector2(1f, 0.5f);
		rect.pivot = new Vector2(1f, 0.5f);
		rect.anchoredPosition = new Vector2(-6f, 0f);
		rect.sizeDelta = new Vector2(26f, 26f);
	}

	// Token: 0x060032BB RID: 12987 RVA: 0x000F51FC File Offset: 0x000F33FC
	private static void PlaceRightButton(RectTransform rect, float xFromRight)
	{
		rect.anchorMin = new Vector2(1f, 0.5f);
		rect.anchorMax = new Vector2(1f, 0.5f);
		rect.pivot = new Vector2(1f, 0.5f);
		rect.anchoredPosition = new Vector2(xFromRight, 0f);
	}

	// Token: 0x060032BC RID: 12988 RVA: 0x000F525C File Offset: 0x000F345C
	private static void SizeButtonToText(LazyButton button, float minWidth = 48f)
	{
		TextMeshProUGUI buttonLabel = UISteamWorkshopCreatorWindow.GetButtonLabel(button);
		float num = minWidth;
		if (buttonLabel != null)
		{
			num = Mathf.Max(minWidth, buttonLabel.preferredWidth + 24f);
		}
		((RectTransform)button.transform).sizeDelta = new Vector2(num, 26f);
	}

	// Token: 0x060032BD RID: 12989 RVA: 0x000F52AC File Offset: 0x000F34AC
	private TMP_InputField CreateInputField(Transform parent, string name)
	{
		RectTransform rectTransform = this.CreatePanel(parent, name, new Color(0.08f, 0.06f, 0.05f, 1f));
		RectTransform rectTransform2 = this.CreateRect(rectTransform, "Text Area");
		UISteamWorkshopCreatorWindow.Stretch(rectTransform2, Vector2.zero, Vector2.one, new Vector2(6f, 4f), new Vector2(-6f, -4f));
		rectTransform2.gameObject.AddComponent<RectMask2D>();
		TextMeshProUGUI textMeshProUGUI = this.CreateLabel(rectTransform2, "Placeholder", "", TextAlignmentOptions.MidlineLeft, false);
		UISteamWorkshopCreatorWindow.Stretch(textMeshProUGUI.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
		textMeshProUGUI.enableWordWrapping = false;
		textMeshProUGUI.overflowMode = TextOverflowModes.Overflow;
		Color color = textMeshProUGUI.color;
		color.a = 0.45f;
		textMeshProUGUI.color = color;
		TextMeshProUGUI textMeshProUGUI2 = this.CreateLabel(rectTransform2, "Text", "", TextAlignmentOptions.MidlineLeft, false);
		UISteamWorkshopCreatorWindow.Stretch(textMeshProUGUI2.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
		textMeshProUGUI2.enableWordWrapping = false;
		textMeshProUGUI2.overflowMode = TextOverflowModes.Overflow;
		textMeshProUGUI2.raycastTarget = true;
		textMeshProUGUI2.margin = new Vector4(2f, 0f, 0f, 0f);
		textMeshProUGUI.margin = new Vector4(2f, 0f, 0f, 0f);
		TMP_FontAsset font = textMeshProUGUI2.font;
		Material fontSharedMaterial = textMeshProUGUI2.fontSharedMaterial;
		float num = ((textMeshProUGUI2.fontSize > 0f) ? textMeshProUGUI2.fontSize : 16f);
		TMP_InputField tmp_InputField = rectTransform.gameObject.AddComponent<TMP_InputField>();
		tmp_InputField.textViewport = rectTransform2;
		tmp_InputField.textComponent = textMeshProUGUI2;
		tmp_InputField.placeholder = textMeshProUGUI;
		if (font != null)
		{
			tmp_InputField.fontAsset = font;
		}
		UISteamWorkshopCreatorWindow.RestoreLabelFont(textMeshProUGUI2, font, fontSharedMaterial, num);
		UISteamWorkshopCreatorWindow.RestoreLabelFont(textMeshProUGUI, font, fontSharedMaterial, num);
		textMeshProUGUI2.margin = new Vector4(2f, 0f, 0f, 0f);
		textMeshProUGUI.margin = new Vector4(2f, 0f, 0f, 0f);
		tmp_InputField.customCaretColor = true;
		tmp_InputField.caretColor = Color.white;
		tmp_InputField.caretWidth = 2;
		tmp_InputField.lineType = TMP_InputField.LineType.SingleLine;
		return tmp_InputField;
	}

	// Token: 0x060032BE RID: 12990 RVA: 0x000F54F8 File Offset: 0x000F36F8
	private static void RestoreLabelFont(TextMeshProUGUI label, TMP_FontAsset font, Material material, float fontSize)
	{
		if (font != null)
		{
			label.font = font;
		}
		if (material != null)
		{
			label.fontSharedMaterial = material;
		}
		label.fontSize = fontSize;
		label.enableAutoSizing = false;
		label.enableWordWrapping = false;
		label.ForceMeshUpdate(false, false);
	}

	// Token: 0x060032BF RID: 12991 RVA: 0x000F5538 File Offset: 0x000F3738
	private static ScrollRect CreateScroll(Transform parent, string name, out RectTransform viewport, out RectTransform content)
	{
		GameObject gameObject = new GameObject(name, new Type[] { typeof(RectTransform) });
		gameObject.transform.SetParent(parent, false);
		gameObject.transform.localScale = Vector3.one;
		ScrollRect scrollRect = gameObject.AddComponent<ScrollRect>();
		scrollRect.horizontal = false;
		scrollRect.vertical = true;
		scrollRect.movementType = ScrollRect.MovementType.Clamped;
		scrollRect.scrollSensitivity = 24f;
		gameObject.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.15f);
		GameObject gameObject2 = new GameObject("Viewport", new Type[] { typeof(RectTransform) });
		gameObject2.transform.SetParent(gameObject.transform, false);
		gameObject2.transform.localScale = Vector3.one;
		viewport = (RectTransform)gameObject2.transform;
		gameObject2.AddComponent<RectMask2D>();
		gameObject2.AddComponent<Image>().color = new Color(1f, 1f, 1f, 0.02f);
		scrollRect.viewport = viewport;
		GameObject gameObject3 = new GameObject("Content", new Type[] { typeof(RectTransform) });
		gameObject3.transform.SetParent(gameObject2.transform, false);
		gameObject3.transform.localScale = Vector3.one;
		content = (RectTransform)gameObject3.transform;
		scrollRect.content = content;
		return scrollRect;
	}

	// Token: 0x060032C0 RID: 12992 RVA: 0x000F56A0 File Offset: 0x000F38A0
	private static void PinTopStretch(RectTransform rect)
	{
		rect.anchorMin = new Vector2(0f, 1f);
		rect.anchorMax = new Vector2(1f, 1f);
		rect.pivot = new Vector2(0.5f, 1f);
		rect.anchoredPosition = Vector2.zero;
		rect.sizeDelta = new Vector2(0f, 0f);
	}

	// Token: 0x060032C1 RID: 12993 RVA: 0x000F570C File Offset: 0x000F390C
	private static void Stretch(RectTransform rect, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
	{
		rect.anchorMin = anchorMin;
		rect.anchorMax = anchorMax;
		rect.offsetMin = offsetMin;
		rect.offsetMax = offsetMax;
	}

	// Token: 0x04002893 RID: 10387
	private const string HeaderTitle = "Steam Workshop Creator Interface";

	// Token: 0x04002894 RID: 10388
	private const int MaxConsoleLines = 400;

	// Token: 0x04002895 RID: 10389
	private const string TagTranslation = "Translation";

	// Token: 0x04002896 RID: 10390
	private const string TagVoiceOver = "Voiceover";

	// Token: 0x04002897 RID: 10391
	private const float FontSize = 16f;

	// Token: 0x04002898 RID: 10392
	private const float ButtonHeight = 26f;

	// Token: 0x04002899 RID: 10393
	private const float CloseButtonSize = 26f;

	// Token: 0x0400289A RID: 10394
	private const float ItemTileHeight = 50f;

	// Token: 0x0400289B RID: 10395
	private const bool EnableVoiceOverWorkshopType = false;

	// Token: 0x0400289C RID: 10396
	private static readonly string[] TagOptions = new string[] { "Translation" };

	// Token: 0x0400289D RID: 10397
	private static readonly BindingFlags PrivateInstance = BindingFlags.Instance | BindingFlags.NonPublic;

	// Token: 0x0400289E RID: 10398
	private static UISteamWorkshopCreatorWindow instance;

	// Token: 0x0400289F RID: 10399
	private SteamWorkshopCreatorService service;

	// Token: 0x040028A0 RID: 10400
	private readonly List<string> consoleLines = new List<string>();

	// Token: 0x040028A1 RID: 10401
	private readonly StringBuilder consoleBuilder = new StringBuilder();

	// Token: 0x040028A2 RID: 10402
	private const string ButtonTextStyleName = "small_font_bold-btn_red_active";

	// Token: 0x040028A3 RID: 10403
	private TextStyle headerStyle;

	// Token: 0x040028A4 RID: 10404
	private TextStyle bodyStyle;

	// Token: 0x040028A5 RID: 10405
	private TextStyle buttonStyle;

	// Token: 0x040028A6 RID: 10406
	private TextStyle buttonDisabledStyle;

	// Token: 0x040028A7 RID: 10407
	private TextTransition buttonTextTransition;

	// Token: 0x040028A8 RID: 10408
	private TextMeshProUGUI headerSource;

	// Token: 0x040028A9 RID: 10409
	private TextMeshProUGUI bodySource;

	// Token: 0x040028AA RID: 10410
	private Image buttonGraphicTemplate;

	// Token: 0x040028AB RID: 10411
	private RectTransform tilesContent;

	// Token: 0x040028AC RID: 10412
	private TextMeshProUGUI consoleText;

	// Token: 0x040028AD RID: 10413
	private ScrollRect consoleScroll;

	// Token: 0x040028AE RID: 10414
	private GameObject createOverlay;

	// Token: 0x040028AF RID: 10415
	private TMP_InputField nameInput;

	// Token: 0x040028B0 RID: 10416
	private int tagIndex;

	// Token: 0x040028B1 RID: 10417
	private TextMeshProUGUI tagValueLabel;

	// Token: 0x040028B2 RID: 10418
	private UISwitchButton tagSwitch;

	// Token: 0x040028B3 RID: 10419
	private TextMeshProUGUI folderPathLabel;

	// Token: 0x040028B4 RID: 10420
	private LazyButton createSubmitButton;

	// Token: 0x040028B5 RID: 10421
	private string selectedFolder = "";
}
