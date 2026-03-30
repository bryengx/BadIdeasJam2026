using UnityEngine;

public class OpenDoors : MonoBehaviour
{
    public GameObject[] doors;
    public void OpenAllDoors()
    {
        foreach (var item in doors)
        {
            item.SetActive(false);
        }
    }
}
