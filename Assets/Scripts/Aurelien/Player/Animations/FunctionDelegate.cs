using Spine;
using UnityEngine;

public class FunctionDelegate : MonoBehaviour
{
    public delegate void DelegateFunction(TrackEntry trackEntry);
    public static DelegateFunction delegateFunction;
}
