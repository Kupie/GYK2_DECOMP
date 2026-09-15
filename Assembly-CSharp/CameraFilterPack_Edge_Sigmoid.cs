using System;
using UnityEngine;

// Token: 0x02000098 RID: 152
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Edge/Sigmoid")]
public class CameraFilterPack_Edge_Sigmoid : MonoBehaviour
{
	// Token: 0x17000094 RID: 148
	// (get) Token: 0x060003B7 RID: 951 RVA: 0x00013583 File Offset: 0x00011783
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

	// Token: 0x060003B8 RID: 952 RVA: 0x000135B7 File Offset: 0x000117B7
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Edge_Sigmoid");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060003B9 RID: 953 RVA: 0x000135D8 File Offset: 0x000117D8
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
			this.material.SetFloat("_Gain", this.Gain);
			this.material.SetVector("_ScreenResolution", new Vector2((float)Screen.width, (float)Screen.height));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060003BA RID: 954 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060003BB RID: 955 RVA: 0x00013687 File Offset: 0x00011887
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000522 RID: 1314
	public Shader SCShader;

	// Token: 0x04000523 RID: 1315
	private float TimeX = 1f;

	// Token: 0x04000524 RID: 1316
	private Vector4 ScreenResolution;

	// Token: 0x04000525 RID: 1317
	private Material SCMaterial;

	// Token: 0x04000526 RID: 1318
	[Range(1f, 10f)]
	public float Gain = 3f;
}
