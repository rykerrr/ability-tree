using System;
using UnityEngine;

namespace Talent_Tree
{
	[Serializable]
	public class UISetPositionToOtherObjectCenter
	{
		private RectTransform objToSet = default;
		private RectTransform otherObj = default;

		public UISetPositionToOtherObjectCenter(RectTransform objToSet, RectTransform otherObj)
		{
			this.objToSet = objToSet;
			this.otherObj = otherObj;
		}
		
		public void ResetPos()
		{
			objToSet.position = otherObj.position;
		}
	}
}
