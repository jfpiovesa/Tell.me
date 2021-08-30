using Firebase.Database;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreElement : MonoBehaviour
{
   
    FireBabeAuh _fireBabeAuh;
    PerguntaDoTema pergunta;
       
    [Header("Id e nome do tema")]
    public TMP_Text nometemaText;
    public int idtema;//id do tema que esta 

   
    [Header("Nome da cena pras perguntas")]
    public string nomeScene;
    public string tema;
    public int notaf = 0;

    [Header("Barra Xp e  level ")]
    public Image barraImagen;
    public TMP_Text rankTxt;
    private int leveTema = 0;

    [Header("sprits dos temas ")]
    public Sprite[] spritsTemas;
    public GameObject btnTema;
    public Sprite[] spritsImageTema;
    public Image imageTema;

    private void Start()
    {
       
        _fireBabeAuh = FindObjectOfType<FireBabeAuh>() as FireBabeAuh;
        pergunta = FindObjectOfType<PerguntaDoTema>() as PerguntaDoTema;
         
        NewTemaElement(tema);
        StartCoroutine(GetLevel(tema));
        StartCoroutine(GetNotas(tema));
        btnTema.GetComponent<Image>().sprite = spritsTemas[idtema-1];
        imageTema.sprite = spritsImageTema[idtema -1];




    }
   

    public void NewTemaElement(string _nometema)
    {

        tema = _nometema;
        nometemaText.text = tema;

    }
    public void NewNotasEstrelas(string _idtemas)
    {
        idtema = int.Parse(_idtemas);
    }

    public void Buttonclick()
    {
      LoadSceneTema();
    }
    public void LoadSceneTema()
    {
        PlayerPrefs.SetInt("IdTema",idtema);
        PlayerPrefs.SetString("NomeTema",tema);
        PlayerPrefs.SetInt("Nota",notaf);
        UIManager.instance.MenuPerguntas();
        pergunta.DisparaantesPerguntas();
    }
    private IEnumerator GetNotas(string nometema)
    {
       

        var Task = _fireBabeAuh.dbReference.Child("users").Child(_fireBabeAuh.user.UserId).Child("Tema").Child(nometema).Child("Status").Child("Xp").GetValueAsync();
        yield return new WaitUntil(predicate: () => Task.IsCompleted);
      
            if (Task.Exception != null)
            {
             
               SetNotas(tema);
               Debug.Log("SALVO");
               
            }
            else if(Task.Result.Value == null)
            {
                SetNotas(tema);
                Debug.Log("SALVO 1");
            }
            else
            {
                DataSnapshot snapshot = Task.Result;
                string n = snapshot.Value.ToString();
                notaf = int.Parse(n);
                // EstrelasNivel();
                BarraNIvel();
                Debug.Log("carregou");
            }


    }
    private IEnumerator GetLevel(string nometema)
    {


        var Task = _fireBabeAuh.dbReference.Child("users").Child(_fireBabeAuh.user.UserId).Child("Tema").Child(nometema).Child("Status").Child("Level").GetValueAsync();
        yield return new WaitUntil(predicate: () => Task.IsCompleted);

        if (Task.Exception != null)
        {

            SetLevel(0, tema);
            Debug.Log("SALVO");

        }
        else if (Task.Result.Value == null)
        {
            SetLevel(0,tema);
            Debug.Log("SALVO level");
        }
        else
        {
            DataSnapshot snapshot = Task.Result;
            string n = snapshot.Value.ToString();
            leveTema = int.Parse(n);
            // EstrelasNivel();     
            Debug.Log("carregou level");
        }


    }
    private void SetNotas(string nometema)
    {
        
        _fireBabeAuh.dbReference.Child("users").Child(_fireBabeAuh.user.UserId).Child("Tema").Child(nometema).Child("Status").Child("Xp").SetValueAsync(notaf);
      
    }
    private void SetLevel(int level, string nometema)
    {

        _fireBabeAuh.dbReference.Child("users").Child(_fireBabeAuh.user.UserId).Child("Tema").Child(nometema).Child("Status").Child("Level").SetValueAsync(level);
    }
    public void BarraNIvel()
    {
        barraImagen.fillAmount = notaf / 10f;
        rankTxt.text = leveTema.ToString();
    }
}
