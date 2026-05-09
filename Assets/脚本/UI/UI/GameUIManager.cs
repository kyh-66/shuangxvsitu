using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;

    [Header("UI 格子设置")]
    public Image[] maskSlots; // 这里只需要填 5 个格子 (冰~最后一个)

    [Header("状态颜色")]
    public Color activeColor = Color.white;        // 选中：全亮
    public Color inactiveColor = new Color(1f, 1f, 1f, 0.5f); // 未选中：半透明
    public Color lockedColor = Color.black;        // 未解锁：全黑

    // 内部记录当前的解锁状态
    private bool[] currentUnlockStatus;

    // ★★★ 核心修改：设置偏移量为 1 ★★★
    // 意思就是：UI 的第 0 个格子，对应数据的第 0+1 个面具 (即面具 ID 1)
    private int maskDataOffset = 1;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    // --- 刷新解锁状态 ---
    public void InitUnlockState(bool[] unlockStatus)
    {
        currentUnlockStatus = unlockStatus;

        for (int i = 0; i < maskSlots.Length; i++)
        {
            // 计算数据对应的 ID：UI 下标 + 1
            int dataIndex = i + maskDataOffset;

            // 防止越界
            if (dataIndex < unlockStatus.Length)
            {
                // 读取对应 ID 的解锁状态
                maskSlots[i].color = unlockStatus[dataIndex] ? inactiveColor : lockedColor;
            }
        }
    }

    // --- 刷新选中高亮 ---
    public void UpdateMaskUI(int currentMaskID)
    {
        if (currentUnlockStatus == null) return;

        for (int i = 0; i < maskSlots.Length; i++)
        {
            // 计算数据对应的 ID
            int dataIndex = i + maskDataOffset;

            // 如果这个面具(比如冰)还没解锁，保持黑色，不处理高亮
            if (dataIndex < currentUnlockStatus.Length && currentUnlockStatus[dataIndex] == false)
            {
                maskSlots[i].color = lockedColor;
                maskSlots[i].transform.localScale = Vector3.one;
                continue;
            }

            // ★★★ 核心逻辑：判断是否选中 ★★★
            // 如果 当前主角的面具ID 等于 这个格子对应的 dataIndex，那就是它！
            bool isSelected = (currentMaskID == dataIndex);

            if (isSelected)
            {
                maskSlots[i].color = activeColor;
                maskSlots[i].transform.localScale = Vector3.one * 1.2f;
            }
            else
            {
                maskSlots[i].color = inactiveColor;
                maskSlots[i].transform.localScale = Vector3.one;
            }
        }
    }
}