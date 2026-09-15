using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020002A8 RID: 680
public class AttackComponent : MonoBehaviour
{
	// Token: 0x170002CE RID: 718
	// (get) Token: 0x06001155 RID: 4437 RVA: 0x0005799D File Offset: 0x00055B9D
	public bool IsRangedWeapon
	{
		get
		{
			return this.weapon is BowWeapon;
		}
	}

	// Token: 0x170002CF RID: 719
	// (get) Token: 0x06001156 RID: 4438 RVA: 0x000579AD File Offset: 0x00055BAD
	public bool HasEquippedWeapon
	{
		get
		{
			return this.weapon;
		}
	}

	// Token: 0x170002D0 RID: 720
	// (get) Token: 0x06001157 RID: 4439 RVA: 0x000579BA File Offset: 0x00055BBA
	public int Armor
	{
		get
		{
			return this.fighterDefProvider.Armor;
		}
	}

	// Token: 0x170002D1 RID: 721
	// (get) Token: 0x06001158 RID: 4440 RVA: 0x000579C7 File Offset: 0x00055BC7
	// (set) Token: 0x06001159 RID: 4441 RVA: 0x000579CF File Offset: 0x00055BCF
	public bool IsInitialized { get; private set; }

	// Token: 0x0600115A RID: 4442 RVA: 0x000579D8 File Offset: 0x00055BD8
	public void Init(ICombatEntity combatEntity, FighterDef fighterDef = null, List<Weapon> availableWeapons = null)
	{
		if (combatEntity == null)
		{
			Debug.LogError("[AttackComponent]: Attempting to initialize AttackComponent with null combatEntity", this);
			return;
		}
		this.combatEntity = combatEntity;
		if (this.IsInitialized)
		{
			return;
		}
		this.IsInitialized = true;
		this.fighterDefProvider = new AttackComponent.FighterDefProvider(fighterDef, this);
		if (availableWeapons != null)
		{
			this.availableWeapons = availableWeapons;
		}
	}

	// Token: 0x0600115B RID: 4443 RVA: 0x00057A18 File Offset: 0x00055C18
	public void EquipWeapon(ItemDef itemDef)
	{
		Weapon weapon = this.availableWeapons.Find((Weapon w) => w.ItemType == itemDef.type);
		if (!weapon)
		{
			Debug.LogError(string.Format("No suitable weapon for {0} was found", itemDef));
			return;
		}
		this.EquipWeapon(weapon);
		this.weapon.ItemDef = itemDef;
	}

	// Token: 0x0600115C RID: 4444 RVA: 0x00057A80 File Offset: 0x00055C80
	public void EquipWeapon(ItemType itemType)
	{
		Weapon weapon = this.availableWeapons.Find((Weapon w) => w.ItemType == itemType);
		if (!weapon)
		{
			Debug.LogError(string.Format("No suitable weapon for {0} was found", itemType));
			return;
		}
		this.EquipWeapon(weapon);
	}

	// Token: 0x0600115D RID: 4445 RVA: 0x00057ADC File Offset: 0x00055CDC
	public void EquipWeapon(Weapon weapon)
	{
		if (this.weapon)
		{
			this.UnequipWeapon();
		}
		if (!weapon)
		{
			return;
		}
		this.weapon = weapon;
		weapon.gameObject.SetActive(true);
		Action onWeaponChanged = this.OnWeaponChanged;
		if (onWeaponChanged != null)
		{
			onWeaponChanged();
		}
		this.SubscribeToEvents();
	}

	// Token: 0x0600115E RID: 4446 RVA: 0x00057B2F File Offset: 0x00055D2F
	public void UnequipWeapon()
	{
		if (!this.weapon)
		{
			return;
		}
		this.weapon.gameObject.SetActive(false);
		this.UnsubscribeFromEvents();
		this.weapon = null;
		Action onWeaponChanged = this.OnWeaponChanged;
		if (onWeaponChanged == null)
		{
			return;
		}
		onWeaponChanged();
	}

	// Token: 0x0600115F RID: 4447 RVA: 0x00057B6D File Offset: 0x00055D6D
	public void PerformAttack(bool useCustomDirection = false, Vector3 customDirection = default(Vector3), bool useAnimationFromWeapon = true, Action onAnimationFinished = null, bool activateWeapon = true)
	{
		this.PerformAttack(useCustomDirection, customDirection, useAnimationFromWeapon, onAnimationFinished, null, activateWeapon);
	}

	// Token: 0x06001160 RID: 4448 RVA: 0x00057B7D File Offset: 0x00055D7D
	public void PerformAttackByTrigger(bool useCustomDirection = false, Vector3 customDirection = default(Vector3), string customTrigger = null, Action onAnimationFinished = null, bool activateWeapon = true)
	{
		this.PerformAttack(useCustomDirection, customDirection, false, onAnimationFinished, customTrigger, activateWeapon);
	}

	// Token: 0x06001161 RID: 4449 RVA: 0x00057B8D File Offset: 0x00055D8D
	public void PerformAttackMelee(Action onAnimationFinished = null)
	{
		this.PerformAttackMeleeCustom(onAnimationFinished);
	}

	// Token: 0x06001162 RID: 4450 RVA: 0x00057B96 File Offset: 0x00055D96
	public void CancelAttack()
	{
		this.OnAnimationFinished = null;
		this.OnAttackAnimFinished(this.animationComponent);
	}

	// Token: 0x06001163 RID: 4451 RVA: 0x00057BAB File Offset: 0x00055DAB
	public void DetachAnimationFinishedCallback()
	{
		this.OnAnimationFinished = null;
	}

	// Token: 0x06001164 RID: 4452 RVA: 0x00057BB4 File Offset: 0x00055DB4
	public void ActivateWeapon(Vector3 direction)
	{
		if (this.fighterDefProvider == null)
		{
			return;
		}
		AttackContext ctx = this.fighterDefProvider.GetAttackContext(direction);
		this.weapon.Dealers.ForEach(delegate(IDamageDealer dealer)
		{
			dealer.Activate(ctx);
		});
	}

	// Token: 0x06001165 RID: 4453 RVA: 0x00057C00 File Offset: 0x00055E00
	public void OnAttackAnimFinished(AnimationComponentBase animationComponent)
	{
		if (this.animationComponent == animationComponent)
		{
			if (this.useAnimation && AttackComponent.IsAttackAnimationState(animationComponent.AnimationState))
			{
				animationComponent.SetState(global::AnimationState.Idle);
			}
			Action onAnimationFinished = this.OnAnimationFinished;
			if (onAnimationFinished != null)
			{
				onAnimationFinished();
			}
		}
		if (animationComponent.Animator.layerCount > 6)
		{
			animationComponent.SetLayerWeight(6, 0f);
		}
	}

	// Token: 0x06001166 RID: 4454 RVA: 0x00057C62 File Offset: 0x00055E62
	private static bool IsAttackAnimationState(global::AnimationState state)
	{
		return state == global::AnimationState.AttackMelee || state == global::AnimationState.AttackCommon || state == global::AnimationState.AttackSpear || state == global::AnimationState.AttackBow;
	}

	// Token: 0x06001167 RID: 4455 RVA: 0x00057C7A File Offset: 0x00055E7A
	public void DoHit(ICombatEntity target, AttackContext ctx)
	{
		this.HandleHit(target, ctx);
	}

	// Token: 0x06001168 RID: 4456 RVA: 0x00057C84 File Offset: 0x00055E84
	private void PerformAttack(bool useCustomDirection = false, Vector3 customDirection = default(Vector3), bool useAnimationFromWeapon = true, Action onAnimationFinished = null, string customTrigger = null, bool activateWeapon = true)
	{
		if (!this.weapon)
		{
			return;
		}
		if (onAnimationFinished != null)
		{
			this.OnAnimationFinished = onAnimationFinished;
		}
		this.useAnimation = useAnimationFromWeapon;
		if (useAnimationFromWeapon)
		{
			AnimationComponent animationComponent = this.animationComponent;
			if (animationComponent != null)
			{
				animationComponent.SetState(this.weapon.animState);
			}
		}
		else if (customTrigger != null)
		{
			AnimationComponent animationComponent2 = this.animationComponent;
			if (animationComponent2 != null)
			{
				animationComponent2.SetTrigger(customTrigger);
			}
		}
		AnimationComponent animationComponent3 = this.animationComponent;
		if (animationComponent3 != null)
		{
			animationComponent3.SetLayerWeight(6, 1f);
		}
		if (activateWeapon)
		{
			Vector3 vector;
			if (useCustomDirection)
			{
				vector = customDirection;
			}
			else
			{
				AnimationComponent animationComponent4 = this.animationComponent;
				vector = ((animationComponent4 != null) ? animationComponent4.GetDirection().XZ() : Vector3.zero);
			}
			this.ActivateWeapon(vector);
		}
	}

	// Token: 0x06001169 RID: 4457 RVA: 0x00057D2C File Offset: 0x00055F2C
	private void PerformAttackMeleeCustom(Action onAnimationFinished = null)
	{
		if (onAnimationFinished != null)
		{
			this.OnAnimationFinished = onAnimationFinished;
		}
		AnimationComponent animationComponent = this.animationComponent;
		if (animationComponent == null)
		{
			return;
		}
		animationComponent.SetState(global::AnimationState.AttackMelee);
	}

	// Token: 0x0600116A RID: 4458 RVA: 0x00057D4A File Offset: 0x00055F4A
	private void OnEnable()
	{
		if (this.weapon)
		{
			this.EquipWeapon(this.weapon);
		}
	}

	// Token: 0x0600116B RID: 4459 RVA: 0x00057D65 File Offset: 0x00055F65
	private void OnDestroy()
	{
		this.OnWeaponChanged = null;
		this.UnsubscribeFromEvents();
	}

	// Token: 0x0600116C RID: 4460 RVA: 0x00057D74 File Offset: 0x00055F74
	private void HandleHit(ICombatEntity target, AttackContext attackContext)
	{
		int num = attackContext.Damage;
		num = Mathf.Clamp(num - target.ArmorValue, 0, int.MaxValue);
		if (target.IsActiveCombatant)
		{
			target.CombatEntityHpComponent.ApplyDamage(num);
			IKnockbackable knockbackable = target as IKnockbackable;
			if (knockbackable != null && this.fighterDefProvider != null)
			{
				knockbackable.ApplyKnockback(attackContext, this.fighterDefProvider.HasKnockback ? this.fighterDefProvider.KnockbackForce : 0f);
			}
		}
		if (num != 0)
		{
			target.OnOtherCombatTargetHitMe(attackContext);
		}
		if (attackContext.Damage > 0)
		{
			if (attackContext.weaponDef != null)
			{
				string text = "";
				ItemType type = attackContext.weaponDef.type;
				if (type <= ItemType.Sword)
				{
					if (type != ItemType.None)
					{
						if (type == ItemType.Sword)
						{
							text = "sword_hit";
						}
					}
					else
					{
						text = "zombie_hit_player";
					}
				}
				else if (type != ItemType.Bow)
				{
					if (type != ItemType.Spit)
					{
						if (type == ItemType.Pike)
						{
							text = "spear_hit_zombie";
						}
					}
					else
					{
						text = "spitter_hit";
					}
				}
				else
				{
					text = "bow_hit_zombie";
				}
				if (!string.IsNullOrEmpty(text))
				{
					if (attackContext.weaponDef.type == ItemType.Spit || attackContext.weaponDef.type == ItemType.Bow)
					{
						LazyAudio.PlayAtPos(text, attackContext.hitPosition);
					}
					else
					{
						LazyAudio.PlayAtGameObject(text, base.transform, SpatialType.sound3D, true);
					}
				}
			}
			DamageEffectComponent.TryPlayEffect(target, attackContext);
		}
	}

	// Token: 0x0600116D RID: 4461 RVA: 0x00002318 File Offset: 0x00000518
	private void HandleMiss()
	{
	}

	// Token: 0x0600116E RID: 4462 RVA: 0x00057EB0 File Offset: 0x000560B0
	private void SubscribeToEvents()
	{
		foreach (IDamageDealer damageDealer in this.weapon.Dealers)
		{
			damageDealer.OnHit += this.HandleHit;
			damageDealer.OnMiss += this.HandleMiss;
		}
	}

	// Token: 0x0600116F RID: 4463 RVA: 0x00057F24 File Offset: 0x00056124
	private void UnsubscribeFromEvents()
	{
		if (!this.weapon)
		{
			return;
		}
		foreach (IDamageDealer damageDealer in this.weapon.Dealers)
		{
			damageDealer.OnHit -= this.HandleHit;
			damageDealer.OnMiss -= this.HandleMiss;
		}
	}

	// Token: 0x04001356 RID: 4950
	public Action OnWeaponChanged;

	// Token: 0x04001357 RID: 4951
	private Action OnAnimationFinished;

	// Token: 0x04001358 RID: 4952
	public ICombatEntity combatEntity;

	// Token: 0x04001359 RID: 4953
	private AttackComponent.FighterDefProvider fighterDefProvider;

	// Token: 0x0400135A RID: 4954
	public Weapon weapon;

	// Token: 0x0400135B RID: 4955
	public AnimationComponent animationComponent;

	// Token: 0x0400135C RID: 4956
	[SerializeField]
	private List<Weapon> availableWeapons = new List<Weapon>();

	// Token: 0x0400135D RID: 4957
	public LazyConsts.Fighting.TeamType teamType;

	// Token: 0x0400135E RID: 4958
	private bool useAnimation = true;

	// Token: 0x020002A9 RID: 681
	private class FighterDefProvider
	{
		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06001171 RID: 4465 RVA: 0x00057FC4 File Offset: 0x000561C4
		public int Damage
		{
			get
			{
				if (this.fighterDef != null)
				{
					return this.fighterDef.atkDamage.EvaluateInt(this.attackComponent.combatEntity);
				}
				Weapon weapon = this.attackComponent.weapon;
				if (weapon == null)
				{
					return 0;
				}
				return weapon.ItemDef.damage.EvaluateInt(this.attackComponent.combatEntity);
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06001172 RID: 4466 RVA: 0x00058020 File Offset: 0x00056220
		public int Armor
		{
			get
			{
				if (this.fighterDef == null)
				{
					return 0;
				}
				return this.fighterDef.armor.EvaluateInt(this.attackComponent.combatEntity);
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06001173 RID: 4467 RVA: 0x00058047 File Offset: 0x00056247
		public bool HasKnockback
		{
			get
			{
				FighterDef fighterDef = this.fighterDef;
				if (fighterDef == null)
				{
					Weapon weapon = this.attackComponent.weapon;
					return weapon != null && weapon.ItemDef.knockbackForce.More(0f, 1E-05f);
				}
				return fighterDef.HasKnockback;
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06001174 RID: 4468 RVA: 0x00058084 File Offset: 0x00056284
		public float KnockbackForce
		{
			get
			{
				if (this.fighterDef != null)
				{
					return this.fighterDef.knockbackForce.EvaluateFloat(this.attackComponent.combatEntity);
				}
				Weapon weapon = this.attackComponent.weapon;
				if (weapon == null)
				{
					return 0f;
				}
				return weapon.ItemDef.knockbackForce;
			}
		}

		// Token: 0x06001175 RID: 4469 RVA: 0x000580D4 File Offset: 0x000562D4
		public AttackContext GetAttackContext(Vector3 direction)
		{
			ICombatEntity combatEntity = this.attackComponent.combatEntity;
			LazyConsts.Fighting.TeamType teamType = this.attackComponent.teamType;
			FighterDef fighterDef = this.fighterDef;
			ItemDef itemDef = this.attackComponent.weapon.ItemDef;
			Vector3 position = this.attackComponent.weapon.transform.position;
			AttackComponent attackComponent = this.attackComponent;
			return new AttackContext(combatEntity, teamType, fighterDef, itemDef, position, direction, default(Vector3), -1, false, attackComponent, null);
		}

		// Token: 0x06001176 RID: 4470 RVA: 0x0005813C File Offset: 0x0005633C
		public FighterDefProvider(FighterDef fighterDef, AttackComponent attackComponent)
		{
			this.fighterDef = fighterDef;
			this.attackComponent = attackComponent;
		}

		// Token: 0x04001360 RID: 4960
		[CanBeNull]
		private FighterDef fighterDef;

		// Token: 0x04001361 RID: 4961
		private AttackComponent attackComponent;
	}
}
