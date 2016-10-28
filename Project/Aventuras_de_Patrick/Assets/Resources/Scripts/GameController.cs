using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

    #region Actor Reference

    private const string patrickreference = "Patrick";
    private const string developersreference = "Desenvolvedores";
    private const string homemDeCapuzreference = "Homem do capuz";
    private const string vizinhoreference = "Joaquim o vizinho";

    #endregion

    #region Attributes

    public Player player;
    public Animator deathanimator;
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
	void Start () {

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
                for (int i = 0; i < scenes[currentscene].livebackground.Length; i ++)
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

        UpdateSpeakerName();

        PrepareAnswersMoments();

        dialogbox.DialogUpdate();

        AdjustDialogDisplayBoxToNextDialog();

        AdjustDialogDisplayBoxToNextScene();

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

        NavegateScenes();

        CheckGameOverPlayDeathAnimationAndEndGame();

        #endregion
        #region Player Control

        if (Input.GetButtonDown("Submit"))
            if(canprogress)
                dialogbox.Processed();

        #endregion
    }
    #endregion

    #region GameController Fundamental Methods

    #region Dialog Methods

    #region Dialog Speaker Methods

    void UpdateSpeakerName()
    {
        switch (currentscene)
        {
            case 0:
                dialogbox.SetSpeaker(patrickreference);
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
                if(dialogbox.currentdialog == 1)
                {
                    dialogbox.SetSpeaker(patrickreference);
                }
               if (dialogbox.currentdialog == 2)
                {
                    if(dialogbox.dialog.currentdialogline == 0)
                    {
                        dialogbox.SetSpeaker(homemDeCapuzreference);
                    }
                    else
                    {
                        dialogbox.SetSpeaker(patrickreference);
                    }
                }
                break;
            case 4:
                dialogbox.SetSpeaker(patrickreference);
                break;
            case 5:
                dialogbox.SetSpeaker(patrickreference);
                break;
            case 6:
                dialogbox.SetSpeaker(patrickreference);
                break;
            case 7:
                dialogbox.SetSpeaker(patrickreference);
                break;
            case 8:
                if(dialogbox.currentdialog == 0)
                {
                    if(dialogbox.dialog.currentdialogline == 0
                       ||dialogbox.dialog.currentdialogline == 1
                       ||dialogbox.dialog.currentdialogline == 2)
                    {
                        dialogbox.SetSpeaker(patrickreference);
                    }
                    else
                    {
                        dialogbox.SetSpeaker(vizinhoreference);
                    }
                }
                break;
            case 9:
                if(dialogbox.currentdialog == 0)
                {
                    if (dialogbox.dialog.currentdialogline == 0
                        || dialogbox.dialog.currentdialogline == 1
                        || dialogbox.dialog.currentdialogline == 2)
                        dialogbox.SetSpeaker(patrickreference);
                    else
                    {
                        dialogbox.SetSpeaker("");
                    }
                }
                break;
            case 10:
                if (dialogbox.dialog.currentdialogline >= 0 && dialogbox.dialog.currentdialogline <= 2)
                    dialogbox.SetSpeaker(vizinhoreference);
                else if(dialogbox.dialog.currentdialogline == 3)
                {
                    dialogbox.SetSpeaker(patrickreference);
                }
                else
                {
                    dialogbox.SetSpeaker("");
                }
                break;
        }
    }

    #endregion

    #region Dialog Answer Methods
    //method that prepare dialog for a answer moment and alert when is the moment of give the object
    void PrepareAnswersMoments()
    {
        switch (currentscene)
        {
            case 0:
                if (dialogbox.currentdialog == 0)
                    if (dialogbox.dialog.currentdialogline == 3)
                    {
                        if (!dialogbox.hasanswered)
                            dialogbox.dialog.isanswermoment = true;
                    }
                    else { dialogbox.hasanswered = false; }
                break;
            case 3:
                if (dialogbox.currentdialog == 0)
                    if (dialogbox.dialog.currentdialogline == 21)
                    {
                        if (!dialogbox.hasanswered)
                            dialogbox.dialog.isanswermoment = true;
                    }
                    else { dialogbox.hasanswered = false; }
                break;
            case 4:
                if (dialogbox.currentdialog == 0)
                    if (dialogbox.dialog.currentdialogline == 3)
                    {
                        if (!dialogbox.hasanswered)
                            dialogbox.dialog.isanswermoment = true;
                    }
                    else
                    { dialogbox.hasanswered = false; }
                break;
            case 6:
                if (dialogbox.currentdialog == 0)
                    if (dialogbox.dialog.currentdialogline == 6)
                    {
                        if (!dialogbox.hasanswered)
                            dialogbox.dialog.isanswermoment = true;
                    }
                    else { dialogbox.hasanswered = false; }
                break;
            case 7:
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
                    if (dialogbox.dialog.currentdialogline == 6)
                    {
                        if (!dialogbox.hasanswered)
                            dialogbox.dialog.isanswermoment = true;
                    }
                    else { dialogbox.hasanswered = false; }
                break;
        }
    }

    #endregion

    #region Next Dialog Adjust Methods

    //method that adjust next dialog for current dialog not value has the final dialog causing the dialog to end
    void AdjustDialogDisplayBoxToNextDialog()
    {
        switch (currentscene)
        {
            case 0:
                if (dialogbox.currentdialog == 0)
                {
                    if (dialogbox.lastanswerid < 0)
                    {
                        dialogbox.nextdialog = dialogbox.endatdialog;
                        dialogbox.AnswerButtonsSetNextDialog(1, 2, 3);
                    }
                }
                break;
            case 1:
                dialogbox.nextdialog = dialogbox.endatdialog;
                break;
            case 2:
                dialogbox.nextdialog = dialogbox.endatdialog;
                break;
            case 3:
                switch (dialogbox.currentdialog)
                {
                    case 0:
                        if (dialogbox.lastanswerid < 0)
                        {
                            dialogbox.nextdialog = dialogbox.endatdialog;
                            dialogbox.AnswerButtonsSetNextDialog(1, 2, 3);
                        }
                        break;
                    case 1:
                        if (dialogbox.dialog.currentdialogline < 3)
                            dialogbox.nextdialog = dialogbox.endatdialog;
                        else
                        {
                            dialogbox.nextdialog = dialogbox.currentdialog;
                        }
                        break;
                    case 2:
                        if (dialogbox.dialog.currentdialogline < 3)
                            dialogbox.nextdialog = dialogbox.endatdialog;
                        else
                        {
                            dialogbox.nextdialog = dialogbox.currentdialog;
                        }
                        break;
                }
                break;
            case 4:
                if (dialogbox.currentdialog == 0)
                {
                    if (dialogbox.lastanswerid < 0)
                    {
                        dialogbox.nextdialog = dialogbox.endatdialog;
                        dialogbox.AnswerButtonsSetNextDialog(1, 1, 0);
                    }
                }
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                switch (dialogbox.currentdialog)
                {
                    case 0:
                        if (dialogbox.lastanswerid < 0)
                        {
                            dialogbox.nextdialog = dialogbox.endatdialog;
                            dialogbox.AnswerButtonsSetNextDialog(1, 1, 0);
                        }
                        break;
                }
                break;
            case 8:
                break;
            case 9:
                switch(dialogbox.currentdialog)
                {
                    case 0:
                        if (dialogbox.dialog.currentdialogline < 4)
                            dialogbox.nextdialog = dialogbox.endatdialog;
                        else
                        {
                            dialogbox.nextdialog = dialogbox.currentdialog;
                        }
                        break;
                        break;
                }
                break;
            case 10:
                break;
        }
    }

    #endregion

    #region Next Scene Adjust Methods

    void AdjustDialogDisplayBoxToNextScene()
    {
        switch (currentscene)
        {
            case 0:
                if (dialogbox.currentdialog == 0)
                {
                    if (dialogbox.dialog.currentdialogline <= dialogbox.dialog.enddialogatline)
                    {
                        dialogbox.AnswerButtonsSetNextScene(1, 2, 0);
                    }
                }
                break;
            case 3:
                if (dialogbox.currentdialog == 0)
                {
                    if (dialogbox.dialog.currentdialogline <= dialogbox.dialog.enddialogatline)
                    {
                        dialogbox.AnswerButtonsSetNextScene(7, 7, 0);
                    }
                }
                break;
            case 4:
                if (dialogbox.currentdialog == 0)
                {
                    if (dialogbox.dialog.currentdialogline <= dialogbox.dialog.enddialogatline)
                    {
                        dialogbox.AnswerButtonsSetNextScene(5, 6, 0);
                    }
                }
                break;
            case 6:
                if (dialogbox.currentdialog == 0)
                {
                    if (dialogbox.dialog.currentdialogline <= dialogbox.dialog.enddialogatline)
                    {
                        dialogbox.AnswerButtonsSetNextScene(10, 8, 0);
                    }
                }
                break;
            case 7:
                if (dialogbox.currentdialog == 0)
                {
                    if (dialogbox.dialog.currentdialogline <= dialogbox.dialog.enddialogatline)
                    {
                        dialogbox.AnswerButtonsSetNextScene(9, 8, 0);
                    }
                }
                break;
            case 8:
                if (dialogbox.currentdialog == 0)
                {
                    if (dialogbox.dialog.currentdialogline <= dialogbox.dialog.enddialogatline)
                    {
                        dialogbox.AnswerButtonsSetNextScene(9, 10, 0);
                    }
                }
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
                            actors[i].dialoglines = new int[3] { 0, 1, 2 };
                        else { actors[i].dialoglines = new int[0]; }
                        break;
                    default:
                        actors[i].dialoglines = new int[0];
                        break;
                }
            }
            if (actors[i].name == "Homem de Capuz")
            {
                switch (currentscene)
                {
                    case 3:
                        if (dialogbox.currentdialog == 1)
                            actors[i].dialoglines = new int[3] { 0, 1, 2 };
                        if (dialogbox.currentdialog == 2)
                            actors[i].dialoglines = new int[3] { 0, 1, 2 };
                        break;
                    case 6:
                        if (dialogbox.currentdialog == 0)
                        {
                            actors[i].dialoglines = new int[4] { 2, 3, 4, 5 };
                            if (dialogbox.dialog.currentdialogline == 2)
                            {
                                actors[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                            }
                            else if(dialogbox.dialog.currentdialogline == 3)
                            {
                                actors[i].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                            }
                            else if(dialogbox.dialog.currentdialogline == 4)
                            {
                                actors[i].transform.localScale = new Vector3(1f, 1f, 1f);
                            }
                        }
                        break;
                    case 9:
                        if (dialogbox.currentdialog == 0)
                            actors[i].dialoglines = new int[4] { 1, 2, 3, 4 };
                        break;
                    case 10:
                        if (dialogbox.currentdialog == 0)
                            actors[i].dialoglines = new int[4] { 1, 2, 3, 4 };
                        break;
                    default:
                        actors[i].dialoglines = new int[0];
                        break;

                }
                if (actors[i].name == "1")
                {
                    switch (currentscene)
                    {
                        case 7:
                            if (dialogbox.currentdialog == 0)
                                actors[i].dialoglines = new int[1] { 0 };
                            else { actors[i].dialoglines = new int[0]; }
                            break;
                        case 8:
                            if (dialogbox.currentdialog == 0)
                                actors[i].dialoglines = new int[3] { 3, 4, 5 };
                            else { actors[i].dialoglines = new int[0]; }
                            break;
                        case 10:
                            if (dialogbox.currentdialog == 0)
                                actors[i].dialoglines = new int[2] { 0, 1 };
                            break;
                        default:
                            actors[i].dialoglines = new int[0];
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

       if(scenes[currentscene].GetComponent<SceneBehaviour>() != null)
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
        float deathtimer = 1000f;

        if (currentscene == 9 || currentscene == 10)
            if (dialogbox.dialog.currentdialogline == 4)
                deathanimator.SetBool("Death", true);
        
        if (deathanimator.GetBool("Death") == true)
        {
            deathtimer--;
            if(deathtimer <= 0)
                Application.Quit();
        }
    }

    #endregion

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

    #region Navegate Trougth Scenes

    void NavegateScenes()
    {
        switch(currentscene)
        {
            case 1:
                nextscene = 3;
                break;
            case 2:
                nextscene = 4;
                break;
            case 5:
                nextscene = 6;
                break;

        }
    }

    #endregion

    #endregion

    #endregion

    #endregion
}
