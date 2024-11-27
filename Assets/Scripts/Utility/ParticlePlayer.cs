using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePlayer : MonoBehaviour
{

    private ParticleSystem[] _allParticles;
    // Start is called before the first frame update
    void Start()
    {
        _allParticles = GetComponentsInChildren<ParticleSystem>();
    }

    public void Play()
    {
        foreach(ParticleSystem ps in _allParticles)
        {
            ps.Stop();
            ps.Play();
        }
    }

}
