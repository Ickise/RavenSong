using System;
using UnityEngine;

public class Chandelier : MonoBehaviour
{
    private OnBulletHit _onBulletHit;

    private string tagOfOther;

    private void Awake()
    {
        _onBulletHit = GetComponent<OnBulletHit>();
    }

    private void Start()
    {
        _onBulletHit.onBulletHit.AddListener(OnBulletHit);
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