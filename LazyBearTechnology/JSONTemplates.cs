using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000100 RID: 256
	public static class JSONTemplates
	{
		// Token: 0x060004EB RID: 1259 RVA: 0x000195A8 File Offset: 0x000177A8
		public static JSONObject TOJSON(object obj)
		{
			if (JSONTemplates.touched.Add(obj))
			{
				JSONObject obj2 = JSONObject.obj;
				foreach (FieldInfo fieldInfo in obj.GetType().GetFields())
				{
					JSONObject jsonobject = JSONObject.nullJO;
					if (!fieldInfo.GetValue(obj).Equals(null))
					{
						MethodInfo method = typeof(JSONTemplates).GetMethod("From" + fieldInfo.FieldType.Name);
						if (method != null)
						{
							jsonobject = (JSONObject)method.Invoke(null, new object[] { fieldInfo.GetValue(obj) });
						}
						else if (fieldInfo.FieldType == typeof(string))
						{
							jsonobject = JSONObject.CreateStringObject(fieldInfo.GetValue(obj).ToString());
						}
						else
						{
							jsonobject = JSONObject.Create(fieldInfo.GetValue(obj).ToString(), -2, false, false);
						}
					}
					if (jsonobject)
					{
						if (jsonobject.type != JSONObject.Type.NULL)
						{
							obj2.AddField(fieldInfo.Name, jsonobject);
						}
						else
						{
							Debug.LogWarning(string.Concat(new string[]
							{
								"Null for this non-null object, property ",
								fieldInfo.Name,
								" of class ",
								obj.GetType().Name,
								". Object type is ",
								fieldInfo.FieldType.Name
							}));
						}
					}
				}
				foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
				{
					JSONObject jsonobject2 = JSONObject.nullJO;
					if (!propertyInfo.GetValue(obj, null).Equals(null))
					{
						MethodInfo method2 = typeof(JSONTemplates).GetMethod("From" + propertyInfo.PropertyType.Name);
						if (method2 != null)
						{
							jsonobject2 = (JSONObject)method2.Invoke(null, new object[] { propertyInfo.GetValue(obj, null) });
						}
						else if (propertyInfo.PropertyType == typeof(string))
						{
							jsonobject2 = JSONObject.CreateStringObject(propertyInfo.GetValue(obj, null).ToString());
						}
						else
						{
							jsonobject2 = JSONObject.Create(propertyInfo.GetValue(obj, null).ToString(), -2, false, false);
						}
					}
					if (jsonobject2)
					{
						if (jsonobject2.type != JSONObject.Type.NULL)
						{
							obj2.AddField(propertyInfo.Name, jsonobject2);
						}
						else
						{
							Debug.LogWarning(string.Concat(new string[]
							{
								"Null for this non-null object, property ",
								propertyInfo.Name,
								" of class ",
								obj.GetType().Name,
								". Object type is ",
								propertyInfo.PropertyType.Name
							}));
						}
					}
				}
				return obj2;
			}
			Debug.LogWarning("trying to save the same data twice");
			return JSONObject.nullJO;
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x00019878 File Offset: 0x00017A78
		public static Vector2 ToVector2(JSONObject obj)
		{
			float num = (obj["x"] ? obj["x"].f : 0f);
			float num2 = (obj["y"] ? obj["y"].f : 0f);
			return new Vector2(num, num2);
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x000198E0 File Offset: 0x00017AE0
		public static JSONObject FromVector2(Vector2 v)
		{
			JSONObject obj = JSONObject.obj;
			if (v.x != 0f)
			{
				obj.AddField("x", v.x);
			}
			if (v.y != 0f)
			{
				obj.AddField("y", v.y);
			}
			return obj;
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x00019930 File Offset: 0x00017B30
		public static JSONObject FromVector3(Vector3 v)
		{
			JSONObject obj = JSONObject.obj;
			if (v.x != 0f)
			{
				obj.AddField("x", v.x);
			}
			if (v.y != 0f)
			{
				obj.AddField("y", v.y);
			}
			if (v.z != 0f)
			{
				obj.AddField("z", v.z);
			}
			return obj;
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x000199A0 File Offset: 0x00017BA0
		public static Vector3 ToVector3(JSONObject obj)
		{
			float num = (obj["x"] ? obj["x"].f : 0f);
			float num2 = (obj["y"] ? obj["y"].f : 0f);
			float num3 = (obj["z"] ? obj["z"].f : 0f);
			return new Vector3(num, num2, num3);
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00019A34 File Offset: 0x00017C34
		public static JSONObject FromVector4(Vector4 v)
		{
			JSONObject obj = JSONObject.obj;
			if (v.x != 0f)
			{
				obj.AddField("x", v.x);
			}
			if (v.y != 0f)
			{
				obj.AddField("y", v.y);
			}
			if (v.z != 0f)
			{
				obj.AddField("z", v.z);
			}
			if (v.w != 0f)
			{
				obj.AddField("w", v.w);
			}
			return obj;
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x00019AC0 File Offset: 0x00017CC0
		public static Vector4 ToVector4(JSONObject obj)
		{
			float num = (obj["x"] ? obj["x"].f : 0f);
			float num2 = (obj["y"] ? obj["y"].f : 0f);
			float num3 = (obj["z"] ? obj["z"].f : 0f);
			float num4 = (obj["w"] ? obj["w"].f : 0f);
			return new Vector4(num, num2, num3, num4);
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x00019B7C File Offset: 0x00017D7C
		public static JSONObject FromMatrix4X4(Matrix4x4 m)
		{
			JSONObject obj = JSONObject.obj;
			if (m.m00 != 0f)
			{
				obj.AddField("m00", m.m00);
			}
			if (m.m01 != 0f)
			{
				obj.AddField("m01", m.m01);
			}
			if (m.m02 != 0f)
			{
				obj.AddField("m02", m.m02);
			}
			if (m.m03 != 0f)
			{
				obj.AddField("m03", m.m03);
			}
			if (m.m10 != 0f)
			{
				obj.AddField("m10", m.m10);
			}
			if (m.m11 != 0f)
			{
				obj.AddField("m11", m.m11);
			}
			if (m.m12 != 0f)
			{
				obj.AddField("m12", m.m12);
			}
			if (m.m13 != 0f)
			{
				obj.AddField("m13", m.m13);
			}
			if (m.m20 != 0f)
			{
				obj.AddField("m20", m.m20);
			}
			if (m.m21 != 0f)
			{
				obj.AddField("m21", m.m21);
			}
			if (m.m22 != 0f)
			{
				obj.AddField("m22", m.m22);
			}
			if (m.m23 != 0f)
			{
				obj.AddField("m23", m.m23);
			}
			if (m.m30 != 0f)
			{
				obj.AddField("m30", m.m30);
			}
			if (m.m31 != 0f)
			{
				obj.AddField("m31", m.m31);
			}
			if (m.m32 != 0f)
			{
				obj.AddField("m32", m.m32);
			}
			if (m.m33 != 0f)
			{
				obj.AddField("m33", m.m33);
			}
			return obj;
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00019D70 File Offset: 0x00017F70
		public static Matrix4x4 ToMatrix4X4(JSONObject obj)
		{
			Matrix4x4 matrix4x = default(Matrix4x4);
			if (obj["m00"])
			{
				matrix4x.m00 = obj["m00"].f;
			}
			if (obj["m01"])
			{
				matrix4x.m01 = obj["m01"].f;
			}
			if (obj["m02"])
			{
				matrix4x.m02 = obj["m02"].f;
			}
			if (obj["m03"])
			{
				matrix4x.m03 = obj["m03"].f;
			}
			if (obj["m10"])
			{
				matrix4x.m10 = obj["m10"].f;
			}
			if (obj["m11"])
			{
				matrix4x.m11 = obj["m11"].f;
			}
			if (obj["m12"])
			{
				matrix4x.m12 = obj["m12"].f;
			}
			if (obj["m13"])
			{
				matrix4x.m13 = obj["m13"].f;
			}
			if (obj["m20"])
			{
				matrix4x.m20 = obj["m20"].f;
			}
			if (obj["m21"])
			{
				matrix4x.m21 = obj["m21"].f;
			}
			if (obj["m22"])
			{
				matrix4x.m22 = obj["m22"].f;
			}
			if (obj["m23"])
			{
				matrix4x.m23 = obj["m23"].f;
			}
			if (obj["m30"])
			{
				matrix4x.m30 = obj["m30"].f;
			}
			if (obj["m31"])
			{
				matrix4x.m31 = obj["m31"].f;
			}
			if (obj["m32"])
			{
				matrix4x.m32 = obj["m32"].f;
			}
			if (obj["m33"])
			{
				matrix4x.m33 = obj["m33"].f;
			}
			return matrix4x;
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0001A018 File Offset: 0x00018218
		public static JSONObject FromQuaternion(Quaternion q)
		{
			JSONObject obj = JSONObject.obj;
			if (q.w != 0f)
			{
				obj.AddField("w", q.w);
			}
			if (q.x != 0f)
			{
				obj.AddField("x", q.x);
			}
			if (q.y != 0f)
			{
				obj.AddField("y", q.y);
			}
			if (q.z != 0f)
			{
				obj.AddField("z", q.z);
			}
			return obj;
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0001A0A4 File Offset: 0x000182A4
		public static Quaternion ToQuaternion(JSONObject obj)
		{
			float num = (obj["x"] ? obj["x"].f : 0f);
			float num2 = (obj["y"] ? obj["y"].f : 0f);
			float num3 = (obj["z"] ? obj["z"].f : 0f);
			float num4 = (obj["w"] ? obj["w"].f : 0f);
			return new Quaternion(num, num2, num3, num4);
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0001A160 File Offset: 0x00018360
		public static JSONObject FromColor(Color c)
		{
			JSONObject obj = JSONObject.obj;
			if (c.r != 0f)
			{
				obj.AddField("r", c.r);
			}
			if (c.g != 0f)
			{
				obj.AddField("g", c.g);
			}
			if (c.b != 0f)
			{
				obj.AddField("b", c.b);
			}
			if (c.a != 0f)
			{
				obj.AddField("a", c.a);
			}
			return obj;
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0001A1EC File Offset: 0x000183EC
		public static Color ToColor(JSONObject obj)
		{
			Color color = default(Color);
			for (int i = 0; i < obj.Count; i++)
			{
				string text = obj.keys[i];
				if (!(text == "r"))
				{
					if (!(text == "g"))
					{
						if (!(text == "b"))
						{
							if (text == "a")
							{
								color.a = obj[i].f;
							}
						}
						else
						{
							color.b = obj[i].f;
						}
					}
					else
					{
						color.g = obj[i].f;
					}
				}
				else
				{
					color.r = obj[i].f;
				}
			}
			return color;
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0001A2AE File Offset: 0x000184AE
		public static JSONObject FromLayerMask(LayerMask l)
		{
			JSONObject obj = JSONObject.obj;
			obj.AddField("value", l.value);
			return obj;
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0001A2C8 File Offset: 0x000184C8
		public static LayerMask ToLayerMask(JSONObject obj)
		{
			return new LayerMask
			{
				value = (int)obj["value"].n
			};
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0001A2F8 File Offset: 0x000184F8
		public static JSONObject FromRect(Rect r)
		{
			JSONObject obj = JSONObject.obj;
			if (r.x != 0f)
			{
				obj.AddField("x", r.x);
			}
			if (r.y != 0f)
			{
				obj.AddField("y", r.y);
			}
			if (r.height != 0f)
			{
				obj.AddField("height", r.height);
			}
			if (r.width != 0f)
			{
				obj.AddField("width", r.width);
			}
			return obj;
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0001A38C File Offset: 0x0001858C
		public static Rect ToRect(JSONObject obj)
		{
			Rect rect = default(Rect);
			for (int i = 0; i < obj.Count; i++)
			{
				string text = obj.keys[i];
				if (!(text == "x"))
				{
					if (!(text == "y"))
					{
						if (!(text == "height"))
						{
							if (text == "width")
							{
								rect.width = obj[i].f;
							}
						}
						else
						{
							rect.height = obj[i].f;
						}
					}
					else
					{
						rect.y = obj[i].f;
					}
				}
				else
				{
					rect.x = obj[i].f;
				}
			}
			return rect;
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0001A450 File Offset: 0x00018650
		public static JSONObject FromRectOffset(RectOffset r)
		{
			JSONObject obj = JSONObject.obj;
			if (r.bottom != 0)
			{
				obj.AddField("bottom", r.bottom);
			}
			if (r.left != 0)
			{
				obj.AddField("left", r.left);
			}
			if (r.right != 0)
			{
				obj.AddField("right", r.right);
			}
			if (r.top != 0)
			{
				obj.AddField("top", r.top);
			}
			return obj;
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0001A4C8 File Offset: 0x000186C8
		public static RectOffset ToRectOffset(JSONObject obj)
		{
			RectOffset rectOffset = new RectOffset();
			for (int i = 0; i < obj.Count; i++)
			{
				string text = obj.keys[i];
				if (!(text == "bottom"))
				{
					if (!(text == "left"))
					{
						if (!(text == "right"))
						{
							if (text == "top")
							{
								rectOffset.top = (int)obj[i].n;
							}
						}
						else
						{
							rectOffset.right = (int)obj[i].n;
						}
					}
					else
					{
						rectOffset.left = (int)obj[i].n;
					}
				}
				else
				{
					rectOffset.bottom = (int)obj[i].n;
				}
			}
			return rectOffset;
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0001A588 File Offset: 0x00018788
		public static AnimationCurve ToAnimationCurve(JSONObject obj)
		{
			AnimationCurve animationCurve = new AnimationCurve();
			if (obj.HasField("keys"))
			{
				JSONObject field = obj.GetField("keys");
				for (int i = 0; i < field.list.Count; i++)
				{
					animationCurve.AddKey(JSONTemplates.ToKeyframe(field[i]));
				}
			}
			if (obj.HasField("preWrapMode"))
			{
				animationCurve.preWrapMode = (WrapMode)obj.GetField("preWrapMode").n;
			}
			if (obj.HasField("postWrapMode"))
			{
				animationCurve.postWrapMode = (WrapMode)obj.GetField("postWrapMode").n;
			}
			return animationCurve;
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x0001A628 File Offset: 0x00018828
		public static JSONObject FromAnimationCurve(AnimationCurve a)
		{
			JSONObject obj = JSONObject.obj;
			obj.AddField("preWrapMode", a.preWrapMode.ToString());
			obj.AddField("postWrapMode", a.postWrapMode.ToString());
			if (a.keys.Length != 0)
			{
				JSONObject jsonobject = JSONObject.Create();
				for (int i = 0; i < a.keys.Length; i++)
				{
					jsonobject.Add(JSONTemplates.FromKeyframe(a.keys[i]));
				}
				obj.AddField("keys", jsonobject);
			}
			return obj;
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0001A6C0 File Offset: 0x000188C0
		public static Keyframe ToKeyframe(JSONObject obj)
		{
			Keyframe keyframe = new Keyframe((float)(obj.HasField("time") ? obj.GetField("time").n : 0.0), (float)(obj.HasField("value") ? obj.GetField("value").n : 0.0));
			if (obj.HasField("inTangent"))
			{
				keyframe.inTangent = (float)obj.GetField("inTangent").n;
			}
			if (obj.HasField("outTangent"))
			{
				keyframe.outTangent = (float)obj.GetField("outTangent").n;
			}
			if (obj.HasField("tangentMode"))
			{
				keyframe.tangentMode = (int)obj.GetField("tangentMode").n;
			}
			return keyframe;
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0001A798 File Offset: 0x00018998
		public static JSONObject FromKeyframe(Keyframe k)
		{
			JSONObject obj = JSONObject.obj;
			if (k.inTangent != 0f)
			{
				obj.AddField("inTangent", k.inTangent);
			}
			if (k.outTangent != 0f)
			{
				obj.AddField("outTangent", k.outTangent);
			}
			if (k.tangentMode != 0)
			{
				obj.AddField("tangentMode", k.tangentMode);
			}
			if (k.time != 0f)
			{
				obj.AddField("time", k.time);
			}
			if (k.value != 0f)
			{
				obj.AddField("value", k.value);
			}
			return obj;
		}

		// Token: 0x04000249 RID: 585
		private static readonly HashSet<object> touched = new HashSet<object>();
	}
}
