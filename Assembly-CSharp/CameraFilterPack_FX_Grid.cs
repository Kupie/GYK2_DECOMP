using System;
using UnityEngine;

// Token: 0x020000B0 RID: 176
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/FX/Grid")]
public class CameraFilterPack_FX_Grid : MonoBehaviour
{
	// Token: 0x170000AD RID: 173
	// (get) Token: 0x06000448 RID: 1096 RVA: 0x00015973 File Offset: 0x00013B73
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

	// Token: 0x06000449 RID: 1097 RVA: 0x000159A7 File Offset: 0x00013BA7
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/FX_Grid");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600044A RID: 1098 RVA: 0x000159C8 File Offset: 0x00013BC8
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
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600044B RID: 1099 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600044C RID: 1100 RVA: 0x00015A4E File Offset: 0x00013C4E
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040005C9 RID: 1481
	public Shader SCShader;

	// Token: 0x040005CA RID: 1482
	private float TimeX = 1f;

	// Token: 0x040005CB RID: 1483
	private Material SCMaterial;

	// Token: 0x040005CC RID: 1484
	[Range(0f, 5f)]
	public float Distortion = 1f;

	// Token: 0x040005CD RID: 1485
	public static float ChangeDistortion;
}
