using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class ChoiceOptionData
{
    public string text;
    public Action action;

    public ChoiceOptionData(string text, Action action)
    {
        this.text = text;
        this.action = action;
    }
}
public class ChoiceWindowUI : MonoBehaviour
{
    public static ChoiceWindowUI instance;

    [SerializeField] private GameObject window;
    [SerializeField] private TextMeshProUGUI statementText;
    [SerializeField] private Transform buttonParent;
    [SerializeField] private ChoiceButtonUI buttonPrefab;

    private List<ChoiceButtonUI> activeButtons = new List<ChoiceButtonUI>();

    private void Awake()
    {
        instance = this;
        window.SetActive(false);
    }

    public void Open(string statement, params ChoiceOptionData[] choices)
    {
        ClearButtons();

        statementText.text = statement;
        window.SetActive(true);

        foreach (var choice in choices)
        {
            ChoiceButtonUI btn = Instantiate(buttonPrefab, buttonParent);

            btn.Init(choice.text, () =>
            {
                choice.action?.Invoke();
                Close();
            });

            activeButtons.Add(btn);
        }
    }

    public void Close()
    {
        window.SetActive(false);
        ClearButtons();
    }

    private void ClearButtons()
    {
        foreach (var btn in activeButtons)
        {
            Destroy(btn.gameObject);
        }

        activeButtons.Clear();
    }
}
