using System;
using UnityEngine;

// Token: 0x0200004A RID: 74
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blur/Noise")]
public class CameraFilterPack_Blur_Noise : MonoBehaviour
{
	// Token: 0x17000047 RID: 71
	// (get) Token: 0x060001E7 RID: 487 RVA: 0x0000BD1C File Offset: 0x00009F1C
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

	// Token: 0x060001E8 RID: 488 RVA: 0x0000BD50 File Offset: 0x00009F50
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Blur_Noise");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060001E9 RID: 489 RVA: 0x0000BD74 File Offset: 0x00009F74
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
			this.material.SetFloat("_Level", (float)this.Level);
			this.material.SetVector("_Distance", this.Distance);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060001EA RID: 490 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060001EB RID: 491 RVA: 0x0000BE46 File Offset: 0x0000A046
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040002F8 RID: 760
	public Shader SCShader;

	// Token: 0x040002F9 RID: 761
	private float TimeX = 1f;

	// Token: 0x040002FA RID: 762
	private Vector4 ScreenResolution;

	// Token: 0x040002FB RID: 763
	private Material SCMaterial;

	// Token: 0x040002FC RID: 764
	[Range(2f, 16f)]
	public int Level = 4;

	// Token: 0x040002FD RID: 765
	public Vector2 Distance = new Vector2(30f, 0f);
}
