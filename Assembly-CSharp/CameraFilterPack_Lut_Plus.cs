using System;
using UnityEngine;

// Token: 0x020000CF RID: 207
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Lut/Plus")]
public class CameraFilterPack_Lut_Plus : MonoBehaviour
{
	// Token: 0x170000CC RID: 204
	// (get) Token: 0x06000506 RID: 1286 RVA: 0x00018907 File Offset: 0x00016B07
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

	// Token: 0x06000507 RID: 1287 RVA: 0x0001893B File Offset: 0x00016B3B
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Lut_Plus");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000508 RID: 1288 RVA: 0x0001895C File Offset: 0x00016B5C
	public void SetIdentityLut()
	{
		int num = 16;
		Color[] array = new Color[num * num * num];
		float num2 = 1f / (1f * (float)num - 1f);
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num; j++)
			{
				for (int k = 0; k < num; k++)
				{
					array[i + j * num + k * num * num] = new Color((float)i * 1f * num2, (float)j * 1f * num2, (float)k * 1f * num2, 1f);
				}
			}
		}
		if (this.converted3DLut)
		{
			global::UnityEngine.Object.DestroyImmediate(this.converted3DLut);
		}
		this.converted3DLut = new Texture3D(num, num, num, TextureFormat.ARGB32, false);
		this.converted3DLut.SetPixels(array);
		this.converted3DLut.Apply();
	}

	// Token: 0x06000509 RID: 1289 RVA: 0x00018694 File Offset: 0x00016894
	public bool ValidDimensions(Texture2D tex2d)
	{
		return tex2d && tex2d.height == Mathf.FloorToInt(Mathf.Sqrt((float)tex2d.width));
	}

	// Token: 0x0600050A RID: 1290 RVA: 0x00018A34 File Offset: 0x00016C34
	public void Convert(Texture2D temp2DTex)
	{
		if (!temp2DTex)
		{
			this.SetIdentityLut();
			return;
		}
		int num = temp2DTex.width * temp2DTex.height;
		num = temp2DTex.height;
		if (!this.ValidDimensions(temp2DTex))
		{
			Debug.LogWarning("The given 2D texture " + temp2DTex.name + " cannot be used as a 3D LUT.");
			return;
		}
		Color[] pixels = temp2DTex.GetPixels();
		Color[] array = new Color[pixels.Length];
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num; j++)
			{
				for (int k = 0; k < num; k++)
				{
					int num2 = num - j - 1;
					array[i + j * num + k * num * num] = pixels[k * num + i + num2 * num * num];
				}
			}
		}
		if (this.converted3DLut)
		{
			global::UnityEngine.Object.DestroyImmediate(this.converted3DLut);
		}
		this.converted3DLut = new Texture3D(num, num, num, TextureFormat.ARGB32, false);
		this.converted3DLut.SetPixels(array);
		this.converted3DLut.Apply();
	}

	// Token: 0x0600050B RID: 1291 RVA: 0x00018B38 File Offset: 0x00016D38
	private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (this.SCShader != null || !SystemInfo.supports3DTextures)
		{
			this.TimeX += Time.deltaTime;
			if (this.TimeX > 100f)
			{
				this.TimeX = 0f;
			}
			if (this.converted3DLut == null)
			{
				this.Convert(this.LutTexture);
			}
			this.converted3DLut.wrapMode = TextureWrapMode.Clamp;
			this.material.SetFloat("_Blend", this.Blend);
			this.material.SetTexture("_LutTex", this.converted3DLut);
			Graphics.Blit(sourceTexture, destTexture, this.material, (QualitySettings.activeColorSpace == ColorSpace.Linear) ? 1 : 0);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600050C RID: 1292 RVA: 0x00002318 File Offset: 0x00000518
	private void OnValidate()
	{
	}

	// Token: 0x0600050D RID: 1293 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600050E RID: 1294 RVA: 0x00018BFA File Offset: 0x00016DFA
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000695 RID: 1685
	public Shader SCShader;

	// Token: 0x04000696 RID: 1686
	private float TimeX = 1f;

	// Token: 0x04000697 RID: 1687
	private Vector4 ScreenResolution;

	// Token: 0x04000698 RID: 1688
	private Material SCMaterial;

	// Token: 0x04000699 RID: 1689
	public Texture2D LutTexture;

	// Token: 0x0400069A RID: 1690
	private Texture3D converted3DLut;

	// Token: 0x0400069B RID: 1691
	[Range(0f, 1f)]
	public float Blend = 1f;

	// Token: 0x0400069C RID: 1692
	private string MemoPath;
}
