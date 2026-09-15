using System;
using UnityEngine;

// Token: 0x0200003A RID: 58
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blend 2 Camera/Saturation")]
public class CameraFilterPack_Blend2Camera_Saturation : MonoBehaviour
{
	// Token: 0x17000037 RID: 55
	// (get) Token: 0x06000179 RID: 377 RVA: 0x00009EDD File Offset: 0x000080DD
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

	// Token: 0x0600017A RID: 378 RVA: 0x00009F14 File Offset: 0x00008114
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

	// Token: 0x0600017B RID: 379 RVA: 0x00009F78 File Offset: 0x00008178
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

	// Token: 0x0600017C RID: 380 RVA: 0x0000A068 File Offset: 0x00008268
	private void OnValidate()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x0600017D RID: 381 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600017E RID: 382 RVA: 0x0000A068 File Offset: 0x00008268
	private void OnEnable()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x0600017F RID: 383 RVA: 0x0000A0A0 File Offset: 0x000082A0
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

	// Token: 0x0400026E RID: 622
	private string ShaderName = "CameraFilterPack/Blend2Camera_Saturation";

	// Token: 0x0400026F RID: 623
	public Shader SCShader;

	// Token: 0x04000270 RID: 624
	public Camera Camera2;

	// Token: 0x04000271 RID: 625
	private float TimeX = 1f;

	// Token: 0x04000272 RID: 626
	private Vector4 ScreenResolution;

	// Token: 0x04000273 RID: 627
	private Material SCMaterial;

	// Token: 0x04000274 RID: 628
	[Range(0f, 1f)]
	public float SwitchCameraToCamera2;

	// Token: 0x04000275 RID: 629
	[Range(0f, 1f)]
	public float BlendFX = 0.5f;

	// Token: 0x04000276 RID: 630
	private RenderTexture Camera2tex;
}
