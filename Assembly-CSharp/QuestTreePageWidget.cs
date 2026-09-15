using System;
using System.Collections.Generic;
using DG.Tweening;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000950 RID: 2384
public class QuestTreePageWidget : LazyWidget<QuestTreePageWidgetData>
{
	// Token: 0x06003EDE RID: 16094 RVA: 0x0012C5DC File Offset: 0x0012A7DC
	public override void Init()
	{
		base.Init();
		QuestDef.LinkQuests();
		this.scrollRect.Init(new Func<LazyScrollableElement, LazyWidgetBase>(this.GetQuestWidget), new Action<LazyScrollableElement>(this.ReleaseWidgetForParent), this.gamepadNavigationController, null, null, null, null, null);
		SmoothMouseWheelScroll.Ensure(this.scrollRect, null);
	}

	// Token: 0x06003EDF RID: 16095 RVA: 0x0012C638 File Offset: 0x0012A838
	public static void OpenWindowFromScratchOnSelectedQuest(QuestData questData)
	{
		if (MainGame.Instance.GameSave.knowledgeSystem.IsCharTabLocked(CharacterWindowData.CharPage.QuestTree))
		{
			return;
		}
		CharacterWindowData characterWindowData = new CharacterWindowData(MainGame.Instance.GameSave, CharacterWindowData.CharPage.QuestTree, null);
		characterWindowData.FocusOnQuest = questData.id;
		LazyUI.GetWindow<CharacterWindow>().Open(characterWindowData);
		LazyWindow<UIQuestInfoWindowData> window = LazyUI.GetWindow<UIQuestInfoWindow>();
		UIQuestInfoWindowData uiquestInfoWindowData = new UIQuestInfoWindowData(questData);
		window.Open(uiquestInfoWindowData);
	}

	// Token: 0x06003EE0 RID: 16096 RVA: 0x0012C698 File Offset: 0x0012A898
	public override void Hide()
	{
		this.HideCurrentElements();
		base.Hide();
	}

	// Token: 0x06003EE1 RID: 16097 RVA: 0x0012C6A8 File Offset: 0x0012A8A8
	public void Display(string focusOnQuest = "")
	{
		this.HideCurrentElements();
		this.HideConnectors();
		HashSet<string> hashSet = new HashSet<string>();
		for (int i = 0; i < MainGame.Instance.GameSave.questSystemData.questCollection.quests.Count; i++)
		{
			QuestData questData = MainGame.Instance.GameSave.questSystemData.questCollection.quests[i];
			if (!questData.isHidden && (questData.status != QuestStatus.Completed || !QuestTreePageWidget.HasActiveBrother(questData)) && !QuestTreePageWidget.IsAnyBrotherAlreadyAdded(questData, hashSet))
			{
				hashSet.Add(questData.id);
				QuestTreeElementWidgetData questTreeElementWidgetData = new QuestTreeElementWidgetData();
				questTreeElementWidgetData.questData = questData;
				questTreeElementWidgetData.onQuestClicked = new Action<QuestTreeElementWidgetData>(this.OnQuestClicked);
				LazyScrollableElement lazyScrollableElement = this.scrollRect.AddScrollableElement(questTreeElementWidgetData);
				lazyScrollableElement.transform.SetSiblingIndex(i + 4);
				lazyScrollableElement.RectTransform.sizeDelta = this.elementSize;
			}
		}
		if (!string.IsNullOrEmpty(focusOnQuest))
		{
			this.scrollRect.DOKill(false);
			LazyScrollableElement lazyScrollableElement2 = this.FindQuestByDefinition(GameBalance.Me.GetData<QuestDef>(focusOnQuest));
			this.scrollRect.ScrollToTargetInstant(lazyScrollableElement2.RectTransform, RectTransform.Axis.Vertical);
		}
		else
		{
			this.scrollRect.verticalNormalizedPosition = 0f;
		}
		this.UpdateRectContentSize();
		this.ApplyDisplayViewStatuses();
		foreach (LazyScrollableElement lazyScrollableElement3 in this.scrollRect.DisplayingElements)
		{
			QuestTreeElementWidgetData questTreeElementWidgetData2 = lazyScrollableElement3.Data as QuestTreeElementWidgetData;
			QuestData questData2 = questTreeElementWidgetData2.questData;
			lazyScrollableElement3.RectTransform.localPosition = new Vector2((float)questData2.Definition.TreePos.x * this.elementOffset.x + this.edgeOffset.x, (float)questData2.Definition.TreePos.y * this.elementOffset.y * -1f - this.edgeOffset.y);
			questTreeElementWidgetData2.downConnectorPos = lazyScrollableElement3.RectTransform.localPosition + this.connectorPortOffsetDown;
			questTreeElementWidgetData2.upConnectorPos = lazyScrollableElement3.RectTransform.localPosition + this.connectorPortOffsetUp;
			questTreeElementWidgetData2.leftConnectorPos = lazyScrollableElement3.RectTransform.localPosition + this.connectorPortOffsetLeft;
			questTreeElementWidgetData2.rightConnectorPos = lazyScrollableElement3.RectTransform.localPosition + this.connectorPortOffsetRight;
		}
		((RectTransform)base.transform).RefreshContentFitter();
		for (int j = 0; j < this.scrollRect.DisplayingElements.Count; j++)
		{
			List<QuestDef> childDefinitionList = (this.scrollRect.DisplayingElements[j].Data as QuestTreeElementWidgetData).questData.Definition.childDefinitionList;
			for (int k = 0; k < childDefinitionList.Count; k++)
			{
				QuestDef questDef = childDefinitionList[k];
				QuestData questData3;
				if (questDef != null && MainGame.Instance.GameSave.questSystemData.questCollection.questsCache.TryGetValue(questDef.id, out questData3) && !questData3.isHidden)
				{
					LazyScrollableElement lazyScrollableElement4 = this.FindQuestByDefinition(questDef);
					if (!(lazyScrollableElement4 == null))
					{
						QuestTreeConnector elementFromPool = UIPrefabsPooler.Instance.GetElementFromPool<QuestTreeConnector>(this.scrollRect.content);
						QuestTreeConnector elementFromPool2 = UIPrefabsPooler.Instance.GetElementFromPool<QuestTreeConnector>(this.scrollRect.content);
						this.connectors.Add(elementFromPool);
						this.connectors.Add(elementFromPool2);
						elementFromPool.Draw(this.scrollRect.DisplayingElements[j], lazyScrollableElement4, false);
						elementFromPool2.Draw(this.scrollRect.DisplayingElements[j], lazyScrollableElement4, true);
						QuestTreeConnectorType connectorType = elementFromPool.ConnectorType;
						if (connectorType != QuestTreeConnectorType.Active)
						{
							if (connectorType != QuestTreeConnectorType.Inactive)
							{
								throw new ArgumentOutOfRangeException();
							}
							elementFromPool.transform.SetParent(this.inactiveConnectorsContent.transform);
						}
						else
						{
							elementFromPool.transform.SetParent(this.activeConnectorsContent.transform);
						}
						elementFromPool.transform.SetAsFirstSibling();
						elementFromPool2.transform.SetParent(this.backgroundConnectorsContent.transform);
						elementFromPool2.transform.SetAsFirstSibling();
					}
				}
			}
		}
		this.scrollRect.CheckVisibility();
		((RectTransform)base.transform).RefreshContentFitter();
	}

	// Token: 0x06003EE2 RID: 16098 RVA: 0x0012CB5C File Offset: 0x0012AD5C
	private void ApplyDisplayViewStatuses()
	{
		Dictionary<string, QuestViewStatus> dictionary = new Dictionary<string, QuestViewStatus>();
		QuestCollectionData questCollection = MainGame.Instance.GameSave.questSystemData.questCollection;
		foreach (LazyScrollableElement lazyScrollableElement in this.scrollRect.DisplayingElements)
		{
			QuestTreeElementWidgetData questTreeElementWidgetData = (QuestTreeElementWidgetData)lazyScrollableElement.Data;
			questTreeElementWidgetData.displayViewStatus = QuestTreePageWidget.GetDisplayViewStatus(questTreeElementWidgetData.questData, questCollection, dictionary);
		}
	}

	// Token: 0x06003EE3 RID: 16099 RVA: 0x0012CBE4 File Offset: 0x0012ADE4
	private static QuestViewStatus GetDisplayViewStatus(QuestData questData, QuestCollectionData questCollection, Dictionary<string, QuestViewStatus> cache)
	{
		QuestViewStatus questViewStatus;
		if (cache.TryGetValue(questData.id, out questViewStatus))
		{
			return questViewStatus;
		}
		QuestViewStatus viewStatus = questData.ViewStatus;
		if (viewStatus == QuestViewStatus.Unknown)
		{
			cache[questData.id] = QuestViewStatus.Unknown;
			return QuestViewStatus.Unknown;
		}
		cache[questData.id] = viewStatus;
		List<QuestDef> parentDefinitionList = questData.Definition.parentDefinitionList;
		for (int i = 0; i < parentDefinitionList.Count; i++)
		{
			QuestDef questDef = parentDefinitionList[i];
			QuestData questData2;
			if (questDef != null && questCollection.questsCache.TryGetValue(questDef.id, out questData2) && QuestTreePageWidget.GetDisplayViewStatus(questData2, questCollection, cache) == QuestViewStatus.Unknown)
			{
				cache[questData.id] = QuestViewStatus.Unknown;
				return QuestViewStatus.Unknown;
			}
		}
		return viewStatus;
	}

	// Token: 0x06003EE4 RID: 16100 RVA: 0x0012CC88 File Offset: 0x0012AE88
	private static bool IsAnyBrotherAlreadyAdded(QuestData questData, HashSet<string> alreadyAddedToTreeIds)
	{
		List<string> list;
		if (!GameBalance.Me.questBrothersCache.TryGetValue(questData.id, out list))
		{
			return false;
		}
		QuestCollectionData questCollection = MainGame.Instance.GameSave.questSystemData.questCollection;
		for (int i = 0; i < list.Count; i++)
		{
			string text = list[i];
			if (alreadyAddedToTreeIds.Contains(text))
			{
				if (questData.Definition.brotherIds.Contains(text))
				{
					return true;
				}
				QuestData questData2;
				if (questCollection.questsCache.TryGetValue(text, out questData2) && questData2.status == QuestStatus.InProgress)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003EE5 RID: 16101 RVA: 0x0012CD1C File Offset: 0x0012AF1C
	private static bool HasActiveBrother(QuestData questData)
	{
		List<string> list;
		if (!GameBalance.Me.questBrothersCache.TryGetValue(questData.id, out list))
		{
			return false;
		}
		QuestCollectionData questCollection = MainGame.Instance.GameSave.questSystemData.questCollection;
		for (int i = 0; i < list.Count; i++)
		{
			string text = list[i];
			QuestData questData2;
			if (questCollection.questsCache.TryGetValue(text, out questData2) && questData2.IsActiveQuest)
			{
				if (questData.Definition.brotherIds.Contains(text))
				{
					return true;
				}
				if (questData2.status == QuestStatus.InProgress)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003EE6 RID: 16102 RVA: 0x0012CDB0 File Offset: 0x0012AFB0
	private void HideConnectors()
	{
		foreach (QuestTreeConnector questTreeConnector in this.connectors)
		{
			UIPrefabsPooler.Instance.ReleaseElementToPool<QuestTreeConnector>(questTreeConnector);
		}
		this.connectors.Clear();
	}

	// Token: 0x06003EE7 RID: 16103 RVA: 0x0012CE14 File Offset: 0x0012B014
	private LazyScrollableElement FindQuestByDefinition(QuestDef questDef)
	{
		foreach (LazyScrollableElement lazyScrollableElement in this.scrollRect.DisplayingElements)
		{
			if ((lazyScrollableElement.Data as QuestTreeElementWidgetData).questData.Definition.id == questDef.id)
			{
				return lazyScrollableElement;
			}
		}
		Debug.Log("Cannot find element for definition " + questDef.id);
		return null;
	}

	// Token: 0x06003EE8 RID: 16104 RVA: 0x0012CEA8 File Offset: 0x0012B0A8
	private QuestTreeElementWidget GetQuestWidget(LazyScrollableElement parent)
	{
		QuestTreeElementWidget elementFromPool = UIPrefabsPooler.Instance.GetElementFromPool<QuestTreeElementWidget>(parent.RectTransform);
		elementFromPool.Init();
		elementFromPool.Draw(parent.Data);
		((RectTransform)elementFromPool.transform).sizeDelta = this.elementSize;
		parent.GamepadNavigationItem.SetCallbacks(new UnityAction(elementFromPool.button.ForceOnEnter), new UnityAction(elementFromPool.button.ForceOnExit), new UnityAction(elementFromPool.button.ForceOnClick));
		elementFromPool.transform.position = parent.transform.position;
		return elementFromPool;
	}

	// Token: 0x06003EE9 RID: 16105 RVA: 0x0012CF43 File Offset: 0x0012B143
	private void ReleaseWidgetForParent(LazyScrollableElement parent)
	{
		this.ReleaseCommonWidget(parent.Widget as QuestTreeElementWidget);
	}

	// Token: 0x06003EEA RID: 16106 RVA: 0x0012CF56 File Offset: 0x0012B156
	private void ReleaseCommonWidget(QuestTreeElementWidget questTreeElementWidget)
	{
		questTreeElementWidget.DeInit();
		questTreeElementWidget.Hide();
		UIPrefabsPooler.Instance.ReleaseElementToPool<QuestTreeElementWidget>(questTreeElementWidget);
	}

	// Token: 0x06003EEB RID: 16107 RVA: 0x0012CF70 File Offset: 0x0012B170
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
			QuestDef definition = (lazyScrollableElement.Data as QuestTreeElementWidgetData).questData.Definition;
			if (definition == null)
			{
				Debug.Log(string.Format("data is null?:[{0}]", lazyScrollableElement.Data == null));
			}
			if ((float)definition.TreePos.x > num)
			{
				num = (float)definition.TreePos.x;
			}
			if ((float)definition.TreePos.y > num2)
			{
				num2 = (float)definition.TreePos.y;
			}
		}
		this.scrollRect.content.sizeDelta = new Vector2(this.scrollRect.content.sizeDelta.x, num2 * this.elementOffset.y + this.edgeOffset.y * 2f + this.elementOffset.y / 2f);
	}

	// Token: 0x06003EEC RID: 16108 RVA: 0x0012D0C8 File Offset: 0x0012B2C8
	private void HideCurrentElements()
	{
		this.scrollRect.ClearDisplayingScrollableElements();
	}

	// Token: 0x06003EED RID: 16109 RVA: 0x0012D0D8 File Offset: 0x0012B2D8
	private void OnQuestClicked(QuestTreeElementWidgetData widgetData)
	{
		LazyWindow<UIQuestInfoWindowData> window = LazyUI.GetWindow<UIQuestInfoWindow>();
		UIQuestInfoWindowData uiquestInfoWindowData = new UIQuestInfoWindowData(widgetData.questData);
		window.Open(uiquestInfoWindowData);
	}

	// Token: 0x06003EEE RID: 16110 RVA: 0x0012D0FC File Offset: 0x0012B2FC
	public override List<LazyGameKeyTip> GetTips(GamepadNavigationItem gamepadNavigationItem)
	{
		List<LazyGameKeyTip> list = new List<LazyGameKeyTip>();
		if (gamepadNavigationItem != null)
		{
			list.Add(LazyGameKeyTip.Select(true, true, true));
		}
		return list;
	}

	// Token: 0x06003EEF RID: 16111 RVA: 0x0012D127 File Offset: 0x0012B327
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Draw(new QuestTreePageWidgetData());
	}

	// Token: 0x04003170 RID: 12656
	[Space]
	[SerializeField]
	private LazyScrollRect scrollRect;

	// Token: 0x04003171 RID: 12657
	[SerializeField]
	private GamepadNavigationController gamepadNavigationController;

	// Token: 0x04003172 RID: 12658
	[Space]
	[SerializeField]
	private Vector2 edgeOffset = new Vector2(10f, 10f);

	// Token: 0x04003173 RID: 12659
	[SerializeField]
	private Vector2 elementOffset = new Vector2(210f, 80f);

	// Token: 0x04003174 RID: 12660
	[SerializeField]
	private Vector2 elementSize = new Vector2(32f, 32f);

	// Token: 0x04003175 RID: 12661
	[SerializeField]
	private GameObject backgroundConnectorsContent;

	// Token: 0x04003176 RID: 12662
	[SerializeField]
	private GameObject activeConnectorsContent;

	// Token: 0x04003177 RID: 12663
	[SerializeField]
	private GameObject inactiveConnectorsContent;

	// Token: 0x04003178 RID: 12664
	[SerializeField]
	private Vector2 connectorPortOffsetUp = new Vector2(0f, 16f);

	// Token: 0x04003179 RID: 12665
	[SerializeField]
	private Vector2 connectorPortOffsetDown = new Vector2(0f, -16f);

	// Token: 0x0400317A RID: 12666
	[SerializeField]
	private Vector2 connectorPortOffsetRight = new Vector2(16f, 0f);

	// Token: 0x0400317B RID: 12667
	[SerializeField]
	private Vector2 connectorPortOffsetLeft = new Vector2(-16f, 0f);

	// Token: 0x0400317C RID: 12668
	private List<QuestTreeConnector> connectors = new List<QuestTreeConnector>();
}
