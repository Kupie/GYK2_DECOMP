using System;
using UnityEngine;

// Token: 0x020000CB RID: 203
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Light/Rainbow2")]
public class CameraFilterPack_Light_Rainbow2 : MonoBehaviour
{
	// Token: 0x170000C8 RID: 200
	// (get) Token: 0x060004EA RID: 1258 RVA: 0x000180E2 File Offset: 0x000162E2
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

	// Token: 0x060004EB RID: 1259 RVA: 0x00018116 File Offset: 0x00016316
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Light_Rainbow2");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060004EC RID: 1260 RVA: 0x00018138 File Offset: 0x00016338
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
			this.material.SetFloat("_Value", this.Value);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060004ED RID: 1261 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060004EE RID: 1262 RVA: 0x000181EE File Offset: 0x000163EE
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000675 RID: 1653
	public Shader SCShader;

	// Token: 0x04000676 RID: 1654
	private float TimeX = 1f;

	// Token: 0x04000677 RID: 1655
	private Vector4 ScreenResolution;

	// Token: 0x04000678 RID: 1656
	private Material SCMaterial;

	// Token: 0x04000679 RID: 1657
	[Range(0.01f, 5f)]
	public float Value = 1.5f;
}
