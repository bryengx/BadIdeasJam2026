using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PushableObject : MonoBehaviour, IInteractable
{
    public Transform[] positions;
    public float moveSpeed = 1;
    bool isMoving = false;
    Transform currentPoint;
    Transform nextPoint;
    void Start()
    {
        Transform closestPoint = positions[0];
        float closestDistance = Vector3.Distance(closestPoint.position, transform.position);
        foreach (Transform t in positions)
        {
            float distance = Vector3.Distance(t.position, transform.position);
            if (closestDistance > distance)
            {
                closestDistance = distance;
                closestPoint = t;
            }
        }
        transform.position = closestPoint.position;
        currentPoint = closestPoint;
    }
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

    public void Interact(PlayerController2D player)
    {
        if (isMoving)
            return;

        bool moveLeft = transform.position.x < player.transform.position.x;

        Transform leftNode = FindLeftNode();
        Transform rightNode = FindRightNode();

        if (moveLeft && leftNode != null)
        {
            StartMoving(leftNode);
        }
        if (!moveLeft && rightNode != null)
        {
            StartMoving(rightNode);
        }

    }
    void StartMoving(Transform target)
    {
        nextPoint = target;
        isMoving = true;
    }
    Transform FindLeftNode()
    {
        List<Transform> leftPoints = positions.Where(x => x != currentPoint && x.transform.position.x < transform.position.x).ToList();
        if(leftPoints.Count > 0)
        {
            leftPoints.Sort((a, b) => b.position.x.CompareTo(a.position.x));
            return leftPoints[0];
        }
        return null;
    }
    Transform FindRightNode()
    {
        List<Transform> rightPoints = positions.Where(x => x != currentPoint &&  x.transform.position.x > transform.position.x).ToList();
        if (rightPoints.Count > 0)
        {
            rightPoints.Sort((a, b) => a.position.x.CompareTo(b.position.x));
            return rightPoints[0];
        }
        return null;
    }
}
