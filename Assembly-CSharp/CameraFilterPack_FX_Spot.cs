using System;
using UnityEngine;

// Token: 0x020000BA RID: 186
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/FX/Spot")]
public class CameraFilterPack_FX_Spot : MonoBehaviour
{
	// Token: 0x170000B7 RID: 183
	// (get) Token: 0x06000484 RID: 1156 RVA: 0x00016657 File Offset: 0x00014857
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

	// Token: 0x06000485 RID: 1157 RVA: 0x0001668B File Offset: 0x0001488B
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/FX_Spot");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000486 RID: 1158 RVA: 0x000166AC File Offset: 0x000148AC
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
			this.material.SetFloat("_PositionX", this.center.x);
			this.material.SetFloat("_PositionY", this.center.y);
			this.material.SetFloat("_Radius", this.Radius);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x06000487 RID: 1159 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x06000488 RID: 1160 RVA: 0x00016798 File Offset: 0x00014998
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x04000600 RID: 1536
	public Shader SCShader;

	// Token: 0x04000601 RID: 1537
	private float TimeX = 1f;

	// Token: 0x04000602 RID: 1538
	private Vector4 ScreenResolution;

	// Token: 0x04000603 RID: 1539
	private Material SCMaterial;

	// Token: 0x04000604 RID: 1540
	public Vector2 center = new Vector2(0.5f, 0.5f);

	// Token: 0x04000605 RID: 1541
	public float Radius = 0.2f;
}
