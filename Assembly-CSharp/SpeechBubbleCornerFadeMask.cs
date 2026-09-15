using System;
using System.Collections.Generic;
using LazyBearTechnology;
using SoftMasking;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000801 RID: 2049
public class SpeechBubbleCornerFadeMask : MonoBehaviour
{
	// Token: 0x06003480 RID: 13440 RVA: 0x000FC640 File Offset: 0x000FA840
	public static SpeechBubbleCornerFadeMask Ensure(Image background, List<UIBubbleCorner> corners, GameObject host)
	{
		SpeechBubbleCornerFadeMask speechBubbleCornerFadeMask = host.GetComponent<SpeechBubbleCornerFadeMask>();
		if (speechBubbleCornerFadeMask == null)
		{
			speechBubbleCornerFadeMask = host.AddComponent<SpeechBubbleCornerFadeMask>();
		}
		speechBubbleCornerFadeMask.background = background;
		speechBubbleCornerFadeMask.corners = corners;
		speechBubbleCornerFadeMask.EnsureHierarchy();
		return speechBubbleCornerFadeMask;
	}

	// Token: 0x06003481 RID: 13441 RVA: 0x000FC67C File Offset: 0x000FA87C
	public void Sync(UIBubbleCorner currentCorner)
	{
		if (this.cornerPunchMask == null)
		{
			this.EnsureHierarchy();
		}
		if (this.cornerPunchMask == null)
		{
			return;
		}
		Image activeCornerImage = this.GetActiveCornerImage(currentCorner);
		bool flag = activeCornerImage != null && activeCornerImage.sprite != null && activeCornerImage.enabled;
		this.cornerPunchMask.enabled = flag;
		if (!flag)
		{
			return;
		}
		this.cornerPunchMask.sprite = activeCornerImage.sprite;
		this.cornerPunchMask.spriteBorderMode = SoftMask.BorderMode.Simple;
		this.cornerPunchMask.spritePixelsPerUnitMultiplier = activeCornerImage.pixelsPerUnitMultiplier;
		this.cornerPunchMask.separateMask = activeCornerImage.rectTransform;
	}

	// Token: 0x06003482 RID: 13442 RVA: 0x000FC724 File Offset: 0x000FA924
	private void EnsureHierarchy()
	{
		if (this.background == null || this.corners == null || this.corners.Count == 0)
		{
			return;
		}
		Transform parent = this.background.rectTransform.parent;
		SoftMask softMask = ((parent != null) ? parent.GetComponent<SoftMask>() : null);
		if (softMask == null && parent != null)
		{
			Transform transform = parent.Find("CornerOverlapMask");
			if (transform != null)
			{
				softMask = transform.GetComponent<SoftMask>();
			}
		}
		Transform transform2 = ((softMask != null) ? softMask.transform.parent : parent);
		if (transform2 == null)
		{
			return;
		}
		if (softMask != null)
		{
			this.cornerPunchMask = softMask;
			this.ApplyHierarchy(transform2);
			return;
		}
		GameObject gameObject = new GameObject("CornerOverlapMask", new Type[]
		{
			typeof(RectTransform),
			typeof(LayoutElement),
			typeof(SoftMask)
		});
		RectTransform component = gameObject.GetComponent<RectTransform>();
		component.SetParent(transform2, false);
		component.anchorMin = Vector2.zero;
		component.anchorMax = Vector2.one;
		component.offsetMin = Vector2.zero;
		component.offsetMax = Vector2.zero;
		component.pivot = this.background.rectTransform.pivot;
		gameObject.GetComponent<LayoutElement>().ignoreLayout = true;
		this.cornerPunchMask = gameObject.GetComponent<SoftMask>();
		this.cornerPunchMask.source = SoftMask.MaskSource.Sprite;
		this.cornerPunchMask.invertMask = true;
		this.cornerPunchMask.invertOutsides = true;
		this.ApplyHierarchy(transform2);
	}

	// Token: 0x06003483 RID: 13443 RVA: 0x000FC8B0 File Offset: 0x000FAAB0
	private void ApplyHierarchy(Transform container)
	{
		for (int i = 0; i < this.corners.Count; i++)
		{
			if (this.corners[i].rectTransform.parent != container)
			{
				this.corners[i].rectTransform.SetParent(container, false);
			}
		}
		if (this.background.rectTransform.parent != this.cornerPunchMask.transform)
		{
			this.background.rectTransform.SetParent(this.cornerPunchMask.transform, false);
		}
		this.cornerPunchMask.transform.SetAsFirstSibling();
		Image[] componentsInChildren = this.background.GetComponentsInChildren<Image>(true);
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			if (componentsInChildren[j] != this.background)
			{
				componentsInChildren[j].maskable = false;
			}
		}
	}

	// Token: 0x06003484 RID: 13444 RVA: 0x000FC98C File Offset: 0x000FAB8C
	private Image GetActiveCornerImage(UIBubbleCorner currentCorner)
	{
		if (currentCorner == null)
		{
			return null;
		}
		if (SpeechBubbleCornerFadeMask.IsUsable(currentCorner.customImage2))
		{
			return currentCorner.customImage2;
		}
		if (SpeechBubbleCornerFadeMask.IsUsable(currentCorner.customImage))
		{
			return currentCorner.customImage;
		}
		return null;
	}

	// Token: 0x06003485 RID: 13445 RVA: 0x000FC9CF File Offset: 0x000FABCF
	private static bool IsUsable(Image image)
	{
		return image != null && image.enabled && image.gameObject.activeSelf && image.sprite != null;
	}

	// Token: 0x040029FE RID: 10750
	private const string MaskObjectName = "CornerOverlapMask";

	// Token: 0x040029FF RID: 10751
	private Image background;

	// Token: 0x04002A00 RID: 10752
	private List<UIBubbleCorner> corners;

	// Token: 0x04002A01 RID: 10753
	private SoftMask cornerPunchMask;
}
