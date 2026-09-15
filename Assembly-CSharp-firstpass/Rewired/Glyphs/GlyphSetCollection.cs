using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Glyphs
{
	// Token: 0x02000068 RID: 104
	[Serializable]
	public class GlyphSetCollection : ScriptableObject
	{
		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x060005BD RID: 1469 RVA: 0x0000DA3A File Offset: 0x0000BC3A
		// (set) Token: 0x060005BE RID: 1470 RVA: 0x0000DA42 File Offset: 0x0000BC42
		public List<GlyphSet> sets
		{
			get
			{
				return this._sets;
			}
			set
			{
				this._sets = value;
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x060005BF RID: 1471 RVA: 0x0000DA4B File Offset: 0x0000BC4B
		// (set) Token: 0x060005C0 RID: 1472 RVA: 0x0000DA53 File Offset: 0x0000BC53
		public List<GlyphSetCollection> collections
		{
			get
			{
				return this._collections;
			}
			set
			{
				if (value != null && value.Contains(this))
				{
					GlyphSetCollection.LogCircularDependency();
					Debug.LogWarning("Rewired: Set collections aborted due to circular dependency.");
					return;
				}
				this._collections = value;
			}
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0000DA78 File Offset: 0x0000BC78
		public virtual IEnumerable<GlyphSet> IterateSetsRecursively()
		{
			return this.IterateSetsRecursively(new List<GlyphSetCollection> { this });
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x0000DA8C File Offset: 0x0000BC8C
		protected virtual IEnumerable<GlyphSet> IterateSetsRecursively(List<GlyphSetCollection> processedCollections)
		{
			if (processedCollections == null)
			{
				throw new ArgumentNullException("processedCollections");
			}
			if (this._sets != null)
			{
				int setCount = this._sets.Count;
				int num;
				for (int i = 0; i < setCount; i = num + 1)
				{
					if (!(this._sets[i] == null))
					{
						yield return this.sets[i];
					}
					num = i;
				}
			}
			if (this._collections != null)
			{
				int collectionCount = this._collections.Count;
				int num;
				for (int i = 0; i < collectionCount; i = num + 1)
				{
					if (!(this._collections[i] == null))
					{
						if (processedCollections.Contains(this._collections[i]))
						{
							GlyphSetCollection.LogCircularDependency();
						}
						else
						{
							processedCollections.Add(this._collections[i]);
							foreach (GlyphSet glyphSet in this._collections[i].IterateSetsRecursively(processedCollections))
							{
								yield return glyphSet;
							}
							IEnumerator<GlyphSet> enumerator = null;
						}
					}
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x0000DAA3 File Offset: 0x0000BCA3
		private static void LogCircularDependency()
		{
			Debug.LogError("Rewired: Circular dependency detected. This collection is referenced in a child collection. This is not allowed.");
		}

		// Token: 0x0400030E RID: 782
		[Tooltip("The list of glyph sets.")]
		[SerializeField]
		private List<GlyphSet> _sets;

		// Token: 0x0400030F RID: 783
		[Tooltip("The list of glyph set collections.")]
		[SerializeField]
		private List<GlyphSetCollection> _collections;
	}
}
