using System;
using UnityEngine;

// Token: 0x02000079 RID: 121
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Distortion/Twist_Square")]
public class CameraFilterPack_Distortion_Twist_Square : MonoBehaviour
{
	// Token: 0x17000075 RID: 117
	// (get) Token: 0x060002FD RID: 765 RVA: 0x0001073A File Offset: 0x0000E93A
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

	// Token: 0x060002FE RID: 766 RVA: 0x0001076E File Offset: 0x0000E96E
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Distortion_Twist_Square");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060002FF RID: 767 RVA: 0x00010790 File Offset: 0x0000E990
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

	// Token: 0x06000300 RID: 768 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000301 RID: 769 RVA: 0x00010881 File Offset: 0x0000EA81
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400044F RID: 1103
	public Shader SCShader;

	// Token: 0x04000450 RID: 1104
	private float TimeX = 1f;

	// Token: 0x04000451 RID: 1105
	private Vector4 ScreenResolution;

	// Token: 0x04000452 RID: 1106
	private Material SCMaterial;

	// Token: 0x04000453 RID: 1107
	[Range(-2f, 2f)]
	public float CenterX = 0.5f;

	// Token: 0x04000454 RID: 1108
	[Range(-2f, 2f)]
	public float CenterY = 0.5f;

	// Token: 0x04000455 RID: 1109
	[Range(-3.14f, 3.14f)]
	public float Distortion = 0.5f;

	// Token: 0x04000456 RID: 1110
	[Range(-2f, 2f)]
	public float Size = 0.25f;
}
