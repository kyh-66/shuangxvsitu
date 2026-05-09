using UnityEngine;

public class IceBlock : MonoBehaviour
{
    [Header("��������")]
    public float existTime = 2.0f; // �����������ڵ�ʱ�䣨�룩
    public float fallSpeed = 3.0f; // ����ʱ���������ʣ�Խ�����Խ�죩

    private Rigidbody2D rb;
    private Collider2D col;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

       
        Invoke("StartFalling", existTime);
    }

    void StartFalling()
    {
        // 1. �л�Ϊ������Ӱ�죬������������ȥ
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = fallSpeed;

        
        if (col != null)
        {
            col.isTrigger = true;
        }

        
        Destroy(gameObject, 2.0f);
    }
}