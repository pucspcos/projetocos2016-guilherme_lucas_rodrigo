using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Actor : MonoBehaviour {

    public Sprite actorimage;

    public string actorname;

    public string actorbio;

    public int affinitypoints;
    public int[] dialoglines;

    public bool hasdialog;

    void OnEnable()
    {
        Debug.Log(actorname + " has enter in scene for dialog ");
    }
    void OnDisable()
    {
        Debug.Log(actorname + " has left the scene ");
    }

}
