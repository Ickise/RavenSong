using UnityEngine;

public class Chandelier : MonoBehaviour
{
    private string tagOfOther;

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
            case "Bullet":
                Destroy(gameObject);
                break;
            case "Player":
                Destroy(gameObject);
                break;
        }
    }
}