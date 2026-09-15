using System;
using UnityEngine;

// Token: 0x02000B0A RID: 2826
public class TextureReader
{
	// Token: 0x17000B53 RID: 2899
	// (get) Token: 0x06004B4A RID: 19274 RVA: 0x00163BA9 File Offset: 0x00161DA9
	public Texture2D ReadableTexture
	{
		get
		{
			if (!this.readableTexture)
			{
				this.readableTexture = this.CreateReadableTexture();
			}
			return this.readableTexture;
		}
	}

	// Token: 0x06004B4B RID: 19275 RVA: 0x00163BCA File Offset: 0x00161DCA
	public TextureReader(Texture2D texture2D)
	{
		this.sourceTexture = texture2D;
	}

	// Token: 0x06004B4C RID: 19276 RVA: 0x00163BDC File Offset: 0x00161DDC
	private Texture2D CreateReadableTexture()
	{
		RenderTexture temporary = RenderTexture.GetTemporary(this.sourceTexture.width, this.sourceTexture.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
		Texture2D texture2D = new Texture2D(this.sourceTexture.width, this.sourceTexture.height, TextureFormat.Alpha8, false);
		texture2D.filterMode = FilterMode.Point;
		RenderTexture active = RenderTexture.active;
		Graphics.Blit(this.sourceTexture, temporary);
		RenderTexture.active = temporary;
		texture2D.ReadPixels(new Rect(0f, 0f, (float)temporary.width, (float)temporary.height), 0, 0);
		texture2D.Apply();
		RenderTexture.active = active;
		RenderTexture.ReleaseTemporary(temporary);
		return texture2D;
	}

	// Token: 0x04003CAE RID: 15534
	private readonly Texture2D sourceTexture;

	// Token: 0x04003CAF RID: 15535
	private Texture2D readableTexture;
}
