using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.Glyphs.UnityUI
{
	// Token: 0x0200006E RID: 110
	public abstract class UnityUIControllerElementGlyphBase : ControllerElementGlyphBase
	{
		// Token: 0x060005E0 RID: 1504 RVA: 0x0000E310 File Offset: 0x0000C510
		protected override GameObject GetDefaultGlyphOrTextPrefab()
		{
			return UnityUIControllerElementGlyphBase.defaultGlyphOrTextPrefab;
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x060005E1 RID: 1505 RVA: 0x0000E31F File Offset: 0x0000C51F
		// (set) Token: 0x060005E2 RID: 1506 RVA: 0x0000E33F File Offset: 0x0000C53F
		public static GameObject defaultGlyphOrTextPrefab
		{
			get
			{
				if (!(UnityUIControllerElementGlyphBase.s_defaultGlyphOrTextPrefab != null))
				{
					return UnityUIControllerElementGlyphBase.s_defaultGlyphOrTextPrefab = UnityUIControllerElementGlyphBase.CreateDefaultGlyphOrTextPrefab();
				}
				return UnityUIControllerElementGlyphBase.s_defaultGlyphOrTextPrefab;
			}
			set
			{
				UnityUIControllerElementGlyphBase.s_defaultGlyphOrTextPrefab = value;
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x060005E3 RID: 1507 RVA: 0x0000E347 File Offset: 0x0000C547
		// (set) Token: 0x060005E4 RID: 1508 RVA: 0x0000E34E File Offset: 0x0000C54E
		public static Func<GameObject> defaultGlyphOrTextPrefabProvider
		{
			get
			{
				return UnityUIControllerElementGlyphBase.s_defaultGlyphOrTextPrefabProvider;
			}
			set
			{
				UnityUIControllerElementGlyphBase.s_defaultGlyphOrTextPrefabProvider = value;
			}
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0000E358 File Offset: 0x0000C558
		private static GameObject CreateDefaultGlyphOrTextPrefab()
		{
			if (UnityUIControllerElementGlyphBase.s_defaultGlyphOrTextPrefabProvider != null)
			{
				return UnityUIControllerElementGlyphBase.s_defaultGlyphOrTextPrefabProvider();
			}
			GameObject gameObject = new GameObject("Glyph or text prefab");
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			global::UnityEngine.Object.DontDestroyOnLoad(gameObject);
			UnityUIGlyphOrText unityUIGlyphOrText = gameObject.AddComponent<UnityUIGlyphOrText>();
			VerticalLayoutGroup verticalLayoutGroup = gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.childControlHeight = true;
			verticalLayoutGroup.childControlWidth = true;
			verticalLayoutGroup.childForceExpandHeight = true;
			verticalLayoutGroup.childForceExpandWidth = true;
			GameObject gameObject2 = new GameObject("Glyph");
			gameObject2.hideFlags = HideFlags.HideAndDontSave;
			gameObject2.transform.SetParent(gameObject.transform, false);
			Image image = gameObject2.AddComponent<Image>();
			image.preserveAspect = true;
			unityUIGlyphOrText.glyphComponent = image;
			GameObject gameObject3 = new GameObject("Text");
			gameObject3.hideFlags = HideFlags.HideAndDontSave;
			gameObject3.transform.SetParent(gameObject.transform, false);
			Text text = gameObject3.AddComponent<Text>();
			text.alignment = TextAnchor.MiddleCenter;
			text.fontSize = 32;
			text.resizeTextForBestFit = true;
			text.resizeTextMinSize = 10;
			text.resizeTextMaxSize = 32;
			text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
			text.raycastTarget = false;
			unityUIGlyphOrText.textComponent = text;
			return gameObject;
		}

		// Token: 0x0400031B RID: 795
		private static GameObject s_defaultGlyphOrTextPrefab;

		// Token: 0x0400031C RID: 796
		private static Func<GameObject> s_defaultGlyphOrTextPrefabProvider;
	}
}
