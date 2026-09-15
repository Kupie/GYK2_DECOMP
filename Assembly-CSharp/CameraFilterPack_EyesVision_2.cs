using System;
using UnityEngine;

// Token: 0x0200009E RID: 158
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Vision/Eyes 2")]
public class CameraFilterPack_EyesVision_2 : MonoBehaviour
{
	// Token: 0x1700009B RID: 155
	// (get) Token: 0x060003DC RID: 988 RVA: 0x00013DBA File Offset: 0x00011FBA
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

	// Token: 0x060003DD RID: 989 RVA: 0x00013DEE File Offset: 0x00011FEE
	private void Start()
	{
		this.Texture2 = Resources.Load("CameraFilterPack_eyes_vision_2") as Texture2D;
		this.SCShader = Shader.Find("CameraFilterPack/EyesVision_2");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060003DE RID: 990 RVA: 0x00013E24 File Offset: 0x00012024
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
			this.material.SetFloat("_Value", this._EyeWave);
			this.material.SetFloat("_Value2", this._EyeSpeed);
			this.material.SetFloat("_Value3", this._EyeMove);
			this.material.SetFloat("_Value4", this._EyeBlink);
			this.material.SetTexture("_MainTex2", this.Texture2);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060003DF RID: 991 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060003E0 RID: 992 RVA: 0x00013F05 File Offset: 0x00012105
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000549 RID: 1353
	public Shader SCShader;

	// Token: 0x0400054A RID: 1354
	private float TimeX = 1f;

	// Token: 0x0400054B RID: 1355
	[Range(1f, 32f)]
	public float _EyeWave = 15f;

	// Token: 0x0400054C RID: 1356
	[Range(0f, 10f)]
	public float _EyeSpeed = 1f;

	// Token: 0x0400054D RID: 1357
	[Range(0f, 8f)]
	public float _EyeMove = 2f;

	// Token: 0x0400054E RID: 1358
	[Range(0f, 1f)]
	public float _EyeBlink = 1f;

	// Token: 0x0400054F RID: 1359
	private Material SCMaterial;

	// Token: 0x04000550 RID: 1360
	private Texture2D Texture2;
}
