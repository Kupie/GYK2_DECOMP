using System;
using UnityEngine;

// Token: 0x02000095 RID: 149
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Edge/Edge_filter")]
public class CameraFilterPack_Edge_Edge_filter : MonoBehaviour
{
	// Token: 0x17000091 RID: 145
	// (get) Token: 0x060003A5 RID: 933 RVA: 0x000131C7 File Offset: 0x000113C7
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

	// Token: 0x060003A6 RID: 934 RVA: 0x000131FB File Offset: 0x000113FB
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Edge_Edge_filter");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060003A7 RID: 935 RVA: 0x0001321C File Offset: 0x0001141C
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
			this.material.SetFloat("_RedAmplifier", this.RedAmplifier);
			this.material.SetFloat("_GreenAmplifier", this.GreenAmplifier);
			this.material.SetFloat("_BlueAmplifier", this.BlueAmplifier);
			this.material.SetVector("_ScreenResolution", new Vector2((float)Screen.width, (float)Screen.height));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060003A8 RID: 936 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060003A9 RID: 937 RVA: 0x000132F7 File Offset: 0x000114F7
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000512 RID: 1298
	public Shader SCShader;

	// Token: 0x04000513 RID: 1299
	private float TimeX = 1f;

	// Token: 0x04000514 RID: 1300
	private Vector4 ScreenResolution;

	// Token: 0x04000515 RID: 1301
	private Material SCMaterial;

	// Token: 0x04000516 RID: 1302
	[Range(0f, 10f)]
	public float RedAmplifier;

	// Token: 0x04000517 RID: 1303
	[Range(0f, 10f)]
	public float GreenAmplifier = 2f;

	// Token: 0x04000518 RID: 1304
	[Range(0f, 10f)]
	public float BlueAmplifier;
}
