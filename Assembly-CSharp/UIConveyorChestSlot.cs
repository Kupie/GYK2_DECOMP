using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A4B RID: 2635
public class UIConveyorChestSlot : MonoBehaviour
{
	// Token: 0x17000ACB RID: 2763
	// (get) Token: 0x06004709 RID: 18185 RVA: 0x0014FE57 File Offset: 0x0014E057
	public UIItemCell ItemCell
	{
		get
		{
			return this.itemCell;
		}
	}

	// Token: 0x17000ACC RID: 2764
	// (get) Token: 0x0600470A RID: 18186 RVA: 0x0014FE5F File Offset: 0x0014E05F
	public Direction ChestPosDirection
	{
		get
		{
			return this.chestPosDirection;
		}
	}

	// Token: 0x17000ACD RID: 2765
	// (get) Token: 0x0600470B RID: 18187 RVA: 0x0014FE67 File Offset: 0x0014E067
	public int SlotIndex
	{
		get
		{
			return this.slotIndex;
		}
	}

	// Token: 0x17000ACE RID: 2766
	// (get) Token: 0x0600470C RID: 18188 RVA: 0x0014FE6F File Offset: 0x0014E06F
	public UIConveyorChestSlot.State PreviousState
	{
		get
		{
			return this.previousState;
		}
	}

	// Token: 0x17000ACF RID: 2767
	// (get) Token: 0x0600470D RID: 18189 RVA: 0x0014FE77 File Offset: 0x0014E077
	public ConveyorChestSlotData SlotData
	{
		get
		{
			return this.slotData;
		}
	}

	// Token: 0x0600470E RID: 18190 RVA: 0x0014FE80 File Offset: 0x0014E080
	public void Draw(ConveyorChestSlotData slotData, ConveyorChestComponent chestComponent, Action<UIConveyorChestSlot> onPress, Action<UIConveyorChestSlot> onPress2)
	{
		this.onPress = onPress;
		this.onPress2 = onPress2;
		this.slotData = slotData;
		this.isParent = slotData.ConveyorWgoData != null && chestComponent.ParentsData.ContainsValue(slotData.ConveyorWgoData);
		this.isChild = slotData.ConveyorWgoData != null && chestComponent.ConnectedWgoData.Contains(slotData.ConveyorWgoData);
		this.previousState = this.state;
		if (!this.isParent && !this.isChild)
		{
			this.state = UIConveyorChestSlot.State.NotConnected;
		}
		else if (this.isParent)
		{
			this.state = UIConveyorChestSlot.State.EmptyIn;
		}
		else if (string.IsNullOrEmpty(slotData.slotItemId))
		{
			this.state = UIConveyorChestSlot.State.EmptyOut;
		}
		else
		{
			this.state = UIConveyorChestSlot.State.FilledOut;
		}
		this.DrawCurrentState();
		this.ItemCell.OnItemCellPress = new Action<UIItemCell>(this.OnCellPressed);
		this.ItemCell.OnItemCellPress2 = new Action<UIItemCell>(this.OnCellPressed2);
	}

	// Token: 0x0600470F RID: 18191 RVA: 0x0014FF70 File Offset: 0x0014E170
	public void Draw(ConveyorChestSlotData slotData, ConveyorChestOutComponent chestOutComponent, Action<UIConveyorChestSlot> onPress, Action<UIConveyorChestSlot> onPress2)
	{
		this.onPress = onPress;
		this.onPress2 = onPress2;
		this.slotData = slotData;
		this.isChild = slotData.ConveyorWgoData != null && chestOutComponent.ConnectedWgoData.Contains(slotData.ConveyorWgoData);
		this.previousState = this.state;
		if (!this.isChild)
		{
			this.state = UIConveyorChestSlot.State.NotConnected;
		}
		else if (string.IsNullOrEmpty(slotData.slotItemId))
		{
			this.state = UIConveyorChestSlot.State.EmptyOut;
		}
		else
		{
			this.state = UIConveyorChestSlot.State.FilledOut;
		}
		this.DrawCurrentState();
		this.ItemCell.OnItemCellPress = new Action<UIItemCell>(this.OnCellPressed);
		this.ItemCell.OnItemCellPress2 = new Action<UIItemCell>(this.OnCellPressed2);
	}

	// Token: 0x06004710 RID: 18192 RVA: 0x00150023 File Offset: 0x0014E223
	public void SetState(UIConveyorChestSlot.State state)
	{
		this.previousState = this.state;
		this.state = state;
		this.DrawCurrentState();
	}

	// Token: 0x06004711 RID: 18193 RVA: 0x00150040 File Offset: 0x0014E240
	private void DrawCurrentState()
	{
		Canvas canvas = null;
		bool flag = false;
		UIConveyorChestWindow componentInParent = base.GetComponentInParent<UIConveyorChestWindow>(true);
		if (componentInParent != null)
		{
			canvas = componentInParent.Canvas;
			flag = componentInParent.IsItemSelectionModActive;
		}
		else
		{
			UIConveyorVegetablesChestWindow componentInParent2 = base.GetComponentInParent<UIConveyorVegetablesChestWindow>(true);
			if (componentInParent2 != null)
			{
				canvas = componentInParent2.Canvas;
				flag = componentInParent2.IsItemSelectionModActive;
			}
			else
			{
				UIConveyorWineChestWindow componentInParent3 = base.GetComponentInParent<UIConveyorWineChestWindow>(true);
				if (componentInParent3 != null)
				{
					canvas = componentInParent3.Canvas;
					flag = componentInParent3.IsItemSelectionModActive;
				}
			}
		}
		this.questionMark.SetActive(false);
		this.backDirection.SetActive(false);
		this.directionImage.gameObject.SetActive(false);
		switch (this.state)
		{
		case UIConveyorChestSlot.State.EmptyIn:
			this.itemCell.DrawEmpty(true, true, false);
			this.itemCell.Background.sprite = this.itemCellBackEmptyIn;
			this.itemCell.NoSelectionFrames = true;
			this.backDirection.SetActive(true);
			this.directionImage.gameObject.SetActive(true);
			this.directionImage.sprite = this.directionInSprite;
			break;
		case UIConveyorChestSlot.State.EmptyOut:
			this.itemCell.DrawEmpty(false, true, false);
			this.itemCell.Background.sprite = this.itemCellBackOut;
			this.itemCell.NoSelectionFrames = false;
			this.questionMark.SetActive(true);
			this.backDirection.SetActive(true);
			this.directionImage.gameObject.SetActive(true);
			this.directionImage.sprite = this.directionOutSprite;
			break;
		case UIConveyorChestSlot.State.NotConnected:
			this.itemCell.DrawEmpty(true, true, false);
			this.itemCell.NoSelectionFrames = true;
			this.itemCell.Background.sprite = this.itemCellBackNotConnected;
			break;
		case UIConveyorChestSlot.State.FilledOut:
			this.itemCell.Draw(new Item(this.slotData.slotItemId, 1), false, -1, false, 1, false, 0, true, false, false, ItemRelatedWidgetState.NotSet, false);
			this.itemCell.NoSelectionFrames = false;
			this.itemCell.Background.sprite = this.itemCellBackOut;
			this.backDirection.SetActive(true);
			this.directionImage.gameObject.SetActive(true);
			this.directionImage.sprite = this.directionOutSprite;
			break;
		case UIConveyorChestSlot.State.OutDuringSelection:
			if (string.IsNullOrEmpty(this.slotData.slotItemId))
			{
				this.itemCell.DrawEmpty(false, true, false);
			}
			else
			{
				this.itemCell.Draw(new Item(this.slotData.slotItemId, 1), false, -1, false, 1, false, 0, true, false, false, ItemRelatedWidgetState.NotSet, false);
			}
			this.itemCell.Background.sprite = this.itemCellBackOut;
			this.backDirection.SetActive(true);
			this.directionImage.gameObject.SetActive(true);
			this.itemCell.NoSelectionFrames = true;
			this.directionImage.sprite = this.directionOutSprite;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		if (this.state != UIConveyorChestSlot.State.OutDuringSelection)
		{
			if (this.hasCanvases)
			{
				global::UnityEngine.Object.Destroy(this.itemCell.Background.GetComponent<Canvas>());
				global::UnityEngine.Object.Destroy(this.itemCell.Icon.GetComponent<Canvas>());
				this.hasCanvases = false;
			}
		}
		else if (!this.hasCanvases)
		{
			Canvas canvas2 = this.itemCell.Background.gameObject.AddComponent<Canvas>();
			canvas2.overrideSorting = true;
			canvas2.sortingOrder = ((canvas != null) ? (canvas.sortingOrder + 5) : 5);
			Canvas canvas3 = this.itemCell.Icon.gameObject.AddComponent<Canvas>();
			canvas3.overrideSorting = true;
			canvas3.sortingOrder = ((canvas != null) ? (canvas.sortingOrder + 6) : 6);
			this.hasCanvases = true;
		}
		this.itemCell.GamepadNavigationItem.Active = !flag;
		this.UpdateConnectionTooltip();
	}

	// Token: 0x06004712 RID: 18194 RVA: 0x001503F4 File Offset: 0x0014E5F4
	private void UpdateConnectionTooltip()
	{
		bool flag = this.itemCell.DisplayingItem != null && !this.itemCell.DisplayingItem.IsEmpty;
		this.itemCell.CustomTooltipShowAction = (flag ? null : new Action<UIItemCell>(UIConveyorChestSlot.ShowConnectionTooltip));
	}

	// Token: 0x06004713 RID: 18195 RVA: 0x00150444 File Offset: 0x0014E644
	private static void ShowConnectionTooltip(UIItemCell cell)
	{
		UIMouseTooltip.TryShow(cell.transform as RectTransform, "tt_conv_chest_conn", default(Vector2), null);
	}

	// Token: 0x06004714 RID: 18196 RVA: 0x00150474 File Offset: 0x0014E674
	private void OnCellPressed(UIItemCell cell)
	{
		switch (this.state)
		{
		case UIConveyorChestSlot.State.EmptyIn:
		case UIConveyorChestSlot.State.NotConnected:
		case UIConveyorChestSlot.State.OutDuringSelection:
			return;
		case UIConveyorChestSlot.State.EmptyOut:
		case UIConveyorChestSlot.State.FilledOut:
			this.onPress(this);
			this.state = UIConveyorChestSlot.State.OutDuringSelection;
			return;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x06004715 RID: 18197 RVA: 0x001504C0 File Offset: 0x0014E6C0
	private void OnCellPressed2(UIItemCell cell)
	{
		switch (this.state)
		{
		case UIConveyorChestSlot.State.EmptyIn:
		case UIConveyorChestSlot.State.EmptyOut:
		case UIConveyorChestSlot.State.NotConnected:
		case UIConveyorChestSlot.State.OutDuringSelection:
			return;
		case UIConveyorChestSlot.State.FilledOut:
			this.onPress2(this);
			this.state = UIConveyorChestSlot.State.OutDuringSelection;
			return;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x04003765 RID: 14181
	[SerializeField]
	private int slotIndex = -1;

	// Token: 0x04003766 RID: 14182
	[SerializeField]
	private UIItemCell itemCell;

	// Token: 0x04003767 RID: 14183
	[SerializeField]
	private Sprite itemCellBackEmptyIn;

	// Token: 0x04003768 RID: 14184
	[SerializeField]
	private Sprite itemCellBackNotConnected;

	// Token: 0x04003769 RID: 14185
	[SerializeField]
	private Sprite itemCellBackOut;

	// Token: 0x0400376A RID: 14186
	[SerializeField]
	private Image directionImage;

	// Token: 0x0400376B RID: 14187
	[SerializeField]
	private GameObject questionMark;

	// Token: 0x0400376C RID: 14188
	[SerializeField]
	private GameObject backDirection;

	// Token: 0x0400376D RID: 14189
	[SerializeField]
	private Sprite directionInSprite;

	// Token: 0x0400376E RID: 14190
	[SerializeField]
	private Sprite directionOutSprite;

	// Token: 0x0400376F RID: 14191
	[SerializeField]
	private Direction chestPosDirection;

	// Token: 0x04003770 RID: 14192
	private UIConveyorChestSlot.State previousState;

	// Token: 0x04003771 RID: 14193
	private UIConveyorChestSlot.State state;

	// Token: 0x04003772 RID: 14194
	private Action<UIConveyorChestSlot> onPress;

	// Token: 0x04003773 RID: 14195
	private Action<UIConveyorChestSlot> onPress2;

	// Token: 0x04003774 RID: 14196
	private ConveyorChestSlotData slotData;

	// Token: 0x04003775 RID: 14197
	private bool isParent;

	// Token: 0x04003776 RID: 14198
	private bool isChild;

	// Token: 0x04003777 RID: 14199
	private bool hasCanvases;

	// Token: 0x02000A4C RID: 2636
	public enum State
	{
		// Token: 0x04003779 RID: 14201
		EmptyIn,
		// Token: 0x0400377A RID: 14202
		EmptyOut,
		// Token: 0x0400377B RID: 14203
		NotConnected,
		// Token: 0x0400377C RID: 14204
		FilledOut,
		// Token: 0x0400377D RID: 14205
		OutDuringSelection
	}
}
