using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheraBytes.BetterUi
{
	// Token: 0x02000B46 RID: 2886
	[HelpURL("https://documentation.therabytes.de/better-ui/BetterTextMeshPro-Dropdown.html")]
	[AddComponentMenu("Better UI/TextMeshPro/Better TextMeshPro - Dropdown", 30)]
	public class BetterTextMeshProDropdown : TMP_Dropdown, IBetterTransitionUiElement
	{
		// Token: 0x17000B71 RID: 2929
		// (get) Token: 0x06004C97 RID: 19607 RVA: 0x001697EA File Offset: 0x001679EA
		public List<Transitions> BetterTransitions
		{
			get
			{
				return this.betterTransitions;
			}
		}

		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x06004C98 RID: 19608 RVA: 0x001697F2 File Offset: 0x001679F2
		public List<Transitions> ShowHideTransitions
		{
			get
			{
				return this.showHideTransitions;
			}
		}

		// Token: 0x06004C99 RID: 19609 RVA: 0x001697FC File Offset: 0x001679FC
		protected override void DoStateTransition(Selectable.SelectionState state, bool instant)
		{
			base.DoStateTransition(state, instant);
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			foreach (Transitions transitions in this.betterTransitions)
			{
				transitions.SetState(state.ToString(), instant);
			}
		}

		// Token: 0x06004C9A RID: 19610 RVA: 0x00169870 File Offset: 0x00167A70
		protected override GameObject CreateDropdownList(GameObject template)
		{
			foreach (Transitions transitions in this.showHideTransitions)
			{
				transitions.SetState("Show", false);
			}
			return base.CreateDropdownList(template);
		}

		// Token: 0x06004C9B RID: 19611 RVA: 0x001698D0 File Offset: 0x00167AD0
		protected override void DestroyDropdownList(GameObject dropdownList)
		{
			foreach (Transitions transitions in this.showHideTransitions)
			{
				transitions.SetState("Hide", false);
			}
			base.DestroyDropdownList(dropdownList);
		}

		// Token: 0x04003DB6 RID: 15798
		[SerializeField]
		[DefaultTransitionStates]
		private List<Transitions> betterTransitions = new List<Transitions>();

		// Token: 0x04003DB7 RID: 15799
		[SerializeField]
		[TransitionStates(new string[] { "Show", "Hide" })]
		private List<Transitions> showHideTransitions = new List<Transitions>();
	}
}
