using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class Background : MonoBehaviour {

    #region attributes
    public Image backgroundimage;

    public int currentbackground;
    #endregion

    #region methods
    void Start()
    {
        if (backgroundimage == null)
            backgroundimage = GetComponent<Image>();
    }
    #endregion
}
