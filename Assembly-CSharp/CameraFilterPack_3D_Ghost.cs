using System;
using UnityEngine;

// Token: 0x02000009 RID: 9
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/3D/Ghost")]
public class CameraFilterPack_3D_Ghost : MonoBehaviour
{
	// Token: 0x17000007 RID: 7
	// (get) Token: 0x06000028 RID: 40 RVA: 0x0000300C File Offset: 0x0000120C
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

	// Token: 0x06000029 RID: 41 RVA: 0x00003040 File Offset: 0x00001240
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/3D_Ghost");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00003064 File Offset: 0x00001264
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
			this.material.SetFloat("GhostPosX", this.GhostPosX);
			this.material.SetFloat("GhostPosY", this.GhostPosY);
			this.material.SetFloat("GhostFade", this.GhostFade);
			this.material.SetFloat("GhostFade2", this.GhostFade2);
			this.material.SetFloat("GhostSize", this.GhostSize);
			this.material.SetFloat("_FixDistance", this._FixDistance);
			this.material.SetFloat("Drop_Near", this.Ghost_Near);
			this.material.SetFloat("Drop_Far", this.Ghost_Far);
			this.material.SetFloat("Drop_With_Obj", this.GhostWithoutObject);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			base.GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x0600002C RID: 44 RVA: 0x000031EC File Offset: 0x000013EC
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x0400004E RID: 78
	public Shader SCShader;

	// Token: 0x0400004F RID: 79
	private float TimeX = 1f;

	// Token: 0x04000050 RID: 80
	private Vector4 ScreenResolution;

	// Token: 0x04000051 RID: 81
	private Material SCMaterial;

	// Token: 0x04000052 RID: 82
	[Range(0f, 100f)]
	public float _FixDistance = 5f;

	// Token: 0x04000053 RID: 83
	[Range(-0.5f, 0.99f)]
	public float Ghost_Near = 0.08f;

	// Token: 0x04000054 RID: 84
	[Range(0f, 1f)]
	public float Ghost_Far = 0.55f;

	// Token: 0x04000055 RID: 85
	[Range(0f, 2f)]
	public float Intensity = 1f;

	// Token: 0x04000056 RID: 86
	[Range(0f, 1f)]
	public float GhostWithoutObject = 1f;

	// Token: 0x04000057 RID: 87
	[Range(-1f, 1f)]
	public float GhostPosX;

	// Token: 0x04000058 RID: 88
	[Range(-1f, 1f)]
	public float GhostPosY;

	// Token: 0x04000059 RID: 89
	[Range(0.1f, 8f)]
	public float GhostFade2 = 2f;

	// Token: 0x0400005A RID: 90
	[Range(-1f, 1f)]
	public float GhostFade;

	// Token: 0x0400005B RID: 91
	[Range(0.5f, 1.5f)]
	public float GhostSize = 0.9f;
}
