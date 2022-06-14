using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    private void Awake()
    {
        DontDestroyOnLoad(this);
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    public List<AudioClip> musicList = new List<AudioClip>();

    private AudioSource source;
    void Start()
    {
        GetSource();
        PlayMusic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayMusic()
    {
        int start = Random.Range(0, musicList.Count);
        StartCoroutine(AudioQueue(start));
    }
    private IEnumerator AudioQueue(int startpoint)
    {
        int index = startpoint;
        while (true)
        {
            if (!source.isPlaying)
            {
                source.clip = musicList[index];
                source.Play();
                if (index >= musicList.Count)
                {
                    index = 0;
                }
                else
                {
                    index++;
                    //Debug.Log("Index:" + index);
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
    public void MuteMusic()
    {
        source.mute = !source.mute;
    }
    public AudioSource GetSource()
    {
        if(source == null)
        {
            source = gameObject.GetComponent<AudioSource>();
        }
        return source;
    }
}
