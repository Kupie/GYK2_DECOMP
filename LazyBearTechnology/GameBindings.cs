using System;
using System.Collections.Generic;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x0200012B RID: 299
	[CreateAssetMenu(fileName = "GameBindings", menuName = "Lazy/GameBindings", order = 1)]
	public class GameBindings : LazySingletonSO<GameBindings>
	{
		// Token: 0x040002FE RID: 766
		public List<KeyBinding> keyBindings;

		// Token: 0x040002FF RID: 767
		public List<GamepadBinding> gamepadBindings;

		// Token: 0x04000300 RID: 768
		public List<HoldableElement> canBeHoldedForRepeatPress;

		// Token: 0x04000301 RID: 769
		public List<HoldedGroup> holdedGroups;

		// Token: 0x04000302 RID: 770
		public List<BindingAlias> bindingAliases;
	}
}
