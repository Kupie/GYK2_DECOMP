using System;
using UnityEngine;

// Token: 0x02000027 RID: 39
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blend 2 Camera/DarkerColor")]
public class CameraFilterPack_Blend2Camera_DarkerColor : MonoBehaviour
{
	// Token: 0x17000025 RID: 37
	// (get) Token: 0x060000E9 RID: 233 RVA: 0x00007561 File Offset: 0x00005761
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

	// Token: 0x060000EA RID: 234 RVA: 0x00007598 File Offset: 0x00005798
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

	// Token: 0x060000EB RID: 235 RVA: 0x000075FC File Offset: 0x000057FC
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

	// Token: 0x060000EC RID: 236 RVA: 0x000076EC File Offset: 0x000058EC
	private void OnValidate()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x060000ED RID: 237 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060000EE RID: 238 RVA: 0x000076EC File Offset: 0x000058EC
	private void OnEnable()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x060000EF RID: 239 RVA: 0x00007724 File Offset: 0x00005924
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

	// Token: 0x040001AB RID: 427
	private string ShaderName = "CameraFilterPack/Blend2Camera_DarkerColor";

	// Token: 0x040001AC RID: 428
	public Shader SCShader;

	// Token: 0x040001AD RID: 429
	public Camera Camera2;

	// Token: 0x040001AE RID: 430
	private float TimeX = 1f;

	// Token: 0x040001AF RID: 431
	private Vector4 ScreenResolution;

	// Token: 0x040001B0 RID: 432
	private Material SCMaterial;

	// Token: 0x040001B1 RID: 433
	[Range(0f, 1f)]
	public float SwitchCameraToCamera2;

	// Token: 0x040001B2 RID: 434
	[Range(0f, 1f)]
	public float BlendFX = 0.5f;

	// Token: 0x040001B3 RID: 435
	private RenderTexture Camera2tex;
}
