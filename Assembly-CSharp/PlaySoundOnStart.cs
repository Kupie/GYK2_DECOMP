using System;
using LazyBearTechnology;

// Token: 0x02000128 RID: 296
public class PlaySoundOnStart : PlaySoundOnEnable
{
	// Token: 0x06000729 RID: 1833 RVA: 0x0002204F File Offset: 0x0002024F
	private void Start()
	{
		this.soundHandler = LazyAudio.PlayAtGameObject(this.soundId, base.transform, SpatialType.sound3D, true);
	}

	// Token: 0x0600072A RID: 1834 RVA: 0x00002318 File Offset: 0x00000518
	protected override void OnEnable()
	{
	}
}
