using System;
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
        
        private void Awake()
        {
            foreach (var part in parts)
            {
                fullLength += part.Length;
                
                part.SetFill(0);
            }
        }

        public void UpdateFill(int baseLevel)
        {
            var fill = Mathf.Clamp01((float)baseLevel / weightRequiredInBase);
            var lenWithFill = fill * fullLength;
            
            Debug.Log("a");
            
            foreach (var part in parts)
            {
                if (lenWithFill >= part.Length)
                {
                    Debug.Log("b");
                    
                    lenWithFill -= part.Length;
                    
                    part.SetFill(1);
                }
                else
                {
                    Debug.Log("c");
                    
                    var partLen = lenWithFill / part.Length;
                    
                    Debug.Log(partLen);
                    part.SetFill(partLen);

                    break;
                }
            }
        }
    }
}