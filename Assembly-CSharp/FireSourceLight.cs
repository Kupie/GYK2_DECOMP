using System;
using UnityEngine;

// Token: 0x020004FF RID: 1279
[ExecuteInEditMode]
public class FireSourceLight : MonoBehaviour
{
	// Token: 0x17000570 RID: 1392
	// (get) Token: 0x06002140 RID: 8512 RVA: 0x0009CC08 File Offset: 0x0009AE08
	private Light PointLight
	{
		get
		{
			Light light;
			if ((light = this.pointLight) == null)
			{
				light = (this.pointLight = base.GetComponent<Light>());
			}
			return light;
		}
	}

	// Token: 0x17000571 RID: 1393
	// (get) Token: 0x06002141 RID: 8513 RVA: 0x0009CC30 File Offset: 0x0009AE30
	private LightFaker LightFaker
	{
		get
		{
			LightFaker lightFaker;
			if ((lightFaker = this.lightFaker) == null)
			{
				lightFaker = (this.lightFaker = base.GetComponent<LightFaker>() ?? base.GetComponentInParent<LightFaker>());
			}
			return lightFaker;
		}
	}

	// Token: 0x06002142 RID: 8514 RVA: 0x0009CC60 File Offset: 0x0009AE60
	private void Update()
	{
		if (!this.PointLight)
		{
			return;
		}
		this.curPosPeriod -= Time.deltaTime;
		if (this.curPosPeriod < 0f)
		{
			Vector3 vector = new Vector3(global::UnityEngine.Random.Range(-this.posRandomDelta.x, this.posRandomDelta.x), global::UnityEngine.Random.Range(-this.posRandomDelta.y, this.posRandomDelta.y), global::UnityEngine.Random.Range(-this.posRandomDelta.z, this.posRandomDelta.z));
			this.curPosPeriod = this.posChangePeriod;
			this.curPosDirection = (vector - base.transform.localPosition) / this.posChangePeriod;
		}
		else
		{
			base.transform.localPosition += this.curPosDirection * Time.deltaTime;
		}
		this.SyncLightFakerLocalOffset();
		this.curLightPeriod -= Time.deltaTime;
		if (this.curLightPeriod < 0f)
		{
			if (this.colors != null && this.colors.Length > 1)
			{
				int num;
				do
				{
					num = global::UnityEngine.Random.Range(0, this.colors.Length);
				}
				while (num == this.prevLightN);
				if (this.PointLight != null && SwitchLightPolicy.AllowRealPointLights)
				{
					this.PointLight.color = this.colors[num];
				}
				if (SwitchLightPolicy.UseLightRT && this.LightFaker)
				{
					float num2 = ((this.PointLight != null) ? this.PointLight.intensity : 1f);
					float num3 = ((this.PointLight != null) ? this.PointLight.range : 8f);
					this.LightFaker.ApplyExternalState(this.colors[num], num2, num3);
				}
				this.prevLightN = num;
			}
			this.curLightPeriod = this.lightChangePeriod;
		}
	}

	// Token: 0x06002143 RID: 8515 RVA: 0x0009CE58 File Offset: 0x0009B058
	private void SyncLightFakerLocalOffset()
	{
		if (!SwitchLightPolicy.UseLightRT || !this.LightFaker)
		{
			return;
		}
		this.LightFaker.SetLocalVisualOffset(base.transform.position - this.LightFaker.transform.position);
	}

	// Token: 0x04001DD2 RID: 7634
	[Header("Position change")]
	public float posChangePeriod = 0.1f;

	// Token: 0x04001DD3 RID: 7635
	public Vector3 posRandomDelta = Vector3.zero;

	// Token: 0x04001DD4 RID: 7636
	private float curPosPeriod;

	// Token: 0x04001DD5 RID: 7637
	private Vector3 curPosDirection = Vector3.zero;

	// Token: 0x04001DD6 RID: 7638
	[Space]
	[Header("Color change")]
	public float lightChangePeriod = 0.03f;

	// Token: 0x04001DD7 RID: 7639
	public Color[] colors;

	// Token: 0x04001DD8 RID: 7640
	private float curLightPeriod;

	// Token: 0x04001DD9 RID: 7641
	private int prevLightN = -1;

	// Token: 0x04001DDA RID: 7642
	private Light pointLight;

	// Token: 0x04001DDB RID: 7643
	private LightFaker lightFaker;
}
