using System;
using UnityEngine;

// Token: 0x02000067 RID: 103
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Colors/Switching")]
public class CameraFilterPack_Color_Switching : MonoBehaviour
{
	// Token: 0x17000063 RID: 99
	// (get) Token: 0x06000291 RID: 657 RVA: 0x0000EDB2 File Offset: 0x0000CFB2
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

	// Token: 0x06000292 RID: 658 RVA: 0x0000EDE6 File Offset: 0x0000CFE6
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Color_Switching");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000293 RID: 659 RVA: 0x0000EE08 File Offset: 0x0000D008
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
			this.material.SetFloat("_Distortion", (float)this.Color);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000294 RID: 660 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000295 RID: 661 RVA: 0x0000EEBF File Offset: 0x0000D0BF
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040003DA RID: 986
	public Shader SCShader;

	// Token: 0x040003DB RID: 987
	private float TimeX = 1f;

	// Token: 0x040003DC RID: 988
	private Vector4 ScreenResolution;

	// Token: 0x040003DD RID: 989
	private Material SCMaterial;

	// Token: 0x040003DE RID: 990
	[Range(0f, 5f)]
	public int Color = 1;
}
