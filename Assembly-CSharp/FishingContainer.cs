using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000544 RID: 1348
[ExecuteInEditMode]
public class FishingContainer : MonoBehaviour
{
	// Token: 0x17000597 RID: 1431
	// (get) Token: 0x0600228D RID: 8845 RVA: 0x000A2380 File Offset: 0x000A0580
	public FishUnderwaterGfx FishUnderwaterGfx
	{
		get
		{
			return this.fishUnderwaterGfx;
		}
	}

	// Token: 0x17000598 RID: 1432
	// (get) Token: 0x0600228E RID: 8846 RVA: 0x000A2388 File Offset: 0x000A0588
	public RopeRenderer Rope
	{
		get
		{
			return this.rope;
		}
	}

	// Token: 0x17000599 RID: 1433
	// (get) Token: 0x0600228F RID: 8847 RVA: 0x000A2390 File Offset: 0x000A0590
	public RopePoint Bob
	{
		get
		{
			return this.bob;
		}
	}

	// Token: 0x1700059A RID: 1434
	// (get) Token: 0x06002290 RID: 8848 RVA: 0x000A2398 File Offset: 0x000A0598
	public Transform CastStartPoint
	{
		get
		{
			return this.castStartPoint;
		}
	}

	// Token: 0x1700059B RID: 1435
	// (get) Token: 0x06002291 RID: 8849 RVA: 0x000A23A0 File Offset: 0x000A05A0
	public AnimationComponent AnimationComponent
	{
		get
		{
			return this.animationComponent;
		}
	}

	// Token: 0x1700059C RID: 1436
	// (get) Token: 0x06002292 RID: 8850 RVA: 0x000A23A8 File Offset: 0x000A05A8
	public GameObject SignGameObject
	{
		get
		{
			return this.signGgameObject;
		}
	}

	// Token: 0x06002293 RID: 8851 RVA: 0x000A23B0 File Offset: 0x000A05B0
	private void OnEnable()
	{
		if (this.signGgameObject != null)
		{
			this.signGgameObject.SetActive(false);
		}
	}

	// Token: 0x06002294 RID: 8852 RVA: 0x000A23CC File Offset: 0x000A05CC
	public void ResetColor()
	{
		this.SetRopeColor(LazySingletonSerializedSO<FishingSettings>.Instance.ropeGradient.Evaluate(0f));
	}

	// Token: 0x06002295 RID: 8853 RVA: 0x000A23E8 File Offset: 0x000A05E8
	public void SetRopeColor(Color newColor)
	{
		this.rope.SetColor(newColor);
	}

	// Token: 0x06002296 RID: 8854 RVA: 0x000A23F6 File Offset: 0x000A05F6
	public void SetRopeEmission(float intensity)
	{
		this.rope.SetEmission(intensity);
	}

	// Token: 0x04001F35 RID: 7989
	[SerializeField]
	private RopeRenderer rope;

	// Token: 0x04001F36 RID: 7990
	[SerializeField]
	private Transform castStartPoint;

	// Token: 0x04001F37 RID: 7991
	[SerializeField]
	private RopePoint bob;

	// Token: 0x04001F38 RID: 7992
	[SerializeField]
	private AnimationComponent animationComponent;

	// Token: 0x04001F39 RID: 7993
	[SerializeField]
	private FishUnderwaterGfx fishUnderwaterGfx;

	// Token: 0x04001F3A RID: 7994
	[SerializeField]
	private GameObject signGgameObject;
}
