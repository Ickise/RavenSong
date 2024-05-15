using UnityEngine;

public class TranslationLerp : MonoBehaviour
{
    public GameObject _pointLerpFinish;
    public GameObject _pointLerpStart;
    public GameObject _camera;
    public float _time;
    public bool _start;
    
    // Update is called once per frame
    void Update()
    {
        if(_start)
        {
            _time = _time + Time.deltaTime / 2;
            Vector3 _lerp = Vector3.Lerp(_pointLerpStart.transform.position, _pointLerpFinish.transform.position, _time / 2);
            _camera.transform.position = _lerp;
        }
    }
}
