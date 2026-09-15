using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009B9 RID: 2489
public class UIEmbalmWindow : LazyWindow<UIEmbalmWindowData>
{
	// Token: 0x0600424A RID: 16970 RVA: 0x0013ACF4 File Offset: 0x00138EF4
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

	// Token: 0x0600424B RID: 16971 RVA: 0x0013ADAF File Offset: 0x00138FAF
	private void UpdateCollarSkullsTxt()
	{
		if (this.collarSkullsTxt == null)
		{
			return;
		}
		this.collarSkullsTxt.text = this.GetCollarSkullsText();
	}

	// Token: 0x0600424C RID: 16972 RVA: 0x0013ADD4 File Offset: 0x00138FD4
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

	// Token: 0x0600424D RID: 16973 RVA: 0x0013AEAE File Offset: 0x001390AE
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

	// Token: 0x0600424E RID: 16974 RVA: 0x0013AEDF File Offset: 0x001390DF
	private bool OnTakeBodyButtonPressed()
	{
		this.corpseWidget.OnButtonPressed();
		return true;
	}

	// Token: 0x0600424F RID: 16975 RVA: 0x0013AEED File Offset: 0x001390ED
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.ExtractBody, new Func<bool>(this.OnTakeBodyButtonPressed));
		return gameKeyDelegates;
	}

	// Token: 0x06004250 RID: 16976 RVA: 0x0013AF0C File Offset: 0x0013910C
	[LazyUITest]
	protected override void TestDraw()
	{
		Wgo wgo = Wgo.Spawn(new WgoData("embalm_table_1", MainGame.PlayerController.MovablePosition, MainGame.PlayerData.currentGameSceneId), MainGame.PlayerController.CurrentGameScene.transform, true, false, false, false);
		EmbalmInteractionHandler embalmInteractionHandler = new EmbalmInteractionHandler();
		embalmInteractionHandler.Init(wgo);
		embalmInteractionHandler.HasInteraction(MainGame.PlayerController);
		embalmInteractionHandler.Interact(MainGame.PlayerController);
	}

	// Token: 0x06004251 RID: 16977 RVA: 0x0013AF74 File Offset: 0x00139174
	[LazyUITest]
	protected void TestDrawBody()
	{
		WgoData wgoData = new WgoData("embalm_table_1", MainGame.PlayerController.MovablePosition, MainGame.PlayerData.currentGameSceneId);
		Wgo wgo = Wgo.Spawn(wgoData, MainGame.PlayerController.CurrentGameScene.transform, true, false, false, false);
		EmbalmInteractionHandler embalmInteractionHandler = new EmbalmInteractionHandler();
		embalmInteractionHandler.Init(wgo);
		Item item = GameBalance.Me.GetData<BodyDef>("body_0_2").GenerateItem();
		wgoData.Inventory.AddItemToInventory(item, null, false);
		embalmInteractionHandler.HasInteraction(MainGame.PlayerController);
		embalmInteractionHandler.Interact(MainGame.PlayerController);
	}

	// Token: 0x06004252 RID: 16978 RVA: 0x0013B004 File Offset: 0x00139204
	[LazyUITest]
	protected void TestZombieSmallCollar()
	{
		WgoData wgoData = new WgoData("embalm_table_1", MainGame.PlayerController.MovablePosition, MainGame.PlayerData.currentGameSceneId);
		Wgo wgo = Wgo.Spawn(wgoData, MainGame.PlayerController.CurrentGameScene.transform, true, false, false, false);
		EmbalmInteractionHandler embalmInteractionHandler = new EmbalmInteractionHandler();
		embalmInteractionHandler.Init(wgo);
		Item item = GameBalance.Me.GetData<BodyDef>("body_zombie_test_5").GenerateItem();
		MainGame.ZombieSystemData.CreateZombieDrop("zombie", MainGame.PlayerData.position.Value, MainGame.PlayerData.currentGameSceneId, item, "collar_bronze", null, true);
		wgoData.Inventory.AddItemToInventory(item, null, false);
		embalmInteractionHandler.HasInteraction(MainGame.PlayerController);
		embalmInteractionHandler.Interact(MainGame.PlayerController);
	}

	// Token: 0x06004253 RID: 16979 RVA: 0x0013B0C4 File Offset: 0x001392C4
	[LazyUITest]
	protected void TestZombieBigCollar()
	{
		WgoData wgoData = new WgoData("embalm_table_1", MainGame.PlayerController.MovablePosition, MainGame.PlayerData.currentGameSceneId);
		Wgo wgo = Wgo.Spawn(wgoData, MainGame.PlayerController.CurrentGameScene.transform, true, false, false, false);
		EmbalmInteractionHandler embalmInteractionHandler = new EmbalmInteractionHandler();
		embalmInteractionHandler.Init(wgo);
		Item item = GameBalance.Me.GetData<BodyDef>("body_zombie_test_5").GenerateItem();
		MainGame.ZombieSystemData.CreateZombieDrop("zombie", MainGame.PlayerData.position.Value, MainGame.PlayerData.currentGameSceneId, item, "collar_big", null, true);
		wgoData.Inventory.AddItemToInventory(item, null, false);
		embalmInteractionHandler.HasInteraction(MainGame.PlayerController);
		embalmInteractionHandler.Interact(MainGame.PlayerController);
	}

	// Token: 0x040033B5 RID: 13237
	[SerializeField]
	private UIInfoWidget infoWidget;

	// Token: 0x040033B6 RID: 13238
	[SerializeField]
	private UICorpseWidget corpseWidget;

	// Token: 0x040033B7 RID: 13239
	[SerializeField]
	private BodyOrgansInventoryWidget bodyOrgansInventoryWidget;

	// Token: 0x040033B8 RID: 13240
	[SerializeField]
	private BodyPocketInventoryWidget bodyPocketInventoryWidget;

	// Token: 0x040033B9 RID: 13241
	[SerializeField]
	private TMP_Text collarSkullsTxt;

	// Token: 0x040033BA RID: 13242
	private Action onTakeBodyButtonPressed;
}
