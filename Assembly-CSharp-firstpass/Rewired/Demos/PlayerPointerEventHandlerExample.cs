using System;
using System.Collections.Generic;
using System.Text;
using Rewired.Integration.UnityUI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.Demos
{
	// Token: 0x02000100 RID: 256
	[AddComponentMenu("")]
	public sealed class PlayerPointerEventHandlerExample : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler, IScrollHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		// Token: 0x06000C9E RID: 3230 RVA: 0x0002451E File Offset: 0x0002271E
		private void Log(string o)
		{
			this.log.Add(o);
			if (this.log.Count > 10)
			{
				this.log.RemoveAt(0);
			}
		}

		// Token: 0x06000C9F RID: 3231 RVA: 0x00024548 File Offset: 0x00022748
		private void Update()
		{
			if (this.text != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (string text in this.log)
				{
					stringBuilder.AppendLine(text);
				}
				this.text.text = stringBuilder.ToString();
			}
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x000245C4 File Offset: 0x000227C4
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (eventData is PlayerPointerEventData)
			{
				PlayerPointerEventData playerPointerEventData = (PlayerPointerEventData)eventData;
				this.Log(string.Concat(new string[]
				{
					"OnPointerEnter:  Player = ",
					playerPointerEventData.playerId.ToString(),
					", Pointer Index = ",
					playerPointerEventData.inputSourceIndex.ToString(),
					", Source = ",
					PlayerPointerEventHandlerExample.GetSourceName(playerPointerEventData)
				}));
			}
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x00024634 File Offset: 0x00022834
		public void OnPointerExit(PointerEventData eventData)
		{
			if (eventData is PlayerPointerEventData)
			{
				PlayerPointerEventData playerPointerEventData = (PlayerPointerEventData)eventData;
				this.Log(string.Concat(new string[]
				{
					"OnPointerExit:  Player = ",
					playerPointerEventData.playerId.ToString(),
					", Pointer Index = ",
					playerPointerEventData.inputSourceIndex.ToString(),
					", Source = ",
					PlayerPointerEventHandlerExample.GetSourceName(playerPointerEventData)
				}));
			}
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x000246A4 File Offset: 0x000228A4
		public void OnPointerUp(PointerEventData eventData)
		{
			if (eventData is PlayerPointerEventData)
			{
				PlayerPointerEventData playerPointerEventData = (PlayerPointerEventData)eventData;
				this.Log(string.Concat(new string[]
				{
					"OnPointerUp:  Player = ",
					playerPointerEventData.playerId.ToString(),
					", Pointer Index = ",
					playerPointerEventData.inputSourceIndex.ToString(),
					", Source = ",
					PlayerPointerEventHandlerExample.GetSourceName(playerPointerEventData),
					", Button Index = ",
					playerPointerEventData.buttonIndex.ToString()
				}));
			}
		}

		// Token: 0x06000CA3 RID: 3235 RVA: 0x00024730 File Offset: 0x00022930
		public void OnPointerDown(PointerEventData eventData)
		{
			if (eventData is PlayerPointerEventData)
			{
				PlayerPointerEventData playerPointerEventData = (PlayerPointerEventData)eventData;
				this.Log(string.Concat(new string[]
				{
					"OnPointerDown:  Player = ",
					playerPointerEventData.playerId.ToString(),
					", Pointer Index = ",
					playerPointerEventData.inputSourceIndex.ToString(),
					", Source = ",
					PlayerPointerEventHandlerExample.GetSourceName(playerPointerEventData),
					", Button Index = ",
					playerPointerEventData.buttonIndex.ToString()
				}));
			}
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x000247BC File Offset: 0x000229BC
		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData is PlayerPointerEventData)
			{
				PlayerPointerEventData playerPointerEventData = (PlayerPointerEventData)eventData;
				this.Log(string.Concat(new string[]
				{
					"OnPointerClick:  Player = ",
					playerPointerEventData.playerId.ToString(),
					", Pointer Index = ",
					playerPointerEventData.inputSourceIndex.ToString(),
					", Source = ",
					PlayerPointerEventHandlerExample.GetSourceName(playerPointerEventData),
					", Button Index = ",
					playerPointerEventData.buttonIndex.ToString()
				}));
			}
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x00024848 File Offset: 0x00022A48
		public void OnScroll(PointerEventData eventData)
		{
			if (eventData is PlayerPointerEventData)
			{
				PlayerPointerEventData playerPointerEventData = (PlayerPointerEventData)eventData;
				this.Log(string.Concat(new string[]
				{
					"OnScroll:  Player = ",
					playerPointerEventData.playerId.ToString(),
					", Pointer Index = ",
					playerPointerEventData.inputSourceIndex.ToString(),
					", Source = ",
					PlayerPointerEventHandlerExample.GetSourceName(playerPointerEventData)
				}));
			}
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x000248B8 File Offset: 0x00022AB8
		public void OnBeginDrag(PointerEventData eventData)
		{
			if (eventData is PlayerPointerEventData)
			{
				PlayerPointerEventData playerPointerEventData = (PlayerPointerEventData)eventData;
				this.Log(string.Concat(new string[]
				{
					"OnBeginDrag:  Player = ",
					playerPointerEventData.playerId.ToString(),
					", Pointer Index = ",
					playerPointerEventData.inputSourceIndex.ToString(),
					", Source = ",
					PlayerPointerEventHandlerExample.GetSourceName(playerPointerEventData),
					", Button Index = ",
					playerPointerEventData.buttonIndex.ToString()
				}));
			}
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x00024944 File Offset: 0x00022B44
		public void OnDrag(PointerEventData eventData)
		{
			if (eventData is PlayerPointerEventData)
			{
				PlayerPointerEventData playerPointerEventData = (PlayerPointerEventData)eventData;
				this.Log(string.Concat(new string[]
				{
					"OnDrag:  Player = ",
					playerPointerEventData.playerId.ToString(),
					", Pointer Index = ",
					playerPointerEventData.inputSourceIndex.ToString(),
					", Source = ",
					PlayerPointerEventHandlerExample.GetSourceName(playerPointerEventData),
					", Button Index = ",
					playerPointerEventData.buttonIndex.ToString()
				}));
			}
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x000249D0 File Offset: 0x00022BD0
		public void OnEndDrag(PointerEventData eventData)
		{
			if (eventData is PlayerPointerEventData)
			{
				PlayerPointerEventData playerPointerEventData = (PlayerPointerEventData)eventData;
				this.Log(string.Concat(new string[]
				{
					"OnEndDrag:  Player = ",
					playerPointerEventData.playerId.ToString(),
					", Pointer Index = ",
					playerPointerEventData.inputSourceIndex.ToString(),
					", Source = ",
					PlayerPointerEventHandlerExample.GetSourceName(playerPointerEventData),
					", Button Index = ",
					playerPointerEventData.buttonIndex.ToString()
				}));
			}
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x00024A5C File Offset: 0x00022C5C
		private static string GetSourceName(PlayerPointerEventData playerEventData)
		{
			if (playerEventData.sourceType == PointerEventType.Mouse)
			{
				if (playerEventData.mouseSource is Behaviour)
				{
					return (playerEventData.mouseSource as Behaviour).name;
				}
			}
			else if (playerEventData.sourceType == PointerEventType.Touch && playerEventData.touchSource is Behaviour)
			{
				return (playerEventData.touchSource as Behaviour).name;
			}
			return null;
		}

		// Token: 0x0400067A RID: 1658
		public Text text;

		// Token: 0x0400067B RID: 1659
		private const int logLength = 10;

		// Token: 0x0400067C RID: 1660
		private List<string> log = new List<string>();
	}
}
