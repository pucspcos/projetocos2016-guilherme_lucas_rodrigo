using UnityEngine;
using System.Collections;

public class Scene : MonoBehaviour {

    public enum state { dialog, interaction, puzzle };
    public state scenestate;
    public MovieTexture[] livebackground;
    public Sprite[] backgrounds;
    public TextAsset[] dialogs;
    public TextAsset[] answers;

    public string scenename;
    public int sceneid;
}
