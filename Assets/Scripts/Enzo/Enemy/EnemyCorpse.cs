using UnityEngine;

public class EnemyCorpse : MonoBehaviour
{
    [SerializeField] private GameObject prefabIACorpse;
    public void OnIaDeath()
    {
        Instantiate(prefabIACorpse);
    }
}
