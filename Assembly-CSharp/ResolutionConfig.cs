using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000794 RID: 1940
[Serializable]
public class ResolutionConfig
{
	// Token: 0x060031E6 RID: 12774 RVA: 0x000EF364 File Offset: 0x000ED564
	public static bool IsDisplayBelowMinimum()
	{
		int num;
		int num2;
		if (!ResolutionConfig.TryGetNativeDisplaySize(out num, out num2))
		{
			return false;
		}
		Debug.Log(string.Format("{0}: native display [{1}x{2}], minimum [{3}x{4}]", new object[] { "ResolutionConfig", num, num2, 1280, 720 }));
		return num2 < 720;
	}

	// Token: 0x060031E7 RID: 12775 RVA: 0x000EF3D0 File Offset: 0x000ED5D0
	public static bool TryGetNativeDisplaySize(out int width, out int height)
	{
		width = 0;
		height = 0;
		if (Display.main != null)
		{
			ResolutionConfig.ConsiderNativeDisplaySize(Display.main.systemWidth, Display.main.systemHeight, ref width, ref height);
		}
		ResolutionConfig.ConsiderNativeDisplaySize(Screen.currentResolution.width, Screen.currentResolution.height, ref width, ref height);
		return width > 0 && height > 0;
	}

	// Token: 0x060031E8 RID: 12776 RVA: 0x000EF432 File Offset: 0x000ED632
	private static void ConsiderNativeDisplaySize(int candidateWidth, int candidateHeight, ref int width, ref int height)
	{
		if (candidateWidth <= 0 || candidateHeight <= 0)
		{
			return;
		}
		if (height <= 0 || candidateHeight < height)
		{
			width = candidateWidth;
			height = candidateHeight;
		}
	}

	// Token: 0x1700079E RID: 1950
	// (get) Token: 0x060031E9 RID: 12777 RVA: 0x000EF44D File Offset: 0x000ED64D
	public static int Height
	{
		get
		{
			if (ResolutionConfig.currentResolution == null)
			{
				return Screen.height / DevUtils.PixelSize;
			}
			return ResolutionConfig.currentResolution.AppliedHeight / ResolutionConfig.currentResolution.pixelSize;
		}
	}

	// Token: 0x1700079F RID: 1951
	// (get) Token: 0x060031EA RID: 12778 RVA: 0x000EF477 File Offset: 0x000ED677
	public static int Width
	{
		get
		{
			if (ResolutionConfig.currentResolution == null)
			{
				return Screen.width / DevUtils.PixelSize;
			}
			return ResolutionConfig.currentResolution.AppliedWidth / ResolutionConfig.currentResolution.pixelSize;
		}
	}

	// Token: 0x170007A0 RID: 1952
	// (get) Token: 0x060031EB RID: 12779 RVA: 0x000EF4A1 File Offset: 0x000ED6A1
	public static int PixelSize
	{
		get
		{
			ResolutionConfig resolutionConfig = ResolutionConfig.currentResolution;
			if (resolutionConfig == null)
			{
				return DevUtils.PixelSize;
			}
			return resolutionConfig.pixelSize;
		}
	}

	// Token: 0x060031EC RID: 12780 RVA: 0x000EF4B8 File Offset: 0x000ED6B8
	public static float GetUiScaleFactor()
	{
		int num = ResolutionConfig.PixelSize;
		ResolutionConfig resolutionConfig = ResolutionConfig.currentResolution;
		if (resolutionConfig != null)
		{
			int appliedHeight = resolutionConfig.AppliedHeight;
		}
		return (float)num;
	}

	// Token: 0x170007A1 RID: 1953
	// (get) Token: 0x060031ED RID: 12781 RVA: 0x000EF4DF File Offset: 0x000ED6DF
	public bool IsValid
	{
		get
		{
			return this.width > 0 && this.height > 0;
		}
	}

	// Token: 0x170007A2 RID: 1954
	// (get) Token: 0x060031EE RID: 12782 RVA: 0x000EF4F5 File Offset: 0x000ED6F5
	public int ListedWidth
	{
		get
		{
			return this.width;
		}
	}

	// Token: 0x170007A3 RID: 1955
	// (get) Token: 0x060031EF RID: 12783 RVA: 0x000EF4FD File Offset: 0x000ED6FD
	public int ListedHeight
	{
		get
		{
			return this.height;
		}
	}

	// Token: 0x170007A4 RID: 1956
	// (get) Token: 0x060031F0 RID: 12784 RVA: 0x000EF505 File Offset: 0x000ED705
	public int AppliedWidth
	{
		get
		{
			if (!this.HasFakeResolution)
			{
				return this.width;
			}
			return this.fakeWidth;
		}
	}

	// Token: 0x170007A5 RID: 1957
	// (get) Token: 0x060031F1 RID: 12785 RVA: 0x000EF51C File Offset: 0x000ED71C
	public int AppliedHeight
	{
		get
		{
			if (!this.HasFakeResolution)
			{
				return this.height;
			}
			return this.fakeHeight;
		}
	}

	// Token: 0x170007A6 RID: 1958
	// (get) Token: 0x060031F2 RID: 12786 RVA: 0x000EF533 File Offset: 0x000ED733
	public UIWindowSizeType WindowSizeType
	{
		get
		{
			return this.windowSizeType;
		}
	}

	// Token: 0x170007A7 RID: 1959
	// (get) Token: 0x060031F3 RID: 12787 RVA: 0x000EF53B File Offset: 0x000ED73B
	public bool UseMainMenuScaleX2
	{
		get
		{
			return this.useMainMenuScaleX2;
		}
	}

	// Token: 0x170007A8 RID: 1960
	// (get) Token: 0x060031F4 RID: 12788 RVA: 0x000EF543 File Offset: 0x000ED743
	private bool HasFakeResolution
	{
		get
		{
			return this.isFakeResolution && this.fakeWidth > 0 && this.fakeHeight > 0;
		}
	}

	// Token: 0x060031F5 RID: 12789 RVA: 0x000EF564 File Offset: 0x000ED764
	public ResolutionConfig Copy()
	{
		return new ResolutionConfig(this.width, this.height, this.pixelSize, this.windowSizeType, this.customAdditinalString, this.useMainMenuScaleX2, this.isFakeResolution, this.fakeWidth, this.fakeHeight);
	}

	// Token: 0x060031F6 RID: 12790 RVA: 0x000EF5AC File Offset: 0x000ED7AC
	public ResolutionConfig WithSize(int newWidth, int newHeight)
	{
		return new ResolutionConfig(newWidth, newHeight, this.pixelSize, this.windowSizeType, this.customAdditinalString, this.useMainMenuScaleX2, false, 0, 0);
	}

	// Token: 0x060031F7 RID: 12791 RVA: 0x000EF5DC File Offset: 0x000ED7DC
	public int FindIndexForScreenSize(int screenWidth, int screenHeight)
	{
		return ResolutionConfig.FindResolutionConfigIndex(new ResolutionConfig(screenWidth, screenHeight, this.pixelSize, this.windowSizeType, this.customAdditinalString, this.useMainMenuScaleX2, false, 0, 0));
	}

	// Token: 0x170007A9 RID: 1961
	// (get) Token: 0x060031F8 RID: 12792 RVA: 0x000EF610 File Offset: 0x000ED810
	private static List<ResolutionConfig> Resolutions
	{
		get
		{
			if (ResolutionConfig.availableResolutions.Count <= 0)
			{
				return ResolutionConfig.hardcodedResolutions;
			}
			return ResolutionConfig.availableResolutions;
		}
	}

	// Token: 0x060031F9 RID: 12793 RVA: 0x000EF62A File Offset: 0x000ED82A
	public ResolutionConfig(int width, int height)
	{
		this.width = width;
		this.height = height;
		this.pixelSize = ResolutionConfig.GetPixelSize(width, height);
		this.UpdateUIModeType();
		this.customAdditinalString = string.Empty;
		this.useMainMenuScaleX2 = false;
	}

	// Token: 0x060031FA RID: 12794 RVA: 0x000EF668 File Offset: 0x000ED868
	public ResolutionConfig(int width, int height, int pixelSize, UIWindowSizeType windowSizeType = UIWindowSizeType.Big, string customAdditinalString = "", bool useMainMenuScaleX2 = false, bool isFakeResolution = false, int fakeWidth = 0, int fakeHeight = 0)
	{
		this.width = width;
		this.height = height;
		this.pixelSize = pixelSize;
		this.windowSizeType = windowSizeType;
		this.customAdditinalString = customAdditinalString;
		this.useMainMenuScaleX2 = useMainMenuScaleX2;
		this.isFakeResolution = isFakeResolution;
		this.fakeWidth = fakeWidth;
		this.fakeHeight = fakeHeight;
	}

	// Token: 0x060031FB RID: 12795 RVA: 0x000EF6C0 File Offset: 0x000ED8C0
	public static string[] GetResolutionsStringArray()
	{
		List<string> list = new List<string>();
		foreach (ResolutionConfig resolutionConfig in ResolutionConfig.Resolutions)
		{
			list.Add(resolutionConfig.GetResolutionName());
		}
		return list.ToArray();
	}

	// Token: 0x060031FC RID: 12796 RVA: 0x000EF724 File Offset: 0x000ED924
	public static ResolutionConfig GetResolutionConfigByIndex(int index)
	{
		if (index < 0 || index > ResolutionConfig.Resolutions.Count - 1)
		{
			Debug.LogError(string.Format("Cannot find {0} by index [{1}]", "ResolutionConfig", index));
			return ResolutionConfig.Resolutions[0];
		}
		return ResolutionConfig.Resolutions[index];
	}

	// Token: 0x060031FD RID: 12797 RVA: 0x000EF778 File Offset: 0x000ED978
	public static ResolutionConfig GetOptimalResolution()
	{
		int num = Screen.currentResolution.width;
		int num2 = Screen.currentResolution.height;
		if (num <= 0 || num2 <= 0)
		{
			num = Screen.width;
			num2 = Screen.height;
		}
		ResolutionConfig resolutionConfig = null;
		int num3 = -1;
		for (int i = 0; i < ResolutionConfig.Resolutions.Count; i++)
		{
			ResolutionConfig resolutionConfig2 = ResolutionConfig.Resolutions[i];
			if (!resolutionConfig2.HasFakeResolution)
			{
				if (resolutionConfig2.width == num && resolutionConfig2.height == num2)
				{
					return resolutionConfig2.Copy();
				}
				if (resolutionConfig2.width <= num && resolutionConfig2.height <= num2)
				{
					int num4 = resolutionConfig2.width * resolutionConfig2.height;
					if (num4 > num3)
					{
						num3 = num4;
						resolutionConfig = resolutionConfig2;
					}
				}
			}
		}
		if (resolutionConfig != null)
		{
			return resolutionConfig.Copy();
		}
		return new ResolutionConfig(num, num2);
	}

	// Token: 0x060031FE RID: 12798 RVA: 0x000EF848 File Offset: 0x000EDA48
	public static int FindResolutionConfigIndex(int width, int height)
	{
		Debug.Log(string.Format("call [FindResolutionConfigIndex] in [{0}] width:[{1}] height:[{2}]", "ResolutionConfig", width, height));
		for (int i = 0; i < ResolutionConfig.Resolutions.Count; i++)
		{
			Debug.Log(string.Format("{0}: trying to find res [{1}x{2}]. Checking: {3}", new object[]
			{
				"ResolutionConfig",
				width,
				height,
				ResolutionConfig.Resolutions[i].GetResolutionName()
			}));
			if (ResolutionConfig.Resolutions[i].width == width && ResolutionConfig.Resolutions[i].height == height)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x060031FF RID: 12799 RVA: 0x000EF8F8 File Offset: 0x000EDAF8
	public static int FindResolutionConfigIndex(ResolutionConfig resolutionConfig)
	{
		if (resolutionConfig == null)
		{
			return -1;
		}
		for (int i = 0; i < ResolutionConfig.Resolutions.Count; i++)
		{
			ResolutionConfig resolutionConfig2 = ResolutionConfig.Resolutions[i];
			if (resolutionConfig2.width == resolutionConfig.width && resolutionConfig2.height == resolutionConfig.height && resolutionConfig2.pixelSize == resolutionConfig.pixelSize && resolutionConfig2.windowSizeType == resolutionConfig.windowSizeType && resolutionConfig2.customAdditinalString == resolutionConfig.customAdditinalString)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06003200 RID: 12800 RVA: 0x000EF97C File Offset: 0x000EDB7C
	public static bool TryAddAvailableResolution(ResolutionConfig resolutionConfig)
	{
		foreach (ResolutionConfig resolutionConfig2 in ResolutionConfig.availableResolutions)
		{
			if (resolutionConfig2.width == resolutionConfig.width && resolutionConfig2.height == resolutionConfig.height && resolutionConfig2.windowSizeType == resolutionConfig.windowSizeType && resolutionConfig2.customAdditinalString == resolutionConfig.customAdditinalString)
			{
				Debug.LogWarning("Resolution with parameters " + resolutionConfig.GetInfo() + " already exist");
				return false;
			}
		}
		Debug.Log(string.Format("AddResolution [{0}x{1}] size:[{2}]", resolutionConfig.width, resolutionConfig.height, resolutionConfig.windowSizeType));
		ResolutionConfig.availableResolutions.Add(resolutionConfig);
		return true;
	}

	// Token: 0x06003201 RID: 12801 RVA: 0x000EFA60 File Offset: 0x000EDC60
	public static void InitAvailableResolutions()
	{
		if (ResolutionConfig.isInitialized)
		{
			return;
		}
		ResolutionConfig.isInitialized = true;
		ResolutionConfig.currentResolution = null;
		ResolutionConfig.availableResolutions.Clear();
		Resolution[] resolutions = Screen.resolutions;
		for (int i = 0; i < resolutions.Length; i++)
		{
			Resolution resolution = resolutions[i];
			Debug.Log(string.Format("  Processing resolution [{0}x{1}]", resolution.width, resolution.height));
			if (!ResolutionConfig.IsUltraWide(resolution) && resolution.width >= 1280 && resolution.height >= 720 && resolution.width <= 5120)
			{
				List<ResolutionConfig> list = ResolutionConfig.hardcodedResolutions.FindAll((ResolutionConfig r) => r.width == resolution.width && r.height == resolution.height);
				if (list.Count > 0)
				{
					using (List<ResolutionConfig>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ResolutionConfig resolutionConfig = enumerator.Current;
							ResolutionConfig.TryAddAvailableResolution(resolutionConfig);
						}
						goto IL_012A;
					}
				}
				ResolutionConfig.TryAddAvailableResolution(new ResolutionConfig(resolution.width, resolution.height));
			}
			IL_012A:;
		}
	}

	// Token: 0x06003202 RID: 12802 RVA: 0x000EFBB4 File Offset: 0x000EDDB4
	public static void SetResolution(IntVector2 resolution)
	{
		int num = ResolutionConfig.FindResolutionConfigIndex(resolution.x, resolution.y);
		if (num != -1)
		{
			ResolutionConfig.currentResolution = ResolutionConfig.Resolutions[num].Copy();
			Debug.Log("ResolutionConfig: Set resolution [" + ResolutionConfig.currentResolution.GetInfo() + "]");
			return;
		}
		ResolutionConfig.currentResolution = ResolutionConfig.Resolutions[ResolutionConfig.FindClosestResolutionConfigIndex(resolution.x, resolution.y)];
		Debug.LogWarning(string.Format("{0}:cannot find resolution [{1}] in {2}. Applied closest one [{3}]", new object[]
		{
			"ResolutionConfig",
			resolution,
			"Resolutions",
			ResolutionConfig.currentResolution
		}));
	}

	// Token: 0x06003203 RID: 12803 RVA: 0x000EFC64 File Offset: 0x000EDE64
	public static void SetResolution(ResolutionConfig resolutionConfig)
	{
		if (resolutionConfig == null)
		{
			ResolutionConfig.currentResolution = null;
			return;
		}
		int num = ResolutionConfig.FindResolutionConfigIndex(resolutionConfig);
		if (num != -1)
		{
			ResolutionConfig.currentResolution = ResolutionConfig.Resolutions[num].Copy();
			Debug.Log("ResolutionConfig: Set resolution by config [" + ResolutionConfig.currentResolution.GetInfo() + "]");
			return;
		}
		int num2 = ResolutionConfig.FindResolutionConfigIndex(resolutionConfig.width, resolutionConfig.height);
		if (num2 != -1)
		{
			ResolutionConfig.currentResolution = ResolutionConfig.Resolutions[num2].Copy();
			Debug.Log("ResolutionConfig: Set resolution by size [" + ResolutionConfig.currentResolution.GetInfo() + "]");
			return;
		}
		int num3 = ((resolutionConfig.height < 720) ? ResolutionConfig.GetPixelSize(resolutionConfig.width, resolutionConfig.height) : ((resolutionConfig.pixelSize > 0) ? resolutionConfig.pixelSize : ResolutionConfig.GetPixelSize(resolutionConfig.width, resolutionConfig.height)));
		ResolutionConfig resolutionConfig2 = new ResolutionConfig(resolutionConfig.width, resolutionConfig.height, num3, resolutionConfig.windowSizeType, resolutionConfig.customAdditinalString, resolutionConfig.useMainMenuScaleX2, resolutionConfig.isFakeResolution, resolutionConfig.fakeWidth, resolutionConfig.fakeHeight);
		if (resolutionConfig2.windowSizeType != UIWindowSizeType.Small && resolutionConfig2.windowSizeType != UIWindowSizeType.Big)
		{
			resolutionConfig2.UpdateUIModeType();
		}
		else if (resolutionConfig2.windowSizeType == UIWindowSizeType.Big && (resolutionConfig2.height < 720 || resolutionConfig2.height / resolutionConfig2.pixelSize <= 360))
		{
			resolutionConfig2.UpdateUIModeType();
		}
		ResolutionConfig.currentResolution = resolutionConfig2;
		Debug.Log("ResolutionConfig: Set normalized custom resolution [" + ResolutionConfig.currentResolution.GetInfo() + "]");
	}

	// Token: 0x06003204 RID: 12804 RVA: 0x000EFDEA File Offset: 0x000EDFEA
	public string GetResolutionName()
	{
		return string.Format("{0}x{1}{2}", this.width, this.height, this.customAdditinalString);
	}

	// Token: 0x06003205 RID: 12805 RVA: 0x000EFE12 File Offset: 0x000EE012
	private static bool IsUltraWide(Resolution resolution)
	{
		return (float)resolution.width / (float)resolution.height > 2f;
	}

	// Token: 0x06003206 RID: 12806 RVA: 0x000EFE2C File Offset: 0x000EE02C
	private static int FindClosestResolutionConfigIndex(int width, int height)
	{
		int num = 0;
		int num2 = int.MaxValue;
		int num3 = int.MaxValue;
		for (int i = 0; i < ResolutionConfig.Resolutions.Count; i++)
		{
			int num4 = Mathf.Abs(ResolutionConfig.Resolutions[i].width - width);
			int num5 = Mathf.Abs(ResolutionConfig.Resolutions[i].height - height);
			if (num4 <= num2 && num5 <= num3)
			{
				num2 = num4;
				num3 = num5;
				num = i;
			}
		}
		return num;
	}

	// Token: 0x06003207 RID: 12807 RVA: 0x000EFEA4 File Offset: 0x000EE0A4
	public static ResolutionConfig CreateSwitchResolutionConfig()
	{
		int num;
		int num2;
		if (!SwitchDisplayResolution.TryGet(out num, out num2) || num2 <= 0)
		{
			return new ResolutionConfig(1280, 720);
		}
		if (num2 > 720)
		{
			return new ResolutionConfig(1280, 720);
		}
		return new ResolutionConfig(num, num2);
	}

	// Token: 0x06003208 RID: 12808 RVA: 0x000EFEF0 File Offset: 0x000EE0F0
	public static int GetPixelSize(int width, int height)
	{
		if (height < 720)
		{
			return 1;
		}
		int num = 2;
		if (height > 1440)
		{
			num = Mathf.CeilToInt((float)height / 540f);
		}
		return num;
	}

	// Token: 0x06003209 RID: 12809 RVA: 0x000EFF20 File Offset: 0x000EE120
	private void UpdateUIModeType()
	{
		this.windowSizeType = ((this.height < 720 || this.height / this.pixelSize <= 360) ? UIWindowSizeType.Small : UIWindowSizeType.Big);
	}

	// Token: 0x0600320A RID: 12810 RVA: 0x000EFF50 File Offset: 0x000EE150
	private string GetInfo()
	{
		string text = string.Format("Resolution: [{0}x{1}] PixelSize: [{2}] windowSizeType:[{3}] customAdditinalString:[{4}] useMainMenuScaleX2:[{5}]", new object[] { this.width, this.height, this.pixelSize, this.windowSizeType, this.customAdditinalString, this.useMainMenuScaleX2 });
		if (this.HasFakeResolution)
		{
			text += string.Format(" fakeResolution:[{0}x{1}]", this.fakeWidth, this.fakeHeight);
		}
		return text;
	}

	// Token: 0x0600320B RID: 12811 RVA: 0x000EFFED File Offset: 0x000EE1ED
	public static void LogCurrentResolutionConfig()
	{
		if (ResolutionConfig.currentResolution == null)
		{
			Debug.LogWarning("ResolutionConfig: currentResolution is null");
			return;
		}
		Debug.Log(string.Format("{0} current config: {1}", "ResolutionConfig", ResolutionConfig.currentResolution));
	}

	// Token: 0x0600320C RID: 12812 RVA: 0x000F001A File Offset: 0x000EE21A
	public override string ToString()
	{
		return "[" + this.GetInfo() + "]";
	}

	// Token: 0x04002827 RID: 10279
	public const int MIN_SCREEN_WIDTH = 1280;

	// Token: 0x04002828 RID: 10280
	public const int MIN_SCREEN_HEIGHT = 720;

	// Token: 0x04002829 RID: 10281
	public const int MAX_SCREEN_WIDTH = 5120;

	// Token: 0x0400282A RID: 10282
	public static ResolutionConfig currentResolution;

	// Token: 0x0400282B RID: 10283
	[SerializeField]
	private int width;

	// Token: 0x0400282C RID: 10284
	[SerializeField]
	private int height;

	// Token: 0x0400282D RID: 10285
	[SerializeField]
	private int pixelSize;

	// Token: 0x0400282E RID: 10286
	[SerializeField]
	private string customAdditinalString;

	// Token: 0x0400282F RID: 10287
	[SerializeField]
	private UIWindowSizeType windowSizeType;

	// Token: 0x04002830 RID: 10288
	[SerializeField]
	private bool useMainMenuScaleX2;

	// Token: 0x04002831 RID: 10289
	[SerializeField]
	private bool isFakeResolution;

	// Token: 0x04002832 RID: 10290
	[SerializeField]
	private int fakeWidth;

	// Token: 0x04002833 RID: 10291
	[SerializeField]
	private int fakeHeight;

	// Token: 0x04002834 RID: 10292
	private static List<ResolutionConfig> availableResolutions = new List<ResolutionConfig>();

	// Token: 0x04002835 RID: 10293
	private static List<ResolutionConfig> hardcodedResolutions = new List<ResolutionConfig>
	{
		new ResolutionConfig(1920, 1080, 2, UIWindowSizeType.Big, "", false, false, 0, 0),
		new ResolutionConfig(2560, 1440, 2, UIWindowSizeType.Big, "(x2)", true, false, 0, 0),
		new ResolutionConfig(2560, 1440, 2, UIWindowSizeType.Big, "(x3)", true, true, 1920, 1080),
		new ResolutionConfig(3840, 2160, 4, UIWindowSizeType.Big, "", false, false, 0, 0),
		new ResolutionConfig(1280, 720, 2, UIWindowSizeType.Small, "", false, false, 0, 0),
		new ResolutionConfig(1280, 800, 2, UIWindowSizeType.Small, "", false, false, 0, 0),
		new ResolutionConfig(2560, 1600, 2, UIWindowSizeType.Big, "", true, false, 0, 0),
		new ResolutionConfig(1366, 768, 2, UIWindowSizeType.Small, "", false, false, 0, 0),
		new ResolutionConfig(3440, 1440, 2, UIWindowSizeType.Big, "", true, false, 0, 0),
		new ResolutionConfig(1920, 1200, 2, UIWindowSizeType.Big, "", true, false, 0, 0),
		new ResolutionConfig(1600, 900, 2, UIWindowSizeType.Big, "", false, false, 0, 0),
		new ResolutionConfig(1360, 768, 2, UIWindowSizeType.Small, "", false, false, 0, 0),
		new ResolutionConfig(1440, 900, 2, UIWindowSizeType.Big, "", true, false, 0, 0),
		new ResolutionConfig(1680, 1050, 2, UIWindowSizeType.Big, "", true, false, 0, 0),
		new ResolutionConfig(2880, 1800, 3, UIWindowSizeType.Big, "", true, false, 0, 0),
		new ResolutionConfig(1280, 1024, 2, UIWindowSizeType.Small, "", true, false, 0, 0),
		new ResolutionConfig(5120, 1440, 2, UIWindowSizeType.Big, "", true, false, 0, 0),
		new ResolutionConfig(1920, 1440, 2, UIWindowSizeType.Big, "(x2)", true, false, 0, 0),
		new ResolutionConfig(1920, 1440, 2, UIWindowSizeType.Big, "(x3)", true, true, 1440, 1080),
		new ResolutionConfig(1280, 768, 2, UIWindowSizeType.Small, "", true, false, 0, 0),
		new ResolutionConfig(1280, 960, 2, UIWindowSizeType.Small, "", true, false, 0, 0),
		new ResolutionConfig(1440, 1080, 2, UIWindowSizeType.Big, "", true, false, 0, 0),
		new ResolutionConfig(1440, 1080, 2, UIWindowSizeType.Big, "", true, false, 0, 0),
		new ResolutionConfig(1600, 1024, 2, UIWindowSizeType.Big, "", false, false, 0, 0),
		new ResolutionConfig(1600, 1200, 2, UIWindowSizeType.Big, "", false, false, 0, 0)
	};

	// Token: 0x04002836 RID: 10294
	private static bool isInitialized;
}
