using System.IO;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Talents/New Talent Asset", fileName = "New Talent Asset")]
public class Talent : ScriptableObject
{
    [SerializeField] private new string name = default;
    [SerializeField] private string description = default;
    [SerializeField] private Sprite icon = default;
    
    [SerializeField] private Vector3 placeholderToUnlock = default;

    public string Name => name;
    public string Description => description;
    public Sprite Icon => icon;
    
    public Vector3 PlaceholderToUnlock => placeholderToUnlock;
    
    // load filename into name
    // + in OnValidate
    protected virtual void OnValidate()
    {
        var assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
            
        name = Path.GetFileNameWithoutExtension(assetPath);
    }
}
