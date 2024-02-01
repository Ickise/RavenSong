using Unity.Mathematics;
using UnityEngine;

public class InstantiateEnemyCorpse : MonoBehaviour
{
    [SerializeField] private GameObject prefabIACorpse;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            OnIaDeath();
        }
    }

    public void OnIaDeath()
    {
        //changer l'animation ici pour qu'elle se joue et ensuite instantiate un corps
        Instantiate(prefabIACorpse, transform.position, quaternion.identity);
    }
}