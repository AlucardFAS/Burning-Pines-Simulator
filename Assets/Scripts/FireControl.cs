using System.Collections.Generic;
using UnityEngine;

public class FireControl : MonoBehaviour
{

    public MeshFilter thisTree;
    public Vector3 positionColl;
    public List<ParticleCollisionEvent> collisionEvents;
    public int colisionNumber;

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
        var b = r.Next(1, 100);
        bool shouldDestroy = false;
        int acceptColision = 0;

        //Debug.Log("Collisions " + numCollisionEvents);
        if (colisionNumber < 10)
        {
            acceptColision = 3;
        }
        else if (colisionNumber >= 10 && colisionNumber < 100)
        {
            acceptColision = 20;
        }
        else
        {
            acceptColision = 100;
        }


        while (i < numCollisionEvents)
        {
            colisionNumber++;

            Debug.Log("Collisions " + colisionNumber);

            if (ps && collisionEvents[i].colliderComponent.CompareTag("Tree") && b > 85 && (colisionNumber%acceptColision) < 15)
            {
                Vector3 pos = collisionEvents[i].intersection;
                Burn(pos, ps.transform.parent.gameObject);
                //shouldDestroy = true;
            }
            i++;
            numCollisionEvents = 0;
        }

        if (shouldDestroy)
        {
            ps.Stop();
            ps.Clear();
            Destroy(ps);
        }
    }

    void Burn(Vector3 positionColl, GameObject fire)
    {
        var r = new System.Random();
        //var rotation = Quaternion.Euler(new Vector3(0, r.Next(70,110), 0));
        GameObject NewFireParticle = Instantiate(fire, positionColl, Quaternion.identity);
        //var newFire = NewFireParticle.GetComponent<ParticleSystem>();
        //var a = newFire.main;
        //a.maxParticles = 1;
        NewFireParticle.GetComponent<ParticleSystem>().Play();
    }
}