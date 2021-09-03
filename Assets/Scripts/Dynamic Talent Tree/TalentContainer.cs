using System;
using UnityEngine;

namespace Talent_Tree.Dynamic_Talent_Tree
{
    [Serializable]
    public class TalentContainer
    {
        [SerializeField] private DynamicTalent dynamicTalent = default;
        
        [SerializeField] private int maxTalentLevel = 5;
        [SerializeField] private int levelWeight = 1;

        private int currentTalentLevel;

        public DynamicTalent DynamicTalent => dynamicTalent;
        public int MaxTalentLevel => maxTalentLevel;
        public int LevelWeight => levelWeight;
        public int CurrentTalentLevel => currentTalentLevel;

        public TalentContainer(DynamicTalent dynamicTalent, int maxTalentLevel, int levelWeight)
        {
            this.dynamicTalent = dynamicTalent;
            this.maxTalentLevel = maxTalentLevel;
            this.levelWeight = levelWeight;
        }
        
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
            return $"[\nName: {dynamicTalent.Name}\nCurrent talent level: {currentTalentLevel}\n" +
                   $"Max talent level: {maxTalentLevel}\nDescription: {dynamicTalent.Description}\n]";
        }
    }
}
