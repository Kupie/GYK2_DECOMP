using System;
using System.Reflection;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000154 RID: 340
	[Serializable]
	public class LazyUITesterMethodData
	{
		// Token: 0x0600074A RID: 1866 RVA: 0x000254C7 File Offset: 0x000236C7
		public LazyUITesterMethodData(Type type, MethodInfo methodInfo)
		{
			this.windowTypeStr = type.AssemblyQualifiedName;
			this.methodStr = methodInfo.Name;
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x000254E7 File Offset: 0x000236E7
		public LazyUITesterMethodData(string json)
		{
			JsonUtility.FromJsonOverwrite(json, this);
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x000254F6 File Offset: 0x000236F6
		public string ToJson()
		{
			return JsonUtility.ToJson(this);
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x00025500 File Offset: 0x00023700
		public void Invoke()
		{
			Type type = Type.GetType(this.windowTypeStr);
			if (type == null)
			{
				Debug.LogError("windowType for windowTypeStr:[" + this.windowTypeStr + "] is null!!!");
				return;
			}
			MethodInfo method = typeof(LazyUI).GetMethod("GetWindow", BindingFlags.Static | BindingFlags.Public);
			if (method == null)
			{
				Debug.LogError("LazyUI.GetWindow<T>() method not found.");
				return;
			}
			LazyWidgetBase lazyWidgetBase = method.MakeGenericMethod(new Type[] { type }).Invoke(null, null) as LazyWidgetBase;
			if (lazyWidgetBase == null)
			{
				Debug.LogError(string.Format("Can't get window of type {0} via LazyUI.GetWindow<T>().", type));
				return;
			}
			Debug.Log("[LazyUITester] windowTypeStr: " + this.windowTypeStr);
			Debug.Log(string.Format("[LazyUITester] Resolved Type: {0}", type));
			Debug.Log("[LazyUITester] Type Name: " + ((type != null) ? type.Name : null));
			Debug.Log("[LazyUITester] w Name: " + ((lazyWidgetBase != null) ? lazyWidgetBase.name : null));
			Debug.Log("[LazyUITester] methodStr: " + this.methodStr);
			MethodInfo method2 = type.GetMethod(this.methodStr, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
			if (method2 == null)
			{
				Debug.LogError(string.Format("Method '{0}' not found on type {1}", this.methodStr, type));
				return;
			}
			method2.Invoke(lazyWidgetBase, null);
		}

		// Token: 0x0400046C RID: 1132
		[SerializeField]
		private string windowTypeStr;

		// Token: 0x0400046D RID: 1133
		[SerializeField]
		private string methodStr;
	}
}
