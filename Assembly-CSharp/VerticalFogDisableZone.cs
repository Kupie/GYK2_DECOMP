using System;
using UnityEngine;

// Token: 0x020001D0 RID: 464
[RequireComponent(typeof(BoxCollider))]
public class VerticalFogDisableZone : MonoBehaviour
{
	// Token: 0x170001FD RID: 509
	// (get) Token: 0x06000BDB RID: 3035 RVA: 0x0003C0B0 File Offset: 0x0003A2B0
	private BoxCollider BoxCollider
	{
		get
		{
			if (!this.boxCollider)
			{
				this.boxCollider = base.GetComponent<BoxCollider>();
			}
			return this.boxCollider;
		}
	}

	// Token: 0x06000BDC RID: 3036 RVA: 0x0003C0D4 File Offset: 0x0003A2D4
	private void Awake()
	{
		Collider[] array = Physics.OverlapBox(this.BoxCollider.bounds.center, this.BoxCollider.bounds.extents);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].GetComponent<PlayerController>() != null)
			{
				this.OnPlayerEnterZone();
				return;
			}
		}
	}

	// Token: 0x06000BDD RID: 3037 RVA: 0x0003C132 File Offset: 0x0003A332
	private void OnValidate()
	{
		this.BoxCollider.isTrigger = true;
	}

	// Token: 0x06000BDE RID: 3038 RVA: 0x0003C140 File Offset: 0x0003A340
	private void OnDrawGizmosSelected()
	{
		this.OnValidate();
	}

	// Token: 0x06000BDF RID: 3039 RVA: 0x0003C148 File Offset: 0x0003A348
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.GetComponent<PlayerController>())
		{
			this.OnPlayerEnterZone();
		}
	}

	// Token: 0x06000BE0 RID: 3040 RVA: 0x0003C162 File Offset: 0x0003A362
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.GetComponent<PlayerController>())
		{
			this.OnPlayerExitZone();
		}
	}

	// Token: 0x06000BE1 RID: 3041 RVA: 0x0003C17C File Offset: 0x0003A37C
	private void OnPlayerEnterZone()
	{
		this.isPlayerInZone = true;
		VerticalFogDisableZone.playerZone = this;
	}

	// Token: 0x06000BE2 RID: 3042 RVA: 0x0003C18B File Offset: 0x0003A38B
	private void OnPlayerExitZone()
	{
		this.isPlayerInZone = false;
		VerticalFogDisableZone.playerZone = null;
	}

	// Token: 0x06000BE3 RID: 3043 RVA: 0x0003C19A File Offset: 0x0003A39A
	private void OnDisable()
	{
		this.OnPlayerExitZone();
	}

	// Token: 0x06000BE4 RID: 3044 RVA: 0x0003C1A4 File Offset: 0x0003A3A4
	private float FindDistanceToClosestBorder()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return float.MaxValue;
		}
		Vector3 position = MainGame.PlayerController.transform.position;
		Bounds bounds = this.BoxCollider.bounds;
		float num = Mathf.Abs(bounds.max.x - position.x);
		float num2 = Mathf.Abs(position.x - bounds.min.x);
		float num3 = Mathf.Abs(bounds.max.z - position.z);
		float num4 = Mathf.Abs(position.z - bounds.min.z);
		return Mathf.Min(new float[] { num, num2, num3, num4 });
	}

	// Token: 0x06000BE5 RID: 3045 RVA: 0x0003C263 File Offset: 0x0003A463
	public static float GetFogDisableAmount()
	{
		if (VerticalFogDisableZone.playerZone == null)
		{
			return 0f;
		}
		VerticalFogDisableZone.normalizedDistance = Mathf.Clamp01(VerticalFogDisableZone.playerZone.FindDistanceToClosestBorder() / VerticalFogDisableZone.playerZone.gradientRange);
		return VerticalFogDisableZone.normalizedDistance;
	}

	// Token: 0x04000CFA RID: 3322
	private BoxCollider boxCollider;

	// Token: 0x04000CFB RID: 3323
	private bool isPlayerInZone;

	// Token: 0x04000CFC RID: 3324
	public float gradientRange = 5f;

	// Token: 0x04000CFD RID: 3325
	private static VerticalFogDisableZone playerZone;

	// Token: 0x04000CFE RID: 3326
	private static float normalizedDistance;
}
