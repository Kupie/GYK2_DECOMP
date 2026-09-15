using System;
using UnityEngine;

// Token: 0x0200050D RID: 1293
[CreateAssetMenu(menuName = "GK2/Materials/Platform Specific Multi Material", fileName = "PlatformSpecificMultiMaterial")]
public class PlatformSpecificMultiMaterial : MultiMaterial
{
	// Token: 0x06002169 RID: 8553 RVA: 0x0009D9B4 File Offset: 0x0009BBB4
	public override Material GetMaterial(PlatformSpecificMaterialType platform)
	{
		Material material;
		switch (platform)
		{
		case PlatformSpecificMaterialType.Switch:
			material = this.switchMaterial;
			break;
		case PlatformSpecificMaterialType.SwitchSimple:
			material = this.switchMaterialSimple;
			break;
		case PlatformSpecificMaterialType.Mobile:
			material = this.mobileMaterial;
			break;
		case PlatformSpecificMaterialType.Minimal:
			material = this.minimalMaterial;
			break;
		default:
			material = this.standaloneMaterial;
			break;
		}
		return material;
	}

	// Token: 0x04001DFC RID: 7676
	[SerializeField]
	private Material standaloneMaterial;

	// Token: 0x04001DFD RID: 7677
	[SerializeField]
	private Material switchMaterial;

	// Token: 0x04001DFE RID: 7678
	[SerializeField]
	private Material switchMaterialSimple;

	// Token: 0x04001DFF RID: 7679
	[SerializeField]
	private Material mobileMaterial;

	// Token: 0x04001E00 RID: 7680
	[SerializeField]
	private Material minimalMaterial;
}
