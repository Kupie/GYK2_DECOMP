using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020009EC RID: 2540
public class UIDialogWindowData : LazyWidgetDataBase
{
	// Token: 0x17000A62 RID: 2658
	// (get) Token: 0x0600443D RID: 17469 RVA: 0x0014451E File Offset: 0x0014271E
	// (set) Token: 0x0600443E RID: 17470 RVA: 0x00144526 File Offset: 0x00142726
	public string Information { get; private set; }

	// Token: 0x17000A63 RID: 2659
	// (get) Token: 0x0600443F RID: 17471 RVA: 0x0014452F File Offset: 0x0014272F
	// (set) Token: 0x06004440 RID: 17472 RVA: 0x00144537 File Offset: 0x00142737
	public string Header { get; private set; }

	// Token: 0x17000A64 RID: 2660
	// (get) Token: 0x06004441 RID: 17473 RVA: 0x00144540 File Offset: 0x00142740
	// (set) Token: 0x06004442 RID: 17474 RVA: 0x00144548 File Offset: 0x00142748
	public List<UIDialogWindowData.ButtonData> ButtonsData { get; set; }

	// Token: 0x17000A65 RID: 2661
	// (get) Token: 0x06004443 RID: 17475 RVA: 0x00144551 File Offset: 0x00142751
	// (set) Token: 0x06004444 RID: 17476 RVA: 0x00144559 File Offset: 0x00142759
	public bool ShowCloseButton { get; set; }

	// Token: 0x17000A66 RID: 2662
	// (get) Token: 0x06004445 RID: 17477 RVA: 0x00144562 File Offset: 0x00142762
	// (set) Token: 0x06004446 RID: 17478 RVA: 0x0014456A File Offset: 0x0014276A
	public Action CloseButtonAction { get; set; }

	// Token: 0x17000A67 RID: 2663
	// (get) Token: 0x06004447 RID: 17479 RVA: 0x00144573 File Offset: 0x00142773
	// (set) Token: 0x06004448 RID: 17480 RVA: 0x0014457B File Offset: 0x0014277B
	public Item Item { get; private set; }

	// Token: 0x17000A68 RID: 2664
	// (get) Token: 0x06004449 RID: 17481 RVA: 0x00144584 File Offset: 0x00142784
	// (set) Token: 0x0600444A RID: 17482 RVA: 0x0014458C File Offset: 0x0014278C
	public bool DrawItemCounter { get; private set; }

	// Token: 0x17000A69 RID: 2665
	// (get) Token: 0x0600444B RID: 17483 RVA: 0x00144595 File Offset: 0x00142795
	// (set) Token: 0x0600444C RID: 17484 RVA: 0x0014459D File Offset: 0x0014279D
	public string ItemIconId { get; private set; }

	// Token: 0x17000A6A RID: 2666
	// (get) Token: 0x0600444D RID: 17485 RVA: 0x001445A6 File Offset: 0x001427A6
	// (set) Token: 0x0600444E RID: 17486 RVA: 0x001445AE File Offset: 0x001427AE
	public string ItemName { get; private set; }

	// Token: 0x17000A6B RID: 2667
	// (get) Token: 0x0600444F RID: 17487 RVA: 0x001445B7 File Offset: 0x001427B7
	// (set) Token: 0x06004450 RID: 17488 RVA: 0x001445BF File Offset: 0x001427BF
	public string InformationBot { get; private set; }

	// Token: 0x17000A6C RID: 2668
	// (get) Token: 0x06004451 RID: 17489 RVA: 0x001445C8 File Offset: 0x001427C8
	// (set) Token: 0x06004452 RID: 17490 RVA: 0x001445D0 File Offset: 0x001427D0
	public int HasItemCount { get; private set; }

	// Token: 0x17000A6D RID: 2669
	// (get) Token: 0x06004453 RID: 17491 RVA: 0x001445D9 File Offset: 0x001427D9
	// (set) Token: 0x06004454 RID: 17492 RVA: 0x001445E1 File Offset: 0x001427E1
	public int NeedItemCount { get; private set; }

	// Token: 0x17000A6E RID: 2670
	// (get) Token: 0x06004455 RID: 17493 RVA: 0x001445EA File Offset: 0x001427EA
	// (set) Token: 0x06004456 RID: 17494 RVA: 0x001445F2 File Offset: 0x001427F2
	public bool ShowAltVersion { get; private set; }

	// Token: 0x06004457 RID: 17495 RVA: 0x001445FB File Offset: 0x001427FB
	public UIDialogWindowData(string header, string information, List<UIDialogWindowData.ButtonData> buttonsData)
	{
		this.Information = information;
		this.Header = header;
		this.ButtonsData = buttonsData;
		if (string.IsNullOrEmpty(header))
		{
			Debug.LogError("Header is empty. We want to show dialog windows only with header!!!");
		}
	}

	// Token: 0x06004458 RID: 17496 RVA: 0x0014462A File Offset: 0x0014282A
	public UIDialogWindowData(string header, string information, UIDialogWindowData.ButtonData oneOption)
		: this(header, information, new List<UIDialogWindowData.ButtonData> { oneOption })
	{
		this.ShowCloseButton = true;
		this.CloseButtonAction = oneOption.onPressed;
	}

	// Token: 0x06004459 RID: 17497 RVA: 0x00144654 File Offset: 0x00142854
	public UIDialogWindowData(string header, string information, Action yesAction, Action noAction, bool replaceForGamepad = false)
		: this(header, information, new List<UIDialogWindowData.ButtonData>
		{
			new UIDialogWindowData.ButtonData(yesAction, LLBase.L("btn_yes"), null, true, GameKey.Select, ""),
			new UIDialogWindowData.ButtonData(noAction, LLBase.L("btn_no"), null, true, GameKey.Back, "")
		})
	{
	}

	// Token: 0x0600445A RID: 17498 RVA: 0x001446B3 File Offset: 0x001428B3
	public UIDialogWindowData(string header, string information, UIDialogWindowData.ButtonData firstOption, UIDialogWindowData.ButtonData secondOption)
		: this(header, information, new List<UIDialogWindowData.ButtonData> { firstOption, secondOption })
	{
	}

	// Token: 0x0600445B RID: 17499 RVA: 0x001446D1 File Offset: 0x001428D1
	public UIDialogWindowData(Item item, string header, string information, UIDialogWindowData.ButtonData firstOption, bool drawCounter = true)
		: this(header, information, new List<UIDialogWindowData.ButtonData> { firstOption })
	{
		this.Item = item;
		this.DrawItemCounter = drawCounter;
	}

	// Token: 0x0600445C RID: 17500 RVA: 0x001446F8 File Offset: 0x001428F8
	public UIDialogWindowData(Item item, string header, string information, Action yesAction, Action noAction, bool replaceForGamepad = false, bool drawCounter = true)
		: this(header, information, new List<UIDialogWindowData.ButtonData>
		{
			new UIDialogWindowData.ButtonData(yesAction, LLBase.L("btn_yes"), null, true, GameKey.Select, ""),
			new UIDialogWindowData.ButtonData(noAction, LLBase.L("btn_no"), null, true, GameKey.Back, "")
		})
	{
		this.Item = item;
		this.DrawItemCounter = drawCounter;
	}

	// Token: 0x0600445D RID: 17501 RVA: 0x00144768 File Offset: 0x00142968
	public UIDialogWindowData(Item item, string header, string information, string informationBot, int hasCount, int needCount, Action yesAction, Action noAction, bool replaceForGamepad = false)
		: this(header, information, new List<UIDialogWindowData.ButtonData>
		{
			new UIDialogWindowData.ButtonData(yesAction, LLBase.L("btn_ok"), null, true, GameKey.Select, ""),
			new UIDialogWindowData.ButtonData(noAction, LLBase.L("btn_cancel"), null, true, GameKey.Back, "")
		})
	{
		this.ShowAltVersion = true;
		this.ItemIconId = item.Definition.iconId;
		this.ItemName = LLBase.L(item.id);
		this.InformationBot = informationBot;
		this.HasItemCount = hasCount;
		this.NeedItemCount = needCount;
	}

	// Token: 0x0600445E RID: 17502 RVA: 0x0014480C File Offset: 0x00142A0C
	public UIDialogWindowData(Item item, string header, string information, string informationBot, int hasCount, int needCount, Action yesAction, bool replaceForGamepad = false)
		: this(header, information, new List<UIDialogWindowData.ButtonData>
		{
			new UIDialogWindowData.ButtonData(yesAction, LLBase.L("btn_ok"), null, true, GameKey.Select, "")
		})
	{
		this.ShowAltVersion = true;
		this.ItemIconId = item.Definition.iconId;
		this.ItemName = LLBase.L(item.id);
		this.InformationBot = informationBot;
		this.HasItemCount = hasCount;
		this.NeedItemCount = needCount;
	}

	// Token: 0x020009ED RID: 2541
	public class ButtonData
	{
		// Token: 0x0600445F RID: 17503 RVA: 0x0014488A File Offset: 0x00142A8A
		public ButtonData(Action onPressed, string text, Func<bool> buttonAvailableCondition = null, bool replaceForGamepad = true, GameKey keyToReplace = null, string textGamepad = "")
		{
			this.text = text;
			this.onPressed = onPressed;
			this.buttonAvailableCondition = buttonAvailableCondition;
			this.replaceForGamepad = replaceForGamepad;
			this.keyToReplace = keyToReplace;
			this.textGamepad = textGamepad;
		}

		// Token: 0x04003549 RID: 13641
		public Action onPressed;

		// Token: 0x0400354A RID: 13642
		public Func<bool> buttonAvailableCondition;

		// Token: 0x0400354B RID: 13643
		public string text;

		// Token: 0x0400354C RID: 13644
		public string textGamepad;

		// Token: 0x0400354D RID: 13645
		public bool replaceForGamepad;

		// Token: 0x0400354E RID: 13646
		public GameKey keyToReplace;
	}
}
