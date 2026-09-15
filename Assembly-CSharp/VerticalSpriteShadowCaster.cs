using System;

// Token: 0x0200052B RID: 1323
public class VerticalSpriteShadowCaster : VerticalSprite
{
	// Token: 0x06002207 RID: 8711 RVA: 0x0009FD8C File Offset: 0x0009DF8C
	protected override void Awake()
	{
		this.invisibleSprite = true;
		this.castShadows = true;
		base.Awake();
		this.ApplyMaterial();
	}
}
