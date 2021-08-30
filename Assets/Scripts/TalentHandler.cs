using UnityEngine;

namespace Talent_Tree
{
    public class TalentHandler : MonoBehaviour
    {
        [SerializeField] private int points;

        public int Points => points;

        public void TryLevelupTalent(UnlockableTalentUI talentUi)
        {
            // TODO: Lower points by level up points (weight)
            var leveledUp = talentUi.TryLevelUp(points);
            
            Debug.Log("Leveled up: " + leveledUp);
        }
    }
}
