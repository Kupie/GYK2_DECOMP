using System;
using UnityEngine;

// Token: 0x02000115 RID: 277
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Vision/SniperScore")]
public class CameraFilterPack_Vision_SniperScore : MonoBehaviour
{
	// Token: 0x17000111 RID: 273
	// (get) Token: 0x060006B7 RID: 1719 RVA: 0x00020021 File Offset: 0x0001E221
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

	// Token: 0x060006B8 RID: 1720 RVA: 0x00020055 File Offset: 0x0001E255
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Vision_SniperScore");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060006B9 RID: 1721 RVA: 0x00020078 File Offset: 0x0001E278
	private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (this.SCShader != null)
		{
			this.TimeX += Time.deltaTime;
			if (this.TimeX > 100f)
			{
				this.TimeX = 0f;
			}
			this.material.SetFloat("_Fade", this.Fade);
			this.material.SetFloat("_TimeX", this.TimeX);
			this.material.SetFloat("_Value", this.Size);
			this.material.SetFloat("_Value2", this.Smooth);
			this.material.SetFloat("_Value3", this.StretchX);
			this.material.SetFloat("_Value4", this.StretchY);
			this.material.SetFloat("_Cible", this._Cible);
			this.material.SetFloat("_ExtraColor", this._ExtraColor);
			this.material.SetFloat("_Distortion", this._Distortion);
			this.material.SetFloat("_PosX", this._PosX);
			this.material.SetFloat("_PosY", this._PosY);
			this.material.SetColor("_Tint", this._Tint);
			this.material.SetFloat("_ExtraLight", this._ExtraLight);
			Vector2 vector = new Vector2((float)Screen.width, (float)Screen.height);
			this.material.SetVector("_ScreenResolution", new Vector4(vector.x, vector.y, vector.y / vector.x, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060006BA RID: 1722 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060006BB RID: 1723 RVA: 0x00020239 File Offset: 0x0001E439
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000899 RID: 2201
	public Shader SCShader;

	// Token: 0x0400089A RID: 2202
	private float TimeX = 1f;

	// Token: 0x0400089B RID: 2203
	private Vector4 ScreenResolution;

	// Token: 0x0400089C RID: 2204
	private Material SCMaterial;

	// Token: 0x0400089D RID: 2205
	[Range(0f, 1f)]
	public float Fade = 1f;

	// Token: 0x0400089E RID: 2206
	[Range(0f, 1f)]
	public float Size = 0.45f;

	// Token: 0x0400089F RID: 2207
	[Range(0.01f, 0.4f)]
	public float Smooth = 0.045f;

	// Token: 0x040008A0 RID: 2208
	[Range(0f, 1f)]
	public float _Cible = 0.5f;

	// Token: 0x040008A1 RID: 2209
	[Range(0f, 1f)]
	public float _Distortion = 0.5f;

	// Token: 0x040008A2 RID: 2210
	[Range(0f, 1f)]
	public float _ExtraColor = 0.5f;

	// Token: 0x040008A3 RID: 2211
	[Range(0f, 1f)]
	public float _ExtraLight = 0.35f;

	// Token: 0x040008A4 RID: 2212
	public Color _Tint = new Color(0f, 0.6f, 0f, 0.25f);

	// Token: 0x040008A5 RID: 2213
	[Range(0f, 10f)]
	private float StretchX = 1f;

	// Token: 0x040008A6 RID: 2214
	[Range(0f, 10f)]
	private float StretchY = 1f;

	// Token: 0x040008A7 RID: 2215
	[Range(-1f, 1f)]
	public float _PosX;

	// Token: 0x040008A8 RID: 2216
	[Range(-1f, 1f)]
	public float _PosY;
}
