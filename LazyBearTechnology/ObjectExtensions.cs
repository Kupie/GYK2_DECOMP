using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

// Token: 0x02000024 RID: 36
public static class ObjectExtensions
{
	// Token: 0x060000A4 RID: 164 RVA: 0x00004898 File Offset: 0x00002A98
	public static T DeepCloneByBinaryFormatter<T>(this T obj)
	{
		T t;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Serialize(memoryStream, obj);
			memoryStream.Position = 0L;
			t = (T)((object)binaryFormatter.Deserialize(memoryStream));
		}
		return t;
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x000048F0 File Offset: 0x00002AF0
	public static T DeepClone<T>(this T original)
	{
		if (original == null)
		{
			return default(T);
		}
		Type type = original.GetType();
		if (type.IsValueType || type == typeof(string))
		{
			return original;
		}
		if (type.IsArray)
		{
			Type type2 = Type.GetType(type.FullName.Replace("[]", string.Empty));
			Array array = original as Array;
			Array array2 = Array.CreateInstance(type2, array.Length);
			for (int i = 0; i < array.Length; i++)
			{
				array2.SetValue(array.GetValue(i).DeepClone<object>(), i);
			}
			return (T)((object)Convert.ChangeType(array2, original.GetType()));
		}
		if (type.IsClass)
		{
			T t = (T)((object)Activator.CreateInstance(original.GetType()));
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				object value = fieldInfo.GetValue(original);
				if (value != null)
				{
					fieldInfo.SetValue(t, value.DeepClone<object>());
				}
			}
			return t;
		}
		throw new ArgumentException("DeepClone: Unknown type [" + type.ToString() + "].");
	}
}
