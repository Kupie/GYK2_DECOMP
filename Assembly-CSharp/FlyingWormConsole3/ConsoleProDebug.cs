using System;
using UnityEngine;

namespace FlyingWormConsole3
{
	// Token: 0x02000C5C RID: 3164
	public static class ConsoleProDebug
	{
		// Token: 0x060050B8 RID: 20664 RVA: 0x00002318 File Offset: 0x00000518
		public static void Clear()
		{
		}

		// Token: 0x060050B9 RID: 20665 RVA: 0x00180B0D File Offset: 0x0017ED0D
		public static void LogToFilter(string inLog, string inFilterName, global::UnityEngine.Object inContext = null)
		{
			Debug.Log(inLog + "\nCPAPI:{\"cmd\":\"Filter\", \"name\":\"" + inFilterName + "\"}", inContext);
		}

		// Token: 0x060050BA RID: 20666 RVA: 0x00180B26 File Offset: 0x0017ED26
		public static void LogAsType(string inLog, string inTypeName, global::UnityEngine.Object inContext = null)
		{
			Debug.Log(inLog + "\nCPAPI:{\"cmd\":\"LogType\", \"name\":\"" + inTypeName + "\"}", inContext);
		}

		// Token: 0x060050BB RID: 20667 RVA: 0x00180B3F File Offset: 0x0017ED3F
		public static void Watch(string inName, string inValue)
		{
			Debug.Log(string.Concat(new string[] { inName, " : ", inValue, "\nCPAPI:{\"cmd\":\"Watch\", \"name\":\"", inName, "\"}" }));
		}

		// Token: 0x060050BC RID: 20668 RVA: 0x00180B75 File Offset: 0x0017ED75
		public static void Search(string inText)
		{
			Debug.Log("\nCPAPI:{\"cmd\":\"Search\", \"text\":\"" + inText + "\"}");
		}
	}
}
