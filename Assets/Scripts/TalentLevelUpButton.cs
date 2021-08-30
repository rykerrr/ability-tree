using System;
using UnityEngine;

namespace Talent_Tree
{
    public class TalentLevelUpButton : MonoBehaviour
    {
        [SerializeField] private TalentHandler talentHandler = default;
        [SerializeField] private UnlockableTalentUI talentUi;

        private void Awake()
        {
            var talentUiNull = ReferenceEquals(talentUi, null);

            if (talentUiNull)
            {
                talentUi = GetComponent<UnlockableTalentUI>();
            }
        }

        public void OnClick_TryLevelThisTalent()
        {
            talentHandler.TryLevelupTalent(talentUi);
        }
    }
}
