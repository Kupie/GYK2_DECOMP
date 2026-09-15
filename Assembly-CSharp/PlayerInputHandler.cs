using System;
using LazyBearTechnology;

// Token: 0x02000392 RID: 914
public class PlayerInputHandler
{
	// Token: 0x1700042B RID: 1067
	// (get) Token: 0x0600187C RID: 6268 RVA: 0x0007383B File Offset: 0x00071A3B
	private PlayerData PlayerData
	{
		get
		{
			return this.playerController.PlayerData;
		}
	}

	// Token: 0x0600187D RID: 6269 RVA: 0x00073848 File Offset: 0x00071A48
	public PlayerInputHandler(PlayerController playerController)
	{
		this.playerController = playerController;
		this.interactionComponent = playerController.PlayerInteractionComponent;
		this.mouseCombatHandler = new PlayerMouseCombatHandler(playerController);
	}

	// Token: 0x0600187E RID: 6270 RVA: 0x0007387D File Offset: 0x00071A7D
	public static bool IsAttackDown()
	{
		return LazyInput.GetKeyDown(GameKey.Attack) || (LazyInput.GetKeyDown(GameKey.LeftClick) && !MouseAimHelper.IsPointerOverUI());
	}

	// Token: 0x0600187F RID: 6271 RVA: 0x000738A3 File Offset: 0x00071AA3
	public static bool IsAttackFocusHeld()
	{
		return LazyInput.GetKey(GameKey.AttackFocus) || LazyInput.GetKey(GameKey.RightClick);
	}

	// Token: 0x06001880 RID: 6272 RVA: 0x000738C0 File Offset: 0x00071AC0
	private bool TryCallFireEvent(PlayerInputHandler.InteractionType interactionType)
	{
		bool flag = this.curInteractionHandler.HasInteraction() || this.curInteractionHandler.HasInteraction2();
		if (this.interactionComponent.WgoUnderInteraction.Data.Events.Count > 0)
		{
			if (!flag)
			{
				return this.interactionComponent.WgoUnderInteraction.Data.FireInteractionEvent();
			}
			if (interactionType != PlayerInputHandler.InteractionType.Interaction1)
			{
				if (interactionType == PlayerInputHandler.InteractionType.Interaction2)
				{
					if (this.curInteractionHandler.HasInteraction2())
					{
						return this.interactionComponent.WgoUnderInteraction.Data.FireInteractionEvent();
					}
				}
			}
			else if (this.curInteractionHandler.HasInteraction())
			{
				return this.interactionComponent.WgoUnderInteraction.Data.FireInteractionEvent();
			}
		}
		return false;
	}

	// Token: 0x06001881 RID: 6273 RVA: 0x00073974 File Offset: 0x00071B74
	public void UpdateInput()
	{
		if (LazyInput.GetKeyDown(GameKey.Interaction))
		{
			if (this.interactionComponent.BigDropUnderInteraction != null && this.interactionComponent.BigDropUnderInteraction.InteractionHandler.HasInteraction())
			{
				this.interactionComponent.BigDropUnderInteraction.InteractionHandler.Interact();
				return;
			}
			if (this.interactionComponent.WgoUnderInteraction != null)
			{
				this.curInteractionHandler = this.interactionComponent.WgoUnderInteraction.InteractionHandler;
				if (this.TryCallFireEvent(PlayerInputHandler.InteractionType.Interaction1))
				{
					return;
				}
				if (this.curInteractionHandler.HasInteraction() && this.curInteractionHandler.Interact(this.playerController))
				{
					return;
				}
			}
			if (this.PlayerData.HasOverheadItem)
			{
				this.PlayerData.DropOverheadItem();
				return;
			}
			return;
		}
		else
		{
			if (LazyInput.GetKeyDown(GameKey.Action))
			{
				if (this.interactionComponent.BigDropUnderInteraction != null && this.interactionComponent.BigDropUnderInteraction.InteractionHandler.HasInteraction2())
				{
					this.interactionComponent.BigDropUnderInteraction.InteractionHandler.Interact2();
					return;
				}
				if (this.interactionComponent.WgoUnderInteraction != null)
				{
					this.curInteractionHandler = this.interactionComponent.WgoUnderInteraction.InteractionHandler;
					if (this.TryCallFireEvent(PlayerInputHandler.InteractionType.Interaction2))
					{
						return;
					}
					if (this.curInteractionHandler.HasInteraction2())
					{
						this.curInteractionHandler.Interact2(this.playerController);
						return;
					}
				}
			}
			if (LazyInput.GetKeyDown(GameKey.InGameMenu))
			{
				LazyUI.GetWindow<UIGamePauseWindow>().Open(null);
				return;
			}
			if (LazyInput.GetKeyDown(GameKey.CharacterWindow))
			{
				this.OpenCharacterWindow(this.PlayerData.lastOpenedPage);
				return;
			}
			if (LazyInput.GetKeyDown(GameKey.Inventory))
			{
				this.OpenCharacterWindow(CharacterWindowData.CharPage.Main);
				return;
			}
			if (LazyInput.GetKeyDown(GameKey.TechTree) && !MainGame.Instance.GameSave.knowledgeSystem.IsCharTabLocked(CharacterWindowData.CharPage.TechTree))
			{
				this.OpenCharacterWindow(CharacterWindowData.CharPage.TechTree);
				return;
			}
			if (LazyInput.GetKeyDown(GameKey.QuestTree) && !MainGame.Instance.GameSave.knowledgeSystem.IsCharTabLocked(CharacterWindowData.CharPage.QuestTree))
			{
				this.OpenCharacterWindow(CharacterWindowData.CharPage.QuestTree);
				return;
			}
			if (LazyInput.GetKeyDown(GameKey.Map) && !MainGame.Instance.GameSave.knowledgeSystem.IsCharTabLocked(CharacterWindowData.CharPage.Map))
			{
				this.OpenCharacterWindow(CharacterWindowData.CharPage.Map);
				return;
			}
			if (LazyInput.GetKeyDown(GameKey.Inspirations) && (!MainGame.Instance.GameSave.knowledgeSystem.IsCharTabLocked(CharacterWindowData.CharPage.Inspiration) || this.HasPendingInspirationTutorial()))
			{
				this.OpenCharacterWindow(CharacterWindowData.CharPage.Inspiration);
				return;
			}
			if (this.playerController.IsControlsEnabled && this.playerController.AttackComponent.HasEquippedWeapon && !this.playerController.AttackComponent.IsRangedWeapon)
			{
				if (LazyInput.GetKey(GameKey.AttackFocus) && this.meleeMode == MeleeMode.WithFocus)
				{
					this.playerController.Ssm.ForceEnterState<AttackSwordFocusedPlayerState>();
				}
				if (LazyInput.GetKeyDown(GameKey.Attack) && !(this.playerController.Ssm.CurState is AttackSwordFocusedPlayerState) && MainGame.PlayerData.staminaSystem.CanPerformAttack())
				{
					switch (this.meleeMode)
					{
					case MeleeMode.WithStop:
						this.playerController.Ssm.ForceEnterState<AttackSwordPlayerState>();
						return;
					case MeleeMode.WithMove:
					case MeleeMode.WithFocus:
						this.playerController.Ssm.ForceEnterState<AttackSwordDefaultPlayerState>();
						return;
					case MeleeMode.Continuous:
						this.playerController.Ssm.ForceEnterState<AttackSwordContinuousPlayerState>();
						return;
					default:
						return;
					}
				}
			}
			if (this.playerController.IsControlsEnabled && this.playerController.AttackComponent.HasEquippedWeapon && this.playerController.AttackComponent.IsRangedWeapon)
			{
				if (LazyInput.GetKey(GameKey.AttackFocus) && this.rangedMode == RangedMode.WithFocus)
				{
					this.playerController.Ssm.ForceEnterState<AttackBowFocusedPlayerState>();
				}
				if (LazyInput.GetKeyDown(GameKey.Attack) && !(this.playerController.Ssm.CurState is AttackBowFocusedPlayerState) && MainGame.PlayerData.staminaSystem.CanPerformAttack())
				{
					RangedMode rangedMode = this.rangedMode;
					if (rangedMode > RangedMode.WithFocus)
					{
						if (rangedMode == RangedMode.WithAim)
						{
							this.playerController.Ssm.ForceEnterState<AttackBowAutoPlayerState>();
						}
					}
					else
					{
						this.playerController.Ssm.ForceEnterState<AttackBowDefaultPlayerState>();
					}
				}
			}
			this.mouseCombatHandler.Update();
			this.UpdateHotBarInteraction();
			return;
		}
	}

	// Token: 0x06001882 RID: 6274 RVA: 0x00073D9C File Offset: 0x00071F9C
	public void UpdateInputInteractionOnly()
	{
		if (!LazyInput.GetKeyDown(GameKey.Interaction))
		{
			this.UpdateHotBarInteraction();
			return;
		}
		if (this.interactionComponent.BigDropUnderInteraction != null && this.interactionComponent.BigDropUnderInteraction.InteractionHandler.HasInteraction())
		{
			this.interactionComponent.BigDropUnderInteraction.InteractionHandler.Interact();
			return;
		}
		if (this.interactionComponent.WgoUnderInteraction != null)
		{
			this.curInteractionHandler = this.interactionComponent.WgoUnderInteraction.InteractionHandler;
			if (this.curInteractionHandler.HasInteraction())
			{
				this.curInteractionHandler.Interact(this.playerController);
				return;
			}
		}
		if (this.PlayerData.HasOverheadItem)
		{
			this.PlayerData.DropOverheadItem();
			return;
		}
	}

	// Token: 0x06001883 RID: 6275 RVA: 0x00073E64 File Offset: 0x00072064
	public bool UpdateHotBarInteraction()
	{
		if (LazyInput.GetKeyDown(GameKey.UseHotBarItem1))
		{
			this.PlayerData.TryUseHotBarItem(this.PlayerData.pinnedItems[0]);
			return true;
		}
		if (LazyInput.GetKeyDown(GameKey.UseHotBarItem2))
		{
			this.PlayerData.TryUseHotBarItem(this.PlayerData.pinnedItems[1]);
			return true;
		}
		if (LazyInput.GetKeyDown(GameKey.UseHotBarItem3))
		{
			this.PlayerData.TryUseHotBarItem(this.PlayerData.pinnedItems[2]);
			return true;
		}
		if (LazyInput.GetKeyDown(GameKey.UseHotBarItem4))
		{
			this.PlayerData.TryUseHotBarItem(this.PlayerData.pinnedItems[3]);
			return true;
		}
		return false;
	}

	// Token: 0x06001884 RID: 6276 RVA: 0x00073F0C File Offset: 0x0007210C
	private void OpenCharacterWindow(CharacterWindowData.CharPage page = CharacterWindowData.CharPage.Main)
	{
		TalentSystemData talentSystemData = MainGame.Instance.GameSave.talentSystemData;
		string text;
		if (!MainGame.PlayerData.sawInspirationTutorialOnce && talentSystemData.TryGetTalentIdWithTwoZeroFaithInspirationsToBuy(out text))
		{
			MainGame.Instance.GameSave.knowledgeSystem.TryUnlockInspirationTab();
			LazyUI.GetWindow<CharacterWindow>().Open(new CharacterWindowData(MainGame.Instance.GameSave, CharacterWindowData.CharPage.Inspiration, text));
			return;
		}
		LazyUI.GetWindow<CharacterWindow>().Open(new CharacterWindowData(MainGame.Instance.GameSave, page, null));
	}

	// Token: 0x06001885 RID: 6277 RVA: 0x00073F8A File Offset: 0x0007218A
	private bool HasPendingInspirationTutorial()
	{
		return !MainGame.PlayerData.sawInspirationTutorialOnce && MainGame.Instance.GameSave.talentSystemData.HasTwoZeroFaithInspirationsToBuyInSameBranch();
	}

	// Token: 0x040017F4 RID: 6132
	private PlayerController playerController;

	// Token: 0x040017F5 RID: 6133
	private PlayerInteractionComponent interactionComponent;

	// Token: 0x040017F6 RID: 6134
	private IWGOInteractionHandler curInteractionHandler;

	// Token: 0x040017F7 RID: 6135
	private PlayerMouseCombatHandler mouseCombatHandler;

	// Token: 0x040017F8 RID: 6136
	public MeleeMode meleeMode = MeleeMode.WithFocus;

	// Token: 0x040017F9 RID: 6137
	public RangedMode rangedMode = RangedMode.WithFocus;

	// Token: 0x02000393 RID: 915
	private enum InteractionType
	{
		// Token: 0x040017FB RID: 6139
		Interaction1,
		// Token: 0x040017FC RID: 6140
		Interaction2
	}
}
