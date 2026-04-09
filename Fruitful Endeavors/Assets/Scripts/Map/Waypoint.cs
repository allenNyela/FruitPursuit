using UnityEngine;

public class Waypoint : MonoBehaviour
{

    [SerializeField]
    public static Transform[] path1;
    [SerializeField]
    public Transform[] path2;

    private void Awake()
    {
       path1 = new Transform[transform.childCount];
       for (int i = 0; i < path1.Length; i++)
       {
          path1[i] = transform.GetChild(i);
       }

       // path2 = new Transform[transform.childCount];
        //for (int i = 0; i < path2.Length; i++)
        //{
        //    path2[i] = transform.GetChild(i);
       // }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
