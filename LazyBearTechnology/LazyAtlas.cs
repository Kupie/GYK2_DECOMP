using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x0200011B RID: 283
	[CreateAssetMenu(fileName = "New Lazy Atlas", menuName = "Lazy/Lazy Atlas", order = 1)]
	public class LazyAtlas : ScriptableObject
	{
		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060005C3 RID: 1475 RVA: 0x0001D3C0 File Offset: 0x0001B5C0
		public List<LazyAtlasItem> Items
		{
			get
			{
				return this.items;
			}
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x0001D3C8 File Offset: 0x0001B5C8
		public LazyAtlasItem GetItemByName(string name)
		{
			foreach (LazyAtlasItem lazyAtlasItem in this.items)
			{
				if (lazyAtlasItem.name == name)
				{
					return lazyAtlasItem;
				}
			}
			return null;
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x0001D42C File Offset: 0x0001B62C
		public LazyAtlasItem GetItemByIndex(int index)
		{
			return this.items[index];
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0001D43C File Offset: 0x0001B63C
		public int GetItemIndexByName(string name)
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i].name == name)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0001D47C File Offset: 0x0001B67C
		public LazyAtlasItem GetItemByItemNameHash(int itemNameHash)
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i].name.GetHashCode() == itemNameHash)
				{
					return this.items[i];
				}
			}
			return null;
		}

		// Token: 0x040002AC RID: 684
		[Tooltip("List of images and folders included in the atlas.")]
		public List<global::UnityEngine.Object> objects = new List<global::UnityEngine.Object>();

		// Token: 0x040002AD RID: 685
		[Tooltip("Common objects that could be included into several atlases.")]
		public LazyAtlasCommonSprites commonObjects;

		// Token: 0x040002AE RID: 686
		public Texture2D texture;

		// Token: 0x040002AF RID: 687
		public TextureFormat textureFormat = TextureFormat.RGBA32;

		// Token: 0x040002B0 RID: 688
		public FilterMode filterMode;

		// Token: 0x040002B1 RID: 689
		public bool exportToTextMeshPro;

		// Token: 0x040002B2 RID: 690
		public TMP_SpriteAsset tmpSpriteAtlas;

		// Token: 0x040002B3 RID: 691
		public bool autoScaleNewElements = true;

		// Token: 0x040002B4 RID: 692
		public bool autoScaleOldElements = true;

		// Token: 0x040002B5 RID: 693
		[SerializeField]
		private List<LazyAtlasItemParams> customItemParams = new List<LazyAtlasItemParams>();

		// Token: 0x040002B6 RID: 694
		public int edgeBleed;

		// Token: 0x040002B7 RID: 695
		public bool fillFromTop;

		// Token: 0x040002B8 RID: 696
		[SerializeField]
		private List<LazyAtlasItem> items = new List<LazyAtlasItem>();
	}
}
