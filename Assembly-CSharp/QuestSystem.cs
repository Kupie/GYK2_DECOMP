using System;
using System.Collections.Generic;

// Token: 0x020004B0 RID: 1200
public class QuestSystem : ICustomUpdatable
{
	// Token: 0x17000550 RID: 1360
	// (get) Token: 0x06001FF7 RID: 8183 RVA: 0x000976F2 File Offset: 0x000958F2
	private static QuestSystemData QuestSystemData
	{
		get
		{
			return MainGame.Instance.GameSave.questSystemData;
		}
	}

	// Token: 0x06001FF8 RID: 8184 RVA: 0x00097704 File Offset: 0x00095904
	public void CustomUpdate(float deltaTime)
	{
		float num = EnvironmentEngine.Instance.ConvertDeltaTimeToGameplayTime01(deltaTime);
		List<QuestSystemData.DelayedQuest> list = new List<QuestSystemData.DelayedQuest>();
		foreach (QuestSystemData.DelayedQuest delayedQuest in QuestSystem.QuestSystemData.delayedQuests.Values)
		{
			delayedQuest.delayTime -= num;
			if (delayedQuest.delayTime <= 0f)
			{
				list.Add(delayedQuest);
			}
		}
		foreach (QuestSystemData.DelayedQuest delayedQuest2 in list)
		{
			QuestSystem.QuestSystemData.delayedQuests.Remove(delayedQuest2.id);
			switch (delayedQuest2.status)
			{
			case QuestStatus.Awaiting:
				QuestSystem.QuestSystemData.AwaitQuest(delayedQuest2.id, 0f);
				break;
			case QuestStatus.InProgress:
				QuestSystem.QuestSystemData.StartQuest(delayedQuest2.id, 0f);
				break;
			case QuestStatus.Completed:
				QuestSystem.QuestSystemData.CompleteQuest(delayedQuest2.id, 0f);
				break;
			}
		}
	}
}
