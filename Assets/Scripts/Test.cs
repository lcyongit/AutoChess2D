
using UnityEngine;
using UnityEngine.EventSystems;

public class Test : MonoBehaviour, IPointerDownHandler
{
    [Header("Components")]
    public Collider2D coll;
    public SpriteRenderer spriteRenderer;
    public GameObject test2GO;

    private void Awake()
    {
        
    }

    private void Start()
    {
        Test2 test2 = test2GO.GetComponent<Test2>();
        test2.fff = 20f;
        test2.gameObject.SetActive(true);


    }
#if UNITY_EDITOR
    [ContextMenu("Print All Tags")]     // Component旁邊的三個點點
    private void PrintAllTags()
    {
        // 取得所有已定義的Tag
        string[] tags = UnityEditorInternal.InternalEditorUtility.tags;

        // 印出每個Tag
        foreach (string tag in tags)
        {
            Debug.Log("Tag: " + tag);
        }
    }
#endif
    private Vector3 GetMouseWorldPosition(Vector3 screenPosition)
    {
        screenPosition.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(screenPosition);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Vector3 worldPosition = GetMouseWorldPosition(eventData.position);
        //Debug.Log(eventData.position);


    }
}
