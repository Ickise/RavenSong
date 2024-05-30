using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GetEntry : MonoBehaviour
{
    [SerializeField, Header("ScriptableObject"),
     Tooltip(
         "Il faut mettre le ScriptableObject EntryData de l'Entry que nous voulons faire débloquer au joueur via cet objet")]
    private EntryData _entryData;

    [SerializeField, Header("Text"),
     Tooltip("Il faut mettre le Text du Button du Bestiaire dont nous voulons changer le titre")]
    private TextMeshProUGUI buttonTitleToUpdate;

    [SerializeField,
     Tooltip("Il faut mettre le Text du Titre de la page du monstre du Bestiaire dont nous voulons changer le titre")]
    private TextMeshProUGUI titleToUpdate;
    
    [SerializeField,
     Tooltip("Il faut mettre les textes de la page du monstre du Bestiaire dont nous voulons changer le texte et l'ordre a une importance")]
    private TextMeshProUGUI[] descriptionListToUpdate;

    [SerializeField, Header("Image"), Tooltip("Il faut l'Image de la page du monstre du Bestiaire dont nous voulons changer le visuel")]
    private Image spriteToUpdate;

    public void UpdateEntry()
    {
        buttonTitleToUpdate.text = _entryData.entryTitle;
        titleToUpdate.text = _entryData.entryTitle;

        for (int i = 0; i < descriptionListToUpdate.Length; i++)
        {
            descriptionListToUpdate[i].text = _entryData.listOfTextEntry[i];
        }
        
        spriteToUpdate.sprite = _entryData.entryVisual;
        spriteToUpdate.SetNativeSize();
        Destroy(this);
    }
}