using System.Collections;
using UnityEngine;

public class ShaderBrillance : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    private IEnumerator Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        yield return new WaitForSeconds(1);
        for (int i = 0; i < meshRenderer.materials.Length; i++)
            meshRenderer.materials[i].SetFloat("_FlashAmount", 0.5f);
    }
}
