using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.U2D;

// Token: 0x0200050F RID: 1295
[ExecuteInEditMode]
public class Object3D : MonoBehaviour
{
	// Token: 0x17000573 RID: 1395
	// (get) Token: 0x0600216D RID: 8557 RVA: 0x0009DA2A File Offset: 0x0009BC2A
	public SpriteAtlas SpriteAtlas
	{
		get
		{
			return this.spriteAtlas;
		}
	}

	// Token: 0x17000574 RID: 1396
	// (get) Token: 0x0600216E RID: 8558 RVA: 0x0009DA32 File Offset: 0x0009BC32
	// (set) Token: 0x0600216F RID: 8559 RVA: 0x0009DA3A File Offset: 0x0009BC3A
	public float TransparencyValue
	{
		get
		{
			return this.transparencyValue;
		}
		set
		{
			this.transparencyValue = value;
			this.SetTransparency(value);
		}
	}

	// Token: 0x17000575 RID: 1397
	// (get) Token: 0x06002170 RID: 8560 RVA: 0x0009DA4A File Offset: 0x0009BC4A
	public List<Object3DMesh> Object3DMeshes
	{
		get
		{
			return this.object3DMeshes;
		}
	}

	// Token: 0x17000576 RID: 1398
	// (get) Token: 0x06002171 RID: 8561 RVA: 0x0009DA52 File Offset: 0x0009BC52
	public List<HorizontalSprite> HorizontalSprites
	{
		get
		{
			return this.horizontalSprites;
		}
	}

	// Token: 0x17000577 RID: 1399
	// (get) Token: 0x06002172 RID: 8562 RVA: 0x0009DA5A File Offset: 0x0009BC5A
	public List<Collider> CachedColliders
	{
		get
		{
			return this.cachedColliders;
		}
	}

	// Token: 0x17000578 RID: 1400
	// (get) Token: 0x06002173 RID: 8563 RVA: 0x0009DA62 File Offset: 0x0009BC62
	private bool IsATree
	{
		get
		{
			return this.object3DMeshes.Any((Object3DMesh mesh) => mesh != null && mesh.IsATree);
		}
	}

	// Token: 0x06002174 RID: 8564 RVA: 0x0009DA90 File Offset: 0x0009BC90
	public void FixCollidersLossyScale()
	{
		foreach (Collider collider in this.cachedColliders)
		{
			if (!(collider == null) && collider.IsLossyScaleNegative())
			{
				BoxCollider boxCollider = collider as BoxCollider;
				if (boxCollider != null)
				{
					boxCollider.FixBoxColliderLossyScale();
				}
			}
		}
	}

	// Token: 0x06002175 RID: 8565 RVA: 0x0009DB00 File Offset: 0x0009BD00
	public void SetTreeHeightValue(float treeHeight)
	{
		if (!this.IsATree)
		{
			return;
		}
		foreach (Object3DMesh object3DMesh in this.object3DMeshes)
		{
			object3DMesh.treeHeight = treeHeight;
		}
	}

	// Token: 0x06002176 RID: 8566 RVA: 0x0009DB5C File Offset: 0x0009BD5C
	public void SetSelectionTint(Color color, float amount)
	{
		for (int i = 0; i < this.object3DMeshes.Count; i++)
		{
			Object3DMesh object3DMesh = this.object3DMeshes[i];
			if (object3DMesh != null)
			{
				object3DMesh.SetSelectionTint(color, amount);
			}
		}
	}

	// Token: 0x06002177 RID: 8567 RVA: 0x0009DB98 File Offset: 0x0009BD98
	private void Awake()
	{
		this.SetAnimatableState(false);
		this.FixCollidersLossyScale();
	}

	// Token: 0x06002178 RID: 8568 RVA: 0x0009DBA8 File Offset: 0x0009BDA8
	public void SetAnimatableState(bool isAnimatable)
	{
		foreach (Object3DMeshAnimation object3DMeshAnimation in this.meshAnimations)
		{
			object3DMeshAnimation.SetAnimatableState(isAnimatable);
		}
	}

	// Token: 0x06002179 RID: 8569 RVA: 0x0009DBFC File Offset: 0x0009BDFC
	public void ResetToStaticState()
	{
		foreach (Object3DMeshAnimation object3DMeshAnimation in this.meshAnimations)
		{
			object3DMeshAnimation.ReturnToStaticState();
		}
	}

	// Token: 0x0600217A RID: 8570 RVA: 0x0009DC4C File Offset: 0x0009BE4C
	private void SetTransparency(float transparencyValue)
	{
		foreach (Object3DMesh object3DMesh in this.Object3DMeshes)
		{
			object3DMesh.SetTransparency(transparencyValue);
		}
	}

	// Token: 0x04001E02 RID: 7682
	private const string TEXTURE_EXTENSION = ".png";

	// Token: 0x04001E03 RID: 7683
	private const string DEFAULT_HOR_SPRITE_NAME = "New Horizontal Sprite";

	// Token: 0x04001E04 RID: 7684
	private const string DEFAULT_VER_SPRITE_NAME = "New Vertical Sprite";

	// Token: 0x04001E05 RID: 7685
	public List<Object3DMeshData> modelsData = new List<Object3DMeshData>();

	// Token: 0x04001E06 RID: 7686
	public List<Object3DSpriteData> spritesData = new List<Object3DSpriteData>();

	// Token: 0x04001E07 RID: 7687
	[SerializeField]
	private List<Object3DMesh> object3DMeshes = new List<Object3DMesh>();

	// Token: 0x04001E08 RID: 7688
	[SerializeField]
	private List<HorizontalSprite> horizontalSprites = new List<HorizontalSprite>();

	// Token: 0x04001E09 RID: 7689
	[SerializeField]
	private List<VerticalSprite> verticalSprites = new List<VerticalSprite>();

	// Token: 0x04001E0A RID: 7690
	[SerializeField]
	private List<Collider> cachedColliders = new List<Collider>();

	// Token: 0x04001E0B RID: 7691
	private SpriteAtlas spriteAtlas;

	// Token: 0x04001E0C RID: 7692
	[SerializeField]
	[HideInInspector]
	private AssetReferenceAtlasedSprite spriteAtlasReference;

	// Token: 0x04001E0D RID: 7693
	[SerializeField]
	private List<Object3DMeshAnimation> meshAnimations = new List<Object3DMeshAnimation>();

	// Token: 0x04001E0E RID: 7694
	[SerializeField]
	[HideInInspector]
	private Animator animator;

	// Token: 0x04001E0F RID: 7695
	private bool isConstructorPart;

	// Token: 0x04001E10 RID: 7696
	private bool isVisible;

	// Token: 0x04001E11 RID: 7697
	private float transparencyValue;
}
