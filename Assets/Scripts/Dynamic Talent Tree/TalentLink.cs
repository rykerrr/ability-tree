using System;
using UnityEngine;

namespace Talent_Tree.Dynamic_Talent_Tree
{
    [Serializable]
    public class TalentLink
    {
        [SerializeField] private DynamicTalent destination = default;
        [SerializeField] private int weightRequiredInBase = 1;
        
        public DynamicTalent Destination => destination;
        public int WeightRequiredInBase => weightRequiredInBase;
    }
}