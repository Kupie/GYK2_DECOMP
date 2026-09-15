using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x0200001C RID: 28
public class LazyWindowInputController
{
	// Token: 0x1700000C RID: 12
	// (get) Token: 0x06000078 RID: 120 RVA: 0x00003F71 File Offset: 0x00002171
	// (set) Token: 0x06000079 RID: 121 RVA: 0x00003F79 File Offset: 0x00002179
	public List<GamepadNavigationItem> CustomNavigationItems
	{
		get
		{
			return this.customNavigationItems;
		}
		set
		{
			this.customNavigationItems = value;
		}
	}

	// Token: 0x0600007A RID: 122 RVA: 0x00003F82 File Offset: 0x00002182
	public LazyWindowInputController(GamepadNavigationController gamepadNavigationController, Dictionary<GameKey, Func<bool>> gameKeyDelegates, Func<GamepadNavigationItem> getFocusedAction)
	{
		this.gamepadNavigationController = gamepadNavigationController;
		this.getFocusedAction = getFocusedAction;
		this.InitKeys(gameKeyDelegates);
	}

	// Token: 0x0600007B RID: 123 RVA: 0x00003FA0 File Offset: 0x000021A0
	public void Enable(bool restoreFocused)
	{
		this.isActive = true;
		if (!LazyInput.IsGamepadActive)
		{
			return;
		}
		this.gamepadNavigationController.Enable();
		if (restoreFocused && this.gamepadNavigationController.HaveSavedFocusForGroup(this.rememberedGroup))
		{
			this.gamepadNavigationController.ReinitItems(false, null, null);
			this.gamepadNavigationController.RestoreFocus(this.rememberedGroup);
			return;
		}
		this.gamepadNavigationController.ReinitItems(true, null, null);
	}

	// Token: 0x0600007C RID: 124 RVA: 0x0000400C File Offset: 0x0000220C
	public void Disable(bool rememberFocused)
	{
		this.isActive = false;
		if (rememberFocused)
		{
			GamepadNavigationItem focusedItem = this.gamepadNavigationController.FocusedItem;
			if (focusedItem != null)
			{
				this.rememberedGroup = focusedItem.group;
				this.gamepadNavigationController.RememberFocused(focusedItem);
			}
		}
		this.gamepadNavigationController.Disable();
	}

	// Token: 0x0600007D RID: 125 RVA: 0x00004055 File Offset: 0x00002255
	public void Update()
	{
		if (!this.isActive)
		{
			return;
		}
		this.UpdatePressed();
	}

	// Token: 0x0600007E RID: 126 RVA: 0x00004068 File Offset: 0x00002268
	public void UpdateGamepadDependentStuff()
	{
		if (!LazyInput.IsGamepadActive)
		{
			this.gamepadNavigationController.Disable();
			return;
		}
		this.gamepadNavigationController.Enable();
		Func<GamepadNavigationItem> func = this.getFocusedAction;
		GamepadNavigationItem gamepadNavigationItem = ((func != null) ? func() : null);
		if (gamepadNavigationItem != null)
		{
			this.gamepadNavigationController.ReinitItems(false, this.CustomNavigationItems, null);
			this.gamepadNavigationController.SetFocusedItem(gamepadNavigationItem);
			return;
		}
		this.gamepadNavigationController.ReinitItems(true, this.CustomNavigationItems, null);
	}

	// Token: 0x0600007F RID: 127 RVA: 0x000040E2 File Offset: 0x000022E2
	public bool OnPressedLeft()
	{
		return this.HandlePressedAndNavigate(GameKey.Left, GUIDirection.Left);
	}

	// Token: 0x06000080 RID: 128 RVA: 0x000040F0 File Offset: 0x000022F0
	public bool OnPressedRight()
	{
		return this.HandlePressedAndNavigate(GameKey.Right, GUIDirection.Right);
	}

	// Token: 0x06000081 RID: 129 RVA: 0x000040FE File Offset: 0x000022FE
	public bool OnPressedUp()
	{
		return this.HandlePressedAndNavigate(GameKey.Up, GUIDirection.Up);
	}

	// Token: 0x06000082 RID: 130 RVA: 0x0000410C File Offset: 0x0000230C
	public bool OnPressedDown()
	{
		return this.HandlePressedAndNavigate(GameKey.Down, GUIDirection.Down);
	}

	// Token: 0x06000083 RID: 131 RVA: 0x0000411A File Offset: 0x0000231A
	public bool OnPressedLeftDpad()
	{
		return this.HandlePressedAndNavigate(GameKey.DpadLeft, GUIDirection.Left);
	}

	// Token: 0x06000084 RID: 132 RVA: 0x00004128 File Offset: 0x00002328
	public bool OnPressedRightDpad()
	{
		return this.HandlePressedAndNavigate(GameKey.DpadRight, GUIDirection.Right);
	}

	// Token: 0x06000085 RID: 133 RVA: 0x00004136 File Offset: 0x00002336
	public bool OnPressedUpDpad()
	{
		return this.HandlePressedAndNavigate(GameKey.DpadUp, GUIDirection.Up);
	}

	// Token: 0x06000086 RID: 134 RVA: 0x00004144 File Offset: 0x00002344
	public bool OnPressedDownDpad()
	{
		return this.HandlePressedAndNavigate(GameKey.DpadDown, GUIDirection.Down);
	}

	// Token: 0x06000087 RID: 135 RVA: 0x00004152 File Offset: 0x00002352
	public bool OnPressedSelect()
	{
		this.gamepadNavigationController.SelectFocusedItem();
		return true;
	}

	// Token: 0x06000088 RID: 136 RVA: 0x00004160 File Offset: 0x00002360
	private bool HandlePressedAndNavigate(GameKey gameKey, GUIDirection direction)
	{
		if (this.gamepadNavigationController.ignoreHoldedKeys)
		{
			LazyInput.WaitForRelease(gameKey);
		}
		this.gamepadNavigationController.Navigate(direction);
		return true;
	}

	// Token: 0x06000089 RID: 137 RVA: 0x00004184 File Offset: 0x00002384
	private void UpdatePressed()
	{
		foreach (KeyValuePair<GameKey, Func<bool>> keyValuePair in this.gameKeyDelegates)
		{
			if (!this.isActive)
			{
				break;
			}
			if (LazyInput.GetKeyDown(keyValuePair.Key) && keyValuePair.Value())
			{
				LazyInput.ClearKeyDown(keyValuePair.Key);
			}
		}
	}

	// Token: 0x0600008A RID: 138 RVA: 0x00004204 File Offset: 0x00002404
	private void InitKeys(Dictionary<GameKey, Func<bool>> gameKeyDelegates)
	{
		this.gameKeyDelegates = gameKeyDelegates;
		this.gameKeyDelegates.TryAdd(GameKey.Select, new Func<bool>(this.OnPressedSelect));
		this.gameKeyDelegates.TryAdd(GameKey.Left, new Func<bool>(this.OnPressedLeft));
		this.gameKeyDelegates.TryAdd(GameKey.Right, new Func<bool>(this.OnPressedRight));
		this.gameKeyDelegates.TryAdd(GameKey.Up, new Func<bool>(this.OnPressedUp));
		this.gameKeyDelegates.TryAdd(GameKey.Down, new Func<bool>(this.OnPressedDown));
		this.gameKeyDelegates.TryAdd(GameKey.DpadLeft, new Func<bool>(this.OnPressedLeftDpad));
		this.gameKeyDelegates.TryAdd(GameKey.DpadRight, new Func<bool>(this.OnPressedRightDpad));
		this.gameKeyDelegates.TryAdd(GameKey.DpadUp, new Func<bool>(this.OnPressedUpDpad));
		this.gameKeyDelegates.TryAdd(GameKey.DpadDown, new Func<bool>(this.OnPressedDownDpad));
	}

	// Token: 0x0400005D RID: 93
	private readonly GamepadNavigationController gamepadNavigationController;

	// Token: 0x0400005E RID: 94
	private List<GamepadNavigationItem> customNavigationItems;

	// Token: 0x0400005F RID: 95
	private Dictionary<GameKey, Func<bool>> gameKeyDelegates;

	// Token: 0x04000060 RID: 96
	private bool isActive;

	// Token: 0x04000061 RID: 97
	private int rememberedGroup;

	// Token: 0x04000062 RID: 98
	private Func<GamepadNavigationItem> getFocusedAction;
}
