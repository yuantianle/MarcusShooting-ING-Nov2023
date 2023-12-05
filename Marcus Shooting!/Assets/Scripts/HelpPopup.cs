using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;  // 引入UI命名空间

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
                                                       // 取消按钮焦点
        EventSystem.current.SetSelectedGameObject(null);
    }
}
