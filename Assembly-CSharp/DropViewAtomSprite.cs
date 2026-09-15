using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200019A RID: 410
public class DropViewAtomSprite : DropViewAtomBase
{
	// Token: 0x06000A69 RID: 2665 RVA: 0x00034986 File Offset: 0x00032B86
	public override void Activate(string iconId)
	{
		this.itemSprite.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(iconId, null);
		base.Activate(iconId);
	}

	// Token: 0x06000A6A RID: 2666 RVA: 0x000349A6 File Offset: 0x00032BA6
	public override SpriteText GetSpriteText()
	{
		return this.spriteText;
	}

	// Token: 0x06000A6B RID: 2667 RVA: 0x000349AE File Offset: 0x00032BAE
	public override void Deactivate()
	{
		this.SetInteractionState(false);
		base.Deactivate();
		this.itemSprite.sprite = null;
	}

	// Token: 0x06000A6C RID: 2668 RVA: 0x000349CC File Offset: 0x00032BCC
	public override void SetInteractionState(bool isUnderInteraction)
	{
		if (this.itemSprite == null)
		{
			return;
		}
		if (this.propertyBlock == null)
		{
			this.propertyBlock = new MaterialPropertyBlock();
		}
		this.itemSprite.GetPropertyBlock(this.propertyBlock);
		this.propertyBlock.SetColor(DropViewAtomSprite.replaceBlueColorId, isUnderInteraction ? Object3DMesh.replaceBlueColor : this.GetDefaultReplaceBlueColor());
		this.itemSprite.SetPropertyBlock(this.propertyBlock);
	}

	// Token: 0x06000A6D RID: 2669 RVA: 0x00034A40 File Offset: 0x00032C40
	private Color GetDefaultReplaceBlueColor()
	{
		Material sharedMaterial = this.itemSprite.sharedMaterial;
		if (sharedMaterial != null && sharedMaterial.HasProperty(DropViewAtomSprite.replaceBlueColorId))
		{
			return sharedMaterial.GetColor(DropViewAtomSprite.replaceBlueColorId);
		}
		return Object3DMesh.replaceBlueTransparent;
	}

	// Token: 0x04000BD5 RID: 3029
	private static readonly int replaceBlueColorId = Shader.PropertyToID("_ReplaceBlueColor");

	// Token: 0x04000BD6 RID: 3030
	[SerializeField]
	private SpriteRenderer itemSprite;

	// Token: 0x04000BD7 RID: 3031
	[SerializeField]
	private SpriteText spriteText;

	// Token: 0x04000BD8 RID: 3032
	private MaterialPropertyBlock propertyBlock;
}
