using System;
using System.Globalization;

// Token: 0x02000467 RID: 1127
public readonly struct GameSaveVersion : IComparable<GameSaveVersion>, IEquatable<GameSaveVersion>
{
	// Token: 0x17000509 RID: 1289
	// (get) Token: 0x06001D89 RID: 7561 RVA: 0x0008B3D8 File Offset: 0x000895D8
	public float Number { get; }

	// Token: 0x1700050A RID: 1290
	// (get) Token: 0x06001D8A RID: 7562 RVA: 0x0008B3E0 File Offset: 0x000895E0
	public string Postfix { get; }

	// Token: 0x1700050B RID: 1291
	// (get) Token: 0x06001D8B RID: 7563 RVA: 0x0008B3E8 File Offset: 0x000895E8
	public int NumberAsInt { get; }

	// Token: 0x1700050C RID: 1292
	// (get) Token: 0x06001D8C RID: 7564 RVA: 0x0008B3F0 File Offset: 0x000895F0
	public bool HasPostfix
	{
		get
		{
			return !string.IsNullOrEmpty(this.Postfix);
		}
	}

	// Token: 0x06001D8D RID: 7565 RVA: 0x0008B400 File Offset: 0x00089600
	public GameSaveVersion(float number, string postfix = null)
	{
		this.Number = number;
		this.Postfix = postfix ?? string.Empty;
		this.NumberAsInt = GameSaveVersion.ToNumberAsInt(number);
	}

	// Token: 0x06001D8E RID: 7566 RVA: 0x0008B425 File Offset: 0x00089625
	private GameSaveVersion(float number, string postfix, int numberAsInt)
	{
		this.Number = number;
		this.Postfix = postfix ?? string.Empty;
		this.NumberAsInt = numberAsInt;
	}

	// Token: 0x06001D8F RID: 7567 RVA: 0x0008B448 File Offset: 0x00089648
	public static GameSaveVersion Parse(string version)
	{
		if (string.IsNullOrWhiteSpace(version))
		{
			throw new ArgumentException("Version string is null or empty.", "version");
		}
		version = version.Trim();
		int i = 0;
		bool flag = false;
		while (i < version.Length)
		{
			char c = version[i];
			if (char.IsDigit(c))
			{
				i++;
			}
			else
			{
				if (c != '.' || flag)
				{
					break;
				}
				flag = true;
				i++;
			}
		}
		if (i == 0)
		{
			throw new ArgumentException("Can't parse version: " + version, "version");
		}
		string text = version.Substring(0, i);
		string text2 = ((i < version.Length) ? version.Substring(i) : string.Empty);
		float num;
		if (!float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out num))
		{
			throw new ArgumentException("Can't parse version number: " + text, "version");
		}
		string text3 = text.Replace(".", string.Empty);
		int num2;
		if (!int.TryParse(text3, NumberStyles.Integer, CultureInfo.InvariantCulture, out num2))
		{
			throw new ArgumentException("Can't parse version as int: " + text3, "version");
		}
		return new GameSaveVersion(num, text2, num2);
	}

	// Token: 0x06001D90 RID: 7568 RVA: 0x0008B554 File Offset: 0x00089754
	public static bool TryParse(string version, out GameSaveVersion result)
	{
		bool flag;
		try
		{
			result = GameSaveVersion.Parse(version);
			flag = true;
		}
		catch (ArgumentException)
		{
			result = default(GameSaveVersion);
			flag = false;
		}
		return flag;
	}

	// Token: 0x06001D91 RID: 7569 RVA: 0x0008B590 File Offset: 0x00089790
	public static GameSaveVersion FromInt(int version)
	{
		string text = version.ToString(CultureInfo.InvariantCulture);
		return new GameSaveVersion(float.Parse((text.Length == 1) ? text : string.Format("{0}.{1}", text[0], text.Substring(1)), CultureInfo.InvariantCulture), string.Empty, version);
	}

	// Token: 0x1700050D RID: 1293
	// (get) Token: 0x06001D92 RID: 7570 RVA: 0x0008B5E8 File Offset: 0x000897E8
	public float ComparisonNumber
	{
		get
		{
			return GameSaveVersion.ToComparisonNumber(this.NumberAsInt, this.Postfix);
		}
	}

	// Token: 0x06001D93 RID: 7571 RVA: 0x0008B5FC File Offset: 0x000897FC
	public int CompareTo(GameSaveVersion other)
	{
		int num = this.NumberAsInt.CompareTo(other.NumberAsInt);
		if (num != 0)
		{
			return num;
		}
		return string.CompareOrdinal(this.Postfix, other.Postfix);
	}

	// Token: 0x06001D94 RID: 7572 RVA: 0x0008B638 File Offset: 0x00089838
	public int CompareTo(float value)
	{
		int num;
		string text;
		GameSaveVersion.SplitComparisonNumber(value, out num, out text);
		int num2 = this.NumberAsInt.CompareTo(num);
		if (num2 != 0)
		{
			return num2;
		}
		if (string.IsNullOrEmpty(text))
		{
			return 0;
		}
		return string.CompareOrdinal(this.Postfix, text);
	}

	// Token: 0x06001D95 RID: 7573 RVA: 0x0008B67A File Offset: 0x0008987A
	public bool Equals(GameSaveVersion other)
	{
		return this.NumberAsInt == other.NumberAsInt && string.Equals(this.Postfix, other.Postfix, StringComparison.Ordinal);
	}

	// Token: 0x06001D96 RID: 7574 RVA: 0x0008B6A0 File Offset: 0x000898A0
	public override bool Equals(object obj)
	{
		if (obj is GameSaveVersion)
		{
			GameSaveVersion gameSaveVersion = (GameSaveVersion)obj;
			return this.Equals(gameSaveVersion);
		}
		return false;
	}

	// Token: 0x06001D97 RID: 7575 RVA: 0x0008B6C5 File Offset: 0x000898C5
	public override int GetHashCode()
	{
		int num = this.NumberAsInt * 397;
		string postfix = this.Postfix;
		return num ^ ((postfix != null) ? postfix.GetHashCode() : 0);
	}

	// Token: 0x06001D98 RID: 7576 RVA: 0x0008B6E8 File Offset: 0x000898E8
	public override string ToString()
	{
		string text = this.Number.ToString(CultureInfo.InvariantCulture);
		if (!this.HasPostfix)
		{
			return text;
		}
		return text + this.Postfix;
	}

	// Token: 0x06001D99 RID: 7577 RVA: 0x0008B71F File Offset: 0x0008991F
	public static bool operator ==(GameSaveVersion left, GameSaveVersion right)
	{
		return left.Equals(right);
	}

	// Token: 0x06001D9A RID: 7578 RVA: 0x0008B729 File Offset: 0x00089929
	public static bool operator !=(GameSaveVersion left, GameSaveVersion right)
	{
		return !left.Equals(right);
	}

	// Token: 0x06001D9B RID: 7579 RVA: 0x0008B736 File Offset: 0x00089936
	public static bool operator <(GameSaveVersion left, GameSaveVersion right)
	{
		return left.CompareTo(right) < 0;
	}

	// Token: 0x06001D9C RID: 7580 RVA: 0x0008B743 File Offset: 0x00089943
	public static bool operator >(GameSaveVersion left, GameSaveVersion right)
	{
		return left.CompareTo(right) > 0;
	}

	// Token: 0x06001D9D RID: 7581 RVA: 0x0008B750 File Offset: 0x00089950
	public static bool operator <=(GameSaveVersion left, GameSaveVersion right)
	{
		return left.CompareTo(right) <= 0;
	}

	// Token: 0x06001D9E RID: 7582 RVA: 0x0008B760 File Offset: 0x00089960
	public static bool operator >=(GameSaveVersion left, GameSaveVersion right)
	{
		return left.CompareTo(right) >= 0;
	}

	// Token: 0x06001D9F RID: 7583 RVA: 0x0008B770 File Offset: 0x00089970
	public static bool operator ==(GameSaveVersion left, int right)
	{
		return left.NumberAsInt == right;
	}

	// Token: 0x06001DA0 RID: 7584 RVA: 0x0008B77C File Offset: 0x0008997C
	public static bool operator !=(GameSaveVersion left, int right)
	{
		return left.NumberAsInt != right;
	}

	// Token: 0x06001DA1 RID: 7585 RVA: 0x0008B78B File Offset: 0x0008998B
	public static bool operator <(GameSaveVersion left, int right)
	{
		return left.NumberAsInt < right;
	}

	// Token: 0x06001DA2 RID: 7586 RVA: 0x0008B797 File Offset: 0x00089997
	public static bool operator >(GameSaveVersion left, int right)
	{
		return left.NumberAsInt > right;
	}

	// Token: 0x06001DA3 RID: 7587 RVA: 0x0008B7A3 File Offset: 0x000899A3
	public static bool operator <=(GameSaveVersion left, int right)
	{
		return left.NumberAsInt <= right;
	}

	// Token: 0x06001DA4 RID: 7588 RVA: 0x0008B7B2 File Offset: 0x000899B2
	public static bool operator >=(GameSaveVersion left, int right)
	{
		return left.NumberAsInt >= right;
	}

	// Token: 0x06001DA5 RID: 7589 RVA: 0x0008B7C1 File Offset: 0x000899C1
	public static bool operator ==(int left, GameSaveVersion right)
	{
		return left == right.NumberAsInt;
	}

	// Token: 0x06001DA6 RID: 7590 RVA: 0x0008B7CD File Offset: 0x000899CD
	public static bool operator !=(int left, GameSaveVersion right)
	{
		return left != right.NumberAsInt;
	}

	// Token: 0x06001DA7 RID: 7591 RVA: 0x0008B7DC File Offset: 0x000899DC
	public static bool operator <(int left, GameSaveVersion right)
	{
		return left < right.NumberAsInt;
	}

	// Token: 0x06001DA8 RID: 7592 RVA: 0x0008B7E8 File Offset: 0x000899E8
	public static bool operator >(int left, GameSaveVersion right)
	{
		return left > right.NumberAsInt;
	}

	// Token: 0x06001DA9 RID: 7593 RVA: 0x0008B7F4 File Offset: 0x000899F4
	public static bool operator <=(int left, GameSaveVersion right)
	{
		return left <= right.NumberAsInt;
	}

	// Token: 0x06001DAA RID: 7594 RVA: 0x0008B803 File Offset: 0x00089A03
	public static bool operator >=(int left, GameSaveVersion right)
	{
		return left >= right.NumberAsInt;
	}

	// Token: 0x06001DAB RID: 7595 RVA: 0x0008B812 File Offset: 0x00089A12
	public static bool operator ==(GameSaveVersion left, float right)
	{
		return left.CompareTo(right) == 0;
	}

	// Token: 0x06001DAC RID: 7596 RVA: 0x0008B81F File Offset: 0x00089A1F
	public static bool operator !=(GameSaveVersion left, float right)
	{
		return left.CompareTo(right) != 0;
	}

	// Token: 0x06001DAD RID: 7597 RVA: 0x0008B82C File Offset: 0x00089A2C
	public static bool operator <(GameSaveVersion left, float right)
	{
		return left.CompareTo(right) < 0;
	}

	// Token: 0x06001DAE RID: 7598 RVA: 0x0008B839 File Offset: 0x00089A39
	public static bool operator >(GameSaveVersion left, float right)
	{
		return left.CompareTo(right) > 0;
	}

	// Token: 0x06001DAF RID: 7599 RVA: 0x0008B846 File Offset: 0x00089A46
	public static bool operator <=(GameSaveVersion left, float right)
	{
		return left.CompareTo(right) <= 0;
	}

	// Token: 0x06001DB0 RID: 7600 RVA: 0x0008B856 File Offset: 0x00089A56
	public static bool operator >=(GameSaveVersion left, float right)
	{
		return left.CompareTo(right) >= 0;
	}

	// Token: 0x06001DB1 RID: 7601 RVA: 0x0008B866 File Offset: 0x00089A66
	public static bool operator ==(float left, GameSaveVersion right)
	{
		return right.CompareTo(left) == 0;
	}

	// Token: 0x06001DB2 RID: 7602 RVA: 0x0008B873 File Offset: 0x00089A73
	public static bool operator !=(float left, GameSaveVersion right)
	{
		return right.CompareTo(left) != 0;
	}

	// Token: 0x06001DB3 RID: 7603 RVA: 0x0008B880 File Offset: 0x00089A80
	public static bool operator <(float left, GameSaveVersion right)
	{
		return right.CompareTo(left) > 0;
	}

	// Token: 0x06001DB4 RID: 7604 RVA: 0x0008B88D File Offset: 0x00089A8D
	public static bool operator >(float left, GameSaveVersion right)
	{
		return right.CompareTo(left) < 0;
	}

	// Token: 0x06001DB5 RID: 7605 RVA: 0x0008B89A File Offset: 0x00089A9A
	public static bool operator <=(float left, GameSaveVersion right)
	{
		return right.CompareTo(left) >= 0;
	}

	// Token: 0x06001DB6 RID: 7606 RVA: 0x0008B8AA File Offset: 0x00089AAA
	public static bool operator >=(float left, GameSaveVersion right)
	{
		return right.CompareTo(left) <= 0;
	}

	// Token: 0x06001DB7 RID: 7607 RVA: 0x0008B8BA File Offset: 0x00089ABA
	private static int ToNumberAsInt(float number)
	{
		return int.Parse(number.ToString(CultureInfo.InvariantCulture).Replace(".", string.Empty), CultureInfo.InvariantCulture);
	}

	// Token: 0x06001DB8 RID: 7608 RVA: 0x0008B8E4 File Offset: 0x00089AE4
	private static float ToComparisonNumber(int numberAsInt, string postfix)
	{
		if (string.IsNullOrEmpty(postfix) || postfix[0] != '.')
		{
			return (float)numberAsInt;
		}
		float num;
		if (float.TryParse("0" + postfix, NumberStyles.Float, CultureInfo.InvariantCulture, out num))
		{
			return (float)numberAsInt + num;
		}
		return (float)numberAsInt;
	}

	// Token: 0x06001DB9 RID: 7609 RVA: 0x0008B92C File Offset: 0x00089B2C
	private static void SplitComparisonNumber(float value, out int numberAsInt, out string postfix)
	{
		decimal num = decimal.Round((decimal)value, 4, MidpointRounding.AwayFromZero);
		numberAsInt = (int)num;
		decimal num2 = num - numberAsInt;
		if (num2 == 0m)
		{
			postfix = string.Empty;
			return;
		}
		string text = num2.ToString(CultureInfo.InvariantCulture);
		postfix = (text.StartsWith("0.", StringComparison.Ordinal) ? text.Substring(1).TrimEnd('0') : ("." + text.TrimEnd('0')));
		if (postfix == ".")
		{
			postfix = string.Empty;
		}
	}
}
