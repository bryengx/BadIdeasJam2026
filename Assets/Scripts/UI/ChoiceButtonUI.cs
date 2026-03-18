using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ChoiceButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;
    private Action action;

    public void Init(string text, Action action)
    {
        label.text = text;
        this.action = action;

        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        action?.Invoke();
    }
}
