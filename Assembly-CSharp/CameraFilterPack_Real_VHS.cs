using System;
using UnityEngine;

// Token: 0x020000E4 RID: 228
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/VHS/Real VHS HQ")]
public class CameraFilterPack_Real_VHS : MonoBehaviour
{
	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x06000590 RID: 1424 RVA: 0x0001B4B1 File Offset: 0x000196B1
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

	// Token: 0x06000591 RID: 1425 RVA: 0x0001B4E8 File Offset: 0x000196E8
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Real_VHS");
		this.VHS = Resources.Load("CameraFilterPack_VHS1") as Texture2D;
		this.VHS2 = Resources.Load("CameraFilterPack_VHS2") as Texture2D;
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000592 RID: 1426 RVA: 0x0001B53E File Offset: 0x0001973E
	public static Texture2D GetRTPixels(Texture2D t, RenderTexture rt, int sx, int sy)
	{
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = rt;
		t.ReadPixels(new Rect(0f, 0f, (float)t.width, (float)t.height), 0, 0);
		RenderTexture.active = active;
		return t;
	}

	// Token: 0x06000593 RID: 1427 RVA: 0x0001B578 File Offset: 0x00019778
	private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (this.SCShader != null)
		{
			this.material.SetTexture("VHS", this.VHS);
			this.material.SetTexture("VHS2", this.VHS2);
			this.material.SetFloat("TRACKING", this.TRACKING);
			this.material.SetFloat("JITTER", this.JITTER);
			this.material.SetFloat("GLITCH", this.GLITCH);
			this.material.SetFloat("NOISE", this.NOISE);
			this.material.SetFloat("Brightness", this.Brightness);
			this.material.SetFloat("CONTRAST", 1f - this.Constrast);
			int num = 382;
			int num2 = 576;
			RenderTexture temporary = RenderTexture.GetTemporary(num, num2, 0);
			temporary.filterMode = FilterMode.Trilinear;
			Graphics.Blit(sourceTexture, temporary, this.material);
			Graphics.Blit(temporary, destTexture);
			RenderTexture.ReleaseTemporary(temporary);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000594 RID: 1428 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000595 RID: 1429 RVA: 0x0001B688 File Offset: 0x00019888
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000748 RID: 1864
	public Shader SCShader;

	// Token: 0x04000749 RID: 1865
	private Material SCMaterial;

	// Token: 0x0400074A RID: 1866
	private Texture2D VHS;

	// Token: 0x0400074B RID: 1867
	private Texture2D VHS2;

	// Token: 0x0400074C RID: 1868
	[Range(0f, 1f)]
	public float TRACKING = 0.212f;

	// Token: 0x0400074D RID: 1869
	[Range(0f, 1f)]
	public float JITTER = 1f;

	// Token: 0x0400074E RID: 1870
	[Range(0f, 1f)]
	public float GLITCH = 1f;

	// Token: 0x0400074F RID: 1871
	[Range(0f, 1f)]
	public float NOISE = 1f;

	// Token: 0x04000750 RID: 1872
	[Range(-1f, 1f)]
	public float Brightness;

	// Token: 0x04000751 RID: 1873
	[Range(0f, 1.5f)]
	public float Constrast = 1f;
}
