using System.Collections.Generic;
using UnityEngine;

public class FireControl : MonoBehaviour
{

    public MeshFilter thisTree;
    public Vector3 positionColl;
    public int colisions;
    public List<ParticleCollisionEvent> collisionEvents;

    void Start()
    {
        thisTree = GetComponent<MeshFilter>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        ParticleSystem ps = other.GetComponent<ParticleSystem>();
        int numCollisionEvents = ps.GetCollisionEvents(thisTree.gameObject, collisionEvents);

        int i = 0;

        var r = new System.Random();
        var b = r.Next(1, 10);
        bool shouldDestroy = false;

        Debug.Log("Collisions " + numCollisionEvents);

        while (i < numCollisionEvents)
        {
            if (ps && collisionEvents[i].colliderComponent.CompareTag("Tree") && b > 8)
            {
                Vector3 pos = collisionEvents[i].intersection;

                Burn(pos, ps.transform.parent.gameObject);
                shouldDestroy = true;
            }
            i++;
        }
        
        if(shouldDestroy)
        {
            ps.Stop();
            ps.Clear();
            Destroy(ps);
        }
    }

    void Burn(Vector3 positionColl, GameObject fire)
    {
        GameObject NewFireParticle = Instantiate(fire, positionColl, Quaternion.identity);
        //var newFire = NewFireParticle.GetComponent<ParticleSystem>();
        //var a = newFire.main;
        //a.maxParticles = 1;
        NewFireParticle.GetComponent<ParticleSystem>().Play();
    }
}