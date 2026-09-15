using System;
using UnityEngine;

// Token: 0x020004FA RID: 1274
[RequireComponent(typeof(SpriteRenderer))]
[ExecuteAlways]
public class DayNightSprite : DayNightLightBase
{
	// Token: 0x0600212D RID: 8493 RVA: 0x0009C950 File Offset: 0x0009AB50
	private void Awake()
	{
		this.spriteRenderer = base.GetComponent<SpriteRenderer>();
		this.ApplyLightMode(this.mode);
	}

	// Token: 0x0600212E RID: 8494 RVA: 0x0009C96C File Offset: 0x0009AB6C
	protected override void ApplyLightMode(DayNightLightBase.LightMode mode)
	{
		this.mode = mode;
		if (this.spriteRenderer == null)
		{
			this.spriteRenderer = base.GetComponent<SpriteRenderer>();
		}
		if (this.spriteRenderer == null)
		{
			return;
		}
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		this.spriteRenderer.GetPropertyBlock(materialPropertyBlock);
		materialPropertyBlock.SetInteger(DayNightLightBase.idLightMode, (int)mode);
		this.spriteRenderer.SetPropertyBlock(materialPropertyBlock);
		LightFaker lightFaker = base.GetComponent<LightFaker>();
		if (lightFaker == null)
		{
			lightFaker = base.GetComponentInParent<LightFaker>();
		}
		if (lightFaker != null && lightFaker.mode != mode)
		{
			lightFaker.ApplyLightModeInt((int)mode);
		}
	}

	// Token: 0x04001DC3 RID: 7619
	[SerializeField]
	private SpriteRenderer spriteRenderer;
}
