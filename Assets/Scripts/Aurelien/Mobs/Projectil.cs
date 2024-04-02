using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Projectil : MonoBehaviour
{
    [SerializeField] private float speedProjectil = 5f, maxHeight = 8f;
    [SerializeField] private LayerMask layerDestroyProjectil;
    private List<Vector2> velocitys = new List<Vector2>();
    public Vector2 playerPos { get; set; }
    private Vector2 directionEndTween, velocity, lastPosition;
    private bool rushEnd, calculeDir;

    private void Start()
    {
        lastPosition = transform.position;
        Vector2 startPos = transform.position;
        transform.DOJump(playerPos, maxHeight, 1, speedProjectil)
        .SetEase(Ease.Linear)
        .SetSpeedBased(true)
        .OnComplete(() =>
        {
            Vector2 total = new Vector2(velocitys.Average(x => x.x), velocitys.Average(x => x.y));
            velocity = total.normalized * speedProjectil;
            rushEnd = true;
            calculeDir = false;
        });
        float a = 0;
        DOTween.To(() => a, x => a = x, 1, speedProjectil * 0.6f).SetSpeedBased(true)
        .OnComplete(() => calculeDir = true);
        Destroy(gameObject, 10);
    }

    private void FixedUpdate()
    {
        if (calculeDir)
        {
            velocity = (Vector2)transform.position - lastPosition;
            lastPosition = transform.position;
            velocitys.Add(velocity);
        }
        else if (rushEnd)
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
