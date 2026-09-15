using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000545 RID: 1349
[ExecuteInEditMode]
public class NpcFishingContainer : MonoBehaviour
{
	// Token: 0x06002298 RID: 8856 RVA: 0x000A2404 File Offset: 0x000A0604
	private void OnEnable()
	{
		this.bobStartPos = this.target.position;
		this.moveProgress = 0f;
		this.hasActivatedAtPeak = false;
		this.animationComponent.SetTrigger("fish_cast");
	}

	// Token: 0x06002299 RID: 8857 RVA: 0x00002318 File Offset: 0x00000518
	private void OnDisable()
	{
	}

	// Token: 0x0600229A RID: 8858 RVA: 0x000A2439 File Offset: 0x000A0639
	private void LateUpdate()
	{
		if (this.moveProgress < 1f)
		{
			this.BringBobToStart();
		}
	}

	// Token: 0x0600229B RID: 8859 RVA: 0x000A2450 File Offset: 0x000A0650
	private void BringBobToStart()
	{
		this.moveProgress += Time.deltaTime / this.bobMovementDuration;
		float num = Mathf.Clamp01(this.moveProgress);
		float num2 = this.bobMovementCurve.Evaluate(num);
		Vector3 vector = Vector3.Lerp(this.castStartPoint.position, this.bobStartPos, num2);
		float num3 = this.bobDivingCurve.Evaluate(num) * this.bobDivingAmount;
		this.bob.transform.position = vector + Vector3.up * num3;
		if (num2 >= 0.2f && !this.hasActivatedAtPeak)
		{
			FXContainerEventListener fxcontainerEventListener;
			if (this.animationComponent.Animator.TryGetComponent<FXContainerEventListener>(out fxcontainerEventListener))
			{
				fxcontainerEventListener.PlayFx("fishing_catch");
				LazyAudio.PlayAtGameObject("fishing_blop", base.transform, SpatialType.sound3D, true);
			}
			this.hasActivatedAtPeak = true;
		}
	}

	// Token: 0x04001F3B RID: 7995
	[SerializeField]
	private RopeRenderer rope;

	// Token: 0x04001F3C RID: 7996
	[SerializeField]
	private Transform castStartPoint;

	// Token: 0x04001F3D RID: 7997
	[SerializeField]
	private RopePoint bob;

	// Token: 0x04001F3E RID: 7998
	[SerializeField]
	private AnimationComponent animationComponent;

	// Token: 0x04001F3F RID: 7999
	[SerializeField]
	private Transform target;

	// Token: 0x04001F40 RID: 8000
	[Header("Bob Movement")]
	[SerializeField]
	private float bobMovementDuration = 0.1f;

	// Token: 0x04001F41 RID: 8001
	[SerializeField]
	private float bobDivingAmount = 0.3f;

	// Token: 0x04001F42 RID: 8002
	[SerializeField]
	private AnimationCurve bobMovementCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	// Token: 0x04001F43 RID: 8003
	[SerializeField]
	private AnimationCurve bobDivingCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.5f, -1f),
		new Keyframe(1f, 0f)
	});

	// Token: 0x04001F44 RID: 8004
	private float moveProgress = 1f;

	// Token: 0x04001F45 RID: 8005
	private Vector3 bobStartPos;

	// Token: 0x04001F46 RID: 8006
	private bool hasActivatedAtPeak;
}
