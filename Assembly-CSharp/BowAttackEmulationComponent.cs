using System;
using System.Collections;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020002AE RID: 686
public class BowAttackEmulationComponent : MonoBehaviour
{
	// Token: 0x170002D7 RID: 727
	// (get) Token: 0x06001184 RID: 4484 RVA: 0x000583D3 File Offset: 0x000565D3
	public bool IsAttacking
	{
		get
		{
			return this.isAttacking;
		}
	}

	// Token: 0x170002D8 RID: 728
	// (get) Token: 0x06001185 RID: 4485 RVA: 0x000583DB File Offset: 0x000565DB
	public IReadOnlyList<WgoData> PossibleTargets
	{
		get
		{
			return this.possibleTargets;
		}
	}

	// Token: 0x06001186 RID: 4486 RVA: 0x000583E4 File Offset: 0x000565E4
	public void Init(AttackComponent attack, AnimationComponent anim)
	{
		this.attackComponent = attack;
		this.animationComponent = anim;
		Wgo componentInParent = base.gameObject.GetComponentInParent<Wgo>();
		if (componentInParent == null)
		{
			Debug.LogError("Wgo not found");
			return;
		}
		FighterDef data = GameBalance.Me.GetData<FighterDef>(componentInParent.Id);
		if (data == null)
		{
			Debug.LogError("FighterDef not found");
			return;
		}
		this.attackComponent.Init(this.attackComponent.combatEntity, data, null);
	}

	// Token: 0x06001187 RID: 4487 RVA: 0x00058456 File Offset: 0x00056656
	public void SetTargets(List<WgoData> targets)
	{
		this.possibleTargets = targets ?? new List<WgoData>();
	}

	// Token: 0x06001188 RID: 4488 RVA: 0x00058468 File Offset: 0x00056668
	public void PerformAttack()
	{
		this.PerformAttack(null);
	}

	// Token: 0x06001189 RID: 4489 RVA: 0x00058474 File Offset: 0x00056674
	public void PerformAttack(float delay, Action onFinished = null)
	{
		this.PerformAttack(delegate
		{
			if (delay > 0f)
			{
				this.StartCoroutine(this.DelayRoutine(delay, onFinished));
				return;
			}
			Action onFinished2 = onFinished;
			if (onFinished2 == null)
			{
				return;
			}
			onFinished2();
		});
	}

	// Token: 0x0600118A RID: 4490 RVA: 0x000584B0 File Offset: 0x000566B0
	public void PerformAttack(Action onFinished = null)
	{
		if (this.isAttacking)
		{
			return;
		}
		if (this.attackComponent == null)
		{
			this.attackComponent = base.GetComponent<AttackComponent>();
		}
		if (this.animationComponent == null)
		{
			this.animationComponent = base.GetComponentInChildren<AnimationComponent>();
		}
		WgoData randomTarget = this.GetRandomTarget();
		if (randomTarget == null || this.attackComponent == null)
		{
			if (onFinished != null)
			{
				onFinished();
			}
			return;
		}
		BowWeapon bowWeapon = this.attackComponent.weapon as BowWeapon;
		if (bowWeapon == null)
		{
			if (onFinished != null)
			{
				onFinished();
			}
			return;
		}
		this.isAttacking = true;
		Vector3 vector = (this.GetTargetPosition(randomTarget) - bowWeapon.transform.position).XZ().normalized;
		if (vector.sqrMagnitude < 0.0001f)
		{
			vector = ((this.animationComponent != null) ? this.animationComponent.GetDirection().XZ().normalized : Vector3.forward);
		}
		if (this.animationComponent == null || this.animationComponent.AnimationEventReceiver == null)
		{
			this.EmitArrow(vector);
			this.isAttacking = false;
			if (onFinished != null)
			{
				onFinished();
			}
			return;
		}
		this.animationComponent.SetLayerWeight(AnimationComponent.Layers.BowAttack, 1f);
		this.animationComponent.SetLayerWeight(AnimationComponent.Layers.ArmorWithBow, 1f);
		this.animationComponent.SetDirection(vector.XZ2());
		Animator animator = this.animationComponent.Animator;
		if (animator != null)
		{
			animator.Update(0f);
		}
		this.attackCoroutine = base.StartCoroutine(this.TurnToTargetAndShootRoutine(vector, onFinished));
	}

	// Token: 0x0600118B RID: 4491 RVA: 0x0005863C File Offset: 0x0005683C
	public void StartLoop()
	{
		AttackComponent component = base.GetComponent<AttackComponent>();
		AnimationComponent componentInChildren = base.GetComponentInChildren<AnimationComponent>();
		if (component == null || componentInChildren == null)
		{
			Debug.LogError("AttackComponent or AnimationComponent not found");
			return;
		}
		this.Init(component, componentInChildren);
		this.StartLoop(this.testLoopDelay);
	}

	// Token: 0x0600118C RID: 4492 RVA: 0x00058688 File Offset: 0x00056888
	public void StartLoop(float delay)
	{
		this.StopLoop();
		this.loopCoroutine = base.StartCoroutine(this.AttackLoopRoutine(delay));
	}

	// Token: 0x0600118D RID: 4493 RVA: 0x000586A4 File Offset: 0x000568A4
	public void StopLoop()
	{
		if (this.loopCoroutine != null)
		{
			base.StopCoroutine(this.loopCoroutine);
			this.loopCoroutine = null;
		}
		if (this.attackCoroutine != null)
		{
			base.StopCoroutine(this.attackCoroutine);
			this.attackCoroutine = null;
		}
		this.ClearEmitArrowListener();
		this.isAttacking = false;
		if (this.animationComponent != null)
		{
			this.animationComponent.SetLayerWeight(AnimationComponent.Layers.BowAttack, 0f);
		}
	}

	// Token: 0x0600118E RID: 4494 RVA: 0x00058714 File Offset: 0x00056914
	private IEnumerator TurnToTargetAndShootRoutine(Vector3 direction, Action onFinished)
	{
		if (this.turnToTargetDelay > 0f)
		{
			yield return new WaitForSeconds(this.turnToTargetDelay);
		}
		if (this.animationComponent == null || this.attackComponent == null)
		{
			this.isAttacking = false;
			Action onFinished2 = onFinished;
			if (onFinished2 != null)
			{
				onFinished2();
			}
			yield break;
		}
		if (this.animationComponent.AnimationEventReceiver.onEvent4 == null)
		{
			this.animationComponent.AnimationEventReceiver.onEvent4 = new UnityEvent();
		}
		this.ClearEmitArrowListener();
		this.emitArrowAction = delegate
		{
			this.ClearEmitArrowListener();
			this.EmitArrow(direction);
		};
		this.animationComponent.AnimationEventReceiver.onEvent4.AddListener(this.emitArrowAction);
		this.attackComponent.PerformAttack(false, default(Vector3), true, delegate
		{
			this.ClearEmitArrowListener();
			this.isAttacking = false;
			this.attackCoroutine = null;
			if (this.animationComponent != null)
			{
				this.animationComponent.SetLayerWeight(AnimationComponent.Layers.BowAttack, 0f);
			}
			Action onFinished3 = onFinished;
			if (onFinished3 == null)
			{
				return;
			}
			onFinished3();
		}, false);
		yield break;
	}

	// Token: 0x0600118F RID: 4495 RVA: 0x00058731 File Offset: 0x00056931
	private IEnumerator AttackLoopRoutine(float delay)
	{
		for (;;)
		{
			BowAttackEmulationComponent.<>c__DisplayClass22_0 CS$<>8__locals1 = new BowAttackEmulationComponent.<>c__DisplayClass22_0();
			CS$<>8__locals1.done = false;
			this.PerformAttack(delegate
			{
				CS$<>8__locals1.done = true;
			});
			float timeout = Time.time + 5f;
			while (!CS$<>8__locals1.done && Time.time < timeout)
			{
				yield return null;
			}
			if (delay > 0f)
			{
				yield return new WaitForSeconds(delay);
			}
			CS$<>8__locals1 = null;
		}
		yield break;
	}

	// Token: 0x06001190 RID: 4496 RVA: 0x00058747 File Offset: 0x00056947
	private IEnumerator DelayRoutine(float time, Action onFinished)
	{
		yield return new WaitForSeconds(time);
		if (onFinished != null)
		{
			onFinished();
		}
		yield break;
	}

	// Token: 0x06001191 RID: 4497 RVA: 0x00058760 File Offset: 0x00056960
	private WgoData GetRandomTarget()
	{
		if (this.possibleTargets == null || this.possibleTargets.Count == 0)
		{
			return null;
		}
		List<WgoData> list = new List<WgoData>(this.possibleTargets.Count);
		for (int i = 0; i < this.possibleTargets.Count; i++)
		{
			WgoData wgoData = this.possibleTargets[i];
			if (wgoData != null)
			{
				list.Add(wgoData);
			}
		}
		if (list.Count == 0)
		{
			return null;
		}
		return list[global::UnityEngine.Random.Range(0, list.Count)];
	}

	// Token: 0x06001192 RID: 4498 RVA: 0x000587E0 File Offset: 0x000569E0
	private Vector3 GetTargetPosition(WgoData target)
	{
		Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(target.UniqueId);
		if (wgoViewGlobal != null)
		{
			return wgoViewGlobal.CombatEntityPosition + Vector3.up * 1.666667f / 2f;
		}
		return target.Position + Vector3.up * 1.666667f / 2f;
	}

	// Token: 0x06001193 RID: 4499 RVA: 0x0005884C File Offset: 0x00056A4C
	private void EmitArrow(Vector3 direction)
	{
		AttackComponent attackComponent = this.attackComponent;
		if (((attackComponent != null) ? attackComponent.weapon : null) as BowWeapon == null || direction.sqrMagnitude < 0.0001f)
		{
			return;
		}
		LazyAudio.PlayAtGameObject("bow_aim_shot", this.attackComponent.transform, SpatialType.sound3D, true);
		this.attackComponent.ActivateWeapon(direction);
	}

	// Token: 0x06001194 RID: 4500 RVA: 0x000588AC File Offset: 0x00056AAC
	private void ClearEmitArrowListener()
	{
		AnimationComponent animationComponent = this.animationComponent;
		bool flag;
		if (animationComponent == null)
		{
			flag = null != null;
		}
		else
		{
			AnimationEventReceiver animationEventReceiver = animationComponent.AnimationEventReceiver;
			flag = ((animationEventReceiver != null) ? animationEventReceiver.onEvent4 : null) != null;
		}
		if (flag && this.emitArrowAction != null)
		{
			this.animationComponent.AnimationEventReceiver.onEvent4.RemoveListener(this.emitArrowAction);
		}
		this.emitArrowAction = null;
	}

	// Token: 0x06001195 RID: 4501 RVA: 0x00058903 File Offset: 0x00056B03
	private void OnDisable()
	{
		this.StopLoop();
	}

	// Token: 0x04001371 RID: 4977
	[SerializeField]
	private AttackComponent attackComponent;

	// Token: 0x04001372 RID: 4978
	[SerializeField]
	private AnimationComponent animationComponent;

	// Token: 0x04001373 RID: 4979
	[SerializeField]
	private float testLoopDelay = 1f;

	// Token: 0x04001374 RID: 4980
	[SerializeField]
	private float turnToTargetDelay = 0.12f;

	// Token: 0x04001375 RID: 4981
	[SerializeField]
	private List<WgoData> possibleTargets = new List<WgoData>();

	// Token: 0x04001376 RID: 4982
	private bool isAttacking;

	// Token: 0x04001377 RID: 4983
	private Coroutine loopCoroutine;

	// Token: 0x04001378 RID: 4984
	private Coroutine attackCoroutine;

	// Token: 0x04001379 RID: 4985
	private UnityAction emitArrowAction;
}
