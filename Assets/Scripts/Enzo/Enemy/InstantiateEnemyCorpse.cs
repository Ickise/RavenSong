using UnityEngine;

[RequireComponent(typeof(OnBulletHit))]
public class InstantiateEnemyCorpse : MonoBehaviour
{
    [SerializeField, Header("IaCorpsePrefab")]
    private GameObject iaCorpsePrefab;
    [SerializeField] private GameObject VFXDeath;
    [SerializeField] private SoundData mobDisparitionSound;

    private OnBulletHit _onBulletHit;

    private void Awake()
    {
        _onBulletHit = GetComponent<OnBulletHit>();
    }

    private void Start()
    {
        _onBulletHit.onBulletHit.AddListener(OnBulletHit);
    }

    private void OnBulletHit(GameObject bullet)
    {
        VFXInstantieur.instance.PlayVFXInWorld(VFXDeath, transform, 3);
        AudioManager.instance.PlaySound(mobDisparitionSound);
        Instantiate(iaCorpsePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}