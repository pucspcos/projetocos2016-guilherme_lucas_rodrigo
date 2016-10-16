using UnityEngine;
using System.Collections;

public class SceneBehaviour : MonoBehaviour {

    #region Attributes

    public float timertogo;

    public float timertostart;
    
    #endregion

    #region Methods

    #region Enable and Disable

    void OnEnable()
    {
        Debug.Log("Scene Behaviour " + gameObject.name + " Active");
    }
    void OnDisable()
    {
        Debug.Log("Scene Behaviour " + gameObject.name + " Desactive");
    }

    #endregion

    #endregion
}
