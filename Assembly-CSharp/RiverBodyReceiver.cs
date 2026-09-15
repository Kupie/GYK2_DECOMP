using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

// Token: 0x0200067E RID: 1662
public class RiverBodyReceiver : MonoBehaviour
{
	// Token: 0x170006D9 RID: 1753
	// (get) Token: 0x06002C02 RID: 11266 RVA: 0x000D02DD File Offset: 0x000CE4DD
	public Wgo Wgo
	{
		get
		{
			return this.wgo;
		}
	}

	// Token: 0x170006DA RID: 1754
	// (get) Token: 0x06002C03 RID: 11267 RVA: 0x000D02E5 File Offset: 0x000CE4E5
	public SplineContainer SplineContainer
	{
		get
		{
			return this.splineContainer;
		}
	}

	// Token: 0x170006DB RID: 1755
	// (get) Token: 0x06002C04 RID: 11268 RVA: 0x000D02ED File Offset: 0x000CE4ED
	public AnimationCurve ThrowHeightCurve
	{
		get
		{
			return this.throwHeightCurve;
		}
	}

	// Token: 0x170006DC RID: 1756
	// (get) Token: 0x06002C05 RID: 11269 RVA: 0x000D02F5 File Offset: 0x000CE4F5
	public float ThrowDuration
	{
		get
		{
			return this.throwDuration;
		}
	}

	// Token: 0x170006DD RID: 1757
	// (get) Token: 0x06002C06 RID: 11270 RVA: 0x000D02FD File Offset: 0x000CE4FD
	public float FlowSpeed
	{
		get
		{
			return this.flowSpeed;
		}
	}

	// Token: 0x170006DE RID: 1758
	// (get) Token: 0x06002C07 RID: 11271 RVA: 0x000D0305 File Offset: 0x000CE505
	public float FallSpeed
	{
		get
		{
			return this.fallSpeed;
		}
	}

	// Token: 0x170006DF RID: 1759
	// (get) Token: 0x06002C08 RID: 11272 RVA: 0x000D030D File Offset: 0x000CE50D
	// (set) Token: 0x06002C09 RID: 11273 RVA: 0x000D0315 File Offset: 0x000CE515
	public bool UseDynamicBubble { get; private set; }

	// Token: 0x170006E0 RID: 1760
	// (get) Token: 0x06002C0A RID: 11274 RVA: 0x000D031E File Offset: 0x000CE51E
	public int SplineCount
	{
		get
		{
			if (!(this.splineContainer != null))
			{
				return 0;
			}
			return this.splineContainer.Splines.Count;
		}
	}

	// Token: 0x170006E1 RID: 1761
	// (get) Token: 0x06002C0B RID: 11275 RVA: 0x000D0340 File Offset: 0x000CE540
	public bool HasFlowSpline
	{
		get
		{
			if (this.SplineCount == 0)
			{
				return false;
			}
			Spline spline = this.splineContainer.Splines[0];
			return spline != null && spline.Count >= 2;
		}
	}

	// Token: 0x06002C0C RID: 11276 RVA: 0x000D037A File Offset: 0x000CE57A
	public void Init(Wgo owner)
	{
		if (owner == null)
		{
			return;
		}
		this.wgo = owner;
		this.RecacheSplineLengths();
		MainGame instance = MainGame.Instance;
		if (instance == null)
		{
			return;
		}
		RiverDropSystem riverDropSystem = instance.riverDropSystem;
		if (riverDropSystem == null)
		{
			return;
		}
		riverDropSystem.HandleReceiverReady(this);
	}

	// Token: 0x06002C0D RID: 11277 RVA: 0x000D03AD File Offset: 0x000CE5AD
	public void DeInit()
	{
		this.UseDynamicBubble = false;
		this.wgo = null;
	}

	// Token: 0x06002C0E RID: 11278 RVA: 0x000D03BD File Offset: 0x000CE5BD
	public void SetDynamicBubbleEnabled(bool enabled)
	{
		this.UseDynamicBubble = enabled;
	}

	// Token: 0x06002C0F RID: 11279 RVA: 0x000D03C8 File Offset: 0x000CE5C8
	public float[] CopySplineWorldLengths()
	{
		this.RecacheSplineLengths();
		if (this.cachedSplineWorldLengths == null || this.cachedSplineWorldLengths.Length == 0)
		{
			return Array.Empty<float>();
		}
		float[] array = new float[this.cachedSplineWorldLengths.Length];
		Array.Copy(this.cachedSplineWorldLengths, array, array.Length);
		return array;
	}

	// Token: 0x06002C10 RID: 11280 RVA: 0x000D0410 File Offset: 0x000CE610
	public bool GetNearestFlowPoint(Vector3 worldPos, out Vector3 worldPoint, out float t)
	{
		worldPoint = worldPos;
		t = 0f;
		if (!this.HasFlowSpline)
		{
			return false;
		}
		Spline spline = this.splineContainer.Splines[0];
		if (spline == null || spline.Count < 2)
		{
			return false;
		}
		float3 @float = this.splineContainer.transform.InverseTransformPoint(worldPos);
		float3 float2;
		SplineUtility.GetNearestPoint<Spline>(spline, @float, out float2, out t, 4, 2);
		worldPoint = this.splineContainer.transform.TransformPoint(float2);
		return true;
	}

	// Token: 0x06002C11 RID: 11281 RVA: 0x000D0498 File Offset: 0x000CE698
	public bool TryEvaluatePose(int splineIndex, float t, out Vector3 worldPos)
	{
		worldPos = default(Vector3);
		if (this.splineContainer == null || splineIndex < 0 || splineIndex >= this.splineContainer.Splines.Count)
		{
			return false;
		}
		worldPos = this.splineContainer.EvaluatePosition(splineIndex, Mathf.Clamp01(t));
		return true;
	}

	// Token: 0x06002C12 RID: 11282 RVA: 0x000D04F1 File Offset: 0x000CE6F1
	public Vector3 GetDynamicBubblePos(Vector3 playerWorldPos)
	{
		return playerWorldPos + Vector3.up * this.bubbleHeight;
	}

	// Token: 0x06002C13 RID: 11283 RVA: 0x000D050C File Offset: 0x000CE70C
	private void RecacheSplineLengths()
	{
		if (this.splineContainer == null || this.splineContainer.Splines == null)
		{
			this.cachedSplineWorldLengths = Array.Empty<float>();
			return;
		}
		int count = this.splineContainer.Splines.Count;
		this.cachedSplineWorldLengths = new float[count];
		for (int i = 0; i < count; i++)
		{
			this.cachedSplineWorldLengths[i] = this.splineContainer.CalculateLength(i);
		}
	}

	// Token: 0x04002396 RID: 9110
	[SerializeField]
	private SplineContainer splineContainer;

	// Token: 0x04002397 RID: 9111
	[SerializeField]
	private AnimationCurve throwHeightCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.5f, 1f),
		new Keyframe(1f, 0f)
	});

	// Token: 0x04002398 RID: 9112
	[SerializeField]
	[Min(0.01f)]
	private float throwDuration = 0.45f;

	// Token: 0x04002399 RID: 9113
	[SerializeField]
	[Min(0.01f)]
	private float flowSpeed = 1.5f;

	// Token: 0x0400239A RID: 9114
	[SerializeField]
	[Min(0.01f)]
	private float fallSpeed = 4f;

	// Token: 0x0400239B RID: 9115
	[SerializeField]
	[Min(0f)]
	private float bubbleHeight = 1f;

	// Token: 0x0400239C RID: 9116
	private float[] cachedSplineWorldLengths;

	// Token: 0x0400239D RID: 9117
	private Wgo wgo;
}
