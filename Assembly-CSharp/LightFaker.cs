using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000B18 RID: 2840
[DisallowMultipleComponent]
[ExecuteAlways]
public class LightFaker : DayNightLightBase
{
	// Token: 0x17000B5B RID: 2907
	// (get) Token: 0x06004BA4 RID: 19364 RVA: 0x00165B95 File Offset: 0x00163D95
	private Light SourceLight
	{
		get
		{
			if (this.sourceLight == null)
			{
				this.sourceLight = base.GetComponent<Light>();
			}
			return this.sourceLight;
		}
	}

	// Token: 0x06004BA5 RID: 19365 RVA: 0x00165BB7 File Offset: 0x00163DB7
	private void Awake()
	{
		this.EnsureCircleRenderer();
		this.ApplyLightMode(this.mode);
		this.UpdateCustomObjectsActiveState();
	}

	// Token: 0x06004BA6 RID: 19366 RVA: 0x00165BD1 File Offset: 0x00163DD1
	private void OnEnable()
	{
		if (Application.isPlaying)
		{
			LightRTManager.Instance.Register(this);
		}
		this.SyncFromSourceLight();
		this.ApplyVisual();
	}

	// Token: 0x06004BA7 RID: 19367 RVA: 0x00165BF1 File Offset: 0x00163DF1
	private void OnDisable()
	{
		if (Application.isPlaying && LightRTManager.Instance != null)
		{
			LightRTManager.Instance.Unregister(this);
		}
	}

	// Token: 0x06004BA8 RID: 19368 RVA: 0x00165C12 File Offset: 0x00163E12
	private void Update()
	{
		this.SyncFromSourceLight();
		this.ApplyVisual();
		this.PropagateModeToSiblings();
	}

	// Token: 0x06004BA9 RID: 19369 RVA: 0x00165C28 File Offset: 0x00163E28
	protected override void ApplyLightMode(DayNightLightBase.LightMode lightMode)
	{
		this.mode = lightMode;
		this.SyncFromSourceLight();
		this.ApplyVisual();
		this.PropagateModeToSiblings();
		DayNightLight dayNightLight;
		if (base.TryGetComponent<DayNightLight>(out dayNightLight) && dayNightLight.mode != lightMode)
		{
			dayNightLight.ApplyLightModeInt((int)lightMode);
		}
	}

	// Token: 0x06004BAA RID: 19370 RVA: 0x00165C68 File Offset: 0x00163E68
	private void PropagateModeToSiblings()
	{
		DayNightSprite[] componentsInChildren = base.GetComponentsInChildren<DayNightSprite>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].mode != this.mode)
			{
				componentsInChildren[i].ApplyLightModeInt((int)this.mode);
			}
		}
	}

	// Token: 0x06004BAB RID: 19371 RVA: 0x00165CAC File Offset: 0x00163EAC
	private void SyncFromSourceLight()
	{
		Light light = this.SourceLight;
		if (light != null)
		{
			this.syncedColor = light.color;
			this.syncedRange = light.range * this.rangeMultiplier;
		}
		this.syncedIntensity = this.GetBaseIntensity();
		float effectiveIntensity = this.GetEffectiveIntensity();
		if (light != null)
		{
			SwitchLightPolicy.ApplyRealLightEnabled(light, effectiveIntensity);
		}
	}

	// Token: 0x06004BAC RID: 19372 RVA: 0x00165D0C File Offset: 0x00163F0C
	private float GetBaseIntensity()
	{
		DayNightLight dayNightLight;
		if (base.TryGetComponent<DayNightLight>(out dayNightLight))
		{
			return dayNightLight.intensity * this.intensityMultiplier;
		}
		Light light = this.SourceLight;
		if (light != null)
		{
			return light.intensity * this.intensityMultiplier;
		}
		return this.syncedIntensity;
	}

	// Token: 0x06004BAD RID: 19373 RVA: 0x00165D58 File Offset: 0x00163F58
	private float GetEffectiveIntensity()
	{
		float globalFloat = Shader.GetGlobalFloat(GlobalShaderParameters.idSunLight);
		DayNightLightBase.LightMode mode = this.mode;
		float num;
		if (mode != DayNightLightBase.LightMode.Night)
		{
			if (mode != DayNightLightBase.LightMode.Day)
			{
				num = this.syncedIntensity;
			}
			else
			{
				num = Mathf.Lerp(0f, this.syncedIntensity, globalFloat);
			}
		}
		else
		{
			num = Mathf.Lerp(this.syncedIntensity, 0f, globalFloat);
		}
		return num;
	}

	// Token: 0x06004BAE RID: 19374 RVA: 0x00165DB0 File Offset: 0x00163FB0
	private void ApplyVisual()
	{
		this.EnsureCircleRenderer();
		if (!this.circleRenderer)
		{
			return;
		}
		float effectiveIntensity = this.GetEffectiveIntensity();
		if (!this.ignoreRangeScale)
		{
			float num = Mathf.Max(0.01f, this.syncedRange * 2f);
			this.circleRenderer.transform.localScale = new Vector3(num, num, 1f);
		}
		if (this.propertyBlock == null)
		{
			this.propertyBlock = new MaterialPropertyBlock();
		}
		this.circleRenderer.GetPropertyBlock(this.propertyBlock);
		this.propertyBlock.SetColor("_Color", this.syncedColor);
		this.propertyBlock.SetFloat(LightFaker.idIntensity, effectiveIntensity);
		this.propertyBlock.SetVector(LightFaker.idLocalOffset, this.localVisualOffset);
		this.propertyBlock.SetTexture(LightFaker.idCookie, this.ResolveCookieTexture());
		this.circleRenderer.SetPropertyBlock(this.propertyBlock);
		this.circleRenderer.enabled = SwitchLightPolicy.UseLightRT && effectiveIntensity > 0.001f;
	}

	// Token: 0x06004BAF RID: 19375 RVA: 0x00165EBD File Offset: 0x001640BD
	public void RefreshPolicyState()
	{
		this.UpdateCustomObjectsActiveState();
		this.SyncFromSourceLight();
		this.ApplyVisual();
	}

	// Token: 0x06004BB0 RID: 19376 RVA: 0x00165ED4 File Offset: 0x001640D4
	private void EnsureCircleRenderer()
	{
		if (this.circleRenderer != null)
		{
			this.circleRenderer.gameObject.layer = 9;
			return;
		}
		Transform transform = base.transform.Find("LightFakerCircle");
		GameObject gameObject;
		if (transform != null)
		{
			gameObject = transform.gameObject;
		}
		else
		{
			gameObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
			MeshRenderer meshRenderer;
			if (gameObject.TryGetComponent<MeshRenderer>(out meshRenderer))
			{
				meshRenderer.receiveShadows = false;
				meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			}
			gameObject.name = "LightFakerCircle";
			gameObject.transform.SetParent(base.transform, false);
			gameObject.transform.rotation = LightFaker.WorldFlatRotation;
			Collider collider;
			if (gameObject.TryGetComponent<Collider>(out collider))
			{
				global::UnityEngine.Object.Destroy(collider);
			}
		}
		gameObject.layer = 9;
		this.circleRenderer = gameObject.GetComponent<MeshRenderer>();
		if (this.circleRenderer.sharedMaterial == null || this.circleRenderer.sharedMaterial.shader.name != "Custom/Light Faker Circle")
		{
			this.circleRenderer.sharedMaterial = LazySingletonSO<GlobalResources>.Instance.fakeLightMaterial;
		}
	}

	// Token: 0x06004BB1 RID: 19377 RVA: 0x00165FDE File Offset: 0x001641DE
	public void ApplyExternalState(Color color, float intensity, float range)
	{
		this.syncedColor = color;
		this.syncedRange = range * this.rangeMultiplier;
		this.syncedIntensity = ((this.mode == DayNightLightBase.LightMode.Static) ? (intensity * this.intensityMultiplier) : this.GetBaseIntensity());
		this.ApplyVisual();
	}

	// Token: 0x06004BB2 RID: 19378 RVA: 0x0016601A File Offset: 0x0016421A
	public void SetLocalVisualOffset(Vector3 worldOffset)
	{
		this.localVisualOffset = worldOffset;
		this.ApplyVisual();
	}

	// Token: 0x06004BB3 RID: 19379 RVA: 0x0016602C File Offset: 0x0016422C
	private Texture ResolveCookieTexture()
	{
		if (this.mask != null)
		{
			return this.mask;
		}
		Light light = this.SourceLight;
		if (light != null)
		{
			Texture2D texture2D = light.cookie as Texture2D;
			if (texture2D != null)
			{
				return texture2D;
			}
		}
		return Texture2D.whiteTexture;
	}

	// Token: 0x06004BB4 RID: 19380 RVA: 0x00166074 File Offset: 0x00164274
	private void UpdateCustomObjectsActiveState()
	{
		if (!SwitchLightPolicy.UseLightRT)
		{
			return;
		}
		foreach (GameObject gameObject in this.keepGameObjectsActiveAccordingToLightingPolicy)
		{
			gameObject.SetActive(this.keptGameObjectsActiveState);
		}
	}

	// Token: 0x04003D04 RID: 15620
	private const string CIRCLE_SHADER = "Custom/Light Faker Circle";

	// Token: 0x04003D05 RID: 15621
	private static readonly int idIntensity = Shader.PropertyToID("_Intensity");

	// Token: 0x04003D06 RID: 15622
	private static readonly int idLocalOffset = Shader.PropertyToID("_LocalOffset");

	// Token: 0x04003D07 RID: 15623
	private static readonly int idCookie = Shader.PropertyToID("_Cookie");

	// Token: 0x04003D08 RID: 15624
	private static readonly float DEFAULT_INTENSITY_MULTIPLIER = 0.1f;

	// Token: 0x04003D09 RID: 15625
	private static readonly Quaternion WorldFlatRotation = Quaternion.Euler(90f, 0f, 0f);

	// Token: 0x04003D0A RID: 15626
	[SerializeField]
	private Light sourceLight;

	// Token: 0x04003D0B RID: 15627
	[SerializeField]
	private MeshRenderer circleRenderer;

	// Token: 0x04003D0C RID: 15628
	[SerializeField]
	[Tooltip("When enabled, circle mesh localScale is not driven by Light.range (keep authored size).")]
	private bool ignoreRangeScale;

	// Token: 0x04003D0D RID: 15629
	[SerializeField]
	private float rangeMultiplier = 1f;

	// Token: 0x04003D0E RID: 15630
	[SerializeField]
	private float intensityMultiplier = LightFaker.DEFAULT_INTENSITY_MULTIPLIER;

	// Token: 0x04003D0F RID: 15631
	[SerializeField]
	[Tooltip("Optional LightRT mask (alpha channel). Overrides Light.cookie when assigned.")]
	private Texture2D mask;

	// Token: 0x04003D10 RID: 15632
	[Space]
	[SerializeField]
	private bool keptGameObjectsActiveState;

	// Token: 0x04003D11 RID: 15633
	[SerializeField]
	private List<GameObject> keepGameObjectsActiveAccordingToLightingPolicy = new List<GameObject>();

	// Token: 0x04003D12 RID: 15634
	private MaterialPropertyBlock propertyBlock;

	// Token: 0x04003D13 RID: 15635
	private float syncedIntensity;

	// Token: 0x04003D14 RID: 15636
	private Color syncedColor = Color.white;

	// Token: 0x04003D15 RID: 15637
	private float syncedRange = 8f;

	// Token: 0x04003D16 RID: 15638
	private Vector3 localVisualOffset;
}
