using System;
using System.Collections;
using UnityEngine;

public class CompleteTask : MonoBehaviour
{
    [SerializeField] private TaskMaker taskMaker;
    [SerializeField] private string taskName;

    [Tooltip("Should this script trigger complete task when player is close?")]
    [SerializeField] private bool onEnterTaskComplete = true;

    [Tooltip("If trigger by distance, at what distance will this trigger task complete. Dont change this on runtime, it wont work lol")]
    [SerializeField] private float triggerDistance = 5f;
    private float sqrDistanc;

    private GameObject player;

    private bool taskCompleted;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        sqrDistanc = triggerDistance * triggerDistance;

        StartCoroutine(CheckDistance());
    }
    private IEnumerator CheckDistance()
    {
        while (taskCompleted == false)
        {
            if(player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
            else
            {
                Vector3 offset = player.transform.position - transform.position;
                float distance = offset.sqrMagnitude;
                if(distance <= sqrDistanc)
                {
                    TaskComplete();
                }
            }

            yield return null;
        }
    }
    //Also called by other classes for specific actioned task complte
    public void TaskComplete()
    {
        if(taskMaker != null)
            taskCompleted = taskMaker.OnCompleteTask.Invoke(taskName);
    }
    private void OnDrawGizmos()
    {
        if (onEnterTaskComplete)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, triggerDistance);
        }
    }
}
