using System;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x02000113 RID: 275
	[AddComponentMenu("")]
	public class TouchControls1_ManipulateCube : MonoBehaviour
	{
		// Token: 0x06000D0B RID: 3339 RVA: 0x000262D4 File Offset: 0x000244D4
		private void OnEnable()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			Player player = ReInput.players.GetPlayer(0);
			if (player == null)
			{
				return;
			}
			player.AddInputEventDelegate(new Action<InputActionEventData>(this.OnMoveReceivedX), UpdateLoopType.Update, InputActionEventType.AxisActive, "Horizontal");
			player.AddInputEventDelegate(new Action<InputActionEventData>(this.OnMoveReceivedX), UpdateLoopType.Update, InputActionEventType.AxisInactive, "Horizontal");
			player.AddInputEventDelegate(new Action<InputActionEventData>(this.OnMoveReceivedY), UpdateLoopType.Update, InputActionEventType.AxisActive, "Vertical");
			player.AddInputEventDelegate(new Action<InputActionEventData>(this.OnMoveReceivedY), UpdateLoopType.Update, InputActionEventType.AxisInactive, "Vertical");
			player.AddInputEventDelegate(new Action<InputActionEventData>(this.OnCycleColor), UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "CycleColor");
			player.AddInputEventDelegate(new Action<InputActionEventData>(this.OnCycleColorReverse), UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "CycleColorReverse");
			player.AddInputEventDelegate(new Action<InputActionEventData>(this.OnRotationReceivedX), UpdateLoopType.Update, InputActionEventType.AxisActive, "RotateHorizontal");
			player.AddInputEventDelegate(new Action<InputActionEventData>(this.OnRotationReceivedX), UpdateLoopType.Update, InputActionEventType.AxisInactive, "RotateHorizontal");
			player.AddInputEventDelegate(new Action<InputActionEventData>(this.OnRotationReceivedY), UpdateLoopType.Update, InputActionEventType.AxisActive, "RotateVertical");
			player.AddInputEventDelegate(new Action<InputActionEventData>(this.OnRotationReceivedY), UpdateLoopType.Update, InputActionEventType.AxisInactive, "RotateVertical");
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x000263FC File Offset: 0x000245FC
		private void OnDisable()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			Player player = ReInput.players.GetPlayer(0);
			if (player == null)
			{
				return;
			}
			player.RemoveInputEventDelegate(new Action<InputActionEventData>(this.OnMoveReceivedX));
			player.RemoveInputEventDelegate(new Action<InputActionEventData>(this.OnMoveReceivedY));
			player.RemoveInputEventDelegate(new Action<InputActionEventData>(this.OnCycleColor));
			player.RemoveInputEventDelegate(new Action<InputActionEventData>(this.OnCycleColorReverse));
			player.RemoveInputEventDelegate(new Action<InputActionEventData>(this.OnRotationReceivedX));
			player.RemoveInputEventDelegate(new Action<InputActionEventData>(this.OnRotationReceivedY));
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x0002648D File Offset: 0x0002468D
		private void OnMoveReceivedX(InputActionEventData data)
		{
			this.OnMoveReceived(new Vector2(data.GetAxis(), 0f));
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x000264A6 File Offset: 0x000246A6
		private void OnMoveReceivedY(InputActionEventData data)
		{
			this.OnMoveReceived(new Vector2(0f, data.GetAxis()));
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x000264BF File Offset: 0x000246BF
		private void OnRotationReceivedX(InputActionEventData data)
		{
			this.OnRotationReceived(new Vector2(data.GetAxis(), 0f));
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x000264D8 File Offset: 0x000246D8
		private void OnRotationReceivedY(InputActionEventData data)
		{
			this.OnRotationReceived(new Vector2(0f, data.GetAxis()));
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x000264F1 File Offset: 0x000246F1
		private void OnCycleColor(InputActionEventData data)
		{
			this.OnCycleColor();
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x000264F9 File Offset: 0x000246F9
		private void OnCycleColorReverse(InputActionEventData data)
		{
			this.OnCycleColorReverse();
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x00026501 File Offset: 0x00024701
		private void OnMoveReceived(Vector2 move)
		{
			base.transform.Translate(move * Time.deltaTime * this.moveSpeed, Space.World);
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x0002652A File Offset: 0x0002472A
		private void OnRotationReceived(Vector2 rotate)
		{
			rotate *= this.rotateSpeed;
			base.transform.Rotate(Vector3.up, -rotate.x, Space.World);
			base.transform.Rotate(Vector3.right, rotate.y, Space.World);
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x0002656C File Offset: 0x0002476C
		private void OnCycleColor()
		{
			if (this.colors.Length == 0)
			{
				return;
			}
			this.currentColorIndex++;
			if (this.currentColorIndex >= this.colors.Length)
			{
				this.currentColorIndex = 0;
			}
			base.GetComponent<Renderer>().material.color = this.colors[this.currentColorIndex];
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x000265CC File Offset: 0x000247CC
		private void OnCycleColorReverse()
		{
			if (this.colors.Length == 0)
			{
				return;
			}
			this.currentColorIndex--;
			if (this.currentColorIndex < 0)
			{
				this.currentColorIndex = this.colors.Length - 1;
			}
			base.GetComponent<Renderer>().material.color = this.colors[this.currentColorIndex];
		}

		// Token: 0x040006D2 RID: 1746
		public float rotateSpeed = 1f;

		// Token: 0x040006D3 RID: 1747
		public float moveSpeed = 1f;

		// Token: 0x040006D4 RID: 1748
		private int currentColorIndex;

		// Token: 0x040006D5 RID: 1749
		private Color[] colors = new Color[]
		{
			Color.white,
			Color.red,
			Color.green,
			Color.blue
		};
	}
}
