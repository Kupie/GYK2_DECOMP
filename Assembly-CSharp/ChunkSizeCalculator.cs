using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020006FB RID: 1787
public static class ChunkSizeCalculator
{
	// Token: 0x06002F41 RID: 12097 RVA: 0x000E2BC0 File Offset: 0x000E0DC0
	public static ChunkBoundsPair CalculateChunkBounds(GameObject obj)
	{
		Vector3 position = obj.transform.position;
		ChunkSizeCalculator.<>c__DisplayClass0_0 CS$<>8__locals1;
		CS$<>8__locals1.boundsWithShadows = new Bounds(position, Vector3.zero);
		CS$<>8__locals1.boundsWithShadowsWereSet = false;
		CS$<>8__locals1.boundsWithoutShadows = new Bounds(position, Vector3.zero);
		CS$<>8__locals1.boundsWithoutShadowsWereSet = false;
		CS$<>8__locals1.shadowBoundExtends = 4f;
		Renderer renderer;
		if (obj.TryGetComponent<Renderer>(out renderer))
		{
			ChunkSizeCalculator.<CalculateChunkBounds>g__EncapsulateRenderer|0_0(renderer, ref CS$<>8__locals1);
		}
		Light light;
		if (obj.TryGetComponent<Light>(out light))
		{
			ChunkSizeCalculator.<CalculateChunkBounds>g__EncapsulateLight|0_1(light, ref CS$<>8__locals1);
		}
		Renderer[] componentsInChildren = obj.GetComponentsInChildren<Renderer>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			ChunkSizeCalculator.<CalculateChunkBounds>g__EncapsulateRenderer|0_0(componentsInChildren[i], ref CS$<>8__locals1);
		}
		Light[] componentsInChildren2 = obj.GetComponentsInChildren<Light>(true);
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			ChunkSizeCalculator.<CalculateChunkBounds>g__EncapsulateLight|0_1(componentsInChildren2[j], ref CS$<>8__locals1);
		}
		ChunkBoundsContributor[] componentsInChildren3 = obj.GetComponentsInChildren<ChunkBoundsContributor>(true);
		for (int k = 0; k < componentsInChildren3.Length; k++)
		{
			ChunkSizeCalculator.<CalculateChunkBounds>g__EncapsulateContributor|0_5(componentsInChildren3[k], ref CS$<>8__locals1);
		}
		return new ChunkBoundsPair(CS$<>8__locals1.boundsWithShadows, CS$<>8__locals1.boundsWithoutShadows);
	}

	// Token: 0x06002F42 RID: 12098 RVA: 0x000E2CC8 File Offset: 0x000E0EC8
	[CompilerGenerated]
	internal static void <CalculateChunkBounds>g__EncapsulateRenderer|0_0(Renderer rendererComponent, ref ChunkSizeCalculator.<>c__DisplayClass0_0 A_1)
	{
		if (rendererComponent.enabled && rendererComponent.GetComponent<ParticleSystem>() == null)
		{
			Bounds bounds = rendererComponent.bounds;
			if (!A_1.boundsWithoutShadowsWereSet)
			{
				A_1.boundsWithoutShadows = bounds;
				A_1.boundsWithoutShadowsWereSet = true;
			}
			else
			{
				A_1.boundsWithoutShadows.Encapsulate(bounds);
			}
			if (rendererComponent.shadowCastingMode == ShadowCastingMode.On)
			{
				Vector3 vector = new Vector3(bounds.size.y * A_1.shadowBoundExtends, 0f, bounds.size.y * A_1.shadowBoundExtends);
				Vector3 size = bounds.size;
				if (vector.x > size.x || vector.z > size.z)
				{
					bounds.Expand(new Vector3(Mathf.Max(0f, vector.x - size.x), 0f, Mathf.Max(0f, vector.z - size.z)));
				}
			}
			if (!A_1.boundsWithShadowsWereSet)
			{
				A_1.boundsWithShadows = bounds;
				A_1.boundsWithShadowsWereSet = true;
				return;
			}
			A_1.boundsWithShadows.Encapsulate(bounds);
		}
	}

	// Token: 0x06002F43 RID: 12099 RVA: 0x000E2DE0 File Offset: 0x000E0FE0
	[CompilerGenerated]
	internal static void <CalculateChunkBounds>g__EncapsulateLight|0_1(Light lightComponent, ref ChunkSizeCalculator.<>c__DisplayClass0_0 A_1)
	{
		Vector3 lossyScale = lightComponent.transform.lossyScale;
		float num = lightComponent.range * Mathf.Max(new float[] { lossyScale.x, lossyScale.y, lossyScale.z });
		ChunkSizeCalculator.<CalculateChunkBounds>g__EncapsulateBounds|0_2(new Bounds(lightComponent.transform.position, Vector3.one * num * 2f), ref A_1);
	}

	// Token: 0x06002F44 RID: 12100 RVA: 0x000E2E52 File Offset: 0x000E1052
	[CompilerGenerated]
	internal static void <CalculateChunkBounds>g__EncapsulateBounds|0_2(Bounds extraBounds, ref ChunkSizeCalculator.<>c__DisplayClass0_0 A_1)
	{
		ChunkSizeCalculator.<CalculateChunkBounds>g__EncapsulateWithShadowsBounds|0_3(extraBounds, ref A_1);
		ChunkSizeCalculator.<CalculateChunkBounds>g__EncapsulateWithoutShadowsBounds|0_4(extraBounds, ref A_1);
	}

	// Token: 0x06002F45 RID: 12101 RVA: 0x000E2E62 File Offset: 0x000E1062
	[CompilerGenerated]
	internal static void <CalculateChunkBounds>g__EncapsulateWithShadowsBounds|0_3(Bounds extraBounds, ref ChunkSizeCalculator.<>c__DisplayClass0_0 A_1)
	{
		if (!A_1.boundsWithShadowsWereSet)
		{
			A_1.boundsWithShadows = extraBounds;
			A_1.boundsWithShadowsWereSet = true;
			return;
		}
		A_1.boundsWithShadows.Encapsulate(extraBounds);
	}

	// Token: 0x06002F46 RID: 12102 RVA: 0x000E2E87 File Offset: 0x000E1087
	[CompilerGenerated]
	internal static void <CalculateChunkBounds>g__EncapsulateWithoutShadowsBounds|0_4(Bounds extraBounds, ref ChunkSizeCalculator.<>c__DisplayClass0_0 A_1)
	{
		if (!A_1.boundsWithoutShadowsWereSet)
		{
			A_1.boundsWithoutShadows = extraBounds;
			A_1.boundsWithoutShadowsWereSet = true;
			return;
		}
		A_1.boundsWithoutShadows.Encapsulate(extraBounds);
	}

	// Token: 0x06002F47 RID: 12103 RVA: 0x000E2EAC File Offset: 0x000E10AC
	[CompilerGenerated]
	internal static void <CalculateChunkBounds>g__EncapsulateContributor|0_5(ChunkBoundsContributor contributor, ref ChunkSizeCalculator.<>c__DisplayClass0_0 A_1)
	{
		if (contributor.UseSeparateBounds)
		{
			Bounds bounds;
			if (contributor.TryGetBoundsWithShadows(out bounds))
			{
				ChunkSizeCalculator.<CalculateChunkBounds>g__EncapsulateWithShadowsBounds|0_3(bounds, ref A_1);
			}
			Bounds bounds2;
			if (contributor.TryGetBoundsWithoutShadows(out bounds2))
			{
				ChunkSizeCalculator.<CalculateChunkBounds>g__EncapsulateWithoutShadowsBounds|0_4(bounds2, ref A_1);
			}
			return;
		}
		Bounds bounds3;
		if (contributor.TryGetBounds(out bounds3))
		{
			ChunkSizeCalculator.<CalculateChunkBounds>g__EncapsulateBounds|0_2(bounds3, ref A_1);
		}
	}
}
