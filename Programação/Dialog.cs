using UnityEngine;
using System.Collections;

public class Dialog : MonoBehaviour {

    #region Attributes
    public TextAsset dialogtextfile;
    public TextAsset answertextfile;

    public string[] dialoglines;

    public int currentdialogline;
    public int enddialogatline;

    public bool istextsplited;

    public string[] answers;

    public bool isanswerssplit;

    public bool isanswermoment;
    #endregion
    #region Methods
    void OnEnable()
    {
        Debug.Log("Dialog active " + dialogtextfile);
    }
    void OnDisable()
    {
        Debug.Log("Dialog Disable");
    }
    public void SplitDialogText()
    {
        if (dialogtextfile != null)
        {
            dialoglines = dialogtextfile.text.Split('\n');
            enddialogatline = dialoglines.Length - 1;
            istextsplited = true;
        }
    }
    public void SplitAnswerText()
    {
        if (answertextfile != null)
        {
            answers = answertextfile.text.Split('\n');
            isanswerssplit = true;
        }
    }
    public void ChangeDialogText()
    {
        istextsplited = false;
        isanswerssplit = false;
        Debug.Log("New Dialog: " + dialogtextfile);
    }
    #endregion
}
