using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

// Token: 0x0200067B RID: 1659
[CreateAssetMenu(menuName = "Skin Preset [GK2]", fileName = "SkinPreset")]
public class SkinPresetGK2 : SkinPresetBase
{
	// Token: 0x06002BEC RID: 11244 RVA: 0x000CFC04 File Offset: 0x000CDE04
	public override int DefineSkinIdFor(char char4, char char5, char char6)
	{
		int num = (this.isPlayerPreset ? (-1) : this.DefineSkinIdForNotPlayer(char4, char5, char6));
		if (num == -1)
		{
			if (char4 == 'b' && char5 == 'd' && char6 == 'y')
			{
				num = this.body.id;
			}
			else if (char4 == 'h' && char5 == 'e' && char6 == 'd')
			{
				num = this.head.id;
			}
			else if (char4 == 'a' && char5 == 'r' && char6 == 'm')
			{
				num = this.arms.id;
			}
			else if (char4 == 'b' && char5 == 'r' && char6 == 'd')
			{
				num = this.beard.id;
			}
			else if (char4 == 'h' && char5 == 'r' && char6 == 's')
			{
				num = this.hairstyle.id;
			}
		}
		return num;
	}

	// Token: 0x06002BED RID: 11245 RVA: 0x000CFCC0 File Offset: 0x000CDEC0
	public override void ApplyShaderParametersTo(List<SpriteRenderer> sprites)
	{
		for (int i = 0; i < sprites.Count; i++)
		{
			SpriteRenderer spriteRenderer = sprites[i];
			if (!this.TryToApply(spriteRenderer, "hed", this.head) && !this.TryToApply(spriteRenderer, "brd", this.beard) && !this.TryToApply(spriteRenderer, "hrs", this.hairstyle) && !this.TryToApply(spriteRenderer, "bdy", this.body) && !this.TryToApply(spriteRenderer, "arm", this.arms) && !this.TryToApply(spriteRenderer, "bdy_over", this.body) && !this.TryToApply(spriteRenderer, "bdy_ovr_hor", this.body))
			{
				this.TryToApply(spriteRenderer, "leg", this.body);
			}
		}
	}

	// Token: 0x06002BEE RID: 11246 RVA: 0x000CFD90 File Offset: 0x000CDF90
	public void ApplyShaderParametersTo(List<Image> images)
	{
		for (int i = 0; i < images.Count; i++)
		{
			Image image = images[i];
			if (!this.TryToApply(image, "hed", this.head) && !this.TryToApply(image, "brd", this.beard) && !this.TryToApply(image, "hrs", this.hairstyle) && !this.TryToApply(image, "bdy", this.body) && !this.TryToApply(image, "arm", this.arms) && !this.TryToApply(image, "bdy_over", this.body) && !this.TryToApply(image, "bdy_ovr_hor", this.body))
			{
				this.TryToApply(image, "leg", this.body);
			}
		}
	}

	// Token: 0x06002BEF RID: 11247 RVA: 0x000CFE60 File Offset: 0x000CE060
	public static SkinPresetGK2 LoadAsset(string id)
	{
		AsyncOperationHandle<SkinPresetGK2> asyncOperationHandle = Addressables.LoadAssetAsync<SkinPresetGK2>("Assets/AddressableAssets/Skins/" + id + ".asset");
		SkinPresetGK2 skinPresetGK = asyncOperationHandle.WaitForCompletion();
		if (skinPresetGK == null)
		{
			Debug.LogError("Couldn't load skin preset = " + id);
			return null;
		}
		if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
		{
			ValueTuple<AsyncOperationHandle<SkinPresetGK2>, int> valueTuple;
			if (SkinPresetGK2.loadedSkinPresets.TryGetValue(skinPresetGK, out valueTuple))
			{
				SkinPresetGK2.loadedSkinPresets[skinPresetGK] = new ValueTuple<AsyncOperationHandle<SkinPresetGK2>, int>(valueTuple.Item1, valueTuple.Item2 + 1);
			}
			else
			{
				SkinPresetGK2.loadedSkinPresets[skinPresetGK] = new ValueTuple<AsyncOperationHandle<SkinPresetGK2>, int>(asyncOperationHandle, 1);
			}
		}
		return skinPresetGK;
	}

	// Token: 0x06002BF0 RID: 11248 RVA: 0x000CFEF4 File Offset: 0x000CE0F4
	public new static SkinPresetGK2 Load(string id)
	{
		if (string.IsNullOrEmpty(id))
		{
			return null;
		}
		AsyncOperationHandle<SkinPresetGK2> asyncOperationHandle = Addressables.LoadAssetAsync<SkinPresetGK2>("Assets/AddressableAssets/Skins/" + id + ".asset");
		SkinPresetGK2 skinPresetGK = asyncOperationHandle.WaitForCompletion();
		if (skinPresetGK == null)
		{
			Debug.LogError("Couldn't load skin preset = " + id);
			return null;
		}
		if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
		{
			ValueTuple<AsyncOperationHandle<SkinPresetGK2>, int> valueTuple;
			if (SkinPresetGK2.loadedSkinPresets.TryGetValue(skinPresetGK, out valueTuple))
			{
				SkinPresetGK2.loadedSkinPresets[skinPresetGK] = new ValueTuple<AsyncOperationHandle<SkinPresetGK2>, int>(valueTuple.Item1, valueTuple.Item2 + 1);
			}
			else
			{
				SkinPresetGK2.loadedSkinPresets[skinPresetGK] = new ValueTuple<AsyncOperationHandle<SkinPresetGK2>, int>(asyncOperationHandle, 1);
			}
		}
		return skinPresetGK;
	}

	// Token: 0x06002BF1 RID: 11249 RVA: 0x000CFF90 File Offset: 0x000CE190
	public static void ReleaseAsset(SkinPresetGK2 skinPreset)
	{
		ValueTuple<AsyncOperationHandle<SkinPresetGK2>, int> valueTuple;
		if (skinPreset != null && SkinPresetGK2.loadedSkinPresets.TryGetValue(skinPreset, out valueTuple))
		{
			int num = valueTuple.Item2 - 1;
			Addressables.Release<SkinPresetGK2>(valueTuple.Item1);
			if (num <= 0)
			{
				SkinPresetGK2.loadedSkinPresets.Remove(skinPreset);
				return;
			}
			SkinPresetGK2.loadedSkinPresets[skinPreset] = new ValueTuple<AsyncOperationHandle<SkinPresetGK2>, int>(valueTuple.Item1, num);
		}
	}

	// Token: 0x06002BF2 RID: 11250 RVA: 0x000CFFF1 File Offset: 0x000CE1F1
	public bool TryToApply(SpriteRenderer sprite, string spriteName, SkinPresetPartGK2 skinPreset)
	{
		return this.TryToApply(sprite.name, sprite.material, spriteName, skinPreset);
	}

	// Token: 0x06002BF3 RID: 11251 RVA: 0x000D0007 File Offset: 0x000CE207
	public bool TryToApply(Image image, string spriteName, SkinPresetPartGK2 skinPreset)
	{
		return this.TryToApply(image.name, image.material, spriteName, skinPreset);
	}

	// Token: 0x06002BF4 RID: 11252 RVA: 0x000D0020 File Offset: 0x000CE220
	private bool TryToApply(string spriteObjName, Material material, string spriteName, SkinPresetPartGK2 skinPreset)
	{
		if (spriteObjName == spriteName)
		{
			material.SetColor(SkinChangerGK2.shaderColorId, skinPreset.color);
			material.SetFloat(SkinChangerGK2.shaderHueShiftId, skinPreset.hue + this.hue);
			material.SetFloat(SkinChangerGK2.shaderSaturationId, skinPreset.saturation + this.saturation);
			material.SetFloat(SkinChangerGK2.shaderValueId, skinPreset.velocity + this.velocity);
			material.SetFloat(SkinChangerGK2.shaderBrightnessId, this.brightness);
			material.SetFloat(SkinChangerGK2.shaderContrastId, this.contrast);
			Texture2D texture2D = ((this.palette == null) ? skinPreset.palette : this.palette);
			material.DisableKeyword(ColorReplaceType.USE_LUT_COLOR_REPLACE.ToString());
			material.DisableKeyword(ColorReplaceType.USE_PALETTE_COLOR_REPLACE.ToString());
			if (texture2D == null)
			{
				material.DisableKeyword(skinPreset.colorReplaceType.ToString());
			}
			else
			{
				material.EnableKeyword(skinPreset.colorReplaceType.ToString());
				if (skinPreset.colorReplaceType == ColorReplaceType.USE_LUT_COLOR_REPLACE)
				{
					material.SetTexture(SkinChangerGK2.shaderPaletteLutId, texture2D);
				}
				else
				{
					material.SetTexture(SkinChangerGK2.shaderPaletteId, texture2D);
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x06002BF5 RID: 11253 RVA: 0x000D0161 File Offset: 0x000CE361
	public int GetSkinPresetPartId(PlayerColorCustomizationType colorCustomizationType)
	{
		if (colorCustomizationType == PlayerColorCustomizationType.Hed)
		{
			return this.hairstyle.id;
		}
		if (colorCustomizationType - PlayerColorCustomizationType.Bdy1 > 2)
		{
			throw new ArgumentOutOfRangeException("colorCustomizationType", colorCustomizationType, null);
		}
		return this.body.id;
	}

	// Token: 0x06002BF6 RID: 11254 RVA: 0x000D0198 File Offset: 0x000CE398
	private int DefineSkinIdForNotPlayer(char char4, char char5, char char6)
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
		return num;
	}

	// Token: 0x04002388 RID: 9096
	public bool isPlayerPreset;

	// Token: 0x04002389 RID: 9097
	public SkinPresetPartGK2 body;

	// Token: 0x0400238A RID: 9098
	public SkinPresetPartGK2 head;

	// Token: 0x0400238B RID: 9099
	public SkinPresetPartGK2 arms;

	// Token: 0x0400238C RID: 9100
	public SkinPresetPartGK2 beard;

	// Token: 0x0400238D RID: 9101
	public SkinPresetPartGK2 hairstyle;

	// Token: 0x0400238E RID: 9102
	[Space]
	public Texture2D palette;

	// Token: 0x0400238F RID: 9103
	public float hue;

	// Token: 0x04002390 RID: 9104
	public float saturation;

	// Token: 0x04002391 RID: 9105
	public float velocity;

	// Token: 0x04002392 RID: 9106
	public float contrast;

	// Token: 0x04002393 RID: 9107
	public float brightness;

	// Token: 0x04002394 RID: 9108
	private static Dictionary<SkinPresetGK2, ValueTuple<AsyncOperationHandle<SkinPresetGK2>, int>> loadedSkinPresets = new Dictionary<SkinPresetGK2, ValueTuple<AsyncOperationHandle<SkinPresetGK2>, int>>();
}
