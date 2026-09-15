using System;
using System.Collections.Generic;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x020000FA RID: 250
	public class LightFlicker : MonoBehaviour
	{
		// Token: 0x06000487 RID: 1159 RVA: 0x00017B90 File Offset: 0x00015D90
		public void Start()
		{
			this.lastTime = Time.realtimeSinceStartup;
			this.dt = 0f;
			this.curFrameCounter = 0;
			this.curStep = 0;
			this.curLight = base.GetComponent<Light>();
			this.isFirst = true;
			this.GenerateStep();
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00017BD0 File Offset: 0x00015DD0
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
			LightFlickerStep lightFlickerStep = this.steps[this.curStep];
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
				this.curLight.color = lightFlickerStep.c;
				this.isFirst = false;
				return;
			}
			this.isTransing = true;
			this.c1 = lightFlickerStep.c;
			this.c0 = this.curLight.color;
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00017CAC File Offset: 0x00015EAC
		public void Update()
		{
			if (this.steps.Count == 0)
			{
				return;
			}
			float num = Time.realtimeSinceStartup - this.lastTime;
			this.transT0 += Time.deltaTime;
			if (this.isTransing)
			{
				float num2 = this.transT0 / this.transTime;
				this.curLight.color = new Color(Mathf.Lerp(this.c0.r, this.c1.r, num2), Mathf.Lerp(this.c0.g, this.c1.g, num2), Mathf.Lerp(this.c0.b, this.c1.b, num2));
				if (this.transT0 > this.transTime)
				{
					this.isTransing = false;
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

		// Token: 0x0400021F RID: 543
		public int fps = 30;

		// Token: 0x04000220 RID: 544
		public List<LightFlickerStep> steps = new List<LightFlickerStep>();

		// Token: 0x04000221 RID: 545
		private float lastTime;

		// Token: 0x04000222 RID: 546
		private float dt;

		// Token: 0x04000223 RID: 547
		private int curFrameCounter;

		// Token: 0x04000224 RID: 548
		private int curStep;

		// Token: 0x04000225 RID: 549
		private int curStepLen;

		// Token: 0x04000226 RID: 550
		private Light curLight;

		// Token: 0x04000227 RID: 551
		public float transTime;

		// Token: 0x04000228 RID: 552
		private bool isFirst = true;

		// Token: 0x04000229 RID: 553
		private bool isTransing;

		// Token: 0x0400022A RID: 554
		private float transT0;

		// Token: 0x0400022B RID: 555
		private Color c0;

		// Token: 0x0400022C RID: 556
		private Color c1;
	}
}
