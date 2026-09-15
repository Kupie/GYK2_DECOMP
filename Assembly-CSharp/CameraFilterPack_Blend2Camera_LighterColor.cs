using System;
using UnityEngine;

// Token: 0x02000030 RID: 48
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blend 2 Camera/LighterColor")]
public class CameraFilterPack_Blend2Camera_LighterColor : MonoBehaviour
{
	// Token: 0x1700002E RID: 46
	// (get) Token: 0x06000130 RID: 304 RVA: 0x000088F1 File Offset: 0x00006AF1
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

	// Token: 0x06000131 RID: 305 RVA: 0x00008928 File Offset: 0x00006B28
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

	// Token: 0x06000132 RID: 306 RVA: 0x0000898C File Offset: 0x00006B8C
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

	// Token: 0x06000133 RID: 307 RVA: 0x00008A7C File Offset: 0x00006C7C
	private void OnValidate()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x06000134 RID: 308 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000135 RID: 309 RVA: 0x00008A7C File Offset: 0x00006C7C
	private void OnEnable()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x06000136 RID: 310 RVA: 0x00008AB4 File Offset: 0x00006CB4
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

	// Token: 0x04000201 RID: 513
	private string ShaderName = "CameraFilterPack/Blend2Camera_LighterColor";

	// Token: 0x04000202 RID: 514
	public Shader SCShader;

	// Token: 0x04000203 RID: 515
	public Camera Camera2;

	// Token: 0x04000204 RID: 516
	private float TimeX = 1f;

	// Token: 0x04000205 RID: 517
	private Vector4 ScreenResolution;

	// Token: 0x04000206 RID: 518
	private Material SCMaterial;

	// Token: 0x04000207 RID: 519
	[Range(0f, 1f)]
	public float SwitchCameraToCamera2;

	// Token: 0x04000208 RID: 520
	[Range(0f, 1f)]
	public float BlendFX = 0.5f;

	// Token: 0x04000209 RID: 521
	private RenderTexture Camera2tex;
}
