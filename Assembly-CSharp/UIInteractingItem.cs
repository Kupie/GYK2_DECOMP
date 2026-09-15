using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000806 RID: 2054
public class UIInteractingItem : MonoBehaviour
{
	// Token: 0x06003497 RID: 13463 RVA: 0x000FCF44 File Offset: 0x000FB144
	public void Init()
	{
		UIInteractingItem.instance = this;
		base.gameObject.SetActive(false);
	}

	// Token: 0x06003498 RID: 13464 RVA: 0x000E5053 File Offset: 0x000E3253
	public void DisableBubble()
	{
		global::UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06003499 RID: 13465 RVA: 0x000FCF58 File Offset: 0x000FB158
	public static UIInteractingItem ShowInteractingItem(Item item, Transform targetTransform)
	{
		if (UIInteractingItem.instance == null)
		{
			Debug.LogError("InteractingItem.ShowInteractingitem error: instance is null");
			return null;
		}
		UIInteractingItem uiinteractingItem = UIInteractingItem.instance.Copy(null, true, "");
		uiinteractingItem.targetTransform = targetTransform;
		uiinteractingItem.ShowItem(item);
		return uiinteractingItem;
	}

	// Token: 0x0600349A RID: 13466 RVA: 0x000FCF94 File Offset: 0x000FB194
	private void ShowItem(Item item)
	{
		this.interactingItem.Draw(item, false, -1, false, 1, false, 0, true, false, false, ItemRelatedWidgetState.NotSet, false);
		base.gameObject.SetActive(true);
		CinemachineCore.CameraUpdatedEvent.AddListener(new UnityAction<CinemachineBrain>(this.UpdatePosition));
	}

	// Token: 0x0600349B RID: 13467 RVA: 0x000FCFDA File Offset: 0x000FB1DA
	private void UpdatePosition(CinemachineBrain brain)
	{
		base.transform.position = CameraSystem.WorldToScreenPoint(this.targetTransform.position);
	}

	// Token: 0x0600349C RID: 13468 RVA: 0x000FCFF7 File Offset: 0x000FB1F7
	private void OnDestroy()
	{
		CinemachineCore.CameraUpdatedEvent.RemoveListener(new UnityAction<CinemachineBrain>(this.UpdatePosition));
	}

	// Token: 0x04002A12 RID: 10770
	[SerializeField]
	private UIItemCell interactingItem;

	// Token: 0x04002A13 RID: 10771
	private static UIInteractingItem instance;

	// Token: 0x04002A14 RID: 10772
	private Transform targetTransform;
}
