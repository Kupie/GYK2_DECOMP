using System;
using UnityEngine;

// Token: 0x02000073 RID: 115
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Distortion/Heat")]
public class CameraFilterPack_Distortion_Heat : MonoBehaviour
{
	// Token: 0x1700006F RID: 111
	// (get) Token: 0x060002D9 RID: 729 RVA: 0x0000FE60 File Offset: 0x0000E060
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

	// Token: 0x060002DA RID: 730 RVA: 0x0000FE94 File Offset: 0x0000E094
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Distortion_Heat");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060002DB RID: 731 RVA: 0x0000FEB8 File Offset: 0x0000E0B8
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
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060002DC RID: 732 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060002DD RID: 733 RVA: 0x0000FF6E File Offset: 0x0000E16E
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000426 RID: 1062
	public Shader SCShader;

	// Token: 0x04000427 RID: 1063
	private float TimeX = 1f;

	// Token: 0x04000428 RID: 1064
	private Vector4 ScreenResolution;

	// Token: 0x04000429 RID: 1065
	private Material SCMaterial;

	// Token: 0x0400042A RID: 1066
	[Range(1f, 100f)]
	public float Distortion = 35f;
}
