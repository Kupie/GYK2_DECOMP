using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004B1 RID: 1201
[Serializable]
public class QuestSystemData
{
	// Token: 0x14000058 RID: 88
	// (add) Token: 0x06001FFA RID: 8186 RVA: 0x00097848 File Offset: 0x00095A48
	// (remove) Token: 0x06001FFB RID: 8187 RVA: 0x00097880 File Offset: 0x00095A80
	public event Action<QuestData> OnQuestStarted;

	// Token: 0x14000059 RID: 89
	// (add) Token: 0x06001FFC RID: 8188 RVA: 0x000978B8 File Offset: 0x00095AB8
	// (remove) Token: 0x06001FFD RID: 8189 RVA: 0x000978F0 File Offset: 0x00095AF0
	public event Action<QuestData> OnQuestCompleted;

	// Token: 0x1400005A RID: 90
	// (add) Token: 0x06001FFE RID: 8190 RVA: 0x00097928 File Offset: 0x00095B28
	// (remove) Token: 0x06001FFF RID: 8191 RVA: 0x00097960 File Offset: 0x00095B60
	public event Action<QuestData> OnQuestCanceled;

	// Token: 0x06002000 RID: 8192 RVA: 0x00097995 File Offset: 0x00095B95
	public QuestSystemData()
	{
	}

	// Token: 0x06002001 RID: 8193 RVA: 0x000979B4 File Offset: 0x00095BB4
	public QuestSystemData(List<QuestDef> questDefs)
	{
		foreach (QuestDef questDef in questDefs)
		{
			this.questCollection.quests.Add(new QuestData(questDef));
		}
	}

	// Token: 0x06002002 RID: 8194 RVA: 0x00097A30 File Offset: 0x00095C30
	public void PrepareForGame()
	{
		this.questCollection.PrepareForGame();
		this.RebuildGlobalEventsFromQuestStatuses();
	}

	// Token: 0x06002003 RID: 8195 RVA: 0x00097A44 File Offset: 0x00095C44
	public void AddQuestData(string questId)
	{
		QuestDef data = GameBalance.Me.GetData<QuestDef>(questId);
		if (data != null)
		{
			this.questCollection.AddQuestData(new QuestData(data));
		}
	}

	// Token: 0x06002004 RID: 8196 RVA: 0x00097A74 File Offset: 0x00095C74
	private void RebuildGlobalEventsFromQuestStatuses()
	{
		GlobalEventsSystem globalEventsSystem = MainGame.Instance.GameSave.globalEventsSystem;
		for (int i = 0; i < this.questCollection.quests.Count; i++)
		{
			QuestData questData = this.questCollection.quests[i];
			if (questData != null && questData.Definition != null)
			{
				QuestStatus status = questData.status;
				if (status != QuestStatus.Awaiting)
				{
					if (status == QuestStatus.InProgress)
					{
						if (questData.Definition.finishCheck.hasTrigger)
						{
							globalEventsSystem.AddEvent(questData.Definition.finishCheck);
						}
					}
				}
				else
				{
					globalEventsSystem.AddEvent(questData.Definition.startCheck);
				}
			}
		}
	}

	// Token: 0x06002005 RID: 8197 RVA: 0x00097B10 File Offset: 0x00095D10
	public void StartQuest(string id, float delayTime = 0f)
	{
		QuestData questData;
		if (!this.questCollection.questsCache.TryGetValue(id, out questData))
		{
			Debug.LogError("Quest wasn't found by id: " + id);
			return;
		}
		QuestStatus status = questData.status;
		if (status == QuestStatus.InProgress || status == QuestStatus.Completed || status == QuestStatus.Canceled)
		{
			Debug.LogError("Quest with id " + id + " can't be started because quest status is already: " + questData.status.ToString());
			return;
		}
		if (this.IsQuestDelayed(questData, delayTime, QuestStatus.InProgress))
		{
			return;
		}
		for (int i = 0; i < this.questCollection.quests.Count; i++)
		{
			QuestData questData2 = this.questCollection.quests[i];
			if (questData2 == null)
			{
				Debug.LogError(string.Format("Quest is null at index {0}", i));
			}
			else if (questData2.Definition == null)
			{
				Debug.LogError("Quest definition is null for quest: " + questData2.id);
			}
			else if (questData2 != questData && questData2.Definition.TreePos == questData.Definition.TreePos && questData2.status == QuestStatus.Completed)
			{
				questData2.isHidden = true;
			}
		}
		questData.Start();
		Action<QuestData> onQuestStarted = this.OnQuestStarted;
		if (onQuestStarted == null)
		{
			return;
		}
		onQuestStarted(questData);
	}

	// Token: 0x06002006 RID: 8198 RVA: 0x00097C40 File Offset: 0x00095E40
	public void OnStartFailed(string id)
	{
		QuestData questData;
		if (!this.questCollection.questsCache.TryGetValue(id, out questData))
		{
			Debug.LogError("Quest wasn't found by id: " + id);
			return;
		}
		questData.FailedStart();
	}

	// Token: 0x06002007 RID: 8199 RVA: 0x00097C7C File Offset: 0x00095E7C
	public void AwaitQuest(string id, float delayTime = 0f)
	{
		QuestData questData;
		if (!this.questCollection.questsCache.TryGetValue(id, out questData))
		{
			Debug.LogError("Quest wasn't found by id: " + id);
			return;
		}
		QuestStatus status = questData.status;
		if (status == QuestStatus.Awaiting || status == QuestStatus.Completed || status == QuestStatus.Canceled)
		{
			return;
		}
		if (this.IsQuestDelayed(questData, delayTime, QuestStatus.Awaiting))
		{
			return;
		}
		questData.Await();
	}

	// Token: 0x06002008 RID: 8200 RVA: 0x00097CD8 File Offset: 0x00095ED8
	public void CompleteQuest(string id, float delayTime = 0f)
	{
		QuestData questData;
		if (!this.questCollection.questsCache.TryGetValue(id, out questData))
		{
			Debug.LogError("Quest wasn't found by id: " + id);
			return;
		}
		QuestStatus status = questData.status;
		if (status == QuestStatus.Completed || status == QuestStatus.Canceled)
		{
			return;
		}
		if (this.IsQuestDelayed(questData, delayTime, QuestStatus.Completed))
		{
			return;
		}
		questData.Complete();
		Action<QuestData> onQuestCompleted = this.OnQuestCompleted;
		if (onQuestCompleted == null)
		{
			return;
		}
		onQuestCompleted(questData);
	}

	// Token: 0x06002009 RID: 8201 RVA: 0x00097D40 File Offset: 0x00095F40
	public void CancelQuest(string id)
	{
		QuestData questData;
		if (!this.questCollection.questsCache.TryGetValue(id, out questData))
		{
			Debug.LogError("Quest wasn't found by id: " + id);
			return;
		}
		this.delayedQuests.Remove(id);
		QuestStatus status = questData.status;
		if (status == QuestStatus.Completed || status == QuestStatus.Canceled)
		{
			return;
		}
		questData.Cancel();
		Action<QuestData> onQuestCanceled = this.OnQuestCanceled;
		if (onQuestCanceled == null)
		{
			return;
		}
		onQuestCanceled(questData);
	}

	// Token: 0x0600200A RID: 8202 RVA: 0x00097DA8 File Offset: 0x00095FA8
	public bool IsQuestInStatus(string id, QuestStatus status)
	{
		QuestData questData;
		return this.questCollection.questsCache.TryGetValue(id, out questData) && questData.status == status;
	}

	// Token: 0x0600200B RID: 8203 RVA: 0x00097DD8 File Offset: 0x00095FD8
	public void ChangeQuestHiddenState(string id, bool state)
	{
		QuestData questData;
		if (!this.questCollection.questsCache.TryGetValue(id, out questData))
		{
			Debug.LogError("Quest wasn't found by id: " + id);
			return;
		}
		questData.isHidden = state;
	}

	// Token: 0x0600200C RID: 8204 RVA: 0x00097E14 File Offset: 0x00096014
	public void ChangeQuestUnknownState(string id, bool state)
	{
		QuestData questData;
		if (!this.questCollection.questsCache.TryGetValue(id, out questData))
		{
			Debug.LogError("Quest wasn't found by id: " + id);
			return;
		}
		questData.isUnknown = state;
	}

	// Token: 0x0600200D RID: 8205 RVA: 0x00097E50 File Offset: 0x00096050
	public bool RaiseCustomQuestTrigger(string id, Action callback)
	{
		LazyExpression lazyExpression;
		if (!this.questCollection.customTriggersCache.TryGetValue(id, out lazyExpression))
		{
			Debug.LogError("CustomTrigger wasn't found by id: " + id);
			return false;
		}
		return lazyExpression.EvaluateWithCallback(callback);
	}

	// Token: 0x0600200E RID: 8206 RVA: 0x00097E8C File Offset: 0x0009608C
	public bool WgoHasReadyToFinishQuest(string wgoId)
	{
		PhrasesByWgo phrasesByWgo;
		if (!FinishPhrasesByWgoParser.TryGetFinishPhrases(wgoId, out phrasesByWgo))
		{
			return false;
		}
		foreach (string text in phrasesByWgo.phrases)
		{
			QuestDef questDef;
			QuestData questData;
			if (GameBalance.Me.questDefByFinishPhrase.TryGetValue(text, out questDef) && MainGame.Instance.GameSave.questSystemData.questCollection.questsCache.TryGetValue(questDef.id, out questData) && questData.status == QuestStatus.InProgress && questDef.finishCheck.IsReadyToFinish())
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600200F RID: 8207 RVA: 0x00097F40 File Offset: 0x00096140
	private bool IsQuestDelayed(QuestData questData, float delayTime, QuestStatus targetStatus)
	{
		if (delayTime.EqualsTo(0f, 0.01f))
		{
			this.delayedQuests.Remove(questData.id);
			return false;
		}
		QuestSystemData.DelayedQuest delayedQuest;
		if (this.delayedQuests.TryGetValue(questData.id, out delayedQuest))
		{
			delayedQuest.delayTime = delayTime;
			return true;
		}
		this.delayedQuests.Add(questData.id, new QuestSystemData.DelayedQuest(questData.id, delayTime, targetStatus));
		return true;
	}

	// Token: 0x04001CB1 RID: 7345
	public QuestCollectionData questCollection = new QuestCollectionData();

	// Token: 0x04001CB2 RID: 7346
	public Dictionary<string, QuestSystemData.DelayedQuest> delayedQuests = new Dictionary<string, QuestSystemData.DelayedQuest>();

	// Token: 0x020004B2 RID: 1202
	[Serializable]
	public class DelayedQuest
	{
		// Token: 0x06002010 RID: 8208 RVA: 0x00021B94 File Offset: 0x0001FD94
		public DelayedQuest()
		{
		}

		// Token: 0x06002011 RID: 8209 RVA: 0x00097FB0 File Offset: 0x000961B0
		public DelayedQuest(string id, float delayTime, QuestStatus status)
		{
			this.id = id;
			this.delayTime = delayTime;
			this.status = status;
		}

		// Token: 0x04001CB3 RID: 7347
		public string id;

		// Token: 0x04001CB4 RID: 7348
		public float delayTime;

		// Token: 0x04001CB5 RID: 7349
		public QuestStatus status;
	}
}
