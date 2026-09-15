using System;

namespace Rewired
{
	// Token: 0x02000019 RID: 25
	public sealed class GamepadTemplate : ControllerTemplate, IGamepadTemplate, IControllerTemplate
	{
		// Token: 0x17000101 RID: 257
		// (get) Token: 0x0600014D RID: 333 RVA: 0x00002DFD File Offset: 0x00000FFD
		IControllerTemplateButton IGamepadTemplate.actionBottomRow1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(4);
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00002DFD File Offset: 0x00000FFD
		IControllerTemplateButton IGamepadTemplate.a
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(4);
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00002E06 File Offset: 0x00001006
		IControllerTemplateButton IGamepadTemplate.actionBottomRow2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(5);
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00002E06 File Offset: 0x00001006
		IControllerTemplateButton IGamepadTemplate.b
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(5);
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00002E0F File Offset: 0x0000100F
		IControllerTemplateButton IGamepadTemplate.actionBottomRow3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(6);
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000152 RID: 338 RVA: 0x00002E0F File Offset: 0x0000100F
		IControllerTemplateButton IGamepadTemplate.c
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(6);
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000153 RID: 339 RVA: 0x00002E18 File Offset: 0x00001018
		IControllerTemplateButton IGamepadTemplate.actionTopRow1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(7);
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00002E18 File Offset: 0x00001018
		IControllerTemplateButton IGamepadTemplate.x
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(7);
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000155 RID: 341 RVA: 0x00002E21 File Offset: 0x00001021
		IControllerTemplateButton IGamepadTemplate.actionTopRow2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(8);
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000156 RID: 342 RVA: 0x00002E21 File Offset: 0x00001021
		IControllerTemplateButton IGamepadTemplate.y
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(8);
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000157 RID: 343 RVA: 0x00002E2A File Offset: 0x0000102A
		IControllerTemplateButton IGamepadTemplate.actionTopRow3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(9);
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00002E2A File Offset: 0x0000102A
		IControllerTemplateButton IGamepadTemplate.z
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(9);
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000159 RID: 345 RVA: 0x00002E34 File Offset: 0x00001034
		IControllerTemplateButton IGamepadTemplate.leftShoulder1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(10);
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600015A RID: 346 RVA: 0x00002E34 File Offset: 0x00001034
		IControllerTemplateButton IGamepadTemplate.leftBumper
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(10);
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x0600015B RID: 347 RVA: 0x00002E3E File Offset: 0x0000103E
		IControllerTemplateAxis IGamepadTemplate.leftShoulder2
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(11);
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x0600015C RID: 348 RVA: 0x00002E3E File Offset: 0x0000103E
		IControllerTemplateAxis IGamepadTemplate.leftTrigger
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(11);
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00002E48 File Offset: 0x00001048
		IControllerTemplateButton IGamepadTemplate.rightShoulder1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(12);
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x0600015E RID: 350 RVA: 0x00002E48 File Offset: 0x00001048
		IControllerTemplateButton IGamepadTemplate.rightBumper
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(12);
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00002E52 File Offset: 0x00001052
		IControllerTemplateAxis IGamepadTemplate.rightShoulder2
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(13);
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000160 RID: 352 RVA: 0x00002E52 File Offset: 0x00001052
		IControllerTemplateAxis IGamepadTemplate.rightTrigger
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(13);
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00002E5C File Offset: 0x0000105C
		IControllerTemplateButton IGamepadTemplate.center1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(14);
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00002E5C File Offset: 0x0000105C
		IControllerTemplateButton IGamepadTemplate.back
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(14);
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00002E66 File Offset: 0x00001066
		IControllerTemplateButton IGamepadTemplate.center2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(15);
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000164 RID: 356 RVA: 0x00002E66 File Offset: 0x00001066
		IControllerTemplateButton IGamepadTemplate.start
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(15);
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000165 RID: 357 RVA: 0x00002E70 File Offset: 0x00001070
		IControllerTemplateButton IGamepadTemplate.center3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(16);
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000166 RID: 358 RVA: 0x00002E70 File Offset: 0x00001070
		IControllerTemplateButton IGamepadTemplate.guide
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(16);
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000167 RID: 359 RVA: 0x00002E7A File Offset: 0x0000107A
		IControllerTemplateThumbStick IGamepadTemplate.leftStick
		{
			get
			{
				return base.GetElement<IControllerTemplateThumbStick>(23);
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00002E84 File Offset: 0x00001084
		IControllerTemplateThumbStick IGamepadTemplate.rightStick
		{
			get
			{
				return base.GetElement<IControllerTemplateThumbStick>(24);
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00002E8E File Offset: 0x0000108E
		IControllerTemplateDPad IGamepadTemplate.dPad
		{
			get
			{
				return base.GetElement<IControllerTemplateDPad>(25);
			}
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00002E98 File Offset: 0x00001098
		public GamepadTemplate(object payload)
			: base(payload)
		{
		}

		// Token: 0x04000045 RID: 69
		public static readonly Guid typeGuid = new Guid("83b427e4-086f-47f3-bb06-be266abd1ca5");

		// Token: 0x04000046 RID: 70
		public const int elementId_leftStickX = 0;

		// Token: 0x04000047 RID: 71
		public const int elementId_leftStickY = 1;

		// Token: 0x04000048 RID: 72
		public const int elementId_rightStickX = 2;

		// Token: 0x04000049 RID: 73
		public const int elementId_rightStickY = 3;

		// Token: 0x0400004A RID: 74
		public const int elementId_actionBottomRow1 = 4;

		// Token: 0x0400004B RID: 75
		public const int elementId_a = 4;

		// Token: 0x0400004C RID: 76
		public const int elementId_actionBottomRow2 = 5;

		// Token: 0x0400004D RID: 77
		public const int elementId_b = 5;

		// Token: 0x0400004E RID: 78
		public const int elementId_actionBottomRow3 = 6;

		// Token: 0x0400004F RID: 79
		public const int elementId_c = 6;

		// Token: 0x04000050 RID: 80
		public const int elementId_actionTopRow1 = 7;

		// Token: 0x04000051 RID: 81
		public const int elementId_x = 7;

		// Token: 0x04000052 RID: 82
		public const int elementId_actionTopRow2 = 8;

		// Token: 0x04000053 RID: 83
		public const int elementId_y = 8;

		// Token: 0x04000054 RID: 84
		public const int elementId_actionTopRow3 = 9;

		// Token: 0x04000055 RID: 85
		public const int elementId_z = 9;

		// Token: 0x04000056 RID: 86
		public const int elementId_leftShoulder1 = 10;

		// Token: 0x04000057 RID: 87
		public const int elementId_leftBumper = 10;

		// Token: 0x04000058 RID: 88
		public const int elementId_leftShoulder2 = 11;

		// Token: 0x04000059 RID: 89
		public const int elementId_leftTrigger = 11;

		// Token: 0x0400005A RID: 90
		public const int elementId_rightShoulder1 = 12;

		// Token: 0x0400005B RID: 91
		public const int elementId_rightBumper = 12;

		// Token: 0x0400005C RID: 92
		public const int elementId_rightShoulder2 = 13;

		// Token: 0x0400005D RID: 93
		public const int elementId_rightTrigger = 13;

		// Token: 0x0400005E RID: 94
		public const int elementId_center1 = 14;

		// Token: 0x0400005F RID: 95
		public const int elementId_back = 14;

		// Token: 0x04000060 RID: 96
		public const int elementId_center2 = 15;

		// Token: 0x04000061 RID: 97
		public const int elementId_start = 15;

		// Token: 0x04000062 RID: 98
		public const int elementId_center3 = 16;

		// Token: 0x04000063 RID: 99
		public const int elementId_guide = 16;

		// Token: 0x04000064 RID: 100
		public const int elementId_leftStickButton = 17;

		// Token: 0x04000065 RID: 101
		public const int elementId_rightStickButton = 18;

		// Token: 0x04000066 RID: 102
		public const int elementId_dPadUp = 19;

		// Token: 0x04000067 RID: 103
		public const int elementId_dPadRight = 20;

		// Token: 0x04000068 RID: 104
		public const int elementId_dPadDown = 21;

		// Token: 0x04000069 RID: 105
		public const int elementId_dPadLeft = 22;

		// Token: 0x0400006A RID: 106
		public const int elementId_leftStick = 23;

		// Token: 0x0400006B RID: 107
		public const int elementId_rightStick = 24;

		// Token: 0x0400006C RID: 108
		public const int elementId_dPad = 25;
	}
}
