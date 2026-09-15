using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A21 RID: 2593
public class UIQuestInfoWindow : LazyWindow<UIQuestInfoWindowData>
{
	// Token: 0x060045A8 RID: 17832 RVA: 0x001494BF File Offset: 0x001476BF
	public override void Init()
	{
		base.Init();
		UIQuestInfoWindow.linkedPool = new Pool(this.linkedEntityWidgetPrefab, base.transform, 1, Pool.PoolType.ImmediateActivation, false, null);
	}

	// Token: 0x060045A9 RID: 17833 RVA: 0x001494E4 File Offset: 0x001476E4
	public override void Redraw()
	{
		base.Redraw();
		this.btnData = new UIDialogWindowData.ButtonData(new Action(this.Close), LLBase.L("btn_ok"), null, true, GameKey.Select, "");
		this.okBtn.Draw(this.btnData);
		for (int i = 1; i < this.questInfoElements.Length; i++)
		{
			this.questInfoElements[i].gameObject.SetActive(false);
		}
		QuestData questData = this.data.QuestData;
		QuestTreeElementWidgetData questTreeElementWidgetData = new QuestTreeElementWidgetData();
		questTreeElementWidgetData.questData = questData;
		questTreeElementWidgetData.hideQuestionMark = true;
		questTreeElementWidgetData.displayViewStatus = questData.ViewStatus;
		this.currentWidget.Draw(questTreeElementWidgetData);
		this.header.text = LLBase.L(questData.id);
		this.questInfoElements[0].Draw(questData, this.ConstructLinkedForQuestData(questData));
		for (int j = 1; j < this.decors.Length - 1; j++)
		{
			this.decors[j].gameObject.SetActive(false);
			this.decors[j].DisableAll();
		}
		this.decors[0].DisableAll();
		QuestInfoDecor[] array = this.decors;
		array[array.Length - 1].DisableAll();
		if (this.shownLinked.Count > 0)
		{
			this.decors[0].centerDown.SetActive(true);
		}
		else
		{
			this.decors[0].noCenter.SetActive(true);
		}
		List<QuestInfoElement> list = new List<QuestInfoElement>();
		List<string> list2 = new List<string>();
		list.Add(this.questInfoElements[0]);
		list2.Add(questData.id);
		bool flag = false;
		int num = 0;
		while (num < questData.Definition.brotherIds.Count && list.Count != this.questInfoElements.Length)
		{
			QuestData questData2 = MainGame.Instance.GameSave.questSystemData.questCollection.questsCache[questData.Definition.brotherIds[num]];
			if (!questData2.isHidden)
			{
				QuestViewStatus viewStatus = questData2.ViewStatus;
				if (viewStatus != QuestViewStatus.Hidden && viewStatus != QuestViewStatus.Unknown && viewStatus != QuestViewStatus.Completed && !list2.Contains(questData2.id))
				{
					if (questData.ViewStatus == QuestViewStatus.Completed && !flag)
					{
						flag = true;
						list.RemoveAt(0);
						list2.RemoveAt(0);
					}
					this.questInfoElements[list.Count].Draw(questData2, this.ConstructLinkedForQuestData(questData2));
					list.Add(this.questInfoElements[list.Count]);
					list2.Add(questData2.id);
				}
			}
			num++;
		}
		int num2 = 0;
		while (num2 < MainGame.Instance.GameSave.questSystemData.questCollection.quests.Count && list.Count != this.questInfoElements.Length)
		{
			QuestData questData3 = MainGame.Instance.GameSave.questSystemData.questCollection.quests[num2];
			if (questData3 != questData && questData3.Definition.TreePos == questData.Definition.TreePos && questData3.status == QuestStatus.InProgress && !list2.Contains(questData3.id))
			{
				this.questInfoElements[list.Count].Draw(questData3, this.ConstructLinkedForQuestData(questData3));
				list.Add(this.questInfoElements[list.Count]);
				list2.Add(questData3.id);
			}
			num2++;
		}
		if (list.Count <= 1)
		{
			if (this.shownLinked.Count > 0)
			{
				QuestInfoDecor[] array2 = this.decors;
				array2[array2.Length - 1].centerUp.SetActive(true);
			}
			else
			{
				QuestInfoDecor[] array3 = this.decors;
				array3[array3.Length - 1].noCenter.SetActive(true);
			}
		}
		else
		{
			QuestInfoElement questInfoElement = list[0];
			for (int k = 1; k < list.Count; k++)
			{
				QuestInfoElement questInfoElement2 = list[k];
				this.decors[k].gameObject.SetActive(true);
				if (questInfoElement.anyLinked)
				{
					if (questInfoElement2.anyLinked)
					{
						this.decors[k].centerUpAndDown.SetActive(true);
					}
					else
					{
						this.decors[k].centerUp.SetActive(true);
					}
				}
				else if (questInfoElement2.anyLinked)
				{
					this.decors[k].centerDown.SetActive(true);
				}
				else
				{
					this.decors[k].noCenter.SetActive(true);
				}
				questInfoElement = questInfoElement2;
			}
			if (questInfoElement.anyLinked)
			{
				QuestInfoDecor[] array4 = this.decors;
				array4[array4.Length - 1].centerUp.SetActive(true);
			}
			else
			{
				QuestInfoDecor[] array5 = this.decors;
				array5[array5.Length - 1].noCenter.SetActive(true);
			}
		}
		string text = string.Empty;
		for (int l = 0; l < this.data.QuestData.Definition.repVisualisationRes.List.Count; l++)
		{
			if (l > 0)
			{
				text += "\n";
			}
			text += this.data.QuestData.Definition.repVisualisationRes.List[l].ToFormattedString(false, (string s, string s1) => s + "+" + s1, false, true, null);
		}
		if (string.IsNullOrEmpty(text))
		{
			this.repLabel.gameObject.SetActive(false);
		}
		else
		{
			this.repLabel.gameObject.SetActive(true);
			string text2 = LLBase.L("ui_reward") + ":";
			text = this.rewardTextStyle.ApplyStyleToString(text2, false, true) + "\n" + text;
		}
		this.repLabel.text = text;
		this.currentWidget.button.interactable = false;
		((RectTransform)base.transform).RefreshContentFitter();
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
		}
	}

	// Token: 0x060045AA RID: 17834 RVA: 0x00149AD4 File Offset: 0x00147CD4
	public override void Hide()
	{
		this.currentWidget.Hide();
		foreach (LinkedEntityWidget linkedEntityWidget in this.shownLinked)
		{
			UIQuestInfoWindow.linkedPool.ReleaseObject<LinkedEntityWidget>(linkedEntityWidget);
		}
		this.shownLinked.Clear();
		base.Hide();
	}

	// Token: 0x060045AB RID: 17835 RVA: 0x00149B48 File Offset: 0x00147D48
	protected override void PrintTips()
	{
		this.lazyButtonTips.Clear();
	}

	// Token: 0x060045AC RID: 17836 RVA: 0x00149B58 File Offset: 0x00147D58
	private List<LinkedEntityWidget> ConstructLinkedForQuestData(QuestData questData)
	{
		List<LinkedEntityWidget> list = new List<LinkedEntityWidget>();
		if (questData.ViewStatus == QuestViewStatus.Completed)
		{
			return list;
		}
		for (int i = 0; i < questData.Definition.finishCheck.phraseReqs.Count; i++)
		{
			LinkedEntityWidget orCreateObject = UIQuestInfoWindow.linkedPool.GetOrCreateObject<LinkedEntityWidget>();
			QuestPhraseRequirement questPhraseRequirement = questData.Definition.finishCheck.phraseReqs[i];
			LinkedEntityWidgetData linkedEntityWidgetData;
			switch (questPhraseRequirement.entity)
			{
			case QuestPhraseRequirement.Entity.Item:
				linkedEntityWidgetData = new LinkedEntityWidgetData(new Item(questPhraseRequirement.itemCount.itemId, questPhraseRequirement.itemCount.count), null);
				if (MainGame.PlayerData.Inventory.Data.GetTotalCountInInventory(questPhraseRequirement.itemCount.itemId, null, false) >= questPhraseRequirement.itemCount.count)
				{
					linkedEntityWidgetData.LabelCustomStyle = this.enoughStyle;
				}
				else
				{
					linkedEntityWidgetData.LabelCustomStyle = this.notEnoughStyle;
				}
				break;
			case QuestPhraseRequirement.Entity.GameResAtom:
				linkedEntityWidgetData = new LinkedEntityWidgetData(questPhraseRequirement.gameResAtom.type, (int)questPhraseRequirement.gameResAtom.value, null);
				if (MainGame.PlayerData.GetRes(questPhraseRequirement.gameResAtom.type, 0f) >= questPhraseRequirement.gameResAtom.value)
				{
					linkedEntityWidgetData.LabelCustomStyle = this.enoughStyle;
				}
				else
				{
					linkedEntityWidgetData.LabelCustomStyle = this.notEnoughStyle;
				}
				break;
			case QuestPhraseRequirement.Entity.Day:
				linkedEntityWidgetData = new LinkedEntityWidgetData(questPhraseRequirement.dayNumber, null);
				if (MainGame.Instance.GameSave.environmentData.CurrentDayNumber == ConstDef.Get(questPhraseRequirement.dayNumber).IntValue)
				{
					linkedEntityWidgetData.LabelCustomStyle = this.enoughStyle;
				}
				else
				{
					linkedEntityWidgetData.LabelCustomStyle = this.notEnoughStyle;
				}
				break;
			case QuestPhraseRequirement.Entity.Order:
				linkedEntityWidgetData = new LinkedEntityWidgetData(GameBalance.Me.GetData<VendorOrderDef>(questPhraseRequirement.order), null);
				if (MainGame.Instance.GameSave.vendorSystem.IsOrderFinished(questPhraseRequirement.order))
				{
					linkedEntityWidgetData.LabelCustomStyle = this.enoughStyle;
				}
				else
				{
					linkedEntityWidgetData.LabelCustomStyle = this.notEnoughStyle;
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			orCreateObject.Draw(linkedEntityWidgetData);
			list.Add(orCreateObject);
			this.shownLinked.Add(orCreateObject);
		}
		return list;
	}

	// Token: 0x060045AD RID: 17837 RVA: 0x00149D88 File Offset: 0x00147F88
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.Select, () => false);
		return gameKeyDelegates;
	}

	// Token: 0x060045AE RID: 17838 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x060045AF RID: 17839 RVA: 0x00149DBC File Offset: 0x00147FBC
	[LazyUITest]
	protected void TestDrawUnknown()
	{
		this.Open(new UIQuestInfoWindowData(new QuestData(GameBalance.Me.GetData<QuestDef>("6_intro_guards_burial"))
		{
			isHidden = false,
			isUnknown = true
		}));
	}

	// Token: 0x060045B0 RID: 17840 RVA: 0x00149DF8 File Offset: 0x00147FF8
	[LazyUITest]
	protected void TestDrawVisible()
	{
		this.Open(new UIQuestInfoWindowData(new QuestData(GameBalance.Me.GetData<QuestDef>("6_intro_guards_burial"))
		{
			isHidden = false,
			isUnknown = false,
			status = QuestStatus.Available
		}));
	}

	// Token: 0x060045B1 RID: 17841 RVA: 0x00149E3C File Offset: 0x0014803C
	[LazyUITest]
	protected void TestDrawRevealed()
	{
		this.Open(new UIQuestInfoWindowData(new QuestData(GameBalance.Me.GetData<QuestDef>("6_intro_guards_burial"))
		{
			isHidden = false,
			isUnknown = false,
			status = QuestStatus.InProgress
		}));
	}

	// Token: 0x060045B2 RID: 17842 RVA: 0x00149E80 File Offset: 0x00148080
	[LazyUITest]
	protected void TestDrawCompleted()
	{
		this.Open(new UIQuestInfoWindowData(new QuestData(GameBalance.Me.GetData<QuestDef>("6_intro_guards_burial"))
		{
			isHidden = false,
			isUnknown = false,
			status = QuestStatus.Completed
		}));
	}

	// Token: 0x04003680 RID: 13952
	[SerializeField]
	private TextMeshProUGUI header;

	// Token: 0x04003681 RID: 13953
	[SerializeField]
	private TextMeshProUGUI repLabel;

	// Token: 0x04003682 RID: 13954
	[SerializeField]
	private QuestTreeElementWidget currentWidget;

	// Token: 0x04003683 RID: 13955
	[SerializeField]
	private LinkedEntityWidget linkedEntityWidgetPrefab;

	// Token: 0x04003684 RID: 13956
	[SerializeField]
	private UIDialogWindowButton okBtn;

	// Token: 0x04003685 RID: 13957
	[SerializeField]
	private TextStyle enoughStyle;

	// Token: 0x04003686 RID: 13958
	[SerializeField]
	private TextStyle notEnoughStyle;

	// Token: 0x04003687 RID: 13959
	[SerializeField]
	private TextStyle rewardTextStyle;

	// Token: 0x04003688 RID: 13960
	[SerializeField]
	private QuestInfoElement[] questInfoElements;

	// Token: 0x04003689 RID: 13961
	[SerializeField]
	private QuestInfoDecor[] decors;

	// Token: 0x0400368A RID: 13962
	private List<LinkedEntityWidget> shownLinked = new List<LinkedEntityWidget>();

	// Token: 0x0400368B RID: 13963
	private static Pool linkedPool;

	// Token: 0x0400368C RID: 13964
	private UIDialogWindowData.ButtonData btnData;
}
