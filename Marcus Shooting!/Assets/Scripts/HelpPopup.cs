using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;  // ����UI�����ռ�

public class HelpPopup : MonoBehaviour
{
    public GameObject helpPanel;

    void Start()
    {
        helpPanel.SetActive(false); 
    }

    public void ToggleHelp()
    {
        helpPanel.SetActive(!helpPanel.activeSelf);
        Time.timeScale = helpPanel.activeSelf ? 0 : 1; //stop the game from running while the help panel is open
                                                       // ȡ����ť����
        EventSystem.current.SetSelectedGameObject(null);
    }
}
