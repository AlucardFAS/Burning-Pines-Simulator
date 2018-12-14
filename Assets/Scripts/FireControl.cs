using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireControl : MonoBehaviour
{

    public MeshFilter thisTree;
    public Vector3 positionColl;
    public List<ParticleCollisionEvent> collisionEvents;
    public int colisionNumber;
    public bool burningTree;
    public ParticleSystem ParticleSystemEffectBase;
    public ParticleSystem ThisFireEffect;
    public Vector3 InitialFire;
    public List<TreeNode> Vertices;
    private float waitTime = 0.5f;
    private float timer = 0.0f;

    void Start()
    {
        thisTree = GetComponent<MeshFilter>();
        collisionEvents = new List<ParticleCollisionEvent>();
        Vertices = thisTree.mesh.vertices.Select((v, i) => new TreeNode() { vertice = transform.TransformPoint(v), isBurning = false, index = i }).ToList();
        var colors = new Color[thisTree.mesh.vertices.Count()];
        for (int i = 0; i < thisTree.mesh.vertices.Count() ; i++)
            colors[i] = new Color(1, 1, 1, 1f);

        thisTree.mesh.colors = colors;
    }

    void OnParticleCollision(GameObject other)
    {
        if (burningTree)
            return;
        ParticleSystem ps = other.GetComponent<ParticleSystem>();
        int numCollisionEvents = ps.GetCollisionEvents(thisTree.gameObject, collisionEvents);

        int i = 0;

        var r = new System.Random();
        var b = r.Next(1, 100);
        bool shouldDestroy = false;
        int acceptColision = 0;

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

            if (ps && collisionEvents[i].colliderComponent.CompareTag("Tree") && b > 85 && (colisionNumber % acceptColision) < 15)
            {
                Vector3 pos = collisionEvents[i].intersection;
                InitialFire = pos;
                var firstFire = FindNearstVector3(pos);
                firstFire.isBurning = true;                
                Burn(thisTree.transform.position, ParticleSystemEffectBase.gameObject);
                burningTree = true;
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
        //var r = new System.Random();
        //var rotation = Quaternion.Euler(new Vector3(0, r.Next(70,110), 0));
        GameObject NewFireParticle = Instantiate(fire, positionColl, Quaternion.identity);
        //var newFire = NewFireParticle.GetComponent<ParticleSystem>();
        //var a = newFire.main;
        //a.maxParticles = 1;
        var ps = NewFireParticle.GetComponent<ParticleSystem>();
        ps.shape.rotation.Set(-90, 0, 0);
        var emission = ps.emission;
        emission.enabled = true;
        ps.Play();
        this.ThisFireEffect = ps;
    }

    private void Update()
    {
        if (!burningTree || Vertices.Count(v => !v.isBurning) == 0)
            return;

        timer += Time.deltaTime;    

        if (timer > waitTime)
        {
            var ver = Vertices.Where(v => v.isBurning).ToArray();
            for (int i = 0; i < 15; i++)
            {
                if (ver.Count() - 1 < i)
                    return;
                var v = ver[i];
                var pos = FindNearstVector3(v.vertice);
                //pos.isBurning = true;
                //Debug.Log(pos.vertice.ToString());
                //Burn(pos.vertice, ParticleSystemEffect.gameObject);
                var tempColors = thisTree.mesh.colors;

                tempColors[pos.index] = new Color(0.25f, 0.25f, 0.25f);
                thisTree.mesh.colors = tempColors;

                pos.isBurning = true;

                var shape = this.ThisFireEffect.shape;
                var main = this.ThisFireEffect.main;

                if (shape.radius > 0.4 || shape.length > 5)
                {
                    shape.radius = 0.8f;
                    shape.length = 5f;
                    main.maxParticles = 100;
                }
                else
                {
                    shape.radius += 0.05f;
                    shape.length += 0.2f;
                    main.maxParticles += 5;
                }
            }

            timer = timer - waitTime;
        }
    }

    private TreeNode FindNearstVector3(Vector3 position)
    {
        var near = new TreeNode();
        float mindistance = 0;

        foreach (var v in Vertices.Where(v => !v.isBurning))
        {
            var distance = Vector3.Distance(v.vertice, position);
            if((mindistance > distance) || mindistance == 0)
            {
                distance = mindistance;
                near = v;
            }
                
        }

        return near;
    }

}

public class TreeNode
{
    public Vector3 vertice;
    public bool isBurning;
    public int index;
}