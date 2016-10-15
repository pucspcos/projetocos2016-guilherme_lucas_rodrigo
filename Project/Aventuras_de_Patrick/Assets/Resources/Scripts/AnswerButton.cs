using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnswerButton : MonoBehaviour {

    #region Attributes

    public Button answerbuttonbutton;

    public int answerbuttonid;
    public int nextdialog;
    public int currentvalue;

    #endregion

    #region Methods

    public void ButtonSetNextDialog(int NextDialog)
    {
        nextdialog = NextDialog;
    }
    public void ButtonSetValue(int Value)
    {
        currentvalue = Value;
    }

    #endregion
}
