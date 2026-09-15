using System;
using LazyBearTechnology;
using UnityEngine.UI;

// Token: 0x020009B3 RID: 2483
public class Dev_ButtonsPressTestWindow : LazyWindow<LazyWidgetDataBase>
{
	// Token: 0x0600422E RID: 16942 RVA: 0x0013AA8C File Offset: 0x00138C8C
	public override void Init()
	{
		base.Init();
		this.makeAllInteractable.onClick.AddListener(delegate
		{
			if (this.isAllInteractable)
			{
				foreach (Button button in base.GetComponentsInChildren<Button>(true))
				{
					if (!(button == this.makeAllInteractable))
					{
						button.interactable = false;
					}
				}
			}
			else
			{
				foreach (Button button2 in base.GetComponentsInChildren<Button>(true))
				{
					if (!(button2 == this.makeAllInteractable))
					{
						button2.interactable = true;
					}
				}
			}
			this.isAllInteractable = !this.isAllInteractable;
		});
	}

	// Token: 0x0600422F RID: 16943 RVA: 0x0013AAB0 File Offset: 0x00138CB0
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Open(null);
	}

	// Token: 0x040033A8 RID: 13224
	public LazyButton makeAllInteractable;

	// Token: 0x040033A9 RID: 13225
	public bool isAllInteractable = true;
}
