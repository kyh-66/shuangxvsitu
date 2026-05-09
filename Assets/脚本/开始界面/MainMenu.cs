using UnityEngine;
using UnityEngine.SceneManagement; // 必须引用这个，才能切换场景

public class MainMenu : MonoBehaviour
{
    // --- 开始游戏 ---
    public void PlayGame()
    {
        // 这里填写你游戏关卡的场景名字，看你之前的截图是 "SampleScene"
        // 如果你的场景改名了，记得这里也要改
        SceneManager.LoadScene("Level 1");

        // 或者使用索引加载（需要在Build Settings里排在第2个）
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // --- 退出游戏 ---
    public void QuitGame()
    {
        Debug.Log("游戏已退出！(在编辑器里看不到效果是正常的)");

        // 这行代码只有打包成exe后才有效
        Application.Quit();

        // 加上这一段，让你在Unity编辑器里点退出也能看到停止运行的效果
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}