using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NpcDialogueManager : MonoBehaviour
{
    public GameObject pressButtonText;
    public GameObject talkObject;
    public bool isInside;
    public bool isTalking;
    public Transform player;
    [TextArea(3, 10)]
    public string[] sentences;
    public string npcName;
    public TextMeshProUGUI listenText;
    public TextMeshProUGUI talkText;
    public Canvas canvas;
    public TMP_FontAsset fontChoice;
    public float letterTimer = 0f;
    public int currentLetter = 0;
    public bool isAnimating = false;
    public AudioClip[] audioClips;
    private AudioSource audioSource;
    private int currentSentence = 0;
    public Vector3 pressButtonScale;
    public Vector3 pressButtonPosition;
    public Vector2 pressButtonSize = new Vector2(200f, 50f);
    public Vector3 talkTextScale;
    public Vector3 talkTextPosition;
    public Vector2 talkTextSize = new Vector2(1057.101f, 106f);

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        canvas = GetComponentInChildren<Canvas>();
        audioSource = GetComponent<AudioSource>();
        pressButtonText = new GameObject("Listen Text");
        talkObject = new GameObject("TalkText");
        listenText = pressButtonText.AddComponent<TextMeshProUGUI>();
        talkText = talkObject.AddComponent<TextMeshProUGUI>();
        pressButtonText.transform.SetParent(canvas.transform, false);
        talkObject.transform.SetParent(canvas.transform, false);
        listenText.transform.localScale = pressButtonScale;
        listenText.transform.localPosition = pressButtonPosition;
        listenText.rectTransform.sizeDelta = pressButtonSize;
        listenText.alignment = TMPro.TextAlignmentOptions.Center;
        listenText.font = fontChoice;
        pressButtonText.SetActive(false);
        talkObject.SetActive(false);
        pressButtonScale = new Vector3(0.01f, 0.01f, 0.01f);
        pressButtonPosition=  new Vector3(0.01f, 0.01f, 0.01f);
        talkTextScale = new Vector3(0.01f, 0.01f, 0.01f);
        talkTextPosition= new Vector3(0f, 2.5f, 0f);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            listenText.text = "-Listen-";
            pressButtonText.SetActive(true);
            isInside = true;
            isTalking = false; 
            talkObject.transform.SetParent(canvas.transform, false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        pressButtonText.SetActive(false);
        talkObject.SetActive(false);
        isInside = false;
        isTalking = false; 
    }

    private void Update()
    {
        if (isInside && Input.GetKeyDown(KeyCode.W))
        {
            pressButtonText.SetActive(false);
            talkObject.SetActive(true);
            talkObject.transform.SetParent(canvas.transform, false);
            talkText.gameObject.SetActive(true);
            talkText.transform.localScale = talkTextScale;
            talkText.transform.localPosition = talkTextPosition;
            talkText.rectTransform.sizeDelta = talkTextSize;
            talkText.alignment = TMPro.TextAlignmentOptions.Center;
            talkText.font = fontChoice;
            isInside = false;
            player.rotation = Quaternion.Euler(0, 0, 0);
            player.GetComponent<GhostMovement>().enabled = false;
            isTalking = true;
            currentSentence = 0;
            StartTextAnimation(npcName + sentences[currentSentence]);
        }

        if (isTalking && Input.GetKeyDown(KeyCode.Z))
        {
            if (isAnimating) 
            {
               
                audioSource.Stop();
                currentLetter = talkText.text.Length;
                talkText.maxVisibleCharacters = currentLetter;
                isAnimating = false;
            }
            
                if (currentSentence < sentences.Length - 1)
                {
                    
                    currentSentence++;
                    StartTextAnimation(npcName + sentences[currentSentence]);
                }
                else
                {
                    
                    Debug.Log("End of NPC Dialogue");
                    talkText.gameObject.SetActive(false);
                    player.GetComponent<GhostMovement>().enabled = true;
                    currentSentence = 0;
                    isTalking = false;
                }
            }
        

       
        if (isAnimating)
        {
            
            letterTimer += Time.deltaTime;
            if (letterTimer >= 0.010f) 
            {
                letterTimer = 0f;
                currentLetter++;
                if (currentLetter <= talkText.text.Length)
                {
                    talkText.maxVisibleCharacters = currentLetter;
                }
                else
                {
                    isAnimating = false;
                }
            }
        }
    }

    private void StartTextAnimation(string sentence)
    {
    
        letterTimer = 0f;
        currentLetter = 0;
        isAnimating = true;
        audioSource.PlayOneShot(audioClips[currentSentence]); 
        talkText.text = "";
        talkText.maxVisibleCharacters = 0;
        talkText.SetText(sentence);
    }
}