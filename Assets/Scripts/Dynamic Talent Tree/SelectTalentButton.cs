using UnityEngine;

namespace Talent_Tree.Dynamic_Talent_Tree
{
    public class SelectTalentButton : MonoBehaviour
    {
        [SerializeField] private TalentOverviewUI talentOverviewUI = default;
        [SerializeField] private DynamicTalentUI talentUi = default;

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
            talentOverviewUI.SelectTalent(talentUi);
        }
    }
}
