using UnityEngine;

namespace Talent_Tree
{
    public class SelectTalentButton : MonoBehaviour
    {
        [SerializeField] private TalentOverviewUI talentOverviewUI = default;
        [SerializeField] private UnlockableTalentUI talentUi = default;

        private void Awake()
        {
            var talentUiNull = ReferenceEquals(talentUi, null);

            if (talentUiNull)
            {
                talentUi = GetComponent<UnlockableTalentUI>();
            }
        }

        public void OnClick_TrySelectThisTalent()
        {
            talentOverviewUI.SelectTalent(talentUi);
        }
    }
}
