using System;
using UnityEngine;
using UnityEngine.Events;

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
        public UnityEvent OnTaskComplete;
    }

    public static event Action<Task[]> ShowTask;
    public static event Action<string> OnTaskComplete;
    public Func<string,bool> OnCompleteTask;

    [SerializeField] private Task[] Tasks;
    [SerializeField] private bool onCollisionTrigger = true;

    [Tooltip("Must the tasks be completed in order?")]
    [SerializeField] private bool taskOrder = true;

    [SerializeField] private UnityEvent OnTasksComplete;

    private bool tasksCreated = false;
    private bool tasksCompleted = false;

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
        if (tasksCreated || onCollisionTrigger == false) return;

        bool player = collision.gameObject.CompareTag("Player");
        if (player)
        {
            ShowTasks();
        }
    }
    public void ShowTasks()
    {
        if (Tasks.Length == 0)
        {
            Debug.LogError("No tasks found!");
            return;
        }
        tasksCreated = true;
        ShowTask?.Invoke(Tasks);
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
                    task.OnTaskComplete?.Invoke();

                    OnTaskComplete?.Invoke(taskName);
                    found = true;

                    CheckTasks();
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

    private void CheckTasks()
    {
        int i = 0;
        foreach (var task in Tasks)
        {
            if (task.completed) i++;
        }
        if(i == Tasks.Length && tasksCompleted == false)
        {
            tasksCompleted = true;
            OnTasksComplete?.Invoke();
        }
    }
}
