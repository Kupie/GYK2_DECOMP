using System;
using UnityEngine;

// Token: 0x02000077 RID: 119
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Distortion/ShockWave Manual")]
public class CameraFilterPack_Distortion_ShockWaveManual : MonoBehaviour
{
	// Token: 0x17000073 RID: 115
	// (get) Token: 0x060002F1 RID: 753 RVA: 0x000103F1 File Offset: 0x0000E5F1
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

	// Token: 0x060002F2 RID: 754 RVA: 0x00010425 File Offset: 0x0000E625
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Distortion_ShockWaveManual");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060002F3 RID: 755 RVA: 0x00010448 File Offset: 0x0000E648
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
			this.material.SetFloat("_Value3", this.Value);
			this.material.SetFloat("_Value4", this.Size);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060002F4 RID: 756 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060002F5 RID: 757 RVA: 0x00010540 File Offset: 0x0000E740
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400043F RID: 1087
	public Shader SCShader;

	// Token: 0x04000440 RID: 1088
	private float TimeX = 1f;

	// Token: 0x04000441 RID: 1089
	private Vector4 ScreenResolution;

	// Token: 0x04000442 RID: 1090
	private Material SCMaterial;

	// Token: 0x04000443 RID: 1091
	[Range(-1.5f, 1.5f)]
	public float PosX = 0.5f;

	// Token: 0x04000444 RID: 1092
	[Range(-1.5f, 1.5f)]
	public float PosY = 0.5f;

	// Token: 0x04000445 RID: 1093
	[Range(-0.1f, 2f)]
	public float Value = 0.5f;

	// Token: 0x04000446 RID: 1094
	[Range(0f, 10f)]
	public float Size = 1f;
}
