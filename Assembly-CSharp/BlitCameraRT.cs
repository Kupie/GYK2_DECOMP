using System;
using UnityEngine;

// Token: 0x02000166 RID: 358
[ExecuteAlways]
public class BlitCameraRT : MonoBehaviour
{
	// Token: 0x060008B7 RID: 2231 RVA: 0x0002D072 File Offset: 0x0002B272
	protected void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.mainCamera != null && this.mainCamera.RenderTexture != null)
		{
			Graphics.Blit(this.mainCamera.RenderTexture, destination);
		}
	}

	// Token: 0x04000A75 RID: 2677
	[SerializeField]
	private MainCamera mainCamera;
}
