using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020009E0 RID: 2528
public class UICorpseWidgetData : LazyWidgetDataBase
{
	// Token: 0x17000A41 RID: 2625
	// (get) Token: 0x060043A3 RID: 17315 RVA: 0x00142048 File Offset: 0x00140248
	// (set) Token: 0x060043A4 RID: 17316 RVA: 0x00142050 File Offset: 0x00140250
	public Action OnButtonPressed { get; private set; }

	// Token: 0x17000A42 RID: 2626
	// (get) Token: 0x060043A5 RID: 17317 RVA: 0x00142059 File Offset: 0x00140259
	// (set) Token: 0x060043A6 RID: 17318 RVA: 0x00142061 File Offset: 0x00140261
	public string IconId { get; private set; }

	// Token: 0x17000A43 RID: 2627
	// (get) Token: 0x060043A7 RID: 17319 RVA: 0x0014206A File Offset: 0x0014026A
	// (set) Token: 0x060043A8 RID: 17320 RVA: 0x00142072 File Offset: 0x00140272
	public int RedSkulls { get; private set; }

	// Token: 0x17000A44 RID: 2628
	// (get) Token: 0x060043A9 RID: 17321 RVA: 0x0014207B File Offset: 0x0014027B
	// (set) Token: 0x060043AA RID: 17322 RVA: 0x00142083 File Offset: 0x00140283
	public int WhiteSkulls { get; private set; }

	// Token: 0x17000A45 RID: 2629
	// (get) Token: 0x060043AB RID: 17323 RVA: 0x0014208C File Offset: 0x0014028C
	// (set) Token: 0x060043AC RID: 17324 RVA: 0x00142094 File Offset: 0x00140294
	public bool IsEmpty { get; private set; }

	// Token: 0x17000A46 RID: 2630
	// (get) Token: 0x060043AD RID: 17325 RVA: 0x0014209D File Offset: 0x0014029D
	// (set) Token: 0x060043AE RID: 17326 RVA: 0x001420A5 File Offset: 0x001402A5
	public bool ButtonInteractable { get; private set; }

	// Token: 0x17000A47 RID: 2631
	// (get) Token: 0x060043AF RID: 17327 RVA: 0x001420AE File Offset: 0x001402AE
	// (set) Token: 0x060043B0 RID: 17328 RVA: 0x001420B6 File Offset: 0x001402B6
	public string DescriptionText { get; private set; }

	// Token: 0x17000A48 RID: 2632
	// (get) Token: 0x060043B1 RID: 17329 RVA: 0x001420BF File Offset: 0x001402BF
	// (set) Token: 0x060043B2 RID: 17330 RVA: 0x001420C7 File Offset: 0x001402C7
	public string HeaderText { get; private set; }

	// Token: 0x17000A49 RID: 2633
	// (get) Token: 0x060043B3 RID: 17331 RVA: 0x001420D0 File Offset: 0x001402D0
	// (set) Token: 0x060043B4 RID: 17332 RVA: 0x001420D8 File Offset: 0x001402D8
	public string ButtonText { get; private set; }

	// Token: 0x17000A4A RID: 2634
	// (get) Token: 0x060043B5 RID: 17333 RVA: 0x001420E1 File Offset: 0x001402E1
	// (set) Token: 0x060043B6 RID: 17334 RVA: 0x001420E9 File Offset: 0x001402E9
	public WgoData WgoData { get; private set; }

	// Token: 0x17000A4B RID: 2635
	// (get) Token: 0x060043B7 RID: 17335 RVA: 0x001420F2 File Offset: 0x001402F2
	// (set) Token: 0x060043B8 RID: 17336 RVA: 0x001420FA File Offset: 0x001402FA
	public Item Body { get; private set; }

	// Token: 0x17000A4C RID: 2636
	// (get) Token: 0x060043B9 RID: 17337 RVA: 0x00142103 File Offset: 0x00140303
	// (set) Token: 0x060043BA RID: 17338 RVA: 0x0014210B File Offset: 0x0014030B
	public ZombieWgoData ZombieWgoData { get; private set; }

	// Token: 0x17000A4D RID: 2637
	// (get) Token: 0x060043BB RID: 17339 RVA: 0x00142114 File Offset: 0x00140314
	// (set) Token: 0x060043BC RID: 17340 RVA: 0x0014211C File Offset: 0x0014031C
	public bool IsZombie { get; private set; }

	// Token: 0x17000A4E RID: 2638
	// (get) Token: 0x060043BD RID: 17341 RVA: 0x00142125 File Offset: 0x00140325
	// (set) Token: 0x060043BE RID: 17342 RVA: 0x0014212D File Offset: 0x0014032D
	public GameKey GameKeyToExhume { get; private set; }

	// Token: 0x17000A4F RID: 2639
	// (get) Token: 0x060043BF RID: 17343 RVA: 0x00142136 File Offset: 0x00140336
	// (set) Token: 0x060043C0 RID: 17344 RVA: 0x0014213E File Offset: 0x0014033E
	public Action<LazyButton> OnNonInteractableButtonOver { get; private set; }

	// Token: 0x17000A50 RID: 2640
	// (get) Token: 0x060043C1 RID: 17345 RVA: 0x00142147 File Offset: 0x00140347
	// (set) Token: 0x060043C2 RID: 17346 RVA: 0x0014214F File Offset: 0x0014034F
	public int CollarRedSkullsLimit { get; set; } = -1;

	// Token: 0x060043C3 RID: 17347 RVA: 0x00142158 File Offset: 0x00140358
	public UICorpseWidgetData(GameKey gameKeyToExhume)
	{
		this.IsEmpty = true;
		this.HeaderText = LLBase.L("ui_grave_corpse_widget_header");
		this.DescriptionText = LLBase.L("ui_put_body");
		this.ButtonText = LLBase.L("btn_take_body_two_lines");
		this.IconId = "i_body";
		this.GameKeyToExhume = gameKeyToExhume;
	}

	// Token: 0x060043C4 RID: 17348 RVA: 0x001421BC File Offset: 0x001403BC
	public UICorpseWidgetData(Item body, WgoData wgoData, Action onButtonPressed, bool buttonInteractable, GameKey gameKeyToExhume, string headerText = null, string descriptionText = null, string buttonText = null, Action<LazyButton> onNonInteractableButtonOver = null)
	{
		this.GameKeyToExhume = gameKeyToExhume;
		this.IsEmpty = false;
		this.ButtonInteractable = buttonInteractable;
		this.OnButtonPressed = onButtonPressed;
		this.WgoData = wgoData;
		this.Body = body;
		this.DescriptionText = descriptionText;
		this.HeaderText = headerText;
		this.ButtonText = buttonText;
		this.IconId = body.Definition.iconId;
		this.RedSkulls = 0;
		this.WhiteSkulls = 0;
		foreach (Item item in body.Inventory)
		{
			this.RedSkulls += item.Definition.redSkulls * item.Count;
			this.WhiteSkulls += item.Definition.whiteSkulls * item.Count;
		}
		this.RedSkulls = Mathf.Clamp(this.RedSkulls, 0, 999);
		this.WhiteSkulls = Mathf.Clamp(this.WhiteSkulls, 0, 999);
		this.ZombieWgoData = MainGame.ZombieSystemData.GetZombie(body.UniqueId);
		this.IsZombie = this.ZombieWgoData != null;
		this.OnNonInteractableButtonOver = onNonInteractableButtonOver;
	}
}
