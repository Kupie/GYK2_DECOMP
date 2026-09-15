using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore;

namespace Rewired.Glyphs.UnityUI
{
	// Token: 0x02000075 RID: 117
	[AddComponentMenu("Rewired/Glyphs/Unity UI/Unity UI Text Mesh Pro Glyph Helper")]
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class UnityUITextMeshProGlyphHelper : MonoBehaviour
	{
		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000616 RID: 1558 RVA: 0x0000EB98 File Offset: 0x0000CD98
		private UnityUITextMeshProGlyphHelper.Tag.Pool<UnityUITextMeshProGlyphHelper.ControllerElementTag> controllerElementTagPool
		{
			get
			{
				if (this.__controllerElementTagPool == null)
				{
					return this.__controllerElementTagPool = new UnityUITextMeshProGlyphHelper.Tag.Pool<UnityUITextMeshProGlyphHelper.ControllerElementTag>();
				}
				return this.__controllerElementTagPool;
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000617 RID: 1559 RVA: 0x0000EBC4 File Offset: 0x0000CDC4
		private UnityUITextMeshProGlyphHelper.Tag.Pool<UnityUITextMeshProGlyphHelper.ActionTag> actionTagPool
		{
			get
			{
				if (this.__actionTagPool == null)
				{
					return this.__actionTagPool = new UnityUITextMeshProGlyphHelper.Tag.Pool<UnityUITextMeshProGlyphHelper.ActionTag>();
				}
				return this.__actionTagPool;
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000618 RID: 1560 RVA: 0x0000EBF0 File Offset: 0x0000CDF0
		private UnityUITextMeshProGlyphHelper.Tag.Pool<UnityUITextMeshProGlyphHelper.PlayerTag> playerTagPool
		{
			get
			{
				if (this.__playerTagPool == null)
				{
					return this.__playerTagPool = new UnityUITextMeshProGlyphHelper.Tag.Pool<UnityUITextMeshProGlyphHelper.PlayerTag>();
				}
				return this.__playerTagPool;
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000619 RID: 1561 RVA: 0x0000EC1C File Offset: 0x0000CE1C
		private Dictionary<string, UnityUITextMeshProGlyphHelper.ParseTagAttributesHandler> tagHandlers
		{
			get
			{
				if (this.__tagHandlers == null)
				{
					Dictionary<string, UnityUITextMeshProGlyphHelper.ParseTagAttributesHandler> dictionary = new Dictionary<string, UnityUITextMeshProGlyphHelper.ParseTagAttributesHandler>();
					dictionary.Add("rewiredelement", new UnityUITextMeshProGlyphHelper.ParseTagAttributesHandler(this.ProcessTag_ControllerElement));
					dictionary.Add("rewiredaction", new UnityUITextMeshProGlyphHelper.ParseTagAttributesHandler(this.ProcessTag_Action));
					dictionary.Add("rewiredplayer", new UnityUITextMeshProGlyphHelper.ParseTagAttributesHandler(this.ProcessTag_Player));
					Dictionary<string, UnityUITextMeshProGlyphHelper.ParseTagAttributesHandler> dictionary2 = dictionary;
					this.__tagHandlers = dictionary;
					return dictionary2;
				}
				return this.__tagHandlers;
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x0600061A RID: 1562 RVA: 0x0000EC8B File Offset: 0x0000CE8B
		// (set) Token: 0x0600061B RID: 1563 RVA: 0x0000EC93 File Offset: 0x0000CE93
		public virtual string text
		{
			get
			{
				return this._text;
			}
			set
			{
				this._text = value;
				this.RequireRebuild();
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x0600061C RID: 1564 RVA: 0x0000ECA2 File Offset: 0x0000CEA2
		// (set) Token: 0x0600061D RID: 1565 RVA: 0x0000ECAA File Offset: 0x0000CEAA
		public virtual ControllerElementGlyphSelectorOptionsSOBase options
		{
			get
			{
				return this._options;
			}
			set
			{
				this._options = value;
				this.RequireRebuild();
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x0600061E RID: 1566 RVA: 0x0000ECB9 File Offset: 0x0000CEB9
		// (set) Token: 0x0600061F RID: 1567 RVA: 0x0000ECC4 File Offset: 0x0000CEC4
		public virtual UnityUITextMeshProGlyphHelper.TMProSpriteOptions spriteOptions
		{
			get
			{
				return this._spriteOptions;
			}
			set
			{
				this._spriteOptions = value;
				int count = this._assignedAssets.Count;
				for (int i = 0; i < count; i++)
				{
					int spriteCount = this._assignedAssets[i].spriteAsset.spriteCount;
					for (int j = 0; j < spriteCount; j++)
					{
						UnityUITextMeshProGlyphHelper.ITMProSprite sprite = this._assignedAssets[i].spriteAsset.GetSprite(j);
						if (sprite != null && !(sprite.sprite == null))
						{
							Rect rect = sprite.sprite.rect;
							sprite.xOffset = rect.width * this._spriteOptions.offsetSizeMultiplier.x + this._spriteOptions.extraOffset.x;
							sprite.yOffset = rect.height * this._spriteOptions.offsetSizeMultiplier.y + this._spriteOptions.extraOffset.y;
							sprite.xAdvance = rect.width * this._spriteOptions.xAdvanceWidthMultiplier + this._spriteOptions.extraXAdvance;
							sprite.scale = this._spriteOptions.scale;
						}
					}
					TMPro_EventManager.ON_SPRITE_ASSET_PROPERTY_CHANGED(true, this._assignedAssets[i].spriteAsset.GetSpriteAsset());
				}
			}
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x0000EE16 File Offset: 0x0000D016
		// (set) Token: 0x06000621 RID: 1569 RVA: 0x0000EE20 File Offset: 0x0000D020
		public virtual Material baseSpriteMaterial
		{
			get
			{
				return this._baseSpriteMaterial;
			}
			set
			{
				this._baseSpriteMaterial = value;
				Material sourceMaterial = ((this._baseSpriteMaterial != null) ? this._baseSpriteMaterial : this._primaryAsset.material);
				this.ForEachAsset(delegate(UnityUITextMeshProGlyphHelper.Asset asset)
				{
					UnityUITextMeshProGlyphHelper.CopyMaterialProperties(sourceMaterial, asset.material);
					if (this._overrideSpriteMaterialProperties)
					{
						UnityUITextMeshProGlyphHelper.CopySpriteMaterialPropertiesToMaterial(this._spriteMaterialProperties, asset.material);
					}
					TMPro_EventManager.ON_MATERIAL_PROPERTY_CHANGED(true, asset.material);
				});
			}
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000622 RID: 1570 RVA: 0x0000EE7A File Offset: 0x0000D07A
		// (set) Token: 0x06000623 RID: 1571 RVA: 0x0000EE84 File Offset: 0x0000D084
		public virtual bool overrideSpriteMaterialProperties
		{
			get
			{
				return this._overrideSpriteMaterialProperties;
			}
			set
			{
				this._overrideSpriteMaterialProperties = value;
				if (value)
				{
					this.ForEachAsset(delegate(UnityUITextMeshProGlyphHelper.Asset asset)
					{
						UnityUITextMeshProGlyphHelper.CopySpriteMaterialPropertiesToMaterial(this._spriteMaterialProperties, asset.material);
						TMPro_EventManager.ON_MATERIAL_PROPERTY_CHANGED(true, asset.material);
					});
					return;
				}
				Material sourceMaterial = ((this._baseSpriteMaterial != null) ? this._baseSpriteMaterial : this._primaryAsset.material);
				this.ForEachAsset(delegate(UnityUITextMeshProGlyphHelper.Asset asset)
				{
					UnityUITextMeshProGlyphHelper.CopyMaterialProperties(sourceMaterial, asset.material);
					TMPro_EventManager.ON_MATERIAL_PROPERTY_CHANGED(true, asset.material);
				});
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000624 RID: 1572 RVA: 0x0000EEED File Offset: 0x0000D0ED
		// (set) Token: 0x06000625 RID: 1573 RVA: 0x0000EEF5 File Offset: 0x0000D0F5
		public virtual UnityUITextMeshProGlyphHelper.SpriteMaterialProperties spriteMaterialProperties
		{
			get
			{
				return this._spriteMaterialProperties;
			}
			set
			{
				this._spriteMaterialProperties = value;
				if (!this._overrideSpriteMaterialProperties)
				{
					return;
				}
				this.ForEachAsset(delegate(UnityUITextMeshProGlyphHelper.Asset asset)
				{
					UnityUITextMeshProGlyphHelper.CopySpriteMaterialPropertiesToMaterial(this._spriteMaterialProperties, asset.material);
					TMPro_EventManager.ON_MATERIAL_PROPERTY_CHANGED(true, asset.material);
				});
			}
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x0000EF19 File Offset: 0x0000D119
		protected virtual void OnEnable()
		{
			this.Initialize();
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x0000EF22 File Offset: 0x0000D122
		protected virtual void Start()
		{
			this.MainUpdate();
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x0000EF2A File Offset: 0x0000D12A
		protected virtual void Update()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			this.MainUpdate();
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x0000EF3C File Offset: 0x0000D13C
		protected virtual void OnDestroy()
		{
			if (this._primaryAsset != null)
			{
				if (this._tmProText != null && this._tmProText.spriteAsset == this._primaryAsset.spriteAsset.GetSpriteAsset())
				{
					this._tmProText.spriteAsset = null;
				}
				this._primaryAsset.Destroy();
				this._primaryAsset = null;
			}
			for (int i = 0; i < this._assignedAssets.Count; i++)
			{
				if (this._assignedAssets[i] != null)
				{
					this._assignedAssets[i].Destroy();
				}
			}
			this._assignedAssets.Clear();
			for (int j = 0; j < this._assetsPool.Count; j++)
			{
				if (this._assetsPool[j] != null)
				{
					this._assetsPool[j].Destroy();
				}
			}
			this._assetsPool.Clear();
			if (this._stubTexture != null)
			{
				global::UnityEngine.Object.Destroy(this._stubTexture);
				this._stubTexture = null;
			}
			for (int k = 0; k < this._currentTags.Count; k++)
			{
				this._currentTags[k].ReturnToPool();
			}
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x0000F066 File Offset: 0x0000D266
		public virtual void ForceUpdate()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			this._rebuildRequired = true;
			this.Update();
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x0000F080 File Offset: 0x0000D280
		protected virtual ControllerElementGlyphSelectorOptions GetOptionsOrDefault()
		{
			if (this._options != null && this._options.options == null)
			{
				Debug.LogError("Rewired: Options missing on " + typeof(ControllerElementGlyphSelectorOptions).Name + ". Global default options will be used instead.");
				return ControllerElementGlyphSelectorOptions.defaultOptions;
			}
			if (!(this._options != null))
			{
				return ControllerElementGlyphSelectorOptions.defaultOptions;
			}
			return this._options.options;
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x0000F0F0 File Offset: 0x0000D2F0
		private bool Initialize()
		{
			if (this._initialized)
			{
				return true;
			}
			this._tmProText = base.GetComponent<TextMeshProUGUI>();
			this._stubTexture = new Texture2D(1, 1);
			this.CreatePrimaryAsset();
			this._initialized = true;
			return true;
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x0000F124 File Offset: 0x0000D324
		private void MainUpdate()
		{
			bool flag = false;
			int count = this._currentTags.Count;
			for (int i = 0; i < count; i++)
			{
				UnityUITextMeshProGlyphHelper.Tag tag = this._currentTags[i];
				switch (tag.tagType)
				{
				case UnityUITextMeshProGlyphHelper.Tag.TagType.ControllerElement:
				{
					UnityUITextMeshProGlyphHelper.ControllerElementTag controllerElementTag = (UnityUITextMeshProGlyphHelper.ControllerElementTag)tag;
					this._glyphsOrTextTemp.Clear();
					this.TryGetControllerElementGlyphsOrText((UnityUITextMeshProGlyphHelper.ControllerElementTag)tag, this._glyphsOrTextTemp);
					if (!UnityUITextMeshProGlyphHelper.IsEqual(this._glyphsOrTextTemp, controllerElementTag.glyphsOrText))
					{
						flag = true;
					}
					break;
				}
				case UnityUITextMeshProGlyphHelper.Tag.TagType.Action:
				{
					UnityUITextMeshProGlyphHelper.ActionTag actionTag = (UnityUITextMeshProGlyphHelper.ActionTag)tag;
					string text;
					this.TryGetActionDisplayName(actionTag, out text);
					if (!string.Equals(actionTag.displayName, text, StringComparison.Ordinal))
					{
						flag = true;
					}
					break;
				}
				case UnityUITextMeshProGlyphHelper.Tag.TagType.Player:
				{
					UnityUITextMeshProGlyphHelper.PlayerTag playerTag = (UnityUITextMeshProGlyphHelper.PlayerTag)tag;
					string text2;
					this.TryGetPlayerDisplayName(playerTag, out text2);
					if (!string.Equals(playerTag.displayName, text2, StringComparison.Ordinal))
					{
						flag = true;
					}
					break;
				}
				default:
					throw new NotImplementedException();
				}
			}
			if (!string.Equals(this._text, this._textPrev, StringComparison.Ordinal))
			{
				this._textPrev = this._text;
				flag = true;
			}
			if (flag || this._rebuildRequired)
			{
				string text3;
				if (this.ParseText(this._textPrev, out text3))
				{
					this._tmProText.text = text3;
				}
				else
				{
					this._tmProText.text = this._text;
				}
			}
			int count2 = this._dirtyAssets.Count;
			if (count2 > 0)
			{
				for (int j = 0; j < count2; j++)
				{
					this._dirtyAssets[j].spriteAsset.UpdateLookupTables();
				}
				this._dirtyAssets.Clear();
			}
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x0000F2B4 File Offset: 0x0000D4B4
		private bool ParseText(string text, out string newText)
		{
			newText = null;
			UnityUITextMeshProGlyphHelper.Tag.Clear(this._currentTags);
			this._currentlyUsedAssets.Clear();
			bool flag = false;
			while (this.ProcessNextTag(ref text, this._processTagSb))
			{
				flag = true;
				newText = text;
			}
			this.RemoveUnusedAssets();
			if (this._rebuildRequired)
			{
				this._rebuildRequired = false;
			}
			return flag;
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x0000F30C File Offset: 0x0000D50C
		private bool ProcessNextTag(ref string text, StringBuilder sb)
		{
			int num = 0;
			UnityUITextMeshProGlyphHelper.ParseTagAttributesHandler parseTagAttributesHandler = null;
			int num2 = -1;
			try
			{
				for (int i = 0; i < text.Length; i++)
				{
					char c = text[i];
					switch (num)
					{
					case 0:
						if (c == '<')
						{
							num2 = i;
							sb.Length = 0;
							num = 1;
						}
						break;
					case 1:
						if (UnityUITextMeshProGlyphHelper.IsValidTagNameChar(c))
						{
							sb.Append(char.ToLowerInvariant(c));
						}
						else if (char.IsWhiteSpace(c))
						{
							if (sb.Length > 0)
							{
								if (this.tagHandlers.TryGetValue(sb.ToString(), out parseTagAttributesHandler))
								{
									sb.Length = 0;
									num = 2;
								}
								else
								{
									num = 0;
									i--;
								}
							}
						}
						else
						{
							num = 0;
							i--;
						}
						break;
					case 2:
					{
						int num3 = text.IndexOf('>', i);
						if (num3 < 0)
						{
							throw new Exception("Malformed tag.");
						}
						string text2;
						if (parseTagAttributesHandler(text, i, num3 - i, out text2))
						{
							sb.Length = 0;
							if (num2 > 0)
							{
								sb.Append(text, 0, num2);
							}
							sb.Append(text2);
							int num4 = num3 + 1;
							if (num4 < text.Length)
							{
								sb.Append(text, num4, text.Length - num4);
							}
							text = sb.ToString();
							return true;
						}
						throw new Exception("Error parsing attributes.");
					}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError(ex);
			}
			return false;
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x0000F49C File Offset: 0x0000D69C
		private bool ProcessTag_ControllerElement(string text, int startIndex, int count, out string replacement)
		{
			UnityUITextMeshProGlyphHelper.ControllerElementTag controllerElementTag;
			if (!UnityUITextMeshProGlyphHelper.ControllerElementTag.TryParseString(text, startIndex, count, this._tempSb, this._tempSb2, this._tempStringDictionary, this.controllerElementTagPool, out controllerElementTag))
			{
				replacement = null;
				return false;
			}
			this._currentTags.Add(controllerElementTag);
			controllerElementTag.glyphsOrText.Clear();
			if (!this.TryGetControllerElementGlyphsOrText(controllerElementTag, controllerElementTag.glyphsOrText))
			{
				replacement = null;
				return true;
			}
			this.TryCreateTMProString(controllerElementTag.glyphsOrText, out replacement);
			return true;
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x0000F510 File Offset: 0x0000D710
		private bool ProcessTag_Action(string text, int startIndex, int count, out string replacement)
		{
			UnityUITextMeshProGlyphHelper.ActionTag actionTag;
			if (!UnityUITextMeshProGlyphHelper.ActionTag.TryParseString(text, startIndex, count, this._tempSb, this._tempSb2, this._tempStringDictionary, this.actionTagPool, out actionTag))
			{
				replacement = null;
				return false;
			}
			this._currentTags.Add(actionTag);
			this.TryGetActionDisplayName(actionTag, out replacement);
			return true;
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x0000F560 File Offset: 0x0000D760
		private bool ProcessTag_Player(string text, int startIndex, int count, out string replacement)
		{
			UnityUITextMeshProGlyphHelper.PlayerTag playerTag;
			if (!UnityUITextMeshProGlyphHelper.PlayerTag.TryParseString(text, startIndex, count, this._tempSb, this._tempSb2, this._tempStringDictionary, this.playerTagPool, out playerTag))
			{
				replacement = null;
				return false;
			}
			this._currentTags.Add(playerTag);
			this.TryGetPlayerDisplayName(playerTag, out replacement);
			return true;
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x0000F5B0 File Offset: 0x0000D7B0
		private bool TryCreateTMProString(List<UnityUITextMeshProGlyphHelper.GlyphOrText> glyphs, out string result)
		{
			StringBuilder tempSb = this._tempSb;
			tempSb.Length = 0;
			int count = glyphs.Count;
			for (int i = 0; i < count; i++)
			{
				string glyphKey = glyphs[i].glyphKey;
				if (glyphs[i].sprite != null && !string.IsNullOrEmpty(glyphKey) && this.TryAssignSprite(glyphs[i].sprite, glyphKey))
				{
					UnityUITextMeshProGlyphHelper.WriteSpriteKey(tempSb, glyphKey);
				}
				else
				{
					tempSb.Append(glyphs[i].name);
				}
				if (i < count - 1)
				{
					tempSb.Append(" ");
				}
			}
			result = tempSb.ToString();
			return !string.IsNullOrEmpty(result);
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x0000F65C File Offset: 0x0000D85C
		private bool TryGetControllerElementGlyphsOrText(UnityUITextMeshProGlyphHelper.ControllerElementTag tag, List<UnityUITextMeshProGlyphHelper.GlyphOrText> results)
		{
			if (tag == null)
			{
				return false;
			}
			this._tempAems.Clear();
			ActionElementMap actionElementMap;
			ActionElementMap actionElementMap2;
			if (!GlyphTools.TryGetActionElementMaps(tag.playerId, tag.actionId, tag.actionRange, this.GetOptionsOrDefault(), this._tempAems, out actionElementMap, out actionElementMap2))
			{
				return false;
			}
			if (actionElementMap != null && actionElementMap2 != null)
			{
				UnityUITextMeshProGlyphHelper.GlyphOrText glyphOrText = default(UnityUITextMeshProGlyphHelper.GlyphOrText);
				this._tempAems.Clear();
				this._tempAems.Add(actionElementMap);
				this._tempAems.Add(actionElementMap2);
				object obj;
				string text;
				if (UnityUITextMeshProGlyphHelper.IsGlyphAllowed(tag.type) && ActionElementMap.TryGetCombinedElementIdentifierGlyph(this._tempAems, out obj) && ActionElementMap.TryGetCombinedElementIdentifierFinalGlyphKey(this._tempAems, out text))
				{
					glyphOrText.glyphKey = text;
					glyphOrText.sprite = obj as Sprite;
					results.Add(glyphOrText);
					return true;
				}
				string text2;
				if (UnityUITextMeshProGlyphHelper.IsTextAllowed(tag.type) && ActionElementMap.TryGetCombinedElementIdentifierName(this._tempAems, out text2))
				{
					glyphOrText.name = text2;
					results.Add(glyphOrText);
					return true;
				}
			}
			bool flag = false;
			this._tempGlyphs.Clear();
			this._tempKeys.Clear();
			bool flag2 = flag | UnityUITextMeshProGlyphHelper.TryGetGlyphsOrText(actionElementMap, tag.type, this._tempGlyphs, this._tempKeys, results);
			this._tempGlyphs.Clear();
			this._tempKeys.Clear();
			return flag2 | UnityUITextMeshProGlyphHelper.TryGetGlyphsOrText(actionElementMap2, tag.type, this._tempGlyphs, this._tempKeys, results);
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x0000F7B8 File Offset: 0x0000D9B8
		private bool TryGetActionDisplayName(UnityUITextMeshProGlyphHelper.ActionTag tag, out string result)
		{
			if (tag == null)
			{
				result = null;
				return false;
			}
			InputAction action = ReInput.mapping.GetAction(tag.actionId);
			if (action == null)
			{
				result = null;
				return false;
			}
			result = action.GetDisplayName(tag.actionRange);
			tag.displayName = result;
			return true;
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x0000F800 File Offset: 0x0000DA00
		private bool TryGetPlayerDisplayName(UnityUITextMeshProGlyphHelper.PlayerTag tag, out string result)
		{
			if (tag == null)
			{
				result = null;
				return false;
			}
			Player player = ReInput.players.GetPlayer(tag.playerId);
			if (player == null)
			{
				result = null;
				return false;
			}
			result = player.descriptiveName;
			tag.displayName = result;
			return true;
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x0000F840 File Offset: 0x0000DA40
		private bool TryAssignSprite(Sprite sprite, string key)
		{
			UnityUITextMeshProGlyphHelper.Asset orCreateAsset = this.GetOrCreateAsset(sprite);
			if (orCreateAsset == null)
			{
				return false;
			}
			UnityUITextMeshProGlyphHelper.ITMProSpriteAsset spriteAsset = orCreateAsset.spriteAsset;
			if (!spriteAsset.Contains(key))
			{
				Rect rect = sprite.rect;
				UnityUITextMeshProGlyphHelper.ITMProSprite itmproSprite = UnityUITextMeshProGlyphHelper.TMProAssetVersionHelper.CreateSprite();
				itmproSprite.width = rect.width;
				itmproSprite.height = rect.height;
				itmproSprite.position = new Vector2(rect.x, rect.y);
				itmproSprite.xOffset = rect.width * this._spriteOptions.offsetSizeMultiplier.x + this._spriteOptions.extraOffset.x;
				itmproSprite.yOffset = rect.height * this._spriteOptions.offsetSizeMultiplier.y + this._spriteOptions.extraOffset.y;
				itmproSprite.xAdvance = rect.width * this._spriteOptions.xAdvanceWidthMultiplier + this._spriteOptions.extraXAdvance;
				itmproSprite.scale = this._spriteOptions.scale;
				itmproSprite.pivot = new Vector2(rect.width * -0.5f, rect.height * 0.5f);
				itmproSprite.name = key;
				itmproSprite.hashCode = TMP_TextUtilities.GetSimpleHashCode(key);
				itmproSprite.sprite = sprite;
				spriteAsset.AddSprite(itmproSprite);
				this.SetDirty(orCreateAsset);
			}
			if (!this._currentlyUsedAssets.Contains(orCreateAsset))
			{
				this._currentlyUsedAssets.Add(orCreateAsset);
			}
			return true;
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x0000F9A9 File Offset: 0x0000DBA9
		private void RequireRebuild()
		{
			this._rebuildRequired = true;
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x0000F9B2 File Offset: 0x0000DBB2
		private void CreatePrimaryAsset()
		{
			if (this._primaryAsset != null)
			{
				return;
			}
			this._primaryAsset = new UnityUITextMeshProGlyphHelper.Asset(null);
			this._tmProText.spriteAsset = this._primaryAsset.spriteAsset.GetSpriteAsset();
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x0000F9E4 File Offset: 0x0000DBE4
		private UnityUITextMeshProGlyphHelper.Asset GetOrCreateAsset(Sprite sprite)
		{
			if (sprite == null || sprite.texture == null)
			{
				return null;
			}
			int count = this._assignedAssets.Count;
			for (int i = 0; i < count; i++)
			{
				if (this._assignedAssets[i] != null && this._assignedAssets[i].spriteAsset.spriteSheet == sprite.texture)
				{
					return this._assignedAssets[i];
				}
			}
			UnityUITextMeshProGlyphHelper.Asset asset = null;
			int count2 = this._assetsPool.Count;
			for (int j = 0; j < count2; j++)
			{
				if (this._assetsPool[j] != null)
				{
					asset = this._assetsPool[j];
					this._assetsPool.RemoveAt(j);
					break;
				}
			}
			if (asset == null)
			{
				asset = this.CreateAsset();
			}
			asset.spriteAsset.spriteSheet = sprite.texture;
			asset.material.SetTexture(ShaderUtilities.ID_MainTex, sprite.texture);
			List<TMP_SpriteAsset> list = this._primaryAsset.spriteAsset.GetSpriteAsset().fallbackSpriteAssets;
			if (list == null)
			{
				list = new List<TMP_SpriteAsset>();
				this._primaryAsset.spriteAsset.GetSpriteAsset().fallbackSpriteAssets = list;
			}
			list.Add(asset.spriteAsset.GetSpriteAsset());
			this._assignedAssets.Add(asset);
			return asset;
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x0000FB30 File Offset: 0x0000DD30
		private UnityUITextMeshProGlyphHelper.Asset CreateAsset()
		{
			UnityUITextMeshProGlyphHelper.Asset asset = new UnityUITextMeshProGlyphHelper.Asset(this._baseSpriteMaterial);
			if (this._overrideSpriteMaterialProperties)
			{
				UnityUITextMeshProGlyphHelper.CopySpriteMaterialPropertiesToMaterial(this._spriteMaterialProperties, asset.material);
			}
			return asset;
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x0000FB64 File Offset: 0x0000DD64
		private void RemoveUnusedAssets()
		{
			int num = 0;
			for (int i = this._assignedAssets.Count - 1; i >= 0; i--)
			{
				UnityUITextMeshProGlyphHelper.Asset asset = this._assignedAssets[i];
				if (asset != null && !this._currentlyUsedAssets.Contains(asset))
				{
					if (num >= 2)
					{
						this._primaryAsset.spriteAsset.GetSpriteAsset().fallbackSpriteAssets.Remove(asset.spriteAsset.GetSpriteAsset());
						asset.spriteAsset.spriteSheet = null;
						asset.spriteAsset.Clear();
						asset.material.SetTexture(ShaderUtilities.ID_MainTex, this._stubTexture);
						this._assetsPool.Add(asset);
						this._assignedAssets.RemoveAt(i);
					}
					else
					{
						num++;
					}
				}
			}
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x0000FC27 File Offset: 0x0000DE27
		private void SetDirty(UnityUITextMeshProGlyphHelper.Asset asset)
		{
			if (this._dirtyAssets.Contains(asset))
			{
				return;
			}
			this._dirtyAssets.Add(asset);
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x0000FC44 File Offset: 0x0000DE44
		private void ForEachAsset(Action<UnityUITextMeshProGlyphHelper.Asset> callback)
		{
			if (callback == null)
			{
				return;
			}
			int num = this._assignedAssets.Count;
			for (int i = 0; i < num; i++)
			{
				if (this._assignedAssets[i] != null)
				{
					callback(this._assignedAssets[i]);
				}
			}
			num = this._assetsPool.Count;
			for (int j = 0; j < num; j++)
			{
				if (this._assetsPool[j] != null)
				{
					callback(this._assetsPool[j]);
				}
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x0600063F RID: 1599 RVA: 0x0000FCC5 File Offset: 0x0000DEC5
		private static int shaderPropertyId_color
		{
			get
			{
				return Shader.PropertyToID("_Color");
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000640 RID: 1600 RVA: 0x0000FCD1 File Offset: 0x0000DED1
		private static string[] s_displayTypeNames
		{
			get
			{
				if (UnityUITextMeshProGlyphHelper.__s_displayTypeNames == null)
				{
					return UnityUITextMeshProGlyphHelper.__s_displayTypeNames = Enum.GetNames(typeof(UnityUITextMeshProGlyphHelper.DisplayType));
				}
				return UnityUITextMeshProGlyphHelper.__s_displayTypeNames;
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000641 RID: 1601 RVA: 0x0000FCF5 File Offset: 0x0000DEF5
		private static UnityUITextMeshProGlyphHelper.DisplayType[] s_displayTypeValues
		{
			get
			{
				if (UnityUITextMeshProGlyphHelper.__s_displayTypeValues == null)
				{
					return UnityUITextMeshProGlyphHelper.__s_displayTypeValues = (UnityUITextMeshProGlyphHelper.DisplayType[])Enum.GetValues(typeof(UnityUITextMeshProGlyphHelper.DisplayType));
				}
				return UnityUITextMeshProGlyphHelper.__s_displayTypeValues;
			}
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000642 RID: 1602 RVA: 0x0000FD1E File Offset: 0x0000DF1E
		private static string[] s_axisRangeNames
		{
			get
			{
				if (UnityUITextMeshProGlyphHelper.__s_axisRangeNames == null)
				{
					return UnityUITextMeshProGlyphHelper.__s_axisRangeNames = Enum.GetNames(typeof(AxisRange));
				}
				return UnityUITextMeshProGlyphHelper.__s_axisRangeNames;
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000643 RID: 1603 RVA: 0x0000FD42 File Offset: 0x0000DF42
		private static AxisRange[] s_axisRangeValues
		{
			get
			{
				if (UnityUITextMeshProGlyphHelper.__s_axisRangeValues == null)
				{
					return UnityUITextMeshProGlyphHelper.__s_axisRangeValues = (AxisRange[])Enum.GetValues(typeof(AxisRange));
				}
				return UnityUITextMeshProGlyphHelper.__s_axisRangeValues;
			}
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x0000FD6C File Offset: 0x0000DF6C
		private static void ParseAttributes(string text, int startIndex, int count, StringBuilder sbKey, StringBuilder sbValue, Dictionary<string, string> results)
		{
			if (string.IsNullOrEmpty(text) || startIndex < 0 || startIndex >= text.Length)
			{
				return;
			}
			results.Clear();
			sbKey.Length = 0;
			sbValue.Length = 0;
			bool flag = true;
			int num = startIndex + count - 1;
			int num2 = 0;
			try
			{
				for (int i = startIndex; i < startIndex + count; i++)
				{
					char c = text[i];
					switch (num2)
					{
					case 0:
						if (UnityUITextMeshProGlyphHelper.IsValidKeyChar(c))
						{
							num2 = 1;
							i--;
							sbKey.Length = 0;
						}
						break;
					case 1:
						if (c == '=')
						{
							if (sbKey.Length == 0)
							{
								throw new Exception("Key was blank.");
							}
							num2 = 2;
						}
						else if (UnityUITextMeshProGlyphHelper.IsValidKeyChar(c))
						{
							sbKey.Append(char.ToLowerInvariant(c));
						}
						else if (!char.IsWhiteSpace(c))
						{
							throw new Exception("Error parsing key.");
						}
						break;
					case 2:
						if ((flag = c == '"') || UnityUITextMeshProGlyphHelper.IsValidNonQuotedValueChar(c))
						{
							if (!flag)
							{
								i--;
							}
							sbValue.Length = 0;
							num2 = 3;
						}
						break;
					case 3:
						if ((flag && c == '"') || (!flag && (i == num || char.IsWhiteSpace(c))))
						{
							if (!flag && i == num)
							{
								sbValue.Append(c);
							}
							if (sbValue.Length == 0)
							{
								throw new Exception("Value was blank.");
							}
							if (results == null)
							{
								results = new Dictionary<string, string>();
							}
							results.Add(sbKey.ToString(), sbValue.ToString());
							num2 = 0;
						}
						else
						{
							sbValue.Append(c);
						}
						break;
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError(ex);
			}
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x0000FF10 File Offset: 0x0000E110
		private static bool IsValidKeyChar(char c)
		{
			return char.IsLetterOrDigit(c) || c == '_';
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x0000FF10 File Offset: 0x0000E110
		private static bool IsValidTagNameChar(char c)
		{
			return char.IsLetterOrDigit(c) || c == '_';
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x0000FF21 File Offset: 0x0000E121
		private static bool IsValidNonQuotedValueChar(char c)
		{
			return char.IsDigit(c);
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x0000FF2C File Offset: 0x0000E12C
		private static bool IsEqual(List<UnityUITextMeshProGlyphHelper.GlyphOrText> a, List<UnityUITextMeshProGlyphHelper.GlyphOrText> b)
		{
			if (a.Count != b.Count)
			{
				return false;
			}
			for (int i = 0; i < a.Count; i++)
			{
				if (a[i] != b[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x0000FF72 File Offset: 0x0000E172
		private static void WriteSpriteKey(StringBuilder sb, string key)
		{
			sb.Append("<sprite name=\"");
			sb.Append(key);
			sb.Append("\">");
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x0000FF94 File Offset: 0x0000E194
		private static bool TryGetGlyphsOrText(ActionElementMap aem, UnityUITextMeshProGlyphHelper.DisplayType displayType, List<Sprite> glyphs, List<string> keys, List<UnityUITextMeshProGlyphHelper.GlyphOrText> results)
		{
			if (aem == null || glyphs == null || results == null)
			{
				return false;
			}
			if (UnityUITextMeshProGlyphHelper.IsGlyphAllowed(displayType) && aem.GetElementIdentifierGlyphs<Sprite>(glyphs) > 0)
			{
				aem.GetElementIdentifierFinalGlyphKeys(keys);
				if (keys.Count != glyphs.Count)
				{
					Debug.LogError("Rewired: Glyph key count does not match glyph count.");
				}
				else
				{
					int count = glyphs.Count;
					for (int i = 0; i < count; i++)
					{
						results.Add(new UnityUITextMeshProGlyphHelper.GlyphOrText
						{
							glyphKey = keys[i],
							sprite = glyphs[i]
						});
					}
					if (count > 0)
					{
						return true;
					}
				}
			}
			if (UnityUITextMeshProGlyphHelper.IsTextAllowed(displayType))
			{
				results.Add(new UnityUITextMeshProGlyphHelper.GlyphOrText
				{
					name = aem.elementIdentifierName
				});
				return true;
			}
			return false;
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x0001004E File Offset: 0x0000E24E
		private static bool IsGlyphAllowed(UnityUITextMeshProGlyphHelper.DisplayType displayType)
		{
			return displayType == UnityUITextMeshProGlyphHelper.DisplayType.Glyph || displayType == UnityUITextMeshProGlyphHelper.DisplayType.GlyphOrText;
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x00010059 File Offset: 0x0000E259
		private static bool IsTextAllowed(UnityUITextMeshProGlyphHelper.DisplayType displayType)
		{
			return displayType == UnityUITextMeshProGlyphHelper.DisplayType.Text || displayType == UnityUITextMeshProGlyphHelper.DisplayType.GlyphOrText;
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x00010068 File Offset: 0x0000E268
		private static void CopyMaterialProperties(Material source, Material destination)
		{
			if (source == null || destination == null)
			{
				return;
			}
			destination.shader = source.shader;
			if (source.shaderKeywords != null)
			{
				string[] array = new string[source.shaderKeywords.Length];
				Array.Copy(source.shaderKeywords, array, source.shaderKeywords.Length);
				destination.shaderKeywords = array;
			}
			else
			{
				destination.shaderKeywords = null;
			}
			if (source.HasProperty(UnityUITextMeshProGlyphHelper.shaderPropertyId_color) && destination.HasProperty(UnityUITextMeshProGlyphHelper.shaderPropertyId_color))
			{
				destination.color = source.color;
			}
			destination.renderQueue = source.renderQueue;
			destination.globalIlluminationFlags = source.globalIlluminationFlags;
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x0001010C File Offset: 0x0000E30C
		private static void CopySpriteMaterialPropertiesToMaterial(UnityUITextMeshProGlyphHelper.SpriteMaterialProperties properties, Material material)
		{
			if (material == null)
			{
				return;
			}
			if (material.HasProperty(UnityUITextMeshProGlyphHelper.shaderPropertyId_color))
			{
				material.color = properties.color;
			}
		}

		// Token: 0x04000329 RID: 809
		[Tooltip("Enter text into this field and not in the TMPro Text field directly. Text will be parsed for special tags, and the final result will be passed on to the Text Mesh Pro Text component. See the documentation for special tag format.")]
		[SerializeField]
		[TextArea(3, 10)]
		private string _text;

		// Token: 0x0400032A RID: 810
		[Tooltip("Optional reference to an object that defines options. If blank, the global default options will be used.")]
		[SerializeField]
		private ControllerElementGlyphSelectorOptionsSOBase _options;

		// Token: 0x0400032B RID: 811
		[Tooltip("Options that control how Text Mesh Pro displays Sprites.")]
		[SerializeField]
		private UnityUITextMeshProGlyphHelper.TMProSpriteOptions _spriteOptions = UnityUITextMeshProGlyphHelper.TMProSpriteOptions.Default;

		// Token: 0x0400032C RID: 812
		[Tooltip("Optional material for Sprites. If blank, the default material will be used.\nMaterial is instantiated for each Sprite Asset, so making changes to values in the base material later will not affect Sprites. Changing the base material at runtime will copy only certain properties from the new material to Sprite materials.")]
		[SerializeField]
		private Material _baseSpriteMaterial;

		// Token: 0x0400032D RID: 813
		[Tooltip("If enabled, local values such as Sprite color will be used instead of the value on the base material.")]
		[SerializeField]
		private bool _overrideSpriteMaterialProperties = true;

		// Token: 0x0400032E RID: 814
		[Tooltip("These properties will override the properties on the Sprite material if Override Sprite Material Properties is enabled.")]
		[SerializeField]
		private UnityUITextMeshProGlyphHelper.SpriteMaterialProperties _spriteMaterialProperties = UnityUITextMeshProGlyphHelper.SpriteMaterialProperties.Default;

		// Token: 0x0400032F RID: 815
		[NonSerialized]
		private TextMeshProUGUI _tmProText;

		// Token: 0x04000330 RID: 816
		[NonSerialized]
		private string _textPrev;

		// Token: 0x04000331 RID: 817
		[NonSerialized]
		private readonly StringBuilder _processTagSb = new StringBuilder();

		// Token: 0x04000332 RID: 818
		[NonSerialized]
		private readonly StringBuilder _tempSb = new StringBuilder();

		// Token: 0x04000333 RID: 819
		[NonSerialized]
		private readonly StringBuilder _tempSb2 = new StringBuilder();

		// Token: 0x04000334 RID: 820
		[NonSerialized]
		private UnityUITextMeshProGlyphHelper.Asset _primaryAsset;

		// Token: 0x04000335 RID: 821
		[NonSerialized]
		private readonly List<UnityUITextMeshProGlyphHelper.Asset> _assignedAssets = new List<UnityUITextMeshProGlyphHelper.Asset>();

		// Token: 0x04000336 RID: 822
		[NonSerialized]
		private readonly List<UnityUITextMeshProGlyphHelper.Asset> _assetsPool = new List<UnityUITextMeshProGlyphHelper.Asset>();

		// Token: 0x04000337 RID: 823
		[NonSerialized]
		private readonly List<ActionElementMap> _tempAems = new List<ActionElementMap>();

		// Token: 0x04000338 RID: 824
		[NonSerialized]
		private readonly List<Sprite> _tempGlyphs = new List<Sprite>();

		// Token: 0x04000339 RID: 825
		[NonSerialized]
		private readonly List<UnityUITextMeshProGlyphHelper.Asset> _dirtyAssets = new List<UnityUITextMeshProGlyphHelper.Asset>();

		// Token: 0x0400033A RID: 826
		[NonSerialized]
		private readonly List<string> _tempKeys = new List<string>();

		// Token: 0x0400033B RID: 827
		[NonSerialized]
		private readonly List<UnityUITextMeshProGlyphHelper.GlyphOrText> _glyphsOrTextTemp = new List<UnityUITextMeshProGlyphHelper.GlyphOrText>();

		// Token: 0x0400033C RID: 828
		[NonSerialized]
		private readonly List<UnityUITextMeshProGlyphHelper.Asset> _currentlyUsedAssets = new List<UnityUITextMeshProGlyphHelper.Asset>();

		// Token: 0x0400033D RID: 829
		[NonSerialized]
		private readonly List<UnityUITextMeshProGlyphHelper.Tag> _currentTags = new List<UnityUITextMeshProGlyphHelper.Tag>();

		// Token: 0x0400033E RID: 830
		[NonSerialized]
		private Dictionary<string, string> _tempStringDictionary = new Dictionary<string, string>();

		// Token: 0x0400033F RID: 831
		[NonSerialized]
		private bool _initialized;

		// Token: 0x04000340 RID: 832
		[NonSerialized]
		private bool _rebuildRequired;

		// Token: 0x04000341 RID: 833
		[NonSerialized]
		private Texture2D _stubTexture;

		// Token: 0x04000342 RID: 834
		private UnityUITextMeshProGlyphHelper.Tag.Pool<UnityUITextMeshProGlyphHelper.ControllerElementTag> __controllerElementTagPool;

		// Token: 0x04000343 RID: 835
		private UnityUITextMeshProGlyphHelper.Tag.Pool<UnityUITextMeshProGlyphHelper.ActionTag> __actionTagPool;

		// Token: 0x04000344 RID: 836
		private UnityUITextMeshProGlyphHelper.Tag.Pool<UnityUITextMeshProGlyphHelper.PlayerTag> __playerTagPool;

		// Token: 0x04000345 RID: 837
		[NonSerialized]
		private Dictionary<string, UnityUITextMeshProGlyphHelper.ParseTagAttributesHandler> __tagHandlers;

		// Token: 0x04000346 RID: 838
		private static string[] __s_displayTypeNames;

		// Token: 0x04000347 RID: 839
		private static UnityUITextMeshProGlyphHelper.DisplayType[] __s_displayTypeValues;

		// Token: 0x04000348 RID: 840
		private static string[] __s_axisRangeNames;

		// Token: 0x04000349 RID: 841
		private static AxisRange[] __s_axisRangeValues;

		// Token: 0x02000076 RID: 118
		// (Invoke) Token: 0x06000653 RID: 1619
		private delegate bool ParseTagAttributesHandler(string text, int startIndex, int count, out string replacement);

		// Token: 0x02000077 RID: 119
		private abstract class Tag
		{
			// Token: 0x06000656 RID: 1622 RVA: 0x00010212 File Offset: 0x0000E412
			protected Tag(UnityUITextMeshProGlyphHelper.Tag.TagType tagType)
			{
				this.tagType = tagType;
			}

			// Token: 0x170002D9 RID: 729
			// (get) Token: 0x06000657 RID: 1623 RVA: 0x00010221 File Offset: 0x0000E421
			// (set) Token: 0x06000658 RID: 1624 RVA: 0x00010229 File Offset: 0x0000E429
			protected UnityUITextMeshProGlyphHelper.Tag.Pool pool
			{
				get
				{
					return this._pool;
				}
				set
				{
					this._pool = value;
				}
			}

			// Token: 0x06000659 RID: 1625 RVA: 0x00010232 File Offset: 0x0000E432
			public void ReturnToPool()
			{
				if (this._pool == null)
				{
					return;
				}
				this._pool.Return(this);
			}

			// Token: 0x0600065A RID: 1626
			protected abstract void Clear();

			// Token: 0x0600065B RID: 1627 RVA: 0x0001024C File Offset: 0x0000E44C
			public static void Clear(List<UnityUITextMeshProGlyphHelper.Tag> list)
			{
				int count = list.Count;
				for (int i = 0; i < count; i++)
				{
					if (list[i] != null)
					{
						list[i].ReturnToPool();
					}
				}
				list.Clear();
			}

			// Token: 0x0400034A RID: 842
			public readonly UnityUITextMeshProGlyphHelper.Tag.TagType tagType;

			// Token: 0x0400034B RID: 843
			private UnityUITextMeshProGlyphHelper.Tag.Pool _pool;

			// Token: 0x02000078 RID: 120
			public enum TagType
			{
				// Token: 0x0400034D RID: 845
				ControllerElement,
				// Token: 0x0400034E RID: 846
				Action,
				// Token: 0x0400034F RID: 847
				Player
			}

			// Token: 0x02000079 RID: 121
			public abstract class Pool
			{
				// Token: 0x0600065C RID: 1628
				public abstract bool Return(UnityUITextMeshProGlyphHelper.Tag obj);
			}

			// Token: 0x0200007A RID: 122
			public sealed class Pool<T> : UnityUITextMeshProGlyphHelper.Tag.Pool where T : UnityUITextMeshProGlyphHelper.Tag, new()
			{
				// Token: 0x0600065E RID: 1630 RVA: 0x00010287 File Offset: 0x0000E487
				public Pool()
				{
					this._list = new List<T>();
				}

				// Token: 0x0600065F RID: 1631 RVA: 0x0001029C File Offset: 0x0000E49C
				public T Get()
				{
					T t;
					if (this._list.Count == 0)
					{
						t = new T();
						if (t != null)
						{
							t.pool = this;
						}
						return t;
					}
					int num = this._list.Count - 1;
					t = this._list[num];
					this._list.RemoveAt(num);
					return t;
				}

				// Token: 0x06000660 RID: 1632 RVA: 0x000102FC File Offset: 0x0000E4FC
				public override bool Return(UnityUITextMeshProGlyphHelper.Tag obj)
				{
					T t = obj as T;
					if (t == null || t.pool != this)
					{
						return false;
					}
					t.Clear();
					if (this._list.Contains(t))
					{
						return false;
					}
					this._list.Add(t);
					return true;
				}

				// Token: 0x04000350 RID: 848
				private readonly List<T> _list;
			}
		}

		// Token: 0x0200007B RID: 123
		private sealed class ControllerElementTag : UnityUITextMeshProGlyphHelper.Tag
		{
			// Token: 0x170002DA RID: 730
			// (get) Token: 0x06000661 RID: 1633 RVA: 0x00010355 File Offset: 0x0000E555
			public List<UnityUITextMeshProGlyphHelper.GlyphOrText> glyphsOrText
			{
				get
				{
					return this._glyphsOrText;
				}
			}

			// Token: 0x06000662 RID: 1634 RVA: 0x00010360 File Offset: 0x0000E560
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(typeof(UnityUITextMeshProGlyphHelper.ControllerElementTag).Name);
				stringBuilder.Append(": ");
				stringBuilder.Append("type = ");
				stringBuilder.Append(this.type);
				stringBuilder.Append(", playerId = ");
				stringBuilder.Append(this.playerId);
				stringBuilder.Append(", actionId = ");
				stringBuilder.Append(this.actionId);
				stringBuilder.Append(", actionRange = ");
				stringBuilder.Append(this.actionRange);
				return stringBuilder.ToString();
			}

			// Token: 0x06000663 RID: 1635 RVA: 0x00010407 File Offset: 0x0000E607
			public ControllerElementTag()
				: base(UnityUITextMeshProGlyphHelper.Tag.TagType.ControllerElement)
			{
				this._glyphsOrText = new List<UnityUITextMeshProGlyphHelper.GlyphOrText>();
				this.Clear();
			}

			// Token: 0x06000664 RID: 1636 RVA: 0x00010421 File Offset: 0x0000E621
			protected override void Clear()
			{
				this.type = UnityUITextMeshProGlyphHelper.DisplayType.GlyphOrText;
				this.playerId = -1;
				this.actionId = -1;
				this.actionRange = AxisRange.Full;
				this._glyphsOrText.Clear();
			}

			// Token: 0x06000665 RID: 1637 RVA: 0x0001044C File Offset: 0x0000E64C
			public static bool TryParseString(string text, int startIndex, int count, StringBuilder sb1, StringBuilder sb2, Dictionary<string, string> workDictionary, UnityUITextMeshProGlyphHelper.Tag.Pool<UnityUITextMeshProGlyphHelper.ControllerElementTag> pool, out UnityUITextMeshProGlyphHelper.ControllerElementTag result)
			{
				result = null;
				if (string.IsNullOrEmpty(text) || startIndex < 0 || startIndex + count >= text.Length)
				{
					return false;
				}
				UnityUITextMeshProGlyphHelper.ParseAttributes(text, startIndex, count, sb1, sb2, workDictionary);
				if (workDictionary.Count == 0)
				{
					return false;
				}
				result = pool.Get();
				bool flag3;
				try
				{
					string text2;
					if (workDictionary.TryGetValue("type", out text2))
					{
						bool flag = false;
						for (int i = 0; i < UnityUITextMeshProGlyphHelper.s_displayTypeNames.Length; i++)
						{
							if (string.Equals(text2, UnityUITextMeshProGlyphHelper.s_displayTypeNames[i], StringComparison.OrdinalIgnoreCase))
							{
								result.type = UnityUITextMeshProGlyphHelper.s_displayTypeValues[i];
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							throw new Exception("Invalid type: " + text2);
						}
					}
					else
					{
						result.type = UnityUITextMeshProGlyphHelper.DisplayType.GlyphOrText;
					}
					if (workDictionary.TryGetValue("playerid", out text2))
					{
						result.playerId = int.Parse(text2);
						if (ReInput.players.GetPlayer(result.playerId) == null)
						{
							throw new Exception("Invalid Player Id: " + result.playerId.ToString());
						}
					}
					else
					{
						if (!workDictionary.TryGetValue("playername", out text2))
						{
							throw new Exception("Player name/id missing.");
						}
						Player player = ReInput.players.GetPlayer(text2);
						if (player == null)
						{
							throw new Exception("Invalid Player name: " + text2);
						}
						result.playerId = player.id;
					}
					if (workDictionary.TryGetValue("actionid", out text2))
					{
						result.actionId = int.Parse(text2);
						if (ReInput.mapping.GetAction(result.actionId) == null)
						{
							throw new Exception("Invalid Action Id: " + result.actionId.ToString());
						}
					}
					else
					{
						if (!workDictionary.TryGetValue("actionname", out text2))
						{
							throw new Exception("Action name/id missing.");
						}
						InputAction action = ReInput.mapping.GetAction(text2);
						if (action == null)
						{
							throw new Exception("Invalid Action name: " + text2);
						}
						result.actionId = action.id;
					}
					if (workDictionary.TryGetValue("actionrange", out text2))
					{
						bool flag2 = false;
						for (int j = 0; j < UnityUITextMeshProGlyphHelper.s_axisRangeNames.Length; j++)
						{
							if (string.Equals(text2, UnityUITextMeshProGlyphHelper.s_axisRangeNames[j], StringComparison.OrdinalIgnoreCase))
							{
								result.actionRange = UnityUITextMeshProGlyphHelper.s_axisRangeValues[j];
								flag2 = true;
								break;
							}
						}
						if (!flag2)
						{
							throw new Exception("Invalid Action Range: " + text2);
						}
					}
					else
					{
						result.actionRange = AxisRange.Full;
					}
					flag3 = true;
				}
				catch (Exception ex)
				{
					Debug.LogError(ex);
					result.ReturnToPool();
					flag3 = false;
				}
				return flag3;
			}

			// Token: 0x04000351 RID: 849
			public UnityUITextMeshProGlyphHelper.DisplayType type;

			// Token: 0x04000352 RID: 850
			public int playerId;

			// Token: 0x04000353 RID: 851
			public int actionId;

			// Token: 0x04000354 RID: 852
			public AxisRange actionRange;

			// Token: 0x04000355 RID: 853
			private readonly List<UnityUITextMeshProGlyphHelper.GlyphOrText> _glyphsOrText;
		}

		// Token: 0x0200007C RID: 124
		private sealed class ActionTag : UnityUITextMeshProGlyphHelper.Tag
		{
			// Token: 0x170002DB RID: 731
			// (get) Token: 0x06000666 RID: 1638 RVA: 0x000106D8 File Offset: 0x0000E8D8
			// (set) Token: 0x06000667 RID: 1639 RVA: 0x000106E0 File Offset: 0x0000E8E0
			public string displayName
			{
				get
				{
					return this._displayName;
				}
				set
				{
					this._displayName = value;
				}
			}

			// Token: 0x06000668 RID: 1640 RVA: 0x000106EC File Offset: 0x0000E8EC
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(typeof(UnityUITextMeshProGlyphHelper.ControllerElementTag).Name);
				stringBuilder.Append(": ");
				stringBuilder.Append("actionId = ");
				stringBuilder.Append(this.actionId);
				stringBuilder.Append(", actionRange = ");
				stringBuilder.Append(this.actionRange);
				return stringBuilder.ToString();
			}

			// Token: 0x06000669 RID: 1641 RVA: 0x0001075C File Offset: 0x0000E95C
			public ActionTag()
				: base(UnityUITextMeshProGlyphHelper.Tag.TagType.Action)
			{
				this.Clear();
			}

			// Token: 0x0600066A RID: 1642 RVA: 0x0001076B File Offset: 0x0000E96B
			protected override void Clear()
			{
				this.actionId = -1;
				this.actionRange = AxisRange.Full;
				this._displayName = null;
			}

			// Token: 0x0600066B RID: 1643 RVA: 0x00010784 File Offset: 0x0000E984
			public static bool TryParseString(string text, int startIndex, int count, StringBuilder sb1, StringBuilder sb2, Dictionary<string, string> workDictionary, UnityUITextMeshProGlyphHelper.Tag.Pool<UnityUITextMeshProGlyphHelper.ActionTag> pool, out UnityUITextMeshProGlyphHelper.ActionTag result)
			{
				result = null;
				if (string.IsNullOrEmpty(text) || startIndex < 0 || startIndex + count >= text.Length)
				{
					return false;
				}
				UnityUITextMeshProGlyphHelper.ParseAttributes(text, startIndex, count, sb1, sb2, workDictionary);
				if (workDictionary.Count == 0)
				{
					return false;
				}
				result = pool.Get();
				bool flag2;
				try
				{
					string text2;
					if (workDictionary.TryGetValue("id", out text2) || workDictionary.TryGetValue("actionid", out text2))
					{
						result.actionId = int.Parse(text2);
						if (ReInput.mapping.GetAction(result.actionId) == null)
						{
							throw new Exception("Invalid Action Id: " + result.actionId.ToString());
						}
					}
					else
					{
						if (!workDictionary.TryGetValue("name", out text2) && !workDictionary.TryGetValue("actionname", out text2))
						{
							throw new Exception("Action name/id missing.");
						}
						InputAction action = ReInput.mapping.GetAction(text2);
						if (action == null)
						{
							throw new Exception("Invalid Action name: " + text2);
						}
						result.actionId = action.id;
					}
					if (workDictionary.TryGetValue("range", out text2) || workDictionary.TryGetValue("actionrange", out text2))
					{
						bool flag = false;
						for (int i = 0; i < UnityUITextMeshProGlyphHelper.s_axisRangeNames.Length; i++)
						{
							if (string.Equals(text2, UnityUITextMeshProGlyphHelper.s_axisRangeNames[i], StringComparison.OrdinalIgnoreCase))
							{
								result.actionRange = UnityUITextMeshProGlyphHelper.s_axisRangeValues[i];
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							throw new Exception("Invalid Action Range: " + text2);
						}
					}
					else
					{
						result.actionRange = AxisRange.Full;
					}
					flag2 = true;
				}
				catch (Exception ex)
				{
					Debug.LogError(ex);
					result.ReturnToPool();
					flag2 = false;
				}
				return flag2;
			}

			// Token: 0x04000356 RID: 854
			public int actionId;

			// Token: 0x04000357 RID: 855
			public AxisRange actionRange;

			// Token: 0x04000358 RID: 856
			private string _displayName;
		}

		// Token: 0x0200007D RID: 125
		private sealed class PlayerTag : UnityUITextMeshProGlyphHelper.Tag
		{
			// Token: 0x170002DC RID: 732
			// (get) Token: 0x0600066C RID: 1644 RVA: 0x00010934 File Offset: 0x0000EB34
			// (set) Token: 0x0600066D RID: 1645 RVA: 0x0001093C File Offset: 0x0000EB3C
			public string displayName
			{
				get
				{
					return this._displayName;
				}
				set
				{
					this._displayName = value;
				}
			}

			// Token: 0x0600066E RID: 1646 RVA: 0x00010948 File Offset: 0x0000EB48
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(typeof(UnityUITextMeshProGlyphHelper.ControllerElementTag).Name);
				stringBuilder.Append(": ");
				stringBuilder.Append("playerId = ");
				stringBuilder.Append(this.playerId);
				return stringBuilder.ToString();
			}

			// Token: 0x0600066F RID: 1647 RVA: 0x0001099A File Offset: 0x0000EB9A
			public PlayerTag()
				: base(UnityUITextMeshProGlyphHelper.Tag.TagType.Player)
			{
				this.Clear();
			}

			// Token: 0x06000670 RID: 1648 RVA: 0x000109A9 File Offset: 0x0000EBA9
			protected override void Clear()
			{
				this.playerId = -1;
				this._displayName = null;
			}

			// Token: 0x06000671 RID: 1649 RVA: 0x000109BC File Offset: 0x0000EBBC
			public static bool TryParseString(string text, int startIndex, int count, StringBuilder sb1, StringBuilder sb2, Dictionary<string, string> workDictionary, UnityUITextMeshProGlyphHelper.Tag.Pool<UnityUITextMeshProGlyphHelper.PlayerTag> pool, out UnityUITextMeshProGlyphHelper.PlayerTag result)
			{
				result = null;
				if (string.IsNullOrEmpty(text) || startIndex < 0 || startIndex + count >= text.Length)
				{
					return false;
				}
				UnityUITextMeshProGlyphHelper.ParseAttributes(text, startIndex, count, sb1, sb2, workDictionary);
				if (workDictionary.Count == 0)
				{
					return false;
				}
				result = pool.Get();
				bool flag;
				try
				{
					string text2;
					if (workDictionary.TryGetValue("id", out text2) || workDictionary.TryGetValue("playerid", out text2))
					{
						result.playerId = int.Parse(text2);
						if (ReInput.players.GetPlayer(result.playerId) == null)
						{
							throw new Exception("Invalid Player Id: " + result.playerId.ToString());
						}
					}
					else
					{
						if (!workDictionary.TryGetValue("name", out text2) && !workDictionary.TryGetValue("playername", out text2))
						{
							throw new Exception("Player name/id missing.");
						}
						Player player = ReInput.players.GetPlayer(text2);
						if (player == null)
						{
							throw new Exception("Invalid Player name: " + text2);
						}
						result.playerId = player.id;
					}
					flag = true;
				}
				catch (Exception ex)
				{
					Debug.LogError(ex);
					result.ReturnToPool();
					flag = false;
				}
				return flag;
			}

			// Token: 0x04000359 RID: 857
			public int playerId;

			// Token: 0x0400035A RID: 858
			private string _displayName;
		}

		// Token: 0x0200007E RID: 126
		private struct GlyphOrText : IEquatable<UnityUITextMeshProGlyphHelper.GlyphOrText>
		{
			// Token: 0x06000672 RID: 1650 RVA: 0x00010AE8 File Offset: 0x0000ECE8
			public override bool Equals(object obj)
			{
				if (!(obj is UnityUITextMeshProGlyphHelper.GlyphOrText))
				{
					return false;
				}
				UnityUITextMeshProGlyphHelper.GlyphOrText glyphOrText = (UnityUITextMeshProGlyphHelper.GlyphOrText)obj;
				return string.Equals(glyphOrText.glyphKey, this.glyphKey, StringComparison.Ordinal) && glyphOrText.sprite == this.sprite && string.Equals(glyphOrText.name, this.name, StringComparison.Ordinal);
			}

			// Token: 0x06000673 RID: 1651 RVA: 0x00010B41 File Offset: 0x0000ED41
			public override int GetHashCode()
			{
				return ((17 * 29 + this.glyphKey.GetHashCode()) * 29 + this.sprite.GetHashCode()) * 29 + this.name.GetHashCode();
			}

			// Token: 0x06000674 RID: 1652 RVA: 0x00010B72 File Offset: 0x0000ED72
			public bool Equals(UnityUITextMeshProGlyphHelper.GlyphOrText other)
			{
				return string.Equals(other.glyphKey, this.glyphKey, StringComparison.Ordinal) && other.sprite == this.sprite && string.Equals(other.name, this.name, StringComparison.Ordinal);
			}

			// Token: 0x06000675 RID: 1653 RVA: 0x00010BAF File Offset: 0x0000EDAF
			public static bool operator ==(UnityUITextMeshProGlyphHelper.GlyphOrText a, UnityUITextMeshProGlyphHelper.GlyphOrText b)
			{
				return string.Equals(a.glyphKey, b.glyphKey, StringComparison.Ordinal) && a.sprite == b.sprite && string.Equals(a.name, b.name, StringComparison.Ordinal);
			}

			// Token: 0x06000676 RID: 1654 RVA: 0x00010BEC File Offset: 0x0000EDEC
			public static bool operator !=(UnityUITextMeshProGlyphHelper.GlyphOrText a, UnityUITextMeshProGlyphHelper.GlyphOrText b)
			{
				return !(a == b);
			}

			// Token: 0x0400035B RID: 859
			public string glyphKey;

			// Token: 0x0400035C RID: 860
			public Sprite sprite;

			// Token: 0x0400035D RID: 861
			public string name;
		}

		// Token: 0x0200007F RID: 127
		private class Asset
		{
			// Token: 0x170002DD RID: 733
			// (get) Token: 0x06000677 RID: 1655 RVA: 0x00010BF8 File Offset: 0x0000EDF8
			public UnityUITextMeshProGlyphHelper.ITMProSpriteAsset spriteAsset
			{
				get
				{
					return this._spriteAsset;
				}
			}

			// Token: 0x170002DE RID: 734
			// (get) Token: 0x06000678 RID: 1656 RVA: 0x00010C00 File Offset: 0x0000EE00
			public Material material
			{
				get
				{
					return this._material;
				}
			}

			// Token: 0x06000679 RID: 1657 RVA: 0x00010C08 File Offset: 0x0000EE08
			public Asset(Material baseMaterial)
			{
				this.id = UnityUITextMeshProGlyphHelper.Asset.s_idCounter++;
				this._spriteAsset = UnityUITextMeshProGlyphHelper.TMProAssetVersionHelper.CreateSpriteAsset();
				TMP_SpriteAsset spriteAsset = this._spriteAsset.GetSpriteAsset();
				spriteAsset.name = typeof(UnityUITextMeshProGlyphHelper).Name + " SpriteAsset " + this.id.ToString();
				spriteAsset.hashCode = TMP_TextUtilities.GetSimpleHashCode(spriteAsset.name);
				this._material = UnityUITextMeshProGlyphHelper.Asset.CreateMaterial(baseMaterial, this.id);
				if (this._spriteAsset != null)
				{
					spriteAsset.material = this.material;
					spriteAsset.materialHashCode = TMP_TextUtilities.GetSimpleHashCode(this.material.name);
				}
			}

			// Token: 0x0600067A RID: 1658 RVA: 0x00010CBC File Offset: 0x0000EEBC
			public static Material CreateMaterial(Material baseMaterial, uint id)
			{
				Material material = ((baseMaterial != null) ? new Material(baseMaterial) : new Material(UnityUITextMeshProGlyphHelper.Asset.tmProShader));
				material.name = typeof(UnityUITextMeshProGlyphHelper).Name + " Material " + id.ToString();
				material.hideFlags = HideFlags.HideInHierarchy;
				return material;
			}

			// Token: 0x0600067B RID: 1659 RVA: 0x00010D11 File Offset: 0x0000EF11
			public void Destroy()
			{
				if (this._spriteAsset != null)
				{
					this._spriteAsset.Destroy();
					this._spriteAsset = null;
				}
				if (this._material != null)
				{
					global::UnityEngine.Object.Destroy(this._material);
					this._material = null;
				}
			}

			// Token: 0x170002DF RID: 735
			// (get) Token: 0x0600067C RID: 1660 RVA: 0x00010D4D File Offset: 0x0000EF4D
			private static Shader tmProShader
			{
				get
				{
					if (UnityUITextMeshProGlyphHelper.Asset.__tmProShader == null)
					{
						ShaderUtilities.GetShaderPropertyIDs();
						UnityUITextMeshProGlyphHelper.Asset.__tmProShader = Shader.Find("TextMeshPro/Sprite");
					}
					return UnityUITextMeshProGlyphHelper.Asset.__tmProShader;
				}
			}

			// Token: 0x0400035E RID: 862
			public readonly uint id;

			// Token: 0x0400035F RID: 863
			private UnityUITextMeshProGlyphHelper.ITMProSpriteAsset _spriteAsset;

			// Token: 0x04000360 RID: 864
			private Material _material;

			// Token: 0x04000361 RID: 865
			private static uint s_idCounter;

			// Token: 0x04000362 RID: 866
			private static Shader __tmProShader;
		}

		// Token: 0x02000080 RID: 128
		[Serializable]
		public struct TMProSpriteOptions : IEquatable<UnityUITextMeshProGlyphHelper.TMProSpriteOptions>
		{
			// Token: 0x170002E0 RID: 736
			// (get) Token: 0x0600067D RID: 1661 RVA: 0x00010D75 File Offset: 0x0000EF75
			// (set) Token: 0x0600067E RID: 1662 RVA: 0x00010D7D File Offset: 0x0000EF7D
			public float scale
			{
				get
				{
					return this._scale;
				}
				set
				{
					this._scale = value;
				}
			}

			// Token: 0x170002E1 RID: 737
			// (get) Token: 0x0600067F RID: 1663 RVA: 0x00010D86 File Offset: 0x0000EF86
			// (set) Token: 0x06000680 RID: 1664 RVA: 0x00010D8E File Offset: 0x0000EF8E
			public Vector2 offsetSizeMultiplier
			{
				get
				{
					return this._offsetSizeMultiplier;
				}
				set
				{
					this._offsetSizeMultiplier = value;
				}
			}

			// Token: 0x170002E2 RID: 738
			// (get) Token: 0x06000681 RID: 1665 RVA: 0x00010D97 File Offset: 0x0000EF97
			// (set) Token: 0x06000682 RID: 1666 RVA: 0x00010D9F File Offset: 0x0000EF9F
			public Vector2 extraOffset
			{
				get
				{
					return this._extraOffset;
				}
				set
				{
					this._extraOffset = value;
				}
			}

			// Token: 0x170002E3 RID: 739
			// (get) Token: 0x06000683 RID: 1667 RVA: 0x00010DA8 File Offset: 0x0000EFA8
			// (set) Token: 0x06000684 RID: 1668 RVA: 0x00010DB0 File Offset: 0x0000EFB0
			public float xAdvanceWidthMultiplier
			{
				get
				{
					return this._xAdvanceWidthMultiplier;
				}
				set
				{
					this._xAdvanceWidthMultiplier = value;
				}
			}

			// Token: 0x170002E4 RID: 740
			// (get) Token: 0x06000685 RID: 1669 RVA: 0x00010DB9 File Offset: 0x0000EFB9
			// (set) Token: 0x06000686 RID: 1670 RVA: 0x00010DC1 File Offset: 0x0000EFC1
			public float extraXAdvance
			{
				get
				{
					return this._extraXAdvance;
				}
				set
				{
					this._extraXAdvance = value;
				}
			}

			// Token: 0x170002E5 RID: 741
			// (get) Token: 0x06000687 RID: 1671 RVA: 0x00010DCC File Offset: 0x0000EFCC
			public static UnityUITextMeshProGlyphHelper.TMProSpriteOptions Default
			{
				get
				{
					return new UnityUITextMeshProGlyphHelper.TMProSpriteOptions
					{
						scale = 1.5f,
						extraOffset = default(Vector2),
						offsetSizeMultiplier = new Vector2(0f, 0.75f),
						xAdvanceWidthMultiplier = 1f
					};
				}
			}

			// Token: 0x06000688 RID: 1672 RVA: 0x00010E20 File Offset: 0x0000F020
			public override bool Equals(object obj)
			{
				if (!(obj is UnityUITextMeshProGlyphHelper.TMProSpriteOptions))
				{
					return false;
				}
				UnityUITextMeshProGlyphHelper.TMProSpriteOptions tmproSpriteOptions = (UnityUITextMeshProGlyphHelper.TMProSpriteOptions)obj;
				return tmproSpriteOptions._scale == this._scale && tmproSpriteOptions._offsetSizeMultiplier == this._offsetSizeMultiplier && tmproSpriteOptions._extraOffset == this._extraOffset && tmproSpriteOptions._xAdvanceWidthMultiplier == this._xAdvanceWidthMultiplier && tmproSpriteOptions._extraXAdvance == this._extraXAdvance;
			}

			// Token: 0x06000689 RID: 1673 RVA: 0x00010E90 File Offset: 0x0000F090
			public override int GetHashCode()
			{
				return ((((17 * 29 + this._scale.GetHashCode()) * 29 + this._offsetSizeMultiplier.GetHashCode()) * 29 + this._extraOffset.GetHashCode()) * 29 + this._xAdvanceWidthMultiplier.GetHashCode()) * 29 + this._extraXAdvance.GetHashCode();
			}

			// Token: 0x0600068A RID: 1674 RVA: 0x00010EF8 File Offset: 0x0000F0F8
			public bool Equals(UnityUITextMeshProGlyphHelper.TMProSpriteOptions other)
			{
				return other._scale == this._scale && other._offsetSizeMultiplier == this._offsetSizeMultiplier && other._extraOffset == this._extraOffset && other._xAdvanceWidthMultiplier == this._xAdvanceWidthMultiplier && other._extraXAdvance == this._extraXAdvance;
			}

			// Token: 0x0600068B RID: 1675 RVA: 0x00010F58 File Offset: 0x0000F158
			public static bool operator ==(UnityUITextMeshProGlyphHelper.TMProSpriteOptions a, UnityUITextMeshProGlyphHelper.TMProSpriteOptions b)
			{
				return a._scale == b._scale && a._offsetSizeMultiplier == b._offsetSizeMultiplier && a._extraOffset == b._extraOffset && a._xAdvanceWidthMultiplier == b._xAdvanceWidthMultiplier && a._extraXAdvance == b._extraXAdvance;
			}

			// Token: 0x0600068C RID: 1676 RVA: 0x00010FB7 File Offset: 0x0000F1B7
			public static bool operator !=(UnityUITextMeshProGlyphHelper.TMProSpriteOptions a, UnityUITextMeshProGlyphHelper.TMProSpriteOptions b)
			{
				return !(a == b);
			}

			// Token: 0x04000363 RID: 867
			[Tooltip("Scale.")]
			[SerializeField]
			private float _scale;

			// Token: 0x04000364 RID: 868
			[Tooltip("This value will be multiplied by the Sprite width and height and applied to offset.")]
			[SerializeField]
			private Vector2 _offsetSizeMultiplier;

			// Token: 0x04000365 RID: 869
			[Tooltip("An extra offset that is cumulative with Offset Size Multiplier.")]
			[SerializeField]
			private Vector2 _extraOffset;

			// Token: 0x04000366 RID: 870
			[Tooltip("This value will be multiplied by the Sprite width applied to X Advance.")]
			[SerializeField]
			private float _xAdvanceWidthMultiplier;

			// Token: 0x04000367 RID: 871
			[Tooltip("An extra offset that is cumulative with X Advance Width Multiplier.")]
			[SerializeField]
			private float _extraXAdvance;
		}

		// Token: 0x02000081 RID: 129
		[Serializable]
		public struct SpriteMaterialProperties
		{
			// Token: 0x170002E6 RID: 742
			// (get) Token: 0x0600068D RID: 1677 RVA: 0x00010FC3 File Offset: 0x0000F1C3
			// (set) Token: 0x0600068E RID: 1678 RVA: 0x00010FCB File Offset: 0x0000F1CB
			public Color color
			{
				get
				{
					return this._color;
				}
				set
				{
					this._color = value;
				}
			}

			// Token: 0x170002E7 RID: 743
			// (get) Token: 0x0600068F RID: 1679 RVA: 0x00010FD4 File Offset: 0x0000F1D4
			public static UnityUITextMeshProGlyphHelper.SpriteMaterialProperties Default
			{
				get
				{
					return new UnityUITextMeshProGlyphHelper.SpriteMaterialProperties
					{
						_color = Color.white
					};
				}
			}

			// Token: 0x04000368 RID: 872
			[Tooltip("Sprite material color.")]
			[SerializeField]
			private Color _color;
		}

		// Token: 0x02000082 RID: 130
		private interface ITMProSprite
		{
			// Token: 0x170002E8 RID: 744
			// (get) Token: 0x06000690 RID: 1680
			// (set) Token: 0x06000691 RID: 1681
			uint id { get; set; }

			// Token: 0x170002E9 RID: 745
			// (get) Token: 0x06000692 RID: 1682
			// (set) Token: 0x06000693 RID: 1683
			float width { get; set; }

			// Token: 0x170002EA RID: 746
			// (get) Token: 0x06000694 RID: 1684
			// (set) Token: 0x06000695 RID: 1685
			float height { get; set; }

			// Token: 0x170002EB RID: 747
			// (get) Token: 0x06000696 RID: 1686
			// (set) Token: 0x06000697 RID: 1687
			float xOffset { get; set; }

			// Token: 0x170002EC RID: 748
			// (get) Token: 0x06000698 RID: 1688
			// (set) Token: 0x06000699 RID: 1689
			float yOffset { get; set; }

			// Token: 0x170002ED RID: 749
			// (get) Token: 0x0600069A RID: 1690
			// (set) Token: 0x0600069B RID: 1691
			float xAdvance { get; set; }

			// Token: 0x170002EE RID: 750
			// (get) Token: 0x0600069C RID: 1692
			// (set) Token: 0x0600069D RID: 1693
			Vector2 position { get; set; }

			// Token: 0x170002EF RID: 751
			// (get) Token: 0x0600069E RID: 1694
			// (set) Token: 0x0600069F RID: 1695
			Vector2 pivot { get; set; }

			// Token: 0x170002F0 RID: 752
			// (get) Token: 0x060006A0 RID: 1696
			// (set) Token: 0x060006A1 RID: 1697
			float scale { get; set; }

			// Token: 0x170002F1 RID: 753
			// (get) Token: 0x060006A2 RID: 1698
			// (set) Token: 0x060006A3 RID: 1699
			string name { get; set; }

			// Token: 0x170002F2 RID: 754
			// (get) Token: 0x060006A4 RID: 1700
			// (set) Token: 0x060006A5 RID: 1701
			uint unicode { get; set; }

			// Token: 0x170002F3 RID: 755
			// (get) Token: 0x060006A6 RID: 1702
			// (set) Token: 0x060006A7 RID: 1703
			int hashCode { get; set; }

			// Token: 0x170002F4 RID: 756
			// (get) Token: 0x060006A8 RID: 1704
			// (set) Token: 0x060006A9 RID: 1705
			Sprite sprite { get; set; }
		}

		// Token: 0x02000083 RID: 131
		private interface ITMProSpriteAsset
		{
			// Token: 0x170002F5 RID: 757
			// (get) Token: 0x060006AA RID: 1706
			int spriteCount { get; }

			// Token: 0x170002F6 RID: 758
			// (get) Token: 0x060006AB RID: 1707
			// (set) Token: 0x060006AC RID: 1708
			Texture spriteSheet { get; set; }

			// Token: 0x060006AD RID: 1709
			TMP_SpriteAsset GetSpriteAsset();

			// Token: 0x060006AE RID: 1710
			UnityUITextMeshProGlyphHelper.ITMProSprite GetSprite(int index);

			// Token: 0x060006AF RID: 1711
			void AddSprite(UnityUITextMeshProGlyphHelper.ITMProSprite sprite);

			// Token: 0x060006B0 RID: 1712
			bool Contains(string spriteName);

			// Token: 0x060006B1 RID: 1713
			void Clear();

			// Token: 0x060006B2 RID: 1714
			void UpdateLookupTables();

			// Token: 0x060006B3 RID: 1715
			void Destroy();
		}

		// Token: 0x02000084 RID: 132
		private static class TMProAssetVersionHelper
		{
			// Token: 0x060006B4 RID: 1716 RVA: 0x00010FF8 File Offset: 0x0000F1F8
			private static bool CheckVersionSupported()
			{
				bool flag = UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.CheckVersionSupported();
				if (UnityUITextMeshProGlyphHelper.TMProAssetVersionHelper._isVersionSupportedChecked)
				{
					return flag;
				}
				UnityUITextMeshProGlyphHelper.TMProAssetVersionHelper._isVersionSupportedChecked = true;
				return flag;
			}

			// Token: 0x060006B5 RID: 1717 RVA: 0x00011020 File Offset: 0x0000F220
			public static UnityUITextMeshProGlyphHelper.ITMProSprite CreateSprite()
			{
				if (!UnityUITextMeshProGlyphHelper.TMProAssetVersionHelper.CheckVersionSupported())
				{
					return new UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_0_0();
				}
				return new UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0();
			}

			// Token: 0x060006B6 RID: 1718 RVA: 0x00011044 File Offset: 0x0000F244
			public static UnityUITextMeshProGlyphHelper.ITMProSpriteAsset CreateSpriteAsset()
			{
				if (!UnityUITextMeshProGlyphHelper.TMProAssetVersionHelper.CheckVersionSupported())
				{
					return new UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_0_0.TMPro_SpriteAsset();
				}
				return new UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.TMPro_SpriteAsset();
			}

			// Token: 0x04000369 RID: 873
			private static bool _isVersionSupportedChecked;
		}

		// Token: 0x02000085 RID: 133
		private class TMProSprite_AssetV1_0_0 : UnityUITextMeshProGlyphHelper.ITMProSprite
		{
			// Token: 0x060006B7 RID: 1719 RVA: 0x00011067 File Offset: 0x0000F267
			public TMProSprite_AssetV1_0_0()
			{
				this.spriteInfo = new TMP_Sprite();
			}

			// Token: 0x170002F7 RID: 759
			// (get) Token: 0x060006B8 RID: 1720 RVA: 0x0001107A File Offset: 0x0000F27A
			// (set) Token: 0x060006B9 RID: 1721 RVA: 0x00011087 File Offset: 0x0000F287
			public uint id
			{
				get
				{
					return (uint)this.spriteInfo.id;
				}
				set
				{
					this.spriteInfo.id = (int)value;
				}
			}

			// Token: 0x170002F8 RID: 760
			// (get) Token: 0x060006BA RID: 1722 RVA: 0x00011095 File Offset: 0x0000F295
			// (set) Token: 0x060006BB RID: 1723 RVA: 0x000110A2 File Offset: 0x0000F2A2
			public float width
			{
				get
				{
					return this.spriteInfo.width;
				}
				set
				{
					this.spriteInfo.width = value;
				}
			}

			// Token: 0x170002F9 RID: 761
			// (get) Token: 0x060006BC RID: 1724 RVA: 0x000110B0 File Offset: 0x0000F2B0
			// (set) Token: 0x060006BD RID: 1725 RVA: 0x000110BD File Offset: 0x0000F2BD
			public float height
			{
				get
				{
					return this.spriteInfo.height;
				}
				set
				{
					this.spriteInfo.height = value;
				}
			}

			// Token: 0x170002FA RID: 762
			// (get) Token: 0x060006BE RID: 1726 RVA: 0x000110CB File Offset: 0x0000F2CB
			// (set) Token: 0x060006BF RID: 1727 RVA: 0x000110D8 File Offset: 0x0000F2D8
			public float xOffset
			{
				get
				{
					return this.spriteInfo.xOffset;
				}
				set
				{
					this.spriteInfo.xOffset = value;
				}
			}

			// Token: 0x170002FB RID: 763
			// (get) Token: 0x060006C0 RID: 1728 RVA: 0x000110E6 File Offset: 0x0000F2E6
			// (set) Token: 0x060006C1 RID: 1729 RVA: 0x000110F3 File Offset: 0x0000F2F3
			public float yOffset
			{
				get
				{
					return this.spriteInfo.yOffset;
				}
				set
				{
					this.spriteInfo.yOffset = value;
				}
			}

			// Token: 0x170002FC RID: 764
			// (get) Token: 0x060006C2 RID: 1730 RVA: 0x00011101 File Offset: 0x0000F301
			// (set) Token: 0x060006C3 RID: 1731 RVA: 0x0001110E File Offset: 0x0000F30E
			public float xAdvance
			{
				get
				{
					return this.spriteInfo.xAdvance;
				}
				set
				{
					this.spriteInfo.xAdvance = value;
				}
			}

			// Token: 0x170002FD RID: 765
			// (get) Token: 0x060006C4 RID: 1732 RVA: 0x0001111C File Offset: 0x0000F31C
			// (set) Token: 0x060006C5 RID: 1733 RVA: 0x00011139 File Offset: 0x0000F339
			public Vector2 position
			{
				get
				{
					return new Vector2(this.spriteInfo.x, this.spriteInfo.y);
				}
				set
				{
					this.spriteInfo.x = value.x;
					this.spriteInfo.y = value.y;
				}
			}

			// Token: 0x170002FE RID: 766
			// (get) Token: 0x060006C6 RID: 1734 RVA: 0x0001115D File Offset: 0x0000F35D
			// (set) Token: 0x060006C7 RID: 1735 RVA: 0x0001116A File Offset: 0x0000F36A
			public Vector2 pivot
			{
				get
				{
					return this.spriteInfo.pivot;
				}
				set
				{
					this.spriteInfo.pivot = value;
				}
			}

			// Token: 0x170002FF RID: 767
			// (get) Token: 0x060006C8 RID: 1736 RVA: 0x00011178 File Offset: 0x0000F378
			// (set) Token: 0x060006C9 RID: 1737 RVA: 0x00011185 File Offset: 0x0000F385
			public float scale
			{
				get
				{
					return this.spriteInfo.scale;
				}
				set
				{
					this.spriteInfo.scale = value;
				}
			}

			// Token: 0x17000300 RID: 768
			// (get) Token: 0x060006CA RID: 1738 RVA: 0x00011193 File Offset: 0x0000F393
			// (set) Token: 0x060006CB RID: 1739 RVA: 0x000111A0 File Offset: 0x0000F3A0
			public string name
			{
				get
				{
					return this.spriteInfo.name;
				}
				set
				{
					this.spriteInfo.name = value;
				}
			}

			// Token: 0x17000301 RID: 769
			// (get) Token: 0x060006CC RID: 1740 RVA: 0x000111AE File Offset: 0x0000F3AE
			// (set) Token: 0x060006CD RID: 1741 RVA: 0x000111BB File Offset: 0x0000F3BB
			public uint unicode
			{
				get
				{
					return (uint)this.spriteInfo.unicode;
				}
				set
				{
					this.spriteInfo.unicode = (int)value;
				}
			}

			// Token: 0x17000302 RID: 770
			// (get) Token: 0x060006CE RID: 1742 RVA: 0x000111C9 File Offset: 0x0000F3C9
			// (set) Token: 0x060006CF RID: 1743 RVA: 0x000111D6 File Offset: 0x0000F3D6
			public int hashCode
			{
				get
				{
					return this.spriteInfo.hashCode;
				}
				set
				{
					this.spriteInfo.hashCode = value;
				}
			}

			// Token: 0x17000303 RID: 771
			// (get) Token: 0x060006D0 RID: 1744 RVA: 0x000111E4 File Offset: 0x0000F3E4
			// (set) Token: 0x060006D1 RID: 1745 RVA: 0x000111F1 File Offset: 0x0000F3F1
			public Sprite sprite
			{
				get
				{
					return this.spriteInfo.sprite;
				}
				set
				{
					this.spriteInfo.sprite = value;
				}
			}

			// Token: 0x0400036A RID: 874
			public TMP_Sprite spriteInfo;

			// Token: 0x02000086 RID: 134
			public class TMPro_SpriteAsset : UnityUITextMeshProGlyphHelper.ITMProSpriteAsset
			{
				// Token: 0x17000304 RID: 772
				// (get) Token: 0x060006D2 RID: 1746 RVA: 0x000111FF File Offset: 0x0000F3FF
				public int spriteCount
				{
					get
					{
						return this._sprites.Count;
					}
				}

				// Token: 0x17000305 RID: 773
				// (get) Token: 0x060006D3 RID: 1747 RVA: 0x0001120C File Offset: 0x0000F40C
				// (set) Token: 0x060006D4 RID: 1748 RVA: 0x00011219 File Offset: 0x0000F419
				public Texture spriteSheet
				{
					get
					{
						return this._spriteAsset.spriteSheet;
					}
					set
					{
						this._spriteAsset.spriteSheet = value;
					}
				}

				// Token: 0x060006D5 RID: 1749 RVA: 0x00011228 File Offset: 0x0000F428
				public TMPro_SpriteAsset()
				{
					this._spriteAsset = ScriptableObject.CreateInstance<TMP_SpriteAsset>();
					this._spriteAsset.hideFlags = HideFlags.DontSave;
					if (this._spriteAsset.spriteInfoList == null)
					{
						this._spriteAsset.spriteInfoList = new List<TMP_Sprite>();
					}
					this._sprites = new List<UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_0_0>();
				}

				// Token: 0x060006D6 RID: 1750 RVA: 0x0001127B File Offset: 0x0000F47B
				public TMP_SpriteAsset GetSpriteAsset()
				{
					return this._spriteAsset;
				}

				// Token: 0x060006D7 RID: 1751 RVA: 0x00011283 File Offset: 0x0000F483
				public UnityUITextMeshProGlyphHelper.ITMProSprite GetSprite(int index)
				{
					if (index >= this._sprites.Count)
					{
						return null;
					}
					return this._sprites[index];
				}

				// Token: 0x060006D8 RID: 1752 RVA: 0x000112A4 File Offset: 0x0000F4A4
				public void AddSprite(UnityUITextMeshProGlyphHelper.ITMProSprite sprite)
				{
					UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_0_0 tmproSprite_AssetV1_0_ = sprite as UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_0_0;
					if (sprite == null)
					{
						throw new ArgumentException();
					}
					tmproSprite_AssetV1_0_.spriteInfo.id = this._spriteAsset.spriteInfoList.Count;
					this._spriteAsset.spriteInfoList.Add(tmproSprite_AssetV1_0_.spriteInfo);
					this._sprites.Add(tmproSprite_AssetV1_0_);
				}

				// Token: 0x060006D9 RID: 1753 RVA: 0x000112FE File Offset: 0x0000F4FE
				public void Clear()
				{
					this._spriteAsset.spriteInfoList.Clear();
					this._sprites.Clear();
				}

				// Token: 0x060006DA RID: 1754 RVA: 0x0001131C File Offset: 0x0000F51C
				public bool Contains(string spriteName)
				{
					int count = this._sprites.Count;
					for (int i = 0; i < count; i++)
					{
						if (string.Equals(this._sprites[i].name, spriteName, StringComparison.Ordinal))
						{
							return true;
						}
					}
					return false;
				}

				// Token: 0x060006DB RID: 1755 RVA: 0x0001135E File Offset: 0x0000F55E
				public void UpdateLookupTables()
				{
					this._spriteAsset.UpdateLookupTables();
				}

				// Token: 0x060006DC RID: 1756 RVA: 0x0001136B File Offset: 0x0000F56B
				public void Destroy()
				{
					if (this._spriteAsset == null)
					{
						return;
					}
					global::UnityEngine.Object.Destroy(this._spriteAsset);
					this._spriteAsset = null;
				}

				// Token: 0x0400036B RID: 875
				private TMP_SpriteAsset _spriteAsset;

				// Token: 0x0400036C RID: 876
				private readonly List<UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_0_0> _sprites;
			}
		}

		// Token: 0x02000087 RID: 135
		private class TMProSprite_AssetV1_1_0 : UnityUITextMeshProGlyphHelper.ITMProSprite
		{
			// Token: 0x060006DD RID: 1757 RVA: 0x0001138E File Offset: 0x0000F58E
			public TMProSprite_AssetV1_1_0()
			{
				this._spriteGlyph = new UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.TMPro_SpriteGlyph();
				this._spriteCharacter = new UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.TMPro_SpriteCharacter();
				this._spriteCharacter.glyph = this._spriteGlyph.source;
			}

			// Token: 0x17000306 RID: 774
			// (get) Token: 0x060006DE RID: 1758 RVA: 0x000113C2 File Offset: 0x0000F5C2
			public UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.TMPro_SpriteGlyph spriteGlyph
			{
				get
				{
					return this._spriteGlyph;
				}
			}

			// Token: 0x17000307 RID: 775
			// (get) Token: 0x060006DF RID: 1759 RVA: 0x000113CA File Offset: 0x0000F5CA
			public UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.TMPro_SpriteCharacter spriteCharacter
			{
				get
				{
					return this._spriteCharacter;
				}
			}

			// Token: 0x17000308 RID: 776
			// (get) Token: 0x060006E0 RID: 1760 RVA: 0x000113D2 File Offset: 0x0000F5D2
			// (set) Token: 0x060006E1 RID: 1761 RVA: 0x000113E4 File Offset: 0x0000F5E4
			public uint id
			{
				get
				{
					return this._spriteGlyph.source.index;
				}
				set
				{
					this._spriteGlyph.source.index = value;
					this._spriteCharacter.glyphIndex = value;
				}
			}

			// Token: 0x17000309 RID: 777
			// (get) Token: 0x060006E2 RID: 1762 RVA: 0x00011404 File Offset: 0x0000F604
			// (set) Token: 0x060006E3 RID: 1763 RVA: 0x0001142C File Offset: 0x0000F62C
			public float width
			{
				get
				{
					return this._spriteGlyph.source.metrics.width;
				}
				set
				{
					GlyphMetrics metrics = this._spriteGlyph.source.metrics;
					metrics.width = value;
					this._spriteGlyph.source.metrics = metrics;
					GlyphRect glyphRect = this._spriteGlyph.source.glyphRect;
					glyphRect.width = (int)value;
					this._spriteGlyph.source.glyphRect = glyphRect;
				}
			}

			// Token: 0x1700030A RID: 778
			// (get) Token: 0x060006E4 RID: 1764 RVA: 0x00011490 File Offset: 0x0000F690
			// (set) Token: 0x060006E5 RID: 1765 RVA: 0x000114B8 File Offset: 0x0000F6B8
			public float height
			{
				get
				{
					return this._spriteGlyph.source.metrics.height;
				}
				set
				{
					GlyphMetrics metrics = this._spriteGlyph.source.metrics;
					metrics.height = value;
					this._spriteGlyph.source.metrics = metrics;
					GlyphRect glyphRect = this._spriteGlyph.source.glyphRect;
					glyphRect.height = (int)value;
					this._spriteGlyph.source.glyphRect = glyphRect;
				}
			}

			// Token: 0x1700030B RID: 779
			// (get) Token: 0x060006E6 RID: 1766 RVA: 0x0001151C File Offset: 0x0000F71C
			// (set) Token: 0x060006E7 RID: 1767 RVA: 0x00011544 File Offset: 0x0000F744
			public float xOffset
			{
				get
				{
					return this._spriteGlyph.source.metrics.horizontalBearingX;
				}
				set
				{
					GlyphMetrics metrics = this._spriteGlyph.source.metrics;
					metrics.horizontalBearingX = value;
					this._spriteGlyph.source.metrics = metrics;
				}
			}

			// Token: 0x1700030C RID: 780
			// (get) Token: 0x060006E8 RID: 1768 RVA: 0x0001157C File Offset: 0x0000F77C
			// (set) Token: 0x060006E9 RID: 1769 RVA: 0x000115A4 File Offset: 0x0000F7A4
			public float yOffset
			{
				get
				{
					return this._spriteGlyph.source.metrics.horizontalBearingY;
				}
				set
				{
					GlyphMetrics metrics = this._spriteGlyph.source.metrics;
					metrics.horizontalBearingY = value;
					this._spriteGlyph.source.metrics = metrics;
				}
			}

			// Token: 0x1700030D RID: 781
			// (get) Token: 0x060006EA RID: 1770 RVA: 0x000115DC File Offset: 0x0000F7DC
			// (set) Token: 0x060006EB RID: 1771 RVA: 0x00011604 File Offset: 0x0000F804
			public float xAdvance
			{
				get
				{
					return this._spriteGlyph.source.metrics.horizontalAdvance;
				}
				set
				{
					GlyphMetrics metrics = this._spriteGlyph.source.metrics;
					metrics.horizontalAdvance = value;
					this._spriteGlyph.source.metrics = metrics;
				}
			}

			// Token: 0x1700030E RID: 782
			// (get) Token: 0x060006EC RID: 1772 RVA: 0x0001163C File Offset: 0x0000F83C
			// (set) Token: 0x060006ED RID: 1773 RVA: 0x00011670 File Offset: 0x0000F870
			public Vector2 position
			{
				get
				{
					GlyphRect glyphRect = this._spriteGlyph.source.glyphRect;
					return new Vector2((float)glyphRect.x, (float)glyphRect.y);
				}
				set
				{
					GlyphRect glyphRect = this._spriteGlyph.source.glyphRect;
					glyphRect.x = (int)value.x;
					glyphRect.y = (int)value.y;
					this._spriteGlyph.source.glyphRect = glyphRect;
				}
			}

			// Token: 0x1700030F RID: 783
			// (get) Token: 0x060006EE RID: 1774 RVA: 0x000116BC File Offset: 0x0000F8BC
			// (set) Token: 0x060006EF RID: 1775 RVA: 0x00003466 File Offset: 0x00001666
			public Vector2 pivot
			{
				get
				{
					return default(Vector2);
				}
				set
				{
				}
			}

			// Token: 0x17000310 RID: 784
			// (get) Token: 0x060006F0 RID: 1776 RVA: 0x000116D2 File Offset: 0x0000F8D2
			// (set) Token: 0x060006F1 RID: 1777 RVA: 0x000116DF File Offset: 0x0000F8DF
			public float scale
			{
				get
				{
					return this._spriteCharacter.scale;
				}
				set
				{
					this._spriteCharacter.scale = value;
				}
			}

			// Token: 0x17000311 RID: 785
			// (get) Token: 0x060006F2 RID: 1778 RVA: 0x000116ED File Offset: 0x0000F8ED
			// (set) Token: 0x060006F3 RID: 1779 RVA: 0x000116FA File Offset: 0x0000F8FA
			public string name
			{
				get
				{
					return this._spriteCharacter.name;
				}
				set
				{
					this._spriteCharacter.name = value;
				}
			}

			// Token: 0x17000312 RID: 786
			// (get) Token: 0x060006F4 RID: 1780 RVA: 0x00011708 File Offset: 0x0000F908
			// (set) Token: 0x060006F5 RID: 1781 RVA: 0x00011715 File Offset: 0x0000F915
			public uint unicode
			{
				get
				{
					return this._spriteCharacter.unicode;
				}
				set
				{
					this._spriteCharacter.unicode = value;
				}
			}

			// Token: 0x17000313 RID: 787
			// (get) Token: 0x060006F6 RID: 1782 RVA: 0x00004C86 File Offset: 0x00002E86
			// (set) Token: 0x060006F7 RID: 1783 RVA: 0x00003466 File Offset: 0x00001666
			public int hashCode
			{
				get
				{
					return 0;
				}
				set
				{
				}
			}

			// Token: 0x17000314 RID: 788
			// (get) Token: 0x060006F8 RID: 1784 RVA: 0x00011723 File Offset: 0x0000F923
			// (set) Token: 0x060006F9 RID: 1785 RVA: 0x00011730 File Offset: 0x0000F930
			public Sprite sprite
			{
				get
				{
					return this._spriteGlyph.sprite;
				}
				set
				{
					this._spriteGlyph.sprite = value;
				}
			}

			// Token: 0x060006FA RID: 1786 RVA: 0x00011740 File Offset: 0x0000F940
			public static bool CheckVersionSupported()
			{
				if (UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.s_isVersionSupported != null)
				{
					return UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.s_isVersionSupported.Value;
				}
				try
				{
					new UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.TMPro_SpriteAsset();
					UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.s_isVersionSupported = new bool?(true);
				}
				catch
				{
					UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.s_isVersionSupported = new bool?(false);
				}
				return UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.s_isVersionSupported.Value;
			}

			// Token: 0x0400036D RID: 877
			private readonly UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.TMPro_SpriteGlyph _spriteGlyph;

			// Token: 0x0400036E RID: 878
			private readonly UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.TMPro_SpriteCharacter _spriteCharacter;

			// Token: 0x0400036F RID: 879
			private static bool? s_isVersionSupported;

			// Token: 0x02000088 RID: 136
			public class TMPro_SpriteCharacter
			{
				// Token: 0x17000315 RID: 789
				// (get) Token: 0x060006FB RID: 1787 RVA: 0x000117A0 File Offset: 0x0000F9A0
				public TMP_SpriteCharacter source
				{
					get
					{
						return this._source;
					}
				}

				// Token: 0x17000316 RID: 790
				// (get) Token: 0x060006FC RID: 1788 RVA: 0x000117A8 File Offset: 0x0000F9A8
				// (set) Token: 0x060006FD RID: 1789 RVA: 0x000117B5 File Offset: 0x0000F9B5
				public Glyph glyph
				{
					get
					{
						return this._source.glyph;
					}
					set
					{
						this._source.glyph = value;
					}
				}

				// Token: 0x17000317 RID: 791
				// (get) Token: 0x060006FE RID: 1790 RVA: 0x000117C3 File Offset: 0x0000F9C3
				// (set) Token: 0x060006FF RID: 1791 RVA: 0x000117D0 File Offset: 0x0000F9D0
				public uint unicode
				{
					get
					{
						return this._source.unicode;
					}
					set
					{
						if (value == 0U)
						{
							value = 65534U;
						}
						this._source.unicode = value;
					}
				}

				// Token: 0x17000318 RID: 792
				// (get) Token: 0x06000700 RID: 1792 RVA: 0x000117E8 File Offset: 0x0000F9E8
				// (set) Token: 0x06000701 RID: 1793 RVA: 0x000117F5 File Offset: 0x0000F9F5
				public string name
				{
					get
					{
						return this._source.name;
					}
					set
					{
						this._source.name = value;
					}
				}

				// Token: 0x17000319 RID: 793
				// (get) Token: 0x06000702 RID: 1794 RVA: 0x00011803 File Offset: 0x0000FA03
				// (set) Token: 0x06000703 RID: 1795 RVA: 0x00011810 File Offset: 0x0000FA10
				public float scale
				{
					get
					{
						return this._source.scale;
					}
					set
					{
						this._source.scale = value;
					}
				}

				// Token: 0x1700031A RID: 794
				// (get) Token: 0x06000704 RID: 1796 RVA: 0x0001181E File Offset: 0x0000FA1E
				// (set) Token: 0x06000705 RID: 1797 RVA: 0x0001182B File Offset: 0x0000FA2B
				public uint glyphIndex
				{
					get
					{
						return this._source.glyphIndex;
					}
					set
					{
						this._source.glyphIndex = value;
					}
				}

				// Token: 0x06000706 RID: 1798 RVA: 0x00011839 File Offset: 0x0000FA39
				public TMPro_SpriteCharacter()
				{
					this._source = new TMP_SpriteCharacter();
				}

				// Token: 0x04000370 RID: 880
				private readonly TMP_SpriteCharacter _source;
			}

			// Token: 0x02000089 RID: 137
			public class TMPro_SpriteGlyph
			{
				// Token: 0x1700031B RID: 795
				// (get) Token: 0x06000707 RID: 1799 RVA: 0x0001184C File Offset: 0x0000FA4C
				public TMP_SpriteGlyph source
				{
					get
					{
						return this._source;
					}
				}

				// Token: 0x1700031C RID: 796
				// (get) Token: 0x06000708 RID: 1800 RVA: 0x00011854 File Offset: 0x0000FA54
				// (set) Token: 0x06000709 RID: 1801 RVA: 0x00011861 File Offset: 0x0000FA61
				public Sprite sprite
				{
					get
					{
						return this._source.sprite;
					}
					set
					{
						this._source.sprite = value;
					}
				}

				// Token: 0x0600070A RID: 1802 RVA: 0x0001186F File Offset: 0x0000FA6F
				public TMPro_SpriteGlyph()
				{
					this._source = new TMP_SpriteGlyph();
					UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0.TMPro_SpriteGlyph.Initialize(this._source);
				}

				// Token: 0x0600070B RID: 1803 RVA: 0x0001188D File Offset: 0x0000FA8D
				private static void Initialize(Glyph glyph)
				{
					glyph.scale = 1f;
					glyph.atlasIndex = 0;
				}

				// Token: 0x04000371 RID: 881
				private readonly TMP_SpriteGlyph _source;
			}

			// Token: 0x0200008A RID: 138
			public class TMPro_SpriteAsset : UnityUITextMeshProGlyphHelper.ITMProSpriteAsset
			{
				// Token: 0x1700031D RID: 797
				// (get) Token: 0x0600070C RID: 1804 RVA: 0x000118A1 File Offset: 0x0000FAA1
				public int spriteCount
				{
					get
					{
						return this._sprites.Count;
					}
				}

				// Token: 0x1700031E RID: 798
				// (get) Token: 0x0600070D RID: 1805 RVA: 0x000118AE File Offset: 0x0000FAAE
				// (set) Token: 0x0600070E RID: 1806 RVA: 0x000118BB File Offset: 0x0000FABB
				public Texture spriteSheet
				{
					get
					{
						return this._spriteAsset.spriteSheet;
					}
					set
					{
						this._spriteAsset.spriteSheet = value;
					}
				}

				// Token: 0x0600070F RID: 1807 RVA: 0x000118CC File Offset: 0x0000FACC
				public TMPro_SpriteAsset()
				{
					this._spriteAsset = ScriptableObject.CreateInstance<TMP_SpriteAsset>();
					this._spriteAsset.hideFlags = HideFlags.DontSave;
					Type typeFromHandle = typeof(TMP_SpriteAsset);
					if (typeFromHandle == null)
					{
						throw new ArgumentNullException("type");
					}
					PropertyInfo property = typeFromHandle.GetProperty("version", BindingFlags.Instance | BindingFlags.Public);
					if (property == null)
					{
						throw new ArgumentNullException("version");
					}
					property.SetValue(this._spriteAsset, "1.1.0");
					this._spriteCharacterTable = typeFromHandle.GetProperty("spriteCharacterTable", BindingFlags.Instance | BindingFlags.Public);
					if (this._spriteCharacterTable == null)
					{
						throw new ArgumentNullException("spriteCharacterTable");
					}
					this._spriteCharacterTableList = (IList)this._spriteCharacterTable.GetValue(this._spriteAsset);
					if (this._spriteCharacterTableList == null)
					{
						throw new ArgumentNullException("spriteCharacterTableList");
					}
					this._spriteGlyphTable = typeFromHandle.GetProperty("spriteGlyphTable", BindingFlags.Instance | BindingFlags.Public);
					if (this._spriteGlyphTable == null)
					{
						throw new ArgumentNullException("spriteGlyphTable");
					}
					this._spriteGlyphTableList = (IList)this._spriteGlyphTable.GetValue(this._spriteAsset);
					if (this._spriteGlyphTableList == null)
					{
						throw new ArgumentNullException("spriteGlyphTableList");
					}
					this._sprites = new List<UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0>();
				}

				// Token: 0x06000710 RID: 1808 RVA: 0x00011A08 File Offset: 0x0000FC08
				public TMP_SpriteAsset GetSpriteAsset()
				{
					return this._spriteAsset;
				}

				// Token: 0x06000711 RID: 1809 RVA: 0x00011A10 File Offset: 0x0000FC10
				public UnityUITextMeshProGlyphHelper.ITMProSprite GetSprite(int index)
				{
					if (index >= this._sprites.Count)
					{
						return null;
					}
					return this._sprites[index];
				}

				// Token: 0x06000712 RID: 1810 RVA: 0x00011A30 File Offset: 0x0000FC30
				public void AddSprite(UnityUITextMeshProGlyphHelper.ITMProSprite sprite)
				{
					UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0 tmproSprite_AssetV1_1_ = sprite as UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0;
					if (tmproSprite_AssetV1_1_ == null)
					{
						throw new ArgumentException();
					}
					tmproSprite_AssetV1_1_.id = (uint)this._spriteCharacterTableList.Count;
					this._spriteCharacterTableList.Add(tmproSprite_AssetV1_1_.spriteCharacter.source);
					this._spriteGlyphTableList.Add(tmproSprite_AssetV1_1_.spriteGlyph.source);
					this._sprites.Add(tmproSprite_AssetV1_1_);
				}

				// Token: 0x06000713 RID: 1811 RVA: 0x00011A98 File Offset: 0x0000FC98
				public void Clear()
				{
					this._spriteCharacterTableList.Clear();
					this._spriteGlyphTableList.Clear();
					this._sprites.Clear();
				}

				// Token: 0x06000714 RID: 1812 RVA: 0x00011ABC File Offset: 0x0000FCBC
				public bool Contains(string spriteName)
				{
					int count = this._sprites.Count;
					for (int i = 0; i < count; i++)
					{
						if (string.Equals(this._sprites[i].name, spriteName, StringComparison.Ordinal))
						{
							return true;
						}
					}
					return false;
				}

				// Token: 0x06000715 RID: 1813 RVA: 0x00011AFE File Offset: 0x0000FCFE
				public void UpdateLookupTables()
				{
					this._spriteAsset.UpdateLookupTables();
				}

				// Token: 0x06000716 RID: 1814 RVA: 0x00011B0B File Offset: 0x0000FD0B
				public void Destroy()
				{
					if (this._spriteAsset == null)
					{
						return;
					}
					global::UnityEngine.Object.Destroy(this._spriteAsset);
					this._spriteAsset = null;
				}

				// Token: 0x04000372 RID: 882
				private readonly PropertyInfo _spriteCharacterTable;

				// Token: 0x04000373 RID: 883
				private readonly PropertyInfo _spriteGlyphTable;

				// Token: 0x04000374 RID: 884
				private readonly IList _spriteCharacterTableList;

				// Token: 0x04000375 RID: 885
				private readonly IList _spriteGlyphTableList;

				// Token: 0x04000376 RID: 886
				private readonly List<UnityUITextMeshProGlyphHelper.TMProSprite_AssetV1_1_0> _sprites;

				// Token: 0x04000377 RID: 887
				private TMP_SpriteAsset _spriteAsset;
			}
		}

		// Token: 0x0200008B RID: 139
		private enum DisplayType
		{
			// Token: 0x04000379 RID: 889
			Glyph,
			// Token: 0x0400037A RID: 890
			Text,
			// Token: 0x0400037B RID: 891
			GlyphOrText
		}
	}
}
