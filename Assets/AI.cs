using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour {
    public float fitness; 
    public DNA dna;
    public bool success = false;

    public ArrayList totalDistances;
    public float distance;
    public float deltaDistance;
    public float currentDistance;

    public Rigidbody rb;

    public bool computing = true;

    public Transform spawn;

    public float startingDistance;
    private bool waiting = true;

	// Use this for initialization
	void Start () {
        
        startingDistance = Vector3.Distance(transform.position, GameObject.FindWithTag("Goal").transform.position);
        totalDistances = new ArrayList();

        rb.AddForce(dna.x, dna.y, 0);

        StartCoroutine(Simulate());
        
    }

    // Update is called once per frame
    void Update() {
        if (waiting == true)
        {
            float temp = Vector3.Distance(transform.position, GameObject.FindWithTag("Goal").transform.position);
            currentDistance = temp;
            //Debug.Log("Distance: " + temp);
            totalDistances.Add(temp);

            if (currentDistance < 0.4f)
            {
                success = true;
            }
        }

	}

    IEnumerator Simulate()
    {
        yield return new WaitForSeconds(5);
        waiting = false;

        distance = int.MaxValue;
        foreach (float temp in totalDistances)
        {
            if (temp < distance)
            {
                distance = temp;
            }
        }

        deltaDistance = (startingDistance - distance);
        //fix fitness algorithm
        fitness =  Mathf.Pow((deltaDistance),3)/Mathf.Sqrt((Mathf.Pow(dna.x, 2) + Mathf.Pow(dna.y, 2)));
        //fitness = deltaDistance;
        if (success)
            fitness *= 2f;
        computing = false;
    }

    public void InitAI(DNA _dna)
    {
        dna = _dna;
    }

    
}
