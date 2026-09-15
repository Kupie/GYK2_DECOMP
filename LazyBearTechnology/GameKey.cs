using System;

namespace LazyBearTechnology
{
	// Token: 0x02000130 RID: 304
	[Serializable]
	public class GameKey : Enumeration
	{
		// Token: 0x060005FC RID: 1532 RVA: 0x0001E789 File Offset: 0x0001C989
		public GameKey(int value)
			: base(value)
		{
		}

		// Token: 0x0400030B RID: 779
		public static GameKey None = new GameKey(0);

		// Token: 0x0400030C RID: 780
		public static GameKey LeftClick = new GameKey(1);

		// Token: 0x0400030D RID: 781
		public static GameKey RightClick = new GameKey(2);

		// Token: 0x0400030E RID: 782
		public static GameKey DoubleClick = new GameKey(3);

		// Token: 0x0400030F RID: 783
		public static GameKey Left = new GameKey(4);

		// Token: 0x04000310 RID: 784
		public static GameKey Right = new GameKey(5);

		// Token: 0x04000311 RID: 785
		public static GameKey Up = new GameKey(6);

		// Token: 0x04000312 RID: 786
		public static GameKey Down = new GameKey(7);

		// Token: 0x04000313 RID: 787
		public static GameKey Select = new GameKey(8);

		// Token: 0x04000314 RID: 788
		public static GameKey Back = new GameKey(9);

		// Token: 0x04000315 RID: 789
		public static GameKey DpadLeft = new GameKey(10);

		// Token: 0x04000316 RID: 790
		public static GameKey DpadRight = new GameKey(11);

		// Token: 0x04000317 RID: 791
		public static GameKey DpadUp = new GameKey(12);

		// Token: 0x04000318 RID: 792
		public static GameKey DpadDown = new GameKey(13);

		// Token: 0x04000319 RID: 793
		public static GameKey Interaction = new GameKey(100);

		// Token: 0x0400031A RID: 794
		public static GameKey Action = new GameKey(101);

		// Token: 0x0400031B RID: 795
		public static GameKey CharacterWindow = new GameKey(102);

		// Token: 0x0400031C RID: 796
		public static GameKey Inventory = new GameKey(103);

		// Token: 0x0400031D RID: 797
		public static GameKey Inspirations = new GameKey(104);

		// Token: 0x0400031E RID: 798
		public static GameKey PrevTab = new GameKey(105);

		// Token: 0x0400031F RID: 799
		public static GameKey NextTab = new GameKey(106);

		// Token: 0x04000320 RID: 800
		public static GameKey PrevSubTab = new GameKey(107);

		// Token: 0x04000321 RID: 801
		public static GameKey NextSubTab = new GameKey(108);

		// Token: 0x04000322 RID: 802
		public static GameKey TechTree = new GameKey(109);

		// Token: 0x04000323 RID: 803
		public static GameKey Rotate = new GameKey(110);

		// Token: 0x04000324 RID: 804
		public static GameKey Fold = new GameKey(111);

		// Token: 0x04000325 RID: 805
		public static GameKey Build = new GameKey(112);

		// Token: 0x04000326 RID: 806
		public static GameKey SpeechSkip = new GameKey(113);

		// Token: 0x04000327 RID: 807
		public static GameKey Attack = new GameKey(114);

		// Token: 0x04000328 RID: 808
		public static GameKey Map = new GameKey(115);

		// Token: 0x04000329 RID: 809
		public static GameKey ChangeWeapon = new GameKey(116);

		// Token: 0x0400032A RID: 810
		public static GameKey QuestTree = new GameKey(117);

		// Token: 0x0400032B RID: 811
		public static GameKey LeftTrigger = new GameKey(140);

		// Token: 0x0400032C RID: 812
		public static GameKey RightTrigger = new GameKey(141);

		// Token: 0x0400032D RID: 813
		public static GameKey IncSlider = new GameKey(142);

		// Token: 0x0400032E RID: 814
		public static GameKey DecSlider = new GameKey(143);

		// Token: 0x0400032F RID: 815
		public static GameKey InGameMenu = new GameKey(144);

		// Token: 0x04000330 RID: 816
		public static GameKey ItemMove = new GameKey(145);

		// Token: 0x04000331 RID: 817
		public static GameKey AcceptVendorDeal = new GameKey(146);

		// Token: 0x04000332 RID: 818
		public static GameKey MoveAllItemsToPlayer = new GameKey(147);

		// Token: 0x04000333 RID: 819
		public static GameKey MoveAllItemsFromPlayer = new GameKey(148);

		// Token: 0x04000334 RID: 820
		public static GameKey ExtractBody = new GameKey(149);

		// Token: 0x04000335 RID: 821
		public static GameKey Plant = new GameKey(150);

		// Token: 0x04000336 RID: 822
		public static GameKey SelectPlantElement = new GameKey(151);

		// Token: 0x04000337 RID: 823
		public static GameKey ClearPlantElement = new GameKey(152);

		// Token: 0x04000338 RID: 824
		public static GameKey FoldAdditionalInfo = new GameKey(153);

		// Token: 0x04000339 RID: 825
		public static GameKey StartResurrection = new GameKey(161);

		// Token: 0x0400033A RID: 826
		public static GameKey SaveDelete = new GameKey(172);

		// Token: 0x0400033B RID: 827
		public static GameKey SaveImport = new GameKey(173);

		// Token: 0x0400033C RID: 828
		public static GameKey UseHotBarItem1 = new GameKey(181);

		// Token: 0x0400033D RID: 829
		public static GameKey UseHotBarItem2 = new GameKey(182);

		// Token: 0x0400033E RID: 830
		public static GameKey UseHotBarItem3 = new GameKey(183);

		// Token: 0x0400033F RID: 831
		public static GameKey UseHotBarItem4 = new GameKey(184);

		// Token: 0x04000340 RID: 832
		public static GameKey Dpad = new GameKey(185);

		// Token: 0x04000341 RID: 833
		public static GameKey MaxSlider = new GameKey(186);

		// Token: 0x04000342 RID: 834
		public static GameKey MinSlider = new GameKey(187);

		// Token: 0x04000343 RID: 835
		public static GameKey RightStick = new GameKey(191);

		// Token: 0x04000344 RID: 836
		public static GameKey LeftStick = new GameKey(192);

		// Token: 0x04000345 RID: 837
		public static GameKey StartCraft = new GameKey(201);

		// Token: 0x04000346 RID: 838
		public static GameKey AddCraftToQueue = new GameKey(202);

		// Token: 0x04000347 RID: 839
		public static GameKey AttackFocus = new GameKey(203);

		// Token: 0x04000348 RID: 840
		public static GameKey RightBumper = new GameKey(204);

		// Token: 0x04000349 RID: 841
		public static GameKey ZombieRollName = new GameKey(221);

		// Token: 0x0400034A RID: 842
		public static GameKey RightStickAsGameKey = new GameKey(225);

		// Token: 0x0400034B RID: 843
		public static GameKey CraftWindowZoneSwitch = new GameKey(230);

		// Token: 0x0400034C RID: 844
		public static GameKey CraftWindowQueueLeft = new GameKey(231);

		// Token: 0x0400034D RID: 845
		public static GameKey CraftWindowQueueRight = new GameKey(232);

		// Token: 0x0400034E RID: 846
		public static GameKey PrayStart = new GameKey(235);

		// Token: 0x0400034F RID: 847
		public static GameKey SpeechSkip2 = new GameKey(251);

		// Token: 0x04000350 RID: 848
		public static GameKey AlchemyStart = new GameKey(252);

		// Token: 0x04000351 RID: 849
		public static GameKey SurveyStart = new GameKey(253);

		// Token: 0x04000352 RID: 850
		public static GameKey SubmitBait = new GameKey(254);

		// Token: 0x04000353 RID: 851
		public static GameKey StartFight = new GameKey(255);

		// Token: 0x04000354 RID: 852
		public static GameKey OrderWindowTraders = new GameKey(256);

		// Token: 0x04000355 RID: 853
		public static GameKey EndPrefight = new GameKey(257);

		// Token: 0x04000356 RID: 854
		public static GameKey AlchemyBoost = new GameKey(258);

		// Token: 0x04000357 RID: 855
		public static GameKey ItemCountWindow_Decrease = new GameKey(280);

		// Token: 0x04000358 RID: 856
		public static GameKey ItemCountWindow_Increase = new GameKey(281);

		// Token: 0x04000359 RID: 857
		public static GameKey ItemCountWindow_Max = new GameKey(282);

		// Token: 0x0400035A RID: 858
		public static GameKey ItemCountWindow_Min = new GameKey(283);

		// Token: 0x0400035B RID: 859
		public static GameKey ItemCountWindow_Apply = new GameKey(284);

		// Token: 0x0400035C RID: 860
		public static GameKey ItemCountWindow_Cancel = new GameKey(285);

		// Token: 0x0400035D RID: 861
		public static GameKey CheatJump = new GameKey(9998);

		// Token: 0x0400035E RID: 862
		public static GameKey CheatButton = new GameKey(9999);
	}
}
