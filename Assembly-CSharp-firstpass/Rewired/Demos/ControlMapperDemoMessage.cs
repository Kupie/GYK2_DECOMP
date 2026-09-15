using System;
using System.Collections;
using Rewired.UI.ControlMapper;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.Demos
{
	// Token: 0x02000111 RID: 273
	[AddComponentMenu("")]
	public class ControlMapperDemoMessage : MonoBehaviour
	{
		// Token: 0x06000CFE RID: 3326 RVA: 0x000261BD File Offset: 0x000243BD
		private void Awake()
		{
			if (this.controlMapper != null)
			{
				this.controlMapper.ScreenClosedEvent += this.OnControlMapperClosed;
				this.controlMapper.ScreenOpenedEvent += this.OnControlMapperOpened;
			}
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x000261FB File Offset: 0x000243FB
		private void Start()
		{
			this.SelectDefault();
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x00026203 File Offset: 0x00024403
		private void OnControlMapperClosed()
		{
			base.gameObject.SetActive(true);
			base.StartCoroutine(this.SelectDefaultDeferred());
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x0002621E File Offset: 0x0002441E
		private void OnControlMapperOpened()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x0002622C File Offset: 0x0002442C
		private void SelectDefault()
		{
			if (EventSystem.current == null)
			{
				return;
			}
			if (this.defaultSelectable != null)
			{
				EventSystem.current.SetSelectedGameObject(this.defaultSelectable.gameObject);
			}
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x0002625F File Offset: 0x0002445F
		private IEnumerator SelectDefaultDeferred()
		{
			yield return null;
			this.SelectDefault();
			yield break;
		}

		// Token: 0x040006CD RID: 1741
		public ControlMapper controlMapper;

		// Token: 0x040006CE RID: 1742
		public Selectable defaultSelectable;
	}
}
