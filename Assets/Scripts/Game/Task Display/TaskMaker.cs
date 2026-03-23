using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TaskMaker : MonoBehaviour
{
    [Serializable]
    public class Task
    {
        [Tooltip("Task name. Please name!")]
        public string name;

        public string taskDiscription;
        public bool completed;
    }

    public static event Action<Task[]> ShowTask;
    public static event Action<string> OnTaskComplete;
    public Func<string,bool> OnCompleteTask;

    [SerializeField] private Task[] Tasks;

    [Tooltip("Must the tasks be completed in order?")]
    [SerializeField] private bool taskOrder = true;

    private bool tasksCreated = false;

    private void OnEnable()
    {
        OnCompleteTask += TaskCompleted;
    }
    private void OnDisable()
    {
        OnCompleteTask -= TaskCompleted;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (tasksCreated) return;

        bool player = collision.gameObject.CompareTag("Player");
        if (player)
        {
            if(Tasks.Length == 0)
            {
                Debug.LogError("No tasks found!");
                return;
            }
            tasksCreated = true;
            ShowTask?.Invoke(Tasks);
        }
    }
    private bool TaskCompleted(string taskName)
    {
        if (tasksCreated == false) return false;

        if(Tasks.Length > 0)
        {
            bool found = false;

            for(int i = 0; i < Tasks.Length; i++)
            {
                if(i > 0 && taskOrder)
                {
                    Task prevTask = Tasks[i - 1];
                    if(prevTask.completed == false)
                    {
                        return false;
                    }
                }

                Task task = Tasks[i];
                
                if (task.name == taskName)
                {
                    task.completed = true;
                    OnTaskComplete?.Invoke(taskName);
                    found = true;
                    break;
                }
            }

            if (found == false)
            {
                Debug.LogWarning($"Uhm. This name task nme '{taskName}' is not found in task maker!. Please make sure that task names are accurate!. You can copy and paste.");
            }

            return found;
        }

        return false;
    }
}
