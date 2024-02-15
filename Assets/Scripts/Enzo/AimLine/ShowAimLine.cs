using UnityEngine;

public class ShowAimLine : MonoBehaviour
{
    [Header("À set up")] 
    [SerializeField] private LineRenderer aimLineRenderer;

    [SerializeField] private Transform playerTransform;

    private void Update()
    {
        ShowLine();
    }

    private void ShowLine()
    {
        if (InputReader.instance.activateAim)
        {
            //récupération de la position de la souris pour que le line renderer suive sa position
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //fait apparaître le line renderer, la première position est le point de départ du line et la seconde position est le point de fin du  line
            aimLineRenderer.enabled = true;
            aimLineRenderer.positionCount = 2;
            aimLineRenderer.SetPosition(0, playerTransform.position);
            aimLineRenderer.SetPosition(1, mousePosition);
        }
        else
        {
            aimLineRenderer.enabled = false;
        }

        /*if (InputReader.instance.leftClick)
        {
            //lorsque le joueur tire, le line disparaît
            InputReader.instance.activateAim = false;
        }*/
    }
}