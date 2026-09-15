using System;
using UnityEngine;

// Token: 0x02000046 RID: 70
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blur/DitherOffset")]
public class CameraFilterPack_Blur_DitherOffset : MonoBehaviour
{
	// Token: 0x17000043 RID: 67
	// (get) Token: 0x060001CF RID: 463 RVA: 0x0000B723 File Offset: 0x00009923
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

	// Token: 0x060001D0 RID: 464 RVA: 0x0000B757 File Offset: 0x00009957
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Blur_DitherOffset");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060001D1 RID: 465 RVA: 0x0000B778 File Offset: 0x00009978
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

	// Token: 0x060001D2 RID: 466 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060001D3 RID: 467 RVA: 0x0000B84A File Offset: 0x00009A4A
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040002DE RID: 734
	public Shader SCShader;

	// Token: 0x040002DF RID: 735
	private float TimeX = 1f;

	// Token: 0x040002E0 RID: 736
	private Vector4 ScreenResolution;

	// Token: 0x040002E1 RID: 737
	private Material SCMaterial;

	// Token: 0x040002E2 RID: 738
	[Range(1f, 16f)]
	public int Level = 4;

	// Token: 0x040002E3 RID: 739
	public Vector2 Distance = new Vector2(30f, 0f);
}
