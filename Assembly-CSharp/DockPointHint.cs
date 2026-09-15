using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000153 RID: 339
public class DockPointHint : MonoBehaviour
{
	// Token: 0x06000806 RID: 2054 RVA: 0x00027614 File Offset: 0x00025814
	public void Display(DockPoint dockPoint)
	{
		this.targetDockPoint = dockPoint;
		string text = this.targetDockPoint.Direction.ToString()[0].ToString();
		this.icon.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("dockpoint_marker_" + text, null);
		this.UpdatePos();
		base.gameObject.SetActive(true);
	}

	// Token: 0x06000807 RID: 2055 RVA: 0x00027683 File Offset: 0x00025883
	public void Remove()
	{
		this.targetDockPoint = null;
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000808 RID: 2056 RVA: 0x00027698 File Offset: 0x00025898
	private void LateUpdate()
	{
		if (this.targetDockPoint == null)
		{
			return;
		}
		this.UpdatePos();
	}

	// Token: 0x06000809 RID: 2057 RVA: 0x000276B0 File Offset: 0x000258B0
	private void UpdatePos()
	{
		Vector3 position = this.targetDockPoint.transform.position;
		position.y += this.yOffset;
		base.transform.position = position;
	}

	// Token: 0x040009FF RID: 2559
	[SerializeField]
	private SpriteRenderer icon;

	// Token: 0x04000A00 RID: 2560
	[SerializeField]
	private float yOffset = 0.02f;

	// Token: 0x04000A01 RID: 2561
	private DockPoint targetDockPoint;
}
