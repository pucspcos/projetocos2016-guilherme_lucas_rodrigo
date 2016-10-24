using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogBox : MonoBehaviour {

    #region attributes
    public GameController gamecontroller;
    public Scene scene;
    public Dialog dialog;
    public GameObject dialogbox;
    public Text text;
    public GameObject speakerbox;
    public Text speakernametext;
    public GameObject answerbox;
    public AnswerButton[] answersbuttons;

    public int currentdialog;
    public int nextdialog;
    public int endatdialog;

    public int currentdialoganswers;
    public int nextdialoganswers;

    public int lastanswerid;

    public bool hasanswered;
    public bool hasnextdialog;
    public bool hasnextanswers;
    public bool onclickenddialog;
    #endregion

    #region methods

    #region Enable And Disable Methods

    void OnEnable()
    {
        Debug.Log("DialogDisplayBox active");
    }
    void OnDisable()
    {
        Debug.Log("DialogDisplayBox disactive");
    }

    #endregion

    #region Awake And Start

    void Awake()
    {
        if (gamecontroller == null)
            gamecontroller = GameObject.Find("GameController").GetComponent<GameController>();
        if (dialogbox == null)
            dialogbox = gameObject;
        if (text == null)
            text = gameObject.GetComponentInChildren<Text>();
    }
    void Start()
    {
        lastanswerid = -1;
    }

    #endregion

    #region Dialog Update Methods

    public void DialogUpdate()
    {

        if (hasnextdialog)
            onclickenddialog = false;
        else { onclickenddialog = true; }

        if (scene.scenestate == Scene.state.dialog)
        {
            if (scene.dialogs.Length >= 2)
            {
                if (nextdialog != currentdialog)
                    hasnextdialog = true;
                else { hasnextdialog = false; }

                dialogbox.SetActive(true);
                dialog.dialogtextfile = scene.dialogs[currentdialog];
                endatdialog = scene.dialogs.Length - 1;
                if (!dialog.istextsplited)
                    dialog.SplitDialogText();
                text.text = dialog.dialoglines[dialog.currentdialogline];
            }

            if (scene.answers.Length >= 2)
            {
                dialog.answertextfile = scene.answers[currentdialoganswers];
                if (!dialog.isanswerssplit)
                    dialog.SplitAnswerText();
                if (answerbox.activeInHierarchy)
                    for (int i = 0; i < dialog.answers.Length - 1; i++)
                    {
                        answersbuttons[i].gameObject.SetActive(true);
                        answersbuttons[i].gameObject.GetComponentInChildren<Text>().text = dialog.answers[i];
                    }
                else
                {
                    foreach (AnswerButton button in answersbuttons)
                        button.gameObject.SetActive(false);
                }
            }
        }
        else { dialogbox.SetActive(false); }
    }

    #endregion

    #region Dialog Fundamental Methods

    #region methods for adjust dialogbox and dialog

    #region Dialog Set Speaker Methods

    public void SetSpeaker(string NewSpeaker)
    {
        speakernametext.text = NewSpeaker;
    }

    #endregion

    #region Dialog Start Methods

    public void StartDialog(int DialogToStart)
    {
        currentdialog = DialogToStart;
        dialog.currentdialogline = 0;
        gameObject.SetActive(true);
        answerbox.SetActive(false);
        dialog.ChangeDialogText();
        lastanswerid = -1;
    }

    #endregion

    #region Dialog Restart Methods

    void RestartDialog()
    {
        dialog.currentdialogline = 0;
        lastanswerid = -1;
        if (currentdialog != nextdialog)
            hasnextdialog = true;
    }

    void RestartDialogAnswers()
    {
        currentdialoganswers = 0;
    }

    #endregion

    #region Dialog End Methods

    void EndDialog()
    {
        scene.scenestate = Scene.state.interaction;
    }

    #endregion

    #region Dialog Answers Methods

    void DisplayAnswers()
    {
        answerbox.SetActive(true);
    }

    #endregion

    #region Player Interaction Process

    public void Processed()
    {
        if (dialogbox.gameObject.activeInHierarchy)
        {
            if (onclickenddialog)
                scene.scenestate = Scene.state.interaction;
            else
            {
                if (dialog.isanswermoment)
                {
                    DisplayAnswers();
                }
                if (hasnextdialog)
                {
                    onclickenddialog = false;

                    if (!dialog.isanswermoment)
                    {

                        if (dialog.currentdialogline == dialog.enddialogatline)
                        {
                            currentdialog = nextdialog;
                            currentdialoganswers = nextdialoganswers;
                            RestartDialog();
                            dialog.ChangeDialogText();
                        }
                        else
                        {
                            dialog.currentdialogline++;
                        }
                    }
                }
                if (!hasnextdialog)
                {
                    if(!dialog.isanswermoment)
                        if (dialog.currentdialogline == dialog.enddialogatline)
                            onclickenddialog = true;
                }
            }
        }
        else
        {
            EndDialog();
        }
    }

    #endregion

    #endregion

    #region methods for answers

    #region Main Methods

    public void OnAnswerContinue()
    {
        if (currentdialoganswers < gamecontroller.scenes[gamecontroller.currentscene].answers.Length - 1)
            currentdialoganswers++;
        hasanswered = true;
        dialog.isanswermoment = false;
        Processed();
        answerbox.SetActive(false);
    }
    public void OnAnswerChangeNextDialog(AnswerButton AnswerButton)
    {
        lastanswerid = AnswerButton.answerbuttonid;
        Debug.Log("Respondeu " + lastanswerid);
        nextdialog = AnswerButton.nextdialog;
        Debug.Log("NextDialog" + nextdialog);
    }
    public void OnAnswerChangeNextScene(AnswerButton AnswerButton)
    {
        gamecontroller.nextscene = AnswerButton.nextscene;
        Debug.Log("NextScene" + AnswerButton.nextscene);
    }
    #endregion

    #region Methods for answersButtons

    public void AnswerButtonsSetNextDialog(int AnswerButton0NextDialog, int AnswerButton1NextDialog, int AnswerButton2NextDialog)
    {
        answersbuttons[0].nextdialog = AnswerButton0NextDialog;
        answersbuttons[1].nextdialog = AnswerButton1NextDialog;
        answersbuttons[2].nextdialog = AnswerButton2NextDialog;
    }

    public void AnswerButtonsSetNextScene(int AnswerButton0NextScene, int AnswerButton1NextScene, int AnswerButton2NextScene)
    {
        answersbuttons[0].nextscene = AnswerButton0NextScene;
        answersbuttons[1].nextscene = AnswerButton1NextScene;
        answersbuttons[2].nextscene = AnswerButton2NextScene;
    }

    #endregion

    #endregion

    #endregion

    #endregion

}
