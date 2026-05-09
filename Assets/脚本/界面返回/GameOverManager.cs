using UnityEngine;
using UnityEngine.SceneManagement; // 必须引用场景管理

public class GameOverManager : MonoBehaviour
{
    // 这个方法给“返回主界面”按钮调用
    public void GoBackToMainMenu()
    {
        // 这里的名字必须和你 Build Settings 里的主界面名字完全一致
        // 根据你的截图，名字应该是 "MainMenu"
        SceneManager.LoadScene("MainMenu");
    }
}