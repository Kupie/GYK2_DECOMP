using System;
using UnityEngine;

// Token: 0x02000035 RID: 53
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blend 2 Camera/Multiply")]
public class CameraFilterPack_Blend2Camera_Multiply : MonoBehaviour
{
	// Token: 0x17000033 RID: 51
	// (get) Token: 0x06000158 RID: 344 RVA: 0x00009391 File Offset: 0x00007591
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

	// Token: 0x06000159 RID: 345 RVA: 0x000093C8 File Offset: 0x000075C8
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

	// Token: 0x0600015A RID: 346 RVA: 0x0000942C File Offset: 0x0000762C
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

	// Token: 0x0600015B RID: 347 RVA: 0x0000951C File Offset: 0x0000771C
	private void OnValidate()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x0600015C RID: 348 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600015D RID: 349 RVA: 0x0000951C File Offset: 0x0000771C
	private void OnEnable()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x0600015E RID: 350 RVA: 0x00009554 File Offset: 0x00007754
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

	// Token: 0x0400022E RID: 558
	private string ShaderName = "CameraFilterPack/Blend2Camera_Multiply";

	// Token: 0x0400022F RID: 559
	public Shader SCShader;

	// Token: 0x04000230 RID: 560
	public Camera Camera2;

	// Token: 0x04000231 RID: 561
	private float TimeX = 1f;

	// Token: 0x04000232 RID: 562
	private Vector4 ScreenResolution;

	// Token: 0x04000233 RID: 563
	private Material SCMaterial;

	// Token: 0x04000234 RID: 564
	[Range(0f, 1f)]
	public float SwitchCameraToCamera2;

	// Token: 0x04000235 RID: 565
	[Range(0f, 1f)]
	public float BlendFX = 0.5f;

	// Token: 0x04000236 RID: 566
	private RenderTexture Camera2tex;
}
