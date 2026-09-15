using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using LinqTools;
using UnityEngine;

// Token: 0x02000772 RID: 1906
public static class CommandFieldAttributeHelper
{
	// Token: 0x06003189 RID: 12681 RVA: 0x000EA920 File Offset: 0x000E8B20
	public static List<FieldInfo> GetCommandSerializedFields(Type type)
	{
		List<FieldInfo> list = new List<FieldInfo>();
		foreach (FieldInfo fieldInfo in from f in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
			where f.GetCustomAttribute<CommandFieldAttribute>() != null
			select f)
		{
			Type fieldType = fieldInfo.FieldType;
			if (!fieldType.IsPrimitive && !(fieldType == typeof(string)) && !(fieldType == typeof(Vector2)) && !(fieldType == typeof(Vector3)) && !fieldType.IsEnum && !typeof(IEnumerable).IsAssignableFrom(fieldType))
			{
				throw new Exception(string.Format("Unsupported type: {0}", fieldType));
			}
			list.Add(fieldInfo);
		}
		return list;
	}

	// Token: 0x0600318A RID: 12682 RVA: 0x000EAA10 File Offset: 0x000E8C10
	public static int GetCommandFieldsSerializedSize<T>(T obj, List<FieldInfo> fieldInfos)
	{
		int num = 0;
		foreach (FieldInfo fieldInfo in fieldInfos)
		{
			Type fieldType = fieldInfo.FieldType;
			object value = fieldInfo.GetValue(obj);
			if (fieldType.IsPrimitive)
			{
				num += Marshal.SizeOf(fieldType);
			}
			else if (fieldType == typeof(string))
			{
				num += Encoding.UTF32.GetBytes((string)value).Length;
			}
			else if (fieldType == typeof(Vector2))
			{
				num += 8;
			}
			else if (fieldType == typeof(Vector3))
			{
				num += 12;
			}
			else
			{
				if (!fieldType.IsEnum)
				{
					if (typeof(IEnumerable).IsAssignableFrom(fieldType))
					{
						using (IEnumerator enumerator2 = ((IEnumerable)value).GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								object obj2 = enumerator2.Current;
								num += CommandFieldAttributeHelper.GetCommandFieldsSerializedSize<object>(obj2, CommandFieldAttributeHelper.GetCommandSerializedFields(obj2.GetType()));
							}
							continue;
						}
					}
					throw new Exception(string.Format("Unsupported type: {0}, value: {1}", fieldType, value));
				}
				Type underlyingType = Enum.GetUnderlyingType(fieldType);
				num += Marshal.SizeOf(underlyingType) * Enum.GetNames(fieldType).Length;
			}
		}
		return num;
	}
}
