using System;
using UnityEngine;

// Token: 0x02000AF6 RID: 2806
public class ParticleActivator : MonoBehaviour
{
	// Token: 0x06004AE7 RID: 19175 RVA: 0x0016180C File Offset: 0x0015FA0C
	private void Awake()
	{
		this.particleSystems = base.GetComponentsInChildren<ParticleSystem>(true);
	}

	// Token: 0x06004AE8 RID: 19176 RVA: 0x0016181C File Offset: 0x0015FA1C
	private void OnEnable()
	{
		foreach (ParticleSystem particleSystem in this.particleSystems)
		{
			if (particleSystem != null)
			{
				particleSystem.Play();
			}
		}
	}

	// Token: 0x06004AE9 RID: 19177 RVA: 0x0016184C File Offset: 0x0015FA4C
	private void OnDisable()
	{
		foreach (ParticleSystem particleSystem in this.particleSystems)
		{
			if (particleSystem != null)
			{
				particleSystem.Stop();
			}
		}
	}

	// Token: 0x04003C7A RID: 15482
	private ParticleSystem[] particleSystems;
}
