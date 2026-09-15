using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004AB RID: 1195
[Serializable]
public class QuestCollectionData
{
	// Token: 0x06001FE5 RID: 8165 RVA: 0x00096FA4 File Offset: 0x000951A4
	public void PrepareForGame()
	{
		this.questsCache = new Dictionary<string, QuestData>();
		this.questStatusFilteredQuests = new Dictionary<QuestStatus, List<QuestData>>();
		this.customTriggersCache = new Dictionary<string, LazyExpression>();
		for (int i = 0; i < this.quests.Count; i++)
		{
			QuestData questData = this.quests[i];
			this.questsCache.Add(questData.id, questData);
			if (questData.Definition == null)
			{
				Debug.LogError("Quest definition not found for quest: " + questData.id);
			}
			else
			{
				this.AddCustomTriggersToCache(questData);
			}
		}
		foreach (object obj in Enum.GetValues(typeof(QuestStatus)))
		{
			QuestStatus questStatus = (QuestStatus)obj;
			this.questStatusFilteredQuests.Add(questStatus, new List<QuestData>());
			for (int j = 0; j < this.quests.Count; j++)
			{
				if (this.quests[j].status == questStatus)
				{
					this.questStatusFilteredQuests[questStatus].Add(this.quests[j]);
				}
			}
		}
		this.AddNewQuestsFromBalance();
	}

	// Token: 0x06001FE6 RID: 8166 RVA: 0x000970E4 File Offset: 0x000952E4
	public void AddQuestData(QuestData questData)
	{
		if (this.questsCache.ContainsKey(questData.id))
		{
			Debug.LogError("Quest collection already have same quest: " + questData.id);
			return;
		}
		if (questData.Definition == null)
		{
			Debug.LogError("Quest definition not found for quest: " + questData.id);
			return;
		}
		this.quests.Add(questData);
		this.questsCache.Add(questData.id, questData);
		this.questStatusFilteredQuests[questData.status].Add(questData);
		this.AddCustomTriggersToCache(questData);
	}

	// Token: 0x06001FE7 RID: 8167 RVA: 0x00097174 File Offset: 0x00095374
	private void AddNewQuestsFromBalance()
	{
		List<QuestDef> questDefs = GameBalance.Me.questDefs;
		for (int i = 0; i < questDefs.Count; i++)
		{
			QuestDef questDef = questDefs[i];
			if (questDef != null && !this.questsCache.ContainsKey(questDef.id))
			{
				this.AddQuestData(new QuestData(questDef));
			}
		}
	}

	// Token: 0x06001FE8 RID: 8168 RVA: 0x000971C8 File Offset: 0x000953C8
	private void AddCustomTriggersToCache(QuestData questData)
	{
		for (int i = 0; i < questData.Definition.customTriggers.Count; i++)
		{
			this.customTriggersCache.Add(questData.Definition.customTriggers[i].name, questData.Definition.customTriggers[i].expression);
		}
	}

	// Token: 0x04001C99 RID: 7321
	public List<QuestData> quests = new List<QuestData>();

	// Token: 0x04001C9A RID: 7322
	[NonSerialized]
	public Dictionary<QuestStatus, List<QuestData>> questStatusFilteredQuests;

	// Token: 0x04001C9B RID: 7323
	[NonSerialized]
	public Dictionary<string, QuestData> questsCache;

	// Token: 0x04001C9C RID: 7324
	[NonSerialized]
	public Dictionary<string, LazyExpression> customTriggersCache;
}
