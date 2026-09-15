using System;
using System.Collections;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020002D0 RID: 720
public class AutomaticTurret : MonoBehaviour
{
	// Token: 0x17000308 RID: 776
	// (get) Token: 0x06001279 RID: 4729 RVA: 0x0005B6E1 File Offset: 0x000598E1
	public bool IsAIActive
	{
		get
		{
			return this.isAIActive;
		}
	}

	// Token: 0x0600127A RID: 4730 RVA: 0x0005B6EC File Offset: 0x000598EC
	public void Activate()
	{
		if (this.IsAIActive)
		{
			return;
		}
		if (this.settings == null)
		{
			Debug.LogError("[AutomaticTurret] Turret settings not assigned on " + base.gameObject.name + "!", base.gameObject);
			return;
		}
		this.wgo = base.GetComponentInParent<Wgo>();
		if (this.wgo)
		{
			this.fighterDef = GameBalance.Me.GetData<FighterDef>(this.wgo.Id);
		}
		if (this.fighterDef == null)
		{
			Debug.LogError("[AutomaticTurret] FighterDef not found for wgo: " + this.wgo.Id + ".");
			return;
		}
		this.isAIActive = true;
		this.SubscribeToEvents();
		this.shotIntervalTime = this.fighterDef.atkPause.EvaluateFloat(this.wgo);
		this.waitScanIntervalTime = new WaitForSeconds(this.settings.scanInterval);
		this.waitShootIntervalTime = new WaitForSeconds(this.shotIntervalTime);
		if (this.updateAICoroutine != null)
		{
			base.StopCoroutine(this.updateAICoroutine);
		}
		this.updateAICoroutine = base.StartCoroutine(this.UpdateAI());
	}

	// Token: 0x0600127B RID: 4731 RVA: 0x0005B808 File Offset: 0x00059A08
	public void Deactivate()
	{
		this.UnsubscribeFromEvents();
		this.isAIActive = false;
		if (this.updateAICoroutine != null)
		{
			base.StopCoroutine(this.updateAICoroutine);
		}
		this.currentTarget = null;
	}

	// Token: 0x0600127C RID: 4732 RVA: 0x0005B832 File Offset: 0x00059A32
	private void OnDisable()
	{
		this.Deactivate();
	}

	// Token: 0x0600127D RID: 4733 RVA: 0x0005B83A File Offset: 0x00059A3A
	private IEnumerator UpdateAI()
	{
		while (this.isAIActive)
		{
			this.currentTarget = this.ScanForTarget();
			if (this.currentTarget == null)
			{
				yield return this.waitScanIntervalTime;
			}
			else
			{
				while (this.currentTarget != null)
				{
					if (!this.IsTargetInRange(this.currentTarget))
					{
						this.currentTarget = null;
						yield return this.waitScanIntervalTime;
						break;
					}
					yield return this.DoShootingLogic();
					yield return this.waitShootIntervalTime;
				}
			}
		}
		this.Deactivate();
		yield return null;
		yield break;
	}

	// Token: 0x0600127E RID: 4734 RVA: 0x0005B84C File Offset: 0x00059A4C
	private FightingAgent ScanForTarget()
	{
		this.shootRange = this.fighterDef.atkRange.EvaluateFloat(this.wgo);
		Array.Clear(this.overlapColliders, 0, this.overlapColliders.Length);
		int num = Physics.OverlapCapsuleNonAlloc(base.transform.position + Vector3.down * 2f, base.transform.position + Vector3.up * 2f, this.shootRange, this.overlapColliders, 536870912, QueryTriggerInteraction.Ignore);
		FightingAgent fightingAgent = null;
		float num2 = float.MaxValue;
		for (int i = 0; i < num; i++)
		{
			FightingAgent componentInParent = this.overlapColliders[i].GetComponentInParent<FightingAgent>();
			if (!(componentInParent == null) && (!this.bow || AgentAI.TryLineCastByRecast(this.bow.transform.position, componentInParent.Wgo.CombatEntityPosition)) && componentInParent.Wgo.TeamType != LazyConsts.Fighting.TeamType.Player)
			{
				float num3 = Vector3.Distance(base.transform.position, componentInParent.transform.position);
				if (num3 < num2 && num3 <= this.shootRange)
				{
					fightingAgent = componentInParent;
					num2 = num3;
				}
			}
		}
		return fightingAgent;
	}

	// Token: 0x0600127F RID: 4735 RVA: 0x0005B984 File Offset: 0x00059B84
	private IEnumerator DoShootingLogic()
	{
		foreach (float num in this.settings.shotSampleTimings)
		{
			yield return new WaitForSeconds(num);
			this.ShootAtTarget(ref this.currentTarget);
		}
		List<float>.Enumerator enumerator = default(List<float>.Enumerator);
		yield break;
		yield break;
	}

	// Token: 0x06001280 RID: 4736 RVA: 0x0005B994 File Offset: 0x00059B94
	private bool IsTargetInRange(FightingAgent target)
	{
		return (this.GetAimPosition(target.transform.position) - base.transform.position).magnitude <= this.shootRange;
	}

	// Token: 0x06001281 RID: 4737 RVA: 0x0005B9D8 File Offset: 0x00059BD8
	private void ShootAtTarget(ref FightingAgent target)
	{
		if (target && target.Wgo.Data.HpComponent.Hp > 0)
		{
			Vector3 position = this.bow.transform.position;
			Vector3 normalized = (this.GetAimPosition(target.Wgo.Data.Position) - position).normalized;
			AttackContext attackContext = new AttackContext(this.wgo, LazyConsts.Fighting.TeamType.Player, this.fighterDef, this.bow.ItemDef, this.bow.transform.position, normalized, default(Vector3), -1, false, null, null);
			LazyAudio.PlayAtGameObject("bow_aim_shot", this.bow.transform, SpatialType.sound3D, true);
			this.bow.Activate(attackContext);
			return;
		}
		target = null;
	}

	// Token: 0x06001282 RID: 4738 RVA: 0x0005BAA9 File Offset: 0x00059CA9
	private Vector3 GetAimPosition(Vector3 targetPosition)
	{
		return targetPosition + Vector3.up * 1.666667f / 2f;
	}

	// Token: 0x06001283 RID: 4739 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06001284 RID: 4740 RVA: 0x0005BACA File Offset: 0x00059CCA
	private void SubscribeToEvents()
	{
		this.bow.Dealer.OnHit += this.HandleHit;
		this.bow.Dealer.OnMiss += this.HandleMiss;
	}

	// Token: 0x06001285 RID: 4741 RVA: 0x0005BB04 File Offset: 0x00059D04
	private void UnsubscribeFromEvents()
	{
		if (!this.bow)
		{
			return;
		}
		this.bow.Dealer.OnHit -= this.HandleHit;
		this.bow.Dealer.OnMiss -= this.HandleMiss;
	}

	// Token: 0x06001286 RID: 4742 RVA: 0x0005BB58 File Offset: 0x00059D58
	private void HandleHit(ICombatEntity combatEntity, AttackContext attackContext)
	{
		int num = this.fighterDef.atkDamage.EvaluateInt(this.wgo);
		num = ((!attackContext.hasCustomDamage) ? num : attackContext.customDamage);
		Debug.Log(string.Format("Handle hit to {0}, damage: {1}", combatEntity.CombatEntityUID, num));
		num = Mathf.Clamp(num - combatEntity.ArmorValue, 0, int.MaxValue);
		if (combatEntity.IsActiveCombatant)
		{
			combatEntity.CombatEntityHpComponent.ApplyDamage(num);
			IKnockbackable knockbackable = combatEntity as IKnockbackable;
			if (knockbackable != null)
			{
				knockbackable.ApplyKnockback(attackContext, this.fighterDef.HasKnockback ? this.fighterDef.knockbackForce.EvaluateFloat(this.wgo) : 0f);
			}
		}
		AttackContext attackContext2 = new AttackContext(this.wgo, this.wgo.TeamType, this.fighterDef, this.bow.ItemDef, this.wgo.CombatEntityPosition, attackContext.direction, attackContext.hitPosition);
		if (num != 0)
		{
			combatEntity.OnOtherCombatTargetHitMe(attackContext2);
		}
	}

	// Token: 0x06001287 RID: 4743 RVA: 0x0005BC56 File Offset: 0x00059E56
	private void HandleMiss()
	{
		Debug.Log("Miss happened");
	}

	// Token: 0x04001418 RID: 5144
	[SerializeField]
	private BowWeapon bow;

	// Token: 0x04001419 RID: 5145
	[Header("Settings")]
	[SerializeField]
	private TurretSettings settings;

	// Token: 0x0400141A RID: 5146
	[Header("Debug Settings")]
	[SerializeField]
	private bool debugModeEnabled;

	// Token: 0x0400141B RID: 5147
	private FighterDef fighterDef;

	// Token: 0x0400141C RID: 5148
	private Wgo wgo;

	// Token: 0x0400141D RID: 5149
	private bool isAIActive;

	// Token: 0x0400141E RID: 5150
	private FightingAgent currentTarget;

	// Token: 0x0400141F RID: 5151
	private Coroutine updateAICoroutine;

	// Token: 0x04001420 RID: 5152
	private float shootRange;

	// Token: 0x04001421 RID: 5153
	private float shotIntervalTime;

	// Token: 0x04001422 RID: 5154
	private Collider[] overlapColliders = new Collider[60];

	// Token: 0x04001423 RID: 5155
	private WaitForSeconds waitScanIntervalTime;

	// Token: 0x04001424 RID: 5156
	private WaitForSeconds waitShootIntervalTime;

	// Token: 0x04001425 RID: 5157
	private WaitForSeconds waitForSecondaryShoot;
}
