using UnityEngine;

public class Chandelier : MonoBehaviour
{
    [SerializeField] private OnBulletHit onBulletHit;

    private string tagOfOther;

    private void Start()
    {
        onBulletHit.onBulletHit.AddListener(OnBulletHit);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        SwitchTag(other.gameObject.tag);

        if (other.CompareTag("IA"))
        {
            Destroy(other.gameObject);
        }
    }

    private void SwitchTag(string tag)
    {
        switch (tag)
        {
            case "IA":
                Destroy(gameObject);
                break;
            case "Untagged":
                Destroy(gameObject);
                break;
            case "Player":
                Destroy(gameObject);
                break;
        }
    }

    private void OnBulletHit()
    {
        Destroy(gameObject);
    }
}