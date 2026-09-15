using System;
using System.Text.RegularExpressions;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200051A RID: 1306
[Serializable]
public class Object3DSubMesh
{
	// Token: 0x1700057E RID: 1406
	// (get) Token: 0x060021B9 RID: 8633 RVA: 0x0009E911 File Offset: 0x0009CB11
	public Object3DSubMesh.MeshType Type
	{
		get
		{
			return this.type;
		}
	}

	// Token: 0x1700057F RID: 1407
	// (get) Token: 0x060021BA RID: 8634 RVA: 0x0009E919 File Offset: 0x0009CB19
	public string MainTextureName
	{
		get
		{
			Texture2D texture2D = this.Texture;
			if (texture2D == null)
			{
				return null;
			}
			return texture2D.name;
		}
	}

	// Token: 0x17000580 RID: 1408
	// (get) Token: 0x060021BB RID: 8635 RVA: 0x0009E92C File Offset: 0x0009CB2C
	public Texture2D Texture
	{
		get
		{
			return this.texture;
		}
	}

	// Token: 0x17000581 RID: 1409
	// (get) Token: 0x060021BC RID: 8636 RVA: 0x0009E934 File Offset: 0x0009CB34
	private Texture2D Texture2
	{
		get
		{
			return this.texture2;
		}
	}

	// Token: 0x17000582 RID: 1410
	// (get) Token: 0x060021BD RID: 8637 RVA: 0x0009E93C File Offset: 0x0009CB3C
	private Texture2D NormalMap
	{
		get
		{
			return this.normalMap;
		}
	}

	// Token: 0x17000583 RID: 1411
	// (get) Token: 0x060021BE RID: 8638 RVA: 0x0009E944 File Offset: 0x0009CB44
	public bool UseCrownDeformOnly
	{
		get
		{
			return this.useCrownDeformOnly;
		}
	}

	// Token: 0x17000584 RID: 1412
	// (get) Token: 0x060021BF RID: 8639 RVA: 0x0009E94C File Offset: 0x0009CB4C
	public bool HasLutTexture
	{
		get
		{
			return this.lutTexture;
		}
	}

	// Token: 0x17000585 RID: 1413
	// (get) Token: 0x060021C0 RID: 8640 RVA: 0x0009E959 File Offset: 0x0009CB59
	public Texture2D LutTexture
	{
		get
		{
			return this.lutTexture;
		}
	}

	// Token: 0x17000586 RID: 1414
	// (get) Token: 0x060021C1 RID: 8641 RVA: 0x0009E961 File Offset: 0x0009CB61
	public bool IsShadowMesh
	{
		get
		{
			this.EnsureMeshNameFlagsCached();
			return this.meshNameFlags.IsShadowMesh;
		}
	}

	// Token: 0x17000587 RID: 1415
	// (get) Token: 0x060021C2 RID: 8642 RVA: 0x0009E974 File Offset: 0x0009CB74
	public bool IsGardenPlant
	{
		get
		{
			this.EnsureMeshNameFlagsCached();
			return this.meshNameFlags.IsGardenPlant;
		}
	}

	// Token: 0x060021C3 RID: 8643 RVA: 0x0009E987 File Offset: 0x0009CB87
	public void Init(Object3DMesh parentMesh, Renderer renderer, int materialIndex)
	{
		this.parentMesh = parentMesh;
		this.materialIndex = materialIndex;
		this.renderer = renderer;
		this.cachedSharedMaterials = null;
		this.CacheMeshNameFlags();
	}

	// Token: 0x060021C4 RID: 8644 RVA: 0x0009E9AB File Offset: 0x0009CBAB
	public void SetTexture(Texture2D texture)
	{
		if (texture == null)
		{
			return;
		}
		if (this.type != Object3DSubMesh.MeshType.TreeCrown && this.IsTreeFsTexture(texture.name))
		{
			this.type = Object3DSubMesh.MeshType.TreeCrown;
		}
		this.texture = texture;
		this.ApplyMaterial();
	}

	// Token: 0x060021C5 RID: 8645 RVA: 0x0009E9E4 File Offset: 0x0009CBE4
	public void SetTextureOnly(Texture2D texture)
	{
		if (texture == null || this.renderer == null)
		{
			return;
		}
		this.texture = texture;
		if (this.matPropertyBlock == null)
		{
			this.matPropertyBlock = new MaterialPropertyBlock();
		}
		this.renderer.GetPropertyBlock(this.matPropertyBlock, this.materialIndex);
		this.matPropertyBlock.SetTexture(Object3DMesh.matIdMainTexture, texture);
		this.renderer.SetPropertyBlock(this.matPropertyBlock, this.materialIndex);
		this.isTextureSet = true;
	}

	// Token: 0x060021C6 RID: 8646 RVA: 0x0009EA6C File Offset: 0x0009CC6C
	public void TrySetTexture2(Texture2D texture)
	{
		if (texture == null)
		{
			return;
		}
		string text;
		if (this.IsTreeFmNumericTexture(texture.name, out text) && !this.texture.name.EndsWith("_fs" + text))
		{
			return;
		}
		this.texture2 = texture;
		this.ApplyMaterial();
	}

	// Token: 0x060021C7 RID: 8647 RVA: 0x0009EABE File Offset: 0x0009CCBE
	public void SetNormalMap(Texture2D normalMap)
	{
		this.normalMap = normalMap;
		this.ApplyMaterial();
	}

	// Token: 0x060021C8 RID: 8648 RVA: 0x0009EACD File Offset: 0x0009CCCD
	public void SetUnlit(bool isUnlit)
	{
		this.isUnlit = isUnlit;
		this.ApplyMaterial();
	}

	// Token: 0x060021C9 RID: 8649 RVA: 0x0009EADC File Offset: 0x0009CCDC
	public void SetLutTexture(Texture2D texture)
	{
		this.lutTexture = texture;
		this.ApplyMaterial();
	}

	// Token: 0x060021CA RID: 8650 RVA: 0x0009EAEC File Offset: 0x0009CCEC
	public void ApplyMaterial()
	{
		if (this.renderer == null)
		{
			return;
		}
		this.ApplyAppropriateMaterial();
		this.EnsureInitialized();
		this.ForceSharedMaterials();
		this.ValidatePropertyBlock();
		this.matPropertyBlock.SetInt(Object3DMesh.matIdOpaqueShadowMesh, this.isOpaqueMesh ? 1 : 0);
		this.isTextureSet = this.TrySetPropertyBlockTexture(Object3DMesh.matIdMainTexture, this.Texture);
		this.isTexture2Set = this.TrySetPropertyBlockTexture(Object3DMesh.matIdMainTexture2, this.Texture2);
		this.isNormalSet = this.TrySetPropertyBlockTexture(Object3DMesh.matIdNormalTexture, this.NormalMap);
		this.isLutTextureSet = this.TrySetPropertyBlockTexture(Object3DMesh.matIdLutTexture, this.lutTexture);
		this.matPropertyBlock.SetFloat(Object3DMesh.matIdUseNormalMap, (float)this.isNormalSet.ToInt(0));
		this.matPropertyBlock.SetFloat(Object3DMesh.matIdIsTreeCrown, (float)(this.type == Object3DSubMesh.MeshType.TreeCrown).ToInt(0));
		Object3DSubMesh.VegetationType vegetationType = this.vegetationType;
		if (vegetationType != Object3DSubMesh.VegetationType.Tree)
		{
			if (vegetationType == Object3DSubMesh.VegetationType.Bush)
			{
				this.matPropertyBlock.SetFloat(Object3DMesh.matIdTreeChopAmplitude, Object3DSubMesh.bushChopAmplitude);
			}
		}
		else
		{
			this.matPropertyBlock.SetFloat(Object3DMesh.matIdTreeChopAmplitude, Object3DSubMesh.treeChopAmplitude);
		}
		this.matPropertyBlock.SetFloat(Object3DMesh.matIdUseCrownDeformOnly, (float)this.useCrownDeformOnly.ToInt(0));
		this.matPropertyBlock.SetFloat(Object3DMesh.matIdIsUnlit, (float)this.isUnlit.ToInt(0));
		if (this.parentMesh.isTreeDestructing)
		{
			this.matPropertyBlock.SetFloat(Object3DSubMesh.matIdIsTreeDestructing, 1f);
			this.matPropertyBlock.SetFloat(Object3DSubMesh.matIdChoppingPhase, this.parentMesh.treeChoppingPhase);
			this.matPropertyBlock.SetFloat(Object3DSubMesh.matIdDestructionPhase, this.parentMesh.treeDestructionPhase);
			this.matPropertyBlock.SetFloat(Object3DSubMesh.matIdTreeHeight, this.parentMesh.treeHeight);
		}
		this.matPropertyBlock.SetFloat(Object3DMesh.matIdReplaceBlue, (float)this.useBlueReplace.ToInt(0));
		if (this.useBlueReplace)
		{
			this.matPropertyBlock.SetColor(Object3DMesh.matIdReplaceBlueColor, this.parentMesh.isBlueReplacingToColor ? Object3DMesh.replaceBlueColor : Object3DMesh.replaceBlueTransparent);
		}
		this.matPropertyBlock.SetFloat(Object3DMesh.matIdAlphaCutoff, this.isUnlit ? 0f : 0.5f);
		this.matPropertyBlock.SetFloat(Object3DMesh.matIdTransparencyOcclusionValue, (!this.ignoreTransparencySet) ? this.parentMesh.transparencyValue : 0f);
		this.matPropertyBlock.SetFloat(Object3DMesh.matIdWorldWindEnabled, (float)(this.isWorldWindEnabled ? 1 : 0));
		this.matPropertyBlock.SetColor(Object3DMesh.matIdSelectionTintColor, this.parentMesh.SelectionTintColor);
		this.matPropertyBlock.SetFloat(Object3DMesh.matIdSelectionTintAmount, this.parentMesh.SelectionTintAmount);
		this.renderer.SetPropertyBlock(this.matPropertyBlock, this.materialIndex);
	}

	// Token: 0x060021CB RID: 8651 RVA: 0x0009EDC4 File Offset: 0x0009CFC4
	public void SetSelectionTint(Color color, float amount)
	{
		if (this.renderer == null)
		{
			return;
		}
		if (this.matPropertyBlock == null)
		{
			this.matPropertyBlock = new MaterialPropertyBlock();
		}
		this.renderer.GetPropertyBlock(this.matPropertyBlock, this.materialIndex);
		this.matPropertyBlock.SetColor(Object3DMesh.matIdSelectionTintColor, color);
		this.matPropertyBlock.SetFloat(Object3DMesh.matIdSelectionTintAmount, amount);
		this.renderer.SetPropertyBlock(this.matPropertyBlock, this.materialIndex);
	}

	// Token: 0x060021CC RID: 8652 RVA: 0x0009EE44 File Offset: 0x0009D044
	private void ForceSharedMaterials()
	{
		Material sharedMaterial = this.renderer.sharedMaterial;
		if (sharedMaterial == null)
		{
			return;
		}
		Material matObject3DDeforming = LazySingletonSO<GlobalResources>.Instance.matObject3DDeforming;
		if (sharedMaterial == matObject3DDeforming)
		{
			return;
		}
		if (sharedMaterial.name == matObject3DDeforming.name)
		{
			this.renderer.sharedMaterial = matObject3DDeforming;
			this.cachedSharedMaterials = null;
		}
	}

	// Token: 0x060021CD RID: 8653 RVA: 0x0009EEA2 File Offset: 0x0009D0A2
	private void EnsureMeshNameFlagsCached()
	{
		if (!this.meshNameFlags.IsCached)
		{
			this.CacheMeshNameFlags();
		}
	}

	// Token: 0x060021CE RID: 8654 RVA: 0x0009EEB7 File Offset: 0x0009D0B7
	private void CacheMeshNameFlags()
	{
		this.meshNameFlags = Object3DSubMesh.MeshNameFlags.FromRenderer(this.renderer);
	}

	// Token: 0x060021CF RID: 8655 RVA: 0x0009EECA File Offset: 0x0009D0CA
	private Material[] GetSharedMaterials()
	{
		if (this.cachedSharedMaterials == null)
		{
			this.cachedSharedMaterials = this.renderer.sharedMaterials;
		}
		return this.cachedSharedMaterials;
	}

	// Token: 0x060021D0 RID: 8656 RVA: 0x0009EEEB File Offset: 0x0009D0EB
	private void EnsureInitialized()
	{
		if (this.matPropertyBlock == null)
		{
			this.matPropertyBlock = new MaterialPropertyBlock();
		}
	}

	// Token: 0x060021D1 RID: 8657 RVA: 0x0009EF00 File Offset: 0x0009D100
	private void ValidatePropertyBlock()
	{
		if (this.IsPropertyBlockTextureWasUnSet(this.Texture, this.isTextureSet) || this.IsPropertyBlockTextureWasUnSet(this.NormalMap, this.isNormalSet) || this.IsPropertyBlockTextureWasUnSet(this.Texture2, this.isTexture2Set))
		{
			this.matPropertyBlock = new MaterialPropertyBlock();
		}
		this.matPropertyBlock.Clear();
	}

	// Token: 0x060021D2 RID: 8658 RVA: 0x0009EF5F File Offset: 0x0009D15F
	private void ClearPropertyBlockTexture(int propertyId)
	{
		this.matPropertyBlock.SetTexture(propertyId, Texture2D.blackTexture);
	}

	// Token: 0x060021D3 RID: 8659 RVA: 0x0009EF72 File Offset: 0x0009D172
	private bool TrySetPropertyBlockTexture(int propertyId, Texture2D texture2D)
	{
		if (texture2D != null)
		{
			this.matPropertyBlock.SetTexture(propertyId, texture2D);
		}
		return texture2D != null;
	}

	// Token: 0x060021D4 RID: 8660 RVA: 0x0009EF91 File Offset: 0x0009D191
	private bool IsPropertyBlockTextureWasUnSet(Texture2D texture2D, bool isSetFlag)
	{
		return isSetFlag && texture2D == null;
	}

	// Token: 0x060021D5 RID: 8661 RVA: 0x0009EF9F File Offset: 0x0009D19F
	private bool IsTreeFsTexture(string name)
	{
		return Object3DSubMesh.TreeFsTextureRegex.IsMatch(name);
	}

	// Token: 0x060021D6 RID: 8662 RVA: 0x0009EFAC File Offset: 0x0009D1AC
	private bool IsTreeFmNumericTexture(string name, out string digits)
	{
		Match match = Object3DSubMesh.TreeFmNumericTextureRegex.Match(name);
		digits = (match.Success ? match.Groups[1].Value : string.Empty);
		return match.Success;
	}

	// Token: 0x060021D7 RID: 8663 RVA: 0x0009EFF0 File Offset: 0x0009D1F0
	private void ApplyAppropriateMaterial()
	{
		this.EnsureMeshNameFlagsCached();
		Object3DSubMesh.MeshNameFlags meshNameFlags = this.meshNameFlags;
		if (meshNameFlags.IsWaterSurfaceMesh)
		{
			return;
		}
		Material[] sharedMaterials = this.GetSharedMaterials();
		Material material = sharedMaterials[this.materialIndex];
		if (this.HasLutTexture)
		{
			Material matObject3DLUT = LazySingletonSO<GlobalResources>.Instance.matObject3DLUT;
			if (material != matObject3DLUT)
			{
				sharedMaterials[this.materialIndex] = matObject3DLUT;
				this.ApplyMaterials(sharedMaterials);
			}
			return;
		}
		if (meshNameFlags.IsShadowMesh)
		{
			Material shadowMaterial = LazySingletonSO<GlobalResources>.Instance.shadowMaterial;
			if (material != shadowMaterial)
			{
				sharedMaterials[this.materialIndex] = shadowMaterial;
				this.ApplyMaterials(sharedMaterials);
			}
			return;
		}
		if (meshNameFlags.IsGardenPlant)
		{
			Material matObject3DDeforming = LazySingletonSO<GlobalResources>.Instance.matObject3DDeforming;
			if (material != matObject3DDeforming)
			{
				sharedMaterials[this.materialIndex] = matObject3DDeforming;
				this.ApplyMaterials(sharedMaterials);
			}
			return;
		}
		if (meshNameFlags.IsBlackoutMesh)
		{
			Material matBlackout = LazySingletonSO<GlobalResources>.Instance.matBlackout;
			if (material != matBlackout)
			{
				sharedMaterials[this.materialIndex] = matBlackout;
				this.ApplyMaterials(sharedMaterials);
			}
			return;
		}
		if (meshNameFlags.IsWindClothMesh)
		{
			Material windClothMaterial = LazySingletonSO<GlobalResources>.Instance.windClothMaterial;
			if (material != windClothMaterial)
			{
				sharedMaterials[this.materialIndex] = windClothMaterial;
				this.ApplyMaterials(sharedMaterials);
			}
			return;
		}
		if (meshNameFlags.IsTreeLeavesMesh)
		{
			Material material2;
			if (this.ignoreTransparencySet || this.parentMesh.transparencyValue.EqualsTo(0f, 1E-05f))
			{
				material2 = LazySingletonSO<GlobalResources>.Instance.matObject3D;
				if (material == material2)
				{
					return;
				}
				LazySingletonSO<GlobalResources>.Instance.ReleaseTransparentMaterialFor(this.parentMesh);
			}
			else
			{
				Material material3;
				LazySingletonSO<GlobalResources>.Instance.GetTransparentMaterialFor(this.parentMesh, LazySingletonSO<GlobalResources>.Instance.TransparentMaterialPlus1, out material3, -0.01f);
				material2 = material3;
			}
			sharedMaterials[this.materialIndex] = material2;
			this.ApplyMaterials(sharedMaterials);
			return;
		}
		if (meshNameFlags.IsTreeMesh)
		{
			Material material4;
			if (this.ignoreTransparencySet || this.parentMesh.transparencyValue.EqualsTo(0f, 1E-05f))
			{
				material4 = LazySingletonSO<GlobalResources>.Instance.matObject3D;
				if (material == material4)
				{
					return;
				}
				LazySingletonSO<GlobalResources>.Instance.ReleaseTransparentMaterialFor(this.parentMesh);
			}
			else
			{
				Material material5;
				LazySingletonSO<GlobalResources>.Instance.GetTransparentMaterialFor(this.parentMesh, LazySingletonSO<GlobalResources>.Instance.TransparentMaterial, out material5, 0f);
				material4 = material5;
			}
			sharedMaterials[this.materialIndex] = material4;
			this.ApplyMaterials(sharedMaterials);
			return;
		}
		sharedMaterials[this.materialIndex] = LazySingletonSO<GlobalResources>.Instance.matObject3D;
		this.ApplyMaterials(sharedMaterials);
	}

	// Token: 0x060021D8 RID: 8664 RVA: 0x0009F249 File Offset: 0x0009D449
	private void ApplyMaterials(Material[] mats)
	{
		this.renderer.sharedMaterials = mats;
		this.cachedSharedMaterials = mats;
	}

	// Token: 0x04001E51 RID: 7761
	private const bool isEditor = false;

	// Token: 0x04001E52 RID: 7762
	private static readonly int matIdDestructionPhase = Shader.PropertyToID("_TreeDestrPhase");

	// Token: 0x04001E53 RID: 7763
	private static readonly int matIdChoppingPhase = Shader.PropertyToID("_TreeChopPhase");

	// Token: 0x04001E54 RID: 7764
	private static readonly int matIdIsTreeDestructing = Shader.PropertyToID("_TreeDestruction");

	// Token: 0x04001E55 RID: 7765
	private static readonly int matIdTreeHeight = Shader.PropertyToID("_TreeHeight");

	// Token: 0x04001E56 RID: 7766
	private static readonly float treeChopAmplitude = 0.45f;

	// Token: 0x04001E57 RID: 7767
	private static readonly float bushChopAmplitude = 0.125f;

	// Token: 0x04001E58 RID: 7768
	private LocalKeyword useReplaceColorLocalKeyword;

	// Token: 0x04001E59 RID: 7769
	[SerializeField]
	private Object3DSubMesh.MeshType type;

	// Token: 0x04001E5A RID: 7770
	[SerializeField]
	private Texture2D texture;

	// Token: 0x04001E5B RID: 7771
	[SerializeField]
	[HideInInspector]
	private bool isTextureSet;

	// Token: 0x04001E5C RID: 7772
	[SerializeField]
	private bool useCrownDeformOnly;

	// Token: 0x04001E5D RID: 7773
	[SerializeField]
	private Object3DSubMesh.VegetationType vegetationType;

	// Token: 0x04001E5E RID: 7774
	[SerializeField]
	private Texture2D texture2;

	// Token: 0x04001E5F RID: 7775
	[SerializeField]
	[HideInInspector]
	private bool isTexture2Set;

	// Token: 0x04001E60 RID: 7776
	[SerializeField]
	private Texture2D normalMap;

	// Token: 0x04001E61 RID: 7777
	[SerializeField]
	[HideInInspector]
	private bool isNormalSet;

	// Token: 0x04001E62 RID: 7778
	[SerializeField]
	private Texture2D lutTexture;

	// Token: 0x04001E63 RID: 7779
	[SerializeField]
	[HideInInspector]
	private bool isLutTextureSet;

	// Token: 0x04001E64 RID: 7780
	[SerializeField]
	private bool isOpaqueMesh;

	// Token: 0x04001E65 RID: 7781
	[SerializeField]
	private bool useBlueReplace;

	// Token: 0x04001E66 RID: 7782
	[SerializeField]
	private bool isWorldWindEnabled;

	// Token: 0x04001E67 RID: 7783
	[SerializeField]
	private bool ignoreTransparencySet;

	// Token: 0x04001E68 RID: 7784
	[SerializeField]
	[HideInInspector]
	private bool isUnlit;

	// Token: 0x04001E69 RID: 7785
	private MaterialPropertyBlock matPropertyBlock;

	// Token: 0x04001E6A RID: 7786
	private Material[] cachedSharedMaterials;

	// Token: 0x04001E6B RID: 7787
	private int materialIndex;

	// Token: 0x04001E6C RID: 7788
	private Renderer renderer;

	// Token: 0x04001E6D RID: 7789
	private Object3DMesh parentMesh;

	// Token: 0x04001E6E RID: 7790
	private Object3DSubMesh.MeshNameFlags meshNameFlags;

	// Token: 0x04001E6F RID: 7791
	private static readonly Regex TreeFsTextureRegex = new Regex("_fs\\d{1,2}$", RegexOptions.Compiled);

	// Token: 0x04001E70 RID: 7792
	private static readonly Regex TreeFmNumericTextureRegex = new Regex("_fm(\\d{1,2})$", RegexOptions.Compiled);

	// Token: 0x0200051B RID: 1307
	public enum MeshType
	{
		// Token: 0x04001E72 RID: 7794
		Regular,
		// Token: 0x04001E73 RID: 7795
		TreeCrown
	}

	// Token: 0x0200051C RID: 1308
	private enum VegetationType
	{
		// Token: 0x04001E75 RID: 7797
		Tree,
		// Token: 0x04001E76 RID: 7798
		Bush
	}

	// Token: 0x0200051D RID: 1309
	private struct MeshNameFlags
	{
		// Token: 0x060021DB RID: 8667 RVA: 0x0009F2E0 File Offset: 0x0009D4E0
		public static Object3DSubMesh.MeshNameFlags FromRenderer(Renderer renderer)
		{
			Object3DSubMesh.MeshNameFlags meshNameFlags;
			if (renderer == null || renderer.gameObject == null)
			{
				meshNameFlags = default(Object3DSubMesh.MeshNameFlags);
				return meshNameFlags;
			}
			string name = renderer.gameObject.name;
			bool flag = name.StartsWith("tree_");
			meshNameFlags = new Object3DSubMesh.MeshNameFlags
			{
				IsCached = true,
				IsWaterSurfaceMesh = (name.EndsWith("-ws") || name.EndsWith("_ws")),
				IsShadowMesh = (name.EndsWith("_sh") || name.EndsWith("-sh") || name.EndsWith("_sh2") || name.EndsWith("-sh2")),
				IsGardenPlant = (name.Contains("_plant_") && name.Contains("_stage_")),
				IsBlackoutMesh = name.EndsWith("-blackout"),
				IsWindClothMesh = name.EndsWith("-wind"),
				IsTreeMesh = flag,
				IsTreeLeavesMesh = (flag && name.Contains("_fm"))
			};
			return meshNameFlags;
		}

		// Token: 0x04001E77 RID: 7799
		public bool IsCached;

		// Token: 0x04001E78 RID: 7800
		public bool IsWaterSurfaceMesh;

		// Token: 0x04001E79 RID: 7801
		public bool IsShadowMesh;

		// Token: 0x04001E7A RID: 7802
		public bool IsGardenPlant;

		// Token: 0x04001E7B RID: 7803
		public bool IsBlackoutMesh;

		// Token: 0x04001E7C RID: 7804
		public bool IsWindClothMesh;

		// Token: 0x04001E7D RID: 7805
		public bool IsTreeMesh;

		// Token: 0x04001E7E RID: 7806
		public bool IsTreeLeavesMesh;
	}
}
