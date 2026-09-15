using System;
using System.Reflection;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000137 RID: 311
	public abstract class Enumeration
	{
		// Token: 0x0600063A RID: 1594 RVA: 0x0001F867 File Offset: 0x0001DA67
		protected Enumeration(int value)
		{
			this.value = value;
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x0001F878 File Offset: 0x0001DA78
		public static T GetByStaticFieldName<T>(string name) where T : Enumeration
		{
			T t;
			try
			{
				t = (T)((object)typeof(T).GetField(name, BindingFlags.Static | BindingFlags.Public).GetValue(null));
			}
			catch (Exception)
			{
				Debug.LogError("Can't get static field for class:[T] input name:[" + name + "]");
				t = default(T);
			}
			return t;
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x0001F8D8 File Offset: 0x0001DAD8
		public static string GetNameOfStaticField<T>(int value) where T : Enumeration
		{
			FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public);
			for (int i = 0; i < fields.Length; i++)
			{
				if (((T)((object)fields[i].GetValue(null))).value == value)
				{
					return fields[i].Name;
				}
			}
			return string.Empty;
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x0001F92E File Offset: 0x0001DB2E
		public override int GetHashCode()
		{
			return -1584136870 + this.value.GetHashCode();
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x0001F941 File Offset: 0x0001DB41
		public static bool operator ==(Enumeration key1, Enumeration key2)
		{
			return key1 != null && key2 != null && key1.value == key2.value;
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x0001F959 File Offset: 0x0001DB59
		public static bool operator !=(Enumeration key1, Enumeration key2)
		{
			return key1 != null && key2 != null && key1.value != key2.value;
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x0001F974 File Offset: 0x0001DB74
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			Enumeration enumeration = obj as Enumeration;
			return enumeration != null && this.value == enumeration.value;
		}

		// Token: 0x04000398 RID: 920
		public int value;
	}
}
