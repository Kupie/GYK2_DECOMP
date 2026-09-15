using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006C4 RID: 1732
public class LightFlicker : MonoBehaviour
{
	// Token: 0x06002DE5 RID: 11749 RVA: 0x000DB8C4 File Offset: 0x000D9AC4
	public void Start()
	{
		this.lastTime = Time.realtimeSinceStartup;
		this.dt = 0f;
		this.curFrameCounter = 0;
		this.curStep = 0;
		this.lightSource = base.GetComponent<Light>();
		this.lightFaker = base.GetComponent<LightFaker>();
		this.isFirst = true;
		this.GenerateStep();
	}

	// Token: 0x06002DE6 RID: 11750 RVA: 0x000DB91C File Offset: 0x000D9B1C
	private void GenerateStep()
	{
		int count = this.steps.Count;
		if (count == 0)
		{
			return;
		}
		if (this.curStep >= count)
		{
			this.curStep = 0;
		}
		LightFlicker.LightFlickerStep lightFlickerStep = this.steps[this.curStep];
		if (lightFlickerStep.len2 == 0)
		{
			this.curStepLen = lightFlickerStep.len;
		}
		else
		{
			this.curStepLen = global::UnityEngine.Random.Range(lightFlickerStep.len, lightFlickerStep.len2);
		}
		this.curStepLen += Mathf.RoundToInt(this.transTime * (float)this.fps);
		this.transT0 = 0f;
		if (this.isFirst)
		{
			this.ApplyColor(lightFlickerStep.c);
			this.isFirst = false;
			return;
		}
		this.isInTrans = true;
		this.c1 = lightFlickerStep.c;
		this.c0 = ((this.lightSource != null) ? this.lightSource.color : this.c1);
	}

	// Token: 0x06002DE7 RID: 11751 RVA: 0x000DBA0C File Offset: 0x000D9C0C
	private void ApplyColor(Color color)
	{
		if (this.lightSource != null && SwitchLightPolicy.AllowRealPointLights)
		{
			this.lightSource.color = color;
		}
		if (this.lightFaker)
		{
			float num = ((this.lightSource != null) ? this.lightSource.intensity : 1f);
			float num2 = ((this.lightSource != null) ? this.lightSource.range : 8f);
			this.lightFaker.ApplyExternalState(color, num, num2);
		}
	}

	// Token: 0x06002DE8 RID: 11752 RVA: 0x000DBA98 File Offset: 0x000D9C98
	public void Update()
	{
		if (this.steps.Count == 0)
		{
			return;
		}
		float num = Time.realtimeSinceStartup - this.lastTime;
		this.transT0 += Time.deltaTime;
		if (this.isInTrans)
		{
			float num2 = this.transT0 / this.transTime;
			Color color = new Color(Mathf.Lerp(this.c0.r, this.c1.r, num2), Mathf.Lerp(this.c0.g, this.c1.g, num2), Mathf.Lerp(this.c0.b, this.c1.b, num2));
			this.ApplyColor(color);
			if (this.transT0 > this.transTime)
			{
				this.isInTrans = false;
				this.isFirst = false;
			}
		}
		this.lastTime = Time.realtimeSinceStartup;
		this.dt += num;
		float num3 = 1f / (float)this.fps;
		if (this.dt > num3)
		{
			this.dt -= num3;
			this.curFrameCounter++;
			if (this.curStep >= this.steps.Count)
			{
				this.curStep = 0;
				return;
			}
			if (this.curFrameCounter >= this.curStepLen)
			{
				this.curStep++;
				this.curFrameCounter = 0;
				this.GenerateStep();
			}
		}
	}

	// Token: 0x040024F4 RID: 9460
	public int fps = 30;

	// Token: 0x040024F5 RID: 9461
	public List<LightFlicker.LightFlickerStep> steps = new List<LightFlicker.LightFlickerStep>();

	// Token: 0x040024F6 RID: 9462
	private float lastTime;

	// Token: 0x040024F7 RID: 9463
	private float dt;

	// Token: 0x040024F8 RID: 9464
	private int curFrameCounter;

	// Token: 0x040024F9 RID: 9465
	private int curStep;

	// Token: 0x040024FA RID: 9466
	private int curStepLen;

	// Token: 0x040024FB RID: 9467
	private Light lightSource;

	// Token: 0x040024FC RID: 9468
	private LightFaker lightFaker;

	// Token: 0x040024FD RID: 9469
	public float transTime;

	// Token: 0x040024FE RID: 9470
	private bool isFirst = true;

	// Token: 0x040024FF RID: 9471
	private bool isInTrans;

	// Token: 0x04002500 RID: 9472
	private float transT0;

	// Token: 0x04002501 RID: 9473
	private Color c0;

	// Token: 0x04002502 RID: 9474
	private Color c1;

	// Token: 0x020006C5 RID: 1733
	[Serializable]
	public class LightFlickerStep
	{
		// Token: 0x04002503 RID: 9475
		public int len;

		// Token: 0x04002504 RID: 9476
		public int len2;

		// Token: 0x04002505 RID: 9477
		public Color c;
	}
}
