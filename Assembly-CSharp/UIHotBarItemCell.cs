using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;

// Token: 0x02000868 RID: 2152
public class UIHotBarItemCell : MonoBehaviour
{
	// Token: 0x1700082E RID: 2094
	// (get) Token: 0x0600370D RID: 14093 RVA: 0x0010A2DB File Offset: 0x001084DB
	public UIItemCell UIItemCell
	{
		get
		{
			return this.uiItemCell;
		}
	}

	// Token: 0x0600370E RID: 14094 RVA: 0x0010A2E3 File Offset: 0x001084E3
	public void Init(int index)
	{
		this.index = index;
	}

	// Token: 0x0600370F RID: 14095 RVA: 0x0010A2EC File Offset: 0x001084EC
	public void Draw(Item item, bool isUsable, Action<UIItemCell> onItemCellPress)
	{
		this.uiItemCell.Draw(item, false, -1, false, 1, false, 0, true, true, true, ItemRelatedWidgetState.NotSet, false);
		if (isUsable)
		{
			UIItemCell uiitemCell = this.uiItemCell;
			uiitemCell.OnItemCellPress = (Action<UIItemCell>)Delegate.Combine(uiitemCell.OnItemCellPress, onItemCellPress);
		}
		this.UpdateBtnText();
	}

	// Token: 0x06003710 RID: 14096 RVA: 0x0010A338 File Offset: 0x00108538
	public void DrawNonInteractable(Item item)
	{
		this.uiItemCell.Draw(item, false, -1, false, 1, true, 0, true, true, true, ItemRelatedWidgetState.NotSet, false);
		this.UpdateBtnText();
	}

	// Token: 0x06003711 RID: 14097 RVA: 0x0010A362 File Offset: 0x00108562
	public void Set(Item item)
	{
		MainGame.PlayerData.SetHotBarItemAtIndex(item.id, this.index);
	}

	// Token: 0x06003712 RID: 14098 RVA: 0x0010A37C File Offset: 0x0010857C
	public void UpdateBtnText()
	{
		if (this.keyLabel == null)
		{
			return;
		}
		int gameKeyValue = 181;
		switch (this.index)
		{
		case 0:
			gameKeyValue = GameKey.UseHotBarItem1.value;
			break;
		case 1:
			gameKeyValue = GameKey.UseHotBarItem2.value;
			break;
		case 2:
			gameKeyValue = GameKey.UseHotBarItem3.value;
			break;
		case 3:
			gameKeyValue = GameKey.UseHotBarItem4.value;
			break;
		}
		string keycodeString = LazyInput.ControllerIconLibrary.GetKeycodeString(LazyInput.GameBindings.keyBindings.Find((KeyBinding b) => b.gameKey.value == gameKeyValue).keyCode);
		string text;
		if (this.gameKeyNumberStyle != null)
		{
			text = this.gameKeyNumberStyle.ApplyStyleToString(keycodeString, false, true) ?? "";
		}
		else
		{
			text = keycodeString ?? "";
		}
		this.keyLabel.text = text;
		this.gameKeyStyle.ApplyStyle(this.keyLabel, false, null, null, null);
	}

	// Token: 0x04002BEE RID: 11246
	[SerializeField]
	private UIItemCell uiItemCell;

	// Token: 0x04002BEF RID: 11247
	[SerializeField]
	private TextStyle gameKeyStyle;

	// Token: 0x04002BF0 RID: 11248
	[SerializeField]
	private TextStyle gameKeyNumberStyle;

	// Token: 0x04002BF1 RID: 11249
	[SerializeField]
	private TextMeshProUGUI keyLabel;

	// Token: 0x04002BF2 RID: 11250
	private int index;
}
