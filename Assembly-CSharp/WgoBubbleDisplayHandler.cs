using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200068A RID: 1674
public class WgoBubbleDisplayHandler : MonoBehaviour
{
	// Token: 0x06002CDB RID: 11483 RVA: 0x000D5D04 File Offset: 0x000D3F04
	public static void Display(Wgo wgo)
	{
		if (wgo.IsDespawning)
		{
			return;
		}
		UIObjectBubbleManager.Instance.RequestDisplay(wgo);
	}

	// Token: 0x06002CDC RID: 11484 RVA: 0x000D5D1A File Offset: 0x000D3F1A
	public static void Hide(Wgo wgo)
	{
		UIObjectBubbleManager.Instance.Hide(wgo);
	}

	// Token: 0x06002CDD RID: 11485 RVA: 0x000D5D27 File Offset: 0x000D3F27
	public static void HideWidget<T>(Wgo wgo) where T : LazyWidgetDataBase
	{
		UIObjectBubbleManager.Instance.HideWidget<T>(wgo);
	}

	// Token: 0x06002CDE RID: 11486 RVA: 0x000D5D34 File Offset: 0x000D3F34
	public static void HideWidget(Wgo wgo, LazyWidgetDataBase widgetData)
	{
		UIObjectBubbleManager.Instance.HideWidget(wgo, widgetData);
	}
}
