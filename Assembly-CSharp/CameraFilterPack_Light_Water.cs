using System;
using UnityEngine;

// Token: 0x020000CC RID: 204
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Light/Water")]
public class CameraFilterPack_Light_Water : MonoBehaviour
{
	// Token: 0x170000C9 RID: 201
	// (get) Token: 0x060004F0 RID: 1264 RVA: 0x00018226 File Offset: 0x00016426
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

	// Token: 0x060004F1 RID: 1265 RVA: 0x0001825A File Offset: 0x0001645A
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Light_Water");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060004F2 RID: 1266 RVA: 0x0001827C File Offset: 0x0001647C
	private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (this.SCShader != null)
		{
			this.TimeX += Time.deltaTime * this.Speed;
			if (this.TimeX > 100f)
			{
				this.TimeX = 0f;
			}
			this.material.SetFloat("_TimeX", this.TimeX);
			this.material.SetFloat("_Alpha", this.Alpha);
			this.material.SetFloat("_Distance", this.Distance);
			this.material.SetFloat("_Size", this.Size);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060004F3 RID: 1267 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060004F4 RID: 1268 RVA: 0x00018365 File Offset: 0x00016565
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400067A RID: 1658
	public Shader SCShader;

	// Token: 0x0400067B RID: 1659
	private float TimeX = 1f;

	// Token: 0x0400067C RID: 1660
	private Vector4 ScreenResolution;

	// Token: 0x0400067D RID: 1661
	private Material SCMaterial;

	// Token: 0x0400067E RID: 1662
	[Range(0f, 1f)]
	public float Size = 4f;

	// Token: 0x0400067F RID: 1663
	[Range(0f, 2f)]
	public float Alpha = 0.07f;

	// Token: 0x04000680 RID: 1664
	[Range(0f, 32f)]
	public float Distance = 10f;

	// Token: 0x04000681 RID: 1665
	[Range(-2f, 2f)]
	public float Speed = 0.4f;
}
