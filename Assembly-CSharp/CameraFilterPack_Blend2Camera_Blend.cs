using System;
using UnityEngine;

// Token: 0x02000020 RID: 32
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blend 2 Camera/Blend")]
public class CameraFilterPack_Blend2Camera_Blend : MonoBehaviour
{
	// Token: 0x1700001E RID: 30
	// (get) Token: 0x060000B2 RID: 178 RVA: 0x000065B6 File Offset: 0x000047B6
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

	// Token: 0x060000B3 RID: 179 RVA: 0x000065EC File Offset: 0x000047EC
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

	// Token: 0x060000B4 RID: 180 RVA: 0x00006650 File Offset: 0x00004850
	private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (this.SCShader != null)
		{
			this.TimeX += Time.deltaTime;
			if (this.TimeX > 100f)
			{
				this.TimeX = 0f;
			}
			this.material.SetTexture("_MainTex2", this.Camera2tex);
			this.material.SetFloat("_TimeX", this.TimeX);
			this.material.SetFloat("_Value", this.BlendFX);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x0000671C File Offset: 0x0000491C
	private void OnValidate()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060000B7 RID: 183 RVA: 0x0000671C File Offset: 0x0000491C
	private void OnEnable()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x00006754 File Offset: 0x00004954
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

	// Token: 0x04000162 RID: 354
	private string ShaderName = "CameraFilterPack/Blend2Camera_Blend";

	// Token: 0x04000163 RID: 355
	public Shader SCShader;

	// Token: 0x04000164 RID: 356
	public Camera Camera2;

	// Token: 0x04000165 RID: 357
	private float TimeX = 1f;

	// Token: 0x04000166 RID: 358
	private Vector4 ScreenResolution;

	// Token: 0x04000167 RID: 359
	private Material SCMaterial;

	// Token: 0x04000168 RID: 360
	[Range(0f, 1f)]
	public float BlendFX = 0.5f;

	// Token: 0x04000169 RID: 361
	private RenderTexture Camera2tex;
}
