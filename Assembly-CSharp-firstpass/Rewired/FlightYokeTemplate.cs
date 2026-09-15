using System;

namespace Rewired
{
	// Token: 0x0200001C RID: 28
	public sealed class FlightYokeTemplate : ControllerTemplate, IFlightYokeTemplate, IControllerTemplate
	{
		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x00003093 File Offset: 0x00001293
		IControllerTemplateButton IFlightYokeTemplate.leftPaddle
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(59);
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x0000309D File Offset: 0x0000129D
		IControllerTemplateButton IFlightYokeTemplate.rightPaddle
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(60);
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x00002E18 File Offset: 0x00001018
		IControllerTemplateButton IFlightYokeTemplate.leftGripButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(7);
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x00002E21 File Offset: 0x00001021
		IControllerTemplateButton IFlightYokeTemplate.leftGripButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(8);
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x00002E2A File Offset: 0x0000102A
		IControllerTemplateButton IFlightYokeTemplate.leftGripButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(9);
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x00002E34 File Offset: 0x00001034
		IControllerTemplateButton IFlightYokeTemplate.leftGripButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(10);
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x00002ED6 File Offset: 0x000010D6
		IControllerTemplateButton IFlightYokeTemplate.leftGripButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(11);
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x00002E48 File Offset: 0x00001048
		IControllerTemplateButton IFlightYokeTemplate.leftGripButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(12);
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060001FA RID: 506 RVA: 0x00002EE0 File Offset: 0x000010E0
		IControllerTemplateButton IFlightYokeTemplate.rightGripButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(13);
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060001FB RID: 507 RVA: 0x00002E5C File Offset: 0x0000105C
		IControllerTemplateButton IFlightYokeTemplate.rightGripButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(14);
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060001FC RID: 508 RVA: 0x00002E66 File Offset: 0x00001066
		IControllerTemplateButton IFlightYokeTemplate.rightGripButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(15);
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060001FD RID: 509 RVA: 0x00002E70 File Offset: 0x00001070
		IControllerTemplateButton IFlightYokeTemplate.rightGripButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(16);
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060001FE RID: 510 RVA: 0x00002EEA File Offset: 0x000010EA
		IControllerTemplateButton IFlightYokeTemplate.rightGripButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(17);
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060001FF RID: 511 RVA: 0x00002EF4 File Offset: 0x000010F4
		IControllerTemplateButton IFlightYokeTemplate.rightGripButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(18);
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000200 RID: 512 RVA: 0x00002EFE File Offset: 0x000010FE
		IControllerTemplateButton IFlightYokeTemplate.centerButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(19);
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000201 RID: 513 RVA: 0x00002F08 File Offset: 0x00001108
		IControllerTemplateButton IFlightYokeTemplate.centerButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(20);
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000202 RID: 514 RVA: 0x00002F12 File Offset: 0x00001112
		IControllerTemplateButton IFlightYokeTemplate.centerButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(21);
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000203 RID: 515 RVA: 0x00002F1C File Offset: 0x0000111C
		IControllerTemplateButton IFlightYokeTemplate.centerButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(22);
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000204 RID: 516 RVA: 0x00002F26 File Offset: 0x00001126
		IControllerTemplateButton IFlightYokeTemplate.centerButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(23);
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000205 RID: 517 RVA: 0x00002F30 File Offset: 0x00001130
		IControllerTemplateButton IFlightYokeTemplate.centerButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(24);
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000206 RID: 518 RVA: 0x00002F3A File Offset: 0x0000113A
		IControllerTemplateButton IFlightYokeTemplate.centerButton7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(25);
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000207 RID: 519 RVA: 0x00002F44 File Offset: 0x00001144
		IControllerTemplateButton IFlightYokeTemplate.centerButton8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(26);
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000208 RID: 520 RVA: 0x00003057 File Offset: 0x00001257
		IControllerTemplateButton IFlightYokeTemplate.wheel1Up
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(53);
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000209 RID: 521 RVA: 0x00003061 File Offset: 0x00001261
		IControllerTemplateButton IFlightYokeTemplate.wheel1Down
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(54);
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x0600020A RID: 522 RVA: 0x0000306B File Offset: 0x0000126B
		IControllerTemplateButton IFlightYokeTemplate.wheel1Press
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(55);
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x0600020B RID: 523 RVA: 0x00003075 File Offset: 0x00001275
		IControllerTemplateButton IFlightYokeTemplate.wheel2Up
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(56);
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x0600020C RID: 524 RVA: 0x0000307F File Offset: 0x0000127F
		IControllerTemplateButton IFlightYokeTemplate.wheel2Down
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(57);
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x0600020D RID: 525 RVA: 0x00003089 File Offset: 0x00001289
		IControllerTemplateButton IFlightYokeTemplate.wheel2Press
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(58);
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x0600020E RID: 526 RVA: 0x00002FD0 File Offset: 0x000011D0
		IControllerTemplateButton IFlightYokeTemplate.consoleButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(43);
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x0600020F RID: 527 RVA: 0x00002FA8 File Offset: 0x000011A8
		IControllerTemplateButton IFlightYokeTemplate.consoleButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(44);
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000210 RID: 528 RVA: 0x00003025 File Offset: 0x00001225
		IControllerTemplateButton IFlightYokeTemplate.consoleButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(45);
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000211 RID: 529 RVA: 0x0000302F File Offset: 0x0000122F
		IControllerTemplateButton IFlightYokeTemplate.consoleButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(46);
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000212 RID: 530 RVA: 0x000032FE File Offset: 0x000014FE
		IControllerTemplateButton IFlightYokeTemplate.consoleButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(47);
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000213 RID: 531 RVA: 0x00003308 File Offset: 0x00001508
		IControllerTemplateButton IFlightYokeTemplate.consoleButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(48);
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000214 RID: 532 RVA: 0x00003312 File Offset: 0x00001512
		IControllerTemplateButton IFlightYokeTemplate.consoleButton7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(49);
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000215 RID: 533 RVA: 0x00003039 File Offset: 0x00001239
		IControllerTemplateButton IFlightYokeTemplate.consoleButton8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(50);
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000216 RID: 534 RVA: 0x00003043 File Offset: 0x00001243
		IControllerTemplateButton IFlightYokeTemplate.consoleButton9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(51);
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000217 RID: 535 RVA: 0x0000304D File Offset: 0x0000124D
		IControllerTemplateButton IFlightYokeTemplate.consoleButton10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(52);
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000218 RID: 536 RVA: 0x000030A7 File Offset: 0x000012A7
		IControllerTemplateButton IFlightYokeTemplate.mode1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(61);
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000219 RID: 537 RVA: 0x000030B1 File Offset: 0x000012B1
		IControllerTemplateButton IFlightYokeTemplate.mode2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(62);
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x0600021A RID: 538 RVA: 0x000030BB File Offset: 0x000012BB
		IControllerTemplateButton IFlightYokeTemplate.mode3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(63);
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x0600021B RID: 539 RVA: 0x0000331C File Offset: 0x0000151C
		IControllerTemplateYoke IFlightYokeTemplate.yoke
		{
			get
			{
				return base.GetElement<IControllerTemplateYoke>(69);
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x0600021C RID: 540 RVA: 0x00003326 File Offset: 0x00001526
		IControllerTemplateThrottle IFlightYokeTemplate.lever1
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(70);
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x0600021D RID: 541 RVA: 0x00003330 File Offset: 0x00001530
		IControllerTemplateThrottle IFlightYokeTemplate.lever2
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(71);
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x0600021E RID: 542 RVA: 0x0000333A File Offset: 0x0000153A
		IControllerTemplateThrottle IFlightYokeTemplate.lever3
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(72);
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x0600021F RID: 543 RVA: 0x00003344 File Offset: 0x00001544
		IControllerTemplateThrottle IFlightYokeTemplate.lever4
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(73);
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000220 RID: 544 RVA: 0x0000334E File Offset: 0x0000154E
		IControllerTemplateThrottle IFlightYokeTemplate.lever5
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(74);
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000221 RID: 545 RVA: 0x00003358 File Offset: 0x00001558
		IControllerTemplateHat IFlightYokeTemplate.leftGripHat
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(75);
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000222 RID: 546 RVA: 0x00003362 File Offset: 0x00001562
		IControllerTemplateHat IFlightYokeTemplate.rightGripHat
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(76);
			}
		}

		// Token: 0x06000223 RID: 547 RVA: 0x00002E98 File Offset: 0x00001098
		public FlightYokeTemplate(object payload)
			: base(payload)
		{
		}

		// Token: 0x04000145 RID: 325
		public static readonly Guid typeGuid = new Guid("f311fa16-0ccc-41c0-ac4b-50f7100bb8ff");

		// Token: 0x04000146 RID: 326
		public const int elementId_rotateYoke = 0;

		// Token: 0x04000147 RID: 327
		public const int elementId_yokeZ = 1;

		// Token: 0x04000148 RID: 328
		public const int elementId_leftPaddle = 59;

		// Token: 0x04000149 RID: 329
		public const int elementId_rightPaddle = 60;

		// Token: 0x0400014A RID: 330
		public const int elementId_lever1Axis = 2;

		// Token: 0x0400014B RID: 331
		public const int elementId_lever1MinDetent = 64;

		// Token: 0x0400014C RID: 332
		public const int elementId_lever2Axis = 3;

		// Token: 0x0400014D RID: 333
		public const int elementId_lever2MinDetent = 65;

		// Token: 0x0400014E RID: 334
		public const int elementId_lever3Axis = 4;

		// Token: 0x0400014F RID: 335
		public const int elementId_lever3MinDetent = 66;

		// Token: 0x04000150 RID: 336
		public const int elementId_lever4Axis = 5;

		// Token: 0x04000151 RID: 337
		public const int elementId_lever4MinDetent = 67;

		// Token: 0x04000152 RID: 338
		public const int elementId_lever5Axis = 6;

		// Token: 0x04000153 RID: 339
		public const int elementId_lever5MinDetent = 68;

		// Token: 0x04000154 RID: 340
		public const int elementId_leftGripButton1 = 7;

		// Token: 0x04000155 RID: 341
		public const int elementId_leftGripButton2 = 8;

		// Token: 0x04000156 RID: 342
		public const int elementId_leftGripButton3 = 9;

		// Token: 0x04000157 RID: 343
		public const int elementId_leftGripButton4 = 10;

		// Token: 0x04000158 RID: 344
		public const int elementId_leftGripButton5 = 11;

		// Token: 0x04000159 RID: 345
		public const int elementId_leftGripButton6 = 12;

		// Token: 0x0400015A RID: 346
		public const int elementId_rightGripButton1 = 13;

		// Token: 0x0400015B RID: 347
		public const int elementId_rightGripButton2 = 14;

		// Token: 0x0400015C RID: 348
		public const int elementId_rightGripButton3 = 15;

		// Token: 0x0400015D RID: 349
		public const int elementId_rightGripButton4 = 16;

		// Token: 0x0400015E RID: 350
		public const int elementId_rightGripButton5 = 17;

		// Token: 0x0400015F RID: 351
		public const int elementId_rightGripButton6 = 18;

		// Token: 0x04000160 RID: 352
		public const int elementId_centerButton1 = 19;

		// Token: 0x04000161 RID: 353
		public const int elementId_centerButton2 = 20;

		// Token: 0x04000162 RID: 354
		public const int elementId_centerButton3 = 21;

		// Token: 0x04000163 RID: 355
		public const int elementId_centerButton4 = 22;

		// Token: 0x04000164 RID: 356
		public const int elementId_centerButton5 = 23;

		// Token: 0x04000165 RID: 357
		public const int elementId_centerButton6 = 24;

		// Token: 0x04000166 RID: 358
		public const int elementId_centerButton7 = 25;

		// Token: 0x04000167 RID: 359
		public const int elementId_centerButton8 = 26;

		// Token: 0x04000168 RID: 360
		public const int elementId_wheel1Up = 53;

		// Token: 0x04000169 RID: 361
		public const int elementId_wheel1Down = 54;

		// Token: 0x0400016A RID: 362
		public const int elementId_wheel1Press = 55;

		// Token: 0x0400016B RID: 363
		public const int elementId_wheel2Up = 56;

		// Token: 0x0400016C RID: 364
		public const int elementId_wheel2Down = 57;

		// Token: 0x0400016D RID: 365
		public const int elementId_wheel2Press = 58;

		// Token: 0x0400016E RID: 366
		public const int elementId_leftGripHatUp = 27;

		// Token: 0x0400016F RID: 367
		public const int elementId_leftGripHatUpRight = 28;

		// Token: 0x04000170 RID: 368
		public const int elementId_leftGripHatRight = 29;

		// Token: 0x04000171 RID: 369
		public const int elementId_leftGripHatDownRight = 30;

		// Token: 0x04000172 RID: 370
		public const int elementId_leftGripHatDown = 31;

		// Token: 0x04000173 RID: 371
		public const int elementId_leftGripHatDownLeft = 32;

		// Token: 0x04000174 RID: 372
		public const int elementId_leftGripHatLeft = 33;

		// Token: 0x04000175 RID: 373
		public const int elementId_leftGripHatUpLeft = 34;

		// Token: 0x04000176 RID: 374
		public const int elementId_rightGripHatUp = 35;

		// Token: 0x04000177 RID: 375
		public const int elementId_rightGripHatUpRight = 36;

		// Token: 0x04000178 RID: 376
		public const int elementId_rightGripHatRight = 37;

		// Token: 0x04000179 RID: 377
		public const int elementId_rightGripHatDownRight = 38;

		// Token: 0x0400017A RID: 378
		public const int elementId_rightGripHatDown = 39;

		// Token: 0x0400017B RID: 379
		public const int elementId_rightGripHatDownLeft = 40;

		// Token: 0x0400017C RID: 380
		public const int elementId_rightGripHatLeft = 41;

		// Token: 0x0400017D RID: 381
		public const int elementId_rightGripHatUpLeft = 42;

		// Token: 0x0400017E RID: 382
		public const int elementId_consoleButton1 = 43;

		// Token: 0x0400017F RID: 383
		public const int elementId_consoleButton2 = 44;

		// Token: 0x04000180 RID: 384
		public const int elementId_consoleButton3 = 45;

		// Token: 0x04000181 RID: 385
		public const int elementId_consoleButton4 = 46;

		// Token: 0x04000182 RID: 386
		public const int elementId_consoleButton5 = 47;

		// Token: 0x04000183 RID: 387
		public const int elementId_consoleButton6 = 48;

		// Token: 0x04000184 RID: 388
		public const int elementId_consoleButton7 = 49;

		// Token: 0x04000185 RID: 389
		public const int elementId_consoleButton8 = 50;

		// Token: 0x04000186 RID: 390
		public const int elementId_consoleButton9 = 51;

		// Token: 0x04000187 RID: 391
		public const int elementId_consoleButton10 = 52;

		// Token: 0x04000188 RID: 392
		public const int elementId_mode1 = 61;

		// Token: 0x04000189 RID: 393
		public const int elementId_mode2 = 62;

		// Token: 0x0400018A RID: 394
		public const int elementId_mode3 = 63;

		// Token: 0x0400018B RID: 395
		public const int elementId_yoke = 69;

		// Token: 0x0400018C RID: 396
		public const int elementId_lever1 = 70;

		// Token: 0x0400018D RID: 397
		public const int elementId_lever2 = 71;

		// Token: 0x0400018E RID: 398
		public const int elementId_lever3 = 72;

		// Token: 0x0400018F RID: 399
		public const int elementId_lever4 = 73;

		// Token: 0x04000190 RID: 400
		public const int elementId_lever5 = 74;

		// Token: 0x04000191 RID: 401
		public const int elementId_leftGripHat = 75;

		// Token: 0x04000192 RID: 402
		public const int elementId_rightGripHat = 76;
	}
}
