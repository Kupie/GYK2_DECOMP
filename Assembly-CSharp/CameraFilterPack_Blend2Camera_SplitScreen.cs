using System;
using UnityEngine;

// Token: 0x0200003D RID: 61
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Split Screen/SideBySide")]
public class CameraFilterPack_Blend2Camera_SplitScreen : MonoBehaviour
{
	// Token: 0x1700003A RID: 58
	// (get) Token: 0x06000191 RID: 401 RVA: 0x0000A53D File Offset: 0x0000873D
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

	// Token: 0x06000192 RID: 402 RVA: 0x0000A571 File Offset: 0x00008771
	private void OnValidate()
	{
		this.ScreenSize.x = (float)Screen.width;
		this.ScreenSize.y = (float)Screen.height;
	}

	// Token: 0x06000193 RID: 403 RVA: 0x0000A598 File Offset: 0x00008798
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

	// Token: 0x06000194 RID: 404 RVA: 0x0000A60C File Offset: 0x0000880C
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
			this.material.SetFloat("_Value3", this.SplitX);
			this.material.SetFloat("_Value6", this.SplitY);
			this.material.SetFloat("_Value4", this.Smooth);
			this.material.SetFloat("_Value5", this.Rotation);
			this.material.SetInt("_ForceYSwap", this.ForceYSwap ? 0 : 1);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000195 RID: 405 RVA: 0x0000A571 File Offset: 0x00008771
	private void Update()
	{
		this.ScreenSize.x = (float)Screen.width;
		this.ScreenSize.y = (float)Screen.height;
	}

	// Token: 0x06000196 RID: 406 RVA: 0x0000A743 File Offset: 0x00008943
	private void OnEnable()
	{
		this.Start();
	}

	// Token: 0x06000197 RID: 407 RVA: 0x0000A74B File Offset: 0x0000894B
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

	// Token: 0x04000289 RID: 649
	private string ShaderName = "CameraFilterPack/Blend2Camera_SplitScreen";

	// Token: 0x0400028A RID: 650
	public Shader SCShader;

	// Token: 0x0400028B RID: 651
	public Camera Camera2;

	// Token: 0x0400028C RID: 652
	private float TimeX = 1f;

	// Token: 0x0400028D RID: 653
	private Material SCMaterial;

	// Token: 0x0400028E RID: 654
	[Range(0f, 1f)]
	public float SwitchCameraToCamera2;

	// Token: 0x0400028F RID: 655
	[Range(0f, 1f)]
	public float BlendFX = 1f;

	// Token: 0x04000290 RID: 656
	[Range(-3f, 3f)]
	public float SplitX = 0.5f;

	// Token: 0x04000291 RID: 657
	[Range(-3f, 3f)]
	public float SplitY = 0.5f;

	// Token: 0x04000292 RID: 658
	[Range(0f, 2f)]
	public float Smooth = 0.1f;

	// Token: 0x04000293 RID: 659
	[Range(-3.14f, 3.14f)]
	public float Rotation = 3.14f;

	// Token: 0x04000294 RID: 660
	private bool ForceYSwap;

	// Token: 0x04000295 RID: 661
	private RenderTexture Camera2tex;

	// Token: 0x04000296 RID: 662
	private Vector2 ScreenSize;
}
