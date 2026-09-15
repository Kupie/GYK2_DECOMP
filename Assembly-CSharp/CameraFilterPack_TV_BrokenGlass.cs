using System;
using UnityEngine;

// Token: 0x020000EE RID: 238
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/TV/Broken Glass")]
public class CameraFilterPack_TV_BrokenGlass : MonoBehaviour
{
	// Token: 0x170000EA RID: 234
	// (get) Token: 0x060005CD RID: 1485 RVA: 0x0001C3D8 File Offset: 0x0001A5D8
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

	// Token: 0x060005CE RID: 1486 RVA: 0x0001C40C File Offset: 0x0001A60C
	private void Start()
	{
		this.Texture2 = Resources.Load("CameraFilterPack_TV_BrokenGlass1") as Texture2D;
		this.SCShader = Shader.Find("CameraFilterPack/TV_BrokenGlass");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060005CF RID: 1487 RVA: 0x0001C444 File Offset: 0x0001A644
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
			this.material.SetFloat("_Value", this.LightReflect);
			this.material.SetFloat("_Value2", this.Broken_Small);
			this.material.SetFloat("_Value3", this.Broken_Medium);
			this.material.SetFloat("_Value4", this.Broken_High);
			this.material.SetFloat("_Value5", this.Broken_Big);
			this.material.SetTexture("_MainTex2", this.Texture2);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060005D0 RID: 1488 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060005D1 RID: 1489 RVA: 0x0001C53B File Offset: 0x0001A73B
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400078B RID: 1931
	public Shader SCShader;

	// Token: 0x0400078C RID: 1932
	private float TimeX = 1f;

	// Token: 0x0400078D RID: 1933
	[Range(0f, 128f)]
	public float Broken_Small;

	// Token: 0x0400078E RID: 1934
	[Range(0f, 128f)]
	public float Broken_Medium;

	// Token: 0x0400078F RID: 1935
	[Range(0f, 128f)]
	public float Broken_High;

	// Token: 0x04000790 RID: 1936
	[Range(0f, 128f)]
	public float Broken_Big = 1f;

	// Token: 0x04000791 RID: 1937
	[Range(0f, 0.004f)]
	public float LightReflect = 0.002f;

	// Token: 0x04000792 RID: 1938
	private Material SCMaterial;

	// Token: 0x04000793 RID: 1939
	private Texture2D Texture2;
}
