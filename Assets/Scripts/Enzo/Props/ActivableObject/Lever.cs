using UnityEngine;

public class Lever : MonoBehaviour
{
    public bool isActive;

    private bool isPlayerInRange;

    private GameObject bullet;

    private Rigidbody2D bulletRigidbody;

    private void Start()
    {
        InputReader.instance.onInteractionEvent.AddListener(OnClick);
    }

    private void Update()
    {
        if (bullet != null)
        {
            bullet.transform.position = gameObject.transform.position;
            bulletRigidbody.velocity = Vector2.zero;
            //si jamais change pas la balle, modifier le code pour que même si la balle est trop rapide, je la récupère
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
        
        if (other.CompareTag("Bullet"))
        {
            bullet = other.gameObject;
            bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
            
            if (!isActive)
            {
                isActive = true;
            }
            else
            {
                isActive = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isPlayerInRange = false;
    }

    private void OnClick()
    {
        if (isPlayerInRange)
        {
            if (!isActive)
            {
                isActive = true;
            }
            else
            {
                isActive = false;
            }
        }
    }
}