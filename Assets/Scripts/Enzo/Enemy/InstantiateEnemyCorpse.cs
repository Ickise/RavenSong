using System;
using Unity.Mathematics;
using UnityEngine;

public class InstantiateEnemyCorpse : MonoBehaviour
{
    [SerializeField] private GameObject prefabIACorpse;

    private OnBulletHit _onBulletHit;

    private void Awake()
    {
        _onBulletHit = GetComponent<OnBulletHit>();
    }

    private void Start()
    {
        _onBulletHit.onBulletHit.AddListener(OnBulletHit);
    }

    private void OnBulletHit()
    {
        //lorsque la balle touche l'ennemi, cela fait appraître un cadavre et détruit l'ennemi
        Instantiate(prefabIACorpse, transform.position, quaternion.identity);
        Destroy(gameObject);
    }
}