using TMPro;
using UnityEngine;
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
     Tooltip("Il faut mettre la Description de la page du monstre du Bestiaire dont nous voulons changer le texte")]
    private TextMeshProUGUI descriptionToUpdate;

    [SerializeField,
     Tooltip(
         "Il faut mettre la Description supplémentaire de la page du monstre du Bestiaire dont nous voulons changer le texte")]
    private TextMeshProUGUI moreDescriptionToUpdate;

    [SerializeField, Header("Image"), Tooltip("Il faut l'Image de la page du monstre du Bestiaire dont nous voulons changer le visuel")]
    private Image spriteToUpdate;

    public void UpdateEntry()
    {
        buttonTitleToUpdate.text = _entryData.entryTitle;
        titleToUpdate.text = _entryData.entryTitle;
        descriptionToUpdate.text = _entryData.entryDescription;
        moreDescriptionToUpdate.text = _entryData.entryMoreDescription;
        spriteToUpdate.sprite = _entryData.entryVisual;
        spriteToUpdate.SetNativeSize();
        Destroy(this);
    }
}