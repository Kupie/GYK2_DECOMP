using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x02000022 RID: 34
public static class ListExtensions
{
	// Token: 0x06000097 RID: 151 RVA: 0x00004590 File Offset: 0x00002790
	public static T GetRandom<T>(this List<T> list)
	{
		int num = global::UnityEngine.Random.Range(0, list.Count);
		return list[num];
	}

	// Token: 0x06000098 RID: 152 RVA: 0x000045B1 File Offset: 0x000027B1
	public static T PopFirst<T>(this List<T> list)
	{
		T t = list[0];
		list.RemoveAt(0);
		return t;
	}

	// Token: 0x06000099 RID: 153 RVA: 0x000045C1 File Offset: 0x000027C1
	public static T PopLast<T>(this List<T> list)
	{
		T t = list[list.Count - 1];
		list.RemoveAt(list.Count - 1);
		return t;
	}

	// Token: 0x0600009A RID: 154 RVA: 0x000045E0 File Offset: 0x000027E0
	public static T PopRandom<T>(this List<T> list)
	{
		int num = global::UnityEngine.Random.Range(0, list.Count);
		T t = list[num];
		list.Remove(t);
		return t;
	}

	// Token: 0x0600009B RID: 155 RVA: 0x0000460C File Offset: 0x0000280C
	public static void RemoveUnityNulls<T>(this List<T> list) where T : global::UnityEngine.Object
	{
		for (int i = 0; i < list.Count; i++)
		{
			if (!(list[i] != null))
			{
				list.RemoveAt(i);
				i--;
			}
		}
	}

	// Token: 0x0600009C RID: 156 RVA: 0x0000464C File Offset: 0x0000284C
	public static void Move<T>(this List<T> list, T item, int newIndex)
	{
		int num = list.IndexOf(item);
		if (num == -1)
		{
			throw new ArgumentException("Item not found in list", "item");
		}
		if (newIndex < 0 || newIndex >= list.Count)
		{
			throw new ArgumentOutOfRangeException("newIndex");
		}
		if (num == newIndex)
		{
			return;
		}
		list.RemoveAt(num);
		list.Insert(newIndex, item);
	}

	// Token: 0x0600009D RID: 157 RVA: 0x000046A4 File Offset: 0x000028A4
	public static void RemoveNulls<T>(this List<T> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] == null)
			{
				list.RemoveAt(i);
				i--;
			}
		}
	}

	// Token: 0x0600009E RID: 158 RVA: 0x000046DC File Offset: 0x000028DC
	public static string FormatString<T>(this List<T> list, string separator = "")
	{
		if (list.Count == 0)
		{
			return string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (T t in list)
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append(separator);
			}
			stringBuilder.Append(t);
		}
		return stringBuilder.ToString();
	}

	// Token: 0x0600009F RID: 159 RVA: 0x0000475C File Offset: 0x0000295C
	public static void AddIfNotContains<T>(this List<T> list, T item)
	{
		if (!list.Contains(item))
		{
			list.Add(item);
		}
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x00004770 File Offset: 0x00002970
	public static void Shuffle<T>(this IList<T> list)
	{
		int i = list.Count;
		while (i > 1)
		{
			i--;
			int num = global::UnityEngine.Random.Range(0, i);
			T t = list[num];
			list[num] = list[i];
			list[i] = t;
		}
	}
}
