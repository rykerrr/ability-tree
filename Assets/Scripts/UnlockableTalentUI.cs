using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Talent_Tree
{
    public class UnlockableTalentUI : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private Image iconImage = default;
        
        [Header("Talent properties")]
        [SerializeField] private TalentContainer talentContainer = default;
        [SerializeField] private UnlockState unlockState = UnlockState.NotUnlocked;
        
        [SerializeField] private List<TalentLinkUI> links = new List<TalentLinkUI>();

        private void Awake()
        {
            SetIconImageAsTalent();
        }

        // Will need to keep track of points, remove them if this return true
        public bool TryLevelUp(int points)
        {
            if (unlockState != UnlockState.Unlockable && unlockState != UnlockState.Unlocked)
            {
                Debug.LogWarning("Can't unlock at the moment, unlock state: " + unlockState);
                
                return false;
            }

            var leveledUp = talentContainer.TryLevelupTalent(points);
            if (!leveledUp) return false;
            
            if (talentContainer.CurrentTalentLevel >= 1)
            {
                unlockState = UnlockState.Unlocked;

                foreach (var link in links)
                {
                    if (link.CanTraverse(talentContainer.CurrentTalentLevel))
                    {
                        link.Destination.TrySetUnlockState(UnlockState.Unlockable);
                    }
                }
            }

            Debug.Log("Leveled up talent " + talentContainer.Talent.Name + " to level " +
                      talentContainer.CurrentTalentLevel);
                
            UpdateTalentUI();

            return true;

        }

        private void TrySetUnlockState(UnlockState newState)
        {
            switch (newState)
            {
                case UnlockState.NotUnlocked:
                {
                    unlockState = newState;
                    
                    break;
                }
                case UnlockState.Unlockable:
                {
                    if (unlockState == UnlockState.Unlocked) return;

                    unlockState = newState;
                    
                    break;
                }
                case UnlockState.Unlocked:
                {
                    if (unlockState == UnlockState.NotUnlocked) return;

                    unlockState = newState;
                    
                    break;
                }
            }
        }
        
        private void SetIconImageAsTalent()
        {
            iconImage.sprite = talentContainer.Talent.Icon;
        }
        
        private void UpdateTalentUI()
        {
            Debug.Log("updating ui teehee");
        }
    }
}
