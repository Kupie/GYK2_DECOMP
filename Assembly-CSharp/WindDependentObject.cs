using System;
using UnityEngine;

// Token: 0x0200051F RID: 1311
[RequireComponent(typeof(MeshRenderer))]
public class WindDependentObject : MonoBehaviour
{
	// Token: 0x060021DD RID: 8669 RVA: 0x0009F3FC File Offset: 0x0009D5FC
	private void Awake()
	{
		this.meshRenderer = base.GetComponent<MeshRenderer>();
		if (this.meshRenderer != null)
		{
			this.windClothMaterial = this.meshRenderer.material;
		}
		if (this.windClothMaterial == null)
		{
			Debug.LogError("[WindDependentObject]: Material is null", this);
		}
	}

	// Token: 0x060021DE RID: 8670 RVA: 0x0009F44D File Offset: 0x0009D64D
	private void Start()
	{
		this.weatherSystem = WeatherSystem.Instance;
		this.lastWindValue = this.weatherSystem.WindValue;
	}

	// Token: 0x060021DF RID: 8671 RVA: 0x0009F46C File Offset: 0x0009D66C
	private void Update()
	{
		if (!this.lastWindValue.EqualsTo(this.weatherSystem.WindValue, 1E-05f))
		{
			this.lastWindValue = this.weatherSystem.WindValue;
			if (this.windClothMaterial != null)
			{
				this.windClothMaterial.SetFloat(WindDependentObject.WindStrengthProperty, this.lastWindValue * this.WindModificator);
				this.meshRenderer.material = this.windClothMaterial;
			}
		}
	}

	// Token: 0x04001E82 RID: 7810
	[SerializeField]
	private Material windClothMaterial;

	// Token: 0x04001E83 RID: 7811
	[SerializeField]
	private float lastWindValue;

	// Token: 0x04001E84 RID: 7812
	[SerializeField]
	public float WindModificator = 1f;

	// Token: 0x04001E85 RID: 7813
	private static readonly int WindStrengthProperty = Shader.PropertyToID("_WindIntensity");

	// Token: 0x04001E86 RID: 7814
	private WeatherSystem weatherSystem;

	// Token: 0x04001E87 RID: 7815
	private MeshRenderer meshRenderer;
}
