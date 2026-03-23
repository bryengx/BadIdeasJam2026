using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;

public class TaskUI : MonoBehaviour
{
    [SerializeField] private GameObject taskDisplayPrefab;
    [SerializeField] private GameObject tasksCompletedText;
    [SerializeField] private RectTransform contentParent;
    [SerializeField] private CanvasGroup canvGroup;

    private List<GameObject> unCompletedTasks =  new List<GameObject>();
    private List<GameObject> completedTasks = new List<GameObject>();

    private void OnEnable()
    {
        TaskMaker.ShowTask += ShowTasks;
        TaskMaker.OnTaskComplete += TaskCompleted;
    }
    private void OnDisable()
    {
        TaskMaker.ShowTask -= ShowTasks;
        TaskMaker.OnTaskComplete -= TaskCompleted;
    }
    private void Awake()
    {
        canvGroup.alpha = 0f;
    }
    private void ShowTasks(TaskMaker.Task[] tasks)
    {
        canvGroup.alpha = 1f;

        foreach (TaskMaker.Task task in tasks)
        {
            GameObject taskDisplay = Instantiate(taskDisplayPrefab, contentParent);
            taskDisplay.SetActive(true);

            //Note to anyone making tasks:Task text should be the first child of the task display prefab!
            taskDisplay.name = task.name;
            taskDisplay.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = task.taskDiscription;

            unCompletedTasks.Add(taskDisplay);
        }
    }

    private void TaskCompleted(string taskName)
    {
        GameObject task = unCompletedTasks.Find( x => x.name == taskName);

        if(task == null) return;

        //Note to anyone making tasks:Task tick should be the last child of the task display prefab!
        task.transform.GetChild(task.transform.childCount - 1).gameObject.SetActive(true);

        completedTasks.Add(task);
        unCompletedTasks.Remove(task);

        //Tasks finished
        if(unCompletedTasks.Count == 0)
        {
            StartCoroutine(HideTask());
        }
    }
    private IEnumerator HideTask()
    {
        Debug.Log("<color=green>Tasks completed!</color>");

        yield return new WaitForSeconds(2f);

        for (int i = 0; i < completedTasks.Count; i++)
        {
            Destroy(completedTasks[i]);
        }
        unCompletedTasks = new List<GameObject>();
        tasksCompletedText.SetActive(true);

        yield return new WaitForSeconds(3f);

        canvGroup.alpha = 0;
        tasksCompletedText.SetActive(false);
    }

}
