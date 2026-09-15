using System;
using System.Collections;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000126 RID: 294
[RequireComponent(typeof(ParticleSystem))]
public class PlaySoundOnParticleCount : MonoBehaviour
{
	// Token: 0x0600071C RID: 1820 RVA: 0x00021F05 File Offset: 0x00020105
	private void Awake()
	{
		this.cachedParticleSystem = base.GetComponent<ParticleSystem>();
	}

	// Token: 0x0600071D RID: 1821 RVA: 0x00021F13 File Offset: 0x00020113
	private void OnEnable()
	{
		this.lastCount = ((this.cachedParticleSystem != null) ? this.cachedParticleSystem.particleCount : 0);
	}

	// Token: 0x0600071E RID: 1822 RVA: 0x00021F38 File Offset: 0x00020138
	private void LateUpdate()
	{
		if (this.cachedParticleSystem == null)
		{
			return;
		}
		int particleCount = this.cachedParticleSystem.particleCount;
		if (this.lastCount < this.particleThreshold && particleCount >= this.particleThreshold)
		{
			this.PlaySound();
		}
		this.lastCount = particleCount;
	}

	// Token: 0x0600071F RID: 1823 RVA: 0x00021F84 File Offset: 0x00020184
	private void PlaySound()
	{
		if (string.IsNullOrEmpty(this.soundId))
		{
			return;
		}
		if (this.delay > 0f)
		{
			base.StartCoroutine(this.PlayAfterDelay());
			return;
		}
		this.PlayNow();
	}

	// Token: 0x06000720 RID: 1824 RVA: 0x00021FB5 File Offset: 0x000201B5
	private IEnumerator PlayAfterDelay()
	{
		yield return new WaitForSeconds(this.delay);
		this.PlayNow();
		yield break;
	}

	// Token: 0x06000721 RID: 1825 RVA: 0x00021FC4 File Offset: 0x000201C4
	private void PlayNow()
	{
		LazyAudio.Play(this.soundId, false);
	}

	// Token: 0x04000905 RID: 2309
	[SerializeField]
	[Min(1f)]
	private int particleThreshold = 1;

	// Token: 0x04000906 RID: 2310
	[SerializeField]
	private string soundId;

	// Token: 0x04000907 RID: 2311
	[SerializeField]
	[Min(0f)]
	private float delay;

	// Token: 0x04000908 RID: 2312
	private ParticleSystem cachedParticleSystem;

	// Token: 0x04000909 RID: 2313
	private int lastCount;
}
