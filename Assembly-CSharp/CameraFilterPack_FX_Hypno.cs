using System;
using UnityEngine;

// Token: 0x020000B3 RID: 179
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/FX/Hypno")]
public class CameraFilterPack_FX_Hypno : MonoBehaviour
{
	// Token: 0x170000B0 RID: 176
	// (get) Token: 0x0600045A RID: 1114 RVA: 0x00015CEA File Offset: 0x00013EEA
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

	// Token: 0x0600045B RID: 1115 RVA: 0x00015D1E File Offset: 0x00013F1E
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/FX_Hypno");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600045C RID: 1116 RVA: 0x00015D40 File Offset: 0x00013F40
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
			this.material.SetFloat("_Value", this.Speed);
			this.material.SetFloat("_Value2", this.Red);
			this.material.SetFloat("_Value3", this.Green);
			this.material.SetFloat("_Value4", this.Blue);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600045D RID: 1117 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600045E RID: 1118 RVA: 0x00015E38 File Offset: 0x00014038
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040005D7 RID: 1495
	public Shader SCShader;

	// Token: 0x040005D8 RID: 1496
	private float TimeX = 1f;

	// Token: 0x040005D9 RID: 1497
	private Vector4 ScreenResolution;

	// Token: 0x040005DA RID: 1498
	private Material SCMaterial;

	// Token: 0x040005DB RID: 1499
	[Range(0f, 1f)]
	public float Speed = 1f;

	// Token: 0x040005DC RID: 1500
	[Range(-2f, 2f)]
	public float Red;

	// Token: 0x040005DD RID: 1501
	[Range(-2f, 2f)]
	public float Green = 1f;

	// Token: 0x040005DE RID: 1502
	[Range(-2f, 2f)]
	public float Blue = 1f;
}
