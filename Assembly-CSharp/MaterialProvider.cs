using System;
using UnityEngine;

// Token: 0x0200050A RID: 1290
public class MaterialProvider : MonoBehaviour
{
	// Token: 0x0600215E RID: 8542 RVA: 0x0009D8A7 File Offset: 0x0009BAA7
	private void Awake()
	{
		if (this.targetRenderer == null)
		{
			this.targetRenderer = base.GetComponent<Renderer>();
		}
	}

	// Token: 0x0600215F RID: 8543 RVA: 0x0009D8C3 File Offset: 0x0009BAC3
	private void Start()
	{
		this.ApplyMaterial();
	}

	// Token: 0x06002160 RID: 8544 RVA: 0x0009D8CB File Offset: 0x0009BACB
	public void ApplyMaterial()
	{
		if (this.targetRenderer == null || this.multiMaterial == null)
		{
			return;
		}
		this.targetRenderer.sharedMaterial = this.ResolveMaterial();
	}

	// Token: 0x06002161 RID: 8545 RVA: 0x0009D8FB File Offset: 0x0009BAFB
	public void SetMultiMaterial(MultiMaterial multiMaterial)
	{
		this.multiMaterial = multiMaterial;
	}

	// Token: 0x06002162 RID: 8546 RVA: 0x0009D904 File Offset: 0x0009BB04
	private Material ResolveMaterial()
	{
		return this.multiMaterial.GetMaterial(MaterialProvider.GetMaterialType());
	}

	// Token: 0x06002163 RID: 8547 RVA: 0x0009D916 File Offset: 0x0009BB16
	public static PlatformSpecificMaterialType GetMaterialType()
	{
		return PlatformFeatures.GetWaterMaterialType();
	}

	// Token: 0x06002164 RID: 8548 RVA: 0x0009D920 File Offset: 0x0009BB20
	public static bool ShouldKeepMaterialForBuild(string fieldName)
	{
		PlatformSpecificMaterialType platformSpecificMaterialType;
		return !MaterialProvider.TryGetMaterialTypeFromFieldName(fieldName, out platformSpecificMaterialType) || platformSpecificMaterialType == MaterialProvider.GetMaterialType();
	}

	// Token: 0x06002165 RID: 8549 RVA: 0x0009D944 File Offset: 0x0009BB44
	private static bool TryGetMaterialTypeFromFieldName(string fieldName, out PlatformSpecificMaterialType type)
	{
		if (fieldName == "standaloneMaterial")
		{
			type = PlatformSpecificMaterialType.Standalone;
			return true;
		}
		if (fieldName == "switchMaterial")
		{
			type = PlatformSpecificMaterialType.Switch;
			return true;
		}
		if (fieldName == "switchMaterialSimple")
		{
			type = PlatformSpecificMaterialType.SwitchSimple;
			return true;
		}
		if (fieldName == "mobileMaterial")
		{
			type = PlatformSpecificMaterialType.Mobile;
			return true;
		}
		if (!(fieldName == "minimalMaterial"))
		{
			type = PlatformSpecificMaterialType.Standalone;
			return false;
		}
		type = PlatformSpecificMaterialType.Minimal;
		return true;
	}

	// Token: 0x04001DF4 RID: 7668
	[SerializeField]
	private MultiMaterial multiMaterial;

	// Token: 0x04001DF5 RID: 7669
	[SerializeField]
	private Renderer targetRenderer;
}
