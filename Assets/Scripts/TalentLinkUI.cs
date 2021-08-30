using System;
using UnityEngine;

namespace Talent_Tree
{
    public class TalentLinkUI : MonoBehaviour
    {
        [SerializeField] private UnlockableTalentUI destination = default;
        [SerializeField] private int weightRequiredInBase = 1;

        public UnlockableTalentUI Destination => destination;
        
        public bool CanTraverse(int points) => points >= weightRequiredInBase;
    }
}