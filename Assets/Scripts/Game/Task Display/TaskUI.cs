using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TaskUI : MonoBehaviour
{
    [SerializeField] private GameObject taskDisplayPrefab;
    [SerializeField] private GameObject tasksCompletedText;
    [SerializeField] private RectTransform contentParent;
    [SerializeField] private CanvasGroup canvGroup;
    [SerializeField] private CanvasGroup toHideOnTab;
    [SerializeField] private TextMeshProUGUI tabToHideText;

    private List<GameObject> unCompletedTasks =  new List<GameObject>();
    private List<GameObject> completedTasks = new List<GameObject>();

    public static System.Action OnAllTasksFinished;

    private bool doingTask = false;
    private bool showingTaskUi = false;

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
    private void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame && doingTask)
        {
            showingTaskUi = !showingTaskUi;
        }
        tabToHideText.text = showingTaskUi ? "Press TAB to hide" : "Press TAB to unhide";
        toHideOnTab.alpha = showingTaskUi ? 1f : 0f ;
    }
    private void ShowTasks(TaskMaker.Task[] tasks)
    {
        showingTaskUi = true;
        doingTask = true;
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

        showingTaskUi = true;

        //Tasks finished
        if (unCompletedTasks.Count == 0)
        {
            StartCoroutine(HideTask());
        }
    }
    public IEnumerator HideTask()
    {
        Debug.Log("<color=green>Tasks completed!</color>");

        doingTask = false;

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < completedTasks.Count; i++)
        {
            Destroy(completedTasks[i]);
        }
        unCompletedTasks = new List<GameObject>();
        tasksCompletedText.SetActive(true);
        OnAllTasksFinished?.Invoke();
        yield return new WaitForSeconds(3f);

        canvGroup.alpha = 0;
        tasksCompletedText.SetActive(false);
    }

}
