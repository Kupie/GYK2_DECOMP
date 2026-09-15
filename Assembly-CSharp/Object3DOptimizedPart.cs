using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

// Token: 0x02000517 RID: 1303
[RequireComponent(typeof(MeshRenderer))]
public class Object3DOptimizedPart : MonoBehaviour
{
	// Token: 0x060021AE RID: 8622 RVA: 0x0009E720 File Offset: 0x0009C920
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void ResetStaticCaches()
	{
		Object3DOptimizedPart.atlasTextureCache.Clear();
	}

	// Token: 0x060021AF RID: 8623 RVA: 0x0009E720 File Offset: 0x0009C920
	public static void ClearAtlasTextureCache()
	{
		Object3DOptimizedPart.atlasTextureCache.Clear();
	}

	// Token: 0x060021B0 RID: 8624 RVA: 0x0009E72C File Offset: 0x0009C92C
	public void SetSourceAtlas(SpriteAtlas atlas)
	{
		this.sourceAtlas = atlas;
		this.cachedAtlasTexture = null;
	}

	// Token: 0x060021B1 RID: 8625 RVA: 0x0009E73C File Offset: 0x0009C93C
	public void SetLutTexture(Texture2D texture)
	{
		this.lutTexture = texture;
	}

	// Token: 0x060021B2 RID: 8626 RVA: 0x0009E748 File Offset: 0x0009C948
	public void ApplyPropertyBlock()
	{
		if (this.cachedRenderer == null)
		{
			this.cachedRenderer = base.GetComponent<Renderer>();
		}
		if (this.cachedRenderer == null)
		{
			return;
		}
		if (!this.cachedAtlasTexture)
		{
			this.cachedAtlasTexture = Object3DOptimizedPart.ResolveAtlasTexture(this.sourceAtlas);
		}
		if (!this.cachedAtlasTexture)
		{
			return;
		}
		if (this.mpb == null)
		{
			this.mpb = new MaterialPropertyBlock();
		}
		this.cachedRenderer.GetPropertyBlock(this.mpb);
		this.mpb.SetTexture(Object3DOptimizedPart.matIdMainTexture, this.cachedAtlasTexture);
		if (this.lutTexture != null)
		{
			this.mpb.SetTexture(Object3DOptimizedPart.matIdLutTexture, this.lutTexture);
		}
		this.cachedRenderer.SetPropertyBlock(this.mpb);
	}

	// Token: 0x060021B3 RID: 8627 RVA: 0x0009E819 File Offset: 0x0009CA19
	private void Awake()
	{
		this.ApplyPropertyBlock();
	}

	// Token: 0x060021B4 RID: 8628 RVA: 0x0009E824 File Offset: 0x0009CA24
	private static Texture2D ResolveAtlasTexture(SpriteAtlas atlas)
	{
		if (atlas == null || atlas.spriteCount == 0)
		{
			return null;
		}
		string tag = atlas.tag;
		Texture2D texture2D;
		if (Object3DOptimizedPart.atlasTextureCache.TryGetValue(tag, out texture2D))
		{
			if (texture2D)
			{
				return texture2D;
			}
			Object3DOptimizedPart.atlasTextureCache.Remove(tag);
		}
		Sprite[] array = new Sprite[atlas.spriteCount];
		atlas.GetSprites(array);
		foreach (Sprite sprite in array)
		{
			if (sprite != null && sprite.texture)
			{
				Object3DOptimizedPart.atlasTextureCache[tag] = sprite.texture;
				return sprite.texture;
			}
		}
		return null;
	}

	// Token: 0x04001E46 RID: 7750
	public bool isShadow;

	// Token: 0x04001E47 RID: 7751
	[SerializeField]
	private SpriteAtlas sourceAtlas;

	// Token: 0x04001E48 RID: 7752
	[SerializeField]
	private Texture2D lutTexture;

	// Token: 0x04001E49 RID: 7753
	private static readonly int matIdMainTexture = Shader.PropertyToID("_MainTex");

	// Token: 0x04001E4A RID: 7754
	private static readonly int matIdLutTexture = Shader.PropertyToID("_ReplaceLUT");

	// Token: 0x04001E4B RID: 7755
	private Texture2D cachedAtlasTexture;

	// Token: 0x04001E4C RID: 7756
	private static readonly Dictionary<string, Texture2D> atlasTextureCache = new Dictionary<string, Texture2D>();

	// Token: 0x04001E4D RID: 7757
	private MaterialPropertyBlock mpb;

	// Token: 0x04001E4E RID: 7758
	private Renderer cachedRenderer;
}
