using System;
using UnityEngine;

// Token: 0x02000041 RID: 65
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Weather/Blizzard")]
public class CameraFilterPack_Blizzard : MonoBehaviour
{
	// Token: 0x1700003E RID: 62
	// (get) Token: 0x060001B1 RID: 433 RVA: 0x0000AF31 File Offset: 0x00009131
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

	// Token: 0x060001B2 RID: 434 RVA: 0x0000AF65 File Offset: 0x00009165
	private void Start()
	{
		this.Texture2 = Resources.Load("CameraFilterPack_Blizzard1") as Texture2D;
		this.SCShader = Shader.Find("CameraFilterPack/Blizzard");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060001B3 RID: 435 RVA: 0x0000AF9C File Offset: 0x0000919C
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
			this.material.SetFloat("_Value", this._Speed);
			this.material.SetFloat("_Value2", this._Size);
			this.material.SetFloat("_Value3", this._Fade);
			this.material.SetTexture("_MainTex2", this.Texture2);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060001B4 RID: 436 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060001B5 RID: 437 RVA: 0x0000B067 File Offset: 0x00009267
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040002BA RID: 698
	public Shader SCShader;

	// Token: 0x040002BB RID: 699
	private float TimeX = 1f;

	// Token: 0x040002BC RID: 700
	[Range(0f, 2f)]
	public float _Speed = 1f;

	// Token: 0x040002BD RID: 701
	[Range(0.2f, 2f)]
	public float _Size = 1f;

	// Token: 0x040002BE RID: 702
	[Range(0f, 1f)]
	public float _Fade = 1f;

	// Token: 0x040002BF RID: 703
	private Material SCMaterial;

	// Token: 0x040002C0 RID: 704
	private Texture2D Texture2;
}
