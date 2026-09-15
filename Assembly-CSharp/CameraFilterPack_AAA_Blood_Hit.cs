using System;
using UnityEngine;

// Token: 0x02000013 RID: 19
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/AAA/Blood_Hit")]
public class CameraFilterPack_AAA_Blood_Hit : MonoBehaviour
{
	// Token: 0x17000011 RID: 17
	// (get) Token: 0x06000064 RID: 100 RVA: 0x000048B4 File Offset: 0x00002AB4
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

	// Token: 0x06000065 RID: 101 RVA: 0x000048E8 File Offset: 0x00002AE8
	private void Start()
	{
		this.Texture2 = Resources.Load("CameraFilterPack_AAA_Blood_Hit1") as Texture2D;
		this.SCShader = Shader.Find("CameraFilterPack/AAA_Blood_Hit");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000066 RID: 102 RVA: 0x00004920 File Offset: 0x00002B20
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
			this.material.SetFloat("_Value2", Mathf.Clamp(this.Hit_Left, 0f, 1f));
			this.material.SetFloat("_Value3", Mathf.Clamp(this.Hit_Up, 0f, 1f));
			this.material.SetFloat("_Value4", Mathf.Clamp(this.Hit_Right, 0f, 1f));
			this.material.SetFloat("_Value5", Mathf.Clamp(this.Hit_Down, 0f, 1f));
			this.material.SetFloat("_Value6", Mathf.Clamp(this.Blood_Hit_Left, 0f, 1f));
			this.material.SetFloat("_Value7", Mathf.Clamp(this.Blood_Hit_Up, 0f, 1f));
			this.material.SetFloat("_Value8", Mathf.Clamp(this.Blood_Hit_Right, 0f, 1f));
			this.material.SetFloat("_Value9", Mathf.Clamp(this.Blood_Hit_Down, 0f, 1f));
			this.material.SetFloat("_Value10", Mathf.Clamp(this.Hit_Full, 0f, 1f));
			this.material.SetFloat("_Value11", Mathf.Clamp(this.Blood_Hit_Full_1, 0f, 1f));
			this.material.SetFloat("_Value12", Mathf.Clamp(this.Blood_Hit_Full_2, 0f, 1f));
			this.material.SetFloat("_Value13", Mathf.Clamp(this.Blood_Hit_Full_3, 0f, 1f));
			this.material.SetTexture("_MainTex2", this.Texture2);
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000067 RID: 103 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000068 RID: 104 RVA: 0x00004B7B File Offset: 0x00002D7B
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040000CC RID: 204
	public Shader SCShader;

	// Token: 0x040000CD RID: 205
	private float TimeX = 1f;

	// Token: 0x040000CE RID: 206
	[Range(0f, 1f)]
	public float Hit_Left = 1f;

	// Token: 0x040000CF RID: 207
	[Range(0f, 1f)]
	public float Hit_Up;

	// Token: 0x040000D0 RID: 208
	[Range(0f, 1f)]
	public float Hit_Right;

	// Token: 0x040000D1 RID: 209
	[Range(0f, 1f)]
	public float Hit_Down;

	// Token: 0x040000D2 RID: 210
	[Range(0f, 1f)]
	public float Blood_Hit_Left;

	// Token: 0x040000D3 RID: 211
	[Range(0f, 1f)]
	public float Blood_Hit_Up;

	// Token: 0x040000D4 RID: 212
	[Range(0f, 1f)]
	public float Blood_Hit_Right;

	// Token: 0x040000D5 RID: 213
	[Range(0f, 1f)]
	public float Blood_Hit_Down;

	// Token: 0x040000D6 RID: 214
	[Range(0f, 1f)]
	public float Hit_Full;

	// Token: 0x040000D7 RID: 215
	[Range(0f, 1f)]
	public float Blood_Hit_Full_1;

	// Token: 0x040000D8 RID: 216
	[Range(0f, 1f)]
	public float Blood_Hit_Full_2;

	// Token: 0x040000D9 RID: 217
	[Range(0f, 1f)]
	public float Blood_Hit_Full_3;

	// Token: 0x040000DA RID: 218
	[Range(0f, 1f)]
	public float LightReflect = 0.5f;

	// Token: 0x040000DB RID: 219
	private Material SCMaterial;

	// Token: 0x040000DC RID: 220
	private Texture2D Texture2;
}
