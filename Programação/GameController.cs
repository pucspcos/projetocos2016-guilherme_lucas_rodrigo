using UnityEngine;
using UnityStandardAssets.ImageEffects;
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
        if (gamedata == null)
            gamedata = GameObject.Find("GameData").GetComponent<GameData>();
        if (gamesettings == null)
            gamesettings = GameObject.Find("GameSettings").GetComponent<GameSettings>();
        if (player == null)
            player = GameObject.Find("Player Main Camera").GetComponent<Player>();
        if (playereyesfilter == null)
            playereyesfilter = GameObject.Find("PlayerEyesEffect").GetComponent<CameraEyeEffect>();
        if (background == null)
            background = GameObject.Find("Background").GetComponent<Background>();
        if (musicplayer == null)
            musicplayer = GameObject.Find("MusicPlayer").GetComponent<MusicPlayer>();
        if (dialogbox == null)
            dialogbox = GameObject.Find("DialogDisplayBox").GetComponent<DialogBox>();
        if (pausemenu == null)
            pausemenu = GameObject.Find("PauseMenu").GetComponent<PauseMenu>();

        if (actors == null || actors.Length <= 0)
            Debug.LogWarning("Game Controller has no actors");
        if (scenes == null || scenes.Length <= 0)
            Debug.LogError("No scene on the game controller");
        }
	void Start () {

        SetScenesID();

        SetScenesName();

        LoadAllGameAndPlayerDataToCurrentGame();

        player.inventary = new System.Collections.Generic.List<string>();

        musicplayer.PlayMusic();

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
        #region Music Control

        if (musicplayer.music.volume != gamesettings.musicvolume)
            musicplayer.music.volume = gamesettings.musicvolume;

        if (musicplayer.endatmusicclip != scenes[currentscene].musics.Length)
            musicplayer.LimitMusicLengh(scenes[currentscene].musics.Length - 1);

        if (scenes[currentscene].musics.Length > 0)
            musicplayer.music.clip = scenes[currentscene].musics[musicplayer.currentmusicclip];

        if (!musicplayer.music.isPlaying)
            musicplayer.NextMusic();

        #endregion
        #region Dialog Control

        PrepareAnswersMoments();

        dialogbox.DialogUpdate();

        UploadAnswersValue();

        AdjustDialogDisplayBoxToNextDialog();

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
        #region ObjectsI Control

        UpdateObjectIScene();

        LoadObjectsIInCurrentScene();

        UpdateObjectIVolume();

        #endregion
        #region Puzzle Control

        UpdatePuzzleScene();

        UpdatePuzzle();

        LoadPuzzleInCurrentScene();

        UpdatePuzzleIVolume();

        CheckIfPlayerHasResolvedPuzzle();

        #endregion
        #region Scene Control
        /*
        UpdateSceneBehaviours();
        
        ActiveCurrentSceneAndDisableOthers();
        */
        ManageScenes();

        #endregion
        #region Game Control

        GoToGameSceneWhen();

        SaveAllGameAndPlayerDataOfCurrentGame();

        #endregion
        #region Player Control

        if (Input.GetButtonDown("Confirm"))
            if(canprogress)
                dialogbox.Processed();

        if (Input.GetButtonDown("Pause"))
        {
            if(pausemenu.gameObject.activeInHierarchy)
            {
                pausemenu.hideactors = false;
                pausemenu.affinitybarbox.SetActive(false);
                pausemenu.helpbox.SetActive(false);
                pausemenu.gameObject.SetActive(false);
            }
            else
            {
                pausemenu.OnClickPauseGame();
            }
        }

        #endregion
        #region Player Eye Control

        playereyesfilter.hasaction = CheckPlayerEyesActionMoments();

        UpdateCameraEyeMoments();

        UpdateCameraEyeEffect();

        playereyesfilter.CameraEyesUpdate();

        #endregion
    }
    #endregion

    #region GameController Fundamental Methods

    #region Dialog Methods

    #region Dialog Answer Methods
    //method that prepare dialog for a answer moment and alert when is the moment of give the object
    void PrepareAnswersMoments()
    {
        if (currentscene == 0)
        {
            if (dialogbox.currentdialog == 0)
            {
                if (dialogbox.dialog.currentdialogline == 9)
                    if(!dialogbox.hasanswered) 
                        dialogbox.dialog.isanswermoment = true;
                if (dialogbox.dialog.currentdialogline == 10)
                    dialogbox.hasanswered = false;
            }
            else
            {
                if (dialogbox.currentdialog != 4)
                {
                    if (dialogbox.dialog.currentdialogline == dialogbox.dialog.enddialogatline)
                    {
                        if (!dialogbox.hasanswered)
                            dialogbox.dialog.isanswermoment = true;
                    }
                    else
                    {
                        dialogbox.hasanswered = false;
                    }
                }
                else
                {
                    if (dialogbox.dialog.currentdialogline == dialogbox.dialog.enddialogatline)
                        dialogbox.nextdialog = dialogbox.currentdialog;
                }
            }
        }
        if (currentscene == 3)
        {
            if (dialogbox.currentdialog == 0)
            {
                if (dialogbox.dialog.currentdialogline == 5)
                    if (!dialogbox.hasanswered)
                        dialogbox.dialog.isanswermoment = true;
                if (dialogbox.dialog.currentdialogline == 6)
                    dialogbox.hasanswered = false;
            }
        }
        if(currentscene == 7)
        {
            if(dialogbox.currentdialog == 0)
            {
                if (dialogbox.dialog.currentdialogline == 2)
                    if (!dialogbox.hasanswered)
                        dialogbox.dialog.isanswermoment = true;
                if (dialogbox.dialog.currentdialogline == 3)
                    dialogbox.hasanswered = false;
            }
        }
        #region Test
        /*
        if (currentscene == 1)
        {
            if (dialogbox.currentdialog == 0)
            {
                if (dialogbox.dialog.currentdialogline == dialogbox.dialog.enddialogatline)
                    if (!dialogbox.hasanswered) 
                        dialogbox.dialog.isanswermoment = true;
            }
            if(dialogbox.currentdialog != 0)
            {
                dialogbox.hasanswered = false;
            }
        }
        */
        #endregion
    }

    //method that give the value of affinity the answer buttons will have 
    void UploadAnswersValue()
    {
        if (currentscene == 7)
        {
            if (dialogbox.currentdialog == 0)
            {
                if(dialogbox.dialog.currentdialogline == 2)
                    dialogbox.SetAnswerButtonsValue(5, 10, 3);
            }
        }
        if (currentscene == 1)
        {
            if (dialogbox.currentdialog == 0)
            {
                dialogbox.SetAnswerButtonsValue(0, 0, 0);
            }
        }
    }

    #endregion

    #region Next Dialog Adjust Methods

    //method that adjust next dialog for current dialog not value has the final dialog causing the dialog to end
    void AdjustDialogDisplayBoxToNextDialog()
    {
        if(currentscene == 0)
        {
            if (dialogbox.currentdialog == 0)
            {
                answersresultreaded = new bool[3];
                if (dialogbox.lastanswerid < 0)
                {
                    dialogbox.nextdialog = 5;
                    dialogbox.AnswerButtonsSetNextDialog(1, 2, 3);
                }
            }
            else
            {

                if(CheckAnswersResultReaded() == true)
                {
                    dialogbox.nextdialog = 4;
                }

                if (dialogbox.currentdialog == 1)
                {
                    if (answersresultreaded.Length <= 0)
                        answersresultreaded = new bool[3];
                    else
                    {
                        answersresultreaded[0] = true;
                    }
                }
                if (dialogbox.currentdialog == 2)
                {
                    if (answersresultreaded.Length <= 0)
                        answersresultreaded = new bool[3];
                    else
                    {
                        answersresultreaded[1] = true;
                    }
                }
                if (dialogbox.currentdialog == 2)
                {
                    if (answersresultreaded.Length <= 0)
                        answersresultreaded = new bool[3];
                    else
                    {
                        answersresultreaded[2] = true;
                    }
                }

                if (dialogbox.nextdialog == dialogbox.currentdialog)
                    dialogbox.nextdialog++;

            }
        }
        else if(currentscene >= 1 && currentscene < 3)
        {
            if(dialogbox.currentdialog == 0)
            {
                if(dialogbox.dialog.currentdialogline != dialogbox.dialog.enddialogatline)
                    dialogbox.nextdialog = 1;
                else { dialogbox.nextdialog = dialogbox.currentdialog; }
            }
        }
        else if (currentscene == 3)
        {
            if (dialogbox.currentdialog == 0)
            {
                dialogbox.AnswerButtonsSetNextDialog(1, 2, 3);
                if (dialogbox.lastanswerid < 0)
                    dialogbox.nextdialog = 1;
                
            }
        }
        else if (currentscene >= 4 && currentscene < 8)
        {
            if (dialogbox.currentdialog == 0)
            {
                if (dialogbox.dialog.currentdialogline != dialogbox.dialog.enddialogatline)
                    dialogbox.nextdialog = 1;
                else { dialogbox.nextdialog = dialogbox.currentdialog; }
            }
        }
    }

    #endregion

    #region Check AnswerResultReaded

    //Method that check if the player has read all the possible results of a dialog questions
    bool CheckAnswersResultReaded()
    {
        //create a counter that will add values to it has the player has read the results
        int count = 0;
        //create a whip of type for that will check all answerresultreaded to know how much results the player has read
        for(int i = 0; i < answersresultreaded.Length; i ++)
        {
            if (answersresultreaded[i] == true)
                count++;
            else
            {
                return false;
            }
        }
        //Check if the player has read all possible answer result
        if (count >= answersresultreaded.Length)
            return true;

        return false;
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
            if (!pausemenu.hideactors)
            {
                if (actors[i].name == "Enzo")
                {
                    if (currentscene == 0)
                        if (dialogbox.currentdialog == 0)
                            actors[i].dialoglines = new int[5] { 3, 4, 5, 6, 7 };
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
            }
        }
    }
    void PrepareActorExpressions()
    {
        for (int i = 0; i < actors.Length; i++)
        {
            if (!pausemenu.hideactors)
            {
                if (actors[i].name == "Enzo")
                {
                    if (currentscene == 7)
                    {
                        if (dialogbox.currentdialog == 0)
                        {
                            if (dialogbox.dialog.currentdialogline >= 0 && dialogbox.dialog.currentdialogline < 2)
                                //actors[i].GetComponent<SimpleModel>()
                                Debug.Log("Enzo has do a expression");
                        }
                        else { actors[i].dialoglines = new int[0]; }
                    }
                    else { actors[i].dialoglines = new int[0]; }
                }
            }
            else
            {
                actors[i].dialoglines = new int[0];
            }
        }
    }
    #endregion

    #region ObjectI Methods

    void UpdateObjectIScene()
    {
        foreach (GameObject objecti in scenes[currentscene].objects)
        {
            if (objecti != null)
                objecti.GetComponent<ObjectI>().scene = scenes[currentscene];
        }
    }
    void LoadObjectsIInCurrentScene()
    {
        for (int i = 0; i < scenes.Length; i++)
            if(i != currentscene)
                foreach (GameObject objecti in scenes[i].objects)
                    objecti.SetActive(false);

        foreach (GameObject objecti in scenes[currentscene].objects)
            if (objecti.GetComponent<ObjectI>().isavailable)
                objecti.SetActive(true);
    }
    void UpdateObjectIVolume()
    {
        foreach (GameObject objecti in scenes[currentscene].objects)
            if (objecti.GetComponent<ObjectI>().objectsound.volume != gamesettings.effectsvolume)
                objecti.GetComponent<ObjectI>().objectsound.volume = gamesettings.effectsvolume;
    }

    #endregion

    #region Puzzle Methods

    void UpdatePuzzleScene()
    {
        foreach (Puzzle puzzle in scenes[currentscene].puzzles)
        {
            if (puzzle != null)
                puzzle.scene = scenes[currentscene];
        }
    }
    void UpdatePuzzle()
    {
        foreach (Puzzle puzzlei in scenes[currentscene].puzzles)
        {
            if (!puzzlei.resolved && puzzlei.gameObject.activeInHierarchy)
                if (puzzlei.GetComponent<PuzzlePicture>() != null)
                    puzzlei.GetComponent<PuzzlePicture>().UpdatePicuteStatus();
        }
    }
    void LoadPuzzleInCurrentScene()
    {
        for (int i = 0; i < scenes.Length; i++)
            if (i != currentscene)
                foreach (Puzzle puzzle in scenes[i].puzzles)
                    puzzle.gameObject.SetActive(false);

        foreach (Puzzle puzzle in scenes[currentscene].puzzles)
            if (!puzzle.resolved)
                puzzle.gameObject.SetActive(true);
    }
    void UpdatePuzzleIVolume()
    {
        foreach (Puzzle puzzle in scenes[currentscene].puzzles)
            if (puzzle.puzzlesound.volume != gamesettings.effectsvolume)
                puzzle.puzzlesound.volume = gamesettings.effectsvolume;
    }
    void CheckIfPlayerHasResolvedPuzzle()
    {
        foreach (string puzzle in player.inventary)
            foreach (Puzzle puzzleinscene in scenes[currentscene].puzzles)
                if (puzzle == puzzleinscene.gameObject.name)
                    puzzleinscene.resolved = true;
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
                case 0: scenes[i].scenename = "Sonho"; break;
                case 1: scenes[i].scenename = "Quarto"; break;
                case 2: scenes[i].scenename = "Sala de Estar"; break;
                case 3: scenes[i].scenename = "Rua Casa Abandonada"; break;
                case 4: scenes[i].scenename = "Sala de Aula"; break;
                case 5: scenes[i].scenename = "Rua da Escola"; break;
                case 6: scenes[i].scenename = "Jardim da Casa Abandonada"; break;
                case 7: scenes[i].scenename = "Sala de Aula Magica"; break;
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
    void OnDialogEndSaveGameProgress()
    {
        if (!dialogbox.gameObject.activeInHierarchy)
        {
            gamedata.SaveAllPlayerData();
            gamedata.SaveAllGameData();
            Debug.Log("Game has been save");
        }
    }
    void OnDialogEndSaveGameAndLoadGameScene(int DialogReference, int DialogLineReference, int GameSceneToGo)
    {
        if (dialogbox.currentdialog == DialogReference && dialogbox.dialog.currentdialogline == DialogLineReference)
        {
            gamedata.SaveAllPlayerData();
            gamedata.SaveAllGameData();
            if (!gamedata.issaving || !gamedata.isloading)
                Application.LoadLevel(GameSceneToGo);
        }
    }

    #endregion

    #endregion

    #region Player Eyes Filter Methods

    bool CheckPlayerEyesActionMoments()
    {
        foreach (int actionmoment in playereyesfilter.moments)
            if (actionmoment == dialogbox.dialog.currentdialogline)
                return true;

        return false;
    }

    void UpdateCameraEyeMoments()
    {
        if (currentscene == 0)
        {
            if (dialogbox.currentdialog == 0)
               playereyesfilter.moments = new int[1] { 2 };
        }
        else { playereyesfilter.moments = new int[0]; }

    }

    void UpdateCameraEyeEffect()
    {
        if(currentscene == 0)
        {
            if(dialogbox.currentdialog == 0)
            {
                if(dialogbox.dialog.currentdialogline == 0)
                {
                    playereyesfilter.state = CameraEyeEffect.effectstate.Open;
                }
                if(dialogbox.dialog.currentdialogline == 3)
                {
                    playereyesfilter.state = CameraEyeEffect.effectstate.Close;
                }
            }
        }
    }

    #endregion

    #region Game Methods

    #region Navegation Methods
    void GoToGameSceneWhen()
    {
        if (currentscene == 0)
        {
            if (dialogbox.currentdialog == 0)
            {
                if(dialogbox.dialog.currentdialogline == 6)
                {
                    gamedata.SaveAllPlayerData();
                    gamedata.SaveAllGameData();
                    if (!gamedata.issaving || !gamedata.isloading)
                    { 
                        if (player.playername.Length <= 0) 
                            Application.LoadLevel(9);
                    }
                }
            }
        }
        if (currentscene == 7)
        {
            if (dialogbox.currentdialog == 0)
            {
                if (dialogbox.dialog.currentdialogline >= 0)
                {
                    if (gamedata.playercurrentactor == "")
                    {
                        gamedata.SaveAllPlayerData();
                        gamedata.SaveAllGameData();
                        if (!gamedata.issaving || !gamedata.isloading)
                        {
                            Application.LoadLevel(8);
                        }
                    }
                }
            }
        }
    }
    #endregion

    #region Time Adjust Methods

    #endregion

    #region Check Methods
    bool CheckDataChanges()
    {
        if (player.playercurrentactor != "")
        {
            if (player.playercurrentactor == "Enzo")
            {
                if (gamedata.playername != player.playername
                    || gamedata.playercurrentactor != player.playercurrentactor

                    || gamedata.currentenzoaffinity != player.currentactoraffinity

                    || gamedata.playercurrentscene != currentscene
                    || gamedata.playercurrenttextfile != dialogbox.currentdialog
                    || gamedata.playercurrentdialogline != dialogbox.dialog.currentdialogline
                    || gamedata.currentscenestate != scenes[currentscene].scenestate.ToString()
                    || gamedata.currentdialoganswerstate != dialogbox.hasanswered.ToString())
                    return true;
                else { return false; }
            }
            if (player.playercurrentactor == "Isis")
            {
                if (gamedata.playername != player.playername
                    || gamedata.playercurrentactor != player.playercurrentactor

                    || gamedata.currentisisaffinity != player.currentactoraffinity

                    || gamedata.playercurrentscene != currentscene
                    || gamedata.playercurrenttextfile != dialogbox.currentdialog
                    || gamedata.playercurrentdialogline != dialogbox.dialog.currentdialogline
                    || gamedata.currentscenestate != scenes[currentscene].scenestate.ToString()
                    || gamedata.currentdialoganswerstate != dialogbox.hasanswered.ToString())
                    return true;
                else { return false; }
            }
            if (player.playercurrentactor == "Benjamin")
            {
                if (gamedata.playername != player.playername
                    || gamedata.playercurrentactor != player.playercurrentactor

                    || gamedata.currentbenjaminaffinity != player.currentactoraffinity

                    || gamedata.playercurrentscene != currentscene
                    || gamedata.playercurrenttextfile != dialogbox.currentdialog
                    || gamedata.playercurrentdialogline != dialogbox.dialog.currentdialogline
                    || gamedata.currentscenestate != scenes[currentscene].scenestate.ToString()
                    || gamedata.currentdialoganswerstate != dialogbox.hasanswered.ToString())
                    return true;
                else { return false; }
            }
            if (player.playercurrentactor == "Malika")
            {
                if (gamedata.playername != player.playername
                    || gamedata.playercurrentactor != player.playercurrentactor

                    || gamedata.currentmalikaaffinity != player.currentactoraffinity

                    || gamedata.playercurrentscene != currentscene
                    || gamedata.playercurrenttextfile != dialogbox.currentdialog
                    || gamedata.playercurrentdialogline != dialogbox.dialog.currentdialogline
                    || gamedata.currentscenestate != scenes[currentscene].scenestate.ToString()
                    || gamedata.currentdialoganswerstate != dialogbox.hasanswered.ToString())
                    return true;
                else { return false; }
            }
            if (player.playercurrentactor == "Zaki")
            {
                if (gamedata.playername != player.playername
                    || gamedata.playercurrentactor != player.playercurrentactor

                    || gamedata.currentzakiaffinity != player.currentactoraffinity

                    || gamedata.playercurrentscene != currentscene
                    || gamedata.playercurrenttextfile != dialogbox.currentdialog
                    || gamedata.playercurrentdialogline != dialogbox.dialog.currentdialogline
                    || gamedata.currentscenestate != scenes[currentscene].scenestate.ToString()
                    || gamedata.currentdialoganswerstate != dialogbox.hasanswered.ToString())
                    return true;
                else { return false; }
            }
        }
        else
        {
            if (gamedata.playername != player.playername
                || gamedata.playercurrentactor != player.playercurrentactor

                || gamedata.playercurrentscene != currentscene
                || gamedata.playercurrenttextfile != dialogbox.currentdialog
                || gamedata.playercurrentdialogline != dialogbox.dialog.currentdialogline
                || gamedata.currentscenestate != scenes[currentscene].scenestate.ToString()
                || gamedata.currentdialoganswerstate != dialogbox.hasanswered.ToString())
                return true;
        }

        return false;
    }

    #endregion

    #region SaveData Methods
    void SaveAllGameAndPlayerDataOfCurrentGame()
    {
        gamedata.issaving = CheckDataChanges();
        if(gamedata.issaving)
        {
            gamedata.playername = player.playername;
            gamedata.playercurrentactor = player.playercurrentactor;

            if (player.playercurrentactor != "")
            {
                if (player.playercurrentactor == "Enzo")
                    gamedata.currentenzoaffinity = player.currentactoraffinity;
                if (player.playercurrentactor == "Isis")
                    gamedata.currentisisaffinity = player.currentactoraffinity;
                if (player.playercurrentactor == "Benjamin")
                    gamedata.currentbenjaminaffinity = player.currentactoraffinity;
                if (player.playercurrentactor == "Malika")
                    gamedata.currentmalikaaffinity = player.currentactoraffinity;
                if (player.playercurrentactor == "Zaki")
                    gamedata.currentzakiaffinity = player.currentactoraffinity;
            }

            gamedata.playercurrentscene = currentscene;
            gamedata.playercurrenttextfile = dialogbox.currentdialog;
            gamedata.playercurrentdialogline = dialogbox.dialog.currentdialogline;
            gamedata.currentscenestate = scenes[currentscene].scenestate.ToString();
            gamedata.currentdialoganswerstate = dialogbox.hasanswered.ToString();

            gamedata.gameprogress = currentscene+1;
            gamedata.SetData();
            gamedata.playtime = Time.time;

            gamedata.SaveAllPlayerData();
            gamedata.SaveAllGameData();
        }
    }
    #endregion

    #region LoadData Methods
    void LoadAllGameAndPlayerDataToCurrentGame()
    {
        //Load all GameData that as to load
        gamedata.LoadAllPlayerData();
        gamedata.LoadAllGameData();

        gamedata.LoadLoadRequest();
        gamedata.LoadSaveRequest();

        //Put the values of the gamedata in the game to it run
        if (gamedata.playername != "")
        {
            //Update player name
            player.playername = gamedata.playername;
        }
        else { Debug.LogWarning("The player has no nome"); }
        if (gamedata.playercurrentactor != "")
        {
            //update player current actor
            player.playercurrentactor = gamedata.playercurrentactor;

            //check which actor history line is the player and update it current
            // affinity with that player
            if (gamedata.playercurrentactor == "Enzo")
                player.currentactoraffinity = gamedata.currentenzoaffinity;
            if (gamedata.playercurrentactor == "Isis")
                player.currentactoraffinity = gamedata.currentisisaffinity;
            if (gamedata.playercurrentactor == "Benjamin")
                player.currentactoraffinity = gamedata.currentbenjaminaffinity;
            if (gamedata.playercurrentactor == "Malika")
                player.currentactoraffinity = gamedata.currentmalikaaffinity;
            if (gamedata.playercurrentactor == "Zaki")
                player.currentactoraffinity = gamedata.currentzakiaffinity;
        }
        else { Debug.LogWarning("The player has no actor history line"); }

        //Put the values of the player current state in the history of the game
        currentscene = gamedata.playercurrentscene;
        dialogbox.currentdialog = gamedata.playercurrenttextfile;
        dialogbox.dialog.currentdialogline = gamedata.playercurrentdialogline;
        if (gamedata.currentscenestate == "dialog")
            scenes[currentscene].scenestate = Scene.state.dialog;
        if (gamedata.currentscenestate == "interaction")
            scenes[currentscene].scenestate = Scene.state.interaction;
        if (gamedata.currentscenestate == "puzzle")
            scenes[currentscene].scenestate = Scene.state.puzzle;
        if (gamedata.currentdialoganswerstate == "True")
            dialogbox.hasanswered = true;
        if (gamedata.currentdialoganswerstate == "False")
            dialogbox.hasanswered = false;

    }
    #endregion

    #endregion

    #endregion

    #endregion
}
