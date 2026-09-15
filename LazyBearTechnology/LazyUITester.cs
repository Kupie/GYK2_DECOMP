using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200001A RID: 26
public static class LazyUITester
{
	// Token: 0x1700000B RID: 11
	// (get) Token: 0x06000073 RID: 115 RVA: 0x00003EC6 File Offset: 0x000020C6
	public static bool isTesting
	{
		get
		{
			return !string.IsNullOrEmpty(PlayerPrefs.GetString("LazyUITester.TestMode"));
		}
	}

	// Token: 0x06000074 RID: 116 RVA: 0x00003EDC File Offset: 0x000020DC
	public static void OnGameStart()
	{
		Debug.Log("LazyUITester.OnGameStart");
		string @string = PlayerPrefs.GetString("LazyUITester.TestMode");
		LazyUITester.SetTestMode(null);
		if (!string.IsNullOrEmpty(@string))
		{
			new LazyUITesterMethodData(@string).Invoke();
		}
	}

	// Token: 0x06000075 RID: 117 RVA: 0x00003F17 File Offset: 0x00002117
	public static void SetTestMode(LazyUITesterMethodData methodToRun = null)
	{
		PlayerPrefs.SetString("LazyUITester.TestMode", (methodToRun == null) ? "" : methodToRun.ToJson());
	}

	// Token: 0x0400005B RID: 91
	private const string KEY_TEST_MODE = "LazyUITester.TestMode";
}
