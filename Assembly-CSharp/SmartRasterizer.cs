using System;
using UnityEngine;

// Token: 0x02000177 RID: 375
[ExecuteInEditMode]
public class SmartRasterizer : MonoBehaviour
{
	// Token: 0x0600095F RID: 2399 RVA: 0x0002FD25 File Offset: 0x0002DF25
	private void Start()
	{
		this.shader = Shader.Find("Hidden/SmartRasterizer");
		this.material = new Material(this.shader);
	}

	// Token: 0x06000960 RID: 2400 RVA: 0x0002FD48 File Offset: 0x0002DF48
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (PlatformFeatures.Current.renderMode == PlatformRenderMode.Lightweight || this.material == null)
		{
			Graphics.Blit(source, destination);
			return;
		}
		Graphics.Blit(source, destination, this.material);
	}

	// Token: 0x04000B02 RID: 2818
	private Shader shader;

	// Token: 0x04000B03 RID: 2819
	private Material material;
}
