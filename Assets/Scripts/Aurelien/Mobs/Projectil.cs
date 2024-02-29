using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Projectil : MonoBehaviour
{
    [SerializeField] private float timeToRushPlayer = 2f, maxHeight = 8f;
    [SerializeField] private LayerMask layerDestroyProjectil;
    public Vector2 playerPos { get; set; }
    private Vector2 directionEndTween, velocity, lastPosition;
    private bool rushEnd;

    private void Start()
    {
        lastPosition = transform.position;
        Vector2 startPos = transform.position;
        transform.DOJump(playerPos, maxHeight, 1, timeToRushPlayer)
        .SetEase(Ease.Linear)
        .OnComplete(() => rushEnd = true);
        Destroy(gameObject, 10);
    }

    private void FixedUpdate()
    {
        if (!rushEnd)
        {
            velocity = (Vector2)transform.position - lastPosition;
            lastPosition = transform.position;
        }
        else
            transform.Translate(velocity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((layerDestroyProjectil & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
        {
            if (other.CompareTag("Player"))
                other.GetComponent<Respawn>().RespawnPlayer();
            Destroy(gameObject);
        }
    }
}
