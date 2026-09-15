using System;
using UnityEngine;

// Token: 0x02000085 RID: 133
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Drawing/Lines")]
public class CameraFilterPack_Drawing_Lines : MonoBehaviour
{
	// Token: 0x17000081 RID: 129
	// (get) Token: 0x06000345 RID: 837 RVA: 0x0001190E File Offset: 0x0000FB0E
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

	// Token: 0x06000346 RID: 838 RVA: 0x00011942 File Offset: 0x0000FB42
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Drawing_Lines");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000347 RID: 839 RVA: 0x00011964 File Offset: 0x0000FB64
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
			this.material.SetFloat("_Value", this.Number);
			this.material.SetFloat("_Value2", this.Random);
			this.material.SetFloat("_Value3", this.PositionY);
			this.material.SetFloat("_Value4", this.Value4);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000348 RID: 840 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000349 RID: 841 RVA: 0x00011A5C File Offset: 0x0000FC5C
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040004A0 RID: 1184
	public Shader SCShader;

	// Token: 0x040004A1 RID: 1185
	private float TimeX = 1f;

	// Token: 0x040004A2 RID: 1186
	private Vector4 ScreenResolution;

	// Token: 0x040004A3 RID: 1187
	private Material SCMaterial;

	// Token: 0x040004A4 RID: 1188
	[Range(0.1f, 10f)]
	public float Number = 1f;

	// Token: 0x040004A5 RID: 1189
	[Range(0f, 1f)]
	public float Random = 0.5f;

	// Token: 0x040004A6 RID: 1190
	[Range(0f, 10f)]
	private float PositionY = 1f;

	// Token: 0x040004A7 RID: 1191
	[Range(0f, 10f)]
	private float Value4 = 1f;
}
