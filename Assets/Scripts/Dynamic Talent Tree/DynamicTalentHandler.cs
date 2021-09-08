﻿using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Talent_Tree.Dynamic_Talent_Tree
{
    public class DynamicTalentHandler : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private Transform talentContainer = default;

        [Header("Properties")] [SerializeField]
        private int points = 20;

        public int Points => points;

        private readonly List<DynamicTalentUI> talents = new List<DynamicTalentUI>();
        private StringBuilder sb;
        
        private void Awake()
        {
            var talentList = GetTalentUIsFromTalentsContainer();
            
            talents.AddRange(talentList);
            sb = new StringBuilder();
        }

        private List<DynamicTalentUI> GetTalentUIsFromTalentsContainer()
        {
            var talentList = new List<DynamicTalentUI>();

            foreach (Transform talTransf in talentContainer)
            {
                var talUi = talTransf.GetComponent<DynamicTalentUI>();
                
                talentList.Add(talUi);
            }

            return talentList;
        }

        public void TryLevelupTalent(DynamicTalentUI talentUi)
        {
            // TODO: Lower points by level up points (weight)
            
            Debug.Log($"Talent levelup called on {talentUi}");
            
            var leveledUp = talentUi.TryLevelUp(points);

            if (leveledUp)
            {
                points -= talentUi.LevelWeight;
            }

            Debug.Log($"Leveled up: {leveledUp} Level weight: {talentUi.LevelWeight} Points: {points}");
        }

        [ContextMenu("Log all runtime-loaded talents")]
        public void LogTalentList()
        {
            sb.Clear();

            foreach (var talent in talents)
            {
                sb.Append(talent.GetTalentContainerInfo()).AppendLine();
            }
            
            Debug.Log(sb.ToString());
        }
    }
}