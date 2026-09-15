using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000344 RID: 836
public class SwordAttackEmulationComponent : MonoBehaviour
{
	// Token: 0x170003CF RID: 975
	// (get) Token: 0x0600161D RID: 5661 RVA: 0x0006B11E File Offset: 0x0006931E
	public bool IsAttacking
	{
		get
		{
			return this.isAttacking;
		}
	}

	// Token: 0x0600161E RID: 5662 RVA: 0x0006B126 File Offset: 0x00069326
	public void Init(AttackComponent attack, AnimationComponent anim)
	{
		this.attackComponent = attack;
		this.animationComponent = anim;
	}

	// Token: 0x0600161F RID: 5663 RVA: 0x0006B136 File Offset: 0x00069336
	public void PerformAttack()
	{
		this.PerformAttack(null);
	}

	// Token: 0x06001620 RID: 5664 RVA: 0x0006B140 File Offset: 0x00069340
	public void PerformAttack(Action onFinished = null)
	{
		if (this.isAttacking)
		{
			return;
		}
		this.isAttacking = true;
		if (this.animationComponent != null)
		{
			this.animationComponent.SetLayerWeight(AnimationComponent.Layers.SwordAttack, 1f);
			this.animationComponent.SetLayerWeight(AnimationComponent.Layers.ArmorWithSword, 1f);
			if (this.animationComponent.Animator != null)
			{
				this.animationComponent.Animator.Update(0f);
			}
			this.animationComponent.SetTrigger("attack");
			base.StartCoroutine(this.WaitForAnimationRoutine(onFinished));
			return;
		}
		this.isAttacking = false;
		if (onFinished != null)
		{
			onFinished();
		}
	}

	// Token: 0x06001621 RID: 5665 RVA: 0x0006B1E5 File Offset: 0x000693E5
	private IEnumerator WaitForAnimationRoutine(Action onFinished)
	{
		Animator animator = this.animationComponent.Animator;
		int layerIndex = 11;
		float timeout = Time.time + 0.5f;
		while (!animator.IsInTransition(layerIndex) && Time.time < timeout)
		{
			yield return null;
		}
		timeout = Time.time + 1f;
		while (animator.IsInTransition(layerIndex) && Time.time < timeout)
		{
			yield return null;
		}
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);
		timeout = Time.time + 5f;
		while (stateInfo.normalizedTime < 0.95f && Time.time < timeout)
		{
			stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);
			yield return null;
		}
		this.isAttacking = false;
		if (this.animationComponent != null)
		{
			this.animationComponent.SetLayerWeight(AnimationComponent.Layers.SwordAttack, 0f);
		}
		if (onFinished != null)
		{
			onFinished();
		}
		yield break;
	}

	// Token: 0x06001622 RID: 5666 RVA: 0x0006B1FB File Offset: 0x000693FB
	public void StartLoop()
	{
		this.StartLoop(this.testLoopDelay);
	}

	// Token: 0x06001623 RID: 5667 RVA: 0x0006B209 File Offset: 0x00069409
	public void StartLoop(float delay)
	{
		this.StopLoop();
		this.loopCoroutine = base.StartCoroutine(this.AttackLoopRoutine(delay));
	}

	// Token: 0x06001624 RID: 5668 RVA: 0x0006B224 File Offset: 0x00069424
	public void StopLoop()
	{
		if (this.loopCoroutine != null)
		{
			base.StopCoroutine(this.loopCoroutine);
			this.loopCoroutine = null;
		}
		this.isAttacking = false;
	}

	// Token: 0x06001625 RID: 5669 RVA: 0x0006B248 File Offset: 0x00069448
	private IEnumerator AttackLoopRoutine(float delay)
	{
		for (;;)
		{
			SwordAttackEmulationComponent.<>c__DisplayClass15_0 CS$<>8__locals1 = new SwordAttackEmulationComponent.<>c__DisplayClass15_0();
			CS$<>8__locals1.done = false;
			this.PerformAttack(delegate
			{
				CS$<>8__locals1.done = true;
			});
			while (!CS$<>8__locals1.done)
			{
				yield return null;
			}
			Debug.Log("AttackLoopRoutine: done");
			if (delay > 0f)
			{
				yield return new WaitForSeconds(delay);
			}
			CS$<>8__locals1 = null;
		}
		yield break;
	}

	// Token: 0x06001626 RID: 5670 RVA: 0x0006B25E File Offset: 0x0006945E
	private void OnDisable()
	{
		this.StopLoop();
	}

	// Token: 0x0400167A RID: 5754
	private const string ATTACK_TRIGGER = "attack";

	// Token: 0x0400167B RID: 5755
	[SerializeField]
	private AttackComponent attackComponent;

	// Token: 0x0400167C RID: 5756
	[SerializeField]
	private AnimationComponent animationComponent;

	// Token: 0x0400167D RID: 5757
	[SerializeField]
	private float testLoopDelay = 0.5f;

	// Token: 0x0400167E RID: 5758
	private bool isAttacking;

	// Token: 0x0400167F RID: 5759
	private Coroutine loopCoroutine;
}
