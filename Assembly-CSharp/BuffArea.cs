using System;
using UnityEngine;

// Token: 0x0200013C RID: 316
public class BuffArea : MonoBehaviour
{
	// Token: 0x0600076F RID: 1903 RVA: 0x00023280 File Offset: 0x00021480
	public void Draw(Texture2D texture, Vector3 position, Vector2 size)
	{
		if (this.propertyBlock == null)
		{
			this.propertyBlock = new MaterialPropertyBlock();
		}
		this.meshRenderer.transform.localScale = new Vector3(size.x / base.transform.parent.localScale.x, size.y / base.transform.parent.localScale.y, 1f);
		this.meshRenderer.transform.position = position;
		this.meshRenderer.transform.localPosition = new Vector3(this.meshRenderer.transform.localPosition.x, this.meshRenderer.transform.localPosition.y, 0f);
		this.propertyBlock.SetTexture(this.shaderIdDataTex, texture);
		this.propertyBlock.SetVector(this.shaderIdObjectScale, this.meshRenderer.transform.lossyScale);
		this.meshRenderer.SetPropertyBlock(this.propertyBlock);
	}

	// Token: 0x0400095C RID: 2396
	[SerializeField]
	private MeshRenderer meshRenderer;

	// Token: 0x0400095D RID: 2397
	private readonly int shaderIdDataTex = Shader.PropertyToID("_DataTex");

	// Token: 0x0400095E RID: 2398
	private readonly int shaderIdObjectScale = Shader.PropertyToID("_ObjectScale");

	// Token: 0x0400095F RID: 2399
	private MaterialPropertyBlock propertyBlock;
}
