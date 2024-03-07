using UnityEngine;

public interface IFunctionDelegate
{
    delegate void DelegateFunction();
    DelegateFunction delegateFunction { get; set; }
    public void Function();
}
