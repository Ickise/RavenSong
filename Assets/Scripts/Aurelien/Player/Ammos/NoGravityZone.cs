using System.Collections.Generic;
using UnityEngine;

public class NoGravityZone : MonoBehaviour
{
    [SerializeField] private float forceGravityImpulse, torqueGravityImpulse;
    protected Dictionary<Rigidbody2D, float> elementNoGravity = new Dictionary<Rigidbody2D, float>();

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D rb2DObj = other.GetComponent<Rigidbody2D>();
        if (rb2DObj == null) return;
        rb2DObj.bodyType = RigidbodyType2D.Dynamic;
        elementNoGravity.TryAdd(rb2DObj, rb2DObj.gravityScale);
        rb2DObj.gravityScale = 0f;
        rb2DObj.AddForce(Vector2.up * forceGravityImpulse, ForceMode2D.Force);
        rb2DObj.AddTorque(Random.Range(-torqueGravityImpulse, torqueGravityImpulse) * Mathf.Deg2Rad, ForceMode2D.Force);
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        Rigidbody2D rb2DObj = other.GetComponent<Rigidbody2D>();
        if (rb2DObj == null) return;
        float otherGravityScale;
        if (elementNoGravity.TryGetValue(rb2DObj, out otherGravityScale))
            rb2DObj.gravityScale = otherGravityScale;
        else return;
        elementNoGravity.Remove(rb2DObj);
    }
}
