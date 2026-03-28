using System;
using UnityEngine;
using UnityEngine.Events;
public class DialogOnTrigger : MonoBehaviour
{
    [Tooltip("Must dialog be triggered by on collide?")]
    [SerializeField] private bool ontriggerEnterDialog = true;

    [SerializeField] Dialog.DialogInfo[] dialogs;
    [SerializeField] bool isHighPriority = false;

    [Tooltip("Should the on end dialog event fire even if the dialog unsucssful?")]
    [SerializeField] bool alwaysCallOnEndFunction = true;

    [SerializeField] private UnityEvent onDialogBegin;
    [SerializeField] private UnityEvent onDialogFinish;

    private bool debounce = false;
    private void OnValidate()
    {
        if (dialogs.Length > 2)
        {
            Array.Resize(ref dialogs, 2);
            Debug.LogWarning("2 arrays only!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ontriggerEnterDialog == false) return;
        bool player = collision.gameObject.CompareTag("Player");
        if (player && debounce == false)
        {
            debounce = true;
            TriggerDialog();
        }
    }

    public void TriggerDialog()
    {
        Dialog.CallDialog?.Invoke(dialogs, isHighPriority, OnDialogBegin, OnDialogEnd, alwaysCallOnEndFunction);
    }

    private void OnDialogBegin()
    {
        onDialogBegin?.Invoke();
    }
    private void OnDialogEnd()
    {
        onDialogFinish?.Invoke();
    }
}
