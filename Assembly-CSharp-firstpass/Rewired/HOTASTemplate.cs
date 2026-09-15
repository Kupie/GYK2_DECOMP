using System;

namespace Rewired
{
	// Token: 0x0200001B RID: 27
	public sealed class HOTASTemplate : ControllerTemplate, IHOTASTemplate, IControllerTemplate
	{
		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000198 RID: 408 RVA: 0x00002FF5 File Offset: 0x000011F5
		IControllerTemplateButton IHOTASTemplate.stickTrigger
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(3);
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000199 RID: 409 RVA: 0x00002DFD File Offset: 0x00000FFD
		IControllerTemplateButton IHOTASTemplate.stickTriggerStage2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(4);
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x0600019A RID: 410 RVA: 0x00002E06 File Offset: 0x00001006
		IControllerTemplateButton IHOTASTemplate.stickPinkyButton
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(5);
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x0600019B RID: 411 RVA: 0x00002FFE File Offset: 0x000011FE
		IControllerTemplateButton IHOTASTemplate.stickPinkyTrigger
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(154);
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x0600019C RID: 412 RVA: 0x00002E0F File Offset: 0x0000100F
		IControllerTemplateButton IHOTASTemplate.stickButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(6);
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x0600019D RID: 413 RVA: 0x00002E18 File Offset: 0x00001018
		IControllerTemplateButton IHOTASTemplate.stickButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(7);
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x0600019E RID: 414 RVA: 0x00002E21 File Offset: 0x00001021
		IControllerTemplateButton IHOTASTemplate.stickButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(8);
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x0600019F RID: 415 RVA: 0x00002E2A File Offset: 0x0000102A
		IControllerTemplateButton IHOTASTemplate.stickButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(9);
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x00002E34 File Offset: 0x00001034
		IControllerTemplateButton IHOTASTemplate.stickButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(10);
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x00002ED6 File Offset: 0x000010D6
		IControllerTemplateButton IHOTASTemplate.stickButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(11);
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x00002E48 File Offset: 0x00001048
		IControllerTemplateButton IHOTASTemplate.stickButton7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(12);
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x00002EE0 File Offset: 0x000010E0
		IControllerTemplateButton IHOTASTemplate.stickButton8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(13);
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x00002E5C File Offset: 0x0000105C
		IControllerTemplateButton IHOTASTemplate.stickButton9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(14);
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x00002E66 File Offset: 0x00001066
		IControllerTemplateButton IHOTASTemplate.stickButton10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(15);
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x00002EF4 File Offset: 0x000010F4
		IControllerTemplateButton IHOTASTemplate.stickBaseButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(18);
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x00002EFE File Offset: 0x000010FE
		IControllerTemplateButton IHOTASTemplate.stickBaseButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(19);
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x00002F08 File Offset: 0x00001108
		IControllerTemplateButton IHOTASTemplate.stickBaseButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(20);
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x00002F12 File Offset: 0x00001112
		IControllerTemplateButton IHOTASTemplate.stickBaseButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(21);
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00002F1C File Offset: 0x0000111C
		IControllerTemplateButton IHOTASTemplate.stickBaseButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(22);
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x060001AB RID: 427 RVA: 0x00002F26 File Offset: 0x00001126
		IControllerTemplateButton IHOTASTemplate.stickBaseButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(23);
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060001AC RID: 428 RVA: 0x00002F30 File Offset: 0x00001130
		IControllerTemplateButton IHOTASTemplate.stickBaseButton7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(24);
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060001AD RID: 429 RVA: 0x00002F3A File Offset: 0x0000113A
		IControllerTemplateButton IHOTASTemplate.stickBaseButton8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(25);
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x060001AE RID: 430 RVA: 0x00002F44 File Offset: 0x00001144
		IControllerTemplateButton IHOTASTemplate.stickBaseButton9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(26);
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x060001AF RID: 431 RVA: 0x00002F4E File Offset: 0x0000114E
		IControllerTemplateButton IHOTASTemplate.stickBaseButton10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(27);
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x0000300B File Offset: 0x0000120B
		IControllerTemplateButton IHOTASTemplate.stickBaseButton11
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(161);
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x00003018 File Offset: 0x00001218
		IControllerTemplateButton IHOTASTemplate.stickBaseButton12
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(162);
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x00002FA8 File Offset: 0x000011A8
		IControllerTemplateButton IHOTASTemplate.mode1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(44);
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x00003025 File Offset: 0x00001225
		IControllerTemplateButton IHOTASTemplate.mode2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(45);
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x0000302F File Offset: 0x0000122F
		IControllerTemplateButton IHOTASTemplate.mode3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(46);
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x00003039 File Offset: 0x00001239
		IControllerTemplateButton IHOTASTemplate.throttleButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(50);
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x00003043 File Offset: 0x00001243
		IControllerTemplateButton IHOTASTemplate.throttleButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(51);
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x0000304D File Offset: 0x0000124D
		IControllerTemplateButton IHOTASTemplate.throttleButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(52);
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x00003057 File Offset: 0x00001257
		IControllerTemplateButton IHOTASTemplate.throttleButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(53);
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x00003061 File Offset: 0x00001261
		IControllerTemplateButton IHOTASTemplate.throttleButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(54);
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x060001BA RID: 442 RVA: 0x0000306B File Offset: 0x0000126B
		IControllerTemplateButton IHOTASTemplate.throttleButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(55);
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x060001BB RID: 443 RVA: 0x00003075 File Offset: 0x00001275
		IControllerTemplateButton IHOTASTemplate.throttleButton7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(56);
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x060001BC RID: 444 RVA: 0x0000307F File Offset: 0x0000127F
		IControllerTemplateButton IHOTASTemplate.throttleButton8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(57);
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x060001BD RID: 445 RVA: 0x00003089 File Offset: 0x00001289
		IControllerTemplateButton IHOTASTemplate.throttleButton9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(58);
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x060001BE RID: 446 RVA: 0x00003093 File Offset: 0x00001293
		IControllerTemplateButton IHOTASTemplate.throttleButton10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(59);
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x060001BF RID: 447 RVA: 0x0000309D File Offset: 0x0000129D
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(60);
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x000030A7 File Offset: 0x000012A7
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(61);
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x000030B1 File Offset: 0x000012B1
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(62);
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x000030BB File Offset: 0x000012BB
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(63);
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x000030C5 File Offset: 0x000012C5
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(64);
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x000030CF File Offset: 0x000012CF
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(65);
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x000030D9 File Offset: 0x000012D9
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(66);
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x000030E3 File Offset: 0x000012E3
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(67);
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x000030ED File Offset: 0x000012ED
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(68);
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x000030F7 File Offset: 0x000012F7
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(69);
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x00003101 File Offset: 0x00001301
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton11
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(132);
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x060001CA RID: 458 RVA: 0x0000310E File Offset: 0x0000130E
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton12
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(133);
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x060001CB RID: 459 RVA: 0x0000311B File Offset: 0x0000131B
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton13
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(134);
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060001CC RID: 460 RVA: 0x00003128 File Offset: 0x00001328
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton14
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(135);
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060001CD RID: 461 RVA: 0x00003135 File Offset: 0x00001335
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton15
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(136);
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060001CE RID: 462 RVA: 0x00003142 File Offset: 0x00001342
		IControllerTemplateAxis IHOTASTemplate.throttleSlider1
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(70);
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060001CF RID: 463 RVA: 0x0000314C File Offset: 0x0000134C
		IControllerTemplateAxis IHOTASTemplate.throttleSlider2
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(71);
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x00003156 File Offset: 0x00001356
		IControllerTemplateAxis IHOTASTemplate.throttleSlider3
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(72);
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x00003160 File Offset: 0x00001360
		IControllerTemplateAxis IHOTASTemplate.throttleSlider4
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(73);
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060001D2 RID: 466 RVA: 0x0000316A File Offset: 0x0000136A
		IControllerTemplateAxis IHOTASTemplate.throttleDial1
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(74);
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x00003174 File Offset: 0x00001374
		IControllerTemplateAxis IHOTASTemplate.throttleDial2
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(142);
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x00003181 File Offset: 0x00001381
		IControllerTemplateAxis IHOTASTemplate.throttleDial3
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(143);
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x0000318E File Offset: 0x0000138E
		IControllerTemplateAxis IHOTASTemplate.throttleDial4
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(144);
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060001D6 RID: 470 RVA: 0x0000319B File Offset: 0x0000139B
		IControllerTemplateButton IHOTASTemplate.throttleWheel1Forward
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(145);
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x000031A8 File Offset: 0x000013A8
		IControllerTemplateButton IHOTASTemplate.throttleWheel1Back
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(146);
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x000031B5 File Offset: 0x000013B5
		IControllerTemplateButton IHOTASTemplate.throttleWheel1Press
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(147);
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x000031C2 File Offset: 0x000013C2
		IControllerTemplateButton IHOTASTemplate.throttleWheel2Forward
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(148);
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060001DA RID: 474 RVA: 0x000031CF File Offset: 0x000013CF
		IControllerTemplateButton IHOTASTemplate.throttleWheel2Back
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(149);
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060001DB RID: 475 RVA: 0x000031DC File Offset: 0x000013DC
		IControllerTemplateButton IHOTASTemplate.throttleWheel2Press
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(150);
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060001DC RID: 476 RVA: 0x000031E9 File Offset: 0x000013E9
		IControllerTemplateButton IHOTASTemplate.throttleWheel3Forward
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(151);
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060001DD RID: 477 RVA: 0x000031F6 File Offset: 0x000013F6
		IControllerTemplateButton IHOTASTemplate.throttleWheel3Back
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(152);
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060001DE RID: 478 RVA: 0x00003203 File Offset: 0x00001403
		IControllerTemplateButton IHOTASTemplate.throttleWheel3Press
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(153);
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060001DF RID: 479 RVA: 0x00003210 File Offset: 0x00001410
		IControllerTemplateAxis IHOTASTemplate.leftPedal
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(168);
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x0000321D File Offset: 0x0000141D
		IControllerTemplateAxis IHOTASTemplate.rightPedal
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(169);
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x0000322A File Offset: 0x0000142A
		IControllerTemplateAxis IHOTASTemplate.slidePedals
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(170);
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x00003237 File Offset: 0x00001437
		IControllerTemplateStick IHOTASTemplate.stick
		{
			get
			{
				return base.GetElement<IControllerTemplateStick>(171);
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x00003244 File Offset: 0x00001444
		IControllerTemplateThumbStick IHOTASTemplate.stickMiniStick1
		{
			get
			{
				return base.GetElement<IControllerTemplateThumbStick>(172);
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x00003251 File Offset: 0x00001451
		IControllerTemplateThumbStick IHOTASTemplate.stickMiniStick2
		{
			get
			{
				return base.GetElement<IControllerTemplateThumbStick>(173);
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x0000325E File Offset: 0x0000145E
		IControllerTemplateHat IHOTASTemplate.stickHat1
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(174);
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x0000326B File Offset: 0x0000146B
		IControllerTemplateHat IHOTASTemplate.stickHat2
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(175);
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x00003278 File Offset: 0x00001478
		IControllerTemplateHat IHOTASTemplate.stickHat3
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(176);
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x00003285 File Offset: 0x00001485
		IControllerTemplateHat IHOTASTemplate.stickHat4
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(177);
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x00003292 File Offset: 0x00001492
		IControllerTemplateThrottle IHOTASTemplate.throttle1
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(178);
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060001EA RID: 490 RVA: 0x0000329F File Offset: 0x0000149F
		IControllerTemplateThrottle IHOTASTemplate.throttle2
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(179);
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060001EB RID: 491 RVA: 0x000032AC File Offset: 0x000014AC
		IControllerTemplateThumbStick IHOTASTemplate.throttleMiniStick
		{
			get
			{
				return base.GetElement<IControllerTemplateThumbStick>(180);
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060001EC RID: 492 RVA: 0x000032B9 File Offset: 0x000014B9
		IControllerTemplateHat IHOTASTemplate.throttleHat1
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(181);
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060001ED RID: 493 RVA: 0x000032C6 File Offset: 0x000014C6
		IControllerTemplateHat IHOTASTemplate.throttleHat2
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(182);
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060001EE RID: 494 RVA: 0x000032D3 File Offset: 0x000014D3
		IControllerTemplateHat IHOTASTemplate.throttleHat3
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(183);
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060001EF RID: 495 RVA: 0x000032E0 File Offset: 0x000014E0
		IControllerTemplateHat IHOTASTemplate.throttleHat4
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(184);
			}
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00002E98 File Offset: 0x00001098
		public HOTASTemplate(object payload)
			: base(payload)
		{
		}

		// Token: 0x0400009C RID: 156
		public static readonly Guid typeGuid = new Guid("061a00cf-d8c2-4f8d-8cb5-a15a010bc53e");

		// Token: 0x0400009D RID: 157
		public const int elementId_stickX = 0;

		// Token: 0x0400009E RID: 158
		public const int elementId_stickY = 1;

		// Token: 0x0400009F RID: 159
		public const int elementId_stickRotate = 2;

		// Token: 0x040000A0 RID: 160
		public const int elementId_stickMiniStick1X = 78;

		// Token: 0x040000A1 RID: 161
		public const int elementId_stickMiniStick1Y = 79;

		// Token: 0x040000A2 RID: 162
		public const int elementId_stickMiniStick1Press = 80;

		// Token: 0x040000A3 RID: 163
		public const int elementId_stickMiniStick2X = 81;

		// Token: 0x040000A4 RID: 164
		public const int elementId_stickMiniStick2Y = 82;

		// Token: 0x040000A5 RID: 165
		public const int elementId_stickMiniStick2Press = 83;

		// Token: 0x040000A6 RID: 166
		public const int elementId_stickTrigger = 3;

		// Token: 0x040000A7 RID: 167
		public const int elementId_stickTriggerStage2 = 4;

		// Token: 0x040000A8 RID: 168
		public const int elementId_stickPinkyButton = 5;

		// Token: 0x040000A9 RID: 169
		public const int elementId_stickPinkyTrigger = 154;

		// Token: 0x040000AA RID: 170
		public const int elementId_stickButton1 = 6;

		// Token: 0x040000AB RID: 171
		public const int elementId_stickButton2 = 7;

		// Token: 0x040000AC RID: 172
		public const int elementId_stickButton3 = 8;

		// Token: 0x040000AD RID: 173
		public const int elementId_stickButton4 = 9;

		// Token: 0x040000AE RID: 174
		public const int elementId_stickButton5 = 10;

		// Token: 0x040000AF RID: 175
		public const int elementId_stickButton6 = 11;

		// Token: 0x040000B0 RID: 176
		public const int elementId_stickButton7 = 12;

		// Token: 0x040000B1 RID: 177
		public const int elementId_stickButton8 = 13;

		// Token: 0x040000B2 RID: 178
		public const int elementId_stickButton9 = 14;

		// Token: 0x040000B3 RID: 179
		public const int elementId_stickButton10 = 15;

		// Token: 0x040000B4 RID: 180
		public const int elementId_stickBaseButton1 = 18;

		// Token: 0x040000B5 RID: 181
		public const int elementId_stickBaseButton2 = 19;

		// Token: 0x040000B6 RID: 182
		public const int elementId_stickBaseButton3 = 20;

		// Token: 0x040000B7 RID: 183
		public const int elementId_stickBaseButton4 = 21;

		// Token: 0x040000B8 RID: 184
		public const int elementId_stickBaseButton5 = 22;

		// Token: 0x040000B9 RID: 185
		public const int elementId_stickBaseButton6 = 23;

		// Token: 0x040000BA RID: 186
		public const int elementId_stickBaseButton7 = 24;

		// Token: 0x040000BB RID: 187
		public const int elementId_stickBaseButton8 = 25;

		// Token: 0x040000BC RID: 188
		public const int elementId_stickBaseButton9 = 26;

		// Token: 0x040000BD RID: 189
		public const int elementId_stickBaseButton10 = 27;

		// Token: 0x040000BE RID: 190
		public const int elementId_stickBaseButton11 = 161;

		// Token: 0x040000BF RID: 191
		public const int elementId_stickBaseButton12 = 162;

		// Token: 0x040000C0 RID: 192
		public const int elementId_stickHat1Up = 28;

		// Token: 0x040000C1 RID: 193
		public const int elementId_stickHat1UpRight = 29;

		// Token: 0x040000C2 RID: 194
		public const int elementId_stickHat1Right = 30;

		// Token: 0x040000C3 RID: 195
		public const int elementId_stickHat1DownRight = 31;

		// Token: 0x040000C4 RID: 196
		public const int elementId_stickHat1Down = 32;

		// Token: 0x040000C5 RID: 197
		public const int elementId_stickHat1DownLeft = 33;

		// Token: 0x040000C6 RID: 198
		public const int elementId_stickHat1Left = 34;

		// Token: 0x040000C7 RID: 199
		public const int elementId_stickHat1Up_Left = 35;

		// Token: 0x040000C8 RID: 200
		public const int elementId_stickHat2Up = 36;

		// Token: 0x040000C9 RID: 201
		public const int elementId_stickHat2Up_right = 37;

		// Token: 0x040000CA RID: 202
		public const int elementId_stickHat2Right = 38;

		// Token: 0x040000CB RID: 203
		public const int elementId_stickHat2Down_Right = 39;

		// Token: 0x040000CC RID: 204
		public const int elementId_stickHat2Down = 40;

		// Token: 0x040000CD RID: 205
		public const int elementId_stickHat2Down_Left = 41;

		// Token: 0x040000CE RID: 206
		public const int elementId_stickHat2Left = 42;

		// Token: 0x040000CF RID: 207
		public const int elementId_stickHat2Up_Left = 43;

		// Token: 0x040000D0 RID: 208
		public const int elementId_stickHat3Up = 84;

		// Token: 0x040000D1 RID: 209
		public const int elementId_stickHat3Up_Right = 85;

		// Token: 0x040000D2 RID: 210
		public const int elementId_stickHat3Right = 86;

		// Token: 0x040000D3 RID: 211
		public const int elementId_stickHat3Down_Right = 87;

		// Token: 0x040000D4 RID: 212
		public const int elementId_stickHat3Down = 88;

		// Token: 0x040000D5 RID: 213
		public const int elementId_stickHat3Down_Left = 89;

		// Token: 0x040000D6 RID: 214
		public const int elementId_stickHat3Left = 90;

		// Token: 0x040000D7 RID: 215
		public const int elementId_stickHat3Up_Left = 91;

		// Token: 0x040000D8 RID: 216
		public const int elementId_stickHat4Up = 92;

		// Token: 0x040000D9 RID: 217
		public const int elementId_stickHat4Up_Right = 93;

		// Token: 0x040000DA RID: 218
		public const int elementId_stickHat4Right = 94;

		// Token: 0x040000DB RID: 219
		public const int elementId_stickHat4Down_Right = 95;

		// Token: 0x040000DC RID: 220
		public const int elementId_stickHat4Down = 96;

		// Token: 0x040000DD RID: 221
		public const int elementId_stickHat4Down_Left = 97;

		// Token: 0x040000DE RID: 222
		public const int elementId_stickHat4Left = 98;

		// Token: 0x040000DF RID: 223
		public const int elementId_stickHat4Up_Left = 99;

		// Token: 0x040000E0 RID: 224
		public const int elementId_mode1 = 44;

		// Token: 0x040000E1 RID: 225
		public const int elementId_mode2 = 45;

		// Token: 0x040000E2 RID: 226
		public const int elementId_mode3 = 46;

		// Token: 0x040000E3 RID: 227
		public const int elementId_throttle1Axis = 49;

		// Token: 0x040000E4 RID: 228
		public const int elementId_throttle2Axis = 155;

		// Token: 0x040000E5 RID: 229
		public const int elementId_throttle1MinDetent = 166;

		// Token: 0x040000E6 RID: 230
		public const int elementId_throttle2MinDetent = 167;

		// Token: 0x040000E7 RID: 231
		public const int elementId_throttleButton1 = 50;

		// Token: 0x040000E8 RID: 232
		public const int elementId_throttleButton2 = 51;

		// Token: 0x040000E9 RID: 233
		public const int elementId_throttleButton3 = 52;

		// Token: 0x040000EA RID: 234
		public const int elementId_throttleButton4 = 53;

		// Token: 0x040000EB RID: 235
		public const int elementId_throttleButton5 = 54;

		// Token: 0x040000EC RID: 236
		public const int elementId_throttleButton6 = 55;

		// Token: 0x040000ED RID: 237
		public const int elementId_throttleButton7 = 56;

		// Token: 0x040000EE RID: 238
		public const int elementId_throttleButton8 = 57;

		// Token: 0x040000EF RID: 239
		public const int elementId_throttleButton9 = 58;

		// Token: 0x040000F0 RID: 240
		public const int elementId_throttleButton10 = 59;

		// Token: 0x040000F1 RID: 241
		public const int elementId_throttleBaseButton1 = 60;

		// Token: 0x040000F2 RID: 242
		public const int elementId_throttleBaseButton2 = 61;

		// Token: 0x040000F3 RID: 243
		public const int elementId_throttleBaseButton3 = 62;

		// Token: 0x040000F4 RID: 244
		public const int elementId_throttleBaseButton4 = 63;

		// Token: 0x040000F5 RID: 245
		public const int elementId_throttleBaseButton5 = 64;

		// Token: 0x040000F6 RID: 246
		public const int elementId_throttleBaseButton6 = 65;

		// Token: 0x040000F7 RID: 247
		public const int elementId_throttleBaseButton7 = 66;

		// Token: 0x040000F8 RID: 248
		public const int elementId_throttleBaseButton8 = 67;

		// Token: 0x040000F9 RID: 249
		public const int elementId_throttleBaseButton9 = 68;

		// Token: 0x040000FA RID: 250
		public const int elementId_throttleBaseButton10 = 69;

		// Token: 0x040000FB RID: 251
		public const int elementId_throttleBaseButton11 = 132;

		// Token: 0x040000FC RID: 252
		public const int elementId_throttleBaseButton12 = 133;

		// Token: 0x040000FD RID: 253
		public const int elementId_throttleBaseButton13 = 134;

		// Token: 0x040000FE RID: 254
		public const int elementId_throttleBaseButton14 = 135;

		// Token: 0x040000FF RID: 255
		public const int elementId_throttleBaseButton15 = 136;

		// Token: 0x04000100 RID: 256
		public const int elementId_throttleSlider1 = 70;

		// Token: 0x04000101 RID: 257
		public const int elementId_throttleSlider2 = 71;

		// Token: 0x04000102 RID: 258
		public const int elementId_throttleSlider3 = 72;

		// Token: 0x04000103 RID: 259
		public const int elementId_throttleSlider4 = 73;

		// Token: 0x04000104 RID: 260
		public const int elementId_throttleDial1 = 74;

		// Token: 0x04000105 RID: 261
		public const int elementId_throttleDial2 = 142;

		// Token: 0x04000106 RID: 262
		public const int elementId_throttleDial3 = 143;

		// Token: 0x04000107 RID: 263
		public const int elementId_throttleDial4 = 144;

		// Token: 0x04000108 RID: 264
		public const int elementId_throttleMiniStickX = 75;

		// Token: 0x04000109 RID: 265
		public const int elementId_throttleMiniStickY = 76;

		// Token: 0x0400010A RID: 266
		public const int elementId_throttleMiniStickPress = 77;

		// Token: 0x0400010B RID: 267
		public const int elementId_throttleWheel1Forward = 145;

		// Token: 0x0400010C RID: 268
		public const int elementId_throttleWheel1Back = 146;

		// Token: 0x0400010D RID: 269
		public const int elementId_throttleWheel1Press = 147;

		// Token: 0x0400010E RID: 270
		public const int elementId_throttleWheel2Forward = 148;

		// Token: 0x0400010F RID: 271
		public const int elementId_throttleWheel2Back = 149;

		// Token: 0x04000110 RID: 272
		public const int elementId_throttleWheel2Press = 150;

		// Token: 0x04000111 RID: 273
		public const int elementId_throttleWheel3Forward = 151;

		// Token: 0x04000112 RID: 274
		public const int elementId_throttleWheel3Back = 152;

		// Token: 0x04000113 RID: 275
		public const int elementId_throttleWheel3Press = 153;

		// Token: 0x04000114 RID: 276
		public const int elementId_throttleHat1Up = 100;

		// Token: 0x04000115 RID: 277
		public const int elementId_throttleHat1Up_Right = 101;

		// Token: 0x04000116 RID: 278
		public const int elementId_throttleHat1Right = 102;

		// Token: 0x04000117 RID: 279
		public const int elementId_throttleHat1Down_Right = 103;

		// Token: 0x04000118 RID: 280
		public const int elementId_throttleHat1Down = 104;

		// Token: 0x04000119 RID: 281
		public const int elementId_throttleHat1Down_Left = 105;

		// Token: 0x0400011A RID: 282
		public const int elementId_throttleHat1Left = 106;

		// Token: 0x0400011B RID: 283
		public const int elementId_throttleHat1Up_Left = 107;

		// Token: 0x0400011C RID: 284
		public const int elementId_throttleHat2Up = 108;

		// Token: 0x0400011D RID: 285
		public const int elementId_throttleHat2Up_Right = 109;

		// Token: 0x0400011E RID: 286
		public const int elementId_throttleHat2Right = 110;

		// Token: 0x0400011F RID: 287
		public const int elementId_throttleHat2Down_Right = 111;

		// Token: 0x04000120 RID: 288
		public const int elementId_throttleHat2Down = 112;

		// Token: 0x04000121 RID: 289
		public const int elementId_throttleHat2Down_Left = 113;

		// Token: 0x04000122 RID: 290
		public const int elementId_throttleHat2Left = 114;

		// Token: 0x04000123 RID: 291
		public const int elementId_throttleHat2Up_Left = 115;

		// Token: 0x04000124 RID: 292
		public const int elementId_throttleHat3Up = 116;

		// Token: 0x04000125 RID: 293
		public const int elementId_throttleHat3Up_Right = 117;

		// Token: 0x04000126 RID: 294
		public const int elementId_throttleHat3Right = 118;

		// Token: 0x04000127 RID: 295
		public const int elementId_throttleHat3Down_Right = 119;

		// Token: 0x04000128 RID: 296
		public const int elementId_throttleHat3Down = 120;

		// Token: 0x04000129 RID: 297
		public const int elementId_throttleHat3Down_Left = 121;

		// Token: 0x0400012A RID: 298
		public const int elementId_throttleHat3Left = 122;

		// Token: 0x0400012B RID: 299
		public const int elementId_throttleHat3Up_Left = 123;

		// Token: 0x0400012C RID: 300
		public const int elementId_throttleHat4Up = 124;

		// Token: 0x0400012D RID: 301
		public const int elementId_throttleHat4Up_Right = 125;

		// Token: 0x0400012E RID: 302
		public const int elementId_throttleHat4Right = 126;

		// Token: 0x0400012F RID: 303
		public const int elementId_throttleHat4Down_Right = 127;

		// Token: 0x04000130 RID: 304
		public const int elementId_throttleHat4Down = 128;

		// Token: 0x04000131 RID: 305
		public const int elementId_throttleHat4Down_Left = 129;

		// Token: 0x04000132 RID: 306
		public const int elementId_throttleHat4Left = 130;

		// Token: 0x04000133 RID: 307
		public const int elementId_throttleHat4Up_Left = 131;

		// Token: 0x04000134 RID: 308
		public const int elementId_leftPedal = 168;

		// Token: 0x04000135 RID: 309
		public const int elementId_rightPedal = 169;

		// Token: 0x04000136 RID: 310
		public const int elementId_slidePedals = 170;

		// Token: 0x04000137 RID: 311
		public const int elementId_stick = 171;

		// Token: 0x04000138 RID: 312
		public const int elementId_stickMiniStick1 = 172;

		// Token: 0x04000139 RID: 313
		public const int elementId_stickMiniStick2 = 173;

		// Token: 0x0400013A RID: 314
		public const int elementId_stickHat1 = 174;

		// Token: 0x0400013B RID: 315
		public const int elementId_stickHat2 = 175;

		// Token: 0x0400013C RID: 316
		public const int elementId_stickHat3 = 176;

		// Token: 0x0400013D RID: 317
		public const int elementId_stickHat4 = 177;

		// Token: 0x0400013E RID: 318
		public const int elementId_throttle1 = 178;

		// Token: 0x0400013F RID: 319
		public const int elementId_throttle2 = 179;

		// Token: 0x04000140 RID: 320
		public const int elementId_throttleMiniStick = 180;

		// Token: 0x04000141 RID: 321
		public const int elementId_throttleHat1 = 181;

		// Token: 0x04000142 RID: 322
		public const int elementId_throttleHat2 = 182;

		// Token: 0x04000143 RID: 323
		public const int elementId_throttleHat3 = 183;

		// Token: 0x04000144 RID: 324
		public const int elementId_throttleHat4 = 184;
	}
}
