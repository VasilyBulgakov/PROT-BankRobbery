using UnityEngine;
using System.Collections;

[System.Serializable]
public struct CubeType
{
    public string name;
    public Material material;
    public ParticleSystem particleEffect;
    public bool worldSpaceParticle;
}

public class CubesManager : MonoBehaviour
{
    public static CubesManager Instance;

    public CubeType[] cubeTypes;

    public CubeType ActiveCubeType
    {
        get
        { 
            return cubeTypes[ActiveCubeTypeIndex];
        }
    }

    public int ActiveCubeTypeIndex
    {
        get
        {
            return PlayerPrefs.GetInt(ACTIVE_CUBETYPE_KEY, 0);
        }
        set
        {
            if (value < 0)
                value = cubeTypes.Length - 1;
            else if (value >= cubeTypes.Length)
                value = 0;

            PlayerPrefs.SetInt(ACTIVE_CUBETYPE_KEY, value);     
        }
    }

    const string ACTIVE_CUBETYPE_KEY = "ACTIVE_CUBETYPE";

    void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
