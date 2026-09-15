using System;
using UnityEngine;

// Token: 0x02000176 RID: 374
[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class OrthoAddVerticalOnly : MonoBehaviour
{
	// Token: 0x06000954 RID: 2388 RVA: 0x0002FB61 File Offset: 0x0002DD61
	private void OnEnable()
	{
		if (this.shader == null)
		{
			this.shader = Shader.Find("Hidden/Ortho/AddVerticalOnlyCropX");
		}
		this.cam = base.GetComponent<Camera>();
		this.CacheBase();
		this.EnsureMat();
	}

	// Token: 0x06000955 RID: 2389 RVA: 0x0002FB99 File Offset: 0x0002DD99
	private void OnDisable()
	{
		this.RestoreBase();
	}

	// Token: 0x06000956 RID: 2390 RVA: 0x0002FB99 File Offset: 0x0002DD99
	private void OnDestroy()
	{
		this.RestoreBase();
	}

	// Token: 0x06000957 RID: 2391 RVA: 0x0002FBA1 File Offset: 0x0002DDA1
	private void CacheBase()
	{
		if (this.cached)
		{
			return;
		}
		this.baseOrthoSize = this.cam.orthographicSize;
		this.cached = true;
	}

	// Token: 0x06000958 RID: 2392 RVA: 0x0002FBC4 File Offset: 0x0002DDC4
	private void RestoreBase()
	{
		if (!this.cached || this.cam == null)
		{
			return;
		}
		this.cam.orthographicSize = this.baseOrthoSize;
	}

	// Token: 0x06000959 RID: 2393 RVA: 0x0002FBEE File Offset: 0x0002DDEE
	private void EnsureMat()
	{
		if (this.cropMat == null)
		{
			this.cropMat = new Material(this.shader)
			{
				hideFlags = HideFlags.DontSave
			};
		}
	}

	// Token: 0x0600095A RID: 2394 RVA: 0x0002FC18 File Offset: 0x0002DE18
	private float CalculateVerticalFactor()
	{
		float num = Mathf.Max(1f, this.verticalFactor);
		if (this.autoSqueeze)
		{
			float x = this.cam.transform.eulerAngles.x;
			if (x < this.defaultCameraXAngle)
			{
				float num2 = this.defaultCameraXAngle - x;
				float num3 = 1f + num2 / this.defaultCameraXAngle * this.autoSqueezeFactor;
				num *= num3;
			}
		}
		return num;
	}

	// Token: 0x0600095B RID: 2395 RVA: 0x0002FC84 File Offset: 0x0002DE84
	private void OnPreCull()
	{
		float num = this.CalculateVerticalFactor();
		this.cam.orthographicSize = this.baseOrthoSize * num;
	}

	// Token: 0x0600095C RID: 2396 RVA: 0x0002FB99 File Offset: 0x0002DD99
	private void OnPostRender()
	{
		this.RestoreBase();
	}

	// Token: 0x0600095D RID: 2397 RVA: 0x0002FCAC File Offset: 0x0002DEAC
	private void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		if (this.cropMat == null)
		{
			Graphics.Blit(src, dst);
			return;
		}
		float num = this.CalculateVerticalFactor();
		this.cropMat.SetFloat("_CropXF", num);
		Graphics.Blit(src, dst, this.cropMat, 0);
	}

	// Token: 0x04000AF9 RID: 2809
	[Tooltip(">1 renders more vertical content. 1 = off.")]
	[Range(1f, 3f)]
	public float verticalFactor = 1f;

	// Token: 0x04000AFA RID: 2810
	[Tooltip("Material with the shader below (auto-created if null).")]
	public Material cropMat;

	// Token: 0x04000AFB RID: 2811
	private Camera cam;

	// Token: 0x04000AFC RID: 2812
	private float baseOrthoSize;

	// Token: 0x04000AFD RID: 2813
	private bool cached;

	// Token: 0x04000AFE RID: 2814
	public Shader shader;

	// Token: 0x04000AFF RID: 2815
	public bool autoSqueeze = true;

	// Token: 0x04000B00 RID: 2816
	public float defaultCameraXAngle = 53.13f;

	// Token: 0x04000B01 RID: 2817
	public float autoSqueezeFactor = 1.1f;
}
