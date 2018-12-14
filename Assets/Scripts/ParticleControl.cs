using UnityEngine;

public class ParticleControl : MonoBehaviour {

    public ParticleSystem fire;
    public WindZone windZone;
    public float windInFire;
	// Use this for initialization
	void Start () {
        fire = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //var fo = fire.forceOverLifetime;
        //fo.enabled = true;

        //windInFire = windZone.windTurbulence;

        //while(windInFire >= 1.5f)
        //{
        //    windInFire /= 1.5f;
        //}

        //fo.x = fo.y = -windInFire;
    }
}
