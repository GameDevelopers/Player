using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogManager : MonoBehaviour, IPointerDownHandler
{
    public Text dialogText;
    public GameObject nextText;

    public Queue<string> sentences;

    private string currentSentence;
    public float typingSpeed = 0.1f;

    public bool istyping = false;
    public bool jump = false;

    public CanvasGroup dialoggroup;

    public static DialogManager instance;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        sentences = new Queue<string>();    
    }

    public void Ondialog(string[] lines)
    {
        sentences.Clear();
        foreach(string line in lines)
        {
            sentences.Enqueue(line);
        }
        dialoggroup.alpha = 1;
        dialoggroup.blocksRaycasts = true;

        NextSentence();
    }

    public void NextSentence()
    {
        if (sentences.Count != 0)
        {
            currentSentence = sentences.Dequeue();
            //코루틴 타이핑효과
            istyping = true;
            nextText.SetActive(false);
            StartCoroutine(Typing(currentSentence));
        }
        else
        {
            dialoggroup.alpha = 0;
            dialoggroup.blocksRaycasts = false;
            jump = false;
        }
    }

    IEnumerator Typing(string line)
    {
        dialogText.text = "";
        foreach( char letter in line.ToCharArray())
        {
            dialogText.text += letter;
            if (jump) yield return null;
            else yield return new WaitForSeconds(typingSpeed);
        }
        istyping = false;
    }

    void Update()
    {
        // 대사 한줄 끝
        if (dialogText.text.Equals(currentSentence))
        {
            nextText.SetActive(true);
            istyping = false;
        }

         //if (Input.GetKeyDown(KeyCode.Space))
         //{
         //   if (!istyping)
         //       NextSentence();
         //}
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!istyping)
        NextSentence();
    }


}
