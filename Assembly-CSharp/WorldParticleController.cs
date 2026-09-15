using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B3E RID: 2878
[RequireComponent(typeof(ParticleSystem))]
[DisallowMultipleComponent]
[ExecuteAlways]
public class WorldParticleController : MonoBehaviour
{
	// Token: 0x06004C81 RID: 19585 RVA: 0x001692B8 File Offset: 0x001674B8
	public static void UpdateParameters()
	{
		for (int i = 0; i < WorldParticleController.activeControllers.Count; i++)
		{
			WorldParticleController.activeControllers[i].UpdateParameters_Internal();
		}
	}

	// Token: 0x06004C82 RID: 19586 RVA: 0x001692EC File Offset: 0x001674EC
	private void UpdateParameters_Internal()
	{
		if (this.wereParametersApplied)
		{
			this.RestoreOriginalParameters();
		}
		for (int i = 0; i < this.particleCustomParametersByWind.Count; i++)
		{
			if (!this.particleCustomParametersByWind[i].DoWindAffection(WeatherSystem.Instance.WindValue))
			{
				Debug.LogError("CPWParticleWind failed to affect particle system: " + base.gameObject.name, this);
			}
		}
		this.wereParametersApplied = true;
	}

	// Token: 0x06004C83 RID: 19587 RVA: 0x0016935C File Offset: 0x0016755C
	private void Awake()
	{
		if (this.particleSystem == null)
		{
			base.TryGetComponent<ParticleSystem>(out this.particleSystem);
		}
		foreach (CPWParticleWind cpwparticleWind in this.particleCustomParametersByWind)
		{
			if (cpwparticleWind != null && !cpwparticleWind.particleSystem)
			{
				cpwparticleWind.particleSystem = this.particleSystem;
			}
		}
		this.CacheOriginalParameters(false);
	}

	// Token: 0x06004C84 RID: 19588 RVA: 0x001693E8 File Offset: 0x001675E8
	private void OnEnable()
	{
		WorldParticleController.activeControllers.Add(this);
		this.UpdateParameters_Internal();
	}

	// Token: 0x06004C85 RID: 19589 RVA: 0x001693FB File Offset: 0x001675FB
	private void OnDisable()
	{
		WorldParticleController.activeControllers.Remove(this);
	}

	// Token: 0x06004C86 RID: 19590 RVA: 0x0016940C File Offset: 0x0016760C
	private void CacheOriginalParameters(bool force = false)
	{
		if (this.originalParameters == null || force)
		{
			WorldParticleController.ParticleSystemCache particleSystemCache = new WorldParticleController.ParticleSystemCache();
			particleSystemCache.originalEmissionRate = this.particleSystem.emission.rateOverTime;
			ParticleSystem.MainModule main = this.particleSystem.main;
			particleSystemCache.originalStartColor = main.startColor;
			particleSystemCache.originalStartRotation = main.startRotation;
			particleSystemCache.originalStartRotationX = main.startRotationX;
			particleSystemCache.originalStartRotationY = main.startRotationY;
			particleSystemCache.originalStartRotationZ = main.startRotationZ;
			particleSystemCache.startSize3D = main.startSize3D;
			particleSystemCache.originalStartSize = main.startSize;
			particleSystemCache.originalStartSizeX = main.startSizeX;
			particleSystemCache.originalStartSizeY = main.startSizeY;
			particleSystemCache.originalStartSizeZ = main.startSizeZ;
			ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = this.particleSystem.velocityOverLifetime;
			if (velocityOverLifetime.enabled)
			{
				particleSystemCache.originalVelocityX = velocityOverLifetime.x;
				particleSystemCache.originalVelocityY = velocityOverLifetime.y;
				particleSystemCache.originalVelocityZ = velocityOverLifetime.z;
			}
			this.originalParameters = particleSystemCache;
		}
	}

	// Token: 0x06004C87 RID: 19591 RVA: 0x0016951C File Offset: 0x0016771C
	private void RestoreOriginalParameters()
	{
		if (this.particleSystem == null)
		{
			return;
		}
		WorldParticleController.ParticleSystemCache particleSystemCache = this.originalParameters;
		if (particleSystemCache == null)
		{
			return;
		}
		this.particleSystem.emission.rateOverTime = particleSystemCache.originalEmissionRate;
		ParticleSystem.MainModule main = this.particleSystem.main;
		main.startColor = particleSystemCache.originalStartColor;
		main.startRotation = particleSystemCache.originalStartRotation;
		main.startRotationX = particleSystemCache.originalStartRotationX;
		main.startRotationY = particleSystemCache.originalStartRotationY;
		main.startRotationZ = particleSystemCache.originalStartRotationZ;
		main.startSize3D = particleSystemCache.startSize3D;
		if (!particleSystemCache.startSize3D)
		{
			main.startSize = particleSystemCache.originalStartSize;
		}
		else
		{
			main.startSizeX = particleSystemCache.originalStartSizeX;
			main.startSizeY = particleSystemCache.originalStartSizeY;
			main.startSizeZ = particleSystemCache.originalStartSizeZ;
		}
		ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = this.particleSystem.velocityOverLifetime;
		if (velocityOverLifetime.enabled)
		{
			velocityOverLifetime.x = particleSystemCache.originalVelocityX;
			velocityOverLifetime.y = particleSystemCache.originalVelocityY;
			velocityOverLifetime.z = particleSystemCache.originalVelocityZ;
		}
	}

	// Token: 0x04003D9B RID: 15771
	public static List<WorldParticleController> activeControllers = new List<WorldParticleController>();

	// Token: 0x04003D9C RID: 15772
	[SerializeField]
	private ParticleSystem particleSystem;

	// Token: 0x04003D9D RID: 15773
	[SerializeReference]
	[Space]
	private List<CPWParticleWind> particleCustomParametersByWind = new List<CPWParticleWind>();

	// Token: 0x04003D9E RID: 15774
	private WorldParticleController.ParticleSystemCache originalParameters;

	// Token: 0x04003D9F RID: 15775
	private bool wereParametersApplied;

	// Token: 0x02000B3F RID: 2879
	[Serializable]
	private class ParticleSystemCache
	{
		// Token: 0x04003DA0 RID: 15776
		public ParticleSystem.MinMaxCurve originalEmissionRate;

		// Token: 0x04003DA1 RID: 15777
		public ParticleSystem.MinMaxGradient originalStartColor;

		// Token: 0x04003DA2 RID: 15778
		public ParticleSystem.MinMaxCurve originalStartRotation;

		// Token: 0x04003DA3 RID: 15779
		public ParticleSystem.MinMaxCurve originalStartRotationX;

		// Token: 0x04003DA4 RID: 15780
		public ParticleSystem.MinMaxCurve originalStartRotationY;

		// Token: 0x04003DA5 RID: 15781
		public ParticleSystem.MinMaxCurve originalStartRotationZ;

		// Token: 0x04003DA6 RID: 15782
		public bool startSize3D;

		// Token: 0x04003DA7 RID: 15783
		public ParticleSystem.MinMaxCurve originalStartSize;

		// Token: 0x04003DA8 RID: 15784
		public ParticleSystem.MinMaxCurve originalStartSizeX;

		// Token: 0x04003DA9 RID: 15785
		public ParticleSystem.MinMaxCurve originalStartSizeY;

		// Token: 0x04003DAA RID: 15786
		public ParticleSystem.MinMaxCurve originalStartSizeZ;

		// Token: 0x04003DAB RID: 15787
		public ParticleSystem.MinMaxCurve originalVelocityX;

		// Token: 0x04003DAC RID: 15788
		public ParticleSystem.MinMaxCurve originalVelocityY;

		// Token: 0x04003DAD RID: 15789
		public ParticleSystem.MinMaxCurve originalVelocityZ;
	}
}
