using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020004AC RID: 1196
[Serializable]
public class QuestData : ObjectLinkedToDefinition<QuestDef>
{
	// Token: 0x1700054C RID: 1356
	// (get) Token: 0x06001FEA RID: 8170 RVA: 0x00095371 File Offset: 0x00093571
	private GlobalEventsSystem GlobalEventsSystem
	{
		get
		{
			return MainGame.Instance.GameSave.globalEventsSystem;
		}
	}

	// Token: 0x1700054D RID: 1357
	// (get) Token: 0x06001FEB RID: 8171 RVA: 0x0009723A File Offset: 0x0009543A
	public string Description
	{
		get
		{
			if (this.status != QuestStatus.Completed)
			{
				return LLBase.L("quest_open_" + this.id + "_d");
			}
			return LLBase.L("quest_closed_" + this.id + "_d");
		}
	}

	// Token: 0x1700054E RID: 1358
	// (get) Token: 0x06001FEC RID: 8172 RVA: 0x0009727C File Offset: 0x0009547C
	public QuestViewStatus ViewStatus
	{
		get
		{
			if (this.isHidden)
			{
				return QuestViewStatus.Hidden;
			}
			if (this.isUnknown)
			{
				return QuestViewStatus.Unknown;
			}
			switch (this.status)
			{
			case QuestStatus.Available:
			case QuestStatus.Awaiting:
				return QuestViewStatus.Visible;
			case QuestStatus.InProgress:
				return QuestViewStatus.Revealed;
			case QuestStatus.Completed:
				return QuestViewStatus.Completed;
			default:
				return QuestViewStatus.Hidden;
			}
		}
	}

	// Token: 0x1700054F RID: 1359
	// (get) Token: 0x06001FED RID: 8173 RVA: 0x000972C4 File Offset: 0x000954C4
	public bool IsActiveQuest
	{
		get
		{
			QuestViewStatus viewStatus = this.ViewStatus;
			return viewStatus == QuestViewStatus.Visible || viewStatus == QuestViewStatus.Revealed;
		}
	}

	// Token: 0x06001FEE RID: 8174 RVA: 0x000972E9 File Offset: 0x000954E9
	public QuestData()
	{
	}

	// Token: 0x06001FEF RID: 8175 RVA: 0x000972F1 File Offset: 0x000954F1
	public QuestData(QuestDef questDef)
	{
		this.id = questDef.id;
		this.status = QuestStatus.Available;
		this.isHidden = questDef.isHidden;
		this.isUnknown = questDef.isUnknown;
	}

	// Token: 0x06001FF0 RID: 8176 RVA: 0x00097324 File Offset: 0x00095524
	public void Start()
	{
		QuestStatus questStatus = this.status;
		if (questStatus == QuestStatus.Canceled || questStatus == QuestStatus.Completed)
		{
			return;
		}
		if (base.Definition.hasPosInBalance)
		{
			this.isHidden = false;
		}
		if (base.Definition.startCheck.hasTrigger)
		{
			this.GlobalEventsSystem.RemoveEvent(base.Definition.startCheck);
		}
		if (base.Definition.finishCheck.hasTrigger)
		{
			this.GlobalEventsSystem.AddEvent(base.Definition.finishCheck);
		}
		this.status = QuestStatus.InProgress;
		foreach (LazyExpression lazyExpression in base.Definition.execExpressionsStart)
		{
			lazyExpression.Evaluate();
		}
		Debug.Log("Quest:[" + this.id + "] status = InProgress");
	}

	// Token: 0x06001FF1 RID: 8177 RVA: 0x00097410 File Offset: 0x00095610
	public void FailedStart()
	{
		foreach (LazyExpression lazyExpression in base.Definition.execExpressionsStartFail)
		{
			lazyExpression.Evaluate();
		}
	}

	// Token: 0x06001FF2 RID: 8178 RVA: 0x00097468 File Offset: 0x00095668
	public void Await()
	{
		this.GlobalEventsSystem.AddEvent(base.Definition.startCheck);
		this.status = QuestStatus.Awaiting;
		Debug.Log("Quest:[" + this.id + "] status = Awaiting");
	}

	// Token: 0x06001FF3 RID: 8179 RVA: 0x000974A4 File Offset: 0x000956A4
	public void Complete()
	{
		if (base.Definition.startCheck.hasTrigger)
		{
			this.GlobalEventsSystem.RemoveEvent(base.Definition.startCheck);
		}
		if (base.Definition.finishCheck.hasTrigger)
		{
			this.GlobalEventsSystem.RemoveEvent(base.Definition.finishCheck);
		}
		this.status = QuestStatus.Completed;
		foreach (LazyExpression lazyExpression in base.Definition.execExpressionsFinish)
		{
			lazyExpression.Evaluate();
		}
		Debug.Log("Quest:[" + this.id + "] status = Completed");
	}

	// Token: 0x06001FF4 RID: 8180 RVA: 0x0009756C File Offset: 0x0009576C
	public void Cancel()
	{
		QuestStatus questStatus = this.status;
		if (questStatus == QuestStatus.Completed || questStatus == QuestStatus.Canceled)
		{
			return;
		}
		if (base.Definition.startCheck.hasTrigger)
		{
			this.GlobalEventsSystem.RemoveEvent(base.Definition.startCheck);
		}
		if (base.Definition.finishCheck.hasTrigger)
		{
			this.GlobalEventsSystem.RemoveEvent(base.Definition.finishCheck);
		}
		if (!string.IsNullOrEmpty(base.Definition.finishCheck.phrase))
		{
			MainGame.Instance.GameSave.knowledgeSystem.AddPhraseToBlackList(base.Definition.finishCheck.phrase);
		}
		this.status = QuestStatus.Canceled;
		Debug.Log("Quest:[" + this.id + "] status = Canceled");
	}

	// Token: 0x04001C9D RID: 7325
	public bool isHidden;

	// Token: 0x04001C9E RID: 7326
	public bool isUnknown;

	// Token: 0x04001C9F RID: 7327
	public QuestStatus status;
}
