using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000144 RID: 324
	public class TextAnimator
	{
		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600069D RID: 1693 RVA: 0x00022151 File Offset: 0x00020351
		public bool IsAnimating
		{
			get
			{
				return this.animating;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x0600069E RID: 1694 RVA: 0x00022159 File Offset: 0x00020359
		// (set) Token: 0x0600069F RID: 1695 RVA: 0x00022161 File Offset: 0x00020361
		public float StartTime
		{
			get
			{
				return this.startTime;
			}
			set
			{
				this.startTime = value;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060006A0 RID: 1696 RVA: 0x0002216A File Offset: 0x0002036A
		public float TotalShowTime
		{
			get
			{
				return this.totalShowTime;
			}
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x00022174 File Offset: 0x00020374
		public void ShowMessage(TextMeshProUGUI label, string text, float letterAnimAppearTime = 0.02f)
		{
			this.label = label;
			this.letterAnimAppearTime = letterAnimAppearTime;
			this.text = text;
			this.startTime = Time.time;
			this.totalShowTime = letterAnimAppearTime * (float)this.ReplaceTagsAndSprites(this.text).Length;
			this.animating = true;
			this.ApplyAlphaToTextSinceIndex(0);
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x000221C9 File Offset: 0x000203C9
		public void CustomUpdate()
		{
			if (!this.animating)
			{
				return;
			}
			this.AnimateText();
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x000221DA File Offset: 0x000203DA
		public void Complete()
		{
			this.animating = false;
			this.ApplyAlphaToTextSinceIndex(this.text.Length);
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x000221F4 File Offset: 0x000203F4
		public float GetRemainingTime()
		{
			return this.TotalShowTime - (Time.time - this.startTime);
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x0002220C File Offset: 0x0002040C
		private void AnimateText()
		{
			int num = Mathf.FloorToInt((Time.time - this.startTime) / this.letterAnimAppearTime);
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < this.text.Length; i++)
			{
				char c = this.text[i];
				if (c != '<')
				{
					if (c != '>')
					{
						if (num3 <= 0 && num2++ >= num)
						{
							this.ApplyAlphaToTextSinceIndex(i);
							return;
						}
					}
					else
					{
						num3--;
					}
				}
				else
				{
					num3++;
				}
			}
			this.ApplyAlphaToTextSinceIndex(this.text.Length);
			this.animating = false;
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x000222A0 File Offset: 0x000204A0
		private void ApplyAlphaToTextSinceIndex(int currentTextIndex)
		{
			string text = this.text.Substring(0, currentTextIndex);
			string text2 = this.text.Substring(currentTextIndex);
			text2 = this.ReplaceTagsAndSprites(text2);
			this.label.text = text + "<color=#00000000>" + text2 + "</color>";
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x000222EC File Offset: 0x000204EC
		private string ReplaceTagsAndSprites(string str)
		{
			str = Regex.Replace(str, "<color[^>].+?>", "").Replace("</color>", "");
			str = Regex.Replace(str, "(<sprite[^>]*)>", "$1 color=#00000000>");
			return str;
		}

		// Token: 0x040003EF RID: 1007
		private TextMeshProUGUI label;

		// Token: 0x040003F0 RID: 1008
		private string text;

		// Token: 0x040003F1 RID: 1009
		private float startTime;

		// Token: 0x040003F2 RID: 1010
		private float totalShowTime;

		// Token: 0x040003F3 RID: 1011
		private float letterAnimAppearTime;

		// Token: 0x040003F4 RID: 1012
		private bool animating;
	}
}
