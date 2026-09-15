using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002E8 RID: 744
public class LeglessZombieJumpCommand : MobCommand
{
	// Token: 0x0600138F RID: 5007 RVA: 0x0005F10C File Offset: 0x0005D30C
	public LeglessZombieJumpCommand(Vector3 landingPosition, float jumpDuration, float jumpArcHeight, AnimationCurve jumpHeightCurve)
		: base(MobCommand.CommandType.ZombieJump)
	{
		this.landingPosition = landingPosition;
		this.jumpDuration = Mathf.Max(0.01f, jumpDuration);
		this.jumpArcHeight = Mathf.Max(0f, jumpArcHeight);
		this.jumpHeightCurve = jumpHeightCurve;
	}

	// Token: 0x1700035F RID: 863
	// (get) Token: 0x06001390 RID: 5008 RVA: 0x0005F15C File Offset: 0x0005D35C
	public override Vector2 DirectionToTarget
	{
		get
		{
			return (this.landingPosition - base.Wgo.Data.Position).XZ2().normalized;
		}
	}

	// Token: 0x06001391 RID: 5009 RVA: 0x0005F191 File Offset: 0x0005D391
	public override void Init(FightingAgent agent)
	{
		base.Init(agent);
		this.hpOnPrepareStart = agent.Wgo.Data.HpComponent.Hp;
		this.richAICustom = agent.RichAI;
	}

	// Token: 0x06001392 RID: 5010 RVA: 0x0005F1C4 File Offset: 0x0005D3C4
	public override void OnStart()
	{
		this.phase = LeglessZombieJumpCommand.JumpPhase.Prepare;
		this.agent.RVO_Locked = true;
		this.agent.RichAI.SetPath(null, false);
		base.Wgo.MainWgoPart.AnimationComponent.SetState(global::AnimationState.JumpPrepare);
		base.SetFacingDirection(this.DirectionToTarget, false);
		this.SubscribeToDamage();
	}

	// Token: 0x06001393 RID: 5011 RVA: 0x0005F224 File Offset: 0x0005D424
	public override void OnUpdate(float deltaTime)
	{
		switch (this.phase)
		{
		case LeglessZombieJumpCommand.JumpPhase.Prepare:
			base.SetFacingDirection(this.DirectionToTarget, false);
			if (this.jumpInterruptedByDamage)
			{
				this.FinishCommand();
				return;
			}
			if (this.IsCurrentAnimationFinished())
			{
				this.BeginJump();
				return;
			}
			break;
		case LeglessZombieJumpCommand.JumpPhase.Jump:
			this.UpdateJump(deltaTime);
			return;
		case LeglessZombieJumpCommand.JumpPhase.Land:
			if (this.IsCurrentAnimationFinished())
			{
				this.FinishCommand();
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06001394 RID: 5012 RVA: 0x0005F28C File Offset: 0x0005D48C
	public override void OnFinish()
	{
		this.RestoreNonTriggerColliders();
		this.SetGroundSnappingEnabled(true);
		this.UnsubscribeFromDamage();
		this.agent.RVO_Locked = false;
		this.agent.TeleportToNavmesh(base.Wgo.Data.Position, false);
		WgoPart mainWgoPart = base.Wgo.MainWgoPart;
		if (mainWgoPart == null)
		{
			return;
		}
		AnimationComponentBase animationComponent = mainWgoPart.AnimationComponent;
		if (animationComponent == null)
		{
			return;
		}
		animationComponent.SetState(global::AnimationState.Idle);
	}

	// Token: 0x06001395 RID: 5013 RVA: 0x0005F2F4 File Offset: 0x0005D4F4
	private void BeginJump()
	{
		this.phase = LeglessZombieJumpCommand.JumpPhase.Jump;
		this.jumpStartPosition = base.Wgo.Data.Position;
		this.jumpTime = 0f;
		this.DisableNonTriggerColliders();
		this.SetGroundSnappingEnabled(false);
		base.Wgo.MainWgoPart.AnimationComponent.SetState(global::AnimationState.Jump);
	}

	// Token: 0x06001396 RID: 5014 RVA: 0x0005F350 File Offset: 0x0005D550
	private void UpdateJump(float deltaTime)
	{
		this.jumpTime += deltaTime;
		float num = Mathf.Clamp01(this.jumpTime / this.jumpDuration);
		Vector3 vector = Vector3.Lerp(this.jumpStartPosition, this.landingPosition, num);
		float num2 = ((this.jumpHeightCurve != null) ? this.jumpHeightCurve.Evaluate(num) : (4f * num * (1f - num)));
		Vector3 vector2 = vector + Vector3.up * (num2 * this.jumpArcHeight);
		this.agent.TeleportToNavmesh(vector2, false);
		base.SetFacingDirection(this.DirectionToTarget, false);
		if (num >= 1f)
		{
			this.agent.TeleportToNavmesh(this.landingPosition, false);
			this.SetGroundSnappingEnabled(true);
			this.RestoreNonTriggerColliders();
			this.phase = LeglessZombieJumpCommand.JumpPhase.Land;
			base.Wgo.MainWgoPart.AnimationComponent.SetState(global::AnimationState.JumpLand);
		}
	}

	// Token: 0x06001397 RID: 5015 RVA: 0x0005F430 File Offset: 0x0005D630
	private bool IsCurrentAnimationFinished()
	{
		Animator animator = base.Wgo.MainWgoPart.AnimationComponent.Animator;
		return !animator || animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f;
	}

	// Token: 0x06001398 RID: 5016 RVA: 0x0005F476 File Offset: 0x0005D676
	private void FinishCommand()
	{
		this.agent.StopCommandExecution(true);
	}

	// Token: 0x06001399 RID: 5017 RVA: 0x0005F484 File Offset: 0x0005D684
	private void SubscribeToDamage()
	{
		if (this.isSubscribedToHpChanged)
		{
			return;
		}
		this.agent.Wgo.Data.HpComponent.OnHpChanged += this.HandleHpChanged;
		this.isSubscribedToHpChanged = true;
	}

	// Token: 0x0600139A RID: 5018 RVA: 0x0005F4BC File Offset: 0x0005D6BC
	private void UnsubscribeFromDamage()
	{
		if (!this.isSubscribedToHpChanged)
		{
			return;
		}
		this.agent.Wgo.Data.HpComponent.OnHpChanged -= this.HandleHpChanged;
		this.isSubscribedToHpChanged = false;
	}

	// Token: 0x0600139B RID: 5019 RVA: 0x0005F4F4 File Offset: 0x0005D6F4
	private void HandleHpChanged(HPComponent hpComponent)
	{
		if (this.phase != LeglessZombieJumpCommand.JumpPhase.Prepare)
		{
			return;
		}
		if (hpComponent.Hp < this.hpOnPrepareStart)
		{
			this.jumpInterruptedByDamage = true;
		}
		this.hpOnPrepareStart = hpComponent.Hp;
	}

	// Token: 0x0600139C RID: 5020 RVA: 0x0005F520 File Offset: 0x0005D720
	private void DisableNonTriggerColliders()
	{
		this.disabledNonTriggerColliders.Clear();
		foreach (Collider collider in base.Wgo.GetComponentsInChildren<Collider>(true))
		{
			if (!(collider == null) && !collider.isTrigger && collider.enabled)
			{
				collider.enabled = false;
				this.disabledNonTriggerColliders.Add(collider);
			}
		}
	}

	// Token: 0x0600139D RID: 5021 RVA: 0x0005F584 File Offset: 0x0005D784
	private void RestoreNonTriggerColliders()
	{
		if (this.disabledNonTriggerColliders.Count == 0)
		{
			return;
		}
		for (int i = 0; i < this.disabledNonTriggerColliders.Count; i++)
		{
			Collider collider = this.disabledNonTriggerColliders[i];
			if (collider != null)
			{
				collider.enabled = true;
			}
		}
		this.disabledNonTriggerColliders.Clear();
	}

	// Token: 0x0600139E RID: 5022 RVA: 0x0005F5DD File Offset: 0x0005D7DD
	private void SetGroundSnappingEnabled(bool isEnabled)
	{
		if (this.richAICustom != null)
		{
			this.richAICustom.GroundSnapEnabled = isEnabled;
		}
	}

	// Token: 0x040014BF RID: 5311
	private readonly Vector3 landingPosition;

	// Token: 0x040014C0 RID: 5312
	private readonly float jumpDuration;

	// Token: 0x040014C1 RID: 5313
	private readonly float jumpArcHeight;

	// Token: 0x040014C2 RID: 5314
	private readonly AnimationCurve jumpHeightCurve;

	// Token: 0x040014C3 RID: 5315
	private LeglessZombieJumpCommand.JumpPhase phase;

	// Token: 0x040014C4 RID: 5316
	private Vector3 jumpStartPosition;

	// Token: 0x040014C5 RID: 5317
	private float jumpTime;

	// Token: 0x040014C6 RID: 5318
	private bool jumpInterruptedByDamage;

	// Token: 0x040014C7 RID: 5319
	private int hpOnPrepareStart;

	// Token: 0x040014C8 RID: 5320
	private bool isSubscribedToHpChanged;

	// Token: 0x040014C9 RID: 5321
	private readonly List<Collider> disabledNonTriggerColliders = new List<Collider>();

	// Token: 0x040014CA RID: 5322
	private RichAI_Custom richAICustom;

	// Token: 0x020002E9 RID: 745
	private enum JumpPhase
	{
		// Token: 0x040014CC RID: 5324
		Prepare,
		// Token: 0x040014CD RID: 5325
		Jump,
		// Token: 0x040014CE RID: 5326
		Land
	}
}
