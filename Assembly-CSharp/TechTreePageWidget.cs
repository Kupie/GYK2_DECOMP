using System;
using System.Collections.Generic;
using DG.Tweening;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

// Token: 0x0200095B RID: 2395
public class TechTreePageWidget : LazyWidget<TechTreePageWidgetData>
{
	// Token: 0x17000986 RID: 2438
	// (get) Token: 0x06003F20 RID: 16160 RVA: 0x0012E184 File Offset: 0x0012C384
	public static Pool UnlocksPool
	{
		get
		{
			return TechTreePageWidget.unlocksPool;
		}
	}

	// Token: 0x06003F21 RID: 16161 RVA: 0x0012E18C File Offset: 0x0012C38C
	public override void Init()
	{
		base.Init();
		TechDef.InitTechs();
		this.tabButtonPrefab.gameObject.SetActive(false);
		string[] names = Enum.GetNames(typeof(TechTreeTab));
		int num = 0;
		foreach (object obj in Enum.GetValues(typeof(TechTreeTab)))
		{
			TechTreeTabButton techTreeTabButton = this.tabButtonPrefab.Copy(this.tabButtonPrefab.transform.parent, true, "");
			string text = "tech_tab_" + names[num];
			techTreeTabButton.Init((TechTreeTab)obj, LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(text, "i_tech_tree_tab_placeholder"), text, new Action<TechTreeTabButton>(this.OnTechTreeTabButtonClicked));
			num++;
			techTreeTabButton.gameObject.SetActive(true);
			this.tabButtons.Add(techTreeTabButton);
		}
		this.scrollRect.Init(new Func<LazyScrollableElement, LazyWidgetBase>(this.GetTechWidget), new Action<LazyScrollableElement>(this.ReleaseWidgetForParent), this.gamepadNavigationController, null, null, null, null, null);
		SmoothMouseWheelScroll.Ensure(this.scrollRect, null);
		TechTreePageWidget.unlocksPool = new Pool(this.unlockWidgetPrefab, base.transform, 0, Pool.PoolType.ImmediateActivation, false, null);
	}

	// Token: 0x06003F22 RID: 16162 RVA: 0x0012E2F4 File Offset: 0x0012C4F4
	private void OnDestroy()
	{
		TechTreePageWidget.unlocksPool = null;
	}

	// Token: 0x06003F23 RID: 16163 RVA: 0x0012E2FC File Offset: 0x0012C4FC
	public override void Redraw()
	{
		base.Redraw();
		this.RedrawSpheres();
		for (int i = 0; i < this.tabButtons.Count; i++)
		{
			this.tabButtons[i].gameObject.SetActive(MainGame.Instance.GameSave.knowledgeSystem.IsTechTabUnlocked(this.tabButtons[i].TechTreeTab));
		}
		this.UpdateGamepadDependentStuff();
	}

	// Token: 0x06003F24 RID: 16164 RVA: 0x0012E36C File Offset: 0x0012C56C
	private void RedrawSpheres()
	{
		this.redSpheresLabel.text = Param.FormIconFromPlayerRes("tech_red");
		this.greenSpheresLabel.text = Param.FormIconFromPlayerRes("tech_green");
		this.blueSpheresLabel.text = Param.FormIconFromPlayerRes("tech_blue");
	}

	// Token: 0x06003F25 RID: 16165 RVA: 0x0012E3B8 File Offset: 0x0012C5B8
	public override void Hide()
	{
		this.HideCurrentElements();
		base.Hide();
	}

	// Token: 0x06003F26 RID: 16166 RVA: 0x0012E3C6 File Offset: 0x0012C5C6
	public void DisplayLastTab(string focusOnTech = "")
	{
		this.DisplayTab(this.currentTab, focusOnTech);
	}

	// Token: 0x06003F27 RID: 16167 RVA: 0x0012E3D8 File Offset: 0x0012C5D8
	public void DisplayTab(TechTreeTab tab, string focusOnTech = "")
	{
		this.currentTab = tab;
		foreach (TechTreeTabButton techTreeTabButton in this.tabButtons)
		{
			techTreeTabButton.SetState(techTreeTabButton.TechTreeTab == this.currentTab);
		}
		if (this.smallVersionTabLabel != null)
		{
			this.smallVersionTabLabel.text = LLBase.L(string.Format("tech_tab_{0}", this.currentTab));
		}
		this.HideCurrentElements();
		this.HideConnectors();
		List<TechDef> list = new List<TechDef>();
		foreach (TechDef techDef in GameBalance.Me.techDefs)
		{
			bool flag = TechTreePageWidget.ShouldDisplayTech(techDef);
			TechTreePageWidget.UnlockReputationTechIfAvailable(techDef, flag);
			if (techDef.tab == this.currentTab && flag)
			{
				list.Add(techDef);
			}
		}
		LazyScrollableElement lazyScrollableElement = null;
		float num = 255f;
		float num2 = -255f;
		for (int i = 0; i < list.Count; i++)
		{
			TechDef techDef2 = list[i];
			TechDefType techDefType = techDef2.techDefType;
			LazyScrollableElement lazyScrollableElement2;
			if (techDefType == TechDefType.CharRep || techDefType == TechDefType.DisRep)
			{
				TechTreeCharReputationWidgetData techTreeCharReputationWidgetData = new TechTreeCharReputationWidgetData();
				techTreeCharReputationWidgetData.onTechClicked = new Action<TechTreeElementBaseWidgetData>(this.OnTechClicked);
				techTreeCharReputationWidgetData.techDef = techDef2;
				lazyScrollableElement2 = this.scrollRect.AddScrollableElement(techTreeCharReputationWidgetData);
				lazyScrollableElement2.RectTransform.sizeDelta = this.techElementSizeRep;
			}
			else
			{
				TechTreeElementWidgetData techTreeElementWidgetData = new TechTreeElementWidgetData();
				techTreeElementWidgetData.techDef = techDef2;
				techTreeElementWidgetData.onTechClicked = new Action<TechTreeElementBaseWidgetData>(this.OnTechClicked);
				lazyScrollableElement2 = this.scrollRect.AddScrollableElement(techTreeElementWidgetData);
				lazyScrollableElement2.RectTransform.sizeDelta = this.techElementSize;
			}
			lazyScrollableElement2.transform.SetSiblingIndex(i);
			if (techDef2.TreePos.x < num)
			{
				lazyScrollableElement = lazyScrollableElement2;
				num = techDef2.TreePos.x;
				num2 = techDef2.TreePos.y;
			}
			else if (techDef2.TreePos.y > num2 && techDef2.TreePos.x <= num)
			{
				lazyScrollableElement = lazyScrollableElement2;
				num = techDef2.TreePos.x;
				num2 = techDef2.TreePos.y;
			}
		}
		this.UpdateRectContentSize();
		foreach (LazyScrollableElement lazyScrollableElement3 in this.scrollRect.DisplayingElements)
		{
			TechTreeElementBaseWidgetData techTreeElementBaseWidgetData = lazyScrollableElement3.Data as TechTreeElementBaseWidgetData;
			TechDef techDef3 = techTreeElementBaseWidgetData.techDef;
			bool flag2 = techDef3.techDefType == TechDefType.Common;
			lazyScrollableElement3.RectTransform.localPosition = new Vector2(techDef3.TreePos.x * this.technologyOffset.x + this.edgeOffset.x + (flag2 ? 0f : this.repWidgetOffset.x), Mathf.Ceil(techDef3.TreePos.y * this.technologyOffset.y + this.edgeOffset.y + (flag2 ? 0f : this.repWidgetOffset.y)));
			techTreeElementBaseWidgetData.rightConnectorPos = lazyScrollableElement3.RectTransform.localPosition + (flag2 ? this.commonWidgetConnectorPortData.right : this.repWidgetConnectorPortData.right);
			techTreeElementBaseWidgetData.leftConnectorPos = lazyScrollableElement3.RectTransform.localPosition + (flag2 ? this.commonWidgetConnectorPortData.left : this.repWidgetConnectorPortData.left);
			techTreeElementBaseWidgetData.downConnectorPos = lazyScrollableElement3.RectTransform.localPosition + (flag2 ? this.commonWidgetConnectorPortData.down : this.repWidgetConnectorPortData.down);
			techTreeElementBaseWidgetData.upConnectorPos = lazyScrollableElement3.RectTransform.localPosition + (flag2 ? this.commonWidgetConnectorPortData.up : this.repWidgetConnectorPortData.up);
		}
		((RectTransform)base.transform).RefreshContentFitter();
		for (int j = 0; j < this.scrollRect.DisplayingElements.Count; j++)
		{
			TechDef techDef4 = (this.scrollRect.DisplayingElements[j].Data as TechTreeElementBaseWidgetData).techDef;
			List<TechDef> childDefinitionList = techDef4.childDefinitionList;
			for (int k = 0; k < childDefinitionList.Count; k++)
			{
				TechDef techDef5 = childDefinitionList[k];
				if (techDef5.tab == this.currentTab && TechTreePageWidget.ShouldDisplayTech(techDef5))
				{
					TechTreeConnector elementFromPool = UIPrefabsPooler.Instance.GetElementFromPool<TechTreeConnector>(this.scrollRect.content);
					TechTreeConnector elementFromPool2 = UIPrefabsPooler.Instance.GetElementFromPool<TechTreeConnector>(this.scrollRect.content);
					this.techConnectors.Add(elementFromPool);
					this.techConnectors.Add(elementFromPool2);
					LazyScrollableElement lazyScrollableElement4 = this.FindTechByDefinition(techDef5);
					if (lazyScrollableElement4 == null)
					{
						Debug.LogError(string.Concat(new string[] { "Can't find child element:[", techDef5.id, "] for:[", techDef4.id, "]" }));
					}
					elementFromPool.Draw(this.scrollRect.DisplayingElements[j], lazyScrollableElement4, false);
					elementFromPool2.Draw(this.scrollRect.DisplayingElements[j], lazyScrollableElement4, true);
					elementFromPool.transform.SetParent(elementFromPool.IsConnectorActive ? this.activeConnectorsContent.transform : this.inactiveConnectorsContent.transform);
					elementFromPool.transform.SetAsFirstSibling();
					elementFromPool2.transform.SetParent(this.backgroundConnectorsContent.transform);
					elementFromPool2.transform.SetAsFirstSibling();
				}
			}
		}
		this.scrollRect.CheckVisibility();
		if (LazyInput.IsGamepadActive)
		{
			this.autoScroll.SkipNextAutoscroll = true;
			this.gamepadNavigationController.ReinitItems(false, null, null);
		}
		LazyScrollableElement lazyScrollableElement5 = null;
		TechDef techDef6 = (string.IsNullOrEmpty(focusOnTech) ? null : GameBalance.Me.GetData<TechDef>(focusOnTech));
		if (techDef6 != null && TechTreePageWidget.ShouldDisplayTech(techDef6))
		{
			lazyScrollableElement5 = this.FindTechByDefinition(techDef6);
		}
		if (lazyScrollableElement5 != null)
		{
			this.scrollRect.DOKill(false);
			this.gamepadNavigationController.SetFocusedItem(lazyScrollableElement5.GamepadNavigationItem.GetComponent<GamepadNavigationItem>());
			this.scrollRect.DOKill(false);
			this.scrollRect.ScrollToTargetInstant(lazyScrollableElement5.RectTransform, RectTransform.Axis.Horizontal);
			return;
		}
		if (lazyScrollableElement != null)
		{
			this.gamepadNavigationController.SetFocusedItem(lazyScrollableElement.GamepadNavigationItem.GetComponent<GamepadNavigationItem>());
		}
		else
		{
			this.gamepadNavigationController.FocusOnFirstActive(-1);
		}
		this.scrollRect.DOKill(false);
		this.scrollRect.horizontalNormalizedPosition = 0f;
	}

	// Token: 0x06003F28 RID: 16168 RVA: 0x0003C7FE File Offset: 0x0003A9FE
	private static bool ShouldDisplayTech(TechDef techDef)
	{
		return true;
	}

	// Token: 0x06003F29 RID: 16169 RVA: 0x0012EB1C File Offset: 0x0012CD1C
	private static void UnlockReputationTechIfAvailable(TechDef definition, bool shouldDisplayTech)
	{
		if (shouldDisplayTech && TechTreePageWidget.IsTechAvailableInCurrentBuild(definition) && (definition.techDefType == TechDefType.CharRep || definition.techDefType == TechDefType.DisRep) && definition.TechState == TechState.Available && definition.EnoughResources)
		{
			definition.Unlock(false);
		}
	}

	// Token: 0x06003F2A RID: 16170 RVA: 0x0003C7FE File Offset: 0x0003A9FE
	private static bool IsTechAvailableInCurrentBuild(TechDef techDef)
	{
		return true;
	}

	// Token: 0x06003F2B RID: 16171 RVA: 0x0012EB54 File Offset: 0x0012CD54
	private static void UnlockAvailableReputationTechs()
	{
		foreach (TechDef techDef in GameBalance.Me.techDefs)
		{
			TechTreePageWidget.UnlockReputationTechIfAvailable(techDef, TechTreePageWidget.ShouldDisplayTech(techDef));
		}
	}

	// Token: 0x06003F2C RID: 16172 RVA: 0x0012EBB0 File Offset: 0x0012CDB0
	private void HideConnectors()
	{
		foreach (TechTreeConnector techTreeConnector in this.techConnectors)
		{
			UIPrefabsPooler.Instance.ReleaseElementToPool<TechTreeConnector>(techTreeConnector);
		}
		this.techConnectors.Clear();
	}

	// Token: 0x06003F2D RID: 16173 RVA: 0x0012EC14 File Offset: 0x0012CE14
	private LazyScrollableElement FindTechByDefinition(TechDef techDef)
	{
		foreach (LazyScrollableElement lazyScrollableElement in this.scrollRect.DisplayingElements)
		{
			if ((lazyScrollableElement.Data as TechTreeElementBaseWidgetData).techDef == techDef)
			{
				return lazyScrollableElement;
			}
		}
		Debug.Log("Cannot find element for definition " + techDef.id);
		return null;
	}

	// Token: 0x06003F2E RID: 16174 RVA: 0x0012EC94 File Offset: 0x0012CE94
	private TechTreeElementBaseWidget GetTechWidget(LazyScrollableElement parent)
	{
		TechDefType techDefType = (parent.Data as TechTreeElementBaseWidgetData).techDef.techDefType;
		if (techDefType - TechDefType.CharRep <= 1)
		{
			TechTreeCharReputationWidget elementFromPool = UIPrefabsPooler.Instance.GetElementFromPool<TechTreeCharReputationWidget>(parent.RectTransform);
			elementFromPool.Init();
			elementFromPool.Draw(parent.Data);
			parent.GamepadNavigationItem.SetCallbacks(new UnityAction(elementFromPool.button.ForceOnEnter), new UnityAction(elementFromPool.button.ForceOnExit), new UnityAction(elementFromPool.button.ForceOnClick));
			parent.transform.SetParent(this.techLayerFront.transform);
			return elementFromPool;
		}
		TechTreeElementWidget elementFromPool2 = UIPrefabsPooler.Instance.GetElementFromPool<TechTreeElementWidget>(parent.RectTransform);
		elementFromPool2.Init();
		elementFromPool2.Draw(parent.Data);
		parent.GamepadNavigationItem.SetCallbacks(new UnityAction(elementFromPool2.button.ForceOnEnter), new UnityAction(elementFromPool2.button.ForceOnExit), new UnityAction(elementFromPool2.button.ForceOnClick));
		parent.transform.SetParent(this.techLayerFront.transform);
		elementFromPool2.Background.transform.SetParent(this.techLayerBack.transform);
		elementFromPool2.Background.transform.position = parent.transform.position;
		return elementFromPool2;
	}

	// Token: 0x06003F2F RID: 16175 RVA: 0x0012EDE4 File Offset: 0x0012CFE4
	private void ReleaseWidgetForParent(LazyScrollableElement parent)
	{
		TechTreeCharReputationWidget techTreeCharReputationWidget = parent.Widget as TechTreeCharReputationWidget;
		if (techTreeCharReputationWidget != null)
		{
			this.ReleaseReputationWidget(techTreeCharReputationWidget);
			return;
		}
		this.ReleaseCommonWidget(parent.Widget as TechTreeElementWidget);
	}

	// Token: 0x06003F30 RID: 16176 RVA: 0x0012EE19 File Offset: 0x0012D019
	private void ReleaseReputationWidget(TechTreeCharReputationWidget reputationWidget)
	{
		reputationWidget.DeInit();
		reputationWidget.Hide();
		UIPrefabsPooler.Instance.ReleaseElementToPool<TechTreeCharReputationWidget>(reputationWidget);
	}

	// Token: 0x06003F31 RID: 16177 RVA: 0x0012EE32 File Offset: 0x0012D032
	private void ReleaseCommonWidget(TechTreeElementWidget techTreeElementWidget)
	{
		techTreeElementWidget.DeInit();
		techTreeElementWidget.Hide();
		techTreeElementWidget.Background.transform.SetParent(techTreeElementWidget.transform.GetChild(0).transform);
		UIPrefabsPooler.Instance.ReleaseElementToPool<TechTreeElementWidget>(techTreeElementWidget);
	}

	// Token: 0x06003F32 RID: 16178 RVA: 0x0012EE6C File Offset: 0x0012D06C
	private void UpdateRectContentSize()
	{
		if (this.scrollRect.DisplayingElements.Count == 0)
		{
			return;
		}
		float num = 0f;
		float num2 = 0f;
		foreach (LazyScrollableElement lazyScrollableElement in this.scrollRect.DisplayingElements)
		{
			TechDef techDef = (lazyScrollableElement.Data as TechTreeElementBaseWidgetData).techDef;
			if (techDef == null)
			{
				Debug.Log(string.Format("data is null?:[{0}]", lazyScrollableElement.Data == null));
			}
			if (techDef.TreePos.x > num)
			{
				num = techDef.TreePos.x;
			}
			if (techDef.TreePos.y > num2)
			{
				num2 = techDef.TreePos.y;
			}
		}
		this.scrollRect.content.sizeDelta = new Vector2(num * this.technologyOffset.x + this.edgeOffset.x * 2f + this.edgeOffsetContentAdditional + this.techElementSize.x, this.scrollRect.content.sizeDelta.y);
	}

	// Token: 0x06003F33 RID: 16179 RVA: 0x0012EFA4 File Offset: 0x0012D1A4
	private void HideCurrentElements()
	{
		this.scrollRect.ClearDisplayingScrollableElements();
	}

	// Token: 0x06003F34 RID: 16180 RVA: 0x0012EFB1 File Offset: 0x0012D1B1
	private void OnTechTreeTabButtonClicked(TechTreeTabButton techTreeTabButton)
	{
		if (this.currentTab != techTreeTabButton.TechTreeTab)
		{
			this.DisplayTab(techTreeTabButton.TechTreeTab, "");
		}
	}

	// Token: 0x06003F35 RID: 16181 RVA: 0x0012EFD4 File Offset: 0x0012D1D4
	public bool OnPressedPrevTechTab()
	{
		int num = (int)this.currentTab;
		do
		{
			num--;
			if (num < 0)
			{
				num = this.tabButtons.Count - 1;
			}
		}
		while (MainGame.Instance.GameSave.knowledgeSystem.IsTechTabLocked((TechTreeTab)num));
		this.DisplayTab((TechTreeTab)num, "");
		return true;
	}

	// Token: 0x06003F36 RID: 16182 RVA: 0x0012F024 File Offset: 0x0012D224
	public bool OnPressedNextTechTab()
	{
		int num = (int)this.currentTab;
		do
		{
			num++;
			if (num > this.tabButtons.Count - 1)
			{
				num = 0;
			}
		}
		while (MainGame.Instance.GameSave.knowledgeSystem.IsTechTabLocked((TechTreeTab)num));
		this.DisplayTab((TechTreeTab)num, "");
		return true;
	}

	// Token: 0x06003F37 RID: 16183 RVA: 0x0012F074 File Offset: 0x0012D274
	public void UpdateElements()
	{
		foreach (LazyScrollableElement lazyScrollableElement in this.scrollRect.DisplayingElements)
		{
			if (lazyScrollableElement.Widget != null)
			{
				lazyScrollableElement.Widget.Redraw();
			}
		}
	}

	// Token: 0x06003F38 RID: 16184 RVA: 0x0012F0E0 File Offset: 0x0012D2E0
	private void UpdateConnectors()
	{
		foreach (TechTreeConnector techTreeConnector in this.techConnectors)
		{
			techTreeConnector.UpdateState();
		}
	}

	// Token: 0x06003F39 RID: 16185 RVA: 0x0012F130 File Offset: 0x0012D330
	private void OnTechClicked(TechTreeElementBaseWidgetData techData)
	{
		TechTreePageWidget.<>c__DisplayClass54_0 CS$<>8__locals1 = new TechTreePageWidget.<>c__DisplayClass54_0();
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.techDef = techData.techDef;
		if (!MainGame.Instance.GameSave.knowledgeSystem.IsTechUnlocked(CS$<>8__locals1.techDef.id))
		{
			if (techData.techDef.techDefType == TechDefType.Common)
			{
				LazyWindow<UITechTreeElementWindowData> window = LazyUI.GetWindow<UITechTreeElementWindow>();
				UIDialogWindowData.ButtonData buttonData = new UIDialogWindowData.ButtonData(new Action(CS$<>8__locals1.<OnTechClicked>g__Unlock|0), LLBase.L("btn_unlock"), () => CS$<>8__locals1.techDef.TechState == TechState.Available && CS$<>8__locals1.techDef.EnoughResources, true, GameKey.Select, "");
				UIDialogWindowData.ButtonData buttonData2 = new UIDialogWindowData.ButtonData(new Action(LazyUI.GetWindow<UITechTreeElementWindow>().Close), LLBase.L("btn_cancel"), null, true, GameKey.Back, "");
				window.Open(new UITechTreeElementWindowData(CS$<>8__locals1.techDef, new List<UIDialogWindowData.ButtonData> { buttonData, buttonData2 }, CS$<>8__locals1.techDef.ParentsUnlocked ? string.Empty : LLBase.L("tech_not_all_techs_unlocked"), null));
				return;
			}
		}
		else if (techData.techDef.techDefType == TechDefType.Common)
		{
			LazyWindow<UITechTreeElementWindowData> window2 = LazyUI.GetWindow<UITechTreeElementWindow>();
			UIDialogWindowData.ButtonData buttonData3 = new UIDialogWindowData.ButtonData(new Action(LazyUI.GetWindow<UITechTreeElementWindow>().Close), LLBase.L("btn_ok"), null, true, GameKey.Select, "");
			window2.Open(new UITechTreeElementWindowData(CS$<>8__locals1.techDef, new List<UIDialogWindowData.ButtonData> { buttonData3 }, null, null));
		}
	}

	// Token: 0x06003F3A RID: 16186 RVA: 0x0012F290 File Offset: 0x0012D490
	public void UpdateGamepadDependentStuff()
	{
		bool isGamepadActive = LazyInput.IsGamepadActive;
		if (this.nextSubTabGamepadHelper != null)
		{
			this.nextSubTabGamepadHelper.gameObject.SetActive(isGamepadActive);
			if (isGamepadActive)
			{
				this.nextSubTabGamepadHelper.text = ControllerIconLibrary.GetIconId(GameKey.NextSubTab, null, true);
			}
		}
		if (this.prevSubTabGamepadHelper != null)
		{
			this.prevSubTabGamepadHelper.gameObject.SetActive(isGamepadActive);
			if (isGamepadActive)
			{
				this.prevSubTabGamepadHelper.text = ControllerIconLibrary.GetIconId(GameKey.PrevSubTab, null, true);
			}
		}
	}

	// Token: 0x06003F3B RID: 16187 RVA: 0x0012F318 File Offset: 0x0012D518
	public override List<LazyGameKeyTip> GetTips(GamepadNavigationItem gamepadNavigationItem)
	{
		List<LazyGameKeyTip> list = new List<LazyGameKeyTip>();
		if (gamepadNavigationItem != null)
		{
			list.Add(LazyGameKeyTip.Select(true, true, true));
		}
		return list;
	}

	// Token: 0x06003F3C RID: 16188 RVA: 0x0012F343 File Offset: 0x0012D543
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Draw(new TechTreePageWidgetData());
	}

	// Token: 0x040031AB RID: 12715
	[SerializeField]
	private GamepadNavigationController gamepadNavigationController;

	// Token: 0x040031AC RID: 12716
	[SerializeField]
	private TextMeshProUGUI redSpheresLabel;

	// Token: 0x040031AD RID: 12717
	[SerializeField]
	private TextMeshProUGUI greenSpheresLabel;

	// Token: 0x040031AE RID: 12718
	[SerializeField]
	private TextMeshProUGUI blueSpheresLabel;

	// Token: 0x040031AF RID: 12719
	[SerializeField]
	private GameObject activeConnectorsContent;

	// Token: 0x040031B0 RID: 12720
	[SerializeField]
	private GameObject inactiveConnectorsContent;

	// Token: 0x040031B1 RID: 12721
	[SerializeField]
	private GameObject backgroundConnectorsContent;

	// Token: 0x040031B2 RID: 12722
	[SerializeField]
	private GameObject techLayerBack;

	// Token: 0x040031B3 RID: 12723
	[SerializeField]
	private GameObject techLayerFront;

	// Token: 0x040031B4 RID: 12724
	[SerializeField]
	private LinkedEntityWidget unlockWidgetPrefab;

	// Token: 0x040031B5 RID: 12725
	[SerializeField]
	private TextMeshProUGUI nextSubTabGamepadHelper;

	// Token: 0x040031B6 RID: 12726
	[SerializeField]
	private TextMeshProUGUI prevSubTabGamepadHelper;

	// Token: 0x040031B7 RID: 12727
	[SerializeField]
	private TextMeshProUGUI smallVersionTabLabel;

	// Token: 0x040031B8 RID: 12728
	[SerializeField]
	[Space]
	private TechTreeTabButton tabButtonPrefab;

	// Token: 0x040031B9 RID: 12729
	private List<TechTreeTabButton> tabButtons = new List<TechTreeTabButton>();

	// Token: 0x040031BA RID: 12730
	[Space]
	private TechTreeTab currentTab;

	// Token: 0x040031BB RID: 12731
	[Space]
	[SerializeField]
	private LazyScrollRect scrollRect;

	// Token: 0x040031BC RID: 12732
	[SerializeField]
	private AutoScroll autoScroll;

	// Token: 0x040031BD RID: 12733
	[Space]
	[SerializeField]
	private TechTreeConnectorPortData commonWidgetConnectorPortData;

	// Token: 0x040031BE RID: 12734
	[SerializeField]
	private TechTreeConnectorPortData repWidgetConnectorPortData;

	// Token: 0x040031BF RID: 12735
	[Space]
	[SerializeField]
	private Vector2 edgeOffset = new Vector2(10f, 10f);

	// Token: 0x040031C0 RID: 12736
	[SerializeField]
	private float edgeOffsetContentAdditional;

	// Token: 0x040031C1 RID: 12737
	[SerializeField]
	private Vector2 technologyOffset = new Vector2(210f, 80f);

	// Token: 0x040031C2 RID: 12738
	[SerializeField]
	private Vector2 repWidgetOffset = new Vector2(54f, 16f);

	// Token: 0x040031C3 RID: 12739
	[SerializeField]
	private Vector2 techElementSize = new Vector2(160f, 80f);

	// Token: 0x040031C4 RID: 12740
	[SerializeField]
	private Vector2 techElementSizeRep = new Vector2(46f, 50f);

	// Token: 0x040031C5 RID: 12741
	private List<TechTreeConnector> techConnectors = new List<TechTreeConnector>();

	// Token: 0x040031C6 RID: 12742
	private static Pool unlocksPool;
}
