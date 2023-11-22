using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Projectil : MonoBehaviour
{
    [SerializeField] private float timeToRushPlayer = 2f, maxHeight = 8f;
    [SerializeField] private LayerMask layerDestroyProjectil;
    private Rigidbody2D rb2D;
    private Vector2 directionEndTween;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        Vector2 startPos = transform.position;
        Vector2 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        transform.DOJump(playerPos, maxHeight, 1, timeToRushPlayer).SetEase(Ease.Linear);
        StartCoroutine(SetAngleRB());
        IEnumerator SetAngleRB()
        {
            yield return new WaitForSeconds(timeToRushPlayer - timeToRushPlayer / 20f);
            Vector2 pos1 = transform.position;
            yield return 0;
            Vector2 pos2 = transform.position;
            rb2D.velocity = Vector2.zero;
            directionEndTween = pos2 - pos1;
            transform.DOBlendableMoveBy(directionEndTween * 10000f, 1f / timeToRushPlayer * (40f + Vector2.Distance(startPos, transform.position) / 2f)).SetSpeedBased(true).SetEase(Ease.Linear).SetId("doBlendableMoveBy");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((layerDestroyProjectil & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }
    }
}
