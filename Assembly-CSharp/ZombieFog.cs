using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

// Token: 0x020001D2 RID: 466
public class ZombieFog : MonoBehaviour
{
	// Token: 0x06000BE9 RID: 3049 RVA: 0x0003C2BC File Offset: 0x0003A4BC
	public void DoFade(bool isActive)
	{
		float num = (isActive ? 0f : 1f);
		this.curAlpha = num;
		float num2 = (isActive ? 1f : 0f);
		Tween tween = this.tween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		base.gameObject.SetActive(true);
		this.tween = DOTween.To(() => this.curAlpha, delegate(float x)
		{
			this.curAlpha = x;
			MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
			foreach (MeshRenderer meshRenderer in this.renderers)
			{
				meshRenderer.GetPropertyBlock(materialPropertyBlock);
				Color color = materialPropertyBlock.GetColor(ZombieFog.idFogColor);
				color.a = x;
				materialPropertyBlock.SetColor(ZombieFog.idFogColor, color);
				meshRenderer.SetPropertyBlock(materialPropertyBlock);
			}
			foreach (ParticleSystemRenderer particleSystemRenderer in this.particleRenderers)
			{
				particleSystemRenderer.GetPropertyBlock(materialPropertyBlock);
				Color color2 = materialPropertyBlock.GetColor(ZombieFog.idFogParticlesColor);
				color2.a = x;
				materialPropertyBlock.SetColor(ZombieFog.idFogParticlesColor, color2);
				particleSystemRenderer.SetPropertyBlock(materialPropertyBlock);
			}
		}, num2, this.fadeDuration).OnComplete(delegate
		{
			if (!isActive)
			{
				this.gameObject.SetActive(false);
			}
			this.ResetColors();
		});
	}

	// Token: 0x06000BEA RID: 3050 RVA: 0x0003C367 File Offset: 0x0003A567
	public void ResetActiveState()
	{
		base.gameObject.SetActive(this.initialState);
		this.ResetColors();
	}

	// Token: 0x06000BEB RID: 3051 RVA: 0x0003C380 File Offset: 0x0003A580
	private void ResetColors()
	{
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		for (int i = 0; i < this.renderers.Count; i++)
		{
			MeshRenderer meshRenderer = this.renderers[i];
			meshRenderer.GetPropertyBlock(materialPropertyBlock);
			materialPropertyBlock.SetColor(ZombieFog.idFogColor, this.cachedColorsForObjs[i]);
			meshRenderer.SetPropertyBlock(materialPropertyBlock);
		}
		for (int j = 0; j < this.particleRenderers.Count; j++)
		{
			ParticleSystemRenderer particleSystemRenderer = this.particleRenderers[j];
			particleSystemRenderer.GetPropertyBlock(materialPropertyBlock);
			materialPropertyBlock.SetColor(this.GetColorId(materialPropertyBlock), this.cachedColorsForParticles[j]);
			particleSystemRenderer.SetPropertyBlock(materialPropertyBlock);
		}
	}

	// Token: 0x06000BEC RID: 3052 RVA: 0x0003C424 File Offset: 0x0003A624
	private void Awake()
	{
		if (this.autoFindParticles)
		{
			this.particles = new List<ParticleSystem>(base.GetComponentsInChildren<ParticleSystem>(true));
		}
		using (List<ParticleSystem>.Enumerator enumerator = this.particles.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ParticleSystemRenderer particleSystemRenderer;
				if (enumerator.Current.TryGetComponent<ParticleSystemRenderer>(out particleSystemRenderer) && particleSystemRenderer.enabled)
				{
					this.particleRenderers.Add(particleSystemRenderer);
				}
			}
		}
		foreach (MeshRenderer meshRenderer in this.renderers)
		{
			this.cachedColorsForObjs.Add(meshRenderer.sharedMaterial.GetColor(ZombieFog.idFogColor));
		}
		foreach (ParticleSystemRenderer particleSystemRenderer2 in this.particleRenderers)
		{
			this.cachedColorsForParticles.Add(this.GetColor(particleSystemRenderer2.sharedMaterial));
		}
	}

	// Token: 0x06000BED RID: 3053 RVA: 0x0003C554 File Offset: 0x0003A754
	private int GetColorId(MaterialPropertyBlock propertyBlock)
	{
		if (propertyBlock.HasColor(ZombieFog.idFogParticlesColor))
		{
			return ZombieFog.idFogParticlesColor;
		}
		if (!propertyBlock.HasColor(ZombieFog.idFogParticlesColor2))
		{
			return ZombieFog.idFogParticlesColor3;
		}
		return ZombieFog.idFogParticlesColor2;
	}

	// Token: 0x06000BEE RID: 3054 RVA: 0x0003C581 File Offset: 0x0003A781
	private Color GetColor(Material material)
	{
		if (material.HasColor(ZombieFog.idFogParticlesColor))
		{
			return material.GetColor(ZombieFog.idFogParticlesColor);
		}
		if (!material.HasColor(ZombieFog.idFogParticlesColor2))
		{
			return material.GetColor(ZombieFog.idFogParticlesColor3);
		}
		return material.GetColor(ZombieFog.idFogParticlesColor2);
	}

	// Token: 0x06000BEF RID: 3055 RVA: 0x0003C5C0 File Offset: 0x0003A7C0
	private void Start()
	{
		FightingLevel componentInParent = base.GetComponentInParent<FightingLevel>();
		if (componentInParent != null)
		{
			componentInParent.ZombieFogs.Add(this);
		}
	}

	// Token: 0x06000BF0 RID: 3056 RVA: 0x0003C5EC File Offset: 0x0003A7EC
	private void OnDestroy()
	{
		FightingLevel componentInParent = base.GetComponentInParent<FightingLevel>();
		if (componentInParent)
		{
			componentInParent.ZombieFogs.Remove(this);
		}
	}

	// Token: 0x04000CFF RID: 3327
	private static readonly int idFogColor = Shader.PropertyToID("_FColor");

	// Token: 0x04000D00 RID: 3328
	private static readonly int idFogParticlesColor = Shader.PropertyToID("_Tint");

	// Token: 0x04000D01 RID: 3329
	private static readonly int idFogParticlesColor2 = Shader.PropertyToID("_TintColor");

	// Token: 0x04000D02 RID: 3330
	private static readonly int idFogParticlesColor3 = Shader.PropertyToID("_Color");

	// Token: 0x04000D03 RID: 3331
	public List<MeshRenderer> renderers = new List<MeshRenderer>();

	// Token: 0x04000D04 RID: 3332
	public bool autoFindParticles;

	// Token: 0x04000D05 RID: 3333
	public List<ParticleSystem> particles = new List<ParticleSystem>();

	// Token: 0x04000D06 RID: 3334
	private List<ParticleSystemRenderer> particleRenderers = new List<ParticleSystemRenderer>();

	// Token: 0x04000D07 RID: 3335
	public float fadeDuration = 1f;

	// Token: 0x04000D08 RID: 3336
	public bool initialState = true;

	// Token: 0x04000D09 RID: 3337
	private Tween tween;

	// Token: 0x04000D0A RID: 3338
	private float curAlpha;

	// Token: 0x04000D0B RID: 3339
	private readonly List<Color> cachedColorsForObjs = new List<Color>();

	// Token: 0x04000D0C RID: 3340
	private readonly List<Color> cachedColorsForParticles = new List<Color>();
}
