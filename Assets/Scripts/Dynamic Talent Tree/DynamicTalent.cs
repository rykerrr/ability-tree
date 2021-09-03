using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Talent_Tree.Dynamic_Talent_Tree
{
    [CreateAssetMenu(menuName = "Talents/New Dynamic Talent Asset", fileName = "New Dynamic Talent Asset")]
    public class DynamicTalent : ScriptableObject
    {
        [Header("Talent data")]
        [SerializeField] private new string name = default;
        [SerializeField] private string description = default;
        [SerializeField] private Sprite icon = default;
        
        [SerializeField] private Vector3 placeholderToUnlock = default;

        [Header("Talent links")] 
        [SerializeField] private List<TalentLink> links;
        public string Name => name;
        public string Description => description;
        public Sprite Icon => icon;
        
        public Vector3 PlaceholderToUnlock => placeholderToUnlock;
        public List<TalentLink> Links => links;
        
        // load filename into name
        // + in OnValidate
        protected virtual void OnValidate()
        {
            var assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
                
            name = Path.GetFileNameWithoutExtension(assetPath);
        }
    }
}