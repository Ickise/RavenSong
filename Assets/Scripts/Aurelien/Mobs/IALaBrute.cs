public class IALaBrute : IA
{
    protected override void StateManager()
    {
        if (!IsGrounded) return;
        Walking();
        DetectionPlayer();
    }

    private void Walking()
    {
        if (RaycastHitWall || !RaycastDetectNotVoid)
            if (speedMovement == speedBalader)
                direction = !direction;
            else
                return;
        RunToDirection();
    }

    private void DetectionPlayer()
    {
        if (DetectPlayer)
        {
            direction = transform.position.x < player.position.x;
            speedMovement = speedAttaquePlayer;
        }
        else
            speedMovement = speedBalader;
    }
}
