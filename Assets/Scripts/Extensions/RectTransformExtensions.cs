using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Talent_Tree
{
	public static class RectTransformExtensions
	{
		public static void SetPivot(this RectTransform rectTransform, Vector2 pivot)
		{
			Vector3 deltaPosition = rectTransform.pivot - pivot;    // get change in pivot
			deltaPosition.Scale(rectTransform.rect.size);           // apply sizing
			deltaPosition.Scale(rectTransform.localScale);          // apply scaling
			deltaPosition = rectTransform.rotation * deltaPosition; // apply rotation
     
			rectTransform.pivot = pivot;                            // change the pivot
			rectTransform.localPosition -= deltaPosition;           // reverse the position change
		}
		
		public static void SetAnchors(this RectTransform rectTransform, Vector2 anchorMin, Vector2 anchorMax)
		{
			var parent = rectTransform.parent;
			if (parent) rectTransform.SetParent(null);
			rectTransform.anchorMin = anchorMin;
			rectTransform.anchorMax = anchorMax;
			if (parent) rectTransform.SetParent(parent);
		}

		public static void SetAnchorsAndPivot(this RectTransform rectTransform, Vector2 anchorMin, Vector2 anchorMax
			, Vector2 pivot)
		{
			rectTransform.SetAnchors(anchorMin, anchorMax);
			rectTransform.SetPivot(pivot);
		}
	}
}
