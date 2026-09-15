using System;
using UnityEngine;

// Token: 0x02000012 RID: 18
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/AAA/Blood On Screen")]
public class CameraFilterPack_AAA_BloodOnScreen : MonoBehaviour
{
	// Token: 0x17000010 RID: 16
	// (get) Token: 0x0600005E RID: 94 RVA: 0x00004676 File Offset: 0x00002876
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

	// Token: 0x0600005F RID: 95 RVA: 0x000046AA File Offset: 0x000028AA
	private void Start()
	{
		this.Texture2 = Resources.Load("CameraFilterPack_AAA_BloodOnScreen1") as Texture2D;
		this.SCShader = Shader.Find("CameraFilterPack/AAA_BloodOnScreen");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000060 RID: 96 RVA: 0x000046E0 File Offset: 0x000028E0
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
			this.material.SetFloat("_Value", Mathf.Clamp(this.Blood_On_Screen, 0.02f, 1.6f));
			this.material.SetFloat("_Value2", Mathf.Clamp(this.Blood_Intensify, 0f, 2f));
			this.material.SetFloat("_Value3", Mathf.Clamp(this.Blood_Darkness, 0f, 2f));
			this.material.SetFloat("_Value4", Mathf.Clamp(this.Blood_Fade, 0f, 1f));
			this.material.SetFloat("_Value5", Mathf.Clamp(this.Blood_Distortion_Speed, 0f, 2f));
			this.material.SetColor("_Color2", this.Blood_Color);
			this.material.SetTexture("_MainTex2", this.Texture2);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000061 RID: 97 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000062 RID: 98 RVA: 0x00004838 File Offset: 0x00002A38
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040000C2 RID: 194
	public Shader SCShader;

	// Token: 0x040000C3 RID: 195
	private float TimeX = 1f;

	// Token: 0x040000C4 RID: 196
	[Range(0.02f, 1.6f)]
	public float Blood_On_Screen = 1f;

	// Token: 0x040000C5 RID: 197
	public Color Blood_Color = Color.red;

	// Token: 0x040000C6 RID: 198
	[Range(0f, 2f)]
	public float Blood_Intensify = 0.7f;

	// Token: 0x040000C7 RID: 199
	[Range(0f, 2f)]
	public float Blood_Darkness = 0.5f;

	// Token: 0x040000C8 RID: 200
	[Range(0f, 1f)]
	public float Blood_Distortion_Speed = 0.25f;

	// Token: 0x040000C9 RID: 201
	[Range(0f, 1f)]
	public float Blood_Fade = 1f;

	// Token: 0x040000CA RID: 202
	private Material SCMaterial;

	// Token: 0x040000CB RID: 203
	private Texture2D Texture2;
}
