using System;
using System.Collections.Generic;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x020000FE RID: 254
	[Serializable]
	public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
	{
		// Token: 0x06000491 RID: 1169 RVA: 0x0001805C File Offset: 0x0001625C
		public void OnBeforeSerialize()
		{
			this.serKeys.Clear();
			this.serValues.Clear();
			foreach (KeyValuePair<TKey, TValue> keyValuePair in this)
			{
				this.serKeys.Add(keyValuePair.Key);
				this.serValues.Add(keyValuePair.Value);
			}
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x000180E0 File Offset: 0x000162E0
		public void OnAfterDeserialize()
		{
			base.Clear();
			for (int i = 0; i < this.serKeys.Count; i++)
			{
				base.Add(this.serKeys[i], this.serValues[i]);
			}
		}

		// Token: 0x0400023A RID: 570
		[SerializeField]
		private List<TKey> serKeys = new List<TKey>();

		// Token: 0x0400023B RID: 571
		[SerializeField]
		private List<TValue> serValues = new List<TValue>();
	}
}
