using System;
using UnityEngine;

// Token: 0x02000021 RID: 33
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Chroma Key/BlueScreen")]
public class CameraFilterPack_Blend2Camera_BlueScreen : MonoBehaviour
{
	// Token: 0x1700001F RID: 31
	// (get) Token: 0x060000BA RID: 186 RVA: 0x000067B1 File Offset: 0x000049B1
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

	// Token: 0x060000BB RID: 187 RVA: 0x000067E5 File Offset: 0x000049E5
	private void OnValidate()
	{
		this.ScreenSize.x = (float)Screen.width;
		this.ScreenSize.y = (float)Screen.height;
	}

	// Token: 0x060000BC RID: 188 RVA: 0x0000680C File Offset: 0x00004A0C
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

	// Token: 0x060000BD RID: 189 RVA: 0x00006880 File Offset: 0x00004A80
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
			this.material.SetFloat("_Value2", this.Adjust);
			this.material.SetFloat("_Value3", this.Precision);
			this.material.SetFloat("_Value4", this.Luminosity);
			this.material.SetFloat("_Value5", this.Change_Red);
			this.material.SetFloat("_Value6", this.Change_Green);
			this.material.SetFloat("_Value7", this.Change_Blue);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060000BE RID: 190 RVA: 0x000069B1 File Offset: 0x00004BB1
	private void Update()
	{
		this.ScreenSize.x = (float)Screen.width;
		this.ScreenSize.y = (float)Screen.height;
		bool isPlaying = Application.isPlaying;
	}

	// Token: 0x060000BF RID: 191 RVA: 0x000069DB File Offset: 0x00004BDB
	private void OnEnable()
	{
		this.Start();
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x000069E3 File Offset: 0x00004BE3
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

	// Token: 0x0400016A RID: 362
	private string ShaderName = "CameraFilterPack/Blend2Camera_BlueScreen";

	// Token: 0x0400016B RID: 363
	public Shader SCShader;

	// Token: 0x0400016C RID: 364
	public Camera Camera2;

	// Token: 0x0400016D RID: 365
	private float TimeX = 1f;

	// Token: 0x0400016E RID: 366
	private Material SCMaterial;

	// Token: 0x0400016F RID: 367
	[Range(0f, 1f)]
	public float BlendFX = 1f;

	// Token: 0x04000170 RID: 368
	[Range(-0.2f, 0.2f)]
	public float Adjust;

	// Token: 0x04000171 RID: 369
	[Range(-0.2f, 0.2f)]
	public float Precision;

	// Token: 0x04000172 RID: 370
	[Range(-0.2f, 0.2f)]
	public float Luminosity;

	// Token: 0x04000173 RID: 371
	[Range(-0.3f, 0.3f)]
	public float Change_Red;

	// Token: 0x04000174 RID: 372
	[Range(-0.3f, 0.3f)]
	public float Change_Green;

	// Token: 0x04000175 RID: 373
	[Range(-0.3f, 0.3f)]
	public float Change_Blue;

	// Token: 0x04000176 RID: 374
	private RenderTexture Camera2tex;

	// Token: 0x04000177 RID: 375
	private Vector2 ScreenSize;
}
