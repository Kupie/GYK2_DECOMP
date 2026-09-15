using System;
using UnityEngine;

namespace SoftMasking.Samples
{
	// Token: 0x02000008 RID: 8
	public class ItemsGenerator : MonoBehaviour
	{
		// Token: 0x06000011 RID: 17 RVA: 0x00002300 File Offset: 0x00000500
		public void Generate()
		{
			this.DestroyChildren();
			int num = global::UnityEngine.Random.Range(0, ItemsGenerator.colors.Length - 1);
			for (int i = 0; i < this.count; i++)
			{
				Item item = global::UnityEngine.Object.Instantiate<Item>(this.itemPrefab);
				item.transform.SetParent(this.target, false);
				item.Set(string.Format("{0} {1:D2}", this.baseName, i + 1), this.image, ItemsGenerator.colors[(num + i) % ItemsGenerator.colors.Length], global::UnityEngine.Random.Range(0.4f, 1f), global::UnityEngine.Random.Range(0.4f, 1f));
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000023A7 File Offset: 0x000005A7
		private void DestroyChildren()
		{
			while (this.target.childCount > 0)
			{
				global::UnityEngine.Object.DestroyImmediate(this.target.GetChild(0).gameObject);
			}
		}

		// Token: 0x04000012 RID: 18
		public RectTransform target;

		// Token: 0x04000013 RID: 19
		public Sprite image;

		// Token: 0x04000014 RID: 20
		public int count;

		// Token: 0x04000015 RID: 21
		public string baseName;

		// Token: 0x04000016 RID: 22
		public Item itemPrefab;

		// Token: 0x04000017 RID: 23
		private static readonly Color[] colors = new Color[]
		{
			Color.red,
			Color.green,
			Color.blue,
			Color.cyan,
			Color.yellow,
			Color.magenta,
			Color.gray
		};
	}
}
