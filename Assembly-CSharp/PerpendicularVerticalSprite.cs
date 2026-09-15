using System;
using UnityEngine;

// Token: 0x02000527 RID: 1319
[ExecuteAlways]
public class PerpendicularVerticalSprite : VerticalSprite
{
	// Token: 0x060021F6 RID: 8694 RVA: 0x0009F9D1 File Offset: 0x0009DBD1
	protected override void ApplyMaterial()
	{
		this.matProp = new MaterialPropertyBlock();
		this.depthOffset = Vector3.forward * this.shadowDepthCorrection;
		base.ApplyMaterial();
	}

	// Token: 0x060021F7 RID: 8695 RVA: 0x0009F9FC File Offset: 0x0009DBFC
	private void Update()
	{
		Vector3 position = base.transform.position;
		if (position != this.lastPosition)
		{
			this.lastPosition = position;
			base.SpriteRenderer.GetPropertyBlock(this.matProp);
			this.matProp.SetVector(PerpendicularVerticalSprite.idWorldPos, position + this.depthOffset);
			base.SpriteRenderer.SetPropertyBlock(this.matProp);
		}
	}

	// Token: 0x04001E97 RID: 7831
	[SerializeField]
	[Range(-1f, 1f)]
	protected float shadowDepthCorrection;

	// Token: 0x04001E98 RID: 7832
	private MaterialPropertyBlock matProp;

	// Token: 0x04001E99 RID: 7833
	private static readonly int idWorldPos = Shader.PropertyToID("_ObjectWorldPos");

	// Token: 0x04001E9A RID: 7834
	private Vector3 lastPosition;

	// Token: 0x04001E9B RID: 7835
	private Vector3 depthOffset;
}
