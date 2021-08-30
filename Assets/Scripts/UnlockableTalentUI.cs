using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Talent_Tree
{
    public class UnlockableTalentUI : MonoBehaviour
    {
        [Header("UI References")] 
        [SerializeField] private Image iconImage = default;
        [SerializeField] private Image tintImage = default;
        [SerializeField] private TextMeshProUGUI countText = default;
        
        [Header("Talent properties")]
        [SerializeField] private TalentContainer talentContainer = default;
        [SerializeField] private UnlockState unlockState = UnlockState.NotUnlocked;
        [SerializeField] private Color unlockedTintColor = Color.white;
        
        [SerializeField] private List<TalentLinkUI> links = new List<TalentLinkUI>();
        
        public event Action onTalentUnlocked = delegate { };
        public event Action onTalentLeveledUp = delegate { };
        
        public TalentContainer TalentContainer => talentContainer;
        public int LevelWeight => talentContainer.LevelWeight;
        public List<TalentLinkUI> Links => links;
        
        private void Awake()
        {
            countText.gameObject.SetActive(false);

            SetIconImageAsTalent();
            UpdateTalentUI();
        }

        // Will need to keep track of points, remove them if this return true
        public bool TryLevelUp(int points)
        {
            if (unlockState != UnlockState.Unlockable && unlockState != UnlockState.Unlocked)
            {
                Debug.LogWarning("Can't unlock at the moment, unlock state: " + unlockState);

                return false;
            }

            var levelUpResult = talentContainer.TryLevelupTalent(points);
            if (!levelUpResult) return false;
            
            if (talentContainer.CurrentTalentLevel >= 1)
            {
                if (talentContainer.CurrentTalentLevel == 1)
                {
                    onTalentUnlocked?.Invoke();

                    unlockState = UnlockState.Unlocked;
                }

                foreach (var link in links)
                {
                    if (link.CanTraverse(talentContainer.CurrentTalentLevel))
                    {
                        link.Destination.TrySetUnlockState(UnlockState.Unlockable);
                    }
                }
            }

            UpdateTalentUI();

            onTalentLeveledUp?.Invoke();
            
            Debug.Log("Leveled up talent " + talentContainer.Talent.Name + " to level " +
                      talentContainer.CurrentTalentLevel);
            
            return true;
        }

        public string GetTalentContainerInfo()
        {
            return talentContainer.GetTalentContainerInfo();
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
            
            UpdateTalentUI();
        }
        
        private void SetIconImageAsTalent()
        {
            iconImage.sprite = talentContainer.Talent.Icon;
        }
        
        private void UpdateTalentUI()
        {
            if (unlockState == UnlockState.Unlockable || unlockState == UnlockState.Unlocked)
            {
                if(!countText.gameObject.activeSelf) countText.gameObject.SetActive(true);
                
                tintImage.color = unlockedTintColor;
            }
            
            countText.text = $"{TalentContainer.CurrentTalentLevel}/{TalentContainer.MaxTalentLevel}";

            UpdateLinksUI();
        }

        private void UpdateLinksUI()
        {
            foreach (var link in links)
            {
                link.UpdateFill(talentContainer.CurrentTalentLevel);
            }
        }
    }
}
