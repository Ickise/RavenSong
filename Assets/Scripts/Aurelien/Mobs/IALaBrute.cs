public class IALaBrute : IA
{
    protected override void StateManager()
    {
        Walking();
        DetectionPlayer();
    }

    private void Walking()
    {
        if (RaycastHitWall || !RaycastDetectNotVoid)
            if (currentSpeedMovement == speedBalader)
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
            currentSpeedMovement = speedAttaquePlayer;
        }
        else
            currentSpeedMovement = speedBalader;
    }
}
