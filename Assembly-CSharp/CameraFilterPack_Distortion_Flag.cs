using System;
using UnityEngine;

// Token: 0x02000070 RID: 112
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Distortion/Flag")]
public class CameraFilterPack_Distortion_Flag : MonoBehaviour
{
	// Token: 0x1700006C RID: 108
	// (get) Token: 0x060002C7 RID: 711 RVA: 0x0000FA56 File Offset: 0x0000DC56
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

	// Token: 0x060002C8 RID: 712 RVA: 0x0000FA8A File Offset: 0x0000DC8A
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Distortion_Flag");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060002C9 RID: 713 RVA: 0x0000FAAC File Offset: 0x0000DCAC
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
			this.material.SetFloat("_Distortion", this.Distortion);
			this.material.SetVector("_ScreenResolution", new Vector2((float)Screen.width, (float)Screen.height));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060002CA RID: 714 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060002CB RID: 715 RVA: 0x0000FB5B File Offset: 0x0000DD5B
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000413 RID: 1043
	public Shader SCShader;

	// Token: 0x04000414 RID: 1044
	private float TimeX = 1f;

	// Token: 0x04000415 RID: 1045
	[Range(0f, 2f)]
	public float Distortion = 1f;

	// Token: 0x04000416 RID: 1046
	private Vector4 ScreenResolution;

	// Token: 0x04000417 RID: 1047
	private Material SCMaterial;

	// Token: 0x04000418 RID: 1048
	public static float ChangeDistortion;
}
