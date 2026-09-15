using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Sirenix.Serialization;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x0200013D RID: 317
	public static class LazySerializer
	{
		// Token: 0x0600064D RID: 1613 RVA: 0x0001FB44 File Offset: 0x0001DD44
		public static byte[] Serialize<T>(T o) where T : class
		{
			Stream stream;
			MemoryStream memoryStream = (stream = new MemoryStream());
			LazySerializer.Serialize<T>(o, stream);
			return memoryStream.ToArray();
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x0001FB64 File Offset: 0x0001DD64
		public static void Serialize<T>(T o, Stream stream) where T : class
		{
			BinaryWriter binaryWriter = new BinaryWriter(stream);
			binaryWriter.Write(0L);
			LazySerializer.WriteHeader(binaryWriter, new LazySerializer.Header());
			LazySerializer.SerializerData serializerData = new LazySerializer.SerializerData();
			LazySerializer.SerializeInternal<T>(o, binaryWriter, serializerData);
			LazySerializer.WriteSerializerData(binaryWriter, serializerData);
			binaryWriter.Close();
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x0001FBA6 File Offset: 0x0001DDA6
		public static T Deserialize<T>(byte[] data) where T : new()
		{
			return LazySerializer.Deserialize<T>(new MemoryStream(data));
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x0001FBB4 File Offset: 0x0001DDB4
		public static T Deserialize<T>(Stream stream) where T : new()
		{
			LazySerializer.ClearCache();
			stream.Seek(0L, SeekOrigin.Begin);
			BinaryReader binaryReader = new BinaryReader(stream);
			LazySerializer.SerializerData serializerData = LazySerializer.ReadSerializerData(binaryReader);
			LazySerializer.ReadHeader(binaryReader);
			return LazySerializer.DeserializeInternal<T>(binaryReader, serializerData);
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x0001FBEC File Offset: 0x0001DDEC
		public static void DeserializeInto<T>(T obj, byte[] data)
		{
			LazySerializer.ClearCache();
			BinaryReader binaryReader = new BinaryReader(new MemoryStream(data));
			LazySerializer.SerializerData serializerData = LazySerializer.ReadSerializerData(binaryReader);
			LazySerializer.ReadHeader(binaryReader);
			LazySerializer.DeserializeIntoInternal(obj.GetType(), obj, binaryReader, serializerData);
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x0001FC33 File Offset: 0x0001DE33
		private static void SerializeInternal<T>(T o, BinaryWriter bw, LazySerializer.SerializerData sd) where T : class
		{
			LazySerializer.ClearCache();
			LazySerializer.SerializeInternal(o.GetType(), o, bw, sd);
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x0001FC52 File Offset: 0x0001DE52
		private static void ClearCache()
		{
			LazySerializer.fieldsCache.Clear();
			LazySerializer.fieldsHashesCache.Clear();
			LazySerializer.stringHashes.Clear();
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x0001FC74 File Offset: 0x0001DE74
		private static List<FieldInfo> GetSerializableFieldsForType(Type objType)
		{
			List<FieldInfo> list;
			if (LazySerializer.fieldsCache.TryGetValue(objType, out list))
			{
				return list;
			}
			list = new List<FieldInfo>();
			foreach (FieldInfo fieldInfo in objType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (((fieldInfo.IsPublic && !fieldInfo.IsNotSerialized) || Attribute.IsDefined(fieldInfo, LazySerializer.TYPE_SERIALIZED_FIELD, true) || Attribute.IsDefined(fieldInfo, LazySerializer.TYPE_LAZY_SERIALIZE, true) || Attribute.IsDefined(fieldInfo, LazySerializer.TYPE_ODIN_SERIALIZE, true)) && !Attribute.IsDefined(fieldInfo, LazySerializer.TYPE_LAZY_DONT_SERIALIZE) && !objType.IsSubclassOf(LazySerializer.TYPE_COMPONENT) && !objType.IsSubclassOf(LazySerializer.TYPE_MONOBEHAVIOUR))
				{
					list.Add(fieldInfo);
				}
			}
			LazySerializer.fieldsCache.Add(objType, list);
			return list;
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x0001FD34 File Offset: 0x0001DF34
		private static Dictionary<int, FieldInfo> GetFieldsHashesForType(Type objType)
		{
			Dictionary<int, FieldInfo> dictionary;
			if (objType != null && LazySerializer.fieldsHashesCache.TryGetValue(objType, out dictionary))
			{
				return dictionary;
			}
			dictionary = new Dictionary<int, FieldInfo>();
			if (objType == null)
			{
				return dictionary;
			}
			foreach (FieldInfo fieldInfo in LazySerializer.GetSerializableFieldsForType(objType))
			{
				dictionary.Add(fieldInfo.Name.GetStableHashCode(), fieldInfo);
			}
			LazySerializer.fieldsHashesCache.Add(objType, dictionary);
			return dictionary;
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x0001FDCC File Offset: 0x0001DFCC
		private static void SerializeInternal(Type objType, object o, BinaryWriter bw, LazySerializer.SerializerData sd)
		{
			if (o == null)
			{
				bw.Write(-1);
				return;
			}
			List<FieldInfo> serializableFieldsForType = LazySerializer.GetSerializableFieldsForType(objType);
			if (LazySerializer.TYPE_INTERFACE.IsAssignableFrom(objType))
			{
				objType.GetMethod("OnLazyPreSerialize").Invoke(o, null);
			}
			bw.Write(serializableFieldsForType.Count);
			foreach (FieldInfo fieldInfo in serializableFieldsForType)
			{
				Type fieldType = fieldInfo.FieldType;
				object value = fieldInfo.GetValue(o);
				int stableHashCode = fieldInfo.Name.GetStableHashCode();
				bw.Write(stableHashCode);
				if (!LazySerializer.TrySerializeObject(fieldType, bw, value, sd))
				{
					if (fieldType.IsPrimitive)
					{
						Debug.LogError("Unsupported serialization type: " + fieldType.Name);
					}
					else
					{
						bw.Write(250);
						LazySerializer.SerializeInternal(fieldType, value, bw, sd);
					}
				}
				bw.Flush();
			}
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x0001FEB8 File Offset: 0x0001E0B8
		private static bool TrySerializeObject(Type type, BinaryWriter bw, object value, LazySerializer.SerializerData sd)
		{
			if (value == null)
			{
				bw.Write(0);
				return true;
			}
			if (type == typeof(string))
			{
				string text = (string)value;
				if (text.Length == 0)
				{
					bw.Write(11);
				}
				else if (text.Length > 30)
				{
					bw.Write(9);
					LazySerializer.WriteStringToStream(text, bw, false);
				}
				else
				{
					bw.Write(10);
					LazySerializer.WriteIndexedStringToStream(text, bw, sd);
				}
				return true;
			}
			if (type == typeof(int))
			{
				int num = (int)value;
				if (num != 0)
				{
					if (num != 1)
					{
						bw.Write(3);
						bw.Write(num);
					}
					else
					{
						bw.Write(17);
					}
				}
				else
				{
					bw.Write(16);
				}
				return true;
			}
			if (type == typeof(long))
			{
				bw.Write(4);
				bw.Write((long)value);
				return true;
			}
			if (type == typeof(bool))
			{
				bw.Write(((bool)value) ? 1 : 2);
				return true;
			}
			if (type == typeof(float))
			{
				float num2 = (float)value;
				if (num2 == 0f)
				{
					bw.Write(18);
				}
				else if (num2 == 1f)
				{
					bw.Write(19);
				}
				else
				{
					bw.Write(5);
					bw.Write(num2);
				}
				return true;
			}
			if (type == typeof(double))
			{
				bw.Write(6);
				bw.Write((double)value);
				return true;
			}
			if (type == typeof(byte))
			{
				bw.Write(7);
				bw.Write((byte)value);
				return true;
			}
			if (type == typeof(char))
			{
				bw.Write(8);
				bw.Write((char)value);
				return true;
			}
			if (type == typeof(Vector2))
			{
				Vector2 vector = (Vector2)value;
				if (vector.x == 0f && vector.y == 0f)
				{
					bw.Write(20);
				}
				else if (vector.x == 1f && vector.y == 1f)
				{
					bw.Write(21);
				}
				else
				{
					bw.Write(13);
					bw.Write(vector.x);
					bw.Write(vector.y);
				}
				return true;
			}
			if (type == typeof(Vector3))
			{
				Vector3 vector2 = (Vector3)value;
				if (vector2.x == 0f && vector2.y == 0f && vector2.z == 0f)
				{
					bw.Write(22);
				}
				else if (vector2.x == 1f && vector2.y == 1f && vector2.z == 1f)
				{
					bw.Write(23);
				}
				else
				{
					bw.Write(14);
					bw.Write(vector2.x);
					bw.Write(vector2.y);
					bw.Write(vector2.z);
				}
				return true;
			}
			if (type == typeof(Quaternion))
			{
				Quaternion quaternion = (Quaternion)value;
				if (quaternion.x == 0f && quaternion.y == 0f && quaternion.z == 0f && quaternion.w == 1f)
				{
					bw.Write(24);
				}
				else
				{
					bw.Write(15);
					bw.Write(quaternion.x);
					bw.Write(quaternion.y);
					bw.Write(quaternion.z);
					bw.Write(quaternion.w);
				}
				return true;
			}
			if (type == typeof(byte[]))
			{
				bw.Write(102);
				byte[] array = (byte[])value;
				bw.Write(array.Length);
				bw.Write(array);
				return true;
			}
			if (type.IsArray)
			{
				Type elementType = type.GetElementType();
				Array array2 = value as Array;
				bw.Write(101);
				bw.Write(array2.Length);
				for (int i = 0; i < array2.Length; i++)
				{
					object value2 = array2.GetValue(i);
					if (!LazySerializer.TrySerializeObject(elementType, bw, value2, sd))
					{
						bw.Write(250);
						LazySerializer.SerializeInternal(elementType, value2, bw, sd);
					}
				}
				return true;
			}
			if (value is IList && type.IsGenericType)
			{
				IList list = value as IList;
				bw.Write(100);
				bw.Write(list.Count);
				Type type2 = type.GetGenericArguments()[0];
				for (int j = 0; j < list.Count; j++)
				{
					object obj = list[j];
					if (!LazySerializer.TrySerializeObject(type2, bw, obj, sd))
					{
						bw.Write(250);
						LazySerializer.SerializeInternal(type2, obj, bw, sd);
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x0002037E File Offset: 0x0001E57E
		private static T DeserializeInternal<T>(BinaryReader br, LazySerializer.SerializerData sd) where T : new()
		{
			return (T)((object)LazySerializer.DeserializeInternal(typeof(T), br, sd));
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x00020398 File Offset: 0x0001E598
		private static object DeserializeInternal(Type objType, BinaryReader br, LazySerializer.SerializerData sd)
		{
			object obj = ((objType == null) ? null : Activator.CreateInstance(objType));
			return LazySerializer.DeserializeIntoInternal(objType, obj, br, sd);
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x000203C4 File Offset: 0x0001E5C4
		private static object DeserializeIntoInternal(Type objType, object obj, BinaryReader br, LazySerializer.SerializerData sd)
		{
			Dictionary<int, FieldInfo> fieldsHashesForType = LazySerializer.GetFieldsHashesForType(objType);
			int num = br.ReadInt32();
			if (num == -1)
			{
				return null;
			}
			for (int i = 0; i < num; i++)
			{
				int num2 = br.ReadInt32();
				FieldInfo fieldInfo = null;
				fieldsHashesForType.TryGetValue(num2, out fieldInfo);
				if (fieldInfo == null)
				{
					Debug.LogWarning("Can't find a field to deserialize: " + num2.ToString());
				}
				object obj2 = null;
				try
				{
					obj2 = LazySerializer.DeserializeObject((fieldInfo == null) ? null : fieldInfo.FieldType, br, sd);
				}
				catch (MissingMethodException ex)
				{
					if (fieldInfo != null)
					{
						string[] array = new string[6];
						array[0] = "Error deserializing object: ";
						int num3 = 1;
						Type fieldType = fieldInfo.FieldType;
						array[num3] = ((fieldType != null) ? fieldType.ToString() : null);
						array[2] = ", name = ";
						array[3] = fieldInfo.Name;
						array[4] = ", ex: ";
						array[5] = ex.Message;
						Debug.LogError(string.Concat(array));
					}
					throw;
				}
				if (fieldInfo != null)
				{
					if (obj != null)
					{
						fieldInfo.SetValue(obj, obj2);
					}
				}
				else
				{
					Debug.LogError(string.Concat(new string[]
					{
						"Couldn't find a field to deserialize, obj_type = ",
						(objType != null) ? objType.ToString() : null,
						", field_type = ",
						(obj2 == null) ? "null" : obj2.GetType().ToString(),
						", hash_size = ",
						fieldsHashesForType.Count.ToString()
					}));
				}
			}
			if (objType != null && LazySerializer.TYPE_INTERFACE.IsAssignableFrom(objType))
			{
				objType.GetMethod("OnLazyPostDeserialize").Invoke(obj, null);
			}
			return obj;
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x00020564 File Offset: 0x0001E764
		private static object DeserializeObject(Type objType, BinaryReader br, LazySerializer.SerializerData sd)
		{
			LazySerializer.FieldType fieldType = (LazySerializer.FieldType)br.ReadByte();
			switch (fieldType)
			{
			case LazySerializer.FieldType.NullValue:
				return null;
			case LazySerializer.FieldType.BoolTrue:
				return true;
			case LazySerializer.FieldType.BoolFalse:
				return false;
			case LazySerializer.FieldType.Int32:
				return br.ReadInt32();
			case LazySerializer.FieldType.Int64:
				return br.ReadInt64();
			case LazySerializer.FieldType.Single:
				return br.ReadSingle();
			case LazySerializer.FieldType.Double:
				return br.ReadDouble();
			case LazySerializer.FieldType.Byte:
				return br.ReadByte();
			case LazySerializer.FieldType.Char:
				return br.ReadChar();
			case LazySerializer.FieldType.String:
				return LazySerializer.ReadStringFromStream(br, false);
			case LazySerializer.FieldType.StringIndexer:
				return LazySerializer.ReadIndexedStringFromStream(br, sd);
			case LazySerializer.FieldType.StringEmpty:
				return string.Empty;
			case LazySerializer.FieldType.Json:
				break;
			case LazySerializer.FieldType.Vector2:
				return new Vector2(br.ReadSingle(), br.ReadSingle());
			case LazySerializer.FieldType.Vector3:
				return new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
			case LazySerializer.FieldType.Quaternion:
				return new Quaternion(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
			case LazySerializer.FieldType.Int32_0:
				return 0;
			case LazySerializer.FieldType.Int32_1:
				return 1;
			case LazySerializer.FieldType.Single_0:
				return 0f;
			case LazySerializer.FieldType.Single_1:
				return 1f;
			case LazySerializer.FieldType.Vector2_00:
				return Vector2.zero;
			case LazySerializer.FieldType.Vector2_11:
				return Vector2.one;
			case LazySerializer.FieldType.Vector3_000:
				return Vector3.zero;
			case LazySerializer.FieldType.Vector3_111:
				return Vector3.one;
			case LazySerializer.FieldType.Quaternion_0001:
				return new Quaternion(0f, 0f, 0f, 1f);
			default:
				switch (fieldType)
				{
				case LazySerializer.FieldType.GenericList:
				{
					int num = br.ReadInt32();
					Type type = ((objType == null) ? typeof(object) : objType.GetGenericArguments()[0]);
					IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(new Type[] { type }));
					for (int i = 0; i < num; i++)
					{
						object obj = LazySerializer.DeserializeObject(type, br, sd);
						if (objType != null)
						{
							list.Add(obj);
						}
					}
					return list;
				}
				case LazySerializer.FieldType.Array:
				{
					int num2 = br.ReadInt32();
					Type type2 = ((objType == null) ? typeof(object) : objType.GetElementType());
					Array array = ((type2 == null) ? null : Array.CreateInstance(type2, num2));
					for (int j = 0; j < num2; j++)
					{
						object obj2 = LazySerializer.DeserializeObject(type2, br, sd);
						if (array != null)
						{
							array.SetValue(obj2, j);
						}
					}
					return array;
				}
				case LazySerializer.FieldType.ByteArray:
				{
					int num3 = br.ReadInt32();
					return br.ReadBytes(num3);
				}
				default:
					if (fieldType == LazySerializer.FieldType.LazySerialized)
					{
						return LazySerializer.DeserializeInternal(objType, br, sd);
					}
					break;
				}
				break;
			}
			throw new NotImplementedException("Not implemented deserialization of a field type: " + fieldType.ToString());
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x00020854 File Offset: 0x0001EA54
		private static string ReadStringFromStream(BinaryReader br, bool encrypt = false)
		{
			int num = br.ReadInt32();
			if (num == -1)
			{
				return null;
			}
			int i = num;
			LazySerializer.sb.Length = 0;
			while (i > 0)
			{
				int num2 = ((i <= LazySerializer.buffer.Length) ? i : LazySerializer.buffer.Length);
				i -= num2;
				long num3 = (long)br.Read(LazySerializer.buffer, 0, num2);
				if (encrypt)
				{
					int num4 = 0;
					while ((long)num4 < num3)
					{
						uint num5 = (uint)LazySerializer.buffer[num4];
						if (num5 <= 255U && num5 != 0U && num5 != 109U)
						{
							num5 ^= 109U;
							LazySerializer.buffer[num4] = (char)num5;
						}
						num4++;
					}
				}
				LazySerializer.sb.Append(LazySerializer.buffer, 0, num2);
				if (num3 < (long)num2)
				{
					throw new EndOfStreamException();
				}
			}
			return LazySerializer.sb.ToString();
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x0002091C File Offset: 0x0001EB1C
		private static string ReadIndexedStringFromStream(BinaryReader br, LazySerializer.SerializerData sd)
		{
			int num = br.ReadInt32();
			return sd.strings[num];
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x0002093C File Offset: 0x0001EB3C
		private static void WriteIndexedStringToStream(string s, BinaryWriter bw, LazySerializer.SerializerData sd)
		{
			int num = sd.strings.IndexOf(s);
			if (num == -1)
			{
				sd.strings.Add(s);
				num = sd.strings.Count - 1;
			}
			bw.Write(num);
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x0002097C File Offset: 0x0001EB7C
		private static void WriteStringToStream(string s, BinaryWriter bw, bool encrypt = false)
		{
			if (s == null)
			{
				bw.Write(-1);
				return;
			}
			char[] array = s.ToCharArray();
			bw.Write(array.Length);
			if (encrypt)
			{
				for (int i = 0; i < array.Length; i++)
				{
					uint num = (uint)array[i];
					if (num <= 255U && num != 0U && num != 109U)
					{
						num ^= 109U;
						array[i] = (char)num;
					}
				}
			}
			bw.Write(array);
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x000209DC File Offset: 0x0001EBDC
		private static LazySerializer.Header ReadHeader(BinaryReader br)
		{
			LazySerializer.Header header = new LazySerializer.Header
			{
				sdataOffset = br.ReadInt64(),
				version = br.ReadInt32()
			};
			for (int i = 0; i < 15; i++)
			{
				br.ReadInt32();
			}
			return header;
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x00020A1C File Offset: 0x0001EC1C
		private static void WriteHeader(BinaryWriter bw, LazySerializer.Header header)
		{
			bw.Write(header.sdataOffset);
			bw.Write(header.version);
			for (int i = 0; i < 15; i++)
			{
				bw.Write(0);
			}
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x00020A58 File Offset: 0x0001EC58
		private static void WriteSerializerData(BinaryWriter bw, LazySerializer.SerializerData sd)
		{
			long position = bw.BaseStream.Position;
			bw.BaseStream.Position = 0L;
			bw.Write(position);
			bw.BaseStream.Position = position;
			if (sd.strings == null)
			{
				bw.Write(0);
				return;
			}
			bw.Write(sd.strings.Count);
			foreach (string text in sd.strings)
			{
				LazySerializer.WriteStringToStream(text, bw, true);
			}
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x00020AF8 File Offset: 0x0001ECF8
		private static LazySerializer.SerializerData ReadSerializerData(BinaryReader br)
		{
			long num = br.ReadInt64();
			long position = br.BaseStream.Position;
			br.BaseStream.Position = num;
			LazySerializer.SerializerData serializerData = new LazySerializer.SerializerData();
			int num2 = br.ReadInt32();
			for (int i = 0; i < num2; i++)
			{
				string text = LazySerializer.ReadStringFromStream(br, true);
				serializerData.strings.Add(text);
			}
			br.BaseStream.Position = position;
			return serializerData;
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x00020B64 File Offset: 0x0001ED64
		private static int GetStableHashCode(this string str)
		{
			int num;
			if (LazySerializer.stringHashes.TryGetValue(str, out num))
			{
				return num;
			}
			int num2 = 5381;
			int num3 = num2;
			int num4 = 0;
			while (num4 < str.Length && str[num4] != '\0')
			{
				num2 = ((num2 << 5) + num2) ^ (int)str[num4];
				if (num4 == str.Length - 1 || str[num4 + 1] == '\0')
				{
					break;
				}
				num3 = ((num3 << 5) + num3) ^ (int)str[num4 + 1];
				num4 += 2;
			}
			num = num2 + num3 * 1566083941;
			LazySerializer.stringHashes.Add(str, num);
			return num;
		}

		// Token: 0x0400039A RID: 922
		private const int VERSION = 1;

		// Token: 0x0400039B RID: 923
		private static readonly Type TYPE_SERIALIZED_FIELD = typeof(SerializeField);

		// Token: 0x0400039C RID: 924
		private static readonly Type TYPE_LAZY_SERIALIZE = typeof(LazySerialize);

		// Token: 0x0400039D RID: 925
		private static readonly Type TYPE_ODIN_SERIALIZE = typeof(OdinSerializeAttribute);

		// Token: 0x0400039E RID: 926
		private static readonly Type TYPE_LAZY_DONT_SERIALIZE = typeof(LazyDontSerialize);

		// Token: 0x0400039F RID: 927
		private static readonly Type TYPE_INTERFACE = typeof(ILazyCustomSerialize);

		// Token: 0x040003A0 RID: 928
		private static readonly Type TYPE_COMPONENT = typeof(Component);

		// Token: 0x040003A1 RID: 929
		private static readonly Type TYPE_MONOBEHAVIOUR = typeof(MonoBehaviour);

		// Token: 0x040003A2 RID: 930
		private static Dictionary<Type, List<FieldInfo>> fieldsCache = new Dictionary<Type, List<FieldInfo>>();

		// Token: 0x040003A3 RID: 931
		private static Dictionary<Type, Dictionary<int, FieldInfo>> fieldsHashesCache = new Dictionary<Type, Dictionary<int, FieldInfo>>();

		// Token: 0x040003A4 RID: 932
		private static Dictionary<string, int> stringHashes = new Dictionary<string, int>();

		// Token: 0x040003A5 RID: 933
		private static char[] buffer = new char[10240];

		// Token: 0x040003A6 RID: 934
		private static StringBuilder sb = new StringBuilder();

		// Token: 0x020001EC RID: 492
		private enum FieldType
		{
			// Token: 0x0400066F RID: 1647
			NullValue,
			// Token: 0x04000670 RID: 1648
			BoolTrue,
			// Token: 0x04000671 RID: 1649
			BoolFalse,
			// Token: 0x04000672 RID: 1650
			Int32,
			// Token: 0x04000673 RID: 1651
			Int64,
			// Token: 0x04000674 RID: 1652
			Single,
			// Token: 0x04000675 RID: 1653
			Double,
			// Token: 0x04000676 RID: 1654
			Byte,
			// Token: 0x04000677 RID: 1655
			Char,
			// Token: 0x04000678 RID: 1656
			String,
			// Token: 0x04000679 RID: 1657
			StringIndexer,
			// Token: 0x0400067A RID: 1658
			StringEmpty,
			// Token: 0x0400067B RID: 1659
			Json,
			// Token: 0x0400067C RID: 1660
			Vector2,
			// Token: 0x0400067D RID: 1661
			Vector3,
			// Token: 0x0400067E RID: 1662
			Quaternion,
			// Token: 0x0400067F RID: 1663
			Int32_0,
			// Token: 0x04000680 RID: 1664
			Int32_1,
			// Token: 0x04000681 RID: 1665
			Single_0,
			// Token: 0x04000682 RID: 1666
			Single_1,
			// Token: 0x04000683 RID: 1667
			Vector2_00,
			// Token: 0x04000684 RID: 1668
			Vector2_11,
			// Token: 0x04000685 RID: 1669
			Vector3_000,
			// Token: 0x04000686 RID: 1670
			Vector3_111,
			// Token: 0x04000687 RID: 1671
			Quaternion_0001,
			// Token: 0x04000688 RID: 1672
			GenericList = 100,
			// Token: 0x04000689 RID: 1673
			Array,
			// Token: 0x0400068A RID: 1674
			ByteArray,
			// Token: 0x0400068B RID: 1675
			LazySerialized = 250
		}

		// Token: 0x020001ED RID: 493
		public class Header
		{
			// Token: 0x0400068C RID: 1676
			public int version = 1;

			// Token: 0x0400068D RID: 1677
			public long sdataOffset;
		}

		// Token: 0x020001EE RID: 494
		private class SerializerData
		{
			// Token: 0x0400068E RID: 1678
			public List<string> strings = new List<string>();
		}
	}
}
