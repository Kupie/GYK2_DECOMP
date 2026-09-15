using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000525 RID: 1317
public abstract class GenericSprite : CachedSpriteRenderer
{
	// Token: 0x17000589 RID: 1417
	// (get) Token: 0x060021F0 RID: 8688 RVA: 0x0009F95E File Offset: 0x0009DB5E
	// (set) Token: 0x060021F1 RID: 8689 RVA: 0x00002318 File Offset: 0x00000518
	public int Layer
	{
		get
		{
			return this.layer;
		}
		set
		{
		}
	}

	// Token: 0x060021F2 RID: 8690 RVA: 0x0009F966 File Offset: 0x0009DB66
	protected virtual void Awake()
	{
		this.ApplyMaterial();
	}

	// Token: 0x060021F3 RID: 8691 RVA: 0x0009F970 File Offset: 0x0009DB70
	protected virtual void ApplyMaterial()
	{
		if (this.invisibleSprite)
		{
			base.SpriteRenderer.receiveShadows = false;
			base.SpriteRenderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
			return;
		}
		base.SpriteRenderer.receiveShadows = true;
		base.SpriteRenderer.shadowCastingMode = (this.castShadows ? ShadowCastingMode.On : ShadowCastingMode.Off);
	}

	// Token: 0x04001E92 RID: 7826
	[SerializeField]
	protected bool castShadows;

	// Token: 0x04001E93 RID: 7827
	[SerializeField]
	protected bool invisibleSprite;

	// Token: 0x04001E94 RID: 7828
	[SerializeField]
	protected int layer;

	// Token: 0x04001E95 RID: 7829
	[SerializeField]
	[HideInInspector]
	protected int prevLayer;
}
