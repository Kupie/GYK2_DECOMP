using System;
using UnityEngine;

// Token: 0x020004F9 RID: 1273
[ExecuteAlways]
public class DayNightParticle : DayNightLightBase
{
	// Token: 0x06002128 RID: 8488 RVA: 0x0009C7A1 File Offset: 0x0009A9A1
	private void Awake()
	{
		this.psRenderer = base.GetComponent<ParticleSystemRenderer>();
		this.ApplyLightMode(this.mode);
	}

	// Token: 0x06002129 RID: 8489 RVA: 0x0009C7BC File Offset: 0x0009A9BC
	private void OnEnable()
	{
		if (this.psRenderer == null)
		{
			this.psRenderer = base.GetComponent<ParticleSystemRenderer>();
		}
		this.deferredReapplyFrames = 2;
		this.ApplyLightMode(this.mode);
		this.appliedMaterial = ((this.psRenderer != null) ? this.psRenderer.sharedMaterial : null);
	}

	// Token: 0x0600212A RID: 8490 RVA: 0x0009C818 File Offset: 0x0009AA18
	private void LateUpdate()
	{
		if (this.psRenderer == null)
		{
			return;
		}
		Material sharedMaterial = this.psRenderer.sharedMaterial;
		if (this.deferredReapplyFrames <= 0 && sharedMaterial == this.appliedMaterial)
		{
			return;
		}
		if (this.deferredReapplyFrames > 0)
		{
			this.deferredReapplyFrames--;
		}
		this.ApplyLightMode(this.mode);
		this.appliedMaterial = sharedMaterial;
	}

	// Token: 0x0600212B RID: 8491 RVA: 0x0009C884 File Offset: 0x0009AA84
	protected override void ApplyLightMode(DayNightLightBase.LightMode mode)
	{
		this.mode = mode;
		if (this.psRenderer == null)
		{
			this.psRenderer = base.GetComponent<ParticleSystemRenderer>();
		}
		if (this.psRenderer == null)
		{
			return;
		}
		if (this.propertyBlock == null)
		{
			this.propertyBlock = new MaterialPropertyBlock();
		}
		this.psRenderer.GetPropertyBlock(this.propertyBlock);
		this.propertyBlock.SetInteger(DayNightLightBase.idLightMode, (int)mode);
		this.psRenderer.SetPropertyBlock(this.propertyBlock);
		Material material = (Application.isPlaying ? this.psRenderer.material : this.psRenderer.sharedMaterial);
		if (material != null && !material.IsKeywordEnabled(DayNightLightBase.idSunLightDependencyKeyword))
		{
			material.EnableKeyword(DayNightLightBase.idSunLightDependencyKeyword);
		}
	}

	// Token: 0x04001DBF RID: 7615
	[SerializeField]
	private ParticleSystemRenderer psRenderer;

	// Token: 0x04001DC0 RID: 7616
	private MaterialPropertyBlock propertyBlock;

	// Token: 0x04001DC1 RID: 7617
	private Material appliedMaterial;

	// Token: 0x04001DC2 RID: 7618
	private int deferredReapplyFrames;
}
