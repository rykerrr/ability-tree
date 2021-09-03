using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Talent_Tree.Dynamic_Talent_Tree
{
    public class TalentOverviewUI : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private TalentHandler talentHandler = default;
        
        [Header("UI References")]
        [SerializeField] private Image talentIcon = default;
        [SerializeField] private TextMeshProUGUI talentName = default;
        [SerializeField] private TextMeshProUGUI talentDescription = default;
        [SerializeField] private TextMeshProUGUI talentLevelDisplay = default;
        [SerializeField] private GameObject unlockTalentButton = default;
        [SerializeField] private GameObject talentIconParent = default;
        
        private DynamicTalentUI selectedTalentUi = default;

        private void Awake()
        {
            UpdateDisplay();
        }

        public void SelectTalent(DynamicTalentUI talentUi)
        {
            selectedTalentUi = talentUi;
            
            selectedTalentUi.onTalentLeveledUp += UpdateDisplay;

            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (selectedTalentUi == null)
            {
                DisableOverview();

                return;
            }
            
            if(!unlockTalentButton.activeSelf) unlockTalentButton.SetActive(true);

            var talContainer = selectedTalentUi.TalentContainer;
            var tal = talContainer.DynamicTalent;

            talentName.text = tal.Name;
            talentDescription.text = tal.Description;
            talentLevelDisplay.text = $"Current Level/Max Level: {talContainer.CurrentTalentLevel}/{talContainer.MaxTalentLevel}\n"
                + $"Points required for level up: {talContainer.LevelWeight}";
            
            if(!talentIconParent.activeSelf) talentIconParent.SetActive(true);
            talentIcon.sprite = tal.Icon;
        }

        private void DisableOverview()
        {
            if(unlockTalentButton.activeSelf) unlockTalentButton.SetActive(false);

            talentName.text = "";
            talentDescription.text = "";
            talentLevelDisplay.text = "";

            if(talentIconParent.activeSelf) talentIconParent.SetActive(false);
            talentIcon.sprite = null;
        }

        private void DeselectTalentUI()
        {
            var selectedTalentNotNull = !ReferenceEquals(selectedTalentUi, null);
            if (selectedTalentNotNull)
            {
                selectedTalentUi.onTalentLeveledUp -= UpdateDisplay;
            }
            
            DisableOverview();
        }

        public void OnClick_TryUnlockTalent()
        {
            talentHandler.TryLevelupTalent(selectedTalentUi);
        }
    }
}
