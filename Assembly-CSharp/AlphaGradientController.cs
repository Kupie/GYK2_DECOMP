using System;
using UnityEngine;

// Token: 0x02000119 RID: 281
[ExecuteAlways]
public class AlphaGradientController : MonoBehaviour
{
	// Token: 0x060006CF RID: 1743 RVA: 0x000207ED File Offset: 0x0001E9ED
	private void OnValidate()
	{
		if (this.targetRenderer == null)
		{
			this.targetRenderer = base.GetComponent<Renderer>();
		}
		if (this.propertyBlock == null)
		{
			this.propertyBlock = new MaterialPropertyBlock();
		}
		this.UpdateMaterialProperties();
	}

	// Token: 0x060006D0 RID: 1744 RVA: 0x00020822 File Offset: 0x0001EA22
	private void OnEnable()
	{
		this.propertyBlock = new MaterialPropertyBlock();
		this.UpdateMaterialProperties();
	}

	// Token: 0x060006D1 RID: 1745 RVA: 0x00020838 File Offset: 0x0001EA38
	private void UpdateMaterialProperties()
	{
		if (this.targetRenderer == null)
		{
			return;
		}
		this.targetRenderer.GetPropertyBlock(this.propertyBlock);
		this.propertyBlock.SetFloat(AlphaGradientController.GradientStart, this.gradientStart);
		this.propertyBlock.SetFloat(AlphaGradientController.GradientEnd, this.gradientEnd);
		this.targetRenderer.SetPropertyBlock(this.propertyBlock);
	}

	// Token: 0x040008C1 RID: 2241
	private static readonly int GradientStart = Shader.PropertyToID("_GradientStart");

	// Token: 0x040008C2 RID: 2242
	private static readonly int GradientEnd = Shader.PropertyToID("_GradientEnd");

	// Token: 0x040008C3 RID: 2243
	[SerializeField]
	private Renderer targetRenderer;

	// Token: 0x040008C4 RID: 2244
	[SerializeField]
	[Range(0f, 1f)]
	private float gradientStart;

	// Token: 0x040008C5 RID: 2245
	[SerializeField]
	[Range(0f, 1f)]
	private float gradientEnd = 1f;

	// Token: 0x040008C6 RID: 2246
	private MaterialPropertyBlock propertyBlock;
}
