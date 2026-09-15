using System;
using UnityEngine;

// Token: 0x0200005A RID: 90
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Colors/HUE_Rotate")]
public class CameraFilterPack_Colors_HUE_Rotate : MonoBehaviour
{
	// Token: 0x17000056 RID: 86
	// (get) Token: 0x06000243 RID: 579 RVA: 0x0000DBBD File Offset: 0x0000BDBD
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

	// Token: 0x06000244 RID: 580 RVA: 0x0000DBF1 File Offset: 0x0000BDF1
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Colors_HUE_Rotate");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000245 RID: 581 RVA: 0x0000DC14 File Offset: 0x0000BE14
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
			this.material.SetFloat("_Speed", this.Speed);
			this.material.SetVector("_ScreenResolution", new Vector2((float)Screen.width, (float)Screen.height));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000246 RID: 582 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000247 RID: 583 RVA: 0x0000DCC3 File Offset: 0x0000BEC3
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400038C RID: 908
	public Shader SCShader;

	// Token: 0x0400038D RID: 909
	private float TimeX = 1f;

	// Token: 0x0400038E RID: 910
	private Vector4 ScreenResolution;

	// Token: 0x0400038F RID: 911
	private Material SCMaterial;

	// Token: 0x04000390 RID: 912
	[Range(1f, 20f)]
	public float Speed = 10f;
}
