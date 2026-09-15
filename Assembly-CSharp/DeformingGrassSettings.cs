using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020004FE RID: 1278
[CreateAssetMenu(fileName = "DeformingGrassSettings", menuName = "GK2/Deforming Grass Settings")]
public class DeformingGrassSettings : LazySingletonSO<DeformingGrassSettings>
{
	// Token: 0x0600213E RID: 8510 RVA: 0x0009CBC0 File Offset: 0x0009ADC0
	public void ApplyShadowSettings()
	{
		DeformingGrass[] array = global::UnityEngine.Object.FindObjectsOfType<DeformingGrass>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ApplyShadowSettings();
		}
	}

	// Token: 0x04001DCF RID: 7631
	public bool grassShadow = true;

	// Token: 0x04001DD0 RID: 7632
	[Space(20f)]
	public int grassTextureWidth = 16;

	// Token: 0x04001DD1 RID: 7633
	public int grassTextureHeight = 64;
}
