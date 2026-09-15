using System;
using UnityEngine;

// Token: 0x0200008C RID: 140
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Drawing/Manga_Flash")]
public class CameraFilterPack_Drawing_Manga_Flash : MonoBehaviour
{
	// Token: 0x17000088 RID: 136
	// (get) Token: 0x0600036F RID: 879 RVA: 0x0001212E File Offset: 0x0001032E
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

	// Token: 0x06000370 RID: 880 RVA: 0x00012162 File Offset: 0x00010362
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Drawing_Manga_Flash");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000371 RID: 881 RVA: 0x00012184 File Offset: 0x00010384
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
			this.material.SetFloat("_Value", this.Size);
			this.material.SetFloat("_Value2", (float)this.Speed);
			this.material.SetFloat("_Value3", this.PosX);
			this.material.SetFloat("_Value4", this.PosY);
			this.material.SetFloat("_Intensity", this.Intensity);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000372 RID: 882 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000373 RID: 883 RVA: 0x00012293 File Offset: 0x00010493
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040004C1 RID: 1217
	public Shader SCShader;

	// Token: 0x040004C2 RID: 1218
	private float TimeX = 1f;

	// Token: 0x040004C3 RID: 1219
	private Vector4 ScreenResolution;

	// Token: 0x040004C4 RID: 1220
	private Material SCMaterial;

	// Token: 0x040004C5 RID: 1221
	[Range(1f, 10f)]
	public float Size = 1f;

	// Token: 0x040004C6 RID: 1222
	[Range(0f, 30f)]
	public int Speed = 5;

	// Token: 0x040004C7 RID: 1223
	[Range(-1f, 1f)]
	public float PosX = 0.5f;

	// Token: 0x040004C8 RID: 1224
	[Range(-1f, 1f)]
	public float PosY = 0.5f;

	// Token: 0x040004C9 RID: 1225
	[Range(0f, 1f)]
	public float Intensity = 1f;
}
