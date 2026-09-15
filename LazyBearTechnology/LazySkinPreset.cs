using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LazyBearTechnology
{
	// Token: 0x0200016E RID: 366
	[CreateAssetMenu(menuName = "Skin Preset [Default]", fileName = "SkinPreset")]
	[Serializable]
	public class LazySkinPreset : SkinPresetBase
	{
		// Token: 0x06000804 RID: 2052 RVA: 0x00028048 File Offset: 0x00026248
		public new static LazySkinPreset Load(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return null;
			}
			LazySkinPreset lazySkinPreset = Addressables.LoadAssetAsync<LazySkinPreset>("Skins/" + id + ".asset").WaitForCompletion();
			if (lazySkinPreset == null)
			{
				Debug.LogError("Couldn't load skin preset = " + id);
				return null;
			}
			return lazySkinPreset;
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x0002809C File Offset: 0x0002629C
		public override int DefineSkinIdFor(char char4, char char5, char char6)
		{
			int num = -1;
			if (char4 == 'b' && char5 == 'd' && char6 == 'y')
			{
				num = this.body.id;
			}
			else if (char4 == 'h' && char5 == 'e' && char6 == 'd')
			{
				num = this.head.id;
			}
			else if (char4 == 'b' && char5 == 'o' && char6 == 't')
			{
				num = this.bot.id;
			}
			else if (char4 == 't' && char5 == 'o' && char6 == 'p')
			{
				num = this.top.id;
			}
			return num;
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00028120 File Offset: 0x00026320
		public override void ApplyShaderParametersTo(List<SpriteRenderer> sprites)
		{
			for (int i = 0; i < sprites.Count; i++)
			{
				if (!this.TryToApply(sprites[i], "Body", this.body) && !this.TryToApply(sprites[i], "Head", this.head) && !this.TryToApply(sprites[i], "Top", this.top))
				{
					this.TryToApply(sprites[i], "Bot", this.bot);
				}
			}
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x000281A8 File Offset: 0x000263A8
		public bool TryToApply(SpriteRenderer sprite, string spriteName, SkinPresetPart skinPreset)
		{
			if (sprite.name == spriteName)
			{
				sprite.material.SetColor(SkinChanger.shaderColorId, skinPreset.color);
				sprite.material.SetFloat(SkinChanger.shaderHueShiftId, skinPreset.hue + this.hue);
				sprite.material.SetFloat(SkinChanger.shaderSaturationId, skinPreset.saturation + this.saturation);
				sprite.material.SetFloat(SkinChanger.shaderValueId, skinPreset.velocity + this.velocity);
				sprite.material.SetFloat(SkinChanger.shaderBrightnessId, this.brightness);
				sprite.material.SetFloat(SkinChanger.shaderContrastId, this.contrast);
				Texture2D texture2D = ((this.palette == null) ? skinPreset.palette : this.palette);
				if (texture2D == null)
				{
					sprite.material.DisableKeyword("USE_PALETTE_COLOR_REPLACE");
				}
				else
				{
					sprite.material.EnableKeyword("USE_PALETTE_COLOR_REPLACE");
					sprite.material.SetTexture(SkinChanger.shaderPaletteId, texture2D);
				}
				return true;
			}
			return false;
		}

		// Token: 0x040004DB RID: 1243
		public SkinPresetPart body;

		// Token: 0x040004DC RID: 1244
		public SkinPresetPart head;

		// Token: 0x040004DD RID: 1245
		public SkinPresetPart bot;

		// Token: 0x040004DE RID: 1246
		public SkinPresetPart top;

		// Token: 0x040004DF RID: 1247
		public Texture2D palette;

		// Token: 0x040004E0 RID: 1248
		public float hue;

		// Token: 0x040004E1 RID: 1249
		public float saturation;

		// Token: 0x040004E2 RID: 1250
		public float velocity;

		// Token: 0x040004E3 RID: 1251
		public float contrast;

		// Token: 0x040004E4 RID: 1252
		public float brightness;
	}
}
