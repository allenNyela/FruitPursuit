using UnityEngine;

public class SwitchMat : MonoBehaviour
{
    [SerializeField]
    public Material tempMat;
    [SerializeField]
    public Material defaultMat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void switchToNewMat()
    {
        this.GetComponent<MeshRenderer>().material = tempMat;
    }

    public void switchToDefaultMat()
    {
        this.GetComponent<MeshRenderer>().material = defaultMat;
    }
}
