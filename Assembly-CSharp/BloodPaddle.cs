using System;
using DG.Tweening;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020002B9 RID: 697
public class BloodPaddle : MonoBehaviour, IFightDecal
{
	// Token: 0x170002EB RID: 747
	// (get) Token: 0x060011D9 RID: 4569 RVA: 0x00059355 File Offset: 0x00057555
	// (set) Token: 0x060011DA RID: 4570 RVA: 0x0005935D File Offset: 0x0005755D
	public float Lifetime { get; set; }

	// Token: 0x170002EC RID: 748
	// (get) Token: 0x060011DB RID: 4571 RVA: 0x00059366 File Offset: 0x00057566
	// (set) Token: 0x060011DC RID: 4572 RVA: 0x0005936E File Offset: 0x0005756E
	public float TimeToLive { get; private set; }

	// Token: 0x060011DD RID: 4573 RVA: 0x00059378 File Offset: 0x00057578
	public IFightDecal SpawnDecal(Vector3 position, Direction orientation, string customDeathEffectId = "")
	{
		Vector3 vector = RaycastUtils.TrySnapToTheGround(position, 1f, 10f);
		vector += VisualConsts.GetLayerOffset(2);
		base.transform.position = vector;
		this.Rotate(orientation);
		this.Lifetime = 0f;
		this.scalingProgress = 0f;
		DOTween.To(() => this.scalingProgress, delegate(float x)
		{
			this.scalingProgress = x;
			base.transform.localScale = Vector3.one * this.scaleCurve.Evaluate(this.scalingProgress);
		}, this.scaleDuration * 0.1f, this.scaleDuration);
		return this;
	}

	// Token: 0x060011DE RID: 4574 RVA: 0x00059400 File Offset: 0x00057600
	private void Rotate(Direction direction)
	{
		Vector2 vector = direction.ConvertToVector2XZ();
		base.transform.rotation = Quaternion.Euler(0f, Mathf.Atan2(vector.y, vector.x) * 57.29578f, 0f);
		switch (direction)
		{
		case Direction.Right:
		case Direction.Left:
			base.transform.localScale = new Vector3(1.25f, 1f, 0.8f);
			return;
		case Direction.Up:
		case Direction.Down:
			base.transform.localScale = Vector3.one;
			return;
		default:
			return;
		}
	}

	// Token: 0x060011DF RID: 4575 RVA: 0x0005948E File Offset: 0x0005768E
	private void Awake()
	{
		this.TimeToLive = LazySingletonSO<GlobalResources>.Instance.fighting.decalsLifeTime;
	}

	// Token: 0x040013B1 RID: 5041
	private const float X_SCALE = 1.25f;

	// Token: 0x040013B2 RID: 5042
	private const float Z_SCALE = 0.8f;

	// Token: 0x040013B3 RID: 5043
	public const float MAX_FOOTPRINT_SIZE = 1.76f;

	// Token: 0x040013B4 RID: 5044
	public AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	// Token: 0x040013B5 RID: 5045
	public float scaleDuration = 10f;

	// Token: 0x040013B6 RID: 5046
	private float scalingProgress;
}
