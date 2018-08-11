using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnTouch : MonoBehaviour {

    public ParticleSystem KillEffect;

    private void Start()
    {
        KillEffect.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "enemy")
        {
            KillEffect.Play();
            Destroy(other.gameObject);
        }
    }

}
