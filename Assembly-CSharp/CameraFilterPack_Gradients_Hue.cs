using System;
using UnityEngine;

// Token: 0x020000C4 RID: 196
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Gradients/Hue")]
public class CameraFilterPack_Gradients_Hue : MonoBehaviour
{
	// Token: 0x170000C1 RID: 193
	// (get) Token: 0x060004C0 RID: 1216 RVA: 0x000176FE File Offset: 0x000158FE
	private Material material
	{
		get
		{
			if (this.SCMaterial == null)
			{
				this.SCMaterial = new Material(this.SCShader);
				this.SCMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return this.SCMaterial;
		}
	}

	// Token: 0x060004C1 RID: 1217 RVA: 0x00017732 File Offset: 0x00015932
	private void Start()
	{
		this.SCShader = Shader.Find(this.ShaderName);
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060004C2 RID: 1218 RVA: 0x00017754 File Offset: 0x00015954
	private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (this.SCShader != null)
		{
			this.TimeX += Time.deltaTime;
			if (this.TimeX > 100f)
			{
				this.TimeX = 0f;
			}
			this.material.SetFloat("_TimeX", this.TimeX);
			this.material.SetFloat("_Value", this.Switch);
			this.material.SetFloat("_Value2", this.Fade);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060004C3 RID: 1219 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060004C4 RID: 1220 RVA: 0x00017820 File Offset: 0x00015A20
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000646 RID: 1606
	public Shader SCShader;

	// Token: 0x04000647 RID: 1607
	private string ShaderName = "CameraFilterPack/Gradients_Hue";

	// Token: 0x04000648 RID: 1608
	private float TimeX = 1f;

	// Token: 0x04000649 RID: 1609
	private Vector4 ScreenResolution;

	// Token: 0x0400064A RID: 1610
	private Material SCMaterial;

	// Token: 0x0400064B RID: 1611
	[Range(0f, 1f)]
	public float Switch = 1f;

	// Token: 0x0400064C RID: 1612
	[Range(0f, 1f)]
	public float Fade = 1f;
}
