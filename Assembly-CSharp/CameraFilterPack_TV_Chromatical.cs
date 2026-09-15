using System;
using UnityEngine;

// Token: 0x020000F0 RID: 240
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/TV/Chromatical")]
public class CameraFilterPack_TV_Chromatical : MonoBehaviour
{
	// Token: 0x170000EC RID: 236
	// (get) Token: 0x060005D9 RID: 1497 RVA: 0x0001C9F1 File Offset: 0x0001ABF1
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

	// Token: 0x060005DA RID: 1498 RVA: 0x0001CA25 File Offset: 0x0001AC25
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/TV_Chromatical");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060005DB RID: 1499 RVA: 0x0001CA48 File Offset: 0x0001AC48
	private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (this.SCShader != null)
		{
			this.TimeX += Time.deltaTime * 2f;
			if (this.TimeX > 100f)
			{
				this.TimeX = 0f;
			}
			this.material.SetFloat("_TimeX", this.TimeX);
			this.material.SetVector("_ScreenResolution", new Vector2((float)Screen.width, (float)Screen.height));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060005DC RID: 1500 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060005DD RID: 1501 RVA: 0x0001CAE4 File Offset: 0x0001ACE4
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040007A4 RID: 1956
	public Shader SCShader;

	// Token: 0x040007A5 RID: 1957
	private float TimeX = 1f;

	// Token: 0x040007A6 RID: 1958
	private Vector4 ScreenResolution;

	// Token: 0x040007A7 RID: 1959
	private Material SCMaterial;
}
