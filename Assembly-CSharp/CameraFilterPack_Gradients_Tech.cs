using System;
using UnityEngine;

// Token: 0x020000C8 RID: 200
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Gradients/Tech")]
public class CameraFilterPack_Gradients_Tech : MonoBehaviour
{
	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x060004D8 RID: 1240 RVA: 0x00017CBE File Offset: 0x00015EBE
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

	// Token: 0x060004D9 RID: 1241 RVA: 0x00017CF2 File Offset: 0x00015EF2
	private void Start()
	{
		this.SCShader = Shader.Find(this.ShaderName);
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060004DA RID: 1242 RVA: 0x00017D14 File Offset: 0x00015F14
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

	// Token: 0x060004DB RID: 1243 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060004DC RID: 1244 RVA: 0x00017DE0 File Offset: 0x00015FE0
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000662 RID: 1634
	public Shader SCShader;

	// Token: 0x04000663 RID: 1635
	private string ShaderName = "CameraFilterPack/Gradients_Tech";

	// Token: 0x04000664 RID: 1636
	private float TimeX = 1f;

	// Token: 0x04000665 RID: 1637
	private Vector4 ScreenResolution;

	// Token: 0x04000666 RID: 1638
	private Material SCMaterial;

	// Token: 0x04000667 RID: 1639
	[Range(0f, 1f)]
	public float Switch = 1f;

	// Token: 0x04000668 RID: 1640
	[Range(0f, 1f)]
	public float Fade = 1f;
}
