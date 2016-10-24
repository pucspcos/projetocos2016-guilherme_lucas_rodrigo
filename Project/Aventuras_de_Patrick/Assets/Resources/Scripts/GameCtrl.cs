using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameCtrl : MonoBehaviour {

    #region Actor Reference

    private const string patrickreference = "Patrick";
    private const string developersreference = "Desenvolvedores";
    private const string homemDeCapuzreference = "Homem do capuz";
    private const string vizinhoreference = "Joaquim o vizinho";

    #endregion

    #region Attributes

    public Player player;
    public Animation deathanimation;
    public LiveBackground livebackground;
    public Background background;
    public DialogBox dialogbox;

    public Actor[] actors;
    public Scene[] scenes;

    public int currentscene;
    public int nextscene;
    public bool canprogress;

    #endregion

    #region Methods

    #region Enable and Disable Methods
    void OnEnable()
    {
        Debug.Log("Game Controller active");
    }
    void OnDisable()
    {
        Debug.Log("Game Controller disactive");
    }
    #endregion

    #region Awake and Start Methods
    void Awake()
    {
        if (player == null)
            player = GameObject.Find("Main Camera Patrick").GetComponent<Player>();
        if (livebackground == null)
            livebackground = GameObject.Find("LiveBackground").GetComponent<LiveBackground>();
        if (background == null)
            background = GameObject.Find("Background").GetComponent<Background>();
        if (dialogbox == null)
            dialogbox = GameObject.Find("DialogDisplayBox").GetComponent<DialogBox>();

        if (actors == null || actors.Length <= 0)
            Debug.LogWarning("Game Controller has no actors");
        if (scenes == null || scenes.Length <= 0)
            Debug.LogError("Game controller has no actors");
    }
    void Start()
    {

        SetScenesID();

        SetScenesName();

        SetActorSprite();

        dialogbox.scene = scenes[currentscene];

        canprogress = true;
    }
    #endregion

    #region Updates Methods
    void Update()
    {
        #region Background Control

        if (scenes[currentscene].livebackground.Length <= 0)
            livebackground.gameObject.SetActive(false);
        else
        {
            livebackground.gameObject.SetActive(true);

            if (livebackground.movie.Length != scenes[currentscene].livebackground.Length)
            {
                livebackground.movie = new MovieTexture[scenes[currentscene].livebackground.Length];
                for (int i = 0; i < scenes[currentscene].livebackground.Length; i++)
                    livebackground.movie[i] = scenes[currentscene].livebackground[i];
            }

            livebackground.UpdateLiveBackground();
        }

        if (scenes[currentscene].backgrounds.Length <= 0)
            background.gameObject.SetActive(false);
        else
        {
            background.gameObject.SetActive(true);
            background.backgroundimage.sprite = scenes[currentscene].backgrounds[background.currentbackground];
        }

        #endregion
        #region Dialog Control

        DialogUpdate();

        dialogbox.DialogUpdate();

        dialogbox.scene = scenes[currentscene];

        #endregion
        #region Actors Control

        PrepareActorDialogLines();

        for (int i = 0; i < actors.Length; i++)
        {
            actors[i].hasdialog = CheckActorDialogLines(actors[i]);
            if (actors[i].hasdialog)
                actors[i].gameObject.SetActive(true);
            else { actors[i].gameObject.SetActive(false); }
        }

        #endregion
        #region Scene Control

        TimeToSetScene();

        #endregion
        #region Game Control

        CheckIfTheDialogEndGoToNextScene();

        CheckGameOverPlayDeathAnimationAndEndGame();

        #endregion
        #region Player Control

        if (Input.GetButtonDown("Submit"))
            if (canprogress)
                dialogbox.Processed();

        #endregion
    }
    #endregion

    #region GameController Fundamental Methods

    #region CheckIfTheDialoghasEndGoToNextScene

    void CheckIfTheDialogEndGoToNextScene()
    {
        if (scenes[currentscene].scenestate == Scene.state.interaction)
        {
            dialogbox.StartDialog(0);
            currentscene = nextscene;
        }
    }

    #endregion

    #region Dialog Methods

    #region Dialog Universal Update

    void DialogUpdate()
    {
        switch (currentscene)
        {
            case 0:
                dialogbox.SetSpeaker(patrickreference);

                if (dialogbox.currentdialog == 0)
                {
                    if (dialogbox.lastanswerid < 0)
                    {
                        dialogbox.nextdialog = dialogbox.endatdialog;
                        dialogbox.AnswerButtonsSetNextDialog(1, 2, 3);
                    }

                    if (dialogbox.dialog.currentdialogline <= dialogbox.dialog.enddialogatline)
                    {
                        dialogbox.AnswerButtonsSetNextScene(1, 2, 0);
                    }

                    if (dialogbox.dialog.currentdialogline == 3)
                    {
                        if (!dialogbox.hasanswered)
                            dialogbox.dialog.isanswermoment = true;
                    }
                    else { dialogbox.hasanswered = false; }
                }
                break;
            case 1:
                dialogbox.SetSpeaker(patrickreference);
                break;
            case 2:
                dialogbox.SetSpeaker(patrickreference);
                break;
            case 3:
                if (dialogbox.currentdialog == 0)
                {
                    if (dialogbox.dialog.currentdialogline == 0
                        || dialogbox.dialog.currentdialogline == 1
                        || dialogbox.dialog.currentdialogline == 2
                        || dialogbox.dialog.currentdialogline == 3
                        || dialogbox.dialog.currentdialogline == 4
                        || dialogbox.dialog.currentdialogline == 5
                        || dialogbox.dialog.currentdialogline == 7
                        || dialogbox.dialog.currentdialogline == 9
                        || dialogbox.dialog.currentdialogline == 11
                        || dialogbox.dialog.currentdialogline == 14
                        || dialogbox.dialog.currentdialogline == 15
                        || dialogbox.dialog.currentdialogline == 16
                        || dialogbox.dialog.currentdialogline == 17
                        || dialogbox.dialog.currentdialogline == 20)
                        dialogbox.SetSpeaker(patrickreference);
                    if (dialogbox.dialog.currentdialogline == 6
                        || dialogbox.dialog.currentdialogline == 8
                        || dialogbox.dialog.currentdialogline == 10
                        || dialogbox.dialog.currentdialogline == 12
                        || dialogbox.dialog.currentdialogline == 19)
                        dialogbox.SetSpeaker(developersreference);
                }

                if (dialogbox.currentdialog == 0)
                {
                    if (dialogbox.lastanswerid < 0)
                    {
                        dialogbox.nextdialog = dialogbox.endatdialog;
                        dialogbox.AnswerButtonsSetNextDialog(1, 2, 3);
                    }
                }

                if (dialogbox.currentdialog == 0)
                {
                    if (dialogbox.dialog.currentdialogline <= dialogbox.dialog.enddialogatline)
                    {
                        dialogbox.AnswerButtonsSetNextScene(7, 7, 0);
                    }
                }

                if (dialogbox.currentdialog == 0)
                    if (dialogbox.dialog.currentdialogline == 21)
                    {
                        if (!dialogbox.hasanswered)
                            dialogbox.dialog.isanswermoment = true;
                    }
                    else { dialogbox.hasanswered = false; }

                break;
            case 4:
                dialogbox.SetSpeaker(homemDeCapuzreference);

                if (dialogbox.currentdialog == 0)
                {
                    if (dialogbox.dialog.currentdialogline <= dialogbox.dialog.enddialogatline)
                    {
                        dialogbox.AnswerButtonsSetNextScene(5, 6, 0);
                    }
                }

                if (dialogbox.currentdialog == 0)
                    if (dialogbox.dialog.currentdialogline == 3)
                    {
                        if (!dialogbox.hasanswered)
                            dialogbox.dialog.isanswermoment = true;
                    }
                    else
                    { dialogbox.hasanswered = false; }
                break;
            case 5:
                dialogbox.SetSpeaker(patrickreference);
                break;
            case 6:
                dialogbox.SetSpeaker(patrickreference);

                if (dialogbox.currentdialog == 0)
                {
                    if (dialogbox.dialog.currentdialogline <= dialogbox.dialog.enddialogatline)
                    {
                        dialogbox.AnswerButtonsSetNextScene(10, 8, 0);
                    }
                }

                if (dialogbox.currentdialog == 0)
                    if (dialogbox.dialog.currentdialogline == 6)
                    {
                        if (!dialogbox.hasanswered)
                            dialogbox.dialog.isanswermoment = true;
                    }
                    else { dialogbox.hasanswered = false; }
                break;
            case 7:
                dialogbox.SetSpeaker(patrickreference);

                if (dialogbox.currentdialog == 0)
                {
                    if (dialogbox.dialog.currentdialogline <= dialogbox.dialog.enddialogatline)
                    {
                        dialogbox.AnswerButtonsSetNextScene(9, 8, 0);
                    }
                }

                if (dialogbox.currentdialog == 0)
                    if (dialogbox.dialog.currentdialogline == 1)
                    {
                        if (!dialogbox.hasanswered)
                            dialogbox.dialog.isanswermoment = true;
                    }
                    else { dialogbox.hasanswered = false; }
                break;
            case 8:

                if (dialogbox.currentdialog == 0)
                {
                    if (dialogbox.dialog.currentdialogline <= dialogbox.dialog.enddialogatline)
                    {
                        dialogbox.AnswerButtonsSetNextScene(9, 10, 0);
                    }
                }

                if (dialogbox.currentdialog == 0)
                    if (dialogbox.dialog.currentdialogline == 6)
                    {
                        if (!dialogbox.hasanswered)
                            dialogbox.dialog.isanswermoment = true;
                    }
                    else { dialogbox.hasanswered = false; }
                break;
            case 9:
                break;
            case 10:
                break;
            case 11:
                break;
        }
    }

    #endregion
 
    #endregion

    #region Actors Methods
    bool CheckActorDialogLines(Actor ActorI)
    {
        foreach (int actordialogline in ActorI.dialoglines)
            if (actordialogline == dialogbox.dialog.currentdialogline)
                return true;

        return false;
    }
    void PrepareActorDialogLines()
    {
        for (int i = 0; i < actors.Length; i++)
        {
            if (actors[i].name == "Patrick")
            {
                switch (currentscene)
                {
                    case 0:
                        if (dialogbox.currentdialog == 0)
                            actors[i].dialoglines = new int[4] { 0, 1, 2, 3 };
                        else { actors[i].dialoglines = new int[0]; }
                        break;
                    case 1:
                        if (dialogbox.currentdialog == 0)
                            actors[i].dialoglines = new int[5] { 3, 4, 5, 6, 7 };
                        else { actors[i].dialoglines = new int[0]; }
                        break;
                }
            }
            if (actors[i].name == "Homem de Capuz")
            {
                switch (currentscene)
                {
                    case 0:
                        if (dialogbox.currentdialog == 0)
                            actors[i].dialoglines = new int[4] { 0, 1, 2, 3 };
                        else { actors[i].dialoglines = new int[0]; }
                        break;
                    case 1:
                        if (dialogbox.currentdialog == 0)
                            actors[i].dialoglines = new int[5] { 3, 4, 5, 6, 7 };
                        else { actors[i].dialoglines = new int[0]; }
                        break;

                }
                if (actors[i].name == "Vizinho de Patrick")
                {
                    switch (currentscene)
                    {
                        case 0:
                            if (dialogbox.currentdialog == 0)
                                actors[i].dialoglines = new int[4] { 0, 1, 2, 3 };
                            else { actors[i].dialoglines = new int[0]; }
                            break;
                        case 1:
                            if (dialogbox.currentdialog == 0)
                                actors[i].dialoglines = new int[5] { 3, 4, 5, 6, 7 };
                            else { actors[i].dialoglines = new int[0]; }
                            break;
                    }
                }
            }
        }
    }
    void SetActorSprite()
    {
        foreach (Actor actor in actors)
            actor.gameObject.GetComponent<Image>().sprite = actor.actorimage;
    }
    #endregion

    #region Scenes Methods

    void SetScenesID()
    {
        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i].sceneid = i;
            Debug.Log("Scene " + scenes[i].gameObject.name + " ID equal " + scenes[i].sceneid);
        }
    }
    void SetScenesName()
    {
        for (int i = 0; i < scenes.Length; i++)
        {
            switch (scenes[i].sceneid)
            {
                case 0: scenes[i].scenename = "Patrick Acorda"; break;
                case 1: scenes[i].scenename = "Casa"; break;
                case 2: scenes[i].scenename = "Casa"; break;
                case 3: scenes[i].scenename = "Escola"; break;
                case 4: scenes[i].scenename = "Casa"; break;
                case 5: scenes[i].scenename = "Casa na arvore"; break;
                case 6: scenes[i].scenename = "Outro Lado da Rua"; break;
                case 7: scenes[i].scenename = "Pedido de ajuda"; break;
                case 8: scenes[i].scenename = "Casa do vizinho"; break;
                case 9: scenes[i].scenename = "Conversa dentro da casa do homem"; break;
                case 10: scenes[i].scenename = "Morte do patrick"; break;
                case 11: scenes[i].scenename = "Morte do patrick e de Joaquim"; break;
            }
            Debug.Log("Scene " + scenes[i].scenename + " Avaliable");
        }
    }

    #region Scene progression Cases Methods

    void TimeToSetScene()
    {
        SceneBehaviour currentscenebehaviour;

        if (scenes[currentscene].GetComponent<SceneBehaviour>() != null)
            currentscenebehaviour = scenes[currentscene].GetComponent<SceneBehaviour>();
        else
        {
            scenes[currentscene].gameObject.AddComponent<SceneBehaviour>();
            currentscenebehaviour = scenes[currentscene].GetComponent<SceneBehaviour>();
            currentscenebehaviour.timertostart = 50;
        }

        if (currentscenebehaviour != null)
        {
            if (currentscenebehaviour.timertostart > 0)
            {
                currentscenebehaviour.timertostart--;
                if (canprogress == true)
                    canprogress = false;
                Debug.Log("Tempo para liberar progresso " + currentscenebehaviour.timertostart);
            }
            else
            {
                if (canprogress == false)
                    canprogress = true;
                Debug.Log("Scene Ready");
            }
        }
        else { Debug.LogError("No SceneBehaviour on scene " + currentscene); }
    }

    #endregion

    #endregion

    #region Game Methods

    #region Death Methods

    public void CheckGameOverPlayDeathAnimationAndEndGame()
    {
        if (currentscene == 9 || currentscene == 10)
            if (dialogbox.dialog.currentdialogline == dialogbox.dialog.enddialogatline)
                deathanimation.Play("Death");

        if (!deathanimation.isPlaying)
            Application.Quit();
    }

    #endregion

    #endregion

    #endregion

    #endregion
}
