using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[RequireComponent(typeOf(ParticleSystem))]

public class ControlFire : MonoBehaviour {

	public GameObject FireParticle;
	public Vector3 positionColl;
    
    void OnCollisionEnter(Collision coll) 
    {
		/*if (coll.collider.CompareTag("Plane")) 
		{
			
		}*/
		positionColl = coll.transform.position;
		Burn(positionColl);
	}
	
	void Burn(Vector3 positionColl)
	{
		GameObject NewFireParticle = Instantiate(FireParticle, positionColl, Quaternion.identity);
		NewFireParticle.GetComponent<ParticleSystem>().Play();
	}
}