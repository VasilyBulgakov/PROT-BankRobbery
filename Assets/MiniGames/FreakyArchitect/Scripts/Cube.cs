using UnityEngine;
using System.Collections;

public class Cube : MonoBehaviour
{
    public int orderInStructure = 0;
    public ParticleSystem particle = null;

    //public void StartParticle(float destroyAfter = 10f)
    //{
    //    //if (particle != null)
    //    //    return;

    //    CubeType currentType = CubesManager.Instance.ActiveCubeType;
    //    particle = Instantiate(currentType.particleEffect, transform.position, currentType.particleEffect.transform.rotation) as ParticleSystem;

    //    if (!currentType.worldSpaceParticle)
    //    {
    //        particle.transform.SetParent(transform);
    //    }

    //    Destroy(particle.gameObject, destroyAfter);
    //}
    public void StartParticle(ParticleSystem part, float destroyAfter = 10f)
    {
        //if (particle != null)
        //    return;

        //CubeType currentType = CubesManager.Instance.ActiveCubeType;
        particle = Instantiate(part, transform.position, part.transform.rotation) as ParticleSystem;

        //if (!currentType.worldSpaceParticle)
        //{
        //    particle.transform.SetParent(transform);
        //}

        Destroy(particle.gameObject, destroyAfter);
    }

}
