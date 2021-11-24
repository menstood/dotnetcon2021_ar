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
    [SerializeField]
    private ParticleSystem spawnEffect;
    [SerializeField]
    private float  particleStartLifeTime;
    [SerializeField]
    private float particleEndLifeTime;


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
            var mainModule = spawnEffect.main;
            var lifeTime = mainModule.startLifetime;
            lifeTime.constant = Remap(initialDissolve,targetDissolve, particleStartLifeTime, particleEndLifeTime, dissolveVal);
            mainModule.startLifetime = lifeTime;
        }
        spawnEffect.gameObject.SetActive(false);
    }

    private float Remap(float inMin,float inMax, float outMin,float outMax,float val)
    {
        var t = Mathf.InverseLerp(inMin,inMax,val);
           return Mathf.Lerp(outMin,outMax,t);
    }
}
