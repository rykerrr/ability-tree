using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Talent_Tree
{
    public class TalentHandler : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private Transform talentContainer = default;

        [Header("Properties")] [SerializeField]
        private int points = 20;

        public int Points => points;

        private readonly List<UnlockableTalentUI> talents = new List<UnlockableTalentUI>();
        private StringBuilder sb;
        
        private void Awake()
        {
            var talentList = GetTalentUIsFromTalentsContainer();
            
            talents.AddRange(talentList);
            sb = new StringBuilder();
        }

        private List<UnlockableTalentUI> GetTalentUIsFromTalentsContainer()
        {
            var talentList = new List<UnlockableTalentUI>();

            foreach (Transform talTransf in talentContainer)
            {
                var talUi = talTransf.GetComponent<UnlockableTalentUI>();
                
                talentList.Add(talUi);
            }

            return talentList;
        }

        public void TryLevelupTalent(UnlockableTalentUI talentUi)
        {
            // TODO: Lower points by level up points (weight)
            var leveledUp = talentUi.TryLevelUp(points);

            if (leveledUp)
            {
                points -= talentUi.LevelWeight;
            }

            // Debug.Log("Leveled up: " + leveledUp);
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