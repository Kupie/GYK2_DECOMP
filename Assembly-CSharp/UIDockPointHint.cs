using System;
using Cinemachine;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200082A RID: 2090
public class UIDockPointHint : MonoBehaviour
{
	// Token: 0x06003573 RID: 13683 RVA: 0x00101068 File Offset: 0x000FF268
	public void Display(DockPoint dockPoint)
	{
		CinemachineCore.CameraUpdatedEvent.AddListener(new UnityAction<CinemachineBrain>(this.UpdatePos));
		this.targetWgo = dockPoint.Owner;
		this.targetDockPoint = dockPoint;
		string text = this.targetDockPoint.Direction.ToString()[0].ToString();
		this.icon.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("dockpoint_marker_" + text, null);
		base.gameObject.SetActive(true);
	}

	// Token: 0x06003574 RID: 13684 RVA: 0x001010F3 File Offset: 0x000FF2F3
	public void Remove()
	{
		CinemachineCore.CameraUpdatedEvent.RemoveListener(new UnityAction<CinemachineBrain>(this.UpdatePos));
		global::UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06003575 RID: 13685 RVA: 0x00101116 File Offset: 0x000FF316
	private void UpdatePos(CinemachineBrain brain)
	{
		base.transform.position = CameraSystem.WorldToScreenPoint(this.targetDockPoint.transform.position);
	}

	// Token: 0x04002AE1 RID: 10977
	[SerializeField]
	private RectTransform rectTransform;

	// Token: 0x04002AE2 RID: 10978
	[SerializeField]
	private Image icon;

	// Token: 0x04002AE3 RID: 10979
	private Wgo targetWgo;

	// Token: 0x04002AE4 RID: 10980
	private DockPoint targetDockPoint;
}
