using UnityEngine;

public class TP : MonoBehaviour
{
    public GameObject _player;
    public GameObject _pointTP;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == _player.tag)
        {
            _player.gameObject.transform.position = _pointTP.transform.position;
        }
    }
}
