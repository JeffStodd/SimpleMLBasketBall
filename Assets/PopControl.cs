using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopControl : MonoBehaviour {

    public GameObject prefab;

    List<AI> population = new List<AI>();

    public int populationSize = 100;
    public float percentSurvivors = 0.20f;
    public int generation = 0;

    public Transform spawn;

    public Text generationCount;

    public float totalTime = 0;
    public Text time;
    public Text timeScaleDisplay;

    public Text avgFitness;
    public Text percentSuccess;
    public float fitness = 0;
    public float success = 0;

    public Slider timeSlider;
    public float timeScale = 1;

    // Use this for initialization
    void Start () {
        InitPopulation();
	}
	
	// Update is called once per frame
	void Update () {
        totalTime += Time.deltaTime;
        time.text = "Time Elapsed: " + totalTime;

        Time.timeScale = timeSlider.value;
        timeScale = Time.timeScale;
        timeScaleDisplay.text = "Time Scale: " + Mathf.RoundToInt(timeSlider.value);

        avgFitness.text = "Average Fitness: " + fitness;
        percentSuccess.text = "Percent Succeeding: " + success;

        if (!computing())
        {
            NextGeneration();
            generation++;
            generationCount.text = "Generation: " + generation;
        }
	}

    public void InitPopulation()
    {
        for (int i = 0; i < populationSize; i++)
        {
            GameObject subject = Instantiate(prefab, spawn.position, Quaternion.identity);
            subject.GetComponent<AI>().InitAI(new DNA());
            population.Add(subject.GetComponent<AI>());
        }
    }

    void NextGeneration()
    {
        int maxIndex = Mathf.RoundToInt(populationSize * percentSurvivors);
        List<AI> survivors = new List<AI>();
        for (int i = 0; i < maxIndex; i++)
        {
            survivors.Add(GetFittest());
        }
        for (int i = 0; i < population.Count; i++)
        {
            Destroy(population[i].gameObject);
        }
        population.Clear();

        int mateCutoff = Mathf.RoundToInt(populationSize * percentSurvivors);

        while (population.Count < populationSize)
        {
            for (int i = 0; i < survivors.Count; i++)
            {
                GameObject subject = Instantiate(prefab, spawn.position, Quaternion.identity);
                subject.GetComponent<AI>().InitAI(new DNA(survivors[Random.Range(0, mateCutoff - 1)].dna, survivors[Random.Range(0, mateCutoff/2)].dna));
                if (Random.Range(0, 1) < 0.20) //mutation rate
                    subject.GetComponent<AI>().dna.mutate();
                population.Add(subject.GetComponent<AI>());
                if (population.Count >= populationSize)
                {
                    break;
                }
            }
        }
        for (int i = 0; i < survivors.Count; i++)
        {
            Destroy(survivors[i].gameObject);
        }
    }

    AI GetFittest()
    {
        float maxFitness = float.MinValue;
        int index = 0;
        for (int i = 0; i < population.Count; i++)
        {
            if (population[i].fitness > maxFitness)
            {
                maxFitness = population[i].fitness;
                index = i;
            }
        }
        AI fittest = population[index];
        population.Remove(fittest);
        return fittest;
    }

    bool computing()
    {
        for (int i = 0; i < population.Count; i++)
        {
            if (population[i].computing)
            {
                return true;
            }
        }
        fitness = calcTotalFitness();
        success = calcTotalSuccess();
        return false;
    }

    float calcTotalFitness()
    {
        float sum = 0f;
        for (int i = 0; i< population.Count; i++)
        {
            sum += population[i].fitness;
        }
        return sum/population.Count;
    }

    float calcTotalSuccess()
    {
        float sum = 0f;
        for (int i = 0; i< population.Count; i++)
        {
            //Debug.Log(population[i].success);
            if (population[i].success)
            {
                sum++;
                //Debug.Log(sum);
            }
        }
        return sum/population.Count;
    }
}
