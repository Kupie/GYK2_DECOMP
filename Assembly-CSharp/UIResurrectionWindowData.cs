using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000A28 RID: 2600
public class UIResurrectionWindowData : LazyWidgetDataBase
{
	// Token: 0x17000AB0 RID: 2736
	// (get) Token: 0x060045F8 RID: 17912 RVA: 0x0014B5E0 File Offset: 0x001497E0
	// (set) Token: 0x060045F9 RID: 17913 RVA: 0x0014B5E8 File Offset: 0x001497E8
	public bool IsEmpty { get; private set; }

	// Token: 0x17000AB1 RID: 2737
	// (get) Token: 0x060045FA RID: 17914 RVA: 0x0014B5F1 File Offset: 0x001497F1
	// (set) Token: 0x060045FB RID: 17915 RVA: 0x0014B5F9 File Offset: 0x001497F9
	public Func<bool> CanStartResurrection { get; private set; }

	// Token: 0x17000AB2 RID: 2738
	// (get) Token: 0x060045FC RID: 17916 RVA: 0x0014B602 File Offset: 0x00149802
	// (set) Token: 0x060045FD RID: 17917 RVA: 0x0014B60A File Offset: 0x0014980A
	public UICorpseWidgetData CorpseWidgetData { get; private set; }

	// Token: 0x17000AB3 RID: 2739
	// (get) Token: 0x060045FE RID: 17918 RVA: 0x0014B613 File Offset: 0x00149813
	// (set) Token: 0x060045FF RID: 17919 RVA: 0x0014B61B File Offset: 0x0014981B
	public BodyOrgansInventoryWidgetData BodyOrgansInventoryWidgetData { get; private set; }

	// Token: 0x17000AB4 RID: 2740
	// (get) Token: 0x06004600 RID: 17920 RVA: 0x0014B624 File Offset: 0x00149824
	// (set) Token: 0x06004601 RID: 17921 RVA: 0x0014B62C File Offset: 0x0014982C
	public NeedItemsWidgetData NeedItemsWidgetData { get; private set; }

	// Token: 0x17000AB5 RID: 2741
	// (get) Token: 0x06004602 RID: 17922 RVA: 0x0014B635 File Offset: 0x00149835
	// (set) Token: 0x06004603 RID: 17923 RVA: 0x0014B63D File Offset: 0x0014983D
	public WgoData Wgo { get; private set; }

	// Token: 0x17000AB6 RID: 2742
	// (get) Token: 0x06004604 RID: 17924 RVA: 0x0014B646 File Offset: 0x00149846
	// (set) Token: 0x06004605 RID: 17925 RVA: 0x0014B64E File Offset: 0x0014984E
	public UIInfoWidgetData InfoWidgetData { get; set; }

	// Token: 0x17000AB7 RID: 2743
	// (get) Token: 0x06004606 RID: 17926 RVA: 0x0014B657 File Offset: 0x00149857
	// (set) Token: 0x06004607 RID: 17927 RVA: 0x0014B65F File Offset: 0x0014985F
	public string ZombieName { get; set; }

	// Token: 0x17000AB8 RID: 2744
	// (get) Token: 0x06004608 RID: 17928 RVA: 0x0014B668 File Offset: 0x00149868
	// (set) Token: 0x06004609 RID: 17929 RVA: 0x0014B670 File Offset: 0x00149870
	public bool ZombieNameRandomed { get; set; }

	// Token: 0x17000AB9 RID: 2745
	// (get) Token: 0x0600460A RID: 17930 RVA: 0x0014B679 File Offset: 0x00149879
	// (set) Token: 0x0600460B RID: 17931 RVA: 0x0014B681 File Offset: 0x00149881
	public Item Collar { get; set; } = Item.Empty;

	// Token: 0x17000ABA RID: 2746
	// (get) Token: 0x0600460C RID: 17932 RVA: 0x0014B68A File Offset: 0x0014988A
	// (set) Token: 0x0600460D RID: 17933 RVA: 0x0014B692 File Offset: 0x00149892
	public int RedSkulls { get; private set; }

	// Token: 0x17000ABB RID: 2747
	// (get) Token: 0x0600460E RID: 17934 RVA: 0x0014B69B File Offset: 0x0014989B
	// (set) Token: 0x0600460F RID: 17935 RVA: 0x0014B6A3 File Offset: 0x001498A3
	public int WhiteSkulls { get; private set; }

	// Token: 0x17000ABC RID: 2748
	// (get) Token: 0x06004610 RID: 17936 RVA: 0x0014B6AC File Offset: 0x001498AC
	// (set) Token: 0x06004611 RID: 17937 RVA: 0x0014B6B4 File Offset: 0x001498B4
	[TupleElementNames(new string[] { "body", "head", "headLut" })]
	public ValueTuple<int, int, string> RolledSkin
	{
		[return: TupleElementNames(new string[] { "body", "head", "headLut" })]
		get;
		[param: TupleElementNames(new string[] { "body", "head", "headLut" })]
		private set;
	}

	// Token: 0x06004612 RID: 17938 RVA: 0x0014B6C0 File Offset: 0x001498C0
	public UIResurrectionWindowData(WgoData wgoData)
	{
		wgoData.TrySetWorker(MainGame.PlayerController, null);
		this.InfoWidgetData = new UIInfoWidgetData(wgoData, null, false);
		this.Wgo = wgoData;
		foreach (Item item in wgoData.Inventory.Data.Inventory)
		{
			if (item.Definition.itemGroupIds.Contains("body"))
			{
				this.bodyItem = item;
				break;
			}
		}
		CraftDef craftDef = GameBalance.GetCraftDef("corpse_zombie_transition");
		this.craftElementBodyToZombie = new CraftElement(craftDef.id, 1, new List<NeedItemData>(craftDef.needItems), new CraftParamsData(craftDef.id, new GameRes()));
		this.InfoWidgetData.Icon = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(craftDef.GetCraftResultIcon(wgoData), null);
		this.IsEmpty = this.bodyItem == null;
		if (!this.IsEmpty)
		{
			BodyZombieSkinSerializedItemProperty bodyZombieSkinSerializedItemProperty;
			if (this.bodyItem.TryGetProperty<BodyZombieSkinSerializedItemProperty>(out bodyZombieSkinSerializedItemProperty))
			{
				this.RolledSkin = new ValueTuple<int, int, string>(bodyZombieSkinSerializedItemProperty.body, bodyZombieSkinSerializedItemProperty.head, bodyZombieSkinSerializedItemProperty.headLut);
			}
			else
			{
				Debug.LogError(" Skin was not generated for body:[" + this.bodyItem.id + "]. Use default");
				this.RolledSkin = new ValueTuple<int, int, string>(1001, 1050, "hed_lut_01");
			}
		}
		this.BodyOrgansInventoryWidgetData = new BodyOrgansInventoryWidgetData();
		if (!this.IsEmpty)
		{
			this.CorpseWidgetData = new UICorpseWidgetData(this.bodyItem, wgoData, new Action(this.TakeBody), !this.IsEmpty, GameKey.ExtractBody, LLBase.L("ui_grave_corpse_widget_header"), null, LLBase.L("btn_take_body_two_lines"), null);
			Inventory inventory = new Inventory(this.bodyItem);
			this.BodyOrgansInventoryWidgetData = new BodyOrgansInventoryWidgetData(true, wgoData, null, inventory, null, null, null);
			this.onPrepareResurrectionButtonPressed = new Action(this.PrepareResurrection);
			this.onPrepareResurrectionNonInteractableButtonOver = new Action<LazyButton>(this.ShowResurrectionPrepareButtonTooltip);
			this.CanStartResurrection = () => this.CanStartResurrectionWithReason(out this.cantStartResurrectionReason);
			this.NeedItemsWidgetData = new NeedItemsWidgetData(this.craftElementBodyToZombie.Requirements, this.Wgo.GetCraftableMultiInventory(false), true, this.Wgo);
			bool flag;
			this.ZombieName = MainGame.Instance.GameSave.knowledgeSystem.GetZombieName(out flag);
			this.ZombieNameRandomed = flag;
			foreach (Item item2 in this.bodyItem.Inventory)
			{
				this.RedSkulls += item2.Definition.redSkulls * item2.Count;
				this.WhiteSkulls += item2.Definition.whiteSkulls * item2.Count;
			}
			this.RedSkulls = Mathf.Clamp(this.RedSkulls, 0, 999);
			this.WhiteSkulls = Mathf.Clamp(this.WhiteSkulls, 0, 999);
			return;
		}
		this.CorpseWidgetData = new UICorpseWidgetData(GameKey.ExtractBody);
		this.NeedItemsWidgetData = new NeedItemsWidgetData(this.craftElementBodyToZombie.Requirements, this.Wgo.GetCraftableMultiInventory(false), false, this.Wgo);
		this.BodyOrgansInventoryWidgetData = new BodyOrgansInventoryWidgetData();
		this.onPrepareResurrectionButtonPressed = null;
		this.CanStartResurrection = () => false;
		this.onPrepareResurrectionNonInteractableButtonOver = null;
	}

	// Token: 0x06004613 RID: 17939 RVA: 0x0014BA60 File Offset: 0x00149C60
	private void TakeBody()
	{
		if (this.IsEmpty)
		{
			return;
		}
		PlayerData playerData = MainGame.PlayerData;
		if (!playerData.HasFreeOverheadSlot)
		{
			MainGame.Instance.dropSystem.DropItem(this.bodyItem, this.Wgo.WorldId, this.Wgo.Position, null);
		}
		else
		{
			playerData.AddOverheadItem(this.bodyItem);
		}
		this.Wgo.Inventory.RemoveItemFromInventoryByUID(this.bodyItem, -1);
		LazyUI.GetWindow<UIResurrectionWindow>().Close();
	}

	// Token: 0x06004614 RID: 17940 RVA: 0x0014BAE4 File Offset: 0x00149CE4
	public void OnCollarPressed(UIItemCell cell)
	{
		if (this.IsEmpty)
		{
			return;
		}
		LazyWindow<UIMultiInventoryWindowData> window = LazyUI.GetWindow<UIMultiInventoryWindow>();
		UIMultiInventoryWindowData uimultiInventoryWindowData = new UIMultiInventoryWindowData(MainGame.PlayerData, delegate(UIItemCell cell)
		{
			this.Collar = cell.DisplayingItem;
			LazyUI.GetWindow<UIMultiInventoryWindow>().Close();
			Action action = this.onCollarUpdated;
			if (action == null)
			{
				return;
			}
			action();
		}, (Item item) => item != null && !item.IsEmpty && item.Definition.type == ItemType.Collar && item.Definition.SkullsInBorders(this.WhiteSkulls, this.RedSkulls), true, null, null);
		window.Open(uimultiInventoryWindowData);
		this.currentMultiInventory = uimultiInventoryWindowData.MultiInventory;
	}

	// Token: 0x06004615 RID: 17941 RVA: 0x0014BB38 File Offset: 0x00149D38
	private bool CanStartResurrectionWithReason(out string reason)
	{
		reason = "";
		using (List<Item>.Enumerator enumerator = this.Wgo.Inventory.Data.Inventory.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Definition.itemGroupIds.Contains("zombie"))
				{
					return false;
				}
			}
		}
		if (!this.BodyOrgansInventoryWidgetData.HasAllMainOrgans())
		{
			reason = "ui_resurrection_no_organs";
			return false;
		}
		if (!this.Wgo.GetCraftableMultiInventory(false).HasItemsById(this.craftElementBodyToZombie.Requirements, this.Wgo))
		{
			reason = "ui_resurrection_no_liquid";
			return false;
		}
		if (this.Collar.IsEmpty)
		{
			reason = "ui_resurrection_no_collar";
			return false;
		}
		if (!this.Collar.Definition.SkullsInBorders(this.WhiteSkulls, this.RedSkulls))
		{
			reason = "ui_resurrection_insufficient_collar";
			return false;
		}
		return true;
	}

	// Token: 0x06004616 RID: 17942 RVA: 0x0014BC3C File Offset: 0x00149E3C
	private void ShowResurrectionPrepareButtonTooltip(LazyButton prepareButton)
	{
		UITooltip.ShowResurrectionPrepareButtonWidget(prepareButton, this.cantStartResurrectionReason);
	}

	// Token: 0x06004617 RID: 17943 RVA: 0x0014BC4C File Offset: 0x00149E4C
	private void PrepareResurrection()
	{
		if (this.IsEmpty)
		{
			return;
		}
		if (this.Collar == null || this.Collar.IsEmpty)
		{
			this.Collar = new Item("collar_bronze", 1);
		}
		Item item = new Item((this.bodyItem.id == "body_corpse") ? "body_zombie" : "body_corpse", 1);
		BodyZombieStartItemsSerializedItemProperty bodyZombieStartItemsSerializedItemProperty;
		if (this.bodyItem.TryGetProperty<BodyZombieStartItemsSerializedItemProperty>(out bodyZombieStartItemsSerializedItemProperty))
		{
			item.AddProperty<BodyZombieStartItemsSerializedItemProperty>(bodyZombieStartItemsSerializedItemProperty);
		}
		if (this.currentMultiInventory != null)
		{
			this.currentMultiInventory.RemoveItemFromInventoryByUID(this.Collar, 1);
		}
		this.Wgo.GetCraftableMultiInventory(false).RemoveItems(this.craftElementBodyToZombie.Requirements, this.Wgo);
		this.craftElementBodyToZombie.Requirements.Clear();
		for (int i = 0; i < this.bodyItem.Inventory.Count; i++)
		{
			item.AddItemToInventory(this.bodyItem.Inventory[i], false);
		}
		this.Wgo.Inventory.RemoveItemFromInventoryByUID(this.bodyItem, -1);
		this.Wgo.Inventory.AddItemToInventory(item, null, false);
		ZombieWgoData zombieWgoData = MainGame.ZombieSystemData.CreateZombieDrop("zombie", MainGame.PlayerData.position.Value, MainGame.PlayerData.currentGameSceneId, this.Wgo.Inventory.GetItemByGroupId("body"), this.Collar.id, null, false);
		ZombieSkinHelper.ApplySkinToZombieWgoData(zombieWgoData, this.RolledSkin.Item1, this.RolledSkin.Item2, string.Empty, this.RolledSkin.Item3);
		zombieWgoData.SetName(this.ZombieName, this.ZombieNameRandomed);
		this.ZombieName = string.Empty;
		this.Wgo.SetGameRes("resurrection_prepared", 1);
		LazyUI.GetWindow<UIResurrectionWindow>().Close();
	}

	// Token: 0x040036C0 RID: 14016
	public readonly Action onPrepareResurrectionButtonPressed;

	// Token: 0x040036C1 RID: 14017
	public Action onCollarUpdated;

	// Token: 0x040036C2 RID: 14018
	public Action<LazyButton> onPrepareResurrectionNonInteractableButtonOver;

	// Token: 0x040036D0 RID: 14032
	private Item bodyItem;

	// Token: 0x040036D1 RID: 14033
	private CraftElement craftElementBodyToZombie;

	// Token: 0x040036D2 RID: 14034
	private MultiInventory currentMultiInventory;

	// Token: 0x040036D3 RID: 14035
	private string cantStartResurrectionReason;
}
