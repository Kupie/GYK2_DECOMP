using System;

// Token: 0x020004B6 RID: 1206
[Serializable]
public class StaminaSystem
{
	// Token: 0x1400005B RID: 91
	// (add) Token: 0x06002018 RID: 8216 RVA: 0x00098084 File Offset: 0x00096284
	// (remove) Token: 0x06002019 RID: 8217 RVA: 0x000980BC File Offset: 0x000962BC
	public event Action OnNotEnoughStamina;

	// Token: 0x17000552 RID: 1362
	// (get) Token: 0x0600201A RID: 8218 RVA: 0x000980F1 File Offset: 0x000962F1
	private float RegenerationDelay
	{
		get
		{
			if (this.regenerationDelay <= 0f)
			{
				this.regenerationDelay = GameBalance.Me.GetData<ConstDef>("stamina_regeneration_delay").FloatValue;
			}
			return this.regenerationDelay;
		}
	}

	// Token: 0x17000553 RID: 1363
	// (get) Token: 0x0600201B RID: 8219 RVA: 0x00098120 File Offset: 0x00096320
	private float Regeneration
	{
		get
		{
			if (this.regeneration <= 0f)
			{
				this.regeneration = GameBalance.Me.GetData<ConstDef>("stamina_regeneration").FloatValue;
			}
			return this.regeneration;
		}
	}

	// Token: 0x17000554 RID: 1364
	// (get) Token: 0x0600201C RID: 8220 RVA: 0x0009814F File Offset: 0x0009634F
	private float RegenerationStance
	{
		get
		{
			if (this.regenerationStance <= 0f)
			{
				this.regenerationStance = GameBalance.Me.GetData<ConstDef>("stamina_regeneration_stance").FloatValue;
			}
			return this.regenerationStance;
		}
	}

	// Token: 0x0600201D RID: 8221 RVA: 0x00098180 File Offset: 0x00096380
	public bool CanPerformAttack()
	{
		PlayerController playerController = MainGame.PlayerController;
		if (playerController == null || playerController.AttackComponent == null || !playerController.AttackComponent.HasEquippedWeapon)
		{
			return false;
		}
		if (!PlayerStaminaGameResSystem.GetSystem().CanChangeStamina((float)(-(float)playerController.AttackComponent.weapon.ItemDef.staminaCost.EvaluateInt())))
		{
			Action onNotEnoughStamina = this.OnNotEnoughStamina;
			if (onNotEnoughStamina != null)
			{
				onNotEnoughStamina();
			}
			return false;
		}
		return true;
	}

	// Token: 0x0600201E RID: 8222 RVA: 0x000981F8 File Offset: 0x000963F8
	public void ConsumeStamina(bool startRegenerationDelay = true)
	{
		PlayerController playerController = MainGame.PlayerController;
		if (playerController == null || playerController.AttackComponent == null || !playerController.AttackComponent.HasEquippedWeapon)
		{
			return;
		}
		PlayerStaminaGameResSystem.GetSystem().Add((float)(-(float)playerController.AttackComponent.weapon.ItemDef.staminaCost.EvaluateInt()), false);
		if (startRegenerationDelay)
		{
			this.isStaminaRegenSuspended = false;
			this.isRegenerationDelayed = true;
			this.regenerationDelayTimer = 0f;
			return;
		}
		this.isStaminaRegenSuspended = true;
	}

	// Token: 0x0600201F RID: 8223 RVA: 0x0009827B File Offset: 0x0009647B
	public void BeginRegenerationDelayIfSuspended()
	{
		if (!this.isStaminaRegenSuspended)
		{
			return;
		}
		this.isStaminaRegenSuspended = false;
		this.isRegenerationDelayed = true;
		this.regenerationDelayTimer = 0f;
	}

	// Token: 0x06002020 RID: 8224 RVA: 0x000982A0 File Offset: 0x000964A0
	public void UpdateStaminaLogic(float deltaTime)
	{
		if (this.isStaminaRegenSuspended)
		{
			return;
		}
		if (this.isRegenerationDelayed)
		{
			this.regenerationDelayTimer += deltaTime;
			if (this.regenerationDelayTimer < this.RegenerationDelay)
			{
				return;
			}
			this.regenerationDelayTimer = 0f;
			this.isRegenerationDelayed = false;
		}
		if (PlayerStaminaGameResSystem.GetSystem().HasMax())
		{
			return;
		}
		float num;
		if (MainGame.PlayerController.Ssm.CurState is IStaminaConsumer)
		{
			num = this.RegenerationStance;
		}
		else
		{
			num = this.Regeneration;
		}
		PlayerStaminaGameResSystem.GetSystem().Add(num * deltaTime, false);
	}

	// Token: 0x06002021 RID: 8225 RVA: 0x00098334 File Offset: 0x00096534
	public void SetMax()
	{
		PlayerStaminaGameResSystem system = PlayerStaminaGameResSystem.GetSystem();
		system.Set(system.Max, false);
	}

	// Token: 0x04001CC6 RID: 7366
	public float regenerationDelayTimer;

	// Token: 0x04001CC7 RID: 7367
	public bool isRegenerationDelayed;

	// Token: 0x04001CC8 RID: 7368
	private float regenerationDelay = -1f;

	// Token: 0x04001CC9 RID: 7369
	private float regeneration = -1f;

	// Token: 0x04001CCA RID: 7370
	private float regenerationStance = -1f;

	// Token: 0x04001CCB RID: 7371
	private bool isStaminaRegenSuspended;
}
