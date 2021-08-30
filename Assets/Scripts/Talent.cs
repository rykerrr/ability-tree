using UnityEngine;

[CreateAssetMenu(menuName = "Talents/New Talent Asset", fileName = "New Talent Asset")]
public class Talent : ScriptableObject
{
    [SerializeField] private new string name = default;
    [SerializeField] private Sprite icon;
    
    [SerializeField] private Vector3 placeholderToUnlock = default;

    public string Name => name;
    public Sprite Icon => icon;
    
    public Vector3 PlaceholderToUnlock => placeholderToUnlock;
    
    // load filename into name
    // + in OnValidate
}
