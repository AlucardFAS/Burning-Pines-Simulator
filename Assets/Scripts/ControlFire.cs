using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[RequireComponent(typeOf(ParticleSystem))]

public class ControlFire : MonoBehaviour
{

    public ParticleSystem part;
    public Vector3 positionColl;
    public List<ParticleCollisionEvent> collisionEvents;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        Rigidbody rb = other.GetComponent<Rigidbody>();
        int i = 0;

        while (i < numCollisionEvents)
        {
            if (rb)
            {
                Vector3 pos = collisionEvents[i].intersection;
                Burn(pos);
            }
            i++;
        }
    }

    void Burn(Vector3 positionColl)
    {
        GameObject NewFireParticle = Instantiate(part.gameObject, positionColl, Quaternion.identity);
        var newFire = NewFireParticle.GetComponent<ParticleSystem>();
        var a = newFire.main;
        a.maxParticles = 1;
        NewFireParticle.GetComponent<ParticleSystem>().Play();

    }
}