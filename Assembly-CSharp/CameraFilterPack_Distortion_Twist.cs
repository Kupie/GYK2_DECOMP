using System;
using UnityEngine;

// Token: 0x02000078 RID: 120
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Distortion/Twist")]
public class CameraFilterPack_Distortion_Twist : MonoBehaviour
{
	// Token: 0x17000074 RID: 116
	// (get) Token: 0x060002F7 RID: 759 RVA: 0x00010599 File Offset: 0x0000E799
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

	// Token: 0x060002F8 RID: 760 RVA: 0x000105CD File Offset: 0x0000E7CD
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Distortion_Twist");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060002F9 RID: 761 RVA: 0x000105F0 File Offset: 0x0000E7F0
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
			this.material.SetFloat("_CenterX", this.CenterX);
			this.material.SetFloat("_CenterY", this.CenterY);
			this.material.SetFloat("_Distortion", this.Distortion);
			this.material.SetFloat("_Size", this.Size);
			this.material.SetVector("_ScreenResolution", new Vector2((float)Screen.width, (float)Screen.height));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060002FA RID: 762 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060002FB RID: 763 RVA: 0x000106E1 File Offset: 0x0000E8E1
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000447 RID: 1095
	public Shader SCShader;

	// Token: 0x04000448 RID: 1096
	private float TimeX = 1f;

	// Token: 0x04000449 RID: 1097
	private Vector4 ScreenResolution;

	// Token: 0x0400044A RID: 1098
	private Material SCMaterial;

	// Token: 0x0400044B RID: 1099
	[Range(-2f, 2f)]
	public float CenterX = 0.5f;

	// Token: 0x0400044C RID: 1100
	[Range(-2f, 2f)]
	public float CenterY = 0.5f;

	// Token: 0x0400044D RID: 1101
	[Range(-3.14f, 3.14f)]
	public float Distortion = 1f;

	// Token: 0x0400044E RID: 1102
	[Range(-2f, 2f)]
	public float Size = 1f;
}
