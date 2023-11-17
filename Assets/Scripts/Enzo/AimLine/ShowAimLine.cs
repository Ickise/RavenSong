using UnityEngine;

public class ShowAimLine : MonoBehaviour
{
    [SerializeField] private LineRenderer aimLine;

    [SerializeField] private Transform player;

    private void Update()
    {
        ShowLine();
    }

    private void ShowLine()
    {
        if (InputReader.instance.activateAim)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            aimLine.enabled = true;
            aimLine.positionCount = 2;
            aimLine.SetPosition(0, player.position);
            aimLine.SetPosition(1, mousePosition);
        }
        else 
        {
            aimLine.enabled = false;
        }
        
        if(InputReader.instance.leftClick)
        {
            aimLine.enabled = false;
            InputReader.instance.activateAim = false;
        }
    }
}