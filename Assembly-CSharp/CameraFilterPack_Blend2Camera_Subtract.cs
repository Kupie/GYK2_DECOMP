using System;
using UnityEngine;

// Token: 0x0200003F RID: 63
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blend 2 Camera/Subtract")]
public class CameraFilterPack_Blend2Camera_Subtract : MonoBehaviour
{
	// Token: 0x1700003C RID: 60
	// (get) Token: 0x060001A1 RID: 417 RVA: 0x0000AAF1 File Offset: 0x00008CF1
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

	// Token: 0x060001A2 RID: 418 RVA: 0x0000AB28 File Offset: 0x00008D28
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

	// Token: 0x060001A3 RID: 419 RVA: 0x0000AB8C File Offset: 0x00008D8C
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

	// Token: 0x060001A4 RID: 420 RVA: 0x0000AC7C File Offset: 0x00008E7C
	private void OnValidate()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x060001A5 RID: 421 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060001A6 RID: 422 RVA: 0x0000AC7C File Offset: 0x00008E7C
	private void OnEnable()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x060001A7 RID: 423 RVA: 0x0000ACB4 File Offset: 0x00008EB4
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

	// Token: 0x040002A8 RID: 680
	private string ShaderName = "CameraFilterPack/Blend2Camera_Subtract";

	// Token: 0x040002A9 RID: 681
	public Shader SCShader;

	// Token: 0x040002AA RID: 682
	public Camera Camera2;

	// Token: 0x040002AB RID: 683
	private float TimeX = 1f;

	// Token: 0x040002AC RID: 684
	private Vector4 ScreenResolution;

	// Token: 0x040002AD RID: 685
	private Material SCMaterial;

	// Token: 0x040002AE RID: 686
	[Range(0f, 1f)]
	public float SwitchCameraToCamera2;

	// Token: 0x040002AF RID: 687
	[Range(0f, 1f)]
	public float BlendFX = 0.5f;

	// Token: 0x040002B0 RID: 688
	private RenderTexture Camera2tex;
}
