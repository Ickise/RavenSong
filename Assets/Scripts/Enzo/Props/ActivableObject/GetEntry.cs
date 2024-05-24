using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetEntry : MonoBehaviour
{
    [SerializeField] private EntryData _entryData;

    [SerializeField] private TextMeshProUGUI buttonTitleToUpdate;
    [SerializeField] private TextMeshProUGUI titleToUpdate;
    [SerializeField] private TextMeshProUGUI descriptionToUpdate;
    [SerializeField] private TextMeshProUGUI moreDescriptionToUpdate;
    
    [SerializeField] private Image spriteToUpdate;

    public void UpdateEntry()
    {
        buttonTitleToUpdate.text = _entryData.entryTitle;
        titleToUpdate.text = _entryData.entryTitle;
        descriptionToUpdate.text = _entryData.entryDescription;
        moreDescriptionToUpdate.text = _entryData.entryMoreDescription;
        spriteToUpdate.sprite = _entryData.entryVisual;
        Destroy(this);
    }
}
