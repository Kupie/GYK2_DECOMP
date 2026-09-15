using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x020000FF RID: 255
	public class JSONObject
	{
		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000494 RID: 1172 RVA: 0x00018145 File Offset: 0x00016345
		public bool isContainer
		{
			get
			{
				return this.type == JSONObject.Type.ARRAY || this.type == JSONObject.Type.OBJECT;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000495 RID: 1173 RVA: 0x0001815B File Offset: 0x0001635B
		public int Count
		{
			get
			{
				if (this.list == null)
				{
					return -1;
				}
				return this.list.Count;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000496 RID: 1174 RVA: 0x00018172 File Offset: 0x00016372
		public float f
		{
			get
			{
				return (float)this.n;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000497 RID: 1175 RVA: 0x0001817B File Offset: 0x0001637B
		public int i
		{
			get
			{
				return Mathf.RoundToInt(this.f);
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000498 RID: 1176 RVA: 0x00018188 File Offset: 0x00016388
		public static JSONObject nullJO
		{
			get
			{
				return JSONObject.Create(JSONObject.Type.NULL);
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000499 RID: 1177 RVA: 0x00018190 File Offset: 0x00016390
		public static JSONObject obj
		{
			get
			{
				return JSONObject.Create(JSONObject.Type.OBJECT);
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600049A RID: 1178 RVA: 0x00018198 File Offset: 0x00016398
		public static JSONObject arr
		{
			get
			{
				return JSONObject.Create(JSONObject.Type.ARRAY);
			}
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x000181A0 File Offset: 0x000163A0
		public JSONObject(JSONObject.Type t)
		{
			this.type = t;
			if (t != JSONObject.Type.OBJECT)
			{
				if (t == JSONObject.Type.ARRAY)
				{
					this.list = new List<JSONObject>();
					return;
				}
			}
			else
			{
				this.list = new List<JSONObject>();
				this.keys = new List<string>();
			}
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x000181D9 File Offset: 0x000163D9
		public JSONObject(bool b)
		{
			this.type = JSONObject.Type.BOOL;
			this.b = b;
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x000181EF File Offset: 0x000163EF
		public JSONObject(double d)
		{
			this.type = JSONObject.Type.NUMBER;
			this.n = d;
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x00018208 File Offset: 0x00016408
		public JSONObject(Dictionary<string, string> dic)
		{
			this.type = JSONObject.Type.OBJECT;
			this.keys = new List<string>();
			this.list = new List<JSONObject>();
			foreach (KeyValuePair<string, string> keyValuePair in dic)
			{
				this.keys.Add(keyValuePair.Key);
				this.list.Add(JSONObject.CreateStringObject(keyValuePair.Value));
			}
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x0001829C File Offset: 0x0001649C
		public JSONObject(Dictionary<string, JSONObject> dic)
		{
			this.type = JSONObject.Type.OBJECT;
			this.keys = new List<string>();
			this.list = new List<JSONObject>();
			foreach (KeyValuePair<string, JSONObject> keyValuePair in dic)
			{
				this.keys.Add(keyValuePair.Key);
				this.list.Add(keyValuePair.Value);
			}
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x0001832C File Offset: 0x0001652C
		public JSONObject(JSONObject.AddJSONConents content)
		{
			content(this);
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0001833B File Offset: 0x0001653B
		public JSONObject(JSONObject[] objs)
		{
			this.type = JSONObject.Type.ARRAY;
			this.list = new List<JSONObject>(objs);
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x00018356 File Offset: 0x00016556
		public static JSONObject StringObject(string val)
		{
			return JSONObject.CreateStringObject(val);
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00018360 File Offset: 0x00016560
		public void Absorb(JSONObject obj)
		{
			this.list.AddRange(obj.list);
			this.keys.AddRange(obj.keys);
			this.str = obj.str;
			this.n = obj.n;
			this.b = obj.b;
			this.type = obj.type;
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x000183BF File Offset: 0x000165BF
		public static JSONObject Create()
		{
			return new JSONObject();
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x000183C8 File Offset: 0x000165C8
		public static JSONObject Create(JSONObject.Type t)
		{
			JSONObject jsonobject = JSONObject.Create();
			jsonobject.type = t;
			if (t != JSONObject.Type.OBJECT)
			{
				if (t == JSONObject.Type.ARRAY)
				{
					jsonobject.list = new List<JSONObject>();
				}
			}
			else
			{
				jsonobject.list = new List<JSONObject>();
				jsonobject.keys = new List<string>();
			}
			return jsonobject;
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x0001840E File Offset: 0x0001660E
		public static JSONObject Create(bool val)
		{
			JSONObject jsonobject = JSONObject.Create();
			jsonobject.type = JSONObject.Type.BOOL;
			jsonobject.b = val;
			return jsonobject;
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00018423 File Offset: 0x00016623
		public static JSONObject Create(float val)
		{
			JSONObject jsonobject = JSONObject.Create();
			jsonobject.type = JSONObject.Type.NUMBER;
			jsonobject.n = (double)val;
			return jsonobject;
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x00018439 File Offset: 0x00016639
		public static JSONObject Create(int val)
		{
			JSONObject jsonobject = JSONObject.Create();
			jsonobject.type = JSONObject.Type.NUMBER;
			jsonobject.n = (double)val;
			return jsonobject;
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x0001844F File Offset: 0x0001664F
		public static JSONObject CreateStringObject(string val)
		{
			JSONObject jsonobject = JSONObject.Create();
			jsonobject.type = JSONObject.Type.STRING;
			jsonobject.str = val;
			return jsonobject;
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x00018464 File Offset: 0x00016664
		public static JSONObject CreateBakedObject(string val)
		{
			JSONObject jsonobject = JSONObject.Create();
			jsonobject.type = JSONObject.Type.BAKED;
			jsonobject.str = val;
			return jsonobject;
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x00018479 File Offset: 0x00016679
		public static JSONObject Create(string val, int maxDepth = -2, bool storeExcessLevels = false, bool strict = false)
		{
			JSONObject jsonobject = JSONObject.Create();
			jsonobject.Parse(val, maxDepth, storeExcessLevels, strict);
			return jsonobject;
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0001848C File Offset: 0x0001668C
		public static JSONObject Create(JSONObject.AddJSONConents content)
		{
			JSONObject jsonobject = JSONObject.Create();
			content(jsonobject);
			return jsonobject;
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x000184A8 File Offset: 0x000166A8
		public static JSONObject Create(Dictionary<string, string> dic)
		{
			JSONObject jsonobject = JSONObject.Create();
			jsonobject.type = JSONObject.Type.OBJECT;
			jsonobject.keys = new List<string>();
			jsonobject.list = new List<JSONObject>();
			foreach (KeyValuePair<string, string> keyValuePair in dic)
			{
				jsonobject.keys.Add(keyValuePair.Key);
				jsonobject.list.Add(JSONObject.CreateStringObject(keyValuePair.Value));
			}
			return jsonobject;
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x0001853C File Offset: 0x0001673C
		public JSONObject()
		{
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00018544 File Offset: 0x00016744
		public JSONObject(string str, int maxDepth = -2, bool storeExcessLevels = false, bool strict = false)
		{
			this.Parse(str, maxDepth, storeExcessLevels, strict);
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x00018558 File Offset: 0x00016758
		private void Parse(string str, int maxDepth = -2, bool storeExcessLevels = false, bool strict = false)
		{
			if (string.IsNullOrEmpty(str))
			{
				this.type = JSONObject.Type.NULL;
				return;
			}
			str = str.Trim(JSONObject.WHITESPACE);
			if (strict && str[0] != '[' && str[0] != '{')
			{
				this.type = JSONObject.Type.NULL;
				global::UnityEngine.Debug.LogWarning("Improper (strict) JSON formatting.  First character must be [ or {");
				return;
			}
			if (str.Length <= 0)
			{
				this.type = JSONObject.Type.NULL;
				return;
			}
			if (string.Compare(str, "true", true) == 0)
			{
				this.type = JSONObject.Type.BOOL;
				this.b = true;
				return;
			}
			if (string.Compare(str, "false", true) == 0)
			{
				this.type = JSONObject.Type.BOOL;
				this.b = false;
				return;
			}
			if (string.Compare(str, "null", true) == 0)
			{
				this.type = JSONObject.Type.NULL;
				return;
			}
			if (str == "\"INFINITY\"")
			{
				this.type = JSONObject.Type.NUMBER;
				this.n = double.PositiveInfinity;
				return;
			}
			if (str == "\"NEGINFINITY\"")
			{
				this.type = JSONObject.Type.NUMBER;
				this.n = double.NegativeInfinity;
				return;
			}
			if (str == "\"NaN\"")
			{
				this.type = JSONObject.Type.NUMBER;
				this.n = double.NaN;
				return;
			}
			if (str[0] == '"')
			{
				this.type = JSONObject.Type.STRING;
				this.str = str.Substring(1, str.Length - 2);
				return;
			}
			int num = 1;
			int num2 = 0;
			char c = str[num2];
			if (c != '[')
			{
				if (c != '{')
				{
					try
					{
						this.n = Convert.ToDouble(str, CultureInfo.InvariantCulture);
						this.type = JSONObject.Type.NUMBER;
					}
					catch (FormatException)
					{
						this.type = JSONObject.Type.NULL;
						global::UnityEngine.Debug.LogWarning("improper JSON formatting:" + str);
					}
					return;
				}
				this.type = JSONObject.Type.OBJECT;
				this.keys = new List<string>();
				this.list = new List<JSONObject>();
			}
			else
			{
				this.type = JSONObject.Type.ARRAY;
				this.list = new List<JSONObject>();
			}
			string text = "";
			bool flag = false;
			bool flag2 = false;
			int num3 = 0;
			while (++num2 < str.Length)
			{
				if (Array.IndexOf<char>(JSONObject.WHITESPACE, str[num2]) <= -1)
				{
					if (str[num2] == '\\')
					{
						num2++;
					}
					else
					{
						if (str[num2] == '"')
						{
							if (flag)
							{
								if (!flag2 && num3 == 0 && this.type == JSONObject.Type.OBJECT)
								{
									text = str.Substring(num + 1, num2 - num - 1);
								}
								flag = false;
							}
							else
							{
								if (num3 == 0 && this.type == JSONObject.Type.OBJECT)
								{
									num = num2;
								}
								flag = true;
							}
						}
						if (!flag)
						{
							if (this.type == JSONObject.Type.OBJECT && num3 == 0 && str[num2] == ':')
							{
								num = num2 + 1;
								flag2 = true;
							}
							if (str[num2] == '[' || str[num2] == '{')
							{
								num3++;
							}
							else if (str[num2] == ']' || str[num2] == '}')
							{
								num3--;
							}
							if ((str[num2] == ',' && num3 == 0) || num3 < 0)
							{
								flag2 = false;
								string text2 = str.Substring(num, num2 - num).Trim(JSONObject.WHITESPACE);
								if (text2.Length > 0)
								{
									if (this.type == JSONObject.Type.OBJECT)
									{
										this.keys.Add(text);
									}
									if (maxDepth != -1)
									{
										this.list.Add(JSONObject.Create(text2, (maxDepth < -1) ? (-2) : (maxDepth - 1), false, false));
									}
									else if (storeExcessLevels)
									{
										this.list.Add(JSONObject.CreateBakedObject(text2));
									}
								}
								num = num2 + 1;
							}
						}
					}
				}
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060004B1 RID: 1201 RVA: 0x000188B8 File Offset: 0x00016AB8
		public bool IsNumber
		{
			get
			{
				return this.type == JSONObject.Type.NUMBER;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x000188C3 File Offset: 0x00016AC3
		public bool IsNull
		{
			get
			{
				return this.type == JSONObject.Type.NULL;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060004B3 RID: 1203 RVA: 0x000188CE File Offset: 0x00016ACE
		public bool IsString
		{
			get
			{
				return this.type == JSONObject.Type.STRING;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x000188D9 File Offset: 0x00016AD9
		public bool IsBool
		{
			get
			{
				return this.type == JSONObject.Type.BOOL;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060004B5 RID: 1205 RVA: 0x000188E4 File Offset: 0x00016AE4
		public bool IsArray
		{
			get
			{
				return this.type == JSONObject.Type.ARRAY;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060004B6 RID: 1206 RVA: 0x000188EF File Offset: 0x00016AEF
		public bool IsObject
		{
			get
			{
				return this.type == JSONObject.Type.OBJECT || this.type == JSONObject.Type.BAKED;
			}
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00018905 File Offset: 0x00016B05
		public void Add(bool val)
		{
			this.Add(JSONObject.Create(val));
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x00018913 File Offset: 0x00016B13
		public void Add(float val)
		{
			this.Add(JSONObject.Create(val));
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x00018921 File Offset: 0x00016B21
		public void Add(int val)
		{
			this.Add(JSONObject.Create(val));
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0001892F File Offset: 0x00016B2F
		public void Add(string str)
		{
			this.Add(JSONObject.CreateStringObject(str));
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0001893D File Offset: 0x00016B3D
		public void Add(JSONObject.AddJSONConents content)
		{
			this.Add(JSONObject.Create(content));
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x0001894B File Offset: 0x00016B4B
		public void Add(JSONObject obj)
		{
			if (obj)
			{
				if (this.type != JSONObject.Type.ARRAY)
				{
					this.type = JSONObject.Type.ARRAY;
					if (this.list == null)
					{
						this.list = new List<JSONObject>();
					}
				}
				this.list.Add(obj);
			}
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x00018984 File Offset: 0x00016B84
		public void AddField(string name, bool val)
		{
			this.AddField(name, JSONObject.Create(val));
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x00018993 File Offset: 0x00016B93
		public void AddField(string name, float val)
		{
			this.AddField(name, JSONObject.Create(val));
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x000189A2 File Offset: 0x00016BA2
		public void AddField(string name, int val)
		{
			this.AddField(name, JSONObject.Create(val));
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x000189B1 File Offset: 0x00016BB1
		public void AddField(string name, JSONObject.AddJSONConents content)
		{
			this.AddField(name, JSONObject.Create(content));
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x000189C0 File Offset: 0x00016BC0
		public void AddField(string name, string val)
		{
			this.AddField(name, JSONObject.CreateStringObject(val));
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x000189D0 File Offset: 0x00016BD0
		public void AddField(string name, JSONObject obj)
		{
			if (obj)
			{
				if (this.type != JSONObject.Type.OBJECT)
				{
					if (this.keys == null)
					{
						this.keys = new List<string>();
					}
					if (this.type == JSONObject.Type.ARRAY)
					{
						for (int i = 0; i < this.list.Count; i++)
						{
							this.keys.Add(i.ToString() ?? "");
						}
					}
					else if (this.list == null)
					{
						this.list = new List<JSONObject>();
					}
					this.type = JSONObject.Type.OBJECT;
				}
				this.keys.Add(name);
				this.list.Add(obj);
			}
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x00018A72 File Offset: 0x00016C72
		public void SetField(string name, string val)
		{
			this.SetField(name, JSONObject.CreateStringObject(val));
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x00018A81 File Offset: 0x00016C81
		public void SetField(string name, bool val)
		{
			this.SetField(name, JSONObject.Create(val));
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x00018A90 File Offset: 0x00016C90
		public void SetField(string name, float val)
		{
			this.SetField(name, JSONObject.Create(val));
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x00018A9F File Offset: 0x00016C9F
		public void SetField(string name, int val)
		{
			this.SetField(name, JSONObject.Create(val));
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x00018AAE File Offset: 0x00016CAE
		public void SetField(string name, JSONObject obj)
		{
			if (this.HasField(name))
			{
				this.list.Remove(this[name]);
				this.keys.Remove(name);
			}
			this.AddField(name, obj);
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x00018AE1 File Offset: 0x00016CE1
		public void RemoveField(string name)
		{
			if (this.keys.IndexOf(name) > -1)
			{
				this.list.RemoveAt(this.keys.IndexOf(name));
				this.keys.Remove(name);
			}
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x00018B16 File Offset: 0x00016D16
		public bool GetField(ref bool field, string name, bool fallback)
		{
			if (this.GetField(ref field, name, null))
			{
				return true;
			}
			field = fallback;
			return false;
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x00018B2C File Offset: 0x00016D2C
		public bool GetField(ref bool field, string name, JSONObject.FieldNotFound fail = null)
		{
			if (this.type == JSONObject.Type.OBJECT)
			{
				int num = this.keys.IndexOf(name);
				if (num >= 0)
				{
					field = this.list[num].b;
					return true;
				}
			}
			if (fail != null)
			{
				fail(name);
			}
			return false;
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x00018B73 File Offset: 0x00016D73
		public bool GetField(ref double field, string name, double fallback)
		{
			if (this.GetField(ref field, name, null))
			{
				return true;
			}
			field = fallback;
			return false;
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x00018B88 File Offset: 0x00016D88
		public bool GetField(ref double field, string name, JSONObject.FieldNotFound fail = null)
		{
			if (this.type == JSONObject.Type.OBJECT)
			{
				int num = this.keys.IndexOf(name);
				if (num >= 0)
				{
					field = this.list[num].n;
					return true;
				}
			}
			if (fail != null)
			{
				fail(name);
			}
			return false;
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x00018BCF File Offset: 0x00016DCF
		public bool GetField(ref int field, string name, int fallback)
		{
			if (this.GetField(ref field, name, null))
			{
				return true;
			}
			field = fallback;
			return false;
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00018BE4 File Offset: 0x00016DE4
		public bool GetField(ref int field, string name, JSONObject.FieldNotFound fail = null)
		{
			if (this.IsObject)
			{
				int num = this.keys.IndexOf(name);
				if (num >= 0)
				{
					field = (int)this.list[num].n;
					return true;
				}
			}
			if (fail != null)
			{
				fail(name);
			}
			return false;
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x00018C2B File Offset: 0x00016E2B
		public bool GetField(ref uint field, string name, uint fallback)
		{
			if (this.GetField(ref field, name, null))
			{
				return true;
			}
			field = fallback;
			return false;
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x00018C40 File Offset: 0x00016E40
		public bool GetField(ref uint field, string name, JSONObject.FieldNotFound fail = null)
		{
			if (this.IsObject)
			{
				int num = this.keys.IndexOf(name);
				if (num >= 0)
				{
					field = (uint)this.list[num].n;
					return true;
				}
			}
			if (fail != null)
			{
				fail(name);
			}
			return false;
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x00018C87 File Offset: 0x00016E87
		public bool GetField(ref string field, string name, string fallback)
		{
			if (this.GetField(ref field, name, null))
			{
				return true;
			}
			field = fallback;
			return false;
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00018C9C File Offset: 0x00016E9C
		public bool GetField(ref string field, string name, JSONObject.FieldNotFound fail = null)
		{
			if (this.IsObject)
			{
				int num = this.keys.IndexOf(name);
				if (num >= 0)
				{
					field = this.list[num].str;
					return true;
				}
			}
			if (fail != null)
			{
				fail(name);
			}
			return false;
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00018CE4 File Offset: 0x00016EE4
		public void GetField(string name, JSONObject.GetFieldResponse response, JSONObject.FieldNotFound fail = null)
		{
			if (response != null && this.IsObject)
			{
				int num = this.keys.IndexOf(name);
				if (num >= 0)
				{
					response(this.list[num]);
					return;
				}
			}
			if (fail != null)
			{
				fail(name);
			}
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x00018D2C File Offset: 0x00016F2C
		public JSONObject GetField(string name)
		{
			if (this.IsObject)
			{
				for (int i = 0; i < this.keys.Count; i++)
				{
					if (this.keys[i] == name)
					{
						return this.list[i];
					}
				}
			}
			return null;
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x00018D7C File Offset: 0x00016F7C
		public bool HasFields(string[] names)
		{
			if (!this.IsObject)
			{
				return false;
			}
			for (int i = 0; i < names.Length; i++)
			{
				if (!this.keys.Contains(names[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x00018DB4 File Offset: 0x00016FB4
		public bool HasField(string name)
		{
			if (!this.IsObject)
			{
				return false;
			}
			for (int i = 0; i < this.keys.Count; i++)
			{
				if (this.keys[i] == name)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x00018DF8 File Offset: 0x00016FF8
		public void Clear()
		{
			this.type = JSONObject.Type.NULL;
			if (this.list != null)
			{
				this.list.Clear();
			}
			if (this.keys != null)
			{
				this.keys.Clear();
			}
			this.str = "";
			this.n = 0.0;
			this.b = false;
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00018E53 File Offset: 0x00017053
		public JSONObject Copy()
		{
			return JSONObject.Create(this.Print(false), -2, false, false);
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x00018E65 File Offset: 0x00017065
		public void Merge(JSONObject obj)
		{
			JSONObject.MergeRecur(this, obj);
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x00018E70 File Offset: 0x00017070
		private static void MergeRecur(JSONObject left, JSONObject right)
		{
			if (left.type == JSONObject.Type.NULL)
			{
				left.Absorb(right);
				return;
			}
			if (left.type == JSONObject.Type.OBJECT && right.type == JSONObject.Type.OBJECT)
			{
				for (int i = 0; i < right.list.Count; i++)
				{
					string text = right.keys[i];
					if (right[i].isContainer)
					{
						if (left.HasField(text))
						{
							JSONObject.MergeRecur(left[text], right[i]);
						}
						else
						{
							left.AddField(text, right[i]);
						}
					}
					else if (left.HasField(text))
					{
						left.SetField(text, right[i]);
					}
					else
					{
						left.AddField(text, right[i]);
					}
				}
				return;
			}
			if (left.type == JSONObject.Type.ARRAY && right.type == JSONObject.Type.ARRAY)
			{
				if (right.Count > left.Count)
				{
					global::UnityEngine.Debug.LogError("Cannot merge arrays when right object has more elements");
					return;
				}
				for (int j = 0; j < right.list.Count; j++)
				{
					if (left[j].type == right[j].type)
					{
						if (left[j].isContainer)
						{
							JSONObject.MergeRecur(left[j], right[j]);
						}
						else
						{
							left[j] = right[j];
						}
					}
				}
			}
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00018FBE File Offset: 0x000171BE
		public void Bake()
		{
			if (this.type != JSONObject.Type.BAKED)
			{
				this.str = this.Print(false);
				this.type = JSONObject.Type.BAKED;
			}
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x00018FDD File Offset: 0x000171DD
		public IEnumerable BakeAsync()
		{
			if (this.type != JSONObject.Type.BAKED)
			{
				foreach (string text in this.PrintAsync(false))
				{
					if (text == null)
					{
						yield return text;
					}
					else
					{
						this.str = text;
					}
				}
				IEnumerator<string> enumerator = null;
				this.type = JSONObject.Type.BAKED;
			}
			yield break;
			yield break;
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x00018FF0 File Offset: 0x000171F0
		public string Print(bool pretty = false)
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.Stringify(0, stringBuilder, pretty);
			return stringBuilder.ToString();
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x00019012 File Offset: 0x00017212
		public IEnumerable<string> PrintAsync(bool pretty = false)
		{
			StringBuilder builder = new StringBuilder();
			JSONObject.printWatch.Reset();
			JSONObject.printWatch.Start();
			foreach (object obj in this.StringifyAsync(0, builder, pretty))
			{
				IEnumerable enumerable = (IEnumerable)obj;
				yield return null;
			}
			IEnumerator enumerator = null;
			yield return builder.ToString();
			yield break;
			yield break;
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x00019029 File Offset: 0x00017229
		private IEnumerable StringifyAsync(int depth, StringBuilder builder, bool pretty = false)
		{
			int num = depth;
			depth = num + 1;
			if (num > 100)
			{
				global::UnityEngine.Debug.Log("reached max depth!");
				yield break;
			}
			if (JSONObject.printWatch.Elapsed.TotalSeconds > 0.00800000037997961)
			{
				JSONObject.printWatch.Reset();
				yield return null;
				JSONObject.printWatch.Start();
			}
			switch (this.type)
			{
			case JSONObject.Type.NULL:
				builder.Append("null");
				break;
			case JSONObject.Type.STRING:
				builder.AppendFormat("\"{0}\"", this.str);
				break;
			case JSONObject.Type.NUMBER:
				if (double.IsInfinity(this.n))
				{
					builder.Append("\"INFINITY\"");
				}
				else if (double.IsNegativeInfinity(this.n))
				{
					builder.Append("\"NEGINFINITY\"");
				}
				else if (double.IsNaN(this.n))
				{
					builder.Append("\"NaN\"");
				}
				else
				{
					builder.Append(this.n.ToString(CultureInfo.InvariantCulture));
				}
				break;
			case JSONObject.Type.OBJECT:
				builder.Append("{");
				if (this.list.Count > 0)
				{
					if (pretty)
					{
						builder.Append("\n");
					}
					for (int i = 0; i < this.list.Count; i = num + 1)
					{
						string text = this.keys[i];
						JSONObject jsonobject = this.list[i];
						if (jsonobject)
						{
							if (pretty)
							{
								for (int j = 0; j < depth; j++)
								{
									builder.Append("\t");
								}
							}
							builder.AppendFormat("\"{0}\":", text);
							foreach (object obj in jsonobject.StringifyAsync(depth, builder, pretty))
							{
								IEnumerable enumerable = (IEnumerable)obj;
								yield return enumerable;
							}
							IEnumerator enumerator = null;
							builder.Append(",");
							if (pretty)
							{
								builder.Append("\n");
							}
						}
						num = i;
					}
					if (pretty)
					{
						builder.Length -= 2;
					}
					else
					{
						num = builder.Length;
						builder.Length = num - 1;
					}
				}
				if (pretty && this.list.Count > 0)
				{
					builder.Append("\n");
					for (int k = 0; k < depth - 1; k++)
					{
						builder.Append("\t");
					}
				}
				builder.Append("}");
				break;
			case JSONObject.Type.ARRAY:
				builder.Append("[");
				if (this.list.Count > 0)
				{
					if (pretty)
					{
						builder.Append("\n");
					}
					for (int i = 0; i < this.list.Count; i = num + 1)
					{
						if (this.list[i])
						{
							if (pretty)
							{
								for (int l = 0; l < depth; l++)
								{
									builder.Append("\t");
								}
							}
							foreach (object obj2 in this.list[i].StringifyAsync(depth, builder, pretty))
							{
								IEnumerable enumerable2 = (IEnumerable)obj2;
								yield return enumerable2;
							}
							IEnumerator enumerator = null;
							builder.Append(",");
							if (pretty)
							{
								builder.Append("\n");
							}
						}
						num = i;
					}
					if (pretty)
					{
						builder.Length -= 2;
					}
					else
					{
						num = builder.Length;
						builder.Length = num - 1;
					}
				}
				if (pretty && this.list.Count > 0)
				{
					builder.Append("\n");
					for (int m = 0; m < depth - 1; m++)
					{
						builder.Append("\t");
					}
				}
				builder.Append("]");
				break;
			case JSONObject.Type.BOOL:
				if (this.b)
				{
					builder.Append("true");
				}
				else
				{
					builder.Append("false");
				}
				break;
			case JSONObject.Type.BAKED:
				builder.Append(this.str);
				break;
			}
			yield break;
			yield break;
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x00019050 File Offset: 0x00017250
		private void Stringify(int depth, StringBuilder builder, bool pretty = false)
		{
			if (depth++ > 100)
			{
				global::UnityEngine.Debug.Log("reached max depth!");
				return;
			}
			switch (this.type)
			{
			case JSONObject.Type.NULL:
				builder.Append("null");
				return;
			case JSONObject.Type.STRING:
				builder.AppendFormat("\"{0}\"", this.str);
				return;
			case JSONObject.Type.NUMBER:
				if (double.IsInfinity(this.n))
				{
					builder.Append("\"INFINITY\"");
					return;
				}
				if (double.IsNegativeInfinity(this.n))
				{
					builder.Append("\"NEGINFINITY\"");
					return;
				}
				if (double.IsNaN(this.n))
				{
					builder.Append("\"NaN\"");
					return;
				}
				builder.Append(this.n.ToString(CultureInfo.InvariantCulture));
				return;
			case JSONObject.Type.OBJECT:
				builder.Append("{");
				if (this.list.Count > 0)
				{
					if (pretty)
					{
						builder.Append("\n");
					}
					for (int i = 0; i < this.list.Count; i++)
					{
						string text = this.keys[i];
						JSONObject jsonobject = this.list[i];
						if (jsonobject)
						{
							if (pretty)
							{
								for (int j = 0; j < depth; j++)
								{
									builder.Append("\t");
								}
							}
							builder.AppendFormat("\"{0}\":", text);
							jsonobject.Stringify(depth, builder, pretty);
							builder.Append(",");
							if (pretty)
							{
								builder.Append("\n");
							}
						}
					}
					if (pretty)
					{
						builder.Length -= 2;
					}
					else
					{
						int num = builder.Length;
						builder.Length = num - 1;
					}
				}
				if (pretty && this.list.Count > 0)
				{
					builder.Append("\n");
					for (int k = 0; k < depth - 1; k++)
					{
						builder.Append("\t");
					}
				}
				builder.Append("}");
				return;
			case JSONObject.Type.ARRAY:
				builder.Append("[");
				if (this.list.Count > 0)
				{
					if (pretty)
					{
						builder.Append("\n");
					}
					for (int l = 0; l < this.list.Count; l++)
					{
						if (this.list[l])
						{
							if (pretty)
							{
								for (int m = 0; m < depth; m++)
								{
									builder.Append("\t");
								}
							}
							this.list[l].Stringify(depth, builder, pretty);
							builder.Append(",");
							if (pretty)
							{
								builder.Append("\n");
							}
						}
					}
					if (pretty)
					{
						builder.Length -= 2;
					}
					else
					{
						int num = builder.Length;
						builder.Length = num - 1;
					}
				}
				if (pretty && this.list.Count > 0)
				{
					builder.Append("\n");
					for (int n = 0; n < depth - 1; n++)
					{
						builder.Append("\t");
					}
				}
				builder.Append("]");
				return;
			case JSONObject.Type.BOOL:
				if (this.b)
				{
					builder.Append("true");
					return;
				}
				builder.Append("false");
				return;
			case JSONObject.Type.BAKED:
				builder.Append(this.str);
				return;
			default:
				return;
			}
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0001938C File Offset: 0x0001758C
		public static implicit operator WWWForm(JSONObject obj)
		{
			WWWForm wwwform = new WWWForm();
			for (int i = 0; i < obj.list.Count; i++)
			{
				string text = i.ToString() ?? "";
				if (obj.type == JSONObject.Type.OBJECT)
				{
					text = obj.keys[i];
				}
				string text2 = obj.list[i].ToString();
				if (obj.list[i].type == JSONObject.Type.STRING)
				{
					text2 = text2.Replace("\"", "");
				}
				wwwform.AddField(text, text2);
			}
			return wwwform;
		}

		// Token: 0x170000C5 RID: 197
		public JSONObject this[int index]
		{
			get
			{
				if (this.list.Count > index)
				{
					return this.list[index];
				}
				return null;
			}
			set
			{
				if (this.list.Count > index)
				{
					this.list[index] = value;
				}
			}
		}

		// Token: 0x170000C6 RID: 198
		public JSONObject this[string index]
		{
			get
			{
				return this.GetField(index);
			}
			set
			{
				this.SetField(index, value);
			}
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x0001946A File Offset: 0x0001766A
		public override string ToString()
		{
			return this.Print(false);
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00019473 File Offset: 0x00017673
		public string ToString(bool pretty)
		{
			return this.Print(pretty);
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0001947C File Offset: 0x0001767C
		public Dictionary<string, string> ToDictionary()
		{
			if (this.type == JSONObject.Type.OBJECT)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				int i = 0;
				while (i < this.list.Count)
				{
					JSONObject jsonobject = this.list[i];
					switch (jsonobject.type)
					{
					case JSONObject.Type.STRING:
						dictionary.Add(this.keys[i], jsonobject.str);
						break;
					case JSONObject.Type.NUMBER:
						dictionary.Add(this.keys[i], jsonobject.n.ToString() ?? "");
						break;
					case JSONObject.Type.OBJECT:
					case JSONObject.Type.ARRAY:
						goto IL_00B5;
					case JSONObject.Type.BOOL:
						dictionary.Add(this.keys[i], jsonobject.b.ToString() ?? "");
						break;
					default:
						goto IL_00B5;
					}
					IL_00D5:
					i++;
					continue;
					IL_00B5:
					global::UnityEngine.Debug.LogWarning("Omitting object: " + this.keys[i] + " in dictionary conversion");
					goto IL_00D5;
				}
				return dictionary;
			}
			global::UnityEngine.Debug.LogWarning("Tried to turn non-Object JSONObject into a dictionary");
			return null;
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00019580 File Offset: 0x00017780
		public static implicit operator bool(JSONObject o)
		{
			return o != null;
		}

		// Token: 0x0400023C RID: 572
		private const int MAX_DEPTH = 100;

		// Token: 0x0400023D RID: 573
		private const string INFINITY = "\"INFINITY\"";

		// Token: 0x0400023E RID: 574
		private const string NEGINFINITY = "\"NEGINFINITY\"";

		// Token: 0x0400023F RID: 575
		private const string NaN = "\"NaN\"";

		// Token: 0x04000240 RID: 576
		public static readonly char[] WHITESPACE = new char[] { ' ', '\r', '\n', '\t', '\ufeff', '\t' };

		// Token: 0x04000241 RID: 577
		public JSONObject.Type type;

		// Token: 0x04000242 RID: 578
		public List<JSONObject> list;

		// Token: 0x04000243 RID: 579
		public List<string> keys;

		// Token: 0x04000244 RID: 580
		public string str;

		// Token: 0x04000245 RID: 581
		public double n;

		// Token: 0x04000246 RID: 582
		public bool b;

		// Token: 0x04000247 RID: 583
		private const float maxFrameTime = 0.008f;

		// Token: 0x04000248 RID: 584
		private static readonly Stopwatch printWatch = new Stopwatch();

		// Token: 0x020001D5 RID: 469
		public enum Type
		{
			// Token: 0x04000634 RID: 1588
			NULL,
			// Token: 0x04000635 RID: 1589
			STRING,
			// Token: 0x04000636 RID: 1590
			NUMBER,
			// Token: 0x04000637 RID: 1591
			OBJECT,
			// Token: 0x04000638 RID: 1592
			ARRAY,
			// Token: 0x04000639 RID: 1593
			BOOL,
			// Token: 0x0400063A RID: 1594
			BAKED
		}

		// Token: 0x020001D6 RID: 470
		// (Invoke) Token: 0x06000A11 RID: 2577
		public delegate void AddJSONConents(JSONObject self);

		// Token: 0x020001D7 RID: 471
		// (Invoke) Token: 0x06000A15 RID: 2581
		public delegate void FieldNotFound(string name);

		// Token: 0x020001D8 RID: 472
		// (Invoke) Token: 0x06000A19 RID: 2585
		public delegate void GetFieldResponse(JSONObject obj);
	}
}
