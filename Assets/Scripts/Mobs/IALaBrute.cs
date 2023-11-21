using UnityEngine;

public class IALaBrute : IA
{
    protected override void StateManager()
    {
        Walking();
        DetectionPlayer();
    }

    private void Walking()
    {
        if (IsGrounded && (RaycastHitWall || !RaycastDetectNotVoid))
            if (speedMovement == speedBalader)
                direction = !direction;
            else
                return;
        RunToDirection();
    }

    private void DetectionPlayer()
    {
        if (DetectPlayerY)
        {
            direction = transform.position.x < player.position.x;
            speedMovement = speedAttaquePlayer;
        }
        if (!RaycastDetectPlayer || !RaycastDetectPlayer.transform.CompareTag("Player"))
            speedMovement = speedBalader;
    }
}
