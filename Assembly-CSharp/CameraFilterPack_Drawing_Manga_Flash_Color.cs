using System;
using UnityEngine;

// Token: 0x0200008E RID: 142
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Drawing/Manga_Flash_Color")]
public class CameraFilterPack_Drawing_Manga_Flash_Color : MonoBehaviour
{
	// Token: 0x1700008A RID: 138
	// (get) Token: 0x0600037B RID: 891 RVA: 0x000124D5 File Offset: 0x000106D5
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

	// Token: 0x0600037C RID: 892 RVA: 0x00012509 File Offset: 0x00010709
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Drawing_Manga_Flash_Color");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600037D RID: 893 RVA: 0x0001252C File Offset: 0x0001072C
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
			this.material.SetColor("Color", this.Color);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600037E RID: 894 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600037F RID: 895 RVA: 0x00012651 File Offset: 0x00010851
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040004D3 RID: 1235
	public Shader SCShader;

	// Token: 0x040004D4 RID: 1236
	private float TimeX = 1f;

	// Token: 0x040004D5 RID: 1237
	private Vector4 ScreenResolution;

	// Token: 0x040004D6 RID: 1238
	private Material SCMaterial;

	// Token: 0x040004D7 RID: 1239
	[Range(1f, 10f)]
	public float Size = 1f;

	// Token: 0x040004D8 RID: 1240
	public Color Color = new Color(0f, 0.7f, 1f, 1f);

	// Token: 0x040004D9 RID: 1241
	[Range(0f, 30f)]
	public int Speed = 5;

	// Token: 0x040004DA RID: 1242
	[Range(0f, 1f)]
	public float PosX = 0.5f;

	// Token: 0x040004DB RID: 1243
	[Range(0f, 1f)]
	public float PosY = 0.5f;

	// Token: 0x040004DC RID: 1244
	[Range(0f, 1f)]
	public float Intensity = 1f;
}
