using System;
using UnityEngine;

// Token: 0x0200003E RID: 62
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Split Screen/Split 3D")]
public class CameraFilterPack_Blend2Camera_SplitScreen3D : MonoBehaviour
{
	// Token: 0x1700003B RID: 59
	// (get) Token: 0x06000199 RID: 409 RVA: 0x0000A7E0 File Offset: 0x000089E0
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

	// Token: 0x0600019A RID: 410 RVA: 0x0000A814 File Offset: 0x00008A14
	private void OnValidate()
	{
		this.ScreenSize.x = (float)Screen.width;
		this.ScreenSize.y = (float)Screen.height;
	}

	// Token: 0x0600019B RID: 411 RVA: 0x0000A838 File Offset: 0x00008A38
	private void Start()
	{
		if (this.Camera2 != null)
		{
			this.Camera2tex = new RenderTexture((int)this.ScreenSize.x, (int)this.ScreenSize.y, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
		this.SCShader = Shader.Find(this.ShaderName);
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600019C RID: 412 RVA: 0x0000A8AC File Offset: 0x00008AAC
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
			this.material.SetFloat("_Near", this._Distance);
			this.material.SetFloat("_Far", this._Size);
			this.material.SetFloat("_FixDistance", this._FixDistance);
			this.material.SetFloat("_TimeX", this.TimeX);
			this.material.SetFloat("_Value", this.BlendFX);
			this.material.SetFloat("_Value2", this.SwitchCameraToCamera2);
			this.material.SetFloat("_Value3", this.SplitX);
			this.material.SetFloat("_Value6", this.SplitY);
			this.material.SetFloat("_Value4", this.Smooth);
			this.material.SetFloat("_Value5", this.Rotation);
			this.material.SetInt("_ForceYSwap", this.ForceYSwap ? 0 : 1);
			base.GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600019D RID: 413 RVA: 0x0000A814 File Offset: 0x00008A14
	private void Update()
	{
		this.ScreenSize.x = (float)Screen.width;
		this.ScreenSize.y = (float)Screen.height;
	}

	// Token: 0x0600019E RID: 414 RVA: 0x0000AA31 File Offset: 0x00008C31
	private void OnEnable()
	{
		this.Start();
	}

	// Token: 0x0600019F RID: 415 RVA: 0x0000AA39 File Offset: 0x00008C39
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

	// Token: 0x04000297 RID: 663
	private string ShaderName = "CameraFilterPack/Blend2Camera_SplitScreen3D";

	// Token: 0x04000298 RID: 664
	public Shader SCShader;

	// Token: 0x04000299 RID: 665
	public Camera Camera2;

	// Token: 0x0400029A RID: 666
	private float TimeX = 1f;

	// Token: 0x0400029B RID: 667
	private Material SCMaterial;

	// Token: 0x0400029C RID: 668
	[Range(0f, 100f)]
	public float _FixDistance = 1f;

	// Token: 0x0400029D RID: 669
	[Range(-0.99f, 0.99f)]
	public float _Distance = 0.5f;

	// Token: 0x0400029E RID: 670
	[Range(0f, 0.5f)]
	public float _Size = 0.1f;

	// Token: 0x0400029F RID: 671
	[Range(0f, 1f)]
	public float SwitchCameraToCamera2;

	// Token: 0x040002A0 RID: 672
	[Range(0f, 1f)]
	public float BlendFX = 1f;

	// Token: 0x040002A1 RID: 673
	[Range(-3f, 3f)]
	public float SplitX = 0.5f;

	// Token: 0x040002A2 RID: 674
	[Range(-3f, 3f)]
	public float SplitY = 0.5f;

	// Token: 0x040002A3 RID: 675
	[Range(0f, 2f)]
	public float Smooth = 0.1f;

	// Token: 0x040002A4 RID: 676
	[Range(-3.14f, 3.14f)]
	public float Rotation = 3.14f;

	// Token: 0x040002A5 RID: 677
	private bool ForceYSwap;

	// Token: 0x040002A6 RID: 678
	private RenderTexture Camera2tex;

	// Token: 0x040002A7 RID: 679
	private Vector2 ScreenSize;
}
