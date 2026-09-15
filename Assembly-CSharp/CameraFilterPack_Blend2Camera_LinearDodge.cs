using System;
using UnityEngine;

// Token: 0x02000032 RID: 50
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blend 2 Camera/LinearDodge")]
public class CameraFilterPack_Blend2Camera_LinearDodge : MonoBehaviour
{
	// Token: 0x17000030 RID: 48
	// (get) Token: 0x06000140 RID: 320 RVA: 0x00008D31 File Offset: 0x00006F31
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

	// Token: 0x06000141 RID: 321 RVA: 0x00008D68 File Offset: 0x00006F68
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

	// Token: 0x06000142 RID: 322 RVA: 0x00008DCC File Offset: 0x00006FCC
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

	// Token: 0x06000143 RID: 323 RVA: 0x00008EBC File Offset: 0x000070BC
	private void OnValidate()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x06000144 RID: 324 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000145 RID: 325 RVA: 0x00008EBC File Offset: 0x000070BC
	private void OnEnable()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x06000146 RID: 326 RVA: 0x00008EF4 File Offset: 0x000070F4
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

	// Token: 0x04000213 RID: 531
	private string ShaderName = "CameraFilterPack/Blend2Camera_LinearDodge";

	// Token: 0x04000214 RID: 532
	public Shader SCShader;

	// Token: 0x04000215 RID: 533
	public Camera Camera2;

	// Token: 0x04000216 RID: 534
	private float TimeX = 1f;

	// Token: 0x04000217 RID: 535
	private Vector4 ScreenResolution;

	// Token: 0x04000218 RID: 536
	private Material SCMaterial;

	// Token: 0x04000219 RID: 537
	[Range(0f, 1f)]
	public float SwitchCameraToCamera2;

	// Token: 0x0400021A RID: 538
	[Range(0f, 1f)]
	public float BlendFX = 0.5f;

	// Token: 0x0400021B RID: 539
	private RenderTexture Camera2tex;
}
