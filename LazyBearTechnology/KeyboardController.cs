using System;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000128 RID: 296
	public class KeyboardController : BaseInputController
	{
		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060005EB RID: 1515 RVA: 0x0001E2F8 File Offset: 0x0001C4F8
		// (set) Token: 0x060005EC RID: 1516 RVA: 0x0001E300 File Offset: 0x0001C500
		public KeyboardController.MovementType CurrentMovementType
		{
			get
			{
				return this.currentMovementType;
			}
			set
			{
				this.currentMovementType = value;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060005ED RID: 1517 RVA: 0x0001E309 File Offset: 0x0001C509
		public float MouseScrollDelta
		{
			get
			{
				return this.mouseScrollDelta;
			}
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x0001E311 File Offset: 0x0001C511
		public KeyboardController(GameBindings gameBindings)
		{
			this.bindings = gameBindings.keyBindings;
			this.rewiredPlayer = ReInput.players.GetPlayer(0);
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x0001E338 File Offset: 0x0001C538
		public override void Update()
		{
			base.Update();
			if (!ReInput.isReady)
			{
				return;
			}
			this.rewiredPlayer = ReInput.players.GetPlayer(0);
			for (int i = 0; i < this.bindings.Count; i++)
			{
				GameKey gameKey = this.bindings[i].gameKey;
				KeyCode keyCode = this.bindings[i].keyCode;
				KeyCode[] additionalKeyCodes = this.bindings[i].additionalKeyCodes;
				if (Input.GetKey(keyCode) && !this.holdedKeys.Contains(gameKey))
				{
					this.holdedKeys.Add(gameKey);
				}
				if (Input.GetKeyDown(keyCode) && !this.pressedKeys.Contains(gameKey))
				{
					if (additionalKeyCodes.Length != 0)
					{
						bool flag = true;
						for (int j = 0; j < additionalKeyCodes.Length; j++)
						{
							if (!Input.GetKey(additionalKeyCodes[j]))
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							this.pressedKeys.Add(gameKey);
						}
					}
					else
					{
						this.pressedKeys.Add(gameKey);
					}
				}
			}
			this.HandleMouseClicks();
			this.HandleMouseScroll();
			this.HandleDirection();
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x0001E442 File Offset: 0x0001C642
		public void EraseMouseControl()
		{
			this.mouseScrollDelta = 0f;
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x0001E450 File Offset: 0x0001C650
		private void HandleMouseClicks()
		{
			if (Input.GetMouseButtonDown(0))
			{
				if (Time.time - this.lastClickTime <= 0.5f)
				{
					this.pressedKeys.Add(GameKey.DoubleClick);
					this.lastClickTime = 0f;
					return;
				}
				this.lastClickTime = Time.time;
			}
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x0001E49F File Offset: 0x0001C69F
		private void HandleMouseScroll()
		{
			this.mouseScrollDelta = -this.rewiredPlayer.GetAxis(18);
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x0001E4B8 File Offset: 0x0001C6B8
		private void HandleDirection()
		{
			this.direction = Vector2.zero;
			KeyboardController.MovementType movementType = this.currentMovementType;
			if (movementType != KeyboardController.MovementType.MouseOnly)
			{
				if (movementType != KeyboardController.MovementType.MouseAndKeyboard)
				{
					if (this.holdedKeys.Contains(GameKey.Left))
					{
						this.direction.x = -1f;
					}
					if (this.holdedKeys.Contains(GameKey.Right))
					{
						this.direction.x = 1f;
					}
					if (this.holdedKeys.Contains(GameKey.Up))
					{
						this.direction.y = 1f;
					}
					if (this.holdedKeys.Contains(GameKey.Down))
					{
						this.direction.y = -1f;
					}
				}
				else if (this.holdedKeys.Contains(GameKey.Up) || this.holdedKeys.Contains(GameKey.Down) || this.holdedKeys.Contains(GameKey.Left) || this.holdedKeys.Contains(GameKey.Right))
				{
					this.CalcDirectionFromMousePosition();
					return;
				}
			}
			else if (this.holdedKeys.Contains(GameKey.LeftClick))
			{
				this.CalcDirectionFromMousePosition();
				return;
			}
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x0001E5D8 File Offset: 0x0001C7D8
		private void CalcDirectionFromMousePosition()
		{
			Vector3 mousePosition = Input.mousePosition;
			mousePosition.x -= (float)(Screen.width / 2);
			mousePosition.y -= (float)(Screen.height / 2);
			if (Mathf.Sqrt(Mathf.Pow(mousePosition.x, 2f) + Mathf.Pow(mousePosition.y, 2f)) > 50f)
			{
				if ((double)mousePosition.x < 0.5 * (double)mousePosition.y && (double)mousePosition.x < -0.5 * (double)mousePosition.y)
				{
					this.direction.x = -1f;
				}
				if ((double)mousePosition.x >= 0.5 * (double)mousePosition.y && (double)mousePosition.x >= -0.5 * (double)mousePosition.y)
				{
					this.direction.x = 1f;
				}
				if ((double)mousePosition.y >= 0.5 * (double)mousePosition.x && (double)mousePosition.y >= -0.5 * (double)mousePosition.x)
				{
					this.direction.y = 1f;
				}
				if ((double)mousePosition.y < 0.5 * (double)mousePosition.x && (double)mousePosition.y < -0.5 * (double)mousePosition.x)
				{
					this.direction.y = -1f;
				}
			}
		}

		// Token: 0x040002F3 RID: 755
		public const float DEAD_ZONE_RADIUS = 50f;

		// Token: 0x040002F4 RID: 756
		private const float DOUBLE_CLICK_DELAY = 0.5f;

		// Token: 0x040002F5 RID: 757
		private float lastClickTime;

		// Token: 0x040002F6 RID: 758
		private List<KeyBinding> bindings;

		// Token: 0x040002F7 RID: 759
		private KeyboardController.MovementType currentMovementType;

		// Token: 0x040002F8 RID: 760
		private float mouseScrollDelta;

		// Token: 0x040002F9 RID: 761
		private Player rewiredPlayer;

		// Token: 0x020001EB RID: 491
		public enum MovementType
		{
			// Token: 0x0400066B RID: 1643
			Default,
			// Token: 0x0400066C RID: 1644
			MouseOnly,
			// Token: 0x0400066D RID: 1645
			MouseAndKeyboard
		}
	}
}
