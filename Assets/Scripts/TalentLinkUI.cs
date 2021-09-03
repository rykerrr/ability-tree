using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Talent_Tree
{
    public class TalentLinkUI : MonoBehaviour
    {
        [SerializeField] private UnlockableTalentUI destination = default;
        [SerializeField] private int weightRequiredInBase = 1;
        [SerializeField] private List<TalentLinkPartUI> parts = default;
        
        public UnlockableTalentUI Destination => destination;
        public bool CanTraverse(int points) => points >= weightRequiredInBase;
        
        private float fullLength = 0;
        
        private void Start()
        {
            foreach (var part in parts)
            {
                fullLength += part.Length;
                
                part.SetFillInstant(0);
            }
        }

        public void UpdateFill(int baseLevel)
        {
            var fill = Mathf.Clamp01((float)baseLevel / weightRequiredInBase);
            var lenWithFill = fill * fullLength;
            
            for (int i = 0; i < parts.Count; i++)
            {
                var part = parts[i];
                
                if (lenWithFill >= part.Length)
                {
                    lenWithFill -= part.Length;
                    
                    part.SetFillInstant(1);
                }
                else
                {
                    var partLen = lenWithFill / part.Length;

                    // What if we're setting the fill of the 2nd part only instead?
                    part.SetFillInstant(partLen);
                    
                    break;
                }
            }
        }
    }
}