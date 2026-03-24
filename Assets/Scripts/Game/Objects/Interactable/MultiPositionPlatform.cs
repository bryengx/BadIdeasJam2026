using UnityEngine;

public class MultiPositionPlatform : MonoBehaviour
{
    public Transform[] positions;
    public float moveSpeed = 2.5f;
    bool isMoving = false;
    Transform currentPoint;
    Transform nextPoint;
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isMoving)
        {
            if (Vector3.Distance(transform.position, nextPoint.position) > moveSpeed * Time.deltaTime)
            {
                transform.position += (nextPoint.position - transform.position).normalized * moveSpeed * Time.deltaTime;
            }
            else
            {
                currentPoint = nextPoint;
                isMoving = false;
            }
        }
    }
    public void StartMoving(int posIdx)
    {
        nextPoint = positions[posIdx];
        isMoving = true;
    }
}
