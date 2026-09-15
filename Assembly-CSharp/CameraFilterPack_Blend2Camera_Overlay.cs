using System;
using UnityEngine;

// Token: 0x02000036 RID: 54
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blend 2 Camera/Overlay")]
public class CameraFilterPack_Blend2Camera_Overlay : MonoBehaviour
{
	// Token: 0x17000034 RID: 52
	// (get) Token: 0x06000160 RID: 352 RVA: 0x000095B1 File Offset: 0x000077B1
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

	// Token: 0x06000161 RID: 353 RVA: 0x000095E8 File Offset: 0x000077E8
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

	// Token: 0x06000162 RID: 354 RVA: 0x0000964C File Offset: 0x0000784C
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

	// Token: 0x06000163 RID: 355 RVA: 0x0000973C File Offset: 0x0000793C
	private void OnValidate()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x06000164 RID: 356 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000165 RID: 357 RVA: 0x0000973C File Offset: 0x0000793C
	private void OnEnable()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	// Token: 0x06000166 RID: 358 RVA: 0x00009774 File Offset: 0x00007974
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

	// Token: 0x04000237 RID: 567
	private string ShaderName = "CameraFilterPack/Blend2Camera_Overlay";

	// Token: 0x04000238 RID: 568
	public Shader SCShader;

	// Token: 0x04000239 RID: 569
	public Camera Camera2;

	// Token: 0x0400023A RID: 570
	private float TimeX = 1f;

	// Token: 0x0400023B RID: 571
	private Vector4 ScreenResolution;

	// Token: 0x0400023C RID: 572
	private Material SCMaterial;

	// Token: 0x0400023D RID: 573
	[Range(0f, 1f)]
	public float SwitchCameraToCamera2;

	// Token: 0x0400023E RID: 574
	[Range(0f, 1f)]
	public float BlendFX = 0.5f;

	// Token: 0x0400023F RID: 575
	private RenderTexture Camera2tex;
}
