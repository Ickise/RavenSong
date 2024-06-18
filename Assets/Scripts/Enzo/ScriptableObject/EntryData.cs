using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Entry/EntryData", order = 1)]
public class EntryData : ScriptableObject
{
    public string entryTitle;

    //[TextArea(1,50)] public string[] listOfTextEntry;

//  public Sprite[] entryVisualsList;

    public Sprite entryVisual;
}