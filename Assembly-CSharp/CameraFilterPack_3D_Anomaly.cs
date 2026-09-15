using System;
using UnityEngine;

// Token: 0x02000003 RID: 3
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/3D/Anomaly")]
public class CameraFilterPack_3D_Anomaly : MonoBehaviour
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000004 RID: 4 RVA: 0x00002165 File Offset: 0x00000365
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

	// Token: 0x06000005 RID: 5 RVA: 0x00002199 File Offset: 0x00000399
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/3D_Anomaly");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000006 RID: 6 RVA: 0x000021BC File Offset: 0x000003BC
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
			this.material.SetFloat("_Value2", this.Intensity);
			this.material.SetFloat("Anomaly_Distortion", this.Anomaly_Distortion);
			this.material.SetFloat("Anomaly_Distortion_Size", this.Anomaly_Distortion_Size);
			this.material.SetFloat("Anomaly_Intensity", this.Anomaly_Intensity);
			this.material.SetFloat("_FixDistance", this._FixDistance);
			this.material.SetFloat("Anomaly_Near", this.Anomaly_Near);
			this.material.SetFloat("Anomaly_Far", this.Anomaly_Far);
			this.material.SetFloat("Anomaly_With_Obj", this.AnomalyWithoutObject);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			base.GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000007 RID: 7 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000008 RID: 8 RVA: 0x0000231A File Offset: 0x0000051A
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000004 RID: 4
	public Shader SCShader;

	// Token: 0x04000005 RID: 5
	private float TimeX = 1f;

	// Token: 0x04000006 RID: 6
	private Vector4 ScreenResolution;

	// Token: 0x04000007 RID: 7
	private Material SCMaterial;

	// Token: 0x04000008 RID: 8
	[Range(0f, 100f)]
	public float _FixDistance = 23f;

	// Token: 0x04000009 RID: 9
	[Range(-0.5f, 0.99f)]
	public float Anomaly_Near = 0.045f;

	// Token: 0x0400000A RID: 10
	[Range(0f, 1f)]
	public float Anomaly_Far = 0.11f;

	// Token: 0x0400000B RID: 11
	[Range(0f, 2f)]
	public float Intensity = 1f;

	// Token: 0x0400000C RID: 12
	[Range(0f, 1f)]
	public float AnomalyWithoutObject = 1f;

	// Token: 0x0400000D RID: 13
	[Range(0.1f, 1f)]
	public float Anomaly_Distortion = 0.25f;

	// Token: 0x0400000E RID: 14
	[Range(4f, 64f)]
	public float Anomaly_Distortion_Size = 12f;

	// Token: 0x0400000F RID: 15
	[Range(-4f, 8f)]
	public float Anomaly_Intensity = 2f;
}
