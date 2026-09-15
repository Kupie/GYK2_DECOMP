using System;
using UnityEngine;

// Token: 0x02000514 RID: 1300
[ExecuteAlways]
public class Object3DMeshAnimation : MonoBehaviour
{
	// Token: 0x060021A4 RID: 8612 RVA: 0x0009E532 File Offset: 0x0009C732
	public void SetAnimatableState(bool isAnimatable)
	{
		this.isAnimatable = isAnimatable;
		base.enabled = isAnimatable;
	}

	// Token: 0x060021A5 RID: 8613 RVA: 0x0009E544 File Offset: 0x0009C744
	public void ReturnToStaticState()
	{
		if (this.isAnimatable)
		{
			return;
		}
		if (this.cachedStaticTexture == null)
		{
			return;
		}
		this.targetMesh.GetSubMesh(this.submeshIndex).SetTextureOnly(this.cachedStaticTexture);
		this.cachedStaticTexture = null;
		this.prevTexture = null;
		base.enabled = false;
	}

	// Token: 0x060021A6 RID: 8614 RVA: 0x0009E59C File Offset: 0x0009C79C
	private void LateUpdate()
	{
		if (!this.isAnimatable)
		{
			return;
		}
		if (this.texture == null)
		{
			return;
		}
		if (this.texture == this.prevTexture)
		{
			return;
		}
		this.prevTexture = this.texture;
		Object3DSubMesh subMesh = this.targetMesh.GetSubMesh(this.submeshIndex);
		if (subMesh == null)
		{
			return;
		}
		if (!this.cachedStaticTexture)
		{
			this.cachedStaticTexture = subMesh.Texture;
		}
		subMesh.SetTextureOnly(this.texture);
	}

	// Token: 0x04001E3A RID: 7738
	public Object3DMesh targetMesh;

	// Token: 0x04001E3B RID: 7739
	public int submeshIndex;

	// Token: 0x04001E3C RID: 7740
	public Texture2D texture;

	// Token: 0x04001E3D RID: 7741
	private Texture2D prevTexture;

	// Token: 0x04001E3E RID: 7742
	private Texture2D cachedStaticTexture;

	// Token: 0x04001E3F RID: 7743
	private bool isAnimatable;
}
