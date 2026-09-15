using System;
using System.Collections.Generic;
using System.Globalization;
using Expressive;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x020000E5 RID: 229
	[Serializable]
	public abstract class LazyExpressionBase
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060003FB RID: 1019 RVA: 0x000161B9 File Offset: 0x000143B9
		public bool HasExpression
		{
			get
			{
				return !string.IsNullOrEmpty(this.expressionString);
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060003FC RID: 1020 RVA: 0x000161C9 File Offset: 0x000143C9
		public bool HasPureValue
		{
			get
			{
				return this.pureValueType > PureValueType.None;
			}
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x000161D4 File Offset: 0x000143D4
		public void FromString(string str)
		{
			this.FromString(str, PureValueType.None);
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x000161E0 File Offset: 0x000143E0
		public void FromString(string str, PureValueType expectedPureType)
		{
			str = str.Replace("&quot;", "\"");
			this.expressionString = str;
			this.expression = null;
			this.pureValueType = PureValueType.None;
			bool flag = float.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out this.pureValueFloat);
			if (expectedPureType == PureValueType.None && flag)
			{
				this.pureValueType = PureValueType.Float;
				return;
			}
			if (expectedPureType != PureValueType.None && !this.HasExpressionSymptom(str))
			{
				this.pureValueType = expectedPureType;
				switch (expectedPureType)
				{
				case PureValueType.Float:
				case PureValueType.String:
					break;
				case PureValueType.Bool:
					if (!bool.TryParse(str, out this.pureValueBool))
					{
						this.pureValueBool = str == "1" || (flag && this.pureValueFloat != 0f);
					}
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x000162A0 File Offset: 0x000144A0
		protected virtual bool HasExpressionSymptom(string strToCheck)
		{
			return !string.IsNullOrEmpty(strToCheck) && (strToCheck.Contains('(') || strToCheck.Contains('"') || strToCheck.Contains('+') || strToCheck.Contains('-') || strToCheck.Contains('/') || strToCheck.Contains('*'));
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x000162F3 File Offset: 0x000144F3
		public string GetRawExpressionString()
		{
			return this.expressionString;
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x000162FC File Offset: 0x000144FC
		public void ValidateExpression(string errorMessagePrefix)
		{
			this.CheckExpressionInit();
			try
			{
				if (this.expression != null)
				{
					IReadOnlyCollection<string> referencedVariables = this.expression.ReferencedVariables;
				}
			}
			catch (Exception ex)
			{
				this.HandleEvaluateError(ex, errorMessagePrefix);
			}
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x00016344 File Offset: 0x00014544
		public override string ToString()
		{
			return this.expressionString.ToString();
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00016351 File Offset: 0x00014551
		public string ToUnparsedString()
		{
			return this.expressionStringUnparsed;
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x00016359 File Offset: 0x00014559
		protected virtual void CheckExpressionInit()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x00016360 File Offset: 0x00014560
		protected virtual void HandleEvaluateError(Exception e, string errorMessagePrefix = null)
		{
			string text = string.Format("Error in expression {0} ({1}): {2}", this.expression, this.expressionString, e);
			if (!string.IsNullOrEmpty(errorMessagePrefix))
			{
				text = text.Insert(0, errorMessagePrefix + "\n");
			}
			Debug.LogError(text);
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x000163A6 File Offset: 0x000145A6
		protected virtual string ParseRegex(string input)
		{
			return input;
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x000163A9 File Offset: 0x000145A9
		public static T ParseExpression<T>(string expressionString) where T : LazyExpressionBase, new()
		{
			return LazyExpressionBase.ParseExpression<T>(expressionString, PureValueType.None);
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x000163B4 File Offset: 0x000145B4
		public static T ParseExpression<T>(string expressionString, PureValueType pureValueType) where T : LazyExpressionBase, new()
		{
			expressionString = expressionString.Trim().Replace("&#xd", "").Replace("&#xD", "");
			if (string.IsNullOrEmpty(expressionString))
			{
				return default(T);
			}
			string cyrillicValidationError = LazyExpressionBase.GetCyrillicValidationError(expressionString);
			if (!string.IsNullOrEmpty(cyrillicValidationError))
			{
				Debug.LogError(cyrillicValidationError);
			}
			T t = new T();
			t.expressionStringUnparsed = expressionString;
			expressionString = t.ParseRegex(expressionString);
			t.FromString(expressionString, pureValueType);
			return t;
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0001643C File Offset: 0x0001463C
		public static string GetCyrillicValidationError(string value)
		{
			char c;
			int num;
			if (string.IsNullOrEmpty(value) || !LazyExpressionBase.TryGetCyrillicChar(value, out c, out num))
			{
				return string.Empty;
			}
			string text = "Cyrillic characters are not allowed in LazyExpression [{0}]: '{1}' (U+{2}) at index {3}";
			object[] array = new object[4];
			array[0] = value;
			array[1] = c;
			int num2 = 2;
			int num3 = (int)c;
			array[num2] = num3.ToString("X4");
			array[3] = num;
			return string.Format(text, array);
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0001649C File Offset: 0x0001469C
		private static bool TryGetCyrillicChar(string value, out char cyrillicChar, out int index)
		{
			for (int i = 0; i < value.Length; i++)
			{
				char c = value[i];
				if (LazyExpressionBase.IsCyrillic(c))
				{
					cyrillicChar = c;
					index = i;
					return true;
				}
			}
			cyrillicChar = '\0';
			index = -1;
			return false;
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x000164DC File Offset: 0x000146DC
		private static bool IsCyrillic(char c)
		{
			return (c >= 'Ѐ' && c <= 'ӿ') || (c >= 'Ԁ' && c <= 'ԯ') || (c >= '\u2de0' && c <= '\u2dff') || (c >= 'Ꙁ' && c <= '\ua69f') || (c >= 'ᲀ' && c <= '\u1c8f');
		}

		// Token: 0x040001F4 RID: 500
		[SerializeField]
		protected string expressionString = string.Empty;

		// Token: 0x040001F5 RID: 501
		[SerializeField]
		protected string expressionStringUnparsed = string.Empty;

		// Token: 0x040001F6 RID: 502
		[SerializeField]
		protected PureValueType pureValueType;

		// Token: 0x040001F7 RID: 503
		[SerializeField]
		protected float pureValueFloat;

		// Token: 0x040001F8 RID: 504
		[SerializeField]
		protected bool pureValueBool;

		// Token: 0x040001F9 RID: 505
		[NonSerialized]
		protected Expression expression;
	}
}
