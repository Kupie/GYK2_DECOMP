using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000AD0 RID: 2768
public static class FeatureLog
{
	// Token: 0x06004AB3 RID: 19123 RVA: 0x001609D6 File Offset: 0x0015EBD6
	[Conditional("UNITY_EDITOR")]
	[Conditional("DEV_BUILD")]
	public static void Log(LogChannel channel, string message)
	{
		if (!DevUtils.IsLogChannelEnabled(channel))
		{
			return;
		}
		global::UnityEngine.Debug.Log("#" + FeatureLog.Tag(channel) + "# " + message);
	}

	// Token: 0x06004AB4 RID: 19124 RVA: 0x001609FC File Offset: 0x0015EBFC
	[Conditional("UNITY_EDITOR")]
	[Conditional("DEV_BUILD")]
	public static void Log(LogChannel channel, string message, object context)
	{
		if (!DevUtils.IsLogChannelEnabled(channel))
		{
			return;
		}
		global::UnityEngine.Debug.Log(string.Format("#{0}# {1}. Context: {2}", FeatureLog.Tag(channel), message, context));
	}

	// Token: 0x06004AB5 RID: 19125 RVA: 0x00160A1E File Offset: 0x0015EC1E
	[Conditional("UNITY_EDITOR")]
	[Conditional("DEV_BUILD")]
	public static void Warn(LogChannel channel, string message)
	{
		if (!DevUtils.IsLogChannelEnabled(channel))
		{
			return;
		}
		global::UnityEngine.Debug.LogWarning("#" + FeatureLog.Tag(channel) + "# " + message);
	}

	// Token: 0x06004AB6 RID: 19126 RVA: 0x00160A44 File Offset: 0x0015EC44
	[Conditional("UNITY_EDITOR")]
	[Conditional("DEV_BUILD")]
	public static void Error(LogChannel channel, string message)
	{
		if (!DevUtils.IsLogChannelEnabled(channel))
		{
			return;
		}
		global::UnityEngine.Debug.LogError("#" + FeatureLog.Tag(channel) + "# " + message);
	}

	// Token: 0x06004AB7 RID: 19127 RVA: 0x00160A6A File Offset: 0x0015EC6A
	private static string Tag(LogChannel channel)
	{
		return channel.ToString().ToLowerInvariant();
	}
}
