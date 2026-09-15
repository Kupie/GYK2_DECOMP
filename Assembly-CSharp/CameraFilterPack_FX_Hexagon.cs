using System;
using UnityEngine;

// Token: 0x020000B1 RID: 177
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/FX/Hexagon")]
public class CameraFilterPack_FX_Hexagon : MonoBehaviour
{
	// Token: 0x170000AE RID: 174
	// (get) Token: 0x0600044E RID: 1102 RVA: 0x00015A86 File Offset: 0x00013C86
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

	// Token: 0x0600044F RID: 1103 RVA: 0x00015ABA File Offset: 0x00013CBA
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/FX_Hexagon");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000450 RID: 1104 RVA: 0x00015ADC File Offset: 0x00013CDC
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
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000451 RID: 1105 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000452 RID: 1106 RVA: 0x00015B79 File Offset: 0x00013D79
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040005CE RID: 1486
	public Shader SCShader;

	// Token: 0x040005CF RID: 1487
	private float TimeX = 1f;

	// Token: 0x040005D0 RID: 1488
	private Vector4 ScreenResolution;

	// Token: 0x040005D1 RID: 1489
	private Material SCMaterial;
}
