using System;
using UnityEngine;

// Token: 0x020000B7 RID: 183
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/FX/Psycho")]
public class CameraFilterPack_FX_Psycho : MonoBehaviour
{
	// Token: 0x170000B4 RID: 180
	// (get) Token: 0x06000472 RID: 1138 RVA: 0x0001620A File Offset: 0x0001440A
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

	// Token: 0x06000473 RID: 1139 RVA: 0x0001623E File Offset: 0x0001443E
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/FX_Psycho");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000474 RID: 1140 RVA: 0x00016260 File Offset: 0x00014460
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

	// Token: 0x06000475 RID: 1141 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000476 RID: 1142 RVA: 0x000162E6 File Offset: 0x000144E6
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040005EC RID: 1516
	public Shader SCShader;

	// Token: 0x040005ED RID: 1517
	private Material SCMaterial;

	// Token: 0x040005EE RID: 1518
	private float TimeX = 1f;

	// Token: 0x040005EF RID: 1519
	[Range(0f, 1f)]
	public float Distortion = 1f;
}
