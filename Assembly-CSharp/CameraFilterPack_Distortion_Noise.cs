using System;
using UnityEngine;

// Token: 0x02000075 RID: 117
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Distortion/Noise")]
public class CameraFilterPack_Distortion_Noise : MonoBehaviour
{
	// Token: 0x17000071 RID: 113
	// (get) Token: 0x060002E5 RID: 741 RVA: 0x0001010F File Offset: 0x0000E30F
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

	// Token: 0x060002E6 RID: 742 RVA: 0x00010143 File Offset: 0x0000E343
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Distortion_Noise");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060002E7 RID: 743 RVA: 0x00010164 File Offset: 0x0000E364
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
			this.material.SetFloat("_Distortion", this.Distortion);
			this.material.SetVector("_ScreenResolution", new Vector2((float)Screen.width, (float)Screen.height));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060002E8 RID: 744 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060002E9 RID: 745 RVA: 0x00010213 File Offset: 0x0000E413
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000432 RID: 1074
	public Shader SCShader;

	// Token: 0x04000433 RID: 1075
	private float TimeX = 1f;

	// Token: 0x04000434 RID: 1076
	private Vector4 ScreenResolution;

	// Token: 0x04000435 RID: 1077
	private Material SCMaterial;

	// Token: 0x04000436 RID: 1078
	[Range(0f, 3f)]
	public float Distortion = 1f;
}
