using System;
using UnityEngine;

// Token: 0x02000778 RID: 1912
public static class GpuBenchmarkDatabase
{
	// Token: 0x0600319F RID: 12703 RVA: 0x000EAF70 File Offset: 0x000E9170
	public static int GetBenchmarkPoints()
	{
		return GpuBenchmarkDatabase.GetBenchmarkPoints(SystemInfo.graphicsDeviceName, SystemInfo.graphicsDeviceVendorID, SystemInfo.graphicsDeviceID);
	}

	// Token: 0x060031A0 RID: 12704 RVA: 0x000EAF88 File Offset: 0x000E9188
	public static int GetBenchmarkPoints(string gpuName, int vendorId, int deviceId)
	{
		string text = GpuBenchmarkDatabase.Normalize(gpuName);
		for (int i = 0; i < GpuBenchmarkDatabase.s_names.Length; i++)
		{
			GpuBenchmarkDatabase.NameEntry nameEntry = GpuBenchmarkDatabase.s_names[i];
			if (text.Contains(nameEntry.Match))
			{
				return nameEntry.Points;
			}
		}
		if (vendorId != 0 && deviceId != 0)
		{
			for (int j = 0; j < GpuBenchmarkDatabase.s_pci.Length; j++)
			{
				GpuBenchmarkDatabase.PciEntry pciEntry = GpuBenchmarkDatabase.s_pci[j];
				if (pciEntry.Vendor == vendorId && pciEntry.Device == deviceId)
				{
					return pciEntry.Points;
				}
			}
		}
		return -1;
	}

	// Token: 0x060031A1 RID: 12705 RVA: 0x000EB014 File Offset: 0x000E9214
	private static string Normalize(string s)
	{
		if (string.IsNullOrEmpty(s))
		{
			return string.Empty;
		}
		s = s.ToLowerInvariant();
		char[] array = new char[s.Length];
		int num = 0;
		bool flag = true;
		foreach (char c in s)
		{
			if (char.IsLetterOrDigit(c))
			{
				array[num++] = c;
				flag = false;
			}
			else if (!flag)
			{
				array[num++] = ' ';
				flag = true;
			}
		}
		if (num > 0 && array[num - 1] == ' ')
		{
			num--;
		}
		return new string(array, 0, num);
	}

	// Token: 0x040027A5 RID: 10149
	private static readonly GpuBenchmarkDatabase.NameEntry[] s_names = new GpuBenchmarkDatabase.NameEntry[]
	{
		new GpuBenchmarkDatabase.NameEntry("rtx 3080 laptop gpu 16gb", 520),
		new GpuBenchmarkDatabase.NameEntry("rtx 3080 laptop gpu 8gb", 520),
		new GpuBenchmarkDatabase.NameEntry("iris plus graphics 640", 75),
		new GpuBenchmarkDatabase.NameEntry("iris plus graphics 645", 82),
		new GpuBenchmarkDatabase.NameEntry("iris plus graphics 650", 88),
		new GpuBenchmarkDatabase.NameEntry("iris plus graphics 655", 95),
		new GpuBenchmarkDatabase.NameEntry("iris pro graphics 5200", 55),
		new GpuBenchmarkDatabase.NameEntry("iris pro graphics 6200", 65),
		new GpuBenchmarkDatabase.NameEntry("rtx 3050 ti laptop gpu", 285),
		new GpuBenchmarkDatabase.NameEntry("rtx 3070 ti laptop gpu", 475),
		new GpuBenchmarkDatabase.NameEntry("rtx 3080 ti laptop gpu", 565),
		new GpuBenchmarkDatabase.NameEntry("rtx 5070 ti laptop gpu", 615),
		new GpuBenchmarkDatabase.NameEntry("iris plus graphics g4", 95),
		new GpuBenchmarkDatabase.NameEntry("iris plus graphics g7", 125),
		new GpuBenchmarkDatabase.NameEntry("iris pro graphics 580", 85),
		new GpuBenchmarkDatabase.NameEntry("iris xe graphics 64eu", 105),
		new GpuBenchmarkDatabase.NameEntry("iris xe graphics 80eu", 120),
		new GpuBenchmarkDatabase.NameEntry("iris xe graphics 96eu", 145),
		new GpuBenchmarkDatabase.NameEntry("rtx 2070 super mobile", 395),
		new GpuBenchmarkDatabase.NameEntry("rtx 2080 super mobile", 455),
		new GpuBenchmarkDatabase.NameEntry("arc graphics 4 core", 145),
		new GpuBenchmarkDatabase.NameEntry("arc graphics 7 core", 210),
		new GpuBenchmarkDatabase.NameEntry("arc graphics 8 core", 235),
		new GpuBenchmarkDatabase.NameEntry("rtx 3050 laptop gpu", 255),
		new GpuBenchmarkDatabase.NameEntry("rtx 3060 laptop gpu", 355),
		new GpuBenchmarkDatabase.NameEntry("rtx 3070 laptop gpu", 430),
		new GpuBenchmarkDatabase.NameEntry("rtx 3080 laptop gpu", 520),
		new GpuBenchmarkDatabase.NameEntry("rtx 4050 laptop gpu", 340),
		new GpuBenchmarkDatabase.NameEntry("rtx 4060 laptop gpu", 420),
		new GpuBenchmarkDatabase.NameEntry("rtx 4070 laptop gpu", 485),
		new GpuBenchmarkDatabase.NameEntry("rtx 4080 laptop gpu", 675),
		new GpuBenchmarkDatabase.NameEntry("rtx 4090 laptop gpu", 790),
		new GpuBenchmarkDatabase.NameEntry("rtx 5050 laptop gpu", 345),
		new GpuBenchmarkDatabase.NameEntry("rtx 5060 laptop gpu", 430),
		new GpuBenchmarkDatabase.NameEntry("rtx 5070 laptop gpu", 505),
		new GpuBenchmarkDatabase.NameEntry("rtx 5080 laptop gpu", 735),
		new GpuBenchmarkDatabase.NameEntry("rtx 5090 laptop gpu", 850),
		new GpuBenchmarkDatabase.NameEntry("gtx 1050 ti mobile", 170),
		new GpuBenchmarkDatabase.NameEntry("gtx 1650 ti mobile", 205),
		new GpuBenchmarkDatabase.NameEntry("gtx 1660 ti mobile", 285),
		new GpuBenchmarkDatabase.NameEntry("iris graphics 5100", 43),
		new GpuBenchmarkDatabase.NameEntry("iris graphics 6100", 55),
		new GpuBenchmarkDatabase.NameEntry("iris graphics 540", 65),
		new GpuBenchmarkDatabase.NameEntry("iris graphics 550", 70),
		new GpuBenchmarkDatabase.NameEntry("rtx 4070 ti super", 725),
		new GpuBenchmarkDatabase.NameEntry("rx vega 56 mobile", 285),
		new GpuBenchmarkDatabase.NameEntry("rx vega 64 mobile", 315),
		new GpuBenchmarkDatabase.NameEntry("gtx 650 ti boost", 120),
		new GpuBenchmarkDatabase.NameEntry("hd graphics 2500", 18),
		new GpuBenchmarkDatabase.NameEntry("hd graphics 4000", 26),
		new GpuBenchmarkDatabase.NameEntry("hd graphics 4200", 26),
		new GpuBenchmarkDatabase.NameEntry("hd graphics 4400", 30),
		new GpuBenchmarkDatabase.NameEntry("hd graphics 4600", 35),
		new GpuBenchmarkDatabase.NameEntry("hd graphics 5000", 38),
		new GpuBenchmarkDatabase.NameEntry("hd graphics 5300", 38),
		new GpuBenchmarkDatabase.NameEntry("hd graphics 5500", 42),
		new GpuBenchmarkDatabase.NameEntry("hd graphics 5600", 44),
		new GpuBenchmarkDatabase.NameEntry("hd graphics 6000", 48),
		new GpuBenchmarkDatabase.NameEntry("rtx 4060 ti 16gb", 420),
		new GpuBenchmarkDatabase.NameEntry("rtx 5060 ti 16gb", 515),
		new GpuBenchmarkDatabase.NameEntry("rx 6750 gre 10gb", 385),
		new GpuBenchmarkDatabase.NameEntry("rx 6750 gre 12gb", 395),
		new GpuBenchmarkDatabase.NameEntry("uhd graphics 600", 32),
		new GpuBenchmarkDatabase.NameEntry("uhd graphics 605", 36),
		new GpuBenchmarkDatabase.NameEntry("uhd graphics 610", 45),
		new GpuBenchmarkDatabase.NameEntry("uhd graphics 615", 50),
		new GpuBenchmarkDatabase.NameEntry("uhd graphics 620", 58),
		new GpuBenchmarkDatabase.NameEntry("uhd graphics 630", 63),
		new GpuBenchmarkDatabase.NameEntry("uhd graphics 730", 72),
		new GpuBenchmarkDatabase.NameEntry("uhd graphics 750", 82),
		new GpuBenchmarkDatabase.NameEntry("uhd graphics 770", 90),
		new GpuBenchmarkDatabase.NameEntry("vega 10 graphics", 95),
		new GpuBenchmarkDatabase.NameEntry("vega 11 graphics", 105),
		new GpuBenchmarkDatabase.NameEntry("gtx 1050 mobile", 145),
		new GpuBenchmarkDatabase.NameEntry("gtx 1060 mobile", 235),
		new GpuBenchmarkDatabase.NameEntry("gtx 1070 mobile", 300),
		new GpuBenchmarkDatabase.NameEntry("gtx 1080 mobile", 355),
		new GpuBenchmarkDatabase.NameEntry("gtx 1650 mobile", 180),
		new GpuBenchmarkDatabase.NameEntry("gtx titan black", 250),
		new GpuBenchmarkDatabase.NameEntry("hd graphics 510", 40),
		new GpuBenchmarkDatabase.NameEntry("hd graphics 515", 43),
		new GpuBenchmarkDatabase.NameEntry("hd graphics 520", 48),
		new GpuBenchmarkDatabase.NameEntry("hd graphics 530", 52),
		new GpuBenchmarkDatabase.NameEntry("hd graphics 610", 42),
		new GpuBenchmarkDatabase.NameEntry("hd graphics 615", 48),
		new GpuBenchmarkDatabase.NameEntry("hd graphics 620", 55),
		new GpuBenchmarkDatabase.NameEntry("hd graphics 630", 60),
		new GpuBenchmarkDatabase.NameEntry("rtx 2060 mobile", 300),
		new GpuBenchmarkDatabase.NameEntry("rtx 2070 mobile", 355),
		new GpuBenchmarkDatabase.NameEntry("rtx 2080 mobile", 420),
		new GpuBenchmarkDatabase.NameEntry("rtx 4060 ti 8gb", 410),
		new GpuBenchmarkDatabase.NameEntry("rtx 5060 ti 8gb", 480),
		new GpuBenchmarkDatabase.NameEntry("rx 9060 xt 16gb", 470),
		new GpuBenchmarkDatabase.NameEntry("vega 3 graphics", 45),
		new GpuBenchmarkDatabase.NameEntry("vega 6 graphics", 65),
		new GpuBenchmarkDatabase.NameEntry("vega 7 graphics", 80),
		new GpuBenchmarkDatabase.NameEntry("vega 8 graphics", 92),
		new GpuBenchmarkDatabase.NameEntry("gtx 1650 gddr6", 130),
		new GpuBenchmarkDatabase.NameEntry("gtx 1650 super", 160),
		new GpuBenchmarkDatabase.NameEntry("gtx 1660 super", 195),
		new GpuBenchmarkDatabase.NameEntry("rtx 2060 super", 265),
		new GpuBenchmarkDatabase.NameEntry("rtx 2070 super", 305),
		new GpuBenchmarkDatabase.NameEntry("rtx 2080 super", 345),
		new GpuBenchmarkDatabase.NameEntry("rtx 4070 super", 635),
		new GpuBenchmarkDatabase.NameEntry("rtx 4080 super", 830),
		new GpuBenchmarkDatabase.NameEntry("rx 5500 xt 4gb", 175),
		new GpuBenchmarkDatabase.NameEntry("rx 5500 xt 8gb", 175),
		new GpuBenchmarkDatabase.NameEntry("rx 9060 xt 8gb", 435),
		new GpuBenchmarkDatabase.NameEntry("titan x pascal", 325),
		new GpuBenchmarkDatabase.NameEntry("arc a770 16gb", 310),
		new GpuBenchmarkDatabase.NameEntry("rtx 2060 12gb", 235),
		new GpuBenchmarkDatabase.NameEntry("rtx 3060 12gb", 295),
		new GpuBenchmarkDatabase.NameEntry("rtx 3080 10gb", 570),
		new GpuBenchmarkDatabase.NameEntry("rtx 3080 12gb", 595),
		new GpuBenchmarkDatabase.NameEntry("rx 460 mobile", 100),
		new GpuBenchmarkDatabase.NameEntry("rx 560 mobile", 115),
		new GpuBenchmarkDatabase.NameEntry("rx 570 mobile", 185),
		new GpuBenchmarkDatabase.NameEntry("rx 580 mobile", 225),
		new GpuBenchmarkDatabase.NameEntry("arc a770 8gb", 305),
		new GpuBenchmarkDatabase.NameEntry("gt 1030 ddr4", 28),
		new GpuBenchmarkDatabase.NameEntry("gtx 1050 3gb", 105),
		new GpuBenchmarkDatabase.NameEntry("gtx 1060 3gb", 145),
		new GpuBenchmarkDatabase.NameEntry("gtx 1060 5gb", 155),
		new GpuBenchmarkDatabase.NameEntry("gtx 1060 6gb", 160),
		new GpuBenchmarkDatabase.NameEntry("rtx 3050 6gb", 165),
		new GpuBenchmarkDatabase.NameEntry("rtx 3050 8gb", 205),
		new GpuBenchmarkDatabase.NameEntry("rtx 3060 8gb", 270),
		new GpuBenchmarkDatabase.NameEntry("gtx 1050 ti", 120),
		new GpuBenchmarkDatabase.NameEntry("gtx 1070 ti", 230),
		new GpuBenchmarkDatabase.NameEntry("gtx 1080 ti", 335),
		new GpuBenchmarkDatabase.NameEntry("gtx 1660 ti", 205),
		new GpuBenchmarkDatabase.NameEntry("gtx 960 2gb", 125),
		new GpuBenchmarkDatabase.NameEntry("gtx 960 4gb", 125),
		new GpuBenchmarkDatabase.NameEntry("gtx titan x", 285),
		new GpuBenchmarkDatabase.NameEntry("gtx titan z", 285),
		new GpuBenchmarkDatabase.NameEntry("r5 graphics", 32),
		new GpuBenchmarkDatabase.NameEntry("r6 graphics", 42),
		new GpuBenchmarkDatabase.NameEntry("r7 graphics", 52),
		new GpuBenchmarkDatabase.NameEntry("rtx 2080 ti", 410),
		new GpuBenchmarkDatabase.NameEntry("rtx 3060 ti", 360),
		new GpuBenchmarkDatabase.NameEntry("rtx 3070 ti", 465),
		new GpuBenchmarkDatabase.NameEntry("rtx 3080 ti", 625),
		new GpuBenchmarkDatabase.NameEntry("rtx 3090 ti", 690),
		new GpuBenchmarkDatabase.NameEntry("rtx 4060 ti", 420),
		new GpuBenchmarkDatabase.NameEntry("rtx 4070 ti", 680),
		new GpuBenchmarkDatabase.NameEntry("rtx 5060 ti", 515),
		new GpuBenchmarkDatabase.NameEntry("rtx 5070 ti", 815),
		new GpuBenchmarkDatabase.NameEntry("rx 6650m xt", 380),
		new GpuBenchmarkDatabase.NameEntry("rx 6750 gre", 395),
		new GpuBenchmarkDatabase.NameEntry("rx 6850m xt", 525),
		new GpuBenchmarkDatabase.NameEntry("rx 7600m xt", 400),
		new GpuBenchmarkDatabase.NameEntry("rx 7900 gre", 635),
		new GpuBenchmarkDatabase.NameEntry("rx 7900 xtx", 855),
		new GpuBenchmarkDatabase.NameEntry("rx 9070 gre", 605),
		new GpuBenchmarkDatabase.NameEntry("gtx 650 ti", 95),
		new GpuBenchmarkDatabase.NameEntry("gtx 660 ti", 155),
		new GpuBenchmarkDatabase.NameEntry("gtx 750 ti", 115),
		new GpuBenchmarkDatabase.NameEntry("gtx 780 ti", 245),
		new GpuBenchmarkDatabase.NameEntry("gtx 980 ti", 275),
		new GpuBenchmarkDatabase.NameEntry("rx 460 2gb", 80),
		new GpuBenchmarkDatabase.NameEntry("rx 460 4gb", 80),
		new GpuBenchmarkDatabase.NameEntry("rx 480 4gb", 145),
		new GpuBenchmarkDatabase.NameEntry("rx 480 8gb", 145),
		new GpuBenchmarkDatabase.NameEntry("rx 550 2gb", 55),
		new GpuBenchmarkDatabase.NameEntry("rx 550 4gb", 55),
		new GpuBenchmarkDatabase.NameEntry("rx 5500 xt", 175),
		new GpuBenchmarkDatabase.NameEntry("rx 560 2gb", 75),
		new GpuBenchmarkDatabase.NameEntry("rx 560 4gb", 75),
		new GpuBenchmarkDatabase.NameEntry("rx 5600 xt", 230),
		new GpuBenchmarkDatabase.NameEntry("rx 570 4gb", 135),
		new GpuBenchmarkDatabase.NameEntry("rx 570 8gb", 135),
		new GpuBenchmarkDatabase.NameEntry("rx 5700 xt", 295),
		new GpuBenchmarkDatabase.NameEntry("rx 580 4gb", 155),
		new GpuBenchmarkDatabase.NameEntry("rx 580 8gb", 155),
		new GpuBenchmarkDatabase.NameEntry("rx 6500 xt", 155),
		new GpuBenchmarkDatabase.NameEntry("rx 6600 xt", 285),
		new GpuBenchmarkDatabase.NameEntry("rx 6650 xt", 305),
		new GpuBenchmarkDatabase.NameEntry("rx 6700 xt", 380),
		new GpuBenchmarkDatabase.NameEntry("rx 6750 xt", 405),
		new GpuBenchmarkDatabase.NameEntry("rx 6800 xt", 555),
		new GpuBenchmarkDatabase.NameEntry("rx 6900 xt", 585),
		new GpuBenchmarkDatabase.NameEntry("rx 6950 xt", 625),
		new GpuBenchmarkDatabase.NameEntry("rx 7600 xt", 350),
		new GpuBenchmarkDatabase.NameEntry("rx 7700 xt", 505),
		new GpuBenchmarkDatabase.NameEntry("rx 7800 xt", 590),
		new GpuBenchmarkDatabase.NameEntry("rx 7900 xt", 755),
		new GpuBenchmarkDatabase.NameEntry("rx 9060 xt", 470),
		new GpuBenchmarkDatabase.NameEntry("rx 9070 xt", 815),
		new GpuBenchmarkDatabase.NameEntry("rx vega 56", 210),
		new GpuBenchmarkDatabase.NameEntry("rx vega 64", 230),
		new GpuBenchmarkDatabase.NameEntry("arc a350m", 120),
		new GpuBenchmarkDatabase.NameEntry("arc a370m", 155),
		new GpuBenchmarkDatabase.NameEntry("arc a530m", 220),
		new GpuBenchmarkDatabase.NameEntry("arc a550m", 245),
		new GpuBenchmarkDatabase.NameEntry("arc a570m", 275),
		new GpuBenchmarkDatabase.NameEntry("arc a730m", 335),
		new GpuBenchmarkDatabase.NameEntry("arc a770m", 385),
		new GpuBenchmarkDatabase.NameEntry("gtx titan", 230),
		new GpuBenchmarkDatabase.NameEntry("r9 fury x", 285),
		new GpuBenchmarkDatabase.NameEntry("titan rtx", 425),
		new GpuBenchmarkDatabase.NameEntry("arc 130v", 215),
		new GpuBenchmarkDatabase.NameEntry("arc 140v", 275),
		new GpuBenchmarkDatabase.NameEntry("arc a310", 95),
		new GpuBenchmarkDatabase.NameEntry("arc a380", 135),
		new GpuBenchmarkDatabase.NameEntry("arc a580", 235),
		new GpuBenchmarkDatabase.NameEntry("arc a750", 285),
		new GpuBenchmarkDatabase.NameEntry("arc a770", 310),
		new GpuBenchmarkDatabase.NameEntry("arc b570", 310),
		new GpuBenchmarkDatabase.NameEntry("arc b580", 355),
		new GpuBenchmarkDatabase.NameEntry("gtx 1050", 105),
		new GpuBenchmarkDatabase.NameEntry("gtx 1060", 160),
		new GpuBenchmarkDatabase.NameEntry("gtx 1070", 205),
		new GpuBenchmarkDatabase.NameEntry("gtx 1080", 260),
		new GpuBenchmarkDatabase.NameEntry("gtx 1630", 90),
		new GpuBenchmarkDatabase.NameEntry("gtx 1650", 125),
		new GpuBenchmarkDatabase.NameEntry("gtx 1660", 175),
		new GpuBenchmarkDatabase.NameEntry("gtx 660m", 80),
		new GpuBenchmarkDatabase.NameEntry("gtx 670m", 95),
		new GpuBenchmarkDatabase.NameEntry("gtx 675m", 105),
		new GpuBenchmarkDatabase.NameEntry("gtx 680m", 125),
		new GpuBenchmarkDatabase.NameEntry("gtx 760m", 95),
		new GpuBenchmarkDatabase.NameEntry("gtx 765m", 110),
		new GpuBenchmarkDatabase.NameEntry("gtx 770m", 125),
		new GpuBenchmarkDatabase.NameEntry("gtx 780m", 150),
		new GpuBenchmarkDatabase.NameEntry("gtx 850m", 95),
		new GpuBenchmarkDatabase.NameEntry("gtx 860m", 115),
		new GpuBenchmarkDatabase.NameEntry("gtx 870m", 140),
		new GpuBenchmarkDatabase.NameEntry("gtx 880m", 160),
		new GpuBenchmarkDatabase.NameEntry("gtx 950m", 105),
		new GpuBenchmarkDatabase.NameEntry("gtx 960m", 125),
		new GpuBenchmarkDatabase.NameEntry("gtx 965m", 150),
		new GpuBenchmarkDatabase.NameEntry("gtx 970m", 190),
		new GpuBenchmarkDatabase.NameEntry("gtx 980m", 225),
		new GpuBenchmarkDatabase.NameEntry("hd 7480d", 28),
		new GpuBenchmarkDatabase.NameEntry("hd 7540d", 32),
		new GpuBenchmarkDatabase.NameEntry("hd 7660d", 40),
		new GpuBenchmarkDatabase.NameEntry("r9 m270x", 90),
		new GpuBenchmarkDatabase.NameEntry("r9 m280x", 105),
		new GpuBenchmarkDatabase.NameEntry("r9 m290x", 125),
		new GpuBenchmarkDatabase.NameEntry("r9 m295x", 145),
		new GpuBenchmarkDatabase.NameEntry("r9 m370x", 95),
		new GpuBenchmarkDatabase.NameEntry("r9 m390x", 145),
		new GpuBenchmarkDatabase.NameEntry("rtx 2060", 235),
		new GpuBenchmarkDatabase.NameEntry("rtx 2070", 275),
		new GpuBenchmarkDatabase.NameEntry("rtx 2080", 325),
		new GpuBenchmarkDatabase.NameEntry("rtx 3050", 205),
		new GpuBenchmarkDatabase.NameEntry("rtx 3060", 295),
		new GpuBenchmarkDatabase.NameEntry("rtx 3070", 410),
		new GpuBenchmarkDatabase.NameEntry("rtx 3080", 595),
		new GpuBenchmarkDatabase.NameEntry("rtx 3090", 640),
		new GpuBenchmarkDatabase.NameEntry("rtx 4060", 335),
		new GpuBenchmarkDatabase.NameEntry("rtx 4070", 545),
		new GpuBenchmarkDatabase.NameEntry("rtx 4080", 820),
		new GpuBenchmarkDatabase.NameEntry("rtx 4090", 1000),
		new GpuBenchmarkDatabase.NameEntry("rtx 5050", 315),
		new GpuBenchmarkDatabase.NameEntry("rtx 5060", 420),
		new GpuBenchmarkDatabase.NameEntry("rtx 5070", 675),
		new GpuBenchmarkDatabase.NameEntry("rtx 5080", 895),
		new GpuBenchmarkDatabase.NameEntry("rtx 5090", 1170),
		new GpuBenchmarkDatabase.NameEntry("rx 5300m", 210),
		new GpuBenchmarkDatabase.NameEntry("rx 5500m", 255),
		new GpuBenchmarkDatabase.NameEntry("rx 5600m", 315),
		new GpuBenchmarkDatabase.NameEntry("rx 5700m", 365),
		new GpuBenchmarkDatabase.NameEntry("rx 6300m", 180),
		new GpuBenchmarkDatabase.NameEntry("rx 6500m", 240),
		new GpuBenchmarkDatabase.NameEntry("rx 6550m", 260),
		new GpuBenchmarkDatabase.NameEntry("rx 6600m", 335),
		new GpuBenchmarkDatabase.NameEntry("rx 6700m", 420),
		new GpuBenchmarkDatabase.NameEntry("rx 6800m", 490),
		new GpuBenchmarkDatabase.NameEntry("rx 7600m", 360),
		new GpuBenchmarkDatabase.NameEntry("rx 7600s", 335),
		new GpuBenchmarkDatabase.NameEntry("rx 7700s", 410),
		new GpuBenchmarkDatabase.NameEntry("rx 7800m", 520),
		new GpuBenchmarkDatabase.NameEntry("rx 7900m", 650),
		new GpuBenchmarkDatabase.NameEntry("titan xp", 345),
		new GpuBenchmarkDatabase.NameEntry("gt 1010", 42),
		new GpuBenchmarkDatabase.NameEntry("gt 1030", 50),
		new GpuBenchmarkDatabase.NameEntry("gtx 650", 75),
		new GpuBenchmarkDatabase.NameEntry("gtx 660", 135),
		new GpuBenchmarkDatabase.NameEntry("gtx 670", 175),
		new GpuBenchmarkDatabase.NameEntry("gtx 680", 190),
		new GpuBenchmarkDatabase.NameEntry("gtx 690", 220),
		new GpuBenchmarkDatabase.NameEntry("gtx 750", 95),
		new GpuBenchmarkDatabase.NameEntry("gtx 760", 155),
		new GpuBenchmarkDatabase.NameEntry("gtx 770", 185),
		new GpuBenchmarkDatabase.NameEntry("gtx 780", 215),
		new GpuBenchmarkDatabase.NameEntry("gtx 950", 105),
		new GpuBenchmarkDatabase.NameEntry("gtx 960", 125),
		new GpuBenchmarkDatabase.NameEntry("gtx 970", 175),
		new GpuBenchmarkDatabase.NameEntry("gtx 980", 220),
		new GpuBenchmarkDatabase.NameEntry("hd 7750", 65),
		new GpuBenchmarkDatabase.NameEntry("hd 7770", 80),
		new GpuBenchmarkDatabase.NameEntry("hd 7790", 105),
		new GpuBenchmarkDatabase.NameEntry("hd 7850", 125),
		new GpuBenchmarkDatabase.NameEntry("hd 7870", 150),
		new GpuBenchmarkDatabase.NameEntry("hd 7950", 175),
		new GpuBenchmarkDatabase.NameEntry("hd 7970", 200),
		new GpuBenchmarkDatabase.NameEntry("r7 250x", 70),
		new GpuBenchmarkDatabase.NameEntry("r7 260x", 100),
		new GpuBenchmarkDatabase.NameEntry("r9 270x", 150),
		new GpuBenchmarkDatabase.NameEntry("r9 280x", 195),
		new GpuBenchmarkDatabase.NameEntry("r9 290x", 235),
		new GpuBenchmarkDatabase.NameEntry("r9 380x", 185),
		new GpuBenchmarkDatabase.NameEntry("r9 390x", 240),
		new GpuBenchmarkDatabase.NameEntry("r9 fury", 265),
		new GpuBenchmarkDatabase.NameEntry("r9 nano", 260),
		new GpuBenchmarkDatabase.NameEntry("rx 5300", 150),
		new GpuBenchmarkDatabase.NameEntry("rx 540x", 95),
		new GpuBenchmarkDatabase.NameEntry("rx 5500", 165),
		new GpuBenchmarkDatabase.NameEntry("rx 550x", 105),
		new GpuBenchmarkDatabase.NameEntry("rx 5700", 265),
		new GpuBenchmarkDatabase.NameEntry("rx 6400", 130),
		new GpuBenchmarkDatabase.NameEntry("rx 6600", 245),
		new GpuBenchmarkDatabase.NameEntry("rx 6700", 340),
		new GpuBenchmarkDatabase.NameEntry("rx 6800", 490),
		new GpuBenchmarkDatabase.NameEntry("rx 7600", 320),
		new GpuBenchmarkDatabase.NameEntry("rx 9070", 725),
		new GpuBenchmarkDatabase.NameEntry("gt 630", 45),
		new GpuBenchmarkDatabase.NameEntry("gt 640", 60),
		new GpuBenchmarkDatabase.NameEntry("gt 710", 20),
		new GpuBenchmarkDatabase.NameEntry("gt 720", 28),
		new GpuBenchmarkDatabase.NameEntry("gt 730", 42),
		new GpuBenchmarkDatabase.NameEntry("gt 740", 68),
		new GpuBenchmarkDatabase.NameEntry("r7 240", 35),
		new GpuBenchmarkDatabase.NameEntry("r7 250", 55),
		new GpuBenchmarkDatabase.NameEntry("r7 260", 80),
		new GpuBenchmarkDatabase.NameEntry("r7 265", 125),
		new GpuBenchmarkDatabase.NameEntry("r7 360", 95),
		new GpuBenchmarkDatabase.NameEntry("r7 370", 125),
		new GpuBenchmarkDatabase.NameEntry("r9 270", 135),
		new GpuBenchmarkDatabase.NameEntry("r9 280", 170),
		new GpuBenchmarkDatabase.NameEntry("r9 285", 185),
		new GpuBenchmarkDatabase.NameEntry("r9 290", 215),
		new GpuBenchmarkDatabase.NameEntry("r9 380", 165),
		new GpuBenchmarkDatabase.NameEntry("r9 390", 220),
		new GpuBenchmarkDatabase.NameEntry("rx 460", 80),
		new GpuBenchmarkDatabase.NameEntry("rx 470", 125),
		new GpuBenchmarkDatabase.NameEntry("rx 480", 145),
		new GpuBenchmarkDatabase.NameEntry("rx 540", 90),
		new GpuBenchmarkDatabase.NameEntry("rx 550", 55),
		new GpuBenchmarkDatabase.NameEntry("rx 560", 75),
		new GpuBenchmarkDatabase.NameEntry("rx 570", 135),
		new GpuBenchmarkDatabase.NameEntry("rx 580", 155),
		new GpuBenchmarkDatabase.NameEntry("rx 590", 170),
		new GpuBenchmarkDatabase.NameEntry("8050s", 235),
		new GpuBenchmarkDatabase.NameEntry("8060s", 330),
		new GpuBenchmarkDatabase.NameEntry("920mx", 45),
		new GpuBenchmarkDatabase.NameEntry("930mx", 60),
		new GpuBenchmarkDatabase.NameEntry("940mx", 75),
		new GpuBenchmarkDatabase.NameEntry("mx110", 42),
		new GpuBenchmarkDatabase.NameEntry("mx130", 60),
		new GpuBenchmarkDatabase.NameEntry("mx150", 90),
		new GpuBenchmarkDatabase.NameEntry("mx230", 75),
		new GpuBenchmarkDatabase.NameEntry("mx250", 92),
		new GpuBenchmarkDatabase.NameEntry("mx330", 88),
		new GpuBenchmarkDatabase.NameEntry("mx350", 115),
		new GpuBenchmarkDatabase.NameEntry("mx450", 155),
		new GpuBenchmarkDatabase.NameEntry("mx550", 175),
		new GpuBenchmarkDatabase.NameEntry("mx570", 210),
		new GpuBenchmarkDatabase.NameEntry("610m", 72),
		new GpuBenchmarkDatabase.NameEntry("660m", 145),
		new GpuBenchmarkDatabase.NameEntry("680m", 220),
		new GpuBenchmarkDatabase.NameEntry("740m", 120),
		new GpuBenchmarkDatabase.NameEntry("760m", 175),
		new GpuBenchmarkDatabase.NameEntry("780m", 245),
		new GpuBenchmarkDatabase.NameEntry("820m", 35),
		new GpuBenchmarkDatabase.NameEntry("830m", 45),
		new GpuBenchmarkDatabase.NameEntry("840m", 55),
		new GpuBenchmarkDatabase.NameEntry("860m", 230),
		new GpuBenchmarkDatabase.NameEntry("880m", 275),
		new GpuBenchmarkDatabase.NameEntry("890m", 300),
		new GpuBenchmarkDatabase.NameEntry("920m", 35),
		new GpuBenchmarkDatabase.NameEntry("930m", 50),
		new GpuBenchmarkDatabase.NameEntry("940m", 62),
		new GpuBenchmarkDatabase.NameEntry("vii", 300)
	};

	// Token: 0x040027A6 RID: 10150
	private static readonly GpuBenchmarkDatabase.PciEntry[] s_pci = new GpuBenchmarkDatabase.PciEntry[]
	{
		new GpuBenchmarkDatabase.PciEntry(4098, 29605, 625),
		new GpuBenchmarkDatabase.PciEntry(4098, 29615, 585),
		new GpuBenchmarkDatabase.PciEntry(4318, 5056, 220),
		new GpuBenchmarkDatabase.PciEntry(4318, 5058, 175),
		new GpuBenchmarkDatabase.PciEntry(4318, 5121, 125),
		new GpuBenchmarkDatabase.PciEntry(4318, 5126, 125),
		new GpuBenchmarkDatabase.PciEntry(4318, 6088, 275),
		new GpuBenchmarkDatabase.PciEntry(4318, 6918, 335),
		new GpuBenchmarkDatabase.PciEntry(4318, 7040, 260),
		new GpuBenchmarkDatabase.PciEntry(4318, 7041, 205),
		new GpuBenchmarkDatabase.PciEntry(4318, 7042, 230),
		new GpuBenchmarkDatabase.PciEntry(4318, 7170, 145),
		new GpuBenchmarkDatabase.PciEntry(4318, 7171, 160),
		new GpuBenchmarkDatabase.PciEntry(4318, 7172, 155),
		new GpuBenchmarkDatabase.PciEntry(4318, 7174, 160),
		new GpuBenchmarkDatabase.PciEntry(4318, 7297, 100),
		new GpuBenchmarkDatabase.PciEntry(4318, 7298, 120),
		new GpuBenchmarkDatabase.PciEntry(4318, 7299, 105),
		new GpuBenchmarkDatabase.PciEntry(4318, 7425, 50),
		new GpuBenchmarkDatabase.PciEntry(4318, 7684, 410),
		new GpuBenchmarkDatabase.PciEntry(4318, 7687, 410),
		new GpuBenchmarkDatabase.PciEntry(4318, 7809, 345),
		new GpuBenchmarkDatabase.PciEntry(4318, 7812, 305),
		new GpuBenchmarkDatabase.PciEntry(4318, 7815, 325),
		new GpuBenchmarkDatabase.PciEntry(4318, 7817, 225),
		new GpuBenchmarkDatabase.PciEntry(4318, 7938, 275),
		new GpuBenchmarkDatabase.PciEntry(4318, 7942, 265),
		new GpuBenchmarkDatabase.PciEntry(4318, 8085, 205),
		new GpuBenchmarkDatabase.PciEntry(4318, 8578, 205),
		new GpuBenchmarkDatabase.PciEntry(4318, 8580, 175),
		new GpuBenchmarkDatabase.PciEntry(4318, 8583, 160),
		new GpuBenchmarkDatabase.PciEntry(4318, 8707, 690),
		new GpuBenchmarkDatabase.PciEntry(4318, 8708, 640),
		new GpuBenchmarkDatabase.PciEntry(4318, 8726, 570),
		new GpuBenchmarkDatabase.PciEntry(4318, 9348, 410),
		new GpuBenchmarkDatabase.PciEntry(4318, 9350, 360),
		new GpuBenchmarkDatabase.PciEntry(4318, 9351, 295),
		new GpuBenchmarkDatabase.PciEntry(4318, 9352, 410),
		new GpuBenchmarkDatabase.PciEntry(4318, 9353, 360),
		new GpuBenchmarkDatabase.PciEntry(4318, 9475, 295),
		new GpuBenchmarkDatabase.PciEntry(4318, 9476, 295),
		new GpuBenchmarkDatabase.PciEntry(4318, 9504, 355),
		new GpuBenchmarkDatabase.PciEntry(4318, 9568, 355),
		new GpuBenchmarkDatabase.PciEntry(4318, 9860, 1000),
		new GpuBenchmarkDatabase.PciEntry(4318, 9865, 725),
		new GpuBenchmarkDatabase.PciEntry(4318, 9986, 830),
		new GpuBenchmarkDatabase.PciEntry(4318, 9987, 830),
		new GpuBenchmarkDatabase.PciEntry(4318, 9988, 820),
		new GpuBenchmarkDatabase.PciEntry(4318, 9989, 725),
		new GpuBenchmarkDatabase.PciEntry(4318, 9993, 545),
		new GpuBenchmarkDatabase.PciEntry(4318, 10007, 790),
		new GpuBenchmarkDatabase.PciEntry(4318, 10071, 790),
		new GpuBenchmarkDatabase.PciEntry(4318, 10114, 680),
		new GpuBenchmarkDatabase.PciEntry(4318, 10115, 635),
		new GpuBenchmarkDatabase.PciEntry(4318, 10118, 545),
		new GpuBenchmarkDatabase.PciEntry(4318, 10120, 410),
		new GpuBenchmarkDatabase.PciEntry(4318, 10144, 675),
		new GpuBenchmarkDatabase.PciEntry(4318, 10208, 675),
		new GpuBenchmarkDatabase.PciEntry(4318, 10243, 410),
		new GpuBenchmarkDatabase.PciEntry(4318, 10245, 420),
		new GpuBenchmarkDatabase.PciEntry(4318, 10248, 335),
		new GpuBenchmarkDatabase.PciEntry(4318, 10272, 485),
		new GpuBenchmarkDatabase.PciEntry(4318, 10336, 485),
		new GpuBenchmarkDatabase.PciEntry(4318, 10370, 335),
		new GpuBenchmarkDatabase.PciEntry(4318, 10400, 420),
		new GpuBenchmarkDatabase.PciEntry(4318, 10401, 340),
		new GpuBenchmarkDatabase.PciEntry(4318, 10464, 420),
		new GpuBenchmarkDatabase.PciEntry(4318, 10465, 340),
		new GpuBenchmarkDatabase.PciEntry(4318, 11141, 1170),
		new GpuBenchmarkDatabase.PciEntry(4318, 11266, 895),
		new GpuBenchmarkDatabase.PciEntry(4318, 11269, 815),
		new GpuBenchmarkDatabase.PciEntry(4318, 11273, 675),
		new GpuBenchmarkDatabase.PciEntry(4318, 11288, 850),
		new GpuBenchmarkDatabase.PciEntry(4318, 11289, 735),
		new GpuBenchmarkDatabase.PciEntry(4318, 11352, 850),
		new GpuBenchmarkDatabase.PciEntry(4318, 11353, 735),
		new GpuBenchmarkDatabase.PciEntry(4318, 11524, 515),
		new GpuBenchmarkDatabase.PciEntry(4318, 11525, 420),
		new GpuBenchmarkDatabase.PciEntry(4318, 11544, 505),
		new GpuBenchmarkDatabase.PciEntry(4318, 11545, 430),
		new GpuBenchmarkDatabase.PciEntry(4318, 11608, 505),
		new GpuBenchmarkDatabase.PciEntry(4318, 11609, 430),
		new GpuBenchmarkDatabase.PciEntry(4318, 11651, 315),
		new GpuBenchmarkDatabase.PciEntry(4318, 11672, 345),
		new GpuBenchmarkDatabase.PciEntry(4318, 11736, 345),
		new GpuBenchmarkDatabase.PciEntry(4318, 12036, 675),
		new GpuBenchmarkDatabase.PciEntry(4318, 12038, 420),
		new GpuBenchmarkDatabase.PciEntry(4318, 12056, 615),
		new GpuBenchmarkDatabase.PciEntry(4318, 12120, 615),
		new GpuBenchmarkDatabase.PciEntry(32902, 22160, 385),
		new GpuBenchmarkDatabase.PciEntry(32902, 22161, 335),
		new GpuBenchmarkDatabase.PciEntry(32902, 22162, 245),
		new GpuBenchmarkDatabase.PciEntry(32902, 22163, 155),
		new GpuBenchmarkDatabase.PciEntry(32902, 22164, 120),
		new GpuBenchmarkDatabase.PciEntry(32902, 22166, 275),
		new GpuBenchmarkDatabase.PciEntry(32902, 22167, 220),
		new GpuBenchmarkDatabase.PciEntry(32902, 22176, 310),
		new GpuBenchmarkDatabase.PciEntry(32902, 22177, 285),
		new GpuBenchmarkDatabase.PciEntry(32902, 22178, 235),
		new GpuBenchmarkDatabase.PciEntry(32902, 22181, 135),
		new GpuBenchmarkDatabase.PciEntry(32902, 22182, 95),
		new GpuBenchmarkDatabase.PciEntry(32902, 57865, 355),
		new GpuBenchmarkDatabase.PciEntry(32902, 57867, 355),
		new GpuBenchmarkDatabase.PciEntry(32902, 57868, 310)
	};

	// Token: 0x02000779 RID: 1913
	private readonly struct NameEntry
	{
		// Token: 0x060031A3 RID: 12707 RVA: 0x000EDEF3 File Offset: 0x000EC0F3
		public NameEntry(string match, int points)
		{
			this.Match = match;
			this.Points = points;
		}

		// Token: 0x040027A7 RID: 10151
		public readonly string Match;

		// Token: 0x040027A8 RID: 10152
		public readonly int Points;
	}

	// Token: 0x0200077A RID: 1914
	private readonly struct PciEntry
	{
		// Token: 0x060031A4 RID: 12708 RVA: 0x000EDF03 File Offset: 0x000EC103
		public PciEntry(int vendor, int device, int points)
		{
			this.Vendor = vendor;
			this.Device = device;
			this.Points = points;
		}

		// Token: 0x040027A9 RID: 10153
		public readonly int Vendor;

		// Token: 0x040027AA RID: 10154
		public readonly int Device;

		// Token: 0x040027AB RID: 10155
		public readonly int Points;
	}
}
