using UnityEngine;

public class Roll : MonoBehaviour
{
  [Header("À set up")] 
  [SerializeField] private float coolDownToRoll;
  [SerializeField] private float timeToGetRoll;
  [SerializeField] private float speed;
  [SerializeField] private float rollDistance;

  private void Update()
  {
    CanRoll();
  }

  private void CanRoll()
  {
    timeToGetRoll += Time.deltaTime;

    if (timeToGetRoll >= coolDownToRoll)
    {
      Debug.Log("do rool");
      timeToGetRoll = 0;
    }
  }
}
