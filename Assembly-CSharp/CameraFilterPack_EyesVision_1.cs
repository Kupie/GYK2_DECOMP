using System;
using UnityEngine;

// Token: 0x0200009D RID: 157
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Vision/Eyes 1")]
public class CameraFilterPack_EyesVision_1 : MonoBehaviour
{
	// Token: 0x1700009A RID: 154
	// (get) Token: 0x060003D6 RID: 982 RVA: 0x00013C15 File Offset: 0x00011E15
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

	// Token: 0x060003D7 RID: 983 RVA: 0x00013C49 File Offset: 0x00011E49
	private void Start()
	{
		this.Texture2 = Resources.Load("CameraFilterPack_eyes_vision_1") as Texture2D;
		this.SCShader = Shader.Find("CameraFilterPack/EyesVision_1");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060003D8 RID: 984 RVA: 0x00013C80 File Offset: 0x00011E80
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

	// Token: 0x060003D9 RID: 985 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060003DA RID: 986 RVA: 0x00013D61 File Offset: 0x00011F61
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000541 RID: 1345
	public Shader SCShader;

	// Token: 0x04000542 RID: 1346
	private float TimeX = 1f;

	// Token: 0x04000543 RID: 1347
	[Range(1f, 32f)]
	public float _EyeWave = 15f;

	// Token: 0x04000544 RID: 1348
	[Range(0f, 10f)]
	public float _EyeSpeed = 1f;

	// Token: 0x04000545 RID: 1349
	[Range(0f, 8f)]
	public float _EyeMove = 2f;

	// Token: 0x04000546 RID: 1350
	[Range(0f, 1f)]
	public float _EyeBlink = 1f;

	// Token: 0x04000547 RID: 1351
	private Material SCMaterial;

	// Token: 0x04000548 RID: 1352
	private Texture2D Texture2;
}
