using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x0200016F RID: 367
	public class SkinChanger
	{
		// Token: 0x06000809 RID: 2057 RVA: 0x000282C3 File Offset: 0x000264C3
		public SkinChanger(GameObject gameObject, bool applyShader = true)
		{
			this.sprites = gameObject.GetComponentsInChildren<SpriteRenderer>(true).ToList<SpriteRenderer>();
			this.InitSkinChanger(gameObject, applyShader);
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x000282F0 File Offset: 0x000264F0
		public SkinChanger(GameObject gameObject, List<SpriteRenderer> spriteRenderers, bool applyShader = true)
		{
			this.sprites = spriteRenderers;
			this.InitSkinChanger(gameObject, applyShader);
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x00028312 File Offset: 0x00026512
		public void ApplySkin(SkinPresetBase skinPreset, SkinPresetBase fallbackSkin = null)
		{
			this.skin = skinPreset;
			this.fallbackSkin = fallbackSkin;
			if (this.skin != null)
			{
				this.ApplyShaderParameters();
			}
			this.skinnedSpriteTopLevelHash.Clear();
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x00028344 File Offset: 0x00026544
		public void CustomLateUpdate()
		{
			if (this.skin == null)
			{
				return;
			}
			for (int i = 0; i < this.sprites.Count; i++)
			{
				SpriteRenderer spriteRenderer = this.sprites[i];
				bool flag = false;
				int num = 0;
				if (spriteRenderer.sprite != null)
				{
					num = spriteRenderer.sprite.GetInstanceID();
					if (!SkinChanger.validSpriteHash.TryGetValue(num, out flag))
					{
						flag = this.IsValidSprite(spriteRenderer);
						SkinChanger.validSpriteHash.Add(num, flag);
					}
				}
				if (flag)
				{
					Sprite sprite;
					if (!this.skinnedSpriteTopLevelHash.TryGetValue(num, out sprite))
					{
						string name = spriteRenderer.sprite.name;
						char c = name[4];
						char c2 = name[5];
						char c3 = name[6];
						int num2 = this.skin.DefineSkinIdFor(c, c2, c3);
						int num3 = -1;
						if (num2 == 0 || string.IsNullOrEmpty(name))
						{
							spriteRenderer.enabled = false;
							spriteRenderer.sprite = null;
						}
						else
						{
							spriteRenderer.enabled = true;
							if (num2 != -1)
							{
								GarbagelessStrings.StringToChars(ref name, ref SkinChanger.chars);
								GarbagelessStrings.IntToCharsWithLeadingZeros(num2, ref SkinChanger.chars, 3, 0);
								int num4 = GarbagelessStrings.GetHashCode(ref SkinChanger.chars);
								if (!SkinChanger.spriteHash.TryGetValue(num4, out sprite))
								{
									string text = GarbagelessStrings.CharsToString(ref SkinChanger.chars);
									sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(text, null);
									if (sprite == null)
									{
										if (this.fallbackSkin != null)
										{
											num3 = this.fallbackSkin.DefineSkinIdFor(c, c2, c3);
											if (num3 != 0)
											{
												GarbagelessStrings.IntToCharsWithLeadingZeros(num3, ref SkinChanger.chars, 3, 0);
												num4 = GarbagelessStrings.GetHashCode(ref SkinChanger.chars);
												if (!SkinChanger.spriteHash.TryGetValue(num4, out sprite))
												{
													text = GarbagelessStrings.CharsToString(ref SkinChanger.chars);
													sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(text, null);
													if (sprite == null)
													{
														spriteRenderer.enabled = false;
													}
												}
											}
										}
										else
										{
											spriteRenderer.enabled = false;
											spriteRenderer.sprite = null;
										}
									}
									if (!SkinChanger.spriteHash.ContainsKey(num4))
									{
										SkinChanger.spriteHash.Add(num4, sprite);
									}
								}
								spriteRenderer.sprite = sprite;
							}
						}
						this.skinnedSpriteTopLevelHash.Add(num, (num2 == -1 || num3 == -1) ? spriteRenderer.sprite : sprite);
					}
					else
					{
						spriteRenderer.sprite = sprite;
						spriteRenderer.enabled = sprite != null;
					}
				}
			}
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x00028598 File Offset: 0x00026798
		public void ApplyShaderToAllSprites(Shader shader)
		{
			this.ApplyShaderToSprites(shader, this.sprites);
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x000285A8 File Offset: 0x000267A8
		public void ApplyShaderToSprites(Shader shader, List<SpriteRenderer> sprites = null)
		{
			int num = 0;
			for (;;)
			{
				int num2 = num;
				int? num3 = ((sprites != null) ? new int?(sprites.Count) : null);
				if (!((num2 < num3.GetValueOrDefault()) & (num3 != null)))
				{
					break;
				}
				sprites[num].material.shader = shader;
				num++;
			}
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x000285FD File Offset: 0x000267FD
		public void ApplyShaderParameters()
		{
			this.skin.ApplyShaderParametersTo(this.sprites);
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x00028610 File Offset: 0x00026810
		public void SetSpriteLayerPosition(string layerName, Vector2 layerPosition)
		{
			SpriteRenderer spriteRenderer = this.sprites.Find((SpriteRenderer s) => s.name == layerName);
			if (spriteRenderer == null)
			{
				Debug.LogError("$Trying to set position for non exists layer:[" + layerName + "]");
				return;
			}
			spriteRenderer.transform.localPosition = new Vector3(layerPosition.x, layerPosition.y, spriteRenderer.transform.localPosition.z);
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x00028694 File Offset: 0x00026894
		private void InitSkinChanger(GameObject gameObject, bool applyShader)
		{
			this.gameObject = gameObject;
			for (int i = 0; i < this.sprites.Count; i++)
			{
				if (this.sprites[i].gameObject.name.StartsWith("-"))
				{
					this.sprites.RemoveAt(i);
					i--;
				}
			}
			if (applyShader)
			{
				this.ApplyShaderToAllSprites(Shader.Find("Sprites/ColorAdjust"));
			}
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x00028704 File Offset: 0x00026904
		private bool IsValidSprite(SpriteRenderer spriteRenderer)
		{
			if (spriteRenderer.sprite == null)
			{
				return false;
			}
			string name = spriteRenderer.sprite.name;
			if (name.Length <= 7)
			{
				return false;
			}
			for (int i = 0; i < 3; i++)
			{
				char c = name[i];
				if (c < '0' || c > '9')
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040004E5 RID: 1253
		protected List<SpriteRenderer> sprites;

		// Token: 0x040004E6 RID: 1254
		protected GameObject gameObject;

		// Token: 0x040004E7 RID: 1255
		private SkinPresetBase skin;

		// Token: 0x040004E8 RID: 1256
		private SkinPresetBase fallbackSkin;

		// Token: 0x040004E9 RID: 1257
		private Dictionary<int, Sprite> skinnedSpriteTopLevelHash = new Dictionary<int, Sprite>();

		// Token: 0x040004EA RID: 1258
		private static Dictionary<int, Sprite> spriteHash = new Dictionary<int, Sprite>();

		// Token: 0x040004EB RID: 1259
		private static Dictionary<int, bool> validSpriteHash = new Dictionary<int, bool>();

		// Token: 0x040004EC RID: 1260
		private static char[] chars = new char[100];

		// Token: 0x040004ED RID: 1261
		public static readonly int shaderColorId = Shader.PropertyToID("_Color");

		// Token: 0x040004EE RID: 1262
		public static readonly int shaderHueShiftId = Shader.PropertyToID("_HueShift");

		// Token: 0x040004EF RID: 1263
		public static readonly int shaderSaturationId = Shader.PropertyToID("_Sat");

		// Token: 0x040004F0 RID: 1264
		public static readonly int shaderValueId = Shader.PropertyToID("_Val");

		// Token: 0x040004F1 RID: 1265
		public static readonly int shaderPaletteId = Shader.PropertyToID("_Palette");

		// Token: 0x040004F2 RID: 1266
		public static readonly int shaderBrightnessId = Shader.PropertyToID("_Brightness");

		// Token: 0x040004F3 RID: 1267
		public static readonly int shaderContrastId = Shader.PropertyToID("_Contrast");
	}
}
