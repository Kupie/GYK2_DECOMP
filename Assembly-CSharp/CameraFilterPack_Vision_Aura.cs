using System;
using UnityEngine;

// Token: 0x0200010B RID: 267
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Vision/Aura")]
public class CameraFilterPack_Vision_Aura : MonoBehaviour
{
	// Token: 0x17000107 RID: 263
	// (get) Token: 0x0600067B RID: 1659 RVA: 0x0001EEEA File Offset: 0x0001D0EA
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

	// Token: 0x0600067C RID: 1660 RVA: 0x0001EF1E File Offset: 0x0001D11E
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Vision_Aura");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600067D RID: 1661 RVA: 0x0001EF40 File Offset: 0x0001D140
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
			this.material.SetFloat("_Value", this.Twist);
			this.material.SetColor("_Value2", this.Color);
			this.material.SetFloat("_Value3", this.PosX);
			this.material.SetFloat("_Value4", this.PosY);
			this.material.SetFloat("_Value5", this.Speed);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600067E RID: 1662 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600067F RID: 1663 RVA: 0x0001F04E File Offset: 0x0001D24E
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000846 RID: 2118
	public Shader SCShader;

	// Token: 0x04000847 RID: 2119
	private float TimeX = 1f;

	// Token: 0x04000848 RID: 2120
	private Vector4 ScreenResolution;

	// Token: 0x04000849 RID: 2121
	private Material SCMaterial;

	// Token: 0x0400084A RID: 2122
	[Range(0f, 2f)]
	public float Twist = 1f;

	// Token: 0x0400084B RID: 2123
	[Range(-4f, 4f)]
	public float Speed = 1f;

	// Token: 0x0400084C RID: 2124
	public Color Color = new Color(0.16f, 0.57f, 0.19f);

	// Token: 0x0400084D RID: 2125
	[Range(-1f, 2f)]
	public float PosX = 0.5f;

	// Token: 0x0400084E RID: 2126
	[Range(-1f, 2f)]
	public float PosY = 0.5f;
}
