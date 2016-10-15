using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(RawImage))]
[RequireComponent(typeof(AudioSource))]
public class LiveBackground : MonoBehaviour {

    #region Attributes
    public enum LiveBackgroundBehaviour { random, inorder, justone }
    public LiveBackgroundBehaviour livebackgroundbehaviour;

    public MovieTexture[] movie;
    private AudioSource movieaudio;

    public int currentmovie;
    public int nextmovie;

    public bool replayable;
    public bool playautomatic;
    public bool hasended;

    #endregion

    #region Methods

    #region Awake And Start

    void Awake()
    {
        movieaudio = GetComponent<AudioSource>();
        UploadLiveBackground();
    }
    void Start()
    {
        if (movie.Length > 0)
        {
            if (replayable)
            {
                movie[currentmovie].loop = true;
                movieaudio.loop = true;
            }
            else
            {
                movie[currentmovie].loop = false;
                movieaudio.loop = false;
            }
            if (playautomatic)
            {
                movie[currentmovie].Play();
                movieaudio.Play();
            }
        }
    }

    #endregion

    #region Update Method

    public void UpdateLiveBackground()
    {
        hasended = CheckMovieEnd();
        if (hasended)
            UploadLiveBackground();
    }


    #endregion

    #region Live Background Fundamental Methods
    
    #region Check Methods

    bool CheckMovieEnd()
    {
        if (!movie[currentmovie].isPlaying)
            return true;

        return false;
    }

    #endregion

    #region Upload LiveBackground Methods

    public void UploadLiveBackground()
    {
        if (movie.Length > 0)
        {
            if (livebackgroundbehaviour == LiveBackgroundBehaviour.justone)
            {
                GetComponent<RawImage>().texture = movie[currentmovie] as MovieTexture;
                movieaudio.clip = movie[currentmovie].audioClip;

                movie[currentmovie].Play();
                movieaudio.Play();
            }
            if (livebackgroundbehaviour == LiveBackgroundBehaviour.random)
            {
                nextmovie = Random.Range(0, movie.Length);
                currentmovie = nextmovie;
                GetComponent<RawImage>().texture = movie[currentmovie] as MovieTexture;
                movieaudio.clip = movie[currentmovie].audioClip;

                movie[currentmovie].Play();
                movieaudio.Play();
            }
            if(livebackgroundbehaviour == LiveBackgroundBehaviour.inorder)
            {
                if (nextmovie < movie.Length - 1)
                {
                    nextmovie++;
                    currentmovie = nextmovie;
                }
                else
                {
                    nextmovie = 0;
                }

                GetComponent<RawImage>().texture = movie[currentmovie] as MovieTexture;
                movieaudio.clip = movie[currentmovie].audioClip;

                movie[currentmovie].Play();
                movieaudio.Play();
            }
        }
        else
        {
            Debug.Log("No movies to upload");
        }
    }

    #endregion

    #endregion

    #endregion

}
