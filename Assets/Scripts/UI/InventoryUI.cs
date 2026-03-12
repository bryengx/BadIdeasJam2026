using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;
    public InventorySlotUI slotPrefab;
    public Transform slotParent;

    List<InventorySlotUI> slotUIs = new List<InventorySlotUI>();
    private void Awake()
    {
        instance = this;
    }

    public void UpdateUI(List<InventorySlot> slots)
    {
        EnsureSlotCount(slots.Count);

        for (int i = 0; i < slotUIs.Count; i++)
        {
            if (i < slots.Count)
                slotUIs[i].SetSlot(slots[i]);
            else
                slotUIs[i].Clear();
        }
    }

    void EnsureSlotCount(int count)
    {
        // Destroy existing UI slots
        for (int i = 0; i < slotUIs.Count; i++)
        {
            if (slotUIs[i] != null)
                Destroy(slotUIs[i].gameObject);
        }

        slotUIs.Clear();

        RectTransform rect = slotParent.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(count * 70f, rect.sizeDelta.y);

        while (slotUIs.Count < count)
        {
            var slot = Instantiate(slotPrefab, slotParent);
            slotUIs.Add(slot);
        }
    }
    public void Show()
    {
        slotParent.gameObject.SetActive(true);
    }
    public void Hide()
    {
        slotParent.gameObject.SetActive(false);
    }
}