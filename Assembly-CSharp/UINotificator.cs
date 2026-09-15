using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000894 RID: 2196
public class UINotificator : LazySingleton<UINotificator>, ILazyGUIElement
{
	// Token: 0x17000867 RID: 2151
	// (get) Token: 0x0600385F RID: 14431 RVA: 0x0010EBA1 File Offset: 0x0010CDA1
	public IReadOnlyList<UIBaseNotification> DisplayingItems
	{
		get
		{
			return this.displayingItems;
		}
	}

	// Token: 0x06003860 RID: 14432 RVA: 0x0010EBAC File Offset: 0x0010CDAC
	public void Init()
	{
		MainGame.OnGameStarted = (Action)Delegate.Combine(MainGame.OnGameStarted, new Action(this.OnGameStarted));
		MainGame.OnGoToMainMenu = (Action)Delegate.Combine(MainGame.OnGoToMainMenu, new Action(this.OnGoToMainMenu));
		LazyPooler.CreatePool<UIItemAddNotification>(this.prefabsParent.GetComponentInChildren<UIItemAddNotification>(true), 1, Pool.PoolType.ImmediateActivation, false, false, null);
		LazyPooler.CreatePool<UIItemRemoveNotification>(this.prefabsParent.GetComponentInChildren<UIItemRemoveNotification>(true), 1, Pool.PoolType.ImmediateActivation, false, false, null);
		LazyPooler.CreatePool<UIInspirationNotification>(this.prefabsParent.GetComponentInChildren<UIInspirationNotification>(true), 1, Pool.PoolType.ImmediateActivation, false, false, null);
		LazyPooler.CreatePool<UIQuestStartNotification>(this.prefabsParent.GetComponentInChildren<UIQuestStartNotification>(true), 1, Pool.PoolType.ImmediateActivation, false, false, null);
		LazyPooler.CreatePool<UIQuestCompleteNotification>(this.prefabsParent.GetComponentInChildren<UIQuestCompleteNotification>(true), 1, Pool.PoolType.ImmediateActivation, false, false, null);
		LazyPooler.CreatePool<UIMoneyChangedNotification>(this.prefabsParent.GetComponentInChildren<UIMoneyChangedNotification>(true), 1, Pool.PoolType.ImmediateActivation, false, false, null);
		LazyPooler.CreatePool<UISimpleTextNotification>(this.prefabsParent.GetComponentInChildren<UISimpleTextNotification>(true), 1, Pool.PoolType.ImmediateActivation, false, false, null);
		LazyPooler.CreatePool<UISimpleTextWithIconNotification>(this.prefabsParent.GetComponentInChildren<UISimpleTextWithIconNotification>(true), 1, Pool.PoolType.ImmediateActivation, false, false, null);
		LazyPooler.CreatePool<UINewBodyNotification>(this.prefabsParent.GetComponentInChildren<UINewBodyNotification>(true), 1, Pool.PoolType.ImmediateActivation, false, false, null);
		LazyPooler.CreatePool<UITechRepUnlockedNotification>(this.prefabsParent.GetComponentInChildren<UITechRepUnlockedNotification>(true), 1, Pool.PoolType.ImmediateActivation, false, false, null);
		LazyPooler.CreatePool<UITechRevealedNotification>(this.prefabsParent.GetComponentInChildren<UITechRevealedNotification>(true), 1, Pool.PoolType.ImmediateActivation, false, false, null);
		LazyPooler.CreatePool<UICustomizationUnlockedNotification>(this.prefabsParent.GetComponentInChildren<UICustomizationUnlockedNotification>(true), 1, Pool.PoolType.ImmediateActivation, false, false, null);
		LazyPooler.CreatePool<UITalentLevelUpRevealedNotification>(this.prefabsParent.GetComponentInChildren<UITalentLevelUpRevealedNotification>(true), 1, Pool.PoolType.ImmediateActivation, false, false, null);
	}

	// Token: 0x06003861 RID: 14433 RVA: 0x0010ED24 File Offset: 0x0010CF24
	private void OnGameStarted()
	{
		MainGame.Instance.GameSave.talentSystemData.OnInspirationCompleted += this.HandleInspiration;
		MainGame.PlayerData.OnDropCollected += this.HandleAddItems;
		MainGame.PlayerData.inventory.OnInventoryFull += this.HandleInventoryFull;
		MainGame.Instance.GameSave.questSystemData.OnQuestStarted += this.HandleQuestStarted;
		MainGame.Instance.GameSave.questSystemData.OnQuestCompleted += this.HandleQuestCompleted;
		PlayerMoneyGameResSystem system = PlayerMoneyGameResSystem.GetSystem();
		system.onValueChangedDiff = (Action<float>)Delegate.Combine(system.onValueChangedDiff, new Action<float>(this.HandleMoneyChanged));
		KnowledgeSystem.OnTechRevealed = (Action<string>)Delegate.Combine(KnowledgeSystem.OnTechRevealed, new Action<string>(this.HandleTechRevealed));
		KnowledgeSystem.OnTechUnlocked = (Action<string, bool>)Delegate.Combine(KnowledgeSystem.OnTechUnlocked, new Action<string, bool>(this.HandleTechUnlocked));
		KnowledgeSystem.OnTalentLevelUpRevealed = (Action<string>)Delegate.Combine(KnowledgeSystem.OnTalentLevelUpRevealed, new Action<string>(this.HandleTalentLevelUpRevealed));
		PlayerCustomizationData.OnCustomizationUnlocked = (Action<Item>)Delegate.Combine(PlayerCustomizationData.OnCustomizationUnlocked, new Action<Item>(this.HandleCustomizationUnlocked));
	}

	// Token: 0x06003862 RID: 14434 RVA: 0x0010EE68 File Offset: 0x0010D068
	private void OnGoToMainMenu()
	{
		MainGame.Instance.GameSave.talentSystemData.OnInspirationCompleted -= this.HandleInspiration;
		MainGame.PlayerData.OnDropCollected -= this.HandleAddItems;
		MainGame.PlayerData.inventory.OnInventoryFull -= this.HandleInventoryFull;
		MainGame.Instance.GameSave.questSystemData.OnQuestStarted -= this.HandleQuestStarted;
		MainGame.Instance.GameSave.questSystemData.OnQuestCompleted -= this.HandleQuestCompleted;
		PlayerMoneyGameResSystem system = PlayerMoneyGameResSystem.GetSystem();
		system.onValueChangedDiff = (Action<float>)Delegate.Remove(system.onValueChangedDiff, new Action<float>(this.HandleMoneyChanged));
		KnowledgeSystem.OnTechRevealed = (Action<string>)Delegate.Remove(KnowledgeSystem.OnTechRevealed, new Action<string>(this.HandleTechRevealed));
		KnowledgeSystem.OnTechUnlocked = (Action<string, bool>)Delegate.Remove(KnowledgeSystem.OnTechUnlocked, new Action<string, bool>(this.HandleTechUnlocked));
		KnowledgeSystem.OnTalentLevelUpRevealed = (Action<string>)Delegate.Remove(KnowledgeSystem.OnTalentLevelUpRevealed, new Action<string>(this.HandleTalentLevelUpRevealed));
		PlayerCustomizationData.OnCustomizationUnlocked = (Action<Item>)Delegate.Remove(PlayerCustomizationData.OnCustomizationUnlocked, new Action<Item>(this.HandleCustomizationUnlocked));
	}

	// Token: 0x06003863 RID: 14435 RVA: 0x0010EFAC File Offset: 0x0010D1AC
	private void ShowNotification(UIBaseNotification notification)
	{
		notification.RectTransform.SetParent(base.transform);
		notification.RectTransform.anchorMin = new Vector2(1f, 0f);
		notification.RectTransform.anchorMax = new Vector2(1f, 0f);
		notification.RectTransform.pivot = new Vector2(1f, 0f);
		notification.CurrentTime = 0f;
		notification.IsTimerActive = true;
		notification.RectTransform.localPosition = this.appearPoint.localPosition;
		notification.gameObject.SetActive(true);
		notification.Draw();
		notification.RectTransform.RefreshContentFitter();
		this.displayingItems.Add(notification);
		this.UpdateNotificationsPosition();
	}

	// Token: 0x06003864 RID: 14436 RVA: 0x0010F06F File Offset: 0x0010D26F
	private void HideNotification(UIBaseNotification notification)
	{
		notification.gameObject.SetActive(false);
		notification.ReleaseToPool();
	}

	// Token: 0x06003865 RID: 14437 RVA: 0x0010F084 File Offset: 0x0010D284
	private void UpdateNotificationsPosition()
	{
		if (this.displayingItems.Count != 0)
		{
			float num = 0f;
			for (int i = this.displayingItems.Count - 1; i >= 0; i--)
			{
				this.displayingItems[i].transform.DOKill(false);
				this.displayingItems[i].transform.DOLocalMove(this.startPoint.transform.localPosition + new Vector3(0f, num), this.animationTime, false).SetEase(Ease.Linear);
				num += this.offsetY + this.displayingItems[i].RectTransform.sizeDelta.y;
			}
		}
	}

	// Token: 0x06003866 RID: 14438 RVA: 0x0010F148 File Offset: 0x0010D348
	private void Update()
	{
		for (int i = this.displayingItems.Count - 1; i >= 0; i--)
		{
			UIBaseNotification notification = this.displayingItems[i];
			if (notification.IsTimerActive)
			{
				notification.CurrentTime += Time.deltaTime;
				if (notification.CurrentTime >= notification.displayingTime)
				{
					this.displayingItems.RemoveAt(i);
					notification.IsTimerActive = false;
					Vector3 localPosition = notification.transform.localPosition;
					localPosition.x += this.appearPoint.localPosition.x - this.startPoint.localPosition.x;
					notification.transform.DOKill(false);
					notification.transform.DOLocalMove(localPosition, this.animationTime, false).SetEase(Ease.Linear).onComplete = delegate
					{
						this.HideNotification(notification);
					};
				}
			}
		}
	}

	// Token: 0x06003867 RID: 14439 RVA: 0x0010F26C File Offset: 0x0010D46C
	public void ShowSimpleTextNotification(string locale)
	{
		UISimpleTextNotification @object = LazyPooler.GetObject<UISimpleTextNotification>();
		@object.LocalizationKey = locale;
		@object.Text = LLBase.L(locale);
		this.ShowNotification(@object);
	}

	// Token: 0x06003868 RID: 14440 RVA: 0x0010F29C File Offset: 0x0010D49C
	public void ShowSimpleTextWithIconNotification(string locale, string iconId)
	{
		UISimpleTextWithIconNotification @object = LazyPooler.GetObject<UISimpleTextWithIconNotification>();
		@object.LocalizationKey = locale;
		@object.Text = LLBase.L(locale);
		@object.IconId = iconId;
		this.ShowNotification(@object);
	}

	// Token: 0x06003869 RID: 14441 RVA: 0x0010F2D0 File Offset: 0x0010D4D0
	public void ShowNewBodyNotification(string bodyId)
	{
		BodyDef data = GameBalance.Me.GetData<BodyDef>(bodyId);
		if (data == null)
		{
			Debug.LogError("Can't ShowNewBodyNotification body def is null for:[" + bodyId + "]");
			return;
		}
		UINewBodyNotification @object = LazyPooler.GetObject<UINewBodyNotification>();
		@object.ItemDef = GameBalance.Me.GetData<ItemDef>(data.linkedBodyItemId);
		this.ShowNotification(@object);
	}

	// Token: 0x0600386A RID: 14442 RVA: 0x0010F325 File Offset: 0x0010D525
	public void ShowMoneyNotification(int money)
	{
		this.HandleMoneyChanged((float)money);
	}

	// Token: 0x0600386B RID: 14443 RVA: 0x0010F330 File Offset: 0x0010D530
	public void ShowSimpleTextNotificationOneTime(string locale)
	{
		if (UINotificator.isSilent)
		{
			return;
		}
		if (this.displayingItems.Count != 0)
		{
			foreach (UIBaseNotification uibaseNotification in this.displayingItems)
			{
				UISimpleTextNotification uisimpleTextNotification = uibaseNotification as UISimpleTextNotification;
				if (uisimpleTextNotification != null && !string.IsNullOrEmpty(uisimpleTextNotification.LocalizationKey) && uisimpleTextNotification.LocalizationKey == locale)
				{
					return;
				}
			}
		}
		this.ShowSimpleTextNotification(locale);
	}

	// Token: 0x0600386C RID: 14444 RVA: 0x0010F3C0 File Offset: 0x0010D5C0
	public void HandleInventoryFull()
	{
		this.ShowSimpleTextNotificationOneTime("inventory_is_full");
	}

	// Token: 0x0600386D RID: 14445 RVA: 0x0010F3D0 File Offset: 0x0010D5D0
	private void HandleAddItems(List<Item> items)
	{
		if (UINotificator.isSilent)
		{
			return;
		}
		foreach (Item item in items)
		{
			if (item == null)
			{
				Debug.LogError("UINotificator: error item is null");
			}
			else
			{
				bool flag = false;
				for (int i = this.displayingItems.Count - 1; i >= 0; i--)
				{
					UIItemAddNotification uiitemAddNotification = this.displayingItems[i] as UIItemAddNotification;
					if (uiitemAddNotification != null && uiitemAddNotification.ItemId == item.id && uiitemAddNotification.gameObject.activeSelf)
					{
						uiitemAddNotification.AddCountToItem(item.Count);
						LazyAudio.Play("item_pickup");
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					UIItemAddNotification @object = LazyPooler.GetObject<UIItemAddNotification>();
					@object.ItemId = item.id;
					@object.DisplayingCount = item.Count;
					this.ShowNotification(@object);
					LazyAudio.Play("item_pickup");
				}
			}
		}
	}

	// Token: 0x0600386E RID: 14446 RVA: 0x0010F4E0 File Offset: 0x0010D6E0
	private void HandleMoneyChanged(float money)
	{
		if (UINotificator.isSilent)
		{
			return;
		}
		if (this.displayingItems.Count != 0)
		{
			foreach (UIBaseNotification uibaseNotification in this.displayingItems)
			{
				UIMoneyChangedNotification uimoneyChangedNotification = uibaseNotification as UIMoneyChangedNotification;
				if (uimoneyChangedNotification != null)
				{
					uimoneyChangedNotification.AddCount((int)money);
					LazyAudio.Play("coins_sound");
					return;
				}
			}
		}
		UIMoneyChangedNotification @object = LazyPooler.GetObject<UIMoneyChangedNotification>();
		@object.DisplayingCount = (int)money;
		this.ShowNotification(@object);
		LazyAudio.Play("coins_sound");
	}

	// Token: 0x0600386F RID: 14447 RVA: 0x0010F580 File Offset: 0x0010D780
	private void HandleTechRevealed(string techId)
	{
		if (UINotificator.isSilent)
		{
			return;
		}
		UITechRevealedNotification @object = LazyPooler.GetObject<UITechRevealedNotification>();
		@object.TechId = techId;
		@object.IsTechUnlocked = false;
		this.ShowNotification(@object);
	}

	// Token: 0x06003870 RID: 14448 RVA: 0x0010F5B0 File Offset: 0x0010D7B0
	private void HandleTechUnlocked(string techId, bool forceSilent)
	{
		if (UINotificator.isSilent || forceSilent)
		{
			return;
		}
		TechDef data = GameBalance.Me.GetData<TechDef>(techId);
		if (data != null && data.wgoRepLock.List.Count > 0)
		{
			return;
		}
		UITechRevealedNotification @object = LazyPooler.GetObject<UITechRevealedNotification>();
		@object.TechId = techId;
		@object.IsTechUnlocked = true;
		this.ShowNotification(@object);
	}

	// Token: 0x06003871 RID: 14449 RVA: 0x0010F60C File Offset: 0x0010D80C
	private void HandleTechRepUnlocked(string techId)
	{
		if (UINotificator.isSilent)
		{
			return;
		}
		UITechRepUnlockedNotification @object = LazyPooler.GetObject<UITechRepUnlockedNotification>();
		@object.TechId = techId;
		this.ShowNotification(@object);
	}

	// Token: 0x06003872 RID: 14450 RVA: 0x0010F638 File Offset: 0x0010D838
	private void HandleInspiration(string inspirationId)
	{
		if (UINotificator.isSilent)
		{
			return;
		}
		if (MainGame.Instance.GameSave.knowledgeSystem.IsCharTabLocked(CharacterWindowData.CharPage.Inspiration))
		{
			return;
		}
		UIInspirationNotification @object = LazyPooler.GetObject<UIInspirationNotification>();
		@object.InspirationId = inspirationId;
		@object.InspirationTalentId = GameBalance.Me.GetData<InspirationDef>(inspirationId).talentId;
		this.ShowNotification(@object);
	}

	// Token: 0x06003873 RID: 14451 RVA: 0x0010F690 File Offset: 0x0010D890
	private void HandleQuestStarted(QuestData questData)
	{
		if (UINotificator.isSilent || questData.isHidden)
		{
			return;
		}
		UIQuestStartNotification @object = LazyPooler.GetObject<UIQuestStartNotification>();
		@object.QuestData = questData;
		this.ShowNotification(@object);
	}

	// Token: 0x06003874 RID: 14452 RVA: 0x0010F6C4 File Offset: 0x0010D8C4
	private void HandleQuestCompleted(QuestData questData)
	{
		if (UINotificator.isSilent || questData.isHidden)
		{
			return;
		}
		UIQuestCompleteNotification @object = LazyPooler.GetObject<UIQuestCompleteNotification>();
		@object.QuestData = questData;
		this.ShowNotification(@object);
	}

	// Token: 0x06003875 RID: 14453 RVA: 0x0010F6F8 File Offset: 0x0010D8F8
	private void HandleCustomizationUnlocked(Item sourceItem)
	{
		if (UINotificator.isSilent)
		{
			return;
		}
		UICustomizationUnlockedNotification @object = LazyPooler.GetObject<UICustomizationUnlockedNotification>();
		@object.SourceItem = sourceItem;
		this.ShowNotification(@object);
	}

	// Token: 0x06003876 RID: 14454 RVA: 0x0010F724 File Offset: 0x0010D924
	private void HandleTalentLevelUpRevealed(string talentLevelUpId)
	{
		if (UINotificator.isSilent)
		{
			return;
		}
		UITalentLevelUpRevealedNotification @object = LazyPooler.GetObject<UITalentLevelUpRevealedNotification>();
		@object.TalentLevelUpId = talentLevelUpId;
		this.ShowNotification(@object);
	}

	// Token: 0x06003877 RID: 14455 RVA: 0x0010F74D File Offset: 0x0010D94D
	public void ShowTechRevealNotification()
	{
		this.HandleTechRevealed("wood_basic");
	}

	// Token: 0x06003878 RID: 14456 RVA: 0x0010F75A File Offset: 0x0010D95A
	public void ShowTechRepUnlockedNotification()
	{
		this.HandleTechRepUnlocked("stone_basic");
	}

	// Token: 0x06003879 RID: 14457 RVA: 0x0010F767 File Offset: 0x0010D967
	public void ShowItemAddNotification()
	{
		this.HandleAddItems(new List<Item>
		{
			new Item("faith", 15)
		});
	}

	// Token: 0x0600387A RID: 14458 RVA: 0x0010F788 File Offset: 0x0010D988
	public void ShowQuestStartNotification()
	{
		MainGame.Instance.GameSave.questSystemData.questCollection.questsCache["6_intro_guards_burial"].isHidden = false;
		MainGame.Instance.GameSave.questSystemData.StartQuest("6_intro_guards_burial", 0f);
	}

	// Token: 0x0600387B RID: 14459 RVA: 0x0010F7DC File Offset: 0x0010D9DC
	public void ShowQuestCompleteNotification()
	{
		MainGame.Instance.GameSave.questSystemData.questCollection.questsCache["6_intro_guards_burial"].isHidden = false;
		MainGame.Instance.GameSave.questSystemData.CompleteQuest("6_intro_guards_burial", 0f);
	}

	// Token: 0x0600387C RID: 14460 RVA: 0x0010F830 File Offset: 0x0010DA30
	public void ShowInspirationNotification1()
	{
		this.HandleInspiration("lumberjack_1");
	}

	// Token: 0x0600387D RID: 14461 RVA: 0x0010F83D File Offset: 0x0010DA3D
	public void ShowInspirationNotification2()
	{
		this.HandleInspiration("recycler_1");
	}

	// Token: 0x0600387E RID: 14462 RVA: 0x0010F84A File Offset: 0x0010DA4A
	public void ShowInspirationNotification3()
	{
		this.HandleInspiration("insp_mushroom_hunter_1");
	}

	// Token: 0x0600387F RID: 14463 RVA: 0x0010F857 File Offset: 0x0010DA57
	public void ShowInspirationNotification4()
	{
		this.HandleInspiration("undertaker_1");
	}

	// Token: 0x06003880 RID: 14464 RVA: 0x0010F864 File Offset: 0x0010DA64
	public void ShowInspirationNotification5()
	{
		this.HandleInspiration("last_respects_1");
	}

	// Token: 0x06003881 RID: 14465 RVA: 0x0010F871 File Offset: 0x0010DA71
	public void ShowNewBodyNotification()
	{
		this.ShowNewBodyNotification("body_0_1");
	}

	// Token: 0x06003882 RID: 14466 RVA: 0x0010F87E File Offset: 0x0010DA7E
	public void ShowMoneyAddNotification()
	{
		this.HandleMoneyChanged(4321f);
	}

	// Token: 0x06003883 RID: 14467 RVA: 0x0010F88B File Offset: 0x0010DA8B
	public void ShowMoneyRemoveNotification()
	{
		this.HandleMoneyChanged(-1234f);
	}

	// Token: 0x06003884 RID: 14468 RVA: 0x0010F898 File Offset: 0x0010DA98
	public void ShowSimpleTextNotification1()
	{
		this.ShowSimpleTextNotification("parishioner_fail_1");
	}

	// Token: 0x06003885 RID: 14469 RVA: 0x0010F8A5 File Offset: 0x0010DAA5
	public void ShowSimpleTextNotification2()
	{
		this.ShowSimpleTextNotification("1_intro_prison_wake_5");
	}

	// Token: 0x04002CDC RID: 11484
	public static bool isSilent;

	// Token: 0x04002CDD RID: 11485
	[SerializeField]
	private Transform appearPoint;

	// Token: 0x04002CDE RID: 11486
	[SerializeField]
	private Transform startPoint;

	// Token: 0x04002CDF RID: 11487
	[SerializeField]
	private float offsetY;

	// Token: 0x04002CE0 RID: 11488
	[SerializeField]
	private float animationTime = 0.2f;

	// Token: 0x04002CE1 RID: 11489
	[SerializeField]
	private GameObject prefabsParent;

	// Token: 0x04002CE2 RID: 11490
	private List<UIBaseNotification> displayingItems = new List<UIBaseNotification>();
}
