using System.Collections;
using UnityEngine;

public class Dirt : MonoBehaviour
{
    public enum DirtType { Dishes, Floor}
    public DirtType type;
    public int stain = 3;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => stain <= 0);
        Destroy(gameObject);
    }

}
