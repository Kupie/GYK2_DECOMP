using System;
using UnityEngine;

// Token: 0x02000076 RID: 118
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Distortion/ShockWave")]
public class CameraFilterPack_Distortion_ShockWave : MonoBehaviour
{
	// Token: 0x17000072 RID: 114
	// (get) Token: 0x060002EB RID: 747 RVA: 0x0001024B File Offset: 0x0000E44B
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

	// Token: 0x060002EC RID: 748 RVA: 0x0001027F File Offset: 0x0000E47F
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Distortion_ShockWave");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060002ED RID: 749 RVA: 0x000102A0 File Offset: 0x0000E4A0
	private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (this.SCShader != null)
		{
			this.TimeX += Time.deltaTime;
			if (this.TimeX > 100f)
			{
				this.TimeX = 0f;
			}
			this.material.SetFloat("_TimeX", this.TimeX);
			this.material.SetFloat("_Value", this.PosX);
			this.material.SetFloat("_Value2", this.PosY);
			this.material.SetFloat("_Value3", this.Speed);
			this.material.SetFloat("_Value4", this.Size);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060002EE RID: 750 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060002EF RID: 751 RVA: 0x00010398 File Offset: 0x0000E598
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000437 RID: 1079
	public Shader SCShader;

	// Token: 0x04000438 RID: 1080
	private float TimeX = 1f;

	// Token: 0x04000439 RID: 1081
	private Vector4 ScreenResolution;

	// Token: 0x0400043A RID: 1082
	private Material SCMaterial;

	// Token: 0x0400043B RID: 1083
	[Range(-1.5f, 1.5f)]
	public float PosX = 0.5f;

	// Token: 0x0400043C RID: 1084
	[Range(-1.5f, 1.5f)]
	public float PosY = 0.5f;

	// Token: 0x0400043D RID: 1085
	[Range(0f, 10f)]
	public float Speed = 1f;

	// Token: 0x0400043E RID: 1086
	[Range(0f, 10f)]
	private float Size = 1f;
}
