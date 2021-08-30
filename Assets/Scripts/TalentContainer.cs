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

            if (notEnoughPoints || levelTooHigh) return false;

            currentTalentLevel++;
            return true;
        }

        public string GetTalentContainerInfo()
        {
            return $"[\nName: {talent.Name}\nCurrent talent level: {currentTalentLevel}\n" +
                   $"Max talent level: {maxTalentLevel}\nDescription: {talent.Description}\n]";
        }
    }
}
