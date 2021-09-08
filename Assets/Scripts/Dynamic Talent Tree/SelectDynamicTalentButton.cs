using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Talent_Tree.Dynamic_Talent_Tree
{
	public class SelectDynamicTalentButton : MonoBehaviour
	{
		[SerializeField] private DynamicTalentOverviewUI dynamicTalentOverviewUI = default;
		[SerializeField] private DynamicTalentUI talentUi = default;
		[SerializeField] private Button thisButton = default;
		
		public DynamicTalentOverviewUI DynamicTalentOverviewUI
		{
			get => dynamicTalentOverviewUI;
			set
			{
				thisButton.onClick.RemoveAllListeners();
				
				dynamicTalentOverviewUI = value;

				thisButton.onClick.AddListener(OnClick_TrySelectThisTalent);
			}
		}

		private void Awake()
		{
			var talentUiNull = ReferenceEquals(talentUi, null);
			
			if (talentUiNull)
			{
				talentUi = GetComponent<DynamicTalentUI>();
			}
		}

		public void OnClick_TrySelectThisTalent()
		{
			dynamicTalentOverviewUI.SelectTalent(talentUi);
		}
	}
}
