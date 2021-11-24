using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DissolveController : MonoBehaviour
{
    [SerializeField]
    private List<Renderer> dissolveRenderers;
    [SerializeField]
    private float initialDissolve = -1.5f;
    [SerializeField]
    private float targetDissolve = 2f;

    private string dissolveKey = "_DissolveValue";
    void Start()
    {
        StartCoroutine(DoDissolve());
    }

    IEnumerator DoDissolve()
    {
        float dissolveVal = initialDissolve;
        var endOfFrame = new WaitForEndOfFrame();
        var dissolveMaterials = dissolveRenderers
            .Select(renderer => renderer.material)
            .ToList();
        while (dissolveVal <= targetDissolve)
        {
            dissolveVal += Time.deltaTime;
            yield return endOfFrame;
            dissolveMaterials.ForEach(mat => mat.SetFloat(dissolveKey, dissolveVal));
        }
    }
}
