using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NpcDialogueManager : MonoBehaviour
{
    #region private fields
    private AudioSource audioSource;
    private int currentSentence = 0;
    #endregion private fields
    #region public fields
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
    public Vector3 pressButtonScale;
    public Vector3 pressButtonPosition;
    public Vector2 pressButtonSize = new Vector2(200f, 50f);
    public Vector3 talkTextScale;
    public Vector3 talkTextPosition;
    public Vector2 talkTextSize = new Vector2(1057.101f, 106f);

    #endregion public fields
    private void Start()
    {
        //we find the player and set the canvas audiosource and ui elements
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (audioSource != null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            audioSource = this.gameObject.AddComponent<AudioSource>();
        }
        pressButtonText = new GameObject("Listen Text");
        talkObject = new GameObject("TalkText");
        canvas = new GameObject("NPCCanvas").AddComponent<Canvas>();
        canvas.transform.SetParent(transform, false);
        Debug.Log("Canvas created: " + canvas.name);
        canvas.renderMode = RenderMode.WorldSpace;
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        canvasRect.localPosition = new Vector3(0f, 1.5f, 0f);
        canvasRect.localRotation = Quaternion.Euler(0, 0, 0);
        canvasRect.sizeDelta = new Vector2(800f, 600f);

        listenText = pressButtonText.AddComponent<TextMeshProUGUI>();
        talkText = talkObject.AddComponent<TextMeshProUGUI>();


        listenText.transform.SetParent(canvas.transform, false);
        talkText.transform.SetParent(canvas.transform, false);

        pressButtonText.transform.SetParent(canvas.transform, false);
        talkObject.transform.SetParent(canvas.transform, false);

        //we set the scales and positions of our UI
        listenText.transform.localScale = new Vector3(0.02f, 0.05f, 0.01f);
        listenText.transform.localPosition = new Vector3(0,1f,0);
        listenText.text = "Press Z to Listen";
        listenText.rectTransform.sizeDelta = pressButtonSize;
        listenText.alignment = TMPro.TextAlignmentOptions.Center;
        listenText.font = fontChoice;
        pressButtonText.SetActive(false);
        talkObject.SetActive(false);
        pressButtonScale = new Vector3(0.01f, 0.05f, 0.01f);
        pressButtonPosition=  new Vector3(0.01f, 0.05f, 0.01f);
        talkTextScale = new Vector3(0.01f, 0.01f, 0.01f);
        talkTextPosition= new Vector3(0f, 2.5f, 0f);

        //canvas mode

       BoxCollider boxCollider =  this.gameObject.AddComponent<BoxCollider>();

        boxCollider.isTrigger = true;
    }
    #region triggers
    private void OnTriggerEnter(Collider other)
    {
        //if the tag is player we are inside and we apply the UI
        if (other.CompareTag("Player"))
        {
            listenText.text = "-Listen-";
            pressButtonText.SetActive(true);
            isInside = true;
            isTalking = false; 
            talkObject.transform.SetParent(canvas.transform, false);
        }
    }
    //if we exit the trigger we are not inside
    private void OnTriggerExit(Collider other)
    {
        pressButtonText.SetActive(false);
        talkObject.SetActive(false);
        isInside = false;
        isTalking = false; 
    }
    #endregion triggers

    private void Update()
    { 
        //if we are inside and we press w
        if (isInside && Input.GetKeyDown(KeyCode.W))
        {
            //we activate the correct UI and we set up the positions and scales and we rotate the player to face the NPC
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
            isTalking = true;
            currentSentence = 0;
            StartTextAnimation(npcName +":" + " " + sentences[currentSentence]);
        }
        //the dialogue gets advanced 
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
                    StartTextAnimation(npcName + ":" + " " + sentences[currentSentence]);
                }
                else
                {
                    
                    Debug.Log("End of NPC Dialogue");
                    talkText.gameObject.SetActive(false);
                    currentSentence = 0;
                    isTalking = false;
                }
            }
        

       //we continue the dialogue if we are still animating
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
    #region animationTexting
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
#endregion animationTexting