
using UnityEngine;

public class WindControl : MonoBehaviour {

    public float turbulation;
    public float pulseMagnitude;
    public float pulseFrequency;
    public Vector3 rotationWind;
    public bool windOscillate;
    public WindZone windZone;

    // Use this for initialization
    void Start () {
        windZone = GetComponent<WindZone>();

        windZone.windMain = 1;
        windZone.windTurbulence = 1;
        windZone.windPulseFrequency = 1;
        windZone.windPulseMagnitude = 1;
        rotationWind.y = 0;
        windOscillate = true;

        windZone.transform.Rotate(0, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
        
        //Random wind attributes
        if (windOscillate == true)
        {
            var randomizer = new System.Random();

            pulseFrequency = randomizer.Next(0, 100);
            pulseMagnitude = randomizer.Next(0, 150);
            turbulation = randomizer.Next(0,150);

            windZone.windTurbulence = turbulation;
            windZone.windPulseFrequency = pulseFrequency;
            windZone.windPulseMagnitude = pulseMagnitude;
        }
    }
}
