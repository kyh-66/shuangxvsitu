using UnityEngine;
using System.Collections.Generic; // 需要这个来用 List

public class EnemySpawner : MonoBehaviour
{
    [Header("配置")]
    public GameObject monsterPrefab; // 拖入怪物预制体
    public int maxMonsters = 3;      // 最多同时存在几只？
    public float spawnInterval = 5f; // 多久生一只？

    [Header("调试信息 (不用填)")]
    public float timer = 0f;
    // 用一个列表记录我是谁的爸爸
    public List<GameObject> myMonsters = new List<GameObject>();

    void Update()
    {
        // 1. 清理名单：把已经死掉的(null)怪物从名单里划掉
        // 这一步很关键，怪物掉虚空死掉后，这里就会腾出位置
        myMonsters.RemoveAll(item => item == null);

        // 2. 检查数量是否已满
        if (myMonsters.Count >= maxMonsters)
        {
            return; // 满了就不生了，等待
        }

        // 3. 计时生成
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnMonster();
            timer = 0f; // 重置计时器
        }
    }

    void SpawnMonster()
    {
        if (monsterPrefab == null) return;

        // 在当前刷怪点的位置生成
        GameObject newMonster = Instantiate(monsterPrefab, transform.position, Quaternion.identity);

        // 把它加入名单
        myMonsters.Add(newMonster);

        // 如果是在暗界生成的，最好把怪物的父物体设为 World_Dark (可选)
        // newMonster.transform.SetParent(this.transform.parent); 
    }

    // 在编辑器里画个圈，方便你看到刷怪点在哪
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}