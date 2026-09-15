using System;
using UnityEngine;

// Token: 0x020000B9 RID: 185
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/FX/Screens")]
public class CameraFilterPack_FX_Screens : MonoBehaviour
{
	// Token: 0x170000B6 RID: 182
	// (get) Token: 0x0600047E RID: 1150 RVA: 0x000164C5 File Offset: 0x000146C5
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

	// Token: 0x0600047F RID: 1151 RVA: 0x000164F9 File Offset: 0x000146F9
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/FX_Screens");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000480 RID: 1152 RVA: 0x0001651C File Offset: 0x0001471C
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
			this.material.SetFloat("_Value", this.Tiles);
			this.material.SetFloat("_Value2", this.Speed);
			this.material.SetFloat("_Value3", this.PosX);
			this.material.SetFloat("_Value4", this.PosY);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000481 RID: 1153 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000482 RID: 1154 RVA: 0x00016614 File Offset: 0x00014814
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040005F8 RID: 1528
	public Shader SCShader;

	// Token: 0x040005F9 RID: 1529
	private float TimeX = 1f;

	// Token: 0x040005FA RID: 1530
	private Vector4 ScreenResolution;

	// Token: 0x040005FB RID: 1531
	private Material SCMaterial;

	// Token: 0x040005FC RID: 1532
	[Range(0f, 256f)]
	public float Tiles = 8f;

	// Token: 0x040005FD RID: 1533
	[Range(0f, 5f)]
	public float Speed = 0.25f;

	// Token: 0x040005FE RID: 1534
	[Range(-1f, 1f)]
	public float PosX;

	// Token: 0x040005FF RID: 1535
	[Range(-1f, 1f)]
	public float PosY;
}
