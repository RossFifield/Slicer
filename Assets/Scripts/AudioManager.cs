using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private int maxAudio=5;
    private AudioSource audioPlayer;

    private static AudioManager instance = null;

    Dictionary<string,int> currentAudio = new Dictionary<string, int>();

    public static AudioManager GetInstance()
    {
        return instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("MORE THAN ONE INSTANCE OF GAME MANAGER AAAAAAAAAAAAAA");
        }

    }

    public void PlaySoundEffect(string soundpath, Transform position, float volume =1f){
        if(currentAudio.ContainsKey(soundpath)){
            if(currentAudio[soundpath]<maxAudio){
                currentAudio[soundpath] = currentAudio[soundpath] + 1;

                AudioClip clip = Resources.Load(soundpath) as AudioClip;
                audioPlayer.clip =clip;
                AudioSource.PlayClipAtPoint(clip,position.position,volume);
                StartCoroutine(SoundEndIn(clip.length, soundpath));
            }
        }
        else{
            currentAudio.Add(soundpath,1);
            AudioClip clip = Resources.Load(soundpath) as AudioClip;
            audioPlayer.clip = clip;
            AudioSource.PlayClipAtPoint(clip,position.position,volume);
            StartCoroutine(SoundEndIn(clip.length, soundpath));
        }
    }
    public void SoundEnded(string soundpath){
        currentAudio[soundpath] = currentAudio[soundpath] - 1;
    }

    IEnumerator SoundEndIn(float length, string soundPath){
        yield return new WaitForSeconds(length);
        SoundEnded(soundPath);
    }
}
