using System;
using UnityEngine;

namespace Talent_Tree
{
    [Serializable]
    public class TalentContainer
    {
        [SerializeField] private Talent talent = default;
        
        [SerializeField] private int maxTalentLevel = 5;
        [SerializeField] private int levelWeight = 1;

        private int currentTalentLevel;

        public Talent Talent => talent;
        public int MaxTalentLevel => maxTalentLevel;
        public int LevelWeight => levelWeight;
        public int CurrentTalentLevel => currentTalentLevel;

        public bool TryLevelupTalent(int points)
        {
            var notEnoughPoints = points < levelWeight;
            var levelTooHigh = currentTalentLevel >= maxTalentLevel;
            
            if (notEnoughPoints || levelTooHigh)
            {
                Debug.LogWarning(notEnoughPoints ? "Not enough points" : "Level is too high");

                return false;
            }

            currentTalentLevel++;
            return true;
        }
    }
}
