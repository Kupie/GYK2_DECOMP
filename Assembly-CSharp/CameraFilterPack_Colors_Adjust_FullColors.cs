using System;
using UnityEngine;

// Token: 0x02000053 RID: 83
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/ColorsAdjust/FullColors")]
public class CameraFilterPack_Colors_Adjust_FullColors : MonoBehaviour
{
	// Token: 0x17000050 RID: 80
	// (get) Token: 0x0600021D RID: 541 RVA: 0x0000CD6D File Offset: 0x0000AF6D
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

	// Token: 0x0600021E RID: 542 RVA: 0x0000CDA1 File Offset: 0x0000AFA1
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Colors_Adjust_FullColors");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600021F RID: 543 RVA: 0x0000CDC4 File Offset: 0x0000AFC4
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
			this.material.SetFloat("_Red_R", this.Red_R / 100f);
			this.material.SetFloat("_Red_G", this.Red_G / 100f);
			this.material.SetFloat("_Red_B", this.Red_B / 100f);
			this.material.SetFloat("_Green_R", this.Green_R / 100f);
			this.material.SetFloat("_Green_G", this.Green_G / 100f);
			this.material.SetFloat("_Green_B", this.Green_B / 100f);
			this.material.SetFloat("_Blue_R", this.Blue_R / 100f);
			this.material.SetFloat("_Blue_G", this.Blue_G / 100f);
			this.material.SetFloat("_Blue_B", this.Blue_B / 100f);
			this.material.SetFloat("_Red_C", this.Red_Constant / 100f);
			this.material.SetFloat("_Green_C", this.Green_Constant / 100f);
			this.material.SetFloat("_Blue_C", this.Blue_Constant / 100f);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000220 RID: 544 RVA: 0x0000CFB4 File Offset: 0x0000B1B4
	private void Update()
	{
		bool isPlaying = Application.isPlaying;
	}

	// Token: 0x06000221 RID: 545 RVA: 0x0000CFBC File Offset: 0x0000B1BC
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400033E RID: 830
	public Shader SCShader;

	// Token: 0x0400033F RID: 831
	private float TimeX = 1f;

	// Token: 0x04000340 RID: 832
	private Vector4 ScreenResolution;

	// Token: 0x04000341 RID: 833
	private Material SCMaterial;

	// Token: 0x04000342 RID: 834
	[Range(-200f, 200f)]
	public float Red_R = 100f;

	// Token: 0x04000343 RID: 835
	[Range(-200f, 200f)]
	public float Red_G;

	// Token: 0x04000344 RID: 836
	[Range(-200f, 200f)]
	public float Red_B;

	// Token: 0x04000345 RID: 837
	[Range(-200f, 200f)]
	public float Red_Constant;

	// Token: 0x04000346 RID: 838
	[Range(-200f, 200f)]
	public float Green_R;

	// Token: 0x04000347 RID: 839
	[Range(-200f, 200f)]
	public float Green_G = 100f;

	// Token: 0x04000348 RID: 840
	[Range(-200f, 200f)]
	public float Green_B;

	// Token: 0x04000349 RID: 841
	[Range(-200f, 200f)]
	public float Green_Constant;

	// Token: 0x0400034A RID: 842
	[Range(-200f, 200f)]
	public float Blue_R;

	// Token: 0x0400034B RID: 843
	[Range(-200f, 200f)]
	public float Blue_G;

	// Token: 0x0400034C RID: 844
	[Range(-200f, 200f)]
	public float Blue_B = 100f;

	// Token: 0x0400034D RID: 845
	[Range(-200f, 200f)]
	public float Blue_Constant;
}
