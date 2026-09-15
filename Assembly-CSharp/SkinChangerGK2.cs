using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000AB1 RID: 2737
public class SkinChangerGK2
{
	// Token: 0x17000B2D RID: 2861
	// (get) Token: 0x060049EB RID: 18923 RVA: 0x0015D14D File Offset: 0x0015B34D
	// (set) Token: 0x060049EC RID: 18924 RVA: 0x0015D155 File Offset: 0x0015B355
	public bool HasCustomHeadFrames { get; private set; }

	// Token: 0x060049ED RID: 18925 RVA: 0x0015D15E File Offset: 0x0015B35E
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void ResetStaticCaches()
	{
		SkinChangerGK2.spriteHash.Clear();
		SkinChangerGK2.customHeadFramesAvailabilityCache.Clear();
		SkinChangerGK2.customBeardFramesAvailabilityCache.Clear();
	}

	// Token: 0x060049EE RID: 18926 RVA: 0x0015D15E File Offset: 0x0015B35E
	public static void ClearStaticCaches()
	{
		SkinChangerGK2.spriteHash.Clear();
		SkinChangerGK2.customHeadFramesAvailabilityCache.Clear();
		SkinChangerGK2.customBeardFramesAvailabilityCache.Clear();
	}

	// Token: 0x060049EF RID: 18927 RVA: 0x0015D180 File Offset: 0x0015B380
	public SkinChangerGK2(GameObject gameObject, bool applyShader = false)
	{
		this.sprites = gameObject.GetComponentsInChildren<SpriteRenderer>(true).ToList<SpriteRenderer>();
		this.InitSkinChanger(gameObject, applyShader);
	}

	// Token: 0x060049F0 RID: 18928 RVA: 0x0015D1D5 File Offset: 0x0015B3D5
	public SkinChangerGK2(GameObject gameObject, List<SpriteRenderer> spriteRenderers, bool applyShader = false)
	{
		this.sprites = spriteRenderers;
		this.InitSkinChanger(gameObject, applyShader);
	}

	// Token: 0x060049F1 RID: 18929 RVA: 0x0015D214 File Offset: 0x0015B414
	public void ApplySkin(SkinPresetBase skinPreset, SkinPresetBase fallbackSkin = null)
	{
		this.skin = skinPreset;
		this.fallbackSkin = fallbackSkin;
		if (this.skin != null)
		{
			this.ApplyShaderParameters();
		}
		this.skinnedSpriteTopLevelHash.Clear();
		this.headFrameVariantCache.Clear();
		this.headFrameOverride = 1;
		this.DetectCustomHeadFrames();
		this.DetectCustomBeardFrames();
		this.UpdateSkinnedSprites();
	}

	// Token: 0x060049F2 RID: 18930 RVA: 0x0015D272 File Offset: 0x0015B472
	public void SetHeadFrameOverride(int frame)
	{
		this.headFrameOverride = ((frame < 1) ? 1 : frame);
	}

	// Token: 0x060049F3 RID: 18931 RVA: 0x0015D282 File Offset: 0x0015B482
	public void CustomLateUpdate()
	{
		if (this.skin == null)
		{
			return;
		}
		this.UpdateSkinnedSprites();
	}

	// Token: 0x060049F4 RID: 18932 RVA: 0x0015D29C File Offset: 0x0015B49C
	private void UpdateSkinnedSprites()
	{
		for (int i = 0; i < this.sprites.Count; i++)
		{
			SpriteRenderer spriteRenderer = this.sprites[i];
			int num;
			if (this.TryGetAnimationSpriteId(spriteRenderer, out num))
			{
				Sprite sprite = this.ResolveSkinnedSprite(spriteRenderer, num);
				if (this.headFrameOverride > 1 && sprite != null)
				{
					if (this.HasCustomHeadFrames && spriteRenderer == this.headSpriteRenderer)
					{
						sprite = this.ResolveHeadFrameVariant(sprite, num, this.headFrameOverride) ?? sprite;
					}
					else if (this.hasCustomBeardFrames && spriteRenderer == this.beardSpriteRenderer)
					{
						sprite = this.ResolveHeadFrameVariant(sprite, num, this.headFrameOverride) ?? sprite;
					}
				}
				SkinChangerGK2.ApplyResolvedSprite(spriteRenderer, sprite);
			}
		}
	}

	// Token: 0x060049F5 RID: 18933 RVA: 0x0015D34C File Offset: 0x0015B54C
	private Sprite ResolveHeadFrameVariant(Sprite resolvedSprite, int sourceSpriteId, int frame)
	{
		ValueTuple<int, int> valueTuple = new ValueTuple<int, int>(sourceSpriteId, frame);
		Sprite sprite;
		if (this.headFrameVariantCache.TryGetValue(valueTuple, out sprite))
		{
			return sprite;
		}
		string text = SkinChangerGK2.SanitizeSpriteName(resolvedSprite.name);
		if (text.IndexOf("_static_", StringComparison.Ordinal) < 0)
		{
			this.headFrameVariantCache[valueTuple] = null;
			return null;
		}
		string text2 = string.Format("{0}_{1:D2}", text, frame);
		Sprite sprite2 = null;
		if (LazySingletonSO<EasySpritesCollection>.Instance.HasSprite(text2))
		{
			sprite2 = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(text2, null);
		}
		this.headFrameVariantCache[valueTuple] = sprite2;
		return sprite2;
	}

	// Token: 0x060049F6 RID: 18934 RVA: 0x0015D3E0 File Offset: 0x0015B5E0
	private static string SanitizeSpriteName(string spriteName)
	{
		if (string.IsNullOrEmpty(spriteName))
		{
			return string.Empty;
		}
		int num = spriteName.IndexOf("(Clone)", StringComparison.Ordinal);
		if (num >= 0)
		{
			spriteName = spriteName.Substring(0, num);
		}
		return spriteName.Trim();
	}

	// Token: 0x060049F7 RID: 18935 RVA: 0x0015D41C File Offset: 0x0015B61C
	private void DetectCustomHeadFrames()
	{
		this.HasCustomHeadFrames = false;
		SkinPresetGK2 skinPresetGK = this.skin as SkinPresetGK2;
		if (skinPresetGK == null)
		{
			return;
		}
		int id = skinPresetGK.head.id;
		if (id <= 0)
		{
			return;
		}
		bool flag;
		if (SkinChangerGK2.customHeadFramesAvailabilityCache.TryGetValue(id, out flag))
		{
			this.HasCustomHeadFrames = flag;
			return;
		}
		string text = id.ToString("D4");
		bool flag2 = LazySingletonSO<EasySpritesCollection>.Instance.HasSprite(text + "_hed_static_down_02") || LazySingletonSO<EasySpritesCollection>.Instance.HasSprite(text + "_hed_static_down_03");
		SkinChangerGK2.customHeadFramesAvailabilityCache[id] = flag2;
		this.HasCustomHeadFrames = flag2;
	}

	// Token: 0x060049F8 RID: 18936 RVA: 0x0015D4BC File Offset: 0x0015B6BC
	private void DetectCustomBeardFrames()
	{
		this.hasCustomBeardFrames = false;
		SkinPresetGK2 skinPresetGK = this.skin as SkinPresetGK2;
		if (skinPresetGK == null)
		{
			return;
		}
		int id = skinPresetGK.beard.id;
		if (id <= 0)
		{
			return;
		}
		bool flag;
		if (SkinChangerGK2.customBeardFramesAvailabilityCache.TryGetValue(id, out flag))
		{
			this.hasCustomBeardFrames = flag;
			return;
		}
		string text = id.ToString("D4");
		bool flag2 = LazySingletonSO<EasySpritesCollection>.Instance.HasSprite(text + "_brd_static_down_02") || LazySingletonSO<EasySpritesCollection>.Instance.HasSprite(text + "_brd_static_down_03");
		SkinChangerGK2.customBeardFramesAvailabilityCache[id] = flag2;
		this.hasCustomBeardFrames = flag2;
	}

	// Token: 0x060049F9 RID: 18937 RVA: 0x0015D55C File Offset: 0x0015B75C
	private bool TryGetAnimationSpriteId(SpriteRenderer spriteRenderer, out int id)
	{
		id = 0;
		if (spriteRenderer.sprite == null)
		{
			return false;
		}
		id = spriteRenderer.sprite.GetInstanceID();
		bool flag;
		if (!this.validSpriteHash.TryGetValue(id, out flag))
		{
			flag = this.IsValidSprite(spriteRenderer);
			this.validSpriteHash.Add(id, flag);
		}
		return flag;
	}

	// Token: 0x060049FA RID: 18938 RVA: 0x0015D5B4 File Offset: 0x0015B7B4
	private Sprite ResolveSkinnedSprite(SpriteRenderer spriteRenderer, int sourceSpriteId)
	{
		Sprite sprite;
		if (this.skinnedSpriteTopLevelHash.TryGetValue(sourceSpriteId, out sprite))
		{
			return sprite;
		}
		string name = spriteRenderer.sprite.name;
		char c = name[5];
		char c2 = name[6];
		char c3 = name[7];
		int num = this.skin.DefineSkinIdFor(c, c2, c3);
		if (num == 0 || string.IsNullOrEmpty(name))
		{
			sprite = null;
		}
		else if (num == -1)
		{
			sprite = spriteRenderer.sprite;
		}
		else
		{
			sprite = this.GetSkinnedSpriteFromName(name, c, c2, c3, num);
		}
		this.skinnedSpriteTopLevelHash.Add(sourceSpriteId, sprite);
		return sprite;
	}

	// Token: 0x060049FB RID: 18939 RVA: 0x0015D644 File Offset: 0x0015B844
	private Sprite GetSkinnedSpriteFromName(string originalSpriteName, char char5, char char6, char char7, int newSkinId)
	{
		GarbagelessStrings.StringToChars(ref originalSpriteName, ref SkinChangerGK2.chars);
		GarbagelessStrings.IntToCharsWithLeadingZeros(newSkinId, ref SkinChangerGK2.chars, 4, 0);
		int num = GarbagelessStrings.GetHashCode(ref SkinChangerGK2.chars);
		Sprite sprite;
		if (SkinChangerGK2.spriteHash.TryGetValue(num, out sprite))
		{
			return sprite;
		}
		string text = GarbagelessStrings.CharsToString(ref SkinChangerGK2.chars);
		sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(text, null);
		if (sprite == null && this.fallbackSkin != null)
		{
			int num2 = this.fallbackSkin.DefineSkinIdFor(char5, char6, char7);
			if (num2 != 0)
			{
				GarbagelessStrings.IntToCharsWithLeadingZeros(num2, ref SkinChangerGK2.chars, 4, 0);
				num = GarbagelessStrings.GetHashCode(ref SkinChangerGK2.chars);
				if (!SkinChangerGK2.spriteHash.TryGetValue(num, out sprite))
				{
					text = GarbagelessStrings.CharsToString(ref SkinChangerGK2.chars);
					sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(text, null);
				}
			}
		}
		SkinChangerGK2.CacheSprite(num, sprite);
		return sprite;
	}

	// Token: 0x060049FC RID: 18940 RVA: 0x0015D710 File Offset: 0x0015B910
	private static void CacheSprite(int hash, Sprite sprite)
	{
		if (SkinChangerGK2.spriteHash.Count >= 4096)
		{
			SkinChangerGK2.spriteHash.Clear();
		}
		SkinChangerGK2.spriteHash[hash] = sprite;
	}

	// Token: 0x060049FD RID: 18941 RVA: 0x0015D73C File Offset: 0x0015B93C
	private static void ApplyResolvedSprite(SpriteRenderer spriteRenderer, Sprite sprite)
	{
		if (spriteRenderer.sprite != sprite)
		{
			spriteRenderer.sprite = sprite;
		}
		bool flag = sprite != null;
		if (spriteRenderer.enabled != flag)
		{
			spriteRenderer.enabled = flag;
		}
	}

	// Token: 0x060049FE RID: 18942 RVA: 0x0015D776 File Offset: 0x0015B976
	public void ApplyShaderToAllSprites(Shader shader)
	{
		this.ApplyShaderToSprites(shader, this.sprites);
	}

	// Token: 0x060049FF RID: 18943 RVA: 0x0015D788 File Offset: 0x0015B988
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

	// Token: 0x06004A00 RID: 18944 RVA: 0x0015D7DD File Offset: 0x0015B9DD
	public void ApplyShaderParameters()
	{
		this.skin.ApplyShaderParametersTo(this.sprites);
	}

	// Token: 0x06004A01 RID: 18945 RVA: 0x0015D7F0 File Offset: 0x0015B9F0
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

	// Token: 0x06004A02 RID: 18946 RVA: 0x0015D874 File Offset: 0x0015BA74
	private void InitSkinChanger(GameObject gameObject, bool applyShader = false)
	{
		this.gameObject = gameObject;
		for (int i = this.sprites.Count - 1; i >= 0; i--)
		{
			if (this.sprites[i].gameObject.name.StartsWith("-"))
			{
				this.sprites.RemoveAt(i);
			}
		}
		this.headSpriteRenderer = this.FindMarkedRenderer<HeadSpriteComponentMarker>();
		this.beardSpriteRenderer = this.FindMarkedRenderer<BeardSpriteComponentMarker>();
		if (applyShader)
		{
			this.ApplyShaderToAllSprites(Shader.Find("Sprites/ColorReplace"));
		}
	}

	// Token: 0x06004A03 RID: 18947 RVA: 0x0015D8FC File Offset: 0x0015BAFC
	private SpriteRenderer FindMarkedRenderer<T>() where T : Component
	{
		for (int i = 0; i < this.sprites.Count; i++)
		{
			if (this.sprites[i] != null && this.sprites[i].GetComponent<T>() != null)
			{
				return this.sprites[i];
			}
		}
		T componentInChildren = this.gameObject.GetComponentInChildren<T>(true);
		if (!(componentInChildren != null))
		{
			return null;
		}
		return componentInChildren.GetComponent<SpriteRenderer>();
	}

	// Token: 0x06004A04 RID: 18948 RVA: 0x0015D988 File Offset: 0x0015BB88
	private bool IsValidSprite(SpriteRenderer spriteRenderer)
	{
		if (spriteRenderer.sprite == null)
		{
			return false;
		}
		string name = spriteRenderer.sprite.name;
		if (name.Length <= 8)
		{
			return false;
		}
		for (int i = 0; i < 4; i++)
		{
			char c = name[i];
			if (c < '0' || c > '9')
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x040039A7 RID: 14759
	private const int MAX_STATIC_SPRITE_CACHE_SIZE = 4096;

	// Token: 0x040039A8 RID: 14760
	private const string STATIC_TOKEN = "_static_";

	// Token: 0x040039A9 RID: 14761
	protected List<SpriteRenderer> sprites;

	// Token: 0x040039AA RID: 14762
	protected GameObject gameObject;

	// Token: 0x040039AB RID: 14763
	private SkinPresetBase skin;

	// Token: 0x040039AC RID: 14764
	private SkinPresetBase fallbackSkin;

	// Token: 0x040039AD RID: 14765
	private Dictionary<int, Sprite> skinnedSpriteTopLevelHash = new Dictionary<int, Sprite>();

	// Token: 0x040039AE RID: 14766
	private Dictionary<int, bool> validSpriteHash = new Dictionary<int, bool>();

	// Token: 0x040039AF RID: 14767
	[TupleElementNames(new string[] { "sourceSpriteId", "frame" })]
	private Dictionary<ValueTuple<int, int>, Sprite> headFrameVariantCache = new Dictionary<ValueTuple<int, int>, Sprite>();

	// Token: 0x040039B0 RID: 14768
	private SpriteRenderer headSpriteRenderer;

	// Token: 0x040039B1 RID: 14769
	private SpriteRenderer beardSpriteRenderer;

	// Token: 0x040039B2 RID: 14770
	private int headFrameOverride = 1;

	// Token: 0x040039B3 RID: 14771
	private bool hasCustomBeardFrames;

	// Token: 0x040039B4 RID: 14772
	private static Dictionary<int, Sprite> spriteHash = new Dictionary<int, Sprite>();

	// Token: 0x040039B5 RID: 14773
	private static Dictionary<int, bool> customHeadFramesAvailabilityCache = new Dictionary<int, bool>();

	// Token: 0x040039B6 RID: 14774
	private static Dictionary<int, bool> customBeardFramesAvailabilityCache = new Dictionary<int, bool>();

	// Token: 0x040039B7 RID: 14775
	private static char[] chars = new char[100];

	// Token: 0x040039B8 RID: 14776
	public static readonly int shaderColorId = Shader.PropertyToID("_Color");

	// Token: 0x040039B9 RID: 14777
	public static readonly int shaderHueShiftId = Shader.PropertyToID("_HueShift");

	// Token: 0x040039BA RID: 14778
	public static readonly int shaderSaturationId = Shader.PropertyToID("_Sat");

	// Token: 0x040039BB RID: 14779
	public static readonly int shaderValueId = Shader.PropertyToID("_Val");

	// Token: 0x040039BC RID: 14780
	public static readonly int shaderPaletteId = Shader.PropertyToID("_Palette");

	// Token: 0x040039BD RID: 14781
	public static readonly int shaderPaletteLutId = Shader.PropertyToID("_ReplaceLUT");

	// Token: 0x040039BE RID: 14782
	public static readonly int shaderBrightnessId = Shader.PropertyToID("_Brightness");

	// Token: 0x040039BF RID: 14783
	public static readonly int shaderContrastId = Shader.PropertyToID("_Contrast");
}
