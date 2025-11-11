using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public AudioSource menuMusic;
    public AudioSource battleSelectMusic;
    public AudioSource[] bgm;
    private int currentBGM;
    private bool playingBGM;

    public AudioSource[] sfx;

    private bool suspendedByFocus = false;

    private void Update()
    {
        if (!playingBGM || suspendedByFocus) return;

        if (!bgm[currentBGM].isPlaying)
        {
            if (bgm[currentBGM].time > 0f)
            {
                currentBGM++;
                if (currentBGM >= bgm.Length) currentBGM = 0;

                bgm[currentBGM].time = 0f;
                bgm[currentBGM].Play();
            }
        }
    }

    public void StopMusic()
    {
        menuMusic.Stop();
        battleSelectMusic.Stop();
        foreach (AudioSource track in bgm)
        {
            track.Stop();
        }

        playingBGM = false;
    }

    public void PlayMenuMusic()
    {
        StopMusic();
        menuMusic.Play();
    }

    public void PlayBattleSelectMusic()
    {
        if (battleSelectMusic.isPlaying == false)
        {
            StopMusic();
            battleSelectMusic.Play();
        }
    }

    public void PlayBGM()
    {
        StopMusic();
        currentBGM = Random.Range(0, bgm.Length);
        bgm[currentBGM].time = 0f;
        bgm[currentBGM].Play();
        playingBGM = true;
    }

    public void PlaySFX (int sfxToPlay)
    {
        sfx[sfxToPlay].Stop();
        sfx[sfxToPlay].Play();
    }

    private void OnApplicationPause(bool pause)
    {
        suspendedByFocus = pause;
        if (pause)
        {
            if (playingBGM && bgm[currentBGM].isPlaying) bgm[currentBGM].Pause();
            if (menuMusic.isPlaying) menuMusic.Pause();
            if (battleSelectMusic.isPlaying) battleSelectMusic.Pause();
        }
        else
        {
            if (playingBGM && !bgm[currentBGM].isPlaying && bgm[currentBGM].time > 0f) bgm[currentBGM].UnPause();
            if (!menuMusic.isPlaying && menuMusic.time > 0f) menuMusic.UnPause();
            if (!battleSelectMusic.isPlaying && battleSelectMusic.time > 0f) battleSelectMusic.UnPause();
        }
    }

    // Just in case, some platforms send this instead of Pause.
    private void OnApplicationFocus(bool hasFocus)
    {
        suspendedByFocus = !hasFocus;
        if (!hasFocus)
        {
            if (playingBGM && bgm[currentBGM].isPlaying) bgm[currentBGM].Pause();
            if (menuMusic.isPlaying) menuMusic.Pause();
            if (battleSelectMusic.isPlaying) battleSelectMusic.Pause();
        }
        else
        {
            if (playingBGM && !bgm[currentBGM].isPlaying && bgm[currentBGM].time > 0f) bgm[currentBGM].UnPause();
            if (!menuMusic.isPlaying && menuMusic.time > 0f) menuMusic.UnPause();
            if (!battleSelectMusic.isPlaying && battleSelectMusic.time > 0f) battleSelectMusic.UnPause();
        }
    }
}
