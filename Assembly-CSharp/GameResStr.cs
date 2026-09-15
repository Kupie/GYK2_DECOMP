using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x02000ADA RID: 2778
[Serializable]
public class GameResStr
{
	// Token: 0x17000B45 RID: 2885
	// (get) Token: 0x06004AB9 RID: 19129 RVA: 0x00160AAB File Offset: 0x0015ECAB
	public List<string> Keys
	{
		get
		{
			return this.keys;
		}
	}

	// Token: 0x17000B46 RID: 2886
	// (get) Token: 0x06004ABA RID: 19130 RVA: 0x00160AB3 File Offset: 0x0015ECB3
	public List<string> Values
	{
		get
		{
			return this.values;
		}
	}

	// Token: 0x06004ABB RID: 19131 RVA: 0x00160ABB File Offset: 0x0015ECBB
	public GameResStr()
	{
	}

	// Token: 0x06004ABC RID: 19132 RVA: 0x00160ADC File Offset: 0x0015ECDC
	public GameResStr(GameResStr other)
	{
		for (int i = 0; i < other.keys.Count; i++)
		{
			this.Set(other.keys[i], other.values[i]);
		}
	}

	// Token: 0x06004ABD RID: 19133 RVA: 0x00160B39 File Offset: 0x0015ED39
	public GameResStr(string key, string value)
	{
		this.Set(key, value);
	}

	// Token: 0x06004ABE RID: 19134 RVA: 0x00160B60 File Offset: 0x0015ED60
	public string Get(string key, string defaultValue = "")
	{
		int num = this.keys.IndexOf(key);
		if (num != -1)
		{
			return this.values[num];
		}
		return defaultValue;
	}

	// Token: 0x06004ABF RID: 19135 RVA: 0x00160B8C File Offset: 0x0015ED8C
	public string GetKeyByValue(string value, string emptyKey = "")
	{
		int num = this.values.IndexOf(value);
		if (num != -1)
		{
			return this.keys[num];
		}
		return emptyKey;
	}

	// Token: 0x06004AC0 RID: 19136 RVA: 0x00160BB8 File Offset: 0x0015EDB8
	public bool Has(string key)
	{
		return this.keys.Contains(key);
	}

	// Token: 0x06004AC1 RID: 19137 RVA: 0x00160BC6 File Offset: 0x0015EDC6
	public void Clear()
	{
		this.keys.Clear();
		this.values.Clear();
	}

	// Token: 0x06004AC2 RID: 19138 RVA: 0x00160BE0 File Offset: 0x0015EDE0
	public void Set(string key, string value)
	{
		int num = this.keys.IndexOf(key);
		if (num != -1)
		{
			this.values[num] = value;
			return;
		}
		this.keys.Add(key);
		this.values.Add(value);
	}

	// Token: 0x06004AC3 RID: 19139 RVA: 0x00160C24 File Offset: 0x0015EE24
	public void Set(GameResStr gameRes)
	{
		int count = gameRes.Keys.Count;
		for (int i = 0; i < count; i++)
		{
			this.Set(gameRes.Keys[i], gameRes.values[i]);
		}
	}

	// Token: 0x06004AC4 RID: 19140 RVA: 0x00160C68 File Offset: 0x0015EE68
	public void Remove(string key)
	{
		int num = this.keys.IndexOf(key);
		if (num != -1)
		{
			this.keys.RemoveAt(num);
			this.values.RemoveAt(num);
		}
	}

	// Token: 0x06004AC5 RID: 19141 RVA: 0x00160C9E File Offset: 0x0015EE9E
	public bool IsEmpty()
	{
		return this.keys.Count == 0;
	}

	// Token: 0x06004AC6 RID: 19142 RVA: 0x00160CB0 File Offset: 0x0015EEB0
	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder("[GameResStr: ");
		for (int i = 0; i < this.keys.Count; i++)
		{
			if (i > 0)
			{
				stringBuilder.Append(", ");
			}
			stringBuilder.Append(this.keys[i]);
			stringBuilder.Append("=");
			stringBuilder.Append(this.values[i]);
		}
		stringBuilder.Append("]");
		return stringBuilder.ToString();
	}

	// Token: 0x06004AC7 RID: 19143 RVA: 0x00160D34 File Offset: 0x0015EF34
	public override bool Equals(object obj)
	{
		GameResStr gameResStr = obj as GameResStr;
		if (gameResStr == null)
		{
			return false;
		}
		if (this.keys.Count != gameResStr.keys.Count)
		{
			return false;
		}
		for (int i = 0; i < this.keys.Count; i++)
		{
			if (this.keys[i] != gameResStr.keys[i] || this.values[i] != gameResStr.values[i])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06004AC8 RID: 19144 RVA: 0x00160DBE File Offset: 0x0015EFBE
	public override int GetHashCode()
	{
		return (-1419608871 * -1521134295 + EqualityComparer<List<string>>.Default.GetHashCode(this.keys)) * -1521134295 + EqualityComparer<List<string>>.Default.GetHashCode(this.values);
	}

	// Token: 0x04003AED RID: 15085
	[SerializeField]
	private List<string> keys = new List<string>();

	// Token: 0x04003AEE RID: 15086
	[SerializeField]
	private List<string> values = new List<string>();
}
