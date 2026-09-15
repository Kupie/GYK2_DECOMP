using System;
using System.Collections.Generic;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x020000DB RID: 219
	[Serializable]
	public class ApplicationSettings
	{
		// Token: 0x040001A3 RID: 419
		public const string XBOX_ONE_DEFAULT_CONFIGURATION_PATH = "ProjectSettings/XboxOneGame.config";

		// Token: 0x040001A4 RID: 420
		public const string XBOX_SCARLETT_DEFAULT_CONFIGURATION_PATH = "ProjectSettings/ScarlettGame.config";

		// Token: 0x040001A5 RID: 421
		[Header("General")]
		public LazyBuildType buildType;

		// Token: 0x040001A6 RID: 422
		[Tooltip("Defines which should be included only for this build type")]
		public List<string> exclusiveDefines;

		// Token: 0x040001A7 RID: 423
		public string applicationName;

		// Token: 0x040001A8 RID: 424
		public bool showUnityLogo;

		// Token: 0x040001A9 RID: 425
		[Header("Standalone")]
		public string pathToSteamPreorderFile;

		// Token: 0x040001AA RID: 426
		[Header("Nintendo Switch")]
		[Tooltip("Uses to access saves from other application\nPlease use id without \"0x\" prefix")]
		public List<ApplicationSettings.NintendoApplicationData> switchOtherApplicationsData;

		// Token: 0x040001AB RID: 427
		[Tooltip("Path to NMETAOverride file")]
		public string NMETAOverride;

		// Token: 0x040001AC RID: 428
		[Tooltip("Save folder path")]
		public string switchSaveFolderPath = "saveData";

		// Token: 0x040001AD RID: 429
		[Header("Nintendo Switch")]
		[Tooltip("Uses to access saves from other application\nPlease use id without \"0x\" prefix")]
		public List<ApplicationSettings.NintendoApplicationData> switch2OtherApplicationsData;

		// Token: 0x040001AE RID: 430
		[Tooltip("Path to NMETAOverride file")]
		public string NMETAOverrideSwitch2;

		// Token: 0x040001AF RID: 431
		[Tooltip("Save folder path")]
		public string switch2SaveFolderPath = "saveData2";

		// Token: 0x040001B0 RID: 432
		[Header("Xbox")]
		public string xboxSCID;

		// Token: 0x040001B1 RID: 433
		public bool advancedUserModel;

		// Token: 0x040001B2 RID: 434
		[Tooltip("Path to XboxOneGame.config\nIt must be different with XBOX_DEFAULT_CONFIGURATION_PATH")]
		public string xboxConfigPath;

		// Token: 0x040001B3 RID: 435
		public string xboxOverrodeResourcesPath;

		// Token: 0x040001B4 RID: 436
		public string xboxOverrodeLocalizationPath;

		// Token: 0x040001B5 RID: 437
		[Tooltip("Uses to access saves from other applications")]
		public List<string> xboxOtherApplicationIds;

		// Token: 0x040001B6 RID: 438
		[Header("PS4")]
		[Header("PS4")]
		public string pathToPS4ParamFile;

		// Token: 0x040001B7 RID: 439
		public int parentalLevelPS4;

		// Token: 0x040001B8 RID: 440
		public int ageRatingPS4;

		// Token: 0x040001B9 RID: 441
		public string pathToPS4BackgroundImage;

		// Token: 0x040001BA RID: 442
		public string pathToPS4StartupImagePath;

		// Token: 0x040001BB RID: 443
		public string npTitleSecretPS4;

		// Token: 0x040001BC RID: 444
		public string npTitleDatPathPS4;

		// Token: 0x040001BD RID: 445
		public int appTypePS4;

		// Token: 0x040001BE RID: 446
		public int categoryPS4;

		// Token: 0x040001BF RID: 447
		public string npTrophyPackPathPS4;

		// Token: 0x040001C0 RID: 448
		public SonyNPAgeRestriction[] sonyNPAgeRestrictions;

		// Token: 0x040001C1 RID: 449
		[Tooltip("For PlayerPrefs")]
		public string pathToPS4SaveDataImage;

		// Token: 0x040001C2 RID: 450
		[Tooltip("For savedata2 plugin")]
		public string pathToPS4SaveDataImageStreaming = "/app0/Media/StreamingAssets/SaveIcon.png";

		// Token: 0x040001C3 RID: 451
		public ApplicationSettings.PS4TransferringData ps4TransferringData = new ApplicationSettings.PS4TransferringData();

		// Token: 0x040001C4 RID: 452
		public string ps4PatchChangeinfoPath;

		// Token: 0x040001C5 RID: 453
		[Header("PS5")]
		[Header("PS5")]
		public List<ApplicationSettings.PS5PackagePair> ps5PackagePairs;

		// Token: 0x040001C6 RID: 454
		public ApplicationSettings.PS5TransferringData ps5TransferringData;

		// Token: 0x040001C7 RID: 455
		[Header("PS5")]
		public string ps5ParamFilePath;

		// Token: 0x040001C8 RID: 456
		[Header("PS5")]
		public string[] sharedBinarySystemFolders;

		// Token: 0x020001C9 RID: 457
		private enum SupportedPlatforms
		{
			// Token: 0x04000617 RID: 1559
			PC,
			// Token: 0x04000618 RID: 1560
			Switch,
			// Token: 0x04000619 RID: 1561
			Xbox,
			// Token: 0x0400061A RID: 1562
			PS4,
			// Token: 0x0400061B RID: 1563
			PS5,
			// Token: 0x0400061C RID: 1564
			Switch2
		}

		// Token: 0x020001CA RID: 458
		[Serializable]
		public class NintendoApplicationData
		{
			// Token: 0x0400061D RID: 1565
			public string applicationId;

			// Token: 0x0400061E RID: 1566
			public string saveFolderPath;
		}

		// Token: 0x020001CB RID: 459
		[Serializable]
		public class PS5PackagePair
		{
			// Token: 0x0400061F RID: 1567
			public string sourcePath;

			// Token: 0x04000620 RID: 1568
			public string contentLabel;
		}

		// Token: 0x020001CC RID: 460
		[Serializable]
		public class PS5TransferringData
		{
			// Token: 0x04000621 RID: 1569
			public string fingerprint;

			// Token: 0x04000622 RID: 1570
			public string titleId;
		}

		// Token: 0x020001CD RID: 461
		[Serializable]
		public class PS4TransferringData
		{
			// Token: 0x04000623 RID: 1571
			public string fingerprint;

			// Token: 0x04000624 RID: 1572
			public string titleId;
		}
	}
}
