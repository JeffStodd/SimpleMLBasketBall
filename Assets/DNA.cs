using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA
{
    public int x, y;

    public DNA()
    {
        x = Mathf.RoundToInt(Random.Range(-1000, 1000));
        y = Mathf.RoundToInt(Random.Range(-1000, 1000));
    }

    public DNA(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    public DNA(DNA parentA, DNA parentB)
    {
        float aOrB = Random.Range(0, 1);
        if (aOrB > 0.5)
        {
            x = parentA.x;
            y = parentB.y;
        }
        else
        {
            x = parentB.x;
            y = parentA.y;
        }
    }

    public void mutate()
    {
        x += Mathf.RoundToInt(x * Random.Range(-0.33f, 0.33f));
        y += Mathf.RoundToInt(y * Random.Range(-0.33f, 0.33f));
    }
}