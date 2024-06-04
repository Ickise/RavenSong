using UnityEngine;
using Cinemachine;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using UnityEngine.InputSystem;

public class CameraCinematic : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private CameraPointMovement cam;
    [SerializeField] private NoiseSettings noneSettings;
    private PlayerInput playerInput;
    private CinemachineBasicMultiChannelPerlin camComponent;
    private int index;
    [SerializeField] private GameObject SuivantActive;

    private void Start()
    {
        camComponent = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    [System.Serializable]
    public struct evenement
    {
        public float time;
        public Transform moveTo;
        public Ease ease;
        public NoiseSettings noiseSettings;
    }

    [SerializeField] private List<evenement> evenements = new List<evenement>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Events());
            GetComponent<Collider2D>().enabled = false;
            playerInput = other.GetComponent<PlayerInput>();
            playerInput.enabled = false;
            cam.enabled = false;
        }
    }

    private IEnumerator Events()
    {
        if (evenements[index].moveTo != null)
            cam.transform.DOMove(evenements[index].moveTo.position, evenements[index].time).SetEase(evenements[index].ease);
        if (evenements[index].noiseSettings != null)
            camComponent.m_NoiseProfile = evenements[index].noiseSettings;
        else
            camComponent.m_NoiseProfile = noneSettings;
        yield return new WaitForSeconds(evenements[index].time);
        index++;
        if (index >= evenements.Count)
        {
            cam.enabled = true;
            cam.transform.localPosition = Vector2.zero;
            camComponent.m_NoiseProfile = noneSettings;
            playerInput.enabled = true;
            if (SuivantActive != null) SuivantActive.SetActive(true);
            yield break;
        }
        StartCoroutine(Events());
    }
}
