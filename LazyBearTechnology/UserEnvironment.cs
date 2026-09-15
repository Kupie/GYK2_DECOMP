using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000194 RID: 404
	public static class UserEnvironment
	{
		// Token: 0x0600090E RID: 2318 RVA: 0x0002B67C File Offset: 0x0002987C
		public static void LogUserEnvironment()
		{
			try
			{
				Debug.Log(UserEnvironment.BuildReport());
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[UserEnvironment] Failed to collect environment data: " + ex.Message);
			}
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x0002B6C0 File Offset: 0x000298C0
		private static string BuildReport()
		{
			StringBuilder stringBuilder = new StringBuilder(2048);
			stringBuilder.AppendLine("[UserEnvironment]");
			UserEnvironment.AppendApplication(stringBuilder);
			UserEnvironment.AppendOperatingSystem(stringBuilder);
			UserEnvironment.AppendCpu(stringBuilder);
			UserEnvironment.AppendMemory(stringBuilder);
			UserEnvironment.AppendGpu(stringBuilder);
			UserEnvironment.AppendDisplay(stringBuilder);
			UserEnvironment.AppendAudio(stringBuilder);
			UserEnvironment.AppendQuality(stringBuilder);
			return stringBuilder.ToString();
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x0002B718 File Offset: 0x00029918
		private static void AppendApplication(StringBuilder sb)
		{
			sb.Append("App: ").Append(Application.productName);
			sb.Append(" v").Append(Application.version);
			sb.Append(" | Unity ").Append(Application.unityVersion);
			sb.Append(" | ").Append(Application.platform);
			sb.Append(" | ").Append(Application.installMode);
			sb.AppendLine();
			sb.Append("Runtime: ").Append(RuntimeInformation.FrameworkDescription);
			sb.Append(" | CLR ").Append(Environment.Version);
			sb.Append(" | process ").Append(RuntimeInformation.ProcessArchitecture);
			sb.Append(" | OS arch ").Append(RuntimeInformation.OSArchitecture);
			sb.AppendLine();
			sb.Append("Locale: ").Append(Application.systemLanguage);
			sb.Append(" | culture ").Append(CultureInfo.CurrentCulture.Name);
			sb.Append(" | UI ").Append(CultureInfo.CurrentUICulture.Name);
			sb.AppendLine();
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x0002B868 File Offset: 0x00029A68
		private static void AppendOperatingSystem(StringBuilder sb)
		{
			sb.Append("OS: ").Append(UserEnvironment.Safe(SystemInfo.operatingSystem));
			sb.Append(" | family ").Append(SystemInfo.operatingSystemFamily);
			sb.Append(" | 64-bit OS ").Append(Environment.Is64BitOperatingSystem);
			sb.Append(" | 64-bit process ").Append(Environment.Is64BitProcess);
			sb.AppendLine();
			sb.Append("Device: type ").Append(SystemInfo.deviceType);
			sb.Append(" | model ").Append(UserEnvironment.Safe(SystemInfo.deviceModel));
			sb.AppendLine();
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x0002B91C File Offset: 0x00029B1C
		private static void AppendCpu(StringBuilder sb)
		{
			sb.Append("CPU: ").Append(UserEnvironment.Safe(SystemInfo.processorType));
			string text = UserEnvironment.Safe(SystemInfo.processorManufacturer);
			string text2 = UserEnvironment.Safe(SystemInfo.processorModel);
			if (text != "n/a")
			{
				sb.Append(" | manufacturer ").Append(text);
			}
			if (text2 != "n/a" && text2 != UserEnvironment.Safe(SystemInfo.processorType))
			{
				sb.Append(" | model ").Append(text2);
			}
			sb.Append(" | cores ").Append(SystemInfo.processorCount);
			int processorFrequency = SystemInfo.processorFrequency;
			if (processorFrequency > 0)
			{
				sb.Append(" | ").Append(processorFrequency).Append(" MHz");
			}
			sb.AppendLine();
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x0002B9F0 File Offset: 0x00029BF0
		private static void AppendMemory(StringBuilder sb)
		{
			sb.Append("RAM: ").Append(UserEnvironment.FormatMegabytes(SystemInfo.systemMemorySize));
			long totalMemory = GC.GetTotalMemory(false);
			sb.Append(" | managed heap ").Append(UserEnvironment.FormatBytes(totalMemory));
			sb.AppendLine();
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x0002BA40 File Offset: 0x00029C40
		private static void AppendGpu(StringBuilder sb)
		{
			sb.Append("GPU: ").Append(UserEnvironment.Safe(SystemInfo.graphicsDeviceName));
			sb.Append(" | vendor ").Append(UserEnvironment.Safe(SystemInfo.graphicsDeviceVendor));
			sb.Append(" | ").Append(SystemInfo.graphicsDeviceType);
			sb.AppendLine();
			sb.Append("GPU driver: ").Append(UserEnvironment.Safe(SystemInfo.graphicsDeviceVersion));
			sb.Append(" | VRAM ").Append(UserEnvironment.FormatMegabytes(SystemInfo.graphicsMemorySize));
			sb.Append(" | SM ").Append(SystemInfo.graphicsShaderLevel);
			sb.Append(" | max texture ").Append(SystemInfo.maxTextureSize);
			sb.AppendLine();
			sb.Append("GPU features: multithreaded ").Append(SystemInfo.graphicsMultiThreaded);
			sb.Append(" | threading ").Append(SystemInfo.renderingThreadingMode);
			sb.Append(" | compute ").Append(SystemInfo.supportsComputeShaders);
			sb.Append(" | instancing ").Append(SystemInfo.supportsInstancing);
			sb.Append(" | ray tracing ").Append(SystemInfo.supportsRayTracing);
			sb.Append(" | copyTexture ").Append(SystemInfo.copyTextureSupport);
			sb.AppendLine();
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x0002BBA4 File Offset: 0x00029DA4
		private static void AppendDisplay(StringBuilder sb)
		{
			Resolution currentResolution = Screen.currentResolution;
			sb.Append("Screen: window ").Append(Screen.width).Append('x')
				.Append(Screen.height);
			sb.Append(" | desktop ").Append(currentResolution.width).Append('x')
				.Append(currentResolution.height);
			sb.Append(" @ ").Append(UserEnvironment.FormatRefreshRate(currentResolution)).Append(" Hz");
			sb.Append(" | ").Append(Screen.fullScreenMode);
			sb.Append(" | fullscreen ").Append(Screen.fullScreen);
			if (Screen.dpi > 0f)
			{
				sb.Append(" | DPI ").Append(Screen.dpi.ToString("0.#"));
			}
			sb.AppendLine();
			Display[] displays = Display.displays;
			int num = ((displays != null) ? displays.Length : 0);
			sb.Append("Displays: ").Append(num);
			for (int i = 0; i < num; i++)
			{
				Display display = displays[i];
				sb.Append(" | [").Append(i).Append("] native ");
				sb.Append(display.systemWidth).Append('x').Append(display.systemHeight);
				sb.Append(" render ").Append(display.renderingWidth).Append('x')
					.Append(display.renderingHeight);
				if (display.active)
				{
					sb.Append(" active");
				}
			}
			sb.AppendLine();
			UserEnvironment.AppendHdr(sb);
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x0002BD5C File Offset: 0x00029F5C
		private static void AppendHdr(StringBuilder sb)
		{
			try
			{
				sb.Append("HDR: flags ").Append(SystemInfo.hdrDisplaySupportFlags);
				HDROutputSettings main = HDROutputSettings.main;
				if (main != null)
				{
					sb.Append(" | available ").Append(main.available);
					sb.Append(" | active ").Append(main.active);
					if (main.available)
					{
						sb.Append(" | format ").Append(main.graphicsFormat);
					}
				}
				sb.AppendLine();
			}
			catch (Exception)
			{
				sb.AppendLine("HDR: n/a");
			}
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x0002BE08 File Offset: 0x0002A008
		private static void AppendAudio(StringBuilder sb)
		{
			sb.Append("Audio: available ").Append(SystemInfo.supportsAudio);
			if (SystemInfo.supportsAudio)
			{
				AudioConfiguration configuration = AudioSettings.GetConfiguration();
				sb.Append(" | ").Append(configuration.speakerMode);
				sb.Append(" | ").Append(configuration.sampleRate).Append(" Hz");
				sb.Append(" | DSP ").Append(configuration.dspBufferSize);
			}
			sb.AppendLine();
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x0002BE94 File Offset: 0x0002A094
		private static void AppendQuality(StringBuilder sb)
		{
			int qualityLevel = QualitySettings.GetQualityLevel();
			string text = "n/a";
			string[] names = QualitySettings.names;
			if (names != null && qualityLevel >= 0 && qualityLevel < names.Length)
			{
				text = names[qualityLevel];
			}
			sb.Append("Quality: ").Append(text);
			sb.Append(" (").Append(qualityLevel).Append(')');
			sb.Append(" | vSync ").Append(QualitySettings.vSyncCount);
			sb.Append(" | target FPS ").Append(Application.targetFrameRate);
			sb.Append(" | async upload ").Append(QualitySettings.asyncUploadTimeSlice).Append(" ms");
			sb.AppendLine();
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x0002BF44 File Offset: 0x0002A144
		private static string FormatRefreshRate(Resolution resolution)
		{
			double value = resolution.refreshRateRatio.value;
			if (value <= 0.0)
			{
				return "n/a";
			}
			return value.ToString("0.##");
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x0002BF7F File Offset: 0x0002A17F
		private static string Safe(string value)
		{
			if (string.IsNullOrEmpty(value) || value == "n/a")
			{
				return "n/a";
			}
			return value;
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x0002BFA0 File Offset: 0x0002A1A0
		private static string FormatMegabytes(int megabytes)
		{
			if (megabytes <= 0)
			{
				return "n/a";
			}
			if (megabytes >= 1024)
			{
				return megabytes.ToString() + " MB (" + ((float)megabytes / 1024f).ToString("0.#") + " GB)";
			}
			return megabytes.ToString() + " MB";
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x0002BFFC File Offset: 0x0002A1FC
		private static string FormatBytes(long bytes)
		{
			if (bytes < 1024L)
			{
				return bytes.ToString() + " B";
			}
			if (bytes < 1048576L)
			{
				return ((float)bytes / 1024f).ToString("0.#") + " KB";
			}
			if (bytes < 1073741824L)
			{
				return ((float)bytes / 1048576f).ToString("0.#") + " MB";
			}
			return ((float)bytes / 1.0737418E+09f).ToString("0.#") + " GB";
		}
	}
}
