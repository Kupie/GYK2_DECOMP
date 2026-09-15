using System;
using UnityEngine;

// Token: 0x0200012E RID: 302
[RequireComponent(typeof(Collider))]
public class SoundZoneCollider : MonoBehaviour
{
	// Token: 0x1700011F RID: 287
	// (get) Token: 0x06000752 RID: 1874 RVA: 0x00022FE3 File Offset: 0x000211E3
	public SoundZone SoundZone
	{
		get
		{
			if (!this.isSoundZoneCached)
			{
				this.cachedSoundZone = base.GetComponentInParent<SoundZone>();
				this.isSoundZoneCached = true;
			}
			return this.cachedSoundZone;
		}
	}

	// Token: 0x06000753 RID: 1875 RVA: 0x00023006 File Offset: 0x00021206
	public void Initialize(int index)
	{
		this.index = index;
	}

	// Token: 0x06000754 RID: 1876 RVA: 0x0002300F File Offset: 0x0002120F
	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponentInParent<ISoundZoneRecognizable>() != null)
		{
			this.SoundZone.OnPlayerEnter(this.index);
		}
	}

	// Token: 0x06000755 RID: 1877 RVA: 0x0002302A File Offset: 0x0002122A
	private void OnTriggerExit(Collider other)
	{
		if (other.GetComponentInParent<ISoundZoneRecognizable>() != null)
		{
			this.SoundZone.OnPlayerExit(this.index);
		}
	}

	// Token: 0x04000934 RID: 2356
	[SerializeField]
	private int index;

	// Token: 0x04000935 RID: 2357
	private bool isSoundZoneCached;

	// Token: 0x04000936 RID: 2358
	private SoundZone cachedSoundZone;
}
