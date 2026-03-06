using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    [Serializable]
    public class Text
    {
        [TextArea]
        public string text;
        [HideInInspector] public bool read;
    }
    [Serializable]
    public class SpeakerInfo
    {
        public string name;
        public Text[] text;
    }

    public static Action<string> OnTriggerDialog;

    [SerializeField] private string _id;
    [SerializeField] private GameObject canvas;
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private float msgDuration = 3f;
    [SerializeField] SpeakerInfo[] dialogs;

    private void OnValidate()
    {
        if(dialogs.Length > 2)
        {
            Array.Resize(ref dialogs, 2);
            Debug.LogWarning("2 arrays only!");
        }

        if (_id == string.Empty) Debug.LogWarning("Set dialog id!");
        Dialog[] dialogscripts = GameObject.FindObjectsByType<Dialog>(FindObjectsInactive.Exclude,FindObjectsSortMode.None);
        foreach(Dialog dialog in dialogscripts)
        {
            if (dialog == this) continue;

            if(dialog._id == _id)
            {
                Debug.LogError("Found an instance with the same id as this");
            }
        }
    }
    private void Awake()
    {
        canvas.SetActive(false);
        OnTriggerDialog += StartDialog;
    }
    private void OnDisable()
    {
        OnTriggerDialog -= StartDialog;
    }
    private void Start()
    {
        StartDialog(_id);
    }

    private void StartDialog(string recId)
    {
        if (recId != _id) return;

        canvas.SetActive(true);
        StartCoroutine(ShowDialog());
    }
    private IEnumerator ShowDialog()
    {
        if (dialogs.Length < 2)
        {
            Debug.LogError("Populate dialog!");
        }
        else
        {
            int index = 0;
            while (DialogFinished() == false)
            {
                SpeakerInfo speaker = dialogs[index];

                string txt = string.Empty;
                foreach (Text t in speaker.text)
                {
                    if (t.read)
                    {
                        continue;
                    }
                    t.read = true;
                    txt = speaker.name + ": " + t.text;
                    break;
                }
                if (txt != string.Empty)
                {
                    textMesh.text = txt;
                    yield return new WaitForSeconds(msgDuration);
                }
                index = index == 0 ? 1 : 0;

                yield return null;
            }
        }
        canvas.SetActive(false);
    }
    private bool DialogFinished()
    {
        int finished = 0;

        foreach (SpeakerInfo speaker in dialogs)
        {
            int count = speaker.text.Length;
            int i = 0;

            if(count == 0)
            {
                Debug.LogError("There are no messages found in: " + speaker.name);
                return true;
            }

            foreach (Text t in speaker.text)
            {
                if (t.read) i++;
            }
            if (i == count) finished++;
        }

        return finished == 2;
    }
}
