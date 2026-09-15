using System;
using System.Collections.Generic;
using System.Linq;
using LazyBearTechnology;
using LinqTools;

// Token: 0x02000986 RID: 2438
public class UIBaseCraftWindowData : LazyWidgetDataBase
{
	// Token: 0x170009BB RID: 2491
	// (get) Token: 0x06004089 RID: 16521 RVA: 0x00134D2D File Offset: 0x00132F2D
	// (set) Token: 0x0600408A RID: 16522 RVA: 0x00134D35 File Offset: 0x00132F35
	public CraftComponent CraftComponent { get; private set; }

	// Token: 0x170009BC RID: 2492
	// (get) Token: 0x0600408B RID: 16523 RVA: 0x00134D3E File Offset: 0x00132F3E
	// (set) Token: 0x0600408C RID: 16524 RVA: 0x00134D46 File Offset: 0x00132F46
	public HashSet<CraftDef> AllCrafts { get; private set; }

	// Token: 0x170009BD RID: 2493
	// (get) Token: 0x0600408D RID: 16525 RVA: 0x00134D4F File Offset: 0x00132F4F
	// (set) Token: 0x0600408E RID: 16526 RVA: 0x00134D57 File Offset: 0x00132F57
	public List<KeyValuePair<string, List<CraftDef>>> CraftsByTabs { get; private set; }

	// Token: 0x170009BE RID: 2494
	// (get) Token: 0x0600408F RID: 16527 RVA: 0x00134D60 File Offset: 0x00132F60
	// (set) Token: 0x06004090 RID: 16528 RVA: 0x00134D68 File Offset: 0x00132F68
	public List<KeyValuePair<string, List<CraftDef>>> ExtensionCrafts { get; private set; } = new List<KeyValuePair<string, List<CraftDef>>>();

	// Token: 0x170009BF RID: 2495
	// (get) Token: 0x06004091 RID: 16529 RVA: 0x00134D71 File Offset: 0x00132F71
	// (set) Token: 0x06004092 RID: 16530 RVA: 0x00134D79 File Offset: 0x00132F79
	public Action<CraftElement> OnAddToQueuePressed { get; private set; }

	// Token: 0x170009C0 RID: 2496
	// (get) Token: 0x06004093 RID: 16531 RVA: 0x00134D82 File Offset: 0x00132F82
	// (set) Token: 0x06004094 RID: 16532 RVA: 0x00134D8A File Offset: 0x00132F8A
	public Action<CraftElement> OnStartCraftPressed { get; private set; }

	// Token: 0x170009C1 RID: 2497
	// (get) Token: 0x06004095 RID: 16533 RVA: 0x00134D93 File Offset: 0x00132F93
	// (set) Token: 0x06004096 RID: 16534 RVA: 0x00134D9B File Offset: 0x00132F9B
	public Action<CraftElementBase> OnQueueElementRemovePressed { get; private set; }

	// Token: 0x170009C2 RID: 2498
	// (get) Token: 0x06004097 RID: 16535 RVA: 0x00134DA4 File Offset: 0x00132FA4
	// (set) Token: 0x06004098 RID: 16536 RVA: 0x00134DAC File Offset: 0x00132FAC
	public UIInfoWidgetData InfoWidgetData { get; private set; }

	// Token: 0x170009C3 RID: 2499
	// (get) Token: 0x06004099 RID: 16537 RVA: 0x00134DB5 File Offset: 0x00132FB5
	// (set) Token: 0x0600409A RID: 16538 RVA: 0x00134DBD File Offset: 0x00132FBD
	public Wgo AssignedWgo { get; private set; }

	// Token: 0x170009C4 RID: 2500
	// (get) Token: 0x0600409B RID: 16539 RVA: 0x00134DC6 File Offset: 0x00132FC6
	// (set) Token: 0x0600409C RID: 16540 RVA: 0x00134DCE File Offset: 0x00132FCE
	public bool IsAutocraftsWgo { get; private set; }

	// Token: 0x170009C5 RID: 2501
	// (get) Token: 0x0600409D RID: 16541 RVA: 0x00134DD7 File Offset: 0x00132FD7
	public bool IsGravePartRemove
	{
		get
		{
			return this.isGravePartRemove;
		}
	}

	// Token: 0x170009C6 RID: 2502
	// (get) Token: 0x0600409E RID: 16542 RVA: 0x00134DE0 File Offset: 0x00132FE0
	public bool IsAddToQueueDisabledForAllCrafts
	{
		get
		{
			bool flag = false;
			bool flag2 = true;
			foreach (CraftDef craftDef in this.EnumerateDisplayedCrafts())
			{
				flag = true;
				if (!UIBaseCraftWindowData.IsCraftQueueDisabled(craftDef))
				{
					flag2 = false;
				}
			}
			return flag && flag2;
		}
	}

	// Token: 0x0600409F RID: 16543 RVA: 0x00134E38 File Offset: 0x00133038
	private static bool IsCraftQueueDisabled(CraftDef craft)
	{
		return craft.isAddToQueueDisabled || craft.skipQueue;
	}

	// Token: 0x060040A0 RID: 16544 RVA: 0x00134E4A File Offset: 0x0013304A
	private IEnumerable<CraftDef> EnumerateDisplayedCrafts()
	{
		HashSet<CraftDef> seen = new HashSet<CraftDef>();
		if (this.CraftsByTabs != null)
		{
			foreach (KeyValuePair<string, List<CraftDef>> keyValuePair in this.CraftsByTabs)
			{
				if (keyValuePair.Value != null && keyValuePair.Value.Count != 0)
				{
					foreach (CraftDef craftDef in keyValuePair.Value)
					{
						if (craftDef != null && seen.Add(craftDef))
						{
							yield return craftDef;
						}
					}
					List<CraftDef>.Enumerator enumerator2 = default(List<CraftDef>.Enumerator);
				}
			}
			List<KeyValuePair<string, List<CraftDef>>>.Enumerator enumerator = default(List<KeyValuePair<string, List<CraftDef>>>.Enumerator);
		}
		if (this.ExtensionCrafts != null)
		{
			foreach (KeyValuePair<string, List<CraftDef>> keyValuePair2 in this.ExtensionCrafts)
			{
				if (keyValuePair2.Value != null && keyValuePair2.Value.Count != 0)
				{
					foreach (CraftDef craftDef2 in keyValuePair2.Value)
					{
						if (craftDef2 != null && seen.Add(craftDef2))
						{
							yield return craftDef2;
						}
					}
					List<CraftDef>.Enumerator enumerator2 = default(List<CraftDef>.Enumerator);
				}
			}
			List<KeyValuePair<string, List<CraftDef>>>.Enumerator enumerator = default(List<KeyValuePair<string, List<CraftDef>>>.Enumerator);
		}
		yield break;
		yield break;
	}

	// Token: 0x060040A1 RID: 16545 RVA: 0x00134E5C File Offset: 0x0013305C
	public UIBaseCraftWindowData(Wgo wgo, Action<CraftElement> onAddToQueuePressed, Action<CraftElement> onStartCraftPressed = null)
	{
		UIBaseCraftWindowData <>4__this = this;
		this.AssignedWgo = wgo;
		this.isAlchemyWorkbench = this.AssignedWgo.Data.id == "alchemy_workbench";
		this.CraftComponent = wgo.Data.CraftComponent;
		this.AllCrafts = wgo.Data.CraftComponent.CraftsIn.OfType<CraftDef>().ToHashSet<CraftDef>();
		KnowledgeSystem knowledge = MainGame.Instance.GameSave.knowledgeSystem;
		Func<CraftDef, bool> <>9__0;
		foreach (string text in wgo.Data.Definition.attachedWorkbenchExtensionIds)
		{
			IEnumerable<CraftDef> workbenchExtensionCrafts = GameBalance.Me.GetWorkbenchExtensionCrafts(wgo.Data.id, text);
			Func<CraftDef, bool> func;
			if ((func = <>9__0) == null)
			{
				func = (<>9__0 = (CraftDef craft) => !<>4__this.isAlchemyWorkbench && !knowledge.IsOneTimeCraftCompleted(craft));
			}
			List<CraftDef> list = workbenchExtensionCrafts.Where(func).ToList<CraftDef>();
			if (list.Count != 0)
			{
				this.ExtensionCrafts.Add(new KeyValuePair<string, List<CraftDef>>(text, list));
			}
		}
		this.OnAddToQueuePressed = onAddToQueuePressed;
		this.OnStartCraftPressed = onStartCraftPressed;
		this.OnQueueElementRemovePressed = new Action<CraftElementBase>(this.RemoveQueueElement);
		this.InfoWidgetData = new UIInfoWidgetData(this.AssignedWgo.Data, null, true);
		this.IsAutocraftsWgo = wgo.Data.CraftComponent.HasCraftsByBalance && wgo.Data.CraftComponent.AvailableCrafts[0].isAuto;
		this.SortCraftsByTabs();
		List<string> unlockedCrafts = knowledge.unlockedCrafts;
		Comparison<CraftDef> <>9__1;
		foreach (KeyValuePair<string, List<CraftDef>> keyValuePair in this.CraftsByTabs.Concat(this.ExtensionCrafts))
		{
			List<CraftDef> value = keyValuePair.Value;
			Comparison<CraftDef> comparison;
			if ((comparison = <>9__1) == null)
			{
				comparison = (<>9__1 = delegate(CraftDef x, CraftDef y)
				{
					bool flag = unlockedCrafts.Contains(x.id);
					bool flag2 = unlockedCrafts.Contains(y.id);
					if (flag)
					{
						if (flag2)
						{
							return 0;
						}
						return -1;
					}
					else
					{
						if (flag2)
						{
							return 1;
						}
						return 0;
					}
				});
			}
			value.Sort(comparison);
		}
	}

	// Token: 0x060040A2 RID: 16546 RVA: 0x00135094 File Offset: 0x00133294
	public UIBaseCraftWindowData(Wgo graveWgo, Item gravePart, Action<CraftElement> onCraftPressed)
	{
		this.AssignedWgo = graveWgo;
		this.CraftComponent = graveWgo.Data.CraftComponent;
		this.OnAddToQueuePressed = onCraftPressed;
		this.OnStartCraftPressed = onCraftPressed;
		this.InfoWidgetData = new UIInfoWidgetData(graveWgo.Data, gravePart, true);
		this.FillGravePartCrafts(gravePart);
		this.isGravePartRemove = true;
	}

	// Token: 0x060040A3 RID: 16547 RVA: 0x001350FC File Offset: 0x001332FC
	public void SubscribeEvents()
	{
		if (!this.subscribedEvents)
		{
			this.CraftComponent.OnCraftAddedToQueue += this.onCraftAddedToQueue;
			this.CraftComponent.OnCraftRemovedFromQueue += this.onCraftRemovedFromQueue;
			this.CraftComponent.OnCraftStart += this.onCraftStarted;
			this.CraftComponent.OnCraftFinish += this.onCraftEnded;
			this.subscribedEvents = true;
		}
	}

	// Token: 0x060040A4 RID: 16548 RVA: 0x0013515C File Offset: 0x0013335C
	public void UnsubscribeEvents()
	{
		if (this.subscribedEvents)
		{
			this.CraftComponent.OnCraftAddedToQueue -= this.onCraftAddedToQueue;
			this.CraftComponent.OnCraftRemovedFromQueue -= this.onCraftRemovedFromQueue;
			this.CraftComponent.OnCraftStart -= this.onCraftStarted;
			this.CraftComponent.OnCraftFinish -= this.onCraftEnded;
			this.subscribedEvents = false;
		}
	}

	// Token: 0x060040A5 RID: 16549 RVA: 0x001351BC File Offset: 0x001333BC
	public static CraftDef FindGravePartRemoveCraft(CraftComponent craftComponent, Item gravePartItem)
	{
		if (((craftComponent != null) ? craftComponent.AvailableCrafts : null) == null || gravePartItem == null)
		{
			return null;
		}
		foreach (CraftDefBase craftDefBase in craftComponent.AvailableCrafts)
		{
			CraftDef craftDef = craftDefBase as CraftDef;
			if (craftDef != null && (UIBaseCraftWindowData.ContainsGravePart(craftDef.dropFromWgoItemsEnd, gravePartItem.id) || UIBaseCraftWindowData.ContainsGravePart(craftDef.dropFromWgoItemsStart, gravePartItem.id)))
			{
				return craftDef;
			}
		}
		return null;
	}

	// Token: 0x060040A6 RID: 16550 RVA: 0x00135250 File Offset: 0x00133450
	private static bool ContainsGravePart(List<NeedItemData> dropItems, string gravePartItemId)
	{
		if (dropItems == null)
		{
			return false;
		}
		using (List<NeedItemData>.Enumerator enumerator = dropItems.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.id == gravePartItemId)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060040A7 RID: 16551 RVA: 0x001352B0 File Offset: 0x001334B0
	private void FillGravePartCrafts(Item gravePartItem)
	{
		CraftDef craftDef = UIBaseCraftWindowData.FindGravePartRemoveCraft(this.CraftComponent, gravePartItem);
		HashSet<CraftDef> hashSet;
		if (craftDef == null)
		{
			hashSet = new HashSet<CraftDef>();
		}
		else
		{
			(hashSet = new HashSet<CraftDef>()).Add(craftDef);
		}
		this.AllCrafts = hashSet;
		this.SortCraftsByTabs();
	}

	// Token: 0x060040A8 RID: 16552 RVA: 0x001352F0 File Offset: 0x001334F0
	private void RemoveQueueElement(CraftElementBase craftElement)
	{
		CraftElementBase currentCraftElement = this.CraftComponent.CurrentCraftElement;
		if (currentCraftElement != null && currentCraftElement == craftElement)
		{
			this.CraftComponent.RemoveCurNotStartedCraft();
			return;
		}
		this.CraftComponent.RemoveFromQueue(craftElement, false);
	}

	// Token: 0x060040A9 RID: 16553 RVA: 0x0013532C File Offset: 0x0013352C
	private void SortCraftsByTabs()
	{
		this.CraftsByTabs = new List<KeyValuePair<string, List<CraftDef>>>();
		this.CraftsByTabs.Add(new KeyValuePair<string, List<CraftDef>>("default_tab", new List<CraftDef>()));
		foreach (CraftDef craftDef in this.AllCrafts)
		{
			if ((!this.isAlchemyWorkbench || MainGame.Instance.GameSave.knowledgeSystem.unlockedCrafts.Contains(craftDef.id)) && string.IsNullOrEmpty(craftDef.extensionNeedId))
			{
				string tabId = (string.IsNullOrEmpty(craftDef.tabId) ? "default_tab" : craftDef.tabId);
				int num = this.CraftsByTabs.FindIndex((KeyValuePair<string, List<CraftDef>> x) => x.Key == tabId);
				if (num != -1)
				{
					this.CraftsByTabs[num].Value.Add(craftDef);
				}
				else
				{
					this.CraftsByTabs.Add(new KeyValuePair<string, List<CraftDef>>(tabId, new List<CraftDef> { craftDef }));
				}
			}
		}
	}

	// Token: 0x0400329F RID: 12959
	public const string DEFAULT_TAB_ID = "default_tab";

	// Token: 0x040032A7 RID: 12967
	public CraftComponent.DelCraftAddedToQueue onCraftAddedToQueue;

	// Token: 0x040032A8 RID: 12968
	public CraftComponent.DelCraftRemovedFromQueue onCraftRemovedFromQueue;

	// Token: 0x040032A9 RID: 12969
	public Action onCraftStarted;

	// Token: 0x040032AA RID: 12970
	public Action onCraftEnded;

	// Token: 0x040032AB RID: 12971
	private bool subscribedEvents;

	// Token: 0x040032AC RID: 12972
	private bool isAlchemyWorkbench;

	// Token: 0x040032AD RID: 12973
	private bool isGravePartRemove;
}
