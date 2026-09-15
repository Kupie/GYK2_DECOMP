using System;
using UnityEngine;

// Token: 0x02000039 RID: 57
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blend 2 Camera/PinLight")]
public class CameraFilterPack_Blend2Camera_PinLight : MonoBehaviour
{
	// Token: 0x17000036 RID: 54
	// (get) Token: 0x06000171 RID: 369 RVA: 0x00009CC0 File Offset: 0x00007EC0
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

	// Token: 0x06000172 RID: 370 RVA: 0x00009CF4 File Offset: 0x00007EF4
	private void Start()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
		this.SCShader = Shader.Find(this.ShaderName);
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000173 RID: 371 RVA: 0x00009D58 File Offset: 0x00007F58
	private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (this.SCShader != null)
		{
			this.TimeX += Time.deltaTime;
			if (this.TimeX > 100f)
			{
				this.TimeX = 0f;
			}
			if (this.Camera2 != null)
			{
				this.material.SetTexture("_MainTex2", this.Camera2tex);
			}
			this.material.SetFloat("_TimeX", this.TimeX);
			this.material.SetFloat("_Value", this.BlendFX);
			this.material.SetFloat("_Value2", this.SwitchCameraToCamera2);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000174 RID: 372 RVA: 0x00009E48 File Offset: 0x00008048
	private void OnValidate()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x06000175 RID: 373 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000176 RID: 374 RVA: 0x00009E48 File Offset: 0x00008048
	private void OnEnable()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x06000177 RID: 375 RVA: 0x00009E80 File Offset: 0x00008080
	private void OnDisable()
	{
		if (this.Camera2 != null)
		{
			this.Camera2.targetTexture = null;
		}
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000265 RID: 613
	private string ShaderName = "CameraFilterPack/Blend2Camera_PinLight";

	// Token: 0x04000266 RID: 614
	public Shader SCShader;

	// Token: 0x04000267 RID: 615
	public Camera Camera2;

	// Token: 0x04000268 RID: 616
	private float TimeX = 1f;

	// Token: 0x04000269 RID: 617
	private Vector4 ScreenResolution;

	// Token: 0x0400026A RID: 618
	private Material SCMaterial;

	// Token: 0x0400026B RID: 619
	[Range(0f, 1f)]
	public float SwitchCameraToCamera2;

	// Token: 0x0400026C RID: 620
	[Range(0f, 1f)]
	public float BlendFX = 0.5f;

	// Token: 0x0400026D RID: 621
	private RenderTexture Camera2tex;
}
