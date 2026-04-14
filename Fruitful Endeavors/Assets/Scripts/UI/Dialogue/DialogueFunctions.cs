using UnityEngine;
using UnityEngine.Playables;
using Yarn.Unity;

public class DialogueFunctions : MonoBehaviour
{
    public DialogueRunner DialogueRunner;
    public PlayableDirector TutorialTimeline;
    public SwitchMat Switchmat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DialogueRunner.onNodeComplete.AddListener(OnNodeFinished);
        DialogueRunner.onNodeStart.AddListener(OnNodeStarted);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDialogue()
    {
        DialogueRunner.StartDialogue("Tutorial_Dialogue");
    }

    void OnNodeFinished(string nodeName)
    {
        if (nodeName == "Node_1775713450174")
        {
            TutorialTimeline.Resume();
        }

        if (nodeName == "Node_1776154164506")
        {
            TutorialTimeline.Resume();
        }

        if (nodeName == "Node_1776156479815")
        {
            TutorialTimeline.Resume();
        }

        if (nodeName == "Node_1776157366180")
        {
            Switchmat.switchToDefaultMat();
            TutorialTimeline.Resume();        
        }
    }

    void OnNodeStarted(string nodeName)
    {
        if (nodeName == "Node_1776157009657")
        {
            Switchmat.switchToNewMat();
        }
    }

    public void StartNode(string NodeName)
    {
        DialogueRunner.StartDialogue(NodeName);
    }
}
