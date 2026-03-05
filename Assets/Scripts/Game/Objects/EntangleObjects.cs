using System.Collections.Generic;
using UnityEngine;

public class EntangleObjects : MonoBehaviour
{
    public GameObject[] entangledObjects;
    public Dictionary<GameObject, Vector3> entangledPositions = new();
    Vector3 startingPosition;
    void Start()
    {
        startingPosition = transform.position;
        foreach (var entangledObject in entangledObjects)
        {
            entangledPositions[entangledObject] = entangledObject.transform.position;
        }
    }
    void FixedUpdate()
    {
        Vector3 delta = transform.position - startingPosition;

        foreach (var entangledObject in entangledObjects)
        {
            entangledObject.transform.position = entangledPositions[entangledObject] + delta;
        }
    }
}
