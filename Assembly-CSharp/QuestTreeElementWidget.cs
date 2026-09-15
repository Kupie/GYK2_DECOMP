using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200094E RID: 2382
public class QuestTreeElementWidget : LazyWidget<QuestTreeElementWidgetData>
{
	// Token: 0x1700097C RID: 2428
	// (get) Token: 0x06003ED3 RID: 16083 RVA: 0x0012C209 File Offset: 0x0012A409
	public QuestTreeElementWidgetData Data
	{
		get
		{
			return this.data;
		}
	}

	// Token: 0x06003ED4 RID: 16084 RVA: 0x0012C211 File Offset: 0x0012A411
	public override void Init()
	{
		base.Init();
		this.OnDeselect();
	}

	// Token: 0x06003ED5 RID: 16085 RVA: 0x0012C220 File Offset: 0x0012A420
	public override void Redraw()
	{
		this.button.onExit.RemoveAllListeners();
		this.button.onExit.AddListener(new UnityAction(this.OnDeselect));
		this.button.onEnter.RemoveAllListeners();
		this.button.onEnter.AddListener(new UnityAction(this.OnSelect));
		this.button.onClick.RemoveAllListeners();
		this.button.onClick.AddListener(new UnityAction(this.OnClicked));
		base.Redraw();
		this.button.interactable = false;
		this.unknownObj.SetActive(false);
		this.visibleObj.SetActive(false);
		this.revealedObj.SetActive(false);
		this.completedObj.SetActive(false);
		base.name = string.Format("{0}({1})", this.data.questData.Definition.id, this.data.displayViewStatus);
		bool flag = false;
		foreach (GameObject gameObject in this.iconsCustom)
		{
			if (this.data.questData.Definition.iconId == gameObject.name)
			{
				flag = true;
				gameObject.gameObject.SetActive(true);
			}
			else
			{
				gameObject.gameObject.SetActive(false);
			}
		}
		if (flag)
		{
			this.iconDefault.gameObject.SetActive(false);
		}
		else
		{
			this.iconDefault.gameObject.SetActive(true);
			this.iconDefault.sprite = this.data.questData.Definition.Icon;
		}
		switch (this.data.displayViewStatus)
		{
		case QuestViewStatus.Unknown:
		{
			this.unknownObj.SetActive(true);
			GameObject[] array = this.iconsCustom;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].gameObject.SetActive(false);
			}
			this.iconDefault.gameObject.SetActive(false);
			break;
		}
		case QuestViewStatus.Visible:
			this.visibleObj.SetActive(true);
			this.button.interactable = true;
			break;
		case QuestViewStatus.Revealed:
		{
			this.revealedObj.SetActive(true);
			this.button.interactable = true;
			int num = Math.Clamp(this.data.questData.Definition.phase - 1, 0, 4);
			this.revealedImage.sprite = this.phaseSprites[num];
			break;
		}
		case QuestViewStatus.Completed:
			this.button.interactable = true;
			this.completedObj.SetActive(true);
			break;
		}
		if (this.questionMark != null)
		{
			this.questionMark.SetActive(this.ShouldShowQuestionMark());
		}
	}

	// Token: 0x06003ED6 RID: 16086 RVA: 0x0012C4D8 File Offset: 0x0012A6D8
	private bool ShouldShowQuestionMark()
	{
		if (this.data.hideQuestionMark)
		{
			return false;
		}
		QuestViewStatus displayViewStatus = this.data.displayViewStatus;
		if (displayViewStatus != QuestViewStatus.Visible && displayViewStatus != QuestViewStatus.Revealed)
		{
			return false;
		}
		QuestDef definition = this.data.questData.Definition;
		if (string.IsNullOrEmpty(definition.wgoNpcId))
		{
			return false;
		}
		QuestFinishCheck finishCheck = definition.finishCheck;
		return finishCheck != null && !string.IsNullOrEmpty(finishCheck.phrase);
	}

	// Token: 0x06003ED7 RID: 16087 RVA: 0x0012C544 File Offset: 0x0012A744
	private void OnClicked()
	{
		Action<QuestTreeElementWidgetData> onQuestClicked = this.data.onQuestClicked;
		if (onQuestClicked != null)
		{
			onQuestClicked(this.data);
		}
		this.OnDeselect();
		this.Redraw();
	}

	// Token: 0x06003ED8 RID: 16088 RVA: 0x0012C56E File Offset: 0x0012A76E
	private void OnDisable()
	{
		this.OnDeselect();
	}

	// Token: 0x06003ED9 RID: 16089 RVA: 0x0012C576 File Offset: 0x0012A776
	private void OnSelect()
	{
		this.selection.gameObject.SetActive(true);
		this.glow.gameObject.SetActive(true);
	}

	// Token: 0x06003EDA RID: 16090 RVA: 0x0012C59A File Offset: 0x0012A79A
	private void OnDeselect()
	{
		this.selection.gameObject.SetActive(false);
		this.glow.gameObject.SetActive(false);
	}

	// Token: 0x06003EDB RID: 16091 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04003161 RID: 12641
	[SerializeField]
	private Image selection;

	// Token: 0x04003162 RID: 12642
	[SerializeField]
	private Image glow;

	// Token: 0x04003163 RID: 12643
	[SerializeField]
	private Image iconDefault;

	// Token: 0x04003164 RID: 12644
	[SerializeField]
	private GameObject[] iconsCustom;

	// Token: 0x04003165 RID: 12645
	[SerializeField]
	private GameObject unknownObj;

	// Token: 0x04003166 RID: 12646
	[SerializeField]
	private GameObject visibleObj;

	// Token: 0x04003167 RID: 12647
	[SerializeField]
	private GameObject revealedObj;

	// Token: 0x04003168 RID: 12648
	[SerializeField]
	private Image revealedImage;

	// Token: 0x04003169 RID: 12649
	[SerializeField]
	private Sprite[] phaseSprites = new Sprite[5];

	// Token: 0x0400316A RID: 12650
	[SerializeField]
	private GameObject completedObj;

	// Token: 0x0400316B RID: 12651
	[SerializeField]
	private GameObject questionMark;

	// Token: 0x0400316C RID: 12652
	public LazyButton button;
}
