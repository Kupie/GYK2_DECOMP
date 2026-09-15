using System;
using UnityEngine;

// Token: 0x0200006B RID: 107
[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Distortion/BlackHole")]
public class CameraFilterPack_Distortion_BlackHole : MonoBehaviour
{
	// Token: 0x17000067 RID: 103
	// (get) Token: 0x060002A9 RID: 681 RVA: 0x0000F367 File Offset: 0x0000D567
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

	// Token: 0x060002AA RID: 682 RVA: 0x0000F39B File Offset: 0x0000D59B
	private void Start()
	{
		this.SCShader = Shader.Find("CameraFilterPack/Distortion_BlackHole");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060002AB RID: 683 RVA: 0x0000F3BC File Offset: 0x0000D5BC
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
			this.material.SetFloat("_PositionX", this.PositionX);
			this.material.SetFloat("_PositionY", this.PositionY);
			this.material.SetFloat("_Distortion", this.Size);
			this.material.SetFloat("_Distortion2", this.Distortion);
			this.material.SetVector("_ScreenResolution", new Vector2((float)Screen.width, (float)Screen.height));
			Graphics.Blit(sourceTexture, destTexture, this.material);
			return;
		}
		Graphics.Blit(sourceTexture, destTexture);
	}

	// Token: 0x060002AC RID: 684 RVA: 0x00002318 File Offset: 0x00000518
	private void Update()
	{
	}

	// Token: 0x060002AD RID: 685 RVA: 0x0000F4AD File Offset: 0x0000D6AD
	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	// Token: 0x040003F4 RID: 1012
	public Shader SCShader;

	// Token: 0x040003F5 RID: 1013
	private float TimeX = 1f;

	// Token: 0x040003F6 RID: 1014
	private Vector4 ScreenResolution;

	// Token: 0x040003F7 RID: 1015
	private Material SCMaterial;

	// Token: 0x040003F8 RID: 1016
	[Range(-1f, 1f)]
	public float PositionX;

	// Token: 0x040003F9 RID: 1017
	[Range(-1f, 1f)]
	public float PositionY;

	// Token: 0x040003FA RID: 1018
	[Range(-5f, 5f)]
	public float Size = 0.05f;

	// Token: 0x040003FB RID: 1019
	[Range(0f, 180f)]
	public float Distortion = 30f;
}
