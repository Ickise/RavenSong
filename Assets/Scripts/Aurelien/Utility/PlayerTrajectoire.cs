using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrajectoire : MonoBehaviour
{
    [SerializeField] private Sprite objectTrajectoire;
    [SerializeField] private float timeToDisappear = 600f, spawnrate = 1f, objectScale = 0.3f;
    [SerializeField] private Color color = Color.cyan;
    private GameObject poubelle;

    void Start()
    {
        poubelle = new GameObject("poubelle");
        StartCoroutine(Trajectoire());
    }

    private IEnumerator Trajectoire()
    {
        GameObject indicator = new GameObject("indicator");
        indicator.transform.position = transform.position;
        indicator.transform.parent = poubelle.transform;
        SpriteRenderer sprite = indicator.AddComponent<SpriteRenderer>();
        sprite.sprite = objectTrajectoire;
        sprite.color = color;
        indicator.transform.localScale = Vector2.one * objectScale;
        Destroy(indicator, timeToDisappear);
        yield return new WaitForSeconds(spawnrate);
        StartCoroutine(Trajectoire());
    }
}
