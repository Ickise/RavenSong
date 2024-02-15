using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private Door[] _door;

    public bool isActive;

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

    private void OnBulletHit(GameObject bullet)
    {
        CanChangeBool();
        foreach (var gameObject in _door)
        {
            gameObject.OpenDoor(isActive);
        }
    }
}