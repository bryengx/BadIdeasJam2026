using UnityEngine;

public class TaskAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip taskShowSFX;
    [SerializeField] private AudioClip taskCheckSFX;
    [SerializeField] private AudioClip taskCompleteSFX;

    private void OnEnable()
    {
        TaskMaker.ShowTask += PlayTaskShowSFX;
        TaskMaker.OnTaskComplete += PlayTaskCheckSFX;
        TaskUI.OnAllTasksFinished += PlayTaskCompleteSFX;
    }

    private void OnDisable()
    {
        TaskMaker.ShowTask -= PlayTaskShowSFX;
        TaskMaker.OnTaskComplete -= PlayTaskCheckSFX;
        TaskUI.OnAllTasksFinished -= PlayTaskCompleteSFX;
    }

    private void PlayTaskShowSFX(TaskMaker.Task[] tasks)
    {
        if (taskShowSFX) audioSource.PlayOneShot(taskShowSFX);
    }

    private void PlayTaskCheckSFX(string name)
    {
        if (taskCheckSFX) audioSource.PlayOneShot(taskCheckSFX);
    }

    private void PlayTaskCompleteSFX()
    {
        if (taskCompleteSFX) audioSource.PlayOneShot(taskCompleteSFX);
    }
}
