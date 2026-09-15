using System;

namespace Rewired
{
	// Token: 0x0200001E RID: 30
	public sealed class SixDofControllerTemplate : ControllerTemplate, ISixDofControllerTemplate, IControllerTemplate
	{
		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x0600022A RID: 554 RVA: 0x0000338E File Offset: 0x0000158E
		IControllerTemplateAxis ISixDofControllerTemplate.extraAxis1
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(8);
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x0600022B RID: 555 RVA: 0x00003397 File Offset: 0x00001597
		IControllerTemplateAxis ISixDofControllerTemplate.extraAxis2
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(9);
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x0600022C RID: 556 RVA: 0x000033A1 File Offset: 0x000015A1
		IControllerTemplateAxis ISixDofControllerTemplate.extraAxis3
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(10);
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x0600022D RID: 557 RVA: 0x00002E3E File Offset: 0x0000103E
		IControllerTemplateAxis ISixDofControllerTemplate.extraAxis4
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(11);
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x0600022E RID: 558 RVA: 0x00002E48 File Offset: 0x00001048
		IControllerTemplateButton ISixDofControllerTemplate.button1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(12);
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x0600022F RID: 559 RVA: 0x00002EE0 File Offset: 0x000010E0
		IControllerTemplateButton ISixDofControllerTemplate.button2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(13);
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000230 RID: 560 RVA: 0x00002E5C File Offset: 0x0000105C
		IControllerTemplateButton ISixDofControllerTemplate.button3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(14);
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000231 RID: 561 RVA: 0x00002E66 File Offset: 0x00001066
		IControllerTemplateButton ISixDofControllerTemplate.button4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(15);
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000232 RID: 562 RVA: 0x00002E70 File Offset: 0x00001070
		IControllerTemplateButton ISixDofControllerTemplate.button5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(16);
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000233 RID: 563 RVA: 0x00002EEA File Offset: 0x000010EA
		IControllerTemplateButton ISixDofControllerTemplate.button6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(17);
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000234 RID: 564 RVA: 0x00002EF4 File Offset: 0x000010F4
		IControllerTemplateButton ISixDofControllerTemplate.button7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(18);
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000235 RID: 565 RVA: 0x00002EFE File Offset: 0x000010FE
		IControllerTemplateButton ISixDofControllerTemplate.button8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(19);
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000236 RID: 566 RVA: 0x00002F08 File Offset: 0x00001108
		IControllerTemplateButton ISixDofControllerTemplate.button9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(20);
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000237 RID: 567 RVA: 0x00002F12 File Offset: 0x00001112
		IControllerTemplateButton ISixDofControllerTemplate.button10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(21);
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000238 RID: 568 RVA: 0x00002F1C File Offset: 0x0000111C
		IControllerTemplateButton ISixDofControllerTemplate.button11
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(22);
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000239 RID: 569 RVA: 0x00002F26 File Offset: 0x00001126
		IControllerTemplateButton ISixDofControllerTemplate.button12
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(23);
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x0600023A RID: 570 RVA: 0x00002F30 File Offset: 0x00001130
		IControllerTemplateButton ISixDofControllerTemplate.button13
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(24);
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x0600023B RID: 571 RVA: 0x00002F3A File Offset: 0x0000113A
		IControllerTemplateButton ISixDofControllerTemplate.button14
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(25);
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x0600023C RID: 572 RVA: 0x00002F44 File Offset: 0x00001144
		IControllerTemplateButton ISixDofControllerTemplate.button15
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(26);
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x0600023D RID: 573 RVA: 0x00002F4E File Offset: 0x0000114E
		IControllerTemplateButton ISixDofControllerTemplate.button16
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(27);
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x0600023E RID: 574 RVA: 0x00002F58 File Offset: 0x00001158
		IControllerTemplateButton ISixDofControllerTemplate.button17
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(28);
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x0600023F RID: 575 RVA: 0x00002F62 File Offset: 0x00001162
		IControllerTemplateButton ISixDofControllerTemplate.button18
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(29);
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000240 RID: 576 RVA: 0x00002F6C File Offset: 0x0000116C
		IControllerTemplateButton ISixDofControllerTemplate.button19
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(30);
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000241 RID: 577 RVA: 0x00002F76 File Offset: 0x00001176
		IControllerTemplateButton ISixDofControllerTemplate.button20
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(31);
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000242 RID: 578 RVA: 0x0000306B File Offset: 0x0000126B
		IControllerTemplateButton ISixDofControllerTemplate.button21
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(55);
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000243 RID: 579 RVA: 0x00003075 File Offset: 0x00001275
		IControllerTemplateButton ISixDofControllerTemplate.button22
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(56);
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000244 RID: 580 RVA: 0x0000307F File Offset: 0x0000127F
		IControllerTemplateButton ISixDofControllerTemplate.button23
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(57);
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000245 RID: 581 RVA: 0x00003089 File Offset: 0x00001289
		IControllerTemplateButton ISixDofControllerTemplate.button24
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(58);
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000246 RID: 582 RVA: 0x00003093 File Offset: 0x00001293
		IControllerTemplateButton ISixDofControllerTemplate.button25
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(59);
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000247 RID: 583 RVA: 0x0000309D File Offset: 0x0000129D
		IControllerTemplateButton ISixDofControllerTemplate.button26
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(60);
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000248 RID: 584 RVA: 0x000030A7 File Offset: 0x000012A7
		IControllerTemplateButton ISixDofControllerTemplate.button27
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(61);
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000249 RID: 585 RVA: 0x000030B1 File Offset: 0x000012B1
		IControllerTemplateButton ISixDofControllerTemplate.button28
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(62);
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x0600024A RID: 586 RVA: 0x000030BB File Offset: 0x000012BB
		IControllerTemplateButton ISixDofControllerTemplate.button29
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(63);
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x0600024B RID: 587 RVA: 0x000030C5 File Offset: 0x000012C5
		IControllerTemplateButton ISixDofControllerTemplate.button30
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(64);
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x0600024C RID: 588 RVA: 0x000030CF File Offset: 0x000012CF
		IControllerTemplateButton ISixDofControllerTemplate.button31
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(65);
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x0600024D RID: 589 RVA: 0x000030D9 File Offset: 0x000012D9
		IControllerTemplateButton ISixDofControllerTemplate.button32
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(66);
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x0600024E RID: 590 RVA: 0x000033AB File Offset: 0x000015AB
		IControllerTemplateHat ISixDofControllerTemplate.hat1
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(48);
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x0600024F RID: 591 RVA: 0x000033B5 File Offset: 0x000015B5
		IControllerTemplateHat ISixDofControllerTemplate.hat2
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(49);
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000250 RID: 592 RVA: 0x000033BF File Offset: 0x000015BF
		IControllerTemplateThrottle ISixDofControllerTemplate.throttle1
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(52);
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000251 RID: 593 RVA: 0x000033C9 File Offset: 0x000015C9
		IControllerTemplateThrottle ISixDofControllerTemplate.throttle2
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(53);
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000252 RID: 594 RVA: 0x000033D3 File Offset: 0x000015D3
		IControllerTemplateStick6D ISixDofControllerTemplate.stick
		{
			get
			{
				return base.GetElement<IControllerTemplateStick6D>(54);
			}
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00002E98 File Offset: 0x00001098
		public SixDofControllerTemplate(object payload)
			: base(payload)
		{
		}

		// Token: 0x04000197 RID: 407
		public static readonly Guid typeGuid = new Guid("2599beb3-522b-43dd-a4ef-93fd60e5eafa");

		// Token: 0x04000198 RID: 408
		public const int elementId_positionX = 1;

		// Token: 0x04000199 RID: 409
		public const int elementId_positionY = 2;

		// Token: 0x0400019A RID: 410
		public const int elementId_positionZ = 0;

		// Token: 0x0400019B RID: 411
		public const int elementId_rotationX = 3;

		// Token: 0x0400019C RID: 412
		public const int elementId_rotationY = 5;

		// Token: 0x0400019D RID: 413
		public const int elementId_rotationZ = 4;

		// Token: 0x0400019E RID: 414
		public const int elementId_throttle1Axis = 6;

		// Token: 0x0400019F RID: 415
		public const int elementId_throttle1MinDetent = 50;

		// Token: 0x040001A0 RID: 416
		public const int elementId_throttle2Axis = 7;

		// Token: 0x040001A1 RID: 417
		public const int elementId_throttle2MinDetent = 51;

		// Token: 0x040001A2 RID: 418
		public const int elementId_extraAxis1 = 8;

		// Token: 0x040001A3 RID: 419
		public const int elementId_extraAxis2 = 9;

		// Token: 0x040001A4 RID: 420
		public const int elementId_extraAxis3 = 10;

		// Token: 0x040001A5 RID: 421
		public const int elementId_extraAxis4 = 11;

		// Token: 0x040001A6 RID: 422
		public const int elementId_button1 = 12;

		// Token: 0x040001A7 RID: 423
		public const int elementId_button2 = 13;

		// Token: 0x040001A8 RID: 424
		public const int elementId_button3 = 14;

		// Token: 0x040001A9 RID: 425
		public const int elementId_button4 = 15;

		// Token: 0x040001AA RID: 426
		public const int elementId_button5 = 16;

		// Token: 0x040001AB RID: 427
		public const int elementId_button6 = 17;

		// Token: 0x040001AC RID: 428
		public const int elementId_button7 = 18;

		// Token: 0x040001AD RID: 429
		public const int elementId_button8 = 19;

		// Token: 0x040001AE RID: 430
		public const int elementId_button9 = 20;

		// Token: 0x040001AF RID: 431
		public const int elementId_button10 = 21;

		// Token: 0x040001B0 RID: 432
		public const int elementId_button11 = 22;

		// Token: 0x040001B1 RID: 433
		public const int elementId_button12 = 23;

		// Token: 0x040001B2 RID: 434
		public const int elementId_button13 = 24;

		// Token: 0x040001B3 RID: 435
		public const int elementId_button14 = 25;

		// Token: 0x040001B4 RID: 436
		public const int elementId_button15 = 26;

		// Token: 0x040001B5 RID: 437
		public const int elementId_button16 = 27;

		// Token: 0x040001B6 RID: 438
		public const int elementId_button17 = 28;

		// Token: 0x040001B7 RID: 439
		public const int elementId_button18 = 29;

		// Token: 0x040001B8 RID: 440
		public const int elementId_button19 = 30;

		// Token: 0x040001B9 RID: 441
		public const int elementId_button20 = 31;

		// Token: 0x040001BA RID: 442
		public const int elementId_button21 = 55;

		// Token: 0x040001BB RID: 443
		public const int elementId_button22 = 56;

		// Token: 0x040001BC RID: 444
		public const int elementId_button23 = 57;

		// Token: 0x040001BD RID: 445
		public const int elementId_button24 = 58;

		// Token: 0x040001BE RID: 446
		public const int elementId_button25 = 59;

		// Token: 0x040001BF RID: 447
		public const int elementId_button26 = 60;

		// Token: 0x040001C0 RID: 448
		public const int elementId_button27 = 61;

		// Token: 0x040001C1 RID: 449
		public const int elementId_button28 = 62;

		// Token: 0x040001C2 RID: 450
		public const int elementId_button29 = 63;

		// Token: 0x040001C3 RID: 451
		public const int elementId_button30 = 64;

		// Token: 0x040001C4 RID: 452
		public const int elementId_button31 = 65;

		// Token: 0x040001C5 RID: 453
		public const int elementId_button32 = 66;

		// Token: 0x040001C6 RID: 454
		public const int elementId_hat1Up = 32;

		// Token: 0x040001C7 RID: 455
		public const int elementId_hat1UpRight = 33;

		// Token: 0x040001C8 RID: 456
		public const int elementId_hat1Right = 34;

		// Token: 0x040001C9 RID: 457
		public const int elementId_hat1DownRight = 35;

		// Token: 0x040001CA RID: 458
		public const int elementId_hat1Down = 36;

		// Token: 0x040001CB RID: 459
		public const int elementId_hat1DownLeft = 37;

		// Token: 0x040001CC RID: 460
		public const int elementId_hat1Left = 38;

		// Token: 0x040001CD RID: 461
		public const int elementId_hat1UpLeft = 39;

		// Token: 0x040001CE RID: 462
		public const int elementId_hat2Up = 40;

		// Token: 0x040001CF RID: 463
		public const int elementId_hat2UpRight = 41;

		// Token: 0x040001D0 RID: 464
		public const int elementId_hat2Right = 42;

		// Token: 0x040001D1 RID: 465
		public const int elementId_hat2DownRight = 43;

		// Token: 0x040001D2 RID: 466
		public const int elementId_hat2Down = 44;

		// Token: 0x040001D3 RID: 467
		public const int elementId_hat2DownLeft = 45;

		// Token: 0x040001D4 RID: 468
		public const int elementId_hat2Left = 46;

		// Token: 0x040001D5 RID: 469
		public const int elementId_hat2UpLeft = 47;

		// Token: 0x040001D6 RID: 470
		public const int elementId_hat1 = 48;

		// Token: 0x040001D7 RID: 471
		public const int elementId_hat2 = 49;

		// Token: 0x040001D8 RID: 472
		public const int elementId_throttle1 = 52;

		// Token: 0x040001D9 RID: 473
		public const int elementId_throttle2 = 53;

		// Token: 0x040001DA RID: 474
		public const int elementId_stick = 54;
	}
}
