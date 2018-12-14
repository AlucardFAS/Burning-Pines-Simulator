using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireControl : MonoBehaviour
{

    public MeshFilter thisTree;
    public Renderer thisRenderer;
    public List<ParticleCollisionEvent> collisionEvents;
    public int colisionNumber;
    public bool burningTree;
    public bool emittingParticles;
    public ParticleSystem ParticleSystemEffectBase;
    public ParticleSystem ParticleSystemEmitterBase;
    public ParticleSystem ThisFireEffect;
    public ParticleSystem ThisFireEmission;
    public List<TreeNode> Vertices;
    public Color BurnedTreeColor = new Color(0, 0, 0, 1);
    public Color NormalTreeColor = new Color(1, 1, 1, 1);
    public float BurnedPercentage;
    private float waitTime = 0.1f;
    private float timer = 0.0f;
    

    void Start()
    {
        thisTree = GetComponent<MeshFilter>();
        thisRenderer = thisTree.GetComponent<Renderer>();
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
                var firstFire = FindNearstVector3(pos);
                firstFire.isBurning = true;    
                this.ThisFireEffect = Burn(thisTree.transform.position, ParticleSystemEffectBase.gameObject);
                burningTree = true;
                thisTree.GetComponent<MeshCollider>().enabled = false;
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

    ParticleSystem Burn(Vector3 positionColl, GameObject fire)
    {
        GameObject NewFireParticle = Instantiate(fire, positionColl, fire.transform.rotation);
        var ps = NewFireParticle.GetComponent<ParticleSystem>();
        var emission = ps.emission;
        emission.enabled = true;
        ps.Play();
        return ps;
    }

    private void Update()
    {
        if (!burningTree || Vertices.Count(v => v.isBurning && v.burnedPercetage < 1) == 0)
            return;

        timer += Time.deltaTime;

        if (timer > waitTime)
        {
            var ver = Vertices.Where(v => v.isBurning && v.burnedPercetage < 1).ToArray();
            if (ver.Count() > 0) {
                var tempColors = thisTree.mesh.colors;
                for (int i = 0; i < 40; i++)
                {
                    if (ver.Count() - 1 < i)
                        return;
                    var v = ver[i];
                    v.burnedPercetage += 0.5f;
                    var pos = FindNearstVector3(v.vertice);

                    var newColor = Color.Lerp(NormalTreeColor, BurnedTreeColor, v.burnedPercetage);
                    tempColors[pos.index] = newColor;
                    Debug.Log("Burned " + v.burnedPercetage + "  Color  " + newColor.ToString());

                    pos.isBurning = true;

                    var shape = this.ThisFireEffect.shape;
                    var main = this.ThisFireEffect.main;

                    if (shape.radius > 0.4 || shape.length > 5)
                    {
                        shape.radius = 1f;
                        shape.length = 5f;
                        main.maxParticles = 120;
                    }
                    else
                    {
                        shape.radius += 0.05f;
                        shape.length += 0.2f;
                        main.maxParticles += 5;
                    }

                }
                thisTree.mesh.colors = tempColors;
                this.BurnedPercentage = (float)Vertices.Count(v => v.isBurning) / (float)Vertices.Count();

            }
            if (!emittingParticles && this.BurnedPercentage > 0.6)
            {
                this.ThisFireEmission = Burn(thisTree.transform.position, ParticleSystemEmitterBase.gameObject);
                emittingParticles = true;
            }

            //thisRenderer.material.SetColor("_Color", Color.Lerp(NormalTreeColor, BurnedTreeColor, this.BurnedPercentage ));
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
    public float burnedPercetage;
}