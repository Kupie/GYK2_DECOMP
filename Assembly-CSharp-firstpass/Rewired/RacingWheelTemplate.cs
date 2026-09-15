using System;

namespace Rewired
{
	// Token: 0x0200001A RID: 26
	public sealed class RacingWheelTemplate : ControllerTemplate, IRacingWheelTemplate, IControllerTemplate
	{
		// Token: 0x1700011E RID: 286
		// (get) Token: 0x0600016C RID: 364 RVA: 0x00002EB2 File Offset: 0x000010B2
		IControllerTemplateAxis IRacingWheelTemplate.wheel
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(0);
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600016D RID: 365 RVA: 0x00002EBB File Offset: 0x000010BB
		IControllerTemplateAxis IRacingWheelTemplate.accelerator
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(1);
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x0600016E RID: 366 RVA: 0x00002EC4 File Offset: 0x000010C4
		IControllerTemplateAxis IRacingWheelTemplate.brake
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(2);
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x0600016F RID: 367 RVA: 0x00002ECD File Offset: 0x000010CD
		IControllerTemplateAxis IRacingWheelTemplate.clutch
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(3);
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000170 RID: 368 RVA: 0x00002DFD File Offset: 0x00000FFD
		IControllerTemplateButton IRacingWheelTemplate.shiftDown
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(4);
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000171 RID: 369 RVA: 0x00002E06 File Offset: 0x00001006
		IControllerTemplateButton IRacingWheelTemplate.shiftUp
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(5);
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000172 RID: 370 RVA: 0x00002E0F File Offset: 0x0000100F
		IControllerTemplateButton IRacingWheelTemplate.wheelButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(6);
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000173 RID: 371 RVA: 0x00002E18 File Offset: 0x00001018
		IControllerTemplateButton IRacingWheelTemplate.wheelButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(7);
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000174 RID: 372 RVA: 0x00002E21 File Offset: 0x00001021
		IControllerTemplateButton IRacingWheelTemplate.wheelButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(8);
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000175 RID: 373 RVA: 0x00002E2A File Offset: 0x0000102A
		IControllerTemplateButton IRacingWheelTemplate.wheelButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(9);
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00002E34 File Offset: 0x00001034
		IControllerTemplateButton IRacingWheelTemplate.wheelButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(10);
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00002ED6 File Offset: 0x000010D6
		IControllerTemplateButton IRacingWheelTemplate.wheelButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(11);
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000178 RID: 376 RVA: 0x00002E48 File Offset: 0x00001048
		IControllerTemplateButton IRacingWheelTemplate.wheelButton7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(12);
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00002EE0 File Offset: 0x000010E0
		IControllerTemplateButton IRacingWheelTemplate.wheelButton8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(13);
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00002E5C File Offset: 0x0000105C
		IControllerTemplateButton IRacingWheelTemplate.wheelButton9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(14);
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00002E66 File Offset: 0x00001066
		IControllerTemplateButton IRacingWheelTemplate.wheelButton10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(15);
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00002E70 File Offset: 0x00001070
		IControllerTemplateButton IRacingWheelTemplate.consoleButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(16);
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00002EEA File Offset: 0x000010EA
		IControllerTemplateButton IRacingWheelTemplate.consoleButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(17);
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x0600017E RID: 382 RVA: 0x00002EF4 File Offset: 0x000010F4
		IControllerTemplateButton IRacingWheelTemplate.consoleButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(18);
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00002EFE File Offset: 0x000010FE
		IControllerTemplateButton IRacingWheelTemplate.consoleButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(19);
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000180 RID: 384 RVA: 0x00002F08 File Offset: 0x00001108
		IControllerTemplateButton IRacingWheelTemplate.consoleButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(20);
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000181 RID: 385 RVA: 0x00002F12 File Offset: 0x00001112
		IControllerTemplateButton IRacingWheelTemplate.consoleButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(21);
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000182 RID: 386 RVA: 0x00002F1C File Offset: 0x0000111C
		IControllerTemplateButton IRacingWheelTemplate.consoleButton7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(22);
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00002F26 File Offset: 0x00001126
		IControllerTemplateButton IRacingWheelTemplate.consoleButton8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(23);
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000184 RID: 388 RVA: 0x00002F30 File Offset: 0x00001130
		IControllerTemplateButton IRacingWheelTemplate.consoleButton9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(24);
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000185 RID: 389 RVA: 0x00002F3A File Offset: 0x0000113A
		IControllerTemplateButton IRacingWheelTemplate.consoleButton10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(25);
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000186 RID: 390 RVA: 0x00002F44 File Offset: 0x00001144
		IControllerTemplateButton IRacingWheelTemplate.shifter1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(26);
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00002F4E File Offset: 0x0000114E
		IControllerTemplateButton IRacingWheelTemplate.shifter2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(27);
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000188 RID: 392 RVA: 0x00002F58 File Offset: 0x00001158
		IControllerTemplateButton IRacingWheelTemplate.shifter3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(28);
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000189 RID: 393 RVA: 0x00002F62 File Offset: 0x00001162
		IControllerTemplateButton IRacingWheelTemplate.shifter4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(29);
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x0600018A RID: 394 RVA: 0x00002F6C File Offset: 0x0000116C
		IControllerTemplateButton IRacingWheelTemplate.shifter5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(30);
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x0600018B RID: 395 RVA: 0x00002F76 File Offset: 0x00001176
		IControllerTemplateButton IRacingWheelTemplate.shifter6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(31);
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x0600018C RID: 396 RVA: 0x00002F80 File Offset: 0x00001180
		IControllerTemplateButton IRacingWheelTemplate.shifter7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(32);
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x0600018D RID: 397 RVA: 0x00002F8A File Offset: 0x0000118A
		IControllerTemplateButton IRacingWheelTemplate.shifter8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(33);
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x0600018E RID: 398 RVA: 0x00002F94 File Offset: 0x00001194
		IControllerTemplateButton IRacingWheelTemplate.shifter9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(34);
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x0600018F RID: 399 RVA: 0x00002F9E File Offset: 0x0000119E
		IControllerTemplateButton IRacingWheelTemplate.shifter10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(35);
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000190 RID: 400 RVA: 0x00002FA8 File Offset: 0x000011A8
		IControllerTemplateButton IRacingWheelTemplate.reverseGear
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(44);
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000191 RID: 401 RVA: 0x00002FB2 File Offset: 0x000011B2
		IControllerTemplateButton IRacingWheelTemplate.select
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(36);
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000192 RID: 402 RVA: 0x00002FBC File Offset: 0x000011BC
		IControllerTemplateButton IRacingWheelTemplate.start
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(37);
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000193 RID: 403 RVA: 0x00002FC6 File Offset: 0x000011C6
		IControllerTemplateButton IRacingWheelTemplate.systemButton
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(38);
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000194 RID: 404 RVA: 0x00002FD0 File Offset: 0x000011D0
		IControllerTemplateButton IRacingWheelTemplate.horn
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(43);
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000195 RID: 405 RVA: 0x00002FDA File Offset: 0x000011DA
		IControllerTemplateDPad IRacingWheelTemplate.dPad
		{
			get
			{
				return base.GetElement<IControllerTemplateDPad>(45);
			}
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00002E98 File Offset: 0x00001098
		public RacingWheelTemplate(object payload)
			: base(payload)
		{
		}

		// Token: 0x0400006D RID: 109
		public static readonly Guid typeGuid = new Guid("104e31d8-9115-4dd5-a398-2e54d35e6c83");

		// Token: 0x0400006E RID: 110
		public const int elementId_wheel = 0;

		// Token: 0x0400006F RID: 111
		public const int elementId_accelerator = 1;

		// Token: 0x04000070 RID: 112
		public const int elementId_brake = 2;

		// Token: 0x04000071 RID: 113
		public const int elementId_clutch = 3;

		// Token: 0x04000072 RID: 114
		public const int elementId_shiftDown = 4;

		// Token: 0x04000073 RID: 115
		public const int elementId_shiftUp = 5;

		// Token: 0x04000074 RID: 116
		public const int elementId_wheelButton1 = 6;

		// Token: 0x04000075 RID: 117
		public const int elementId_wheelButton2 = 7;

		// Token: 0x04000076 RID: 118
		public const int elementId_wheelButton3 = 8;

		// Token: 0x04000077 RID: 119
		public const int elementId_wheelButton4 = 9;

		// Token: 0x04000078 RID: 120
		public const int elementId_wheelButton5 = 10;

		// Token: 0x04000079 RID: 121
		public const int elementId_wheelButton6 = 11;

		// Token: 0x0400007A RID: 122
		public const int elementId_wheelButton7 = 12;

		// Token: 0x0400007B RID: 123
		public const int elementId_wheelButton8 = 13;

		// Token: 0x0400007C RID: 124
		public const int elementId_wheelButton9 = 14;

		// Token: 0x0400007D RID: 125
		public const int elementId_wheelButton10 = 15;

		// Token: 0x0400007E RID: 126
		public const int elementId_consoleButton1 = 16;

		// Token: 0x0400007F RID: 127
		public const int elementId_consoleButton2 = 17;

		// Token: 0x04000080 RID: 128
		public const int elementId_consoleButton3 = 18;

		// Token: 0x04000081 RID: 129
		public const int elementId_consoleButton4 = 19;

		// Token: 0x04000082 RID: 130
		public const int elementId_consoleButton5 = 20;

		// Token: 0x04000083 RID: 131
		public const int elementId_consoleButton6 = 21;

		// Token: 0x04000084 RID: 132
		public const int elementId_consoleButton7 = 22;

		// Token: 0x04000085 RID: 133
		public const int elementId_consoleButton8 = 23;

		// Token: 0x04000086 RID: 134
		public const int elementId_consoleButton9 = 24;

		// Token: 0x04000087 RID: 135
		public const int elementId_consoleButton10 = 25;

		// Token: 0x04000088 RID: 136
		public const int elementId_shifter1 = 26;

		// Token: 0x04000089 RID: 137
		public const int elementId_shifter2 = 27;

		// Token: 0x0400008A RID: 138
		public const int elementId_shifter3 = 28;

		// Token: 0x0400008B RID: 139
		public const int elementId_shifter4 = 29;

		// Token: 0x0400008C RID: 140
		public const int elementId_shifter5 = 30;

		// Token: 0x0400008D RID: 141
		public const int elementId_shifter6 = 31;

		// Token: 0x0400008E RID: 142
		public const int elementId_shifter7 = 32;

		// Token: 0x0400008F RID: 143
		public const int elementId_shifter8 = 33;

		// Token: 0x04000090 RID: 144
		public const int elementId_shifter9 = 34;

		// Token: 0x04000091 RID: 145
		public const int elementId_shifter10 = 35;

		// Token: 0x04000092 RID: 146
		public const int elementId_reverseGear = 44;

		// Token: 0x04000093 RID: 147
		public const int elementId_select = 36;

		// Token: 0x04000094 RID: 148
		public const int elementId_start = 37;

		// Token: 0x04000095 RID: 149
		public const int elementId_systemButton = 38;

		// Token: 0x04000096 RID: 150
		public const int elementId_horn = 43;

		// Token: 0x04000097 RID: 151
		public const int elementId_dPadUp = 39;

		// Token: 0x04000098 RID: 152
		public const int elementId_dPadRight = 40;

		// Token: 0x04000099 RID: 153
		public const int elementId_dPadDown = 41;

		// Token: 0x0400009A RID: 154
		public const int elementId_dPadLeft = 42;

		// Token: 0x0400009B RID: 155
		public const int elementId_dPad = 45;
	}
}
