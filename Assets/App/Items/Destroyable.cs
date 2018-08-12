using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public float Health;
    public ParticleSystem BasicDamage;
    public ParticleSystem DeathParticle;

    private void Start()
    {
        BasicDamage.Stop();
        DeathParticle.Stop();
    }


    public void TakeDamage(float damage)
    {
        Health -= damage;
        BasicDamage.Stop();
        BasicDamage.Play();
        if(Health <= 0)
        {
            DeathParticle.Play();
            Destroy(gameObject, DeathParticle.main.duration);
        }

    }
}
