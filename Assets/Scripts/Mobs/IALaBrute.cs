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
            if (speedMovement == speedRoaming)
                direction = !direction;
            else
                return;
        RunToDirection();
    }

    private void DetectionPlayer()
    {
        if (DetectPlayerX)
        {
            direction = transform.position.x < player.position.x;
            speedMovement = speedChasingPlayer;
        }
        if (DistanceBetweenIAandPlayer > detectPlayerRange || !RaycastDetectPlayer.transform.CompareTag("Player"))
            speedMovement = speedRoaming;
    }
}
