using System;
using UnityEngine;

// Token: 0x020003B9 RID: 953
public abstract class TeleportDataBase
{
	// Token: 0x060019A0 RID: 6560 RVA: 0x000790B8 File Offset: 0x000772B8
	public TeleportDataBase(string environmentPreset = "outdoor", string soundOnTeleport = "", Action onGameSceneLoaded = null, bool donNotFade = false, float delayInFade = 0.3f)
	{
		this.environmentPreset = environmentPreset;
		this.soundOnTeleport = soundOnTeleport;
		this.onGameSceneLoaded = onGameSceneLoaded;
		this.donNotFade = donNotFade;
		this.delayInFade = delayInFade;
	}

	// Token: 0x060019A1 RID: 6561
	public abstract string GetDestinationId();

	// Token: 0x060019A2 RID: 6562
	public abstract GameSceneData GetDestinationSceneData();

	// Token: 0x060019A3 RID: 6563
	public abstract Vector3 GetPosition();

	// Token: 0x060019A4 RID: 6564 RVA: 0x000790E5 File Offset: 0x000772E5
	public virtual bool CanTeleport(out string error)
	{
		error = string.Empty;
		return true;
	}

	// Token: 0x040018ED RID: 6381
	public string environmentPreset;

	// Token: 0x040018EE RID: 6382
	public string soundOnTeleport;

	// Token: 0x040018EF RID: 6383
	public bool donNotFade;

	// Token: 0x040018F0 RID: 6384
	public Action onGameSceneLoaded;

	// Token: 0x040018F1 RID: 6385
	public float delayInFade;
}
