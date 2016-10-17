using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

    #region Attributes

    public Player player;
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
        
        ManageScenes();

        #endregion
        #region Game Control

        CheckIfTheDialogEndGoToNextScene();

        //GoToGameSceneWhen();

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

    }

    #endregion

    #region Dialog Answer Methods
    //method that prepare dialog for a answer moment and alert when is the moment of give the object
    void PrepareAnswersMoments()
    {
        if (currentscene == 0)
        {
            if (dialogbox.currentdialog == 0)
            {
                if (dialogbox.dialog.currentdialogline == 3)
                {
                    if (!dialogbox.hasanswered)
                        dialogbox.dialog.isanswermoment = true;
                }
                else
                {
                    dialogbox.hasanswered = false;
                }
            }
        }
    }

    #endregion

    #region Next Dialog Adjust Methods

    //method that adjust next dialog for current dialog not value has the final dialog causing the dialog to end
    void AdjustDialogDisplayBoxToNextDialog()
    {
        if (currentscene == 0)
        {
            if (dialogbox.currentdialog == 0)
            {
                if (dialogbox.lastanswerid < 0)
                {
                    dialogbox.nextdialog = dialogbox.endatdialog;
                    dialogbox.AnswerButtonsSetNextDialog(1, 2, 3);
                }
            }
        }
    }

    #endregion

    #region Next Scene Adjust Methods

    void AdjustDialogDisplayBoxToNextScene()
    {
        if(currentscene == 0)
        {
            if(dialogbox.currentdialog == 0)
            {
                if(dialogbox.dialog.currentdialogline <= dialogbox.dialog.enddialogatline)
                {
                    dialogbox.AnswerButtonsSetNextScene(1, 2, 0);
                }
            }
        }
        if(currentscene == 1)
        {
            if(dialogbox.currentdialog == 0)
            {
                if(dialogbox.dialog.currentdialogline <= dialogbox.dialog.enddialogatline)
                {
                    dialogbox.AnswerButtonsSetNextScene(3, 4, 0);
                }
            }
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
        for(int i = 0; i < actors.Length; i ++)
        {
            if (actors[i].name == "Patrick")
            {
                if (currentscene == 0)
                    if (dialogbox.currentdialog == 0)
                        actors[i].dialoglines = new int[4] { 0, 1, 2, 3 };
                    else { actors[i].dialoglines = new int[0]; }
                if (currentscene == 1)
                    if (dialogbox.currentdialog == 0)
                        actors[i].dialoglines = new int[5] { 3, 4, 5, 6, 7 };
                    else { actors[i].dialoglines = new int[0]; }
                if (currentscene == 7)
                    if (dialogbox.currentdialog == 0)
                        actors[i].dialoglines = new int[5] { 2, 3, 4, 5, 6 };
                    else { actors[i].dialoglines = new int[0]; }
            }
            if(actors[i].name == "Homem de Capuz")
            {
                if (currentscene == 3)
                {
                    if (dialogbox.currentdialog == 1)
                        actors[i].dialoglines = new int[3] { 0, 1, 2 };
                    else { actors[i].dialoglines = new int[0]; }
                    if (dialogbox.currentdialog == 2)
                        actors[i].dialoglines = new int[3] { 0, 1, 2 };
                    else { actors[i].dialoglines = new int[0]; }
                }
            }
            if (actors[i].name == "Vizinho de Patrick")
            {
                if (currentscene == 6)
                {
                    if (dialogbox.currentdialog == 0)
                        actors[i].dialoglines = new int[1] { 7 };
                    else { actors[i].dialoglines = new int[0]; }
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
                case 3: scenes[i].scenename = "Casa Da Arvore"; break;
                case 4: scenes[i].scenename = ""; break;
                case 5: scenes[i].scenename = ""; break;
                case 6: scenes[i].scenename = ""; break;
                case 7: scenes[i].scenename = ""; break;
                case 8: scenes[i].scenename = ""; break;
                case 9: scenes[i].scenename = ""; break;
                case 10: scenes[i].scenename = ""; break;
                case 11: scenes[i].scenename = ""; break;
            }
            Debug.Log("Scene " + scenes[i].scenename + " Avaliable");
        }
    }

    /*
    void ActiveCurrentSceneAndDisableOthers()
    {
        for (int i = 0; i < scenes.Length; i++)
            if (scenes[i].sceneid == currentscene)
                scenes[i].gameObject.SetActive(true);
            else { scenes[i].gameObject.SetActive(false); }
    }
    */
    /*
    void UpdateSceneBehaviours()
    {

        for (int i = 0; i < scenes.Length; i++)
            if (scenes[i].gameObject.activeInHierarchy)
                if(scenes[i].GetComponent<SceneBehaviour>() != null)
                    scenes[i].GetComponent<SceneBehaviour>().UpdateSceneBehaviour();                
    }
    */

    void ManageScenes()
    {
        if (currentscene == 0)
            OnDialogEndGoTo(1);

        if (currentscene == 3)
        {
            if (dialogbox.currentdialog >= 1 && dialogbox.currentdialog <= 2)
            {
                OnDialogEndGoTo(4);
            }
            if(dialogbox.currentdialog == 3)
            {
                OnDialogEndGoTo(6);
            }
        }

        if (currentscene == 5)
        {
            TimeToSetScene();
            OnDialogEndGoTo(6);
        }

        if (currentscene == 6)
        {
            OnTimeEndGoTo(7);
        }
    }

    #region Scene progression Cases Methods

    void OnDialogEndGoTo(int SceneToGo)
    {
        if (canprogress && !dialogbox.gameObject.activeInHierarchy)
        {
            currentscene = SceneToGo;
            dialogbox.StartDialog(0);
            Debug.Log("You has go to scene" + SceneToGo);
        }
    }
    void TimeToSetScene()
    {
        SceneBehaviour currentscenebehaviour;

       if(scenes[currentscene].GetComponent<SceneBehaviour>() != null)
            currentscenebehaviour = scenes[currentscene].GetComponent<SceneBehaviour>();
        else
        {
            scenes[currentscene].gameObject.AddComponent<SceneBehaviour>();
            currentscenebehaviour = scenes[currentscene].GetComponent<SceneBehaviour>();
            currentscenebehaviour.timertostart = 100;
        }

        if (currentscenebehaviour != null)
        {
            if (currentscenebehaviour.timertostart > 0)
            {
                currentscenebehaviour.timertostart--;
                if (canprogress == true)
                    canprogress = false;
                Debug.Log("Tempo para liberar progress�o " + currentscenebehaviour.timertostart);
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
    void OnTimeEndGoTo(int SceneToGo)
    {
        SceneBehaviour currentscenebehaviour;

        currentscenebehaviour = scenes[currentscene].GetComponent<SceneBehaviour>();

            if (canprogress && currentscenebehaviour.timertogo > 0)
            {
                currentscenebehaviour.timertogo--;
                Debug.Log("Tempo para acontecer proximo evento " + currentscenebehaviour.timertogo);
            }
            else
            {
                currentscene = SceneToGo;
                dialogbox.StartDialog(0);
                Debug.Log("You has go to Scene " + SceneToGo);
            }

    }

    #endregion

    #endregion

    #region Game Methods

    #region CheckIfTheDialoghasEndGoToNextScene

    void CheckIfTheDialogEndGoToNextScene()
    {
        if (scenes[currentscene].scenestate == Scene.state.interaction)
            currentscene = nextscene;
    }

    #endregion

    #region Navegation Methods
    void GoToGameSceneWhen()
    {
        switch(currentscene)
        {
            case 0:
                if (dialogbox.currentdialog == 1)
                {
                    if (dialogbox.dialog.currentdialogline == dialogbox.dialog.enddialogatline)
                    {
                        currentscene = 1;
                    }
                }
                if (dialogbox.currentdialog == 2)
                {
                    if (dialogbox.dialog.currentdialogline == dialogbox.dialog.enddialogatline)
                    {
                        currentscene = 2;
                    }
                }
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
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

    #endregion

    #endregion
}
