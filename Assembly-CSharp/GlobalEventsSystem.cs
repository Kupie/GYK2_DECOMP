using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004A3 RID: 1187
[Serializable]
public class GlobalEventsSystem
{
	// Token: 0x17000548 RID: 1352
	// (get) Token: 0x06001F89 RID: 8073 RVA: 0x00095371 File Offset: 0x00093571
	public static GlobalEventsSystem Me
	{
		get
		{
			return MainGame.Instance.GameSave.globalEventsSystem;
		}
	}

	// Token: 0x06001F8A RID: 8074 RVA: 0x00095382 File Offset: 0x00093582
	public void PrepareForGame()
	{
		this.checkingEvents = new List<GlobalEventsSystem.Event>();
		this.eventsCache = new Dictionary<GlobalEventsSystem.Event.Type, Dictionary<string, GlobalEventsSystem.Event>>();
	}

	// Token: 0x06001F8B RID: 8075 RVA: 0x0009539C File Offset: 0x0009359C
	public void AddEvent(IEventTrigerrable trigerrable)
	{
		GlobalEventsSystem.Event.Type type = trigerrable.Type;
		if (type == GlobalEventsSystem.Event.Type.None)
		{
			return;
		}
		string triggerableId = trigerrable.TriggerableId;
		if (SGuid.IsNullOrEmpty(trigerrable.UniqueId))
		{
			trigerrable.UniqueId = new SGuid();
		}
		Dictionary<string, GlobalEventsSystem.Event> dictionary;
		GlobalEventsSystem.Event @event;
		if (!this.eventsCache.TryGetValue(type, out dictionary))
		{
			@event = new GlobalEventsSystem.Event
			{
				type = type,
				id = triggerableId,
				hasId = !string.IsNullOrEmpty(triggerableId),
				trigerrables = new List<IEventTrigerrable> { trigerrable }
			};
			dictionary = new Dictionary<string, GlobalEventsSystem.Event> { { triggerableId, @event } };
			this.eventsCache.Add(@event.type, dictionary);
		}
		else
		{
			if (dictionary.TryGetValue(triggerableId, out @event))
			{
				@event.trigerrables.Add(trigerrable);
				return;
			}
			@event = new GlobalEventsSystem.Event
			{
				type = type,
				id = triggerableId,
				hasId = !string.IsNullOrEmpty(triggerableId),
				trigerrables = new List<IEventTrigerrable> { trigerrable }
			};
			dictionary.Add(triggerableId, @event);
		}
		this.checkingEvents.Add(@event);
	}

	// Token: 0x06001F8C RID: 8076 RVA: 0x000954A0 File Offset: 0x000936A0
	public void RemoveEvent(IEventTrigerrable trigerrable)
	{
		GlobalEventsSystem.Event.Type type = trigerrable.Type;
		string triggerableId = trigerrable.TriggerableId;
		Dictionary<string, GlobalEventsSystem.Event> dictionary;
		GlobalEventsSystem.Event @event;
		if (this.eventsCache.TryGetValue(type, out dictionary) && dictionary.TryGetValue(triggerableId, out @event))
		{
			Debug.Log(string.Format("Remove Event: {0} {1} {2}", type, triggerableId, trigerrable.UniqueId));
			Debug.Log(string.Format("Available events: {0}:", @event.trigerrables.Count));
			foreach (IEventTrigerrable eventTrigerrable in @event.trigerrables)
			{
				Debug.Log(string.Format("  - {0} {1} {2}", eventTrigerrable.TriggerableId, eventTrigerrable.Type, eventTrigerrable.UniqueId));
			}
			int num = @event.trigerrables.FindIndex((IEventTrigerrable t) => t.TriggerableId == trigerrable.TriggerableId && t.Type == trigerrable.Type && t.UniqueId == trigerrable.UniqueId);
			if (num != -1)
			{
				@event.trigerrables.RemoveAt(num);
			}
			if (@event.trigerrables.Count == 0)
			{
				dictionary.Remove(triggerableId);
				if (dictionary.Count == 0)
				{
					this.eventsCache.Remove(type);
				}
				this.checkingEvents.Remove(@event);
			}
		}
	}

	// Token: 0x06001F8D RID: 8077 RVA: 0x00095608 File Offset: 0x00093808
	public static void FireTrigger(GlobalEventsSystem.Event.Type type, string id = "")
	{
		MainGame instance = MainGame.Instance;
		GlobalEventsSystem globalEventsSystem;
		if (instance == null)
		{
			globalEventsSystem = null;
		}
		else
		{
			GameSave gameSave = instance.GameSave;
			globalEventsSystem = ((gameSave != null) ? gameSave.globalEventsSystem : null);
		}
		GlobalEventsSystem globalEventsSystem2 = globalEventsSystem;
		if (((globalEventsSystem2 != null) ? globalEventsSystem2.eventsCache : null) == null)
		{
			return;
		}
		Dictionary<string, GlobalEventsSystem.Event> dictionary;
		GlobalEventsSystem.Event @event;
		if (globalEventsSystem2.eventsCache.TryGetValue(type, out dictionary) && dictionary != null && dictionary.TryGetValue(id, out @event))
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			string text = string.Empty;
			string text2 = string.Empty;
			foreach (IEventTrigerrable eventTrigerrable in @event.trigerrables)
			{
				if (eventTrigerrable.OnTriggerPassed())
				{
					QuestFinishCheck questFinishCheck = eventTrigerrable as QuestFinishCheck;
					if (questFinishCheck != null)
					{
						if (questFinishCheck.runModificator == QuestCheck.RunModificator.TriggerSolo)
						{
							if (!string.IsNullOrEmpty(text2))
							{
								continue;
							}
							text2 = questFinishCheck.questId;
						}
						list.Add(questFinishCheck.questId);
					}
					else
					{
						QuestCheck questCheck = eventTrigerrable as QuestCheck;
						if (questCheck != null)
						{
							if (questCheck.runModificator == QuestCheck.RunModificator.TriggerSolo)
							{
								if (!string.IsNullOrEmpty(text))
								{
									continue;
								}
								text = questCheck.questId;
							}
							list3.Add(questCheck.questId);
						}
					}
				}
				else
				{
					QuestCheck questCheck2 = eventTrigerrable as QuestCheck;
					if (questCheck2 != null)
					{
						list2.Add(questCheck2.questId);
					}
				}
			}
			foreach (string text3 in list)
			{
				MainGame.Instance.GameSave.questSystemData.CompleteQuest(text3, 0f);
			}
			foreach (string text4 in list2)
			{
				MainGame.Instance.GameSave.questSystemData.OnStartFailed(text4);
			}
			foreach (string text5 in list3)
			{
				MainGame.Instance.GameSave.questSystemData.StartQuest(text5, 0f);
			}
		}
	}

	// Token: 0x06001F8E RID: 8078 RVA: 0x00095860 File Offset: 0x00093A60
	public bool GetEvents(GlobalEventsSystem.Event.Type type, out Dictionary<string, GlobalEventsSystem.Event> events)
	{
		if (this.eventsCache == null)
		{
			events = null;
			return false;
		}
		return this.eventsCache.TryGetValue(type, out events);
	}

	// Token: 0x04001C3D RID: 7229
	[NonSerialized]
	public List<GlobalEventsSystem.Event> checkingEvents = new List<GlobalEventsSystem.Event>();

	// Token: 0x04001C3E RID: 7230
	[NonSerialized]
	private Dictionary<GlobalEventsSystem.Event.Type, Dictionary<string, GlobalEventsSystem.Event>> eventsCache = new Dictionary<GlobalEventsSystem.Event.Type, Dictionary<string, GlobalEventsSystem.Event>>();

	// Token: 0x020004A4 RID: 1188
	[Serializable]
	public class Event
	{
		// Token: 0x04001C3F RID: 7231
		public GlobalEventsSystem.Event.Type type;

		// Token: 0x04001C40 RID: 7232
		public string id;

		// Token: 0x04001C41 RID: 7233
		public bool hasId;

		// Token: 0x04001C42 RID: 7234
		public List<IEventTrigerrable> trigerrables = new List<IEventTrigerrable>();

		// Token: 0x020004A5 RID: 1189
		public enum Type
		{
			// Token: 0x04001C44 RID: 7236
			None,
			// Token: 0x04001C45 RID: 7237
			StartNewGame,
			// Token: 0x04001C46 RID: 7238
			PlayerInsertOverheadToWgoAnItem,
			// Token: 0x04001C47 RID: 7239
			PlayerTakeFromWgoTheItem,
			// Token: 0x04001C48 RID: 7240
			PlayerUseItem,
			// Token: 0x04001C49 RID: 7241
			PlayerTeleport,
			// Token: 0x04001C4A RID: 7242
			PlayerTeleportAfterFadeOut,
			// Token: 0x04001C4B RID: 7243
			PlayerEnterGDZone,
			// Token: 0x04001C4C RID: 7244
			PlayerExitGDZone,
			// Token: 0x04001C4D RID: 7245
			BuildBuilding,
			// Token: 0x04001C4E RID: 7246
			CraftStart,
			// Token: 0x04001C4F RID: 7247
			CraftFinish,
			// Token: 0x04001C50 RID: 7248
			WgoDead,
			// Token: 0x04001C51 RID: 7249
			WalkIntoWorldZone,
			// Token: 0x04001C52 RID: 7250
			AddOverhead,
			// Token: 0x04001C53 RID: 7251
			MultiAnswerSay,
			// Token: 0x04001C54 RID: 7252
			SpeechSay,
			// Token: 0x04001C55 RID: 7253
			SpeechSaid,
			// Token: 0x04001C56 RID: 7254
			CloseUIWindow = 20,
			// Token: 0x04001C57 RID: 7255
			OpenUIWindow,
			// Token: 0x04001C58 RID: 7256
			CustomInteraction,
			// Token: 0x04001C59 RID: 7257
			WgoGoToFinished,
			// Token: 0x04001C5A RID: 7258
			PlayerStartMoving,
			// Token: 0x04001C5B RID: 7259
			PlayerFindWgoToWork,
			// Token: 0x04001C5C RID: 7260
			Interaction,
			// Token: 0x04001C5D RID: 7261
			AfterSleep,
			// Token: 0x04001C5E RID: 7262
			PlayerInsertBodyToAutopsy,
			// Token: 0x04001C5F RID: 7263
			FightSectorCaptured,
			// Token: 0x04001C60 RID: 7264
			FightSectorLost,
			// Token: 0x04001C61 RID: 7265
			FightLineCaptured,
			// Token: 0x04001C62 RID: 7266
			FightLineLost,
			// Token: 0x04001C63 RID: 7267
			PlayerDead,
			// Token: 0x04001C64 RID: 7268
			PlayerHpValueReached,
			// Token: 0x04001C65 RID: 7269
			WgoCustomTagDead,
			// Token: 0x04001C66 RID: 7270
			WgoCustomTagHpValueReached,
			// Token: 0x04001C67 RID: 7271
			PlayerChangeControlByFlow,
			// Token: 0x04001C68 RID: 7272
			ConveyorChestItemAdded,
			// Token: 0x04001C69 RID: 7273
			FightWon,
			// Token: 0x04001C6A RID: 7274
			FightLost,
			// Token: 0x04001C6B RID: 7275
			DonkeyStart,
			// Token: 0x04001C6C RID: 7276
			DonkeyMorgue,
			// Token: 0x04001C6D RID: 7277
			PlayerInsertBodyWithSkulls,
			// Token: 0x04001C6E RID: 7278
			CustomFlowTrigger,
			// Token: 0x04001C6F RID: 7279
			FirstCloseCraftWindow,
			// Token: 0x04001C70 RID: 7280
			RepairTownCluster,
			// Token: 0x04001C71 RID: 7281
			RemoveWgoDataFromScene
		}
	}
}
