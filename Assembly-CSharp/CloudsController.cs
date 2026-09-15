using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000B14 RID: 2836
[ExecuteAlways]
[DefaultExecutionOrder(20)]
public class CloudsController : MonoBehaviour
{
	// Token: 0x06004B7B RID: 19323 RVA: 0x00164CEF File Offset: 0x00162EEF
	private void OnEnable()
	{
		this.EnsureRuntimeState();
		this.lastAppearance = (PlatformCloudAppearance)(-1);
		this.lastWindIntegrateTime = -1f;
		this.loggedMissingRtPathRefs = false;
		this.loggedMissingReplacedPathRefs = false;
	}

	// Token: 0x06004B7C RID: 19324 RVA: 0x00164D17 File Offset: 0x00162F17
	private void OnDisable()
	{
		this.ApplyQuadVisibility(true, false);
		this.ClearReplacedPropertyBlock();
		this.ClearGlobals();
		this.ReleaseRT();
		this.lastAppearance = (PlatformCloudAppearance)(-1);
		this.lastWindIntegrateTime = -1f;
	}

	// Token: 0x06004B7D RID: 19325 RVA: 0x00164D45 File Offset: 0x00162F45
	private void OnDestroy()
	{
		CommandBuffer commandBuffer = this.commandBuffer;
		if (commandBuffer != null)
		{
			commandBuffer.Release();
		}
		this.commandBuffer = null;
	}

	// Token: 0x06004B7E RID: 19326 RVA: 0x00164D5F File Offset: 0x00162F5F
	private void EnsureRuntimeState()
	{
		if (this.commandBuffer == null)
		{
			this.commandBuffer = new CommandBuffer
			{
				name = "Clouds Shadow RT Capture"
			};
		}
		if (this.propertyBlock == null)
		{
			this.propertyBlock = new MaterialPropertyBlock();
		}
	}

	// Token: 0x06004B7F RID: 19327 RVA: 0x00164D94 File Offset: 0x00162F94
	private void LateUpdate()
	{
		this.EnsureRuntimeState();
		PlatformCloudAppearance activeAppearance = CloudsShadowPolicy.ActiveAppearance;
		if (activeAppearance != this.lastAppearance)
		{
			this.OnAppearanceChanged(this.lastAppearance, activeAppearance);
			this.lastAppearance = activeAppearance;
		}
		if (CloudsShadowPolicy.UseCloudsRTOverlay)
		{
			this.UpdateRenderTexturePath();
			return;
		}
		this.ClearGlobals();
		if (CloudsShadowPolicy.UseReplacedCloudMaterial)
		{
			this.UpdateReplacedPath();
			return;
		}
		this.ApplyQuadVisibility(true, false);
		this.ClearReplacedPropertyBlock();
	}

	// Token: 0x06004B80 RID: 19328 RVA: 0x00164DFA File Offset: 0x00162FFA
	private void OnAppearanceChanged(PlatformCloudAppearance previous, PlatformCloudAppearance next)
	{
		this.forwardPassIndex = -1;
		if (previous == PlatformCloudAppearance.RenderTexture && next != PlatformCloudAppearance.RenderTexture)
		{
			this.ClearGlobals();
		}
		if (next != PlatformCloudAppearance.Replaced)
		{
			this.ClearReplacedPropertyBlock();
		}
	}

	// Token: 0x06004B81 RID: 19329 RVA: 0x00164E1C File Offset: 0x0016301C
	private void UpdateRenderTexturePath()
	{
		MeshFilter meshFilter = this.GetDefaultMeshFilter();
		if (meshFilter == null || meshFilter.sharedMesh == null)
		{
			this.LogMissingConfigOnce(ref this.loggedMissingRtPathRefs, "RenderTexture mode needs defaultCloudsQuad (or a child MeshFilter) with a sharedMesh.");
			this.FallbackFromFailedRtPath();
			return;
		}
		Material material = this.ResolveRtMaterial();
		if (material == null)
		{
			this.LogMissingConfigOnce(ref this.loggedMissingRtPathRefs, "RenderTexture mode needs rtShadowMaterial assigned, or GlobalResources.cloudsMaterial.");
			this.FallbackFromFailedRtPath();
			return;
		}
		if (this.forwardPassIndex < 0)
		{
			this.forwardPassIndex = Mathf.Max(0, material.FindPass("FORWARD"));
		}
		this.ApplyQuadVisibility(false, false);
		this.ClearReplacedPropertyBlock();
		this.BuildPropertyBlock(material);
		this.EnsureRT();
		this.CaptureCloudsRT(meshFilter, material);
		this.PublishGlobals();
	}

	// Token: 0x06004B82 RID: 19330 RVA: 0x00164ED0 File Offset: 0x001630D0
	private void FallbackFromFailedRtPath()
	{
		this.ClearGlobals();
		this.ApplyQuadVisibility(true, false);
		this.ClearReplacedPropertyBlock();
	}

	// Token: 0x06004B83 RID: 19331 RVA: 0x00164EE8 File Offset: 0x001630E8
	private void UpdateReplacedPath()
	{
		if (this.replacedCloudsQuad == null)
		{
			this.LogMissingConfigOnce(ref this.loggedMissingReplacedPathRefs, "Replaced mode requires replacedCloudsQuad assigned on World Clouds.");
			this.FallbackFromFailedReplacedPath();
			return;
		}
		Material material = this.ResolveReplacedMaterial();
		MeshRenderer meshRenderer = this.GetReplacedMeshRenderer();
		if (material == null || meshRenderer == null)
		{
			this.LogMissingConfigOnce(ref this.loggedMissingReplacedPathRefs, "Replaced mode needs a MeshRenderer and material on replacedCloudsQuad (or replacedShadowMaterial).");
			this.FallbackFromFailedReplacedPath();
			return;
		}
		this.ApplyQuadVisibility(false, true);
		if (meshRenderer.sharedMaterial != material)
		{
			meshRenderer.sharedMaterial = material;
		}
		this.BuildPropertyBlock(material);
		meshRenderer.SetPropertyBlock(this.propertyBlock);
	}

	// Token: 0x06004B84 RID: 19332 RVA: 0x00164F83 File Offset: 0x00163183
	private void FallbackFromFailedReplacedPath()
	{
		this.ApplyQuadVisibility(true, false);
		this.ClearReplacedPropertyBlock();
	}

	// Token: 0x06004B85 RID: 19333 RVA: 0x00164F93 File Offset: 0x00163193
	private void ApplyQuadVisibility(bool defaultActive, bool replacedActive)
	{
		CloudsController.SetActiveIfNeeded(this.defaultCloudsQuad, defaultActive);
		CloudsController.SetActiveIfNeeded(this.replacedCloudsQuad, replacedActive);
	}

	// Token: 0x06004B86 RID: 19334 RVA: 0x00164FAD File Offset: 0x001631AD
	private static void SetActiveIfNeeded(GameObject go, bool active)
	{
		if (go != null && go.activeSelf != active)
		{
			go.SetActive(active);
		}
	}

	// Token: 0x06004B87 RID: 19335 RVA: 0x00164FC8 File Offset: 0x001631C8
	private void ClearReplacedPropertyBlock()
	{
		MeshRenderer meshRenderer = this.GetReplacedMeshRenderer();
		if (meshRenderer != null)
		{
			meshRenderer.SetPropertyBlock(null);
		}
	}

	// Token: 0x06004B88 RID: 19336 RVA: 0x00164FEC File Offset: 0x001631EC
	private MeshFilter GetDefaultMeshFilter()
	{
		if (this.defaultMeshFilter != null)
		{
			return this.defaultMeshFilter;
		}
		if (this.defaultCloudsQuad != null)
		{
			this.defaultMeshFilter = this.defaultCloudsQuad.GetComponent<MeshFilter>();
			if (this.defaultMeshFilter != null)
			{
				return this.defaultMeshFilter;
			}
		}
		this.defaultMeshFilter = base.GetComponentInChildren<MeshFilter>(true);
		if (this.defaultMeshFilter != null && this.defaultCloudsQuad == null)
		{
			this.defaultCloudsQuad = this.defaultMeshFilter.gameObject;
		}
		return this.defaultMeshFilter;
	}

	// Token: 0x06004B89 RID: 19337 RVA: 0x00165082 File Offset: 0x00163282
	private void LogMissingConfigOnce(ref bool logged, string message)
	{
		if (logged)
		{
			return;
		}
		logged = true;
		Debug.LogError("[CloudsController] " + message, this);
	}

	// Token: 0x06004B8A RID: 19338 RVA: 0x0016509D File Offset: 0x0016329D
	private MeshRenderer GetReplacedMeshRenderer()
	{
		if (this.replacedMeshRenderer == null && this.replacedCloudsQuad != null)
		{
			this.replacedMeshRenderer = this.replacedCloudsQuad.GetComponent<MeshRenderer>();
		}
		return this.replacedMeshRenderer;
	}

	// Token: 0x06004B8B RID: 19339 RVA: 0x001650D2 File Offset: 0x001632D2
	private Material ResolveRtMaterial()
	{
		if (this.rtShadowMaterial != null)
		{
			return this.rtShadowMaterial;
		}
		if (!(LazySingletonSO<GlobalResources>.Instance != null))
		{
			return null;
		}
		return LazySingletonSO<GlobalResources>.Instance.cloudsMaterial;
	}

	// Token: 0x06004B8C RID: 19340 RVA: 0x00165104 File Offset: 0x00163304
	private Material ResolveReplacedMaterial()
	{
		if (this.replacedShadowMaterial != null)
		{
			return this.replacedShadowMaterial;
		}
		MeshRenderer meshRenderer = this.GetReplacedMeshRenderer();
		if (!(meshRenderer != null))
		{
			return null;
		}
		return meshRenderer.sharedMaterial;
	}

	// Token: 0x06004B8D RID: 19341 RVA: 0x0016513E File Offset: 0x0016333E
	private void BuildPropertyBlock(Material material)
	{
		this.propertyBlock.Clear();
		this.propertyBlock.SetFloat(CloudsController.idCloudsDensity, CPCloudsDensity.ResolveDensityForClouds());
		this.AdvanceWindScroll(material);
		this.propertyBlock.SetFloat(CloudsController.idCloudWindOffset, this.windScrollOffset);
	}

	// Token: 0x06004B8E RID: 19342 RVA: 0x00165180 File Offset: 0x00163380
	private void AdvanceWindScroll(Material material)
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		float num = 0f;
		if (this.lastWindIntegrateTime >= 0f)
		{
			num = Mathf.Min(0.1f, Mathf.Max(0f, realtimeSinceStartup - this.lastWindIntegrateTime));
		}
		this.lastWindIntegrateTime = realtimeSinceStartup;
		float num2 = ((material != null && material.HasProperty(CloudsController.idCloudWindSpeed)) ? material.GetFloat(CloudsController.idCloudWindSpeed) : 0f);
		float num3 = ((material != null && material.HasProperty(CloudsController.idCloudWindAccel)) ? material.GetFloat(CloudsController.idCloudWindAccel) : 0f);
		float num4 = num2 + CloudsController.ReadWindValue() * num3;
		this.windScrollOffset += num4 * num;
	}

	// Token: 0x06004B8F RID: 19343 RVA: 0x00165234 File Offset: 0x00163434
	private static float ReadWindValue()
	{
		if (WeatherSystem.Instance != null)
		{
			return Mathf.Clamp01(WeatherSystem.Instance.WindValue);
		}
		return Mathf.Clamp01(Shader.GetGlobalFloat(GlobalShaderParameters.idWindValue));
	}

	// Token: 0x06004B90 RID: 19344 RVA: 0x00165264 File Offset: 0x00163464
	private void EnsureRT()
	{
		int num = Mathf.Max(1, this.resolution);
		if (this.cloudsRT != null && this.cloudsRT.width == num && this.cloudsRT.height == num)
		{
			return;
		}
		this.ReleaseRT();
		this.cloudsRT = new RenderTexture(num, num, 0, RenderTextureFormat.ARGB32)
		{
			name = "_CloudsRT",
			filterMode = FilterMode.Bilinear,
			wrapMode = TextureWrapMode.Clamp,
			useMipMap = false
		};
		this.cloudsRT.Create();
	}

	// Token: 0x06004B91 RID: 19345 RVA: 0x001652EC File Offset: 0x001634EC
	private void CaptureCloudsRT(MeshFilter meshFilter, Material material)
	{
		Bounds bounds = CloudsController.TransformBounds(meshFilter.transform, meshFilter.sharedMesh.bounds);
		Vector3 center = bounds.center;
		float num = Mathf.Max(0.01f, bounds.extents.x);
		float num2 = Mathf.Max(0.01f, bounds.extents.z);
		float num3 = bounds.max.y + 5f;
		float num4 = bounds.min.y - 5f;
		float num5 = Mathf.Max(1f, num3 - num4);
		Matrix4x4 inverse = Matrix4x4.TRS(new Vector3(center.x, num3, center.z), Quaternion.LookRotation(Vector3.down, Vector3.forward), new Vector3(1f, 1f, -1f)).inverse;
		Matrix4x4 matrix4x = Matrix4x4.Ortho(-num, num, -num2, num2, 0.01f, num5);
		Matrix4x4 gpuprojectionMatrix = GL.GetGPUProjectionMatrix(matrix4x, true);
		bool flag = matrix4x.m11 * gpuprojectionMatrix.m11 < 0f;
		this.commandBuffer.Clear();
		this.commandBuffer.SetRenderTarget(this.cloudsRT);
		this.commandBuffer.ClearRenderTarget(true, true, Color.clear);
		this.commandBuffer.SetViewProjectionMatrices(inverse, gpuprojectionMatrix);
		if (flag)
		{
			this.commandBuffer.SetInvertCulling(true);
		}
		this.commandBuffer.DrawMesh(meshFilter.sharedMesh, meshFilter.transform.localToWorldMatrix, material, 0, this.forwardPassIndex, this.propertyBlock);
		if (flag)
		{
			this.commandBuffer.SetInvertCulling(false);
		}
		Graphics.ExecuteCommandBuffer(this.commandBuffer);
		this.viewProjMatrix = gpuprojectionMatrix * inverse;
	}

	// Token: 0x06004B92 RID: 19346 RVA: 0x00165498 File Offset: 0x00163698
	private static Bounds TransformBounds(Transform t, Bounds localBounds)
	{
		Vector3 vector = t.TransformPoint(localBounds.center);
		Vector3 extents = localBounds.extents;
		Vector3 vector2 = t.TransformVector(extents.x, 0f, 0f);
		Vector3 vector3 = t.TransformVector(0f, extents.y, 0f);
		Vector3 vector4 = t.TransformVector(0f, 0f, extents.z);
		Vector3 vector5 = new Vector3(Mathf.Abs(vector2.x) + Mathf.Abs(vector3.x) + Mathf.Abs(vector4.x), Mathf.Abs(vector2.y) + Mathf.Abs(vector3.y) + Mathf.Abs(vector4.y), Mathf.Abs(vector2.z) + Mathf.Abs(vector3.z) + Mathf.Abs(vector4.z));
		return new Bounds(vector, vector5 * 2f);
	}

	// Token: 0x06004B93 RID: 19347 RVA: 0x00165580 File Offset: 0x00163780
	private void PublishGlobals()
	{
		Shader.SetGlobalTexture(CloudsController.idCloudsRT, this.cloudsRT);
		Shader.SetGlobalMatrix(CloudsController.idCloudsRTViewProjMatrix, this.viewProjMatrix);
		Shader.SetGlobalFloat(CloudsController.idCloudsRTIntensity, 1f);
		Shader.SetGlobalFloat(CloudsController.idCloudsShadowStrength, this.shadowStrength);
		this.SetKeyword(true);
	}

	// Token: 0x06004B94 RID: 19348 RVA: 0x001655D3 File Offset: 0x001637D3
	private void ClearGlobals()
	{
		Shader.SetGlobalTexture(CloudsController.idCloudsRT, Texture2D.blackTexture);
		Shader.SetGlobalFloat(CloudsController.idCloudsRTIntensity, 0f);
		this.SetKeyword(false);
	}

	// Token: 0x06004B95 RID: 19349 RVA: 0x001655FA File Offset: 0x001637FA
	private void SetKeyword(bool enable)
	{
		if (this.keywordApplied == enable)
		{
			return;
		}
		this.keywordApplied = enable;
		if (enable)
		{
			Shader.EnableKeyword("USE_CLOUDS_RT");
			return;
		}
		Shader.DisableKeyword("USE_CLOUDS_RT");
	}

	// Token: 0x06004B96 RID: 19350 RVA: 0x00165625 File Offset: 0x00163825
	private void ReleaseRT()
	{
		if (this.cloudsRT == null)
		{
			return;
		}
		this.cloudsRT.Release();
		global::UnityEngine.Object.Destroy(this.cloudsRT);
		this.cloudsRT = null;
	}

	// Token: 0x04003CD6 RID: 15574
	private const float CAPTURE_MARGIN = 5f;

	// Token: 0x04003CD7 RID: 15575
	private const float MaxWindDeltaTime = 0.1f;

	// Token: 0x04003CD8 RID: 15576
	private static readonly int idCloudsRT = Shader.PropertyToID("_CloudsRT");

	// Token: 0x04003CD9 RID: 15577
	private static readonly int idCloudsRTViewProjMatrix = Shader.PropertyToID("_CloudsRTViewProjMatrix");

	// Token: 0x04003CDA RID: 15578
	private static readonly int idCloudsRTIntensity = Shader.PropertyToID("_CloudsRTIntensity");

	// Token: 0x04003CDB RID: 15579
	private static readonly int idCloudsShadowStrength = Shader.PropertyToID("_CloudsShadowStrength");

	// Token: 0x04003CDC RID: 15580
	private static readonly int idCloudsDensity = Shader.PropertyToID("_CloudsDensity");

	// Token: 0x04003CDD RID: 15581
	private static readonly int idCloudWindSpeed = Shader.PropertyToID("_CloudWindSpeed");

	// Token: 0x04003CDE RID: 15582
	private static readonly int idCloudWindAccel = Shader.PropertyToID("_CloudWindAccel");

	// Token: 0x04003CDF RID: 15583
	private static readonly int idCloudWindOffset = Shader.PropertyToID("_CloudWindOffset");

	// Token: 0x04003CE0 RID: 15584
	[Tooltip("Default-mode ASE \"Clouds Shadow\" quad.")]
	[SerializeField]
	private GameObject defaultCloudsQuad;

	// Token: 0x04003CE1 RID: 15585
	[Tooltip("Replaced-mode procedural quad. Prefab starts inactive.")]
	[SerializeField]
	private GameObject replacedCloudsQuad;

	// Token: 0x04003CE2 RID: 15586
	[Tooltip("Procedural material used only for RenderTexture capture. Assign \"Clouds Shadow Procedural\". Falls back to GlobalResources.cloudsMaterial when empty.")]
	[SerializeField]
	private Material rtShadowMaterial;

	// Token: 0x04003CE3 RID: 15587
	[Tooltip("Procedural material for Replaced real-time shadows. Assign \"Clouds Shadow Procedural Replaced\". Falls back to the replaced quad's sharedMaterial.")]
	[SerializeField]
	private Material replacedShadowMaterial;

	// Token: 0x04003CE4 RID: 15588
	[Tooltip("Global RT overlay intensity (multiplies sampled RT alpha). Independent of material _CloudOpacity.")]
	[Range(0f, 1f)]
	[SerializeField]
	private float shadowStrength = 0.35f;

	// Token: 0x04003CE5 RID: 15589
	[Tooltip("Only one mesh is drawn into this RT, so raising the resolution is cheap.")]
	[SerializeField]
	private int resolution = 1024;

	// Token: 0x04003CE6 RID: 15590
	private CommandBuffer commandBuffer;

	// Token: 0x04003CE7 RID: 15591
	private MaterialPropertyBlock propertyBlock;

	// Token: 0x04003CE8 RID: 15592
	private RenderTexture cloudsRT;

	// Token: 0x04003CE9 RID: 15593
	private Matrix4x4 viewProjMatrix;

	// Token: 0x04003CEA RID: 15594
	private int forwardPassIndex = -1;

	// Token: 0x04003CEB RID: 15595
	private bool keywordApplied;

	// Token: 0x04003CEC RID: 15596
	private PlatformCloudAppearance lastAppearance = (PlatformCloudAppearance)(-1);

	// Token: 0x04003CED RID: 15597
	private float windScrollOffset;

	// Token: 0x04003CEE RID: 15598
	private float lastWindIntegrateTime = -1f;

	// Token: 0x04003CEF RID: 15599
	private MeshFilter defaultMeshFilter;

	// Token: 0x04003CF0 RID: 15600
	private MeshRenderer replacedMeshRenderer;

	// Token: 0x04003CF1 RID: 15601
	private bool loggedMissingRtPathRefs;

	// Token: 0x04003CF2 RID: 15602
	private bool loggedMissingReplacedPathRefs;
}
