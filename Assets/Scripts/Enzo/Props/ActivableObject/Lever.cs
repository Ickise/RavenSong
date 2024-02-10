using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private Door _door;

    public bool isActive;

    private GameObject bullet;

    private BulletCollisionDetection _bulletCollisionDetection;

    private OnBulletHit _onBulletHit;

    private Animator leverAnimator;

    private void Awake()
    {
        _onBulletHit = GetComponent<OnBulletHit>();
        leverAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        _onBulletHit.onBulletHit.AddListener(OnBulletHit);
    }

    private void Update()
    {
        BulletMotionless();
    }

    private void BulletMotionless()
    {
        if (bullet != null && !InputReader.instance.canRecall)
        {
            bullet.transform.position = gameObject.transform.position;
        }
    }

    private void CanChangeBool()
    {
        if (!isActive)
        {
            isActive = true;
            leverAnimator.SetInteger("State", 1);
        }
        else
        {
            isActive = false;
            leverAnimator.SetInteger("State", 0);
        }
    }

    private void OnBulletHit()
    {
        bullet = FindObjectOfType<BulletCollisionDetection>().gameObject;
     
        if (bullet == null)
        {
            return;
        }
        
        _bulletCollisionDetection = bullet.GetComponent<BulletCollisionDetection>();
        _bulletCollisionDetection.enabled = false;

        CanChangeBool();
        _door.OpenDoor(isActive);
    }
}