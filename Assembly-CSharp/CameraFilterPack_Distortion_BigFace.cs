using System;
using UnityEngine;

// Token: 0x0200006A RID: 106
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Distortion/BigFace")]
public class CameraFilterPack_Distortion_BigFace : MonoBehaviour
{
	// Token: 0x17000066 RID: 102
	// (get) Token: 0x060002A3 RID: 675 RVA: 0x0000F203 File Offset: 0x0000D403
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

	// Token: 0x060002A4 RID: 676 RVA: 0x0000F237 File Offset: 0x0000D437
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Distortion_BigFace");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060002A5 RID: 677 RVA: 0x0000F258 File Offset: 0x0000D458
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
			this.material.SetFloat("_Size", this._Size);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060002A6 RID: 678 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060002A7 RID: 679 RVA: 0x0000F324 File Offset: 0x0000D524
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040003EE RID: 1006
	public Shader SCShader;

	// Token: 0x040003EF RID: 1007
	private float TimeX = 6.5f;

	// Token: 0x040003F0 RID: 1008
	private Vector4 ScreenResolution;

	// Token: 0x040003F1 RID: 1009
	private Material SCMaterial;

	// Token: 0x040003F2 RID: 1010
	public float _Size = 5f;

	// Token: 0x040003F3 RID: 1011
	[Range(2f, 10f)]
	public float Distortion = 2.5f;
}
