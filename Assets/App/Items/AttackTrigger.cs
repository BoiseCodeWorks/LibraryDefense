using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{

    public float Damage;
    public LayerMask ApplyDamageToLayers;
    BoxCollider collider;

    private void Start()
    {
        collider = GetComponent<BoxCollider>();
        collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        var destroyable = other.GetComponent<Destroyable>();

        var shouldAttack = (ApplyDamageToLayers & 1 << other.gameObject.layer) == 1 << other.gameObject.layer;

        if (destroyable && shouldAttack)
        {
            destroyable.TakeDamage(Damage);
        }

    }

    public void EnableTrigger()
    {
        collider.enabled = true;
    }

    public void DisableTrigger()
    {
        collider.enabled = false;
    }



}
