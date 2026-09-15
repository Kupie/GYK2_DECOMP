using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x02000203 RID: 515
[Serializable]
public class QuestFinishCheck : QuestCheck
{
	// Token: 0x06000C9F RID: 3231 RVA: 0x0003F894 File Offset: 0x0003DA94
	public AnswerData GetAnswerDataByReqs()
	{
		AnswerData answerData = new AnswerData();
		foreach (QuestPhraseRequirement questPhraseRequirement in this.phraseReqs)
		{
			switch (questPhraseRequirement.entity)
			{
			case QuestPhraseRequirement.Entity.Item:
				if (questPhraseRequirement.requirement == QuestPhraseRequirement.Requirement.Lock)
				{
					answerData.AddLockRes(new SmartRes
					{
						items = new List<ItemCount> { questPhraseRequirement.itemCount }
					});
				}
				else
				{
					answerData.AddCostRes(new SmartRes
					{
						items = new List<ItemCount> { questPhraseRequirement.itemCount }
					});
				}
				break;
			case QuestPhraseRequirement.Entity.GameResAtom:
				if (questPhraseRequirement.requirement == QuestPhraseRequirement.Requirement.Lock)
				{
					answerData.AddLockRes(new SmartRes
					{
						gameRes = new GameRes(new List<GameResAtom> { questPhraseRequirement.gameResAtom })
					});
				}
				else
				{
					answerData.AddCostRes(new SmartRes
					{
						gameRes = new GameRes(new List<GameResAtom> { questPhraseRequirement.gameResAtom })
					});
				}
				break;
			case QuestPhraseRequirement.Entity.Day:
				answerData.AddDay(questPhraseRequirement.dayNumber);
				break;
			case QuestPhraseRequirement.Entity.Order:
				answerData.AddOrder(questPhraseRequirement.order);
				break;
			}
		}
		return answerData;
	}

	// Token: 0x06000CA0 RID: 3232 RVA: 0x0003F9E4 File Offset: 0x0003DBE4
	public bool IsReadyToFinish()
	{
		PlayerData playerData = MainGame.PlayerData;
		foreach (QuestPhraseRequirement questPhraseRequirement in this.phraseReqs)
		{
			switch (questPhraseRequirement.entity)
			{
			case QuestPhraseRequirement.Entity.Item:
			{
				QuestPhraseRequirement.Requirement requirement = questPhraseRequirement.requirement;
				if ((requirement == QuestPhraseRequirement.Requirement.Lock || requirement == QuestPhraseRequirement.Requirement.Price) && !playerData.Inventory.Data.HasItemQuantityInInventory(questPhraseRequirement.itemCount.itemId, questPhraseRequirement.itemCount.count))
				{
					return false;
				}
				break;
			}
			case QuestPhraseRequirement.Entity.GameResAtom:
			{
				QuestPhraseRequirement.Requirement requirement = questPhraseRequirement.requirement;
				if ((requirement == QuestPhraseRequirement.Requirement.Lock || requirement == QuestPhraseRequirement.Requirement.Price) && !playerData.IsEnoughRes(questPhraseRequirement.gameResAtom))
				{
					return false;
				}
				break;
			}
			case QuestPhraseRequirement.Entity.Day:
				if (MainGame.Instance.GameSave.environmentData.CurrentDayNumber != ConstDef.Get(questPhraseRequirement.dayNumber).IntValue)
				{
					return false;
				}
				break;
			case QuestPhraseRequirement.Entity.Order:
				if (!MainGame.Instance.GameSave.vendorSystem.IsOrderFinished(questPhraseRequirement.order))
				{
					return false;
				}
				break;
			}
		}
		return true;
	}

	// Token: 0x04000EC3 RID: 3779
	public string phrase;

	// Token: 0x04000EC4 RID: 3780
	public List<QuestPhraseRequirement> phraseReqs = new List<QuestPhraseRequirement>();
}
