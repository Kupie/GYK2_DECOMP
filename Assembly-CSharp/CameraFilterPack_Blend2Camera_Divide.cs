using System;
using UnityEngine;

// Token: 0x02000029 RID: 41
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blend 2 Camera/Divide")]
public class CameraFilterPack_Blend2Camera_Divide : MonoBehaviour
{
	// Token: 0x17000027 RID: 39
	// (get) Token: 0x060000F9 RID: 249 RVA: 0x000079A1 File Offset: 0x00005BA1
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

	// Token: 0x060000FA RID: 250 RVA: 0x000079D8 File Offset: 0x00005BD8
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

	// Token: 0x060000FB RID: 251 RVA: 0x00007A3C File Offset: 0x00005C3C
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

	// Token: 0x060000FC RID: 252 RVA: 0x00007B2C File Offset: 0x00005D2C
	private void OnValidate()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x060000FD RID: 253 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060000FE RID: 254 RVA: 0x00007B2C File Offset: 0x00005D2C
	private void OnEnable()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x060000FF RID: 255 RVA: 0x00007B64 File Offset: 0x00005D64
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

	// Token: 0x040001BD RID: 445
	private string ShaderName = "CameraFilterPack/Blend2Camera_Divide";

	// Token: 0x040001BE RID: 446
	public Shader SCShader;

	// Token: 0x040001BF RID: 447
	public Camera Camera2;

	// Token: 0x040001C0 RID: 448
	private float TimeX = 1f;

	// Token: 0x040001C1 RID: 449
	private Vector4 ScreenResolution;

	// Token: 0x040001C2 RID: 450
	private Material SCMaterial;

	// Token: 0x040001C3 RID: 451
	[Range(0f, 1f)]
	public float SwitchCameraToCamera2;

	// Token: 0x040001C4 RID: 452
	[Range(0f, 1f)]
	public float BlendFX = 0.5f;

	// Token: 0x040001C5 RID: 453
	private RenderTexture Camera2tex;
}
