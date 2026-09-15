using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008F3 RID: 2291
public class UIAutopsyWindow : LazyWindow<UIAutopsyWindowData>
{
	// Token: 0x06003BE9 RID: 15337 RVA: 0x0011E04C File Offset: 0x0011C24C
	public override void Redraw()
	{
		base.Redraw();
		this.corpseWidget.Draw(this.data.CorpseWidgetData);
		this.bodyPocketInventoryWidget.Draw(this.data.BodyPocketInventoryWidgetData);
		this.bodyOrgansInventoryWidget.Draw(this.data.BodyOrgansInventoryWidgetData);
		this.infoWidget.Draw(this.data.InfoWidgetData);
		this.UpdateCollarSkullsTxt();
		if (LazyInput.IsGamepadActive)
		{
			if (this.data.IsEmpty)
			{
				base.GamepadNavigationController.Disable();
			}
			else
			{
				base.GamepadNavigationController.Enable();
				base.GamepadNavigationController.ReinitItems(true, null, null);
			}
		}
		((RectTransform)base.transform).RefreshContentFitter();
	}

	// Token: 0x06003BEA RID: 15338 RVA: 0x0011E107 File Offset: 0x0011C307
	private void UpdateCollarSkullsTxt()
	{
		if (this.collarSkullsTxt == null)
		{
			return;
		}
		this.collarSkullsTxt.text = this.GetCollarSkullsText();
	}

	// Token: 0x06003BEB RID: 15339 RVA: 0x0011E12C File Offset: 0x0011C32C
	private string GetCollarSkullsText()
	{
		if (this.data == null || this.data.IsEmpty || this.data.CorpseWidgetData == null || !this.data.CorpseWidgetData.IsZombie)
		{
			return string.Empty;
		}
		ZombieWgoData zombieWgoData = this.data.CorpseWidgetData.ZombieWgoData;
		Item item = ((zombieWgoData != null) ? zombieWgoData.Collar : null);
		if (item == null || item.IsEmpty)
		{
			return string.Empty;
		}
		ItemDef definition = item.Definition;
		if (definition.redSkullsMaxCollar <= definition.redSkullsMinCollar)
		{
			return string.Empty;
		}
		string text = string.Format("{0}{1}-{2}", "rskull".FontIcon(), definition.redSkullsMinCollar, definition.redSkullsMaxCollar);
		return LLBase.L("zombie_collar") + LLBase.L("colon_symbol") + " " + text;
	}

	// Token: 0x06003BEC RID: 15340 RVA: 0x0011E206 File Offset: 0x0011C406
	public override void Hide()
	{
		base.Hide();
		if (this.data != null)
		{
			this.corpseWidget.Hide();
			this.bodyPocketInventoryWidget.Hide();
			this.bodyOrgansInventoryWidget.Hide();
		}
	}

	// Token: 0x06003BED RID: 15341 RVA: 0x0011E237 File Offset: 0x0011C437
	private bool OnTakeBodyButtonPressed()
	{
		this.corpseWidget.OnButtonPressed();
		return true;
	}

	// Token: 0x06003BEE RID: 15342 RVA: 0x0011E245 File Offset: 0x0011C445
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.ExtractBody, new Func<bool>(this.OnTakeBodyButtonPressed));
		return gameKeyDelegates;
	}

	// Token: 0x06003BEF RID: 15343 RVA: 0x0011E264 File Offset: 0x0011C464
	[LazyUITest]
	protected override void TestDraw()
	{
		Wgo wgo = Wgo.Spawn(new WgoData("autopsy_table_1", MainGame.PlayerController.MovablePosition, MainGame.PlayerData.currentGameSceneId), MainGame.PlayerController.CurrentGameScene.transform, true, false, false, false);
		AutopsyInteractionHandler autopsyInteractionHandler = new AutopsyInteractionHandler();
		autopsyInteractionHandler.Init(wgo);
		autopsyInteractionHandler.HasInteraction(MainGame.PlayerController);
		autopsyInteractionHandler.Interact(MainGame.PlayerController);
	}

	// Token: 0x06003BF0 RID: 15344 RVA: 0x0011E2CC File Offset: 0x0011C4CC
	[LazyUITest]
	protected void TestDrawBody()
	{
		WgoData wgoData = new WgoData("autopsy_table_1", MainGame.PlayerController.MovablePosition, MainGame.PlayerData.currentGameSceneId);
		Wgo wgo = Wgo.Spawn(wgoData, MainGame.PlayerController.CurrentGameScene.transform, true, false, false, false);
		AutopsyInteractionHandler autopsyInteractionHandler = new AutopsyInteractionHandler();
		autopsyInteractionHandler.Init(wgo);
		Item item = GameBalance.Me.GetData<BodyDef>("body_0_2").GenerateItem();
		wgoData.Inventory.AddItemToInventory(item, null, false);
		autopsyInteractionHandler.HasInteraction(MainGame.PlayerController);
		autopsyInteractionHandler.Interact(MainGame.PlayerController);
	}

	// Token: 0x06003BF1 RID: 15345 RVA: 0x0011E35C File Offset: 0x0011C55C
	[LazyUITest]
	protected void TestZombieSmallCollar()
	{
		WgoData wgoData = new WgoData("autopsy_table_1", MainGame.PlayerController.MovablePosition, MainGame.PlayerData.currentGameSceneId);
		Wgo wgo = Wgo.Spawn(wgoData, MainGame.PlayerController.CurrentGameScene.transform, true, false, false, false);
		AutopsyInteractionHandler autopsyInteractionHandler = new AutopsyInteractionHandler();
		autopsyInteractionHandler.Init(wgo);
		Item item = GameBalance.Me.GetData<BodyDef>("body_zombie_test_5").GenerateItem();
		MainGame.ZombieSystemData.CreateZombieDrop("zombie", MainGame.PlayerData.position.Value, MainGame.PlayerData.currentGameSceneId, item, "collar_bronze", null, true);
		wgoData.Inventory.AddItemToInventory(item, null, false);
		autopsyInteractionHandler.HasInteraction(MainGame.PlayerController);
		autopsyInteractionHandler.Interact(MainGame.PlayerController);
	}

	// Token: 0x06003BF2 RID: 15346 RVA: 0x0011E41C File Offset: 0x0011C61C
	[LazyUITest]
	protected void TestZombieBigCollar()
	{
		WgoData wgoData = new WgoData("autopsy_table_1", MainGame.PlayerController.MovablePosition, MainGame.PlayerData.currentGameSceneId);
		Wgo wgo = Wgo.Spawn(wgoData, MainGame.PlayerController.CurrentGameScene.transform, true, false, false, false);
		AutopsyInteractionHandler autopsyInteractionHandler = new AutopsyInteractionHandler();
		autopsyInteractionHandler.Init(wgo);
		Item item = GameBalance.Me.GetData<BodyDef>("body_zombie_test_5").GenerateItem();
		MainGame.ZombieSystemData.CreateZombieDrop("zombie", MainGame.PlayerData.position.Value, MainGame.PlayerData.currentGameSceneId, item, "collar_big", null, true);
		wgoData.Inventory.AddItemToInventory(item, null, false);
		autopsyInteractionHandler.HasInteraction(MainGame.PlayerController);
		autopsyInteractionHandler.Interact(MainGame.PlayerController);
	}

	// Token: 0x04002F2C RID: 12076
	[SerializeField]
	private UIInfoWidget infoWidget;

	// Token: 0x04002F2D RID: 12077
	[SerializeField]
	private UICorpseWidget corpseWidget;

	// Token: 0x04002F2E RID: 12078
	[SerializeField]
	private BodyOrgansInventoryWidget bodyOrgansInventoryWidget;

	// Token: 0x04002F2F RID: 12079
	[SerializeField]
	private BodyPocketInventoryWidget bodyPocketInventoryWidget;

	// Token: 0x04002F30 RID: 12080
	[SerializeField]
	private TMP_Text collarSkullsTxt;

	// Token: 0x04002F31 RID: 12081
	private Action onTakeBodyButtonPressed;
}
