using System;
using UnityEngine;

// Token: 0x02000047 RID: 71
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blur/Focus")]
public class CameraFilterPack_Blur_Focus : MonoBehaviour
{
	// Token: 0x17000044 RID: 68
	// (get) Token: 0x060001D5 RID: 469 RVA: 0x0000B893 File Offset: 0x00009A93
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

	// Token: 0x060001D6 RID: 470 RVA: 0x0000B8C7 File Offset: 0x00009AC7
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Blur_Focus");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060001D7 RID: 471 RVA: 0x0000B8E8 File Offset: 0x00009AE8
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
			this.material.SetFloat("_CenterX", this.CenterX);
			this.material.SetFloat("_CenterY", this.CenterY);
			float num = Mathf.Round(this._Size / 0.2f) * 0.2f;
			this.material.SetFloat("_Size", num);
			this.material.SetFloat("_Circle", this._Eyes);
			this.material.SetVector("_ScreenResolution", new Vector2((float)Screen.width, (float)Screen.height));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060001D8 RID: 472 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060001D9 RID: 473 RVA: 0x0000B9EC File Offset: 0x00009BEC
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040002E4 RID: 740
	public Shader SCShader;

	// Token: 0x040002E5 RID: 741
	private float TimeX = 1f;

	// Token: 0x040002E6 RID: 742
	private Vector4 ScreenResolution;

	// Token: 0x040002E7 RID: 743
	private Material SCMaterial;

	// Token: 0x040002E8 RID: 744
	[Range(-1f, 1f)]
	public float CenterX;

	// Token: 0x040002E9 RID: 745
	[Range(-1f, 1f)]
	public float CenterY;

	// Token: 0x040002EA RID: 746
	[Range(0f, 10f)]
	public float _Size = 5f;

	// Token: 0x040002EB RID: 747
	[Range(0.12f, 64f)]
	public float _Eyes = 2f;
}
