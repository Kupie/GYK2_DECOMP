using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006C6 RID: 1734
[Serializable]
public class ListDictionary<T> : ISerializationCallbackReceiver where T : IEquatable<T>
{
	// Token: 0x17000727 RID: 1831
	// (get) Token: 0x06002DEB RID: 11755 RVA: 0x000DBC1B File Offset: 0x000D9E1B
	public int Count
	{
		get
		{
			return this.list.Count;
		}
	}

	// Token: 0x17000728 RID: 1832
	public T this[int index]
	{
		get
		{
			return this.list[index];
		}
		set
		{
			this.list[index] = value;
			this.dictionary[value] = index;
		}
	}

	// Token: 0x06002DEE RID: 11758 RVA: 0x000DBC52 File Offset: 0x000D9E52
	public bool Contains(T item)
	{
		return this.dictionary.ContainsKey(item);
	}

	// Token: 0x06002DEF RID: 11759 RVA: 0x000DBC60 File Offset: 0x000D9E60
	public int IndexOf(T item)
	{
		if (!this.dictionary.ContainsKey(item))
		{
			return -1;
		}
		return this.dictionary[item];
	}

	// Token: 0x06002DF0 RID: 11760 RVA: 0x000DBC80 File Offset: 0x000D9E80
	public bool TryAdd(T item)
	{
		if (this.dictionary.ContainsKey(item))
		{
			return false;
		}
		int count = this.list.Count;
		this.list.Add(item);
		this.dictionary[item] = count;
		return true;
	}

	// Token: 0x06002DF1 RID: 11761 RVA: 0x000DBCC4 File Offset: 0x000D9EC4
	public bool Remove(T item)
	{
		if (!this.dictionary.ContainsKey(item))
		{
			return false;
		}
		int num = this.dictionary[item];
		this.dictionary.Remove(item);
		this.list.RemoveAt(num);
		return true;
	}

	// Token: 0x06002DF2 RID: 11762 RVA: 0x000DBD08 File Offset: 0x000D9F08
	public void RemoveAt(int index)
	{
		T t = this.list[index];
		this.list.RemoveAt(index);
		this.dictionary.Remove(t);
	}

	// Token: 0x06002DF3 RID: 11763 RVA: 0x000DBD3B File Offset: 0x000D9F3B
	public void Clear()
	{
		this.list.Clear();
		this.dictionary.Clear();
	}

	// Token: 0x06002DF4 RID: 11764 RVA: 0x000DBD53 File Offset: 0x000D9F53
	public IEnumerable<T> IterateByList()
	{
		return this.list;
	}

	// Token: 0x06002DF5 RID: 11765 RVA: 0x000DBD5B File Offset: 0x000D9F5B
	public IEnumerable<KeyValuePair<T, int>> IterateByDictionary()
	{
		return this.dictionary;
	}

	// Token: 0x06002DF6 RID: 11766 RVA: 0x000DBD63 File Offset: 0x000D9F63
	public override string ToString()
	{
		return string.Format("ListDictionary<{0}>({1}): {2}", typeof(T), this.list.Count, string.Join<T>(",", this.list));
	}

	// Token: 0x06002DF7 RID: 11767 RVA: 0x00002318 File Offset: 0x00000518
	public void OnBeforeSerialize()
	{
	}

	// Token: 0x06002DF8 RID: 11768 RVA: 0x000DBD9C File Offset: 0x000D9F9C
	public void OnAfterDeserialize()
	{
		this.dictionary = new Dictionary<T, int>();
		for (int i = 0; i < this.list.Count; i++)
		{
			this.dictionary[this.list[i]] = i;
		}
	}

	// Token: 0x06002DF9 RID: 11769 RVA: 0x000DBDE2 File Offset: 0x000D9FE2
	public T[] ToArray()
	{
		return this.list.ToArray();
	}

	// Token: 0x04002506 RID: 9478
	[SerializeField]
	private List<T> list = new List<T>();

	// Token: 0x04002507 RID: 9479
	private Dictionary<T, int> dictionary = new Dictionary<T, int>();
}
