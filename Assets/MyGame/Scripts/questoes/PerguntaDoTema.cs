using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Firestore;
using System.Linq;

public class PerguntaDoTema : MonoBehaviour
{



    FireBabeAuh _fireBabeAuh;
    public bool disparaperguntas = false;

    [Header("Elementos visuais das perguntas")]
    public TMP_Text TemaPergunta;
    public TMP_Text TemaRespostaergunta;
    public TMP_Text a;
    public TMP_Text b;
    public TMP_Text c;
    public TMP_Text d;
    public TMP_Text tema;

    [Header("valor tema & pergunta atual")]
    public int valorperguntas = 0;
    [SerializeField]
    private string valorHash;
    [SerializeField]
    int idValor;
    public Image infoResposta;
    public GameObject btn_sair, btn_inicializar, painel;



    [Header("Conometro")]
    public Text timerText;
    public float startTime = 10;
    public Image ObjectConometro;


    [Header("Resposta & valores da perguntas")]
    public string respostaTemporaria;
    public bool m_pl = false;
    public float acertos = 0;
    public float valorBarraTamonho;
    private float media = 0;
    private int notafinal = 0;
    string l = "";
    public string _tema;



    [Header("Valores de tentativas e notas toatais ")]
    private int totalDeNOtas = 0;
    private int NotaAtual = 0;
    private int totalDetentativas = 0;
    private int tentativaAtual = 0;
    private int levelAt = 0;
    private int xpAt = 0;

    [Header("Perguntas")]
    public List<string> Hash = new List<string>();
    public string[] h;

    [Header("Valores Xp e level do tema")]
    private int levelTema = 0;
    private int xpTema = 0;


    [Header("Valores pergunta bonus")]
    public string valorA,valorB,valorC,valorD;
    public List<string> bonusListaIds;
    public string[] bonusArrayIds;
    int valorProximaPerguntasBonus = 0;
    int aleatorio;
    private void Start()
    {
       
        _fireBabeAuh = FindObjectOfType<FireBabeAuh>() as FireBabeAuh;
        TxtClear();
        btn_inicializar.SetActive(false);
        btn_sair.SetActive(false);
        painel.SetActive(false);
    }
    private void Update()
    {
        if (disparaperguntas ==  true)
        { 
            ObjectConometro.fillAmount = startTime / 10;
            startTime -= Time.deltaTime;
            string seconds = (startTime % 60).ToString("f0");
            timerText.text = seconds;
            infoResposta.fillAmount = valorperguntas / valorBarraTamonho;
            
        }
        
        if (startTime <= 0)
        {

            resposta(l);
            startTime = 10;
            proximapergunta();

        }
    
    }
    void TxtClear()
    {
        TemaPergunta.text = " ---------------- ";
        a.text = " ---------------- ";
        b.text = " ---------------- ";
        c.text = " ---------------- ";
        d.text = " ---------------- ";
        respostaTemporaria = "";
        m_pl = false;
       
    }
    public void DisparaPerguntas()
    {
       if(h == null)
        {

        }
        else
        {
            StartCoroutine(Dispara());
        }
        
        
    }
    public void DisparaantesPerguntas()
    {
        idValor = PlayerPrefs.GetInt("IdTema");
        TemasNomes();
        StartCoroutine(GetXp());
        StartCoroutine(BonusGetValue());
        StartCoroutine(GetLevel());
        StartCoroutine(HasGetValue());
        StartCoroutine(GetValuesTentativas());
        StartCoroutine(GetValuesNotas());
        btn_inicializar.SetActive(true);
        btn_sair.SetActive(true);
      
    }
    public IEnumerator Dispara()
    {
        btn_inicializar.SetActive(false);
        btn_sair.SetActive(false);
        painel.SetActive(true);
        yield return new  WaitForSeconds(0.5f);    
        StartCoroutine(LoadPerguntas());
        StartCoroutine(GetXpTema());
        StartCoroutine(GetLevelTema());
        aleatorio = Random.Range(1,h.Length);
        disparaperguntas = true;

    }

    void PerguntasEBonus()
    {
       
    }

    private IEnumerator LoadPerguntasSatisfacao()
    {

        string n = bonusArrayIds[valorProximaPerguntasBonus];


        var dbTask = _fireBabeAuh.dbReference.Child("Bonus").Child("Perguntas").Child(n).GetValueAsync();

        yield return new WaitUntil(predicate: () => dbTask.IsCompleted);

        if (dbTask.Exception != null)
        {
            Debug.LogWarning(message: $" failed   to register task with {dbTask.Exception}");
        }

        else
        {
            DataSnapshot snapshot = dbTask.Result;
            string perguntas = snapshot.Child("Pergunta").Value.ToString();         
            string _a = snapshot.Child("1").Child("Pergunta").Value.ToString();
            string _b = snapshot.Child("2").Child("Pergunta").Value.ToString();        
            string _c = snapshot.Child("3").Child("Pergunta").Value.ToString();
            string _d = snapshot.Child("4").Child("Pergunta").Value.ToString();      
            TemaPergunta.text = perguntas;
            respostaTemporaria = "BonusPerguntas";
            a.text = _a;
            b.text = _b;
            c.text = _c;
            d.text = _d;

            string aValue = snapshot.Child("1").Child("valor").Value.ToString();
            string bValue = snapshot.Child("2").Child("valor").Value.ToString();
            string cValue = snapshot.Child("3").Child("valor").Value.ToString();
            string dValue = snapshot.Child("4").Child("valor").Value.ToString();

            valorA = aValue.ToString();
            valorB = bValue.ToString();
            valorC = cValue.ToString();
            valorD = dValue.ToString();

         
        }


    }

    private IEnumerator LoadPerguntas()
    {

        string n = h[valorperguntas];
    
        
        var dbTask = _fireBabeAuh.dbReference.Child("temas").Child(idValor.ToString()).Child("Perguntas").Child(n).GetValueAsync();

        yield return new WaitUntil(predicate: () => dbTask.IsCompleted);

        if (dbTask.Exception != null)
        {
            Debug.LogWarning(message: $" failed   to register task with {dbTask.Exception}");
        }

        else
        {
            DataSnapshot snapshot = dbTask.Result;

            string perguntas = snapshot.Child("Pergunta").Value.ToString();
            string resposta = snapshot.Child("Resposta").Value.ToString();
            string a = snapshot.Child("A").Value.ToString();
            string b = snapshot.Child("B").Value.ToString();
            string c = snapshot.Child("C").Value.ToString();
            string d = snapshot.Child("D").Value.ToString();          
            Perguntas(perguntas,resposta, a, b, c, d);



        }
    

    }
   
    public void Perguntas(string perguntas, string resposta, string A, string B, string C, string D)
    {
        TemaPergunta.text = perguntas;
        respostaTemporaria = resposta;
        a.text = A;
        b.text = B;
        c.text = C;
        d.text = D;
    }
    public void TemasNomes()
    {
        _tema = PlayerPrefs.GetString("NomeTema");
        tema.text  = _tema;
    }

    public void LetraScolha(string letra)
    {
       
        l = letra;
        m_pl = true;

    }

    public void resposta(string alternativa)// verificação de resposta se elas são verdadeiras e se são iguas as corretas se sim soma um acerto se não vai ate final e  pula pra proxima pergunta 
    {
             m_pl = true;

        if (alternativa == "A" && m_pl == true)
        {
            if (respostaTemporaria == "BonusPerguntas")
            {

                Setbonus(valorA);
                acertos += 1;
            }
            else
            {
                if (a.text == respostaTemporaria)
                {
                    acertos += 1;
                }
            }
        }
        else if (alternativa == "B" && m_pl == true)
        {
            if (respostaTemporaria == "BonusPerguntas")
            {

                Setbonus(valorB);
                acertos += 1;
            }
            else
            {
                if (b.text == respostaTemporaria)
                {
                    acertos += 1;
                }
            }
        }
        else if (alternativa == "C" && m_pl == true)
        {
            if (respostaTemporaria == "BonusPerguntas")
            {

                Setbonus(valorC);
                acertos += 1;
            }
            else
            {
                if (c.text == respostaTemporaria)
                {
                    acertos += 1;
                }
            }
        }

        else if (alternativa == "D" && m_pl == true)
        {
            if (respostaTemporaria == "BonusPerguntas")
            {

                Setbonus(valorD);
                acertos += 1;
            }
            else
            {
                if (d.text == respostaTemporaria)
                {
                    acertos += 1;
                }
            }
        }         
        proximapergunta();
    }

    public void proximapergunta()
    {

        //proximas pertuntas  e tempo do conometro entra no valor inicial de novo 
        
       
        valorperguntas ++ ;
        startTime = 10;
      
        if (valorperguntas < h.Length )
        {
           if(valorperguntas == aleatorio)
            {
                StartCoroutine(LoadPerguntasSatisfacao());

                m_pl = false;
            }
           else
            {
                StartCoroutine(LoadPerguntas());
                m_pl = false;
            }
                      
        
        }
        else
        {   // oque fazer quando termina as perguntas.
            media = 10 * (acertos / h.Length);// calcula a media  na porcetagem dos acertos.

            notafinal = Mathf.RoundToInt(media);// arredonda a nota para o proximo inteiro,segindo a regra da matetmarica.

            
            int xpTm = xpTema + notafinal;        
            if (xpTm >= 10)
            {
               
                int lv = (levelTema += 1);
                if (lv >= levelTema)
                {
                    SetLevelTema(lv);
                }
                int TemaXP = xpTm - 10;
                SetXpTema(TemaXP);
                PlayerPrefs.SetInt("Nota", (int)notafinal);
                PlayerPrefs.SetInt("Acertos", (int)acertos);
            }
            else
            {
                SetXpTema(xpTm);
            }


            PlayerPrefs.SetInt("Notatemp", notafinal);
            PlayerPrefs.SetInt("Acertostemp", (int)acertos);
            PlayerPrefs.SetInt("Questoestemp", (int)valorBarraTamonho);
            
            SomaTentativaNotas(notafinal);
            StartCoroutine(LoadCena());
           
            PlayerPrefs.SetInt("IdTema", 0);
            Hash.Clear();
            
        }
        
    }
    public IEnumerator BonusGetValue()
    {

        var dbTask = _fireBabeAuh.dbReference.Child("Bonus").Child("Perguntas").OrderByValue().GetValueAsync();
        yield return new WaitUntil(predicate: () => dbTask.IsCompleted);

        if (dbTask.Exception != null)
        {
            Debug.LogError(" deu ruim has");
        }
        else
        {
            DataSnapshot snapshot = dbTask.Result;
            foreach (DataSnapshot data in snapshot.Children)
            {
                string a = data.Key.ToString();
                bonusListaIds.Add(a);


            }

            bonusArrayIds = bonusListaIds.ToArray();         

        }


    }
    public IEnumerator HasGetValue()
    {

        var dbTask = _fireBabeAuh.dbReference.Child("temas").Child(idValor.ToString()).Child("Perguntas").OrderByValue().GetValueAsync();
        yield return new WaitUntil(predicate: () => dbTask.IsCompleted);

        if (dbTask.Exception != null)
        {
            Debug.LogError(" deu ruim has");
        }
        else
        { // data has  been retrieved
            DataSnapshot snapshot = dbTask.Result;
            foreach (DataSnapshot data in snapshot.Children)
            {
                string a = data.Key.ToString();
                Hash.Add(a);

               
            }
            for (int i = 0; i < h.Length; i++)
            {
                int _a = Random.Range(0, Hash.ToArray().Length);
                h[i] = Hash.ElementAt<string>(_a);
                if (h.Length > 9)
                {
                    break;
                }
            }
            valorBarraTamonho = h.Length;
           

        }
       

    }
    public IEnumerator LoadCena()
    {
           
        UIManager.instance.MenuNota();
        NotaDoTema.instace.NotaResultado();
        yield return new WaitForSeconds(1);
        ClearCenaPeguntas();
    }
    public void ClearCenaPeguntas()
    {
        StartCoroutine(ClearCenaPergunntasTime());

    }
    public IEnumerator ClearCenaPergunntasTime()
    {
        yield return new WaitForSeconds(1);
        disparaperguntas = false;
        valorperguntas = 0;
        acertos = 0;
        notafinal = 0;
        tentativaAtual = 0;
        xpAt = 0;
        xpTema = 0;
        levelTema = 0;
        levelAt = 0;
        TxtClear();
        Hash.Clear();
        bonusListaIds.Clear();
        btn_inicializar.SetActive(true);
        btn_sair.SetActive(true);
        painel.SetActive(false);
    }
    private void Setbonus(string valor)
    {

        _fireBabeAuh.dbReference.Child("users").Child(_fireBabeAuh.user.UserId).Child("Status").Child("Satisfacao").SetValueAsync(valor);
    }

    private void SetLevelTema(int level)
    {

        _fireBabeAuh.dbReference.Child("users").Child(_fireBabeAuh.user.UserId).Child("Tema").Child(_tema).Child("Status").Child("Level").SetValueAsync(level);
    }
    private void SetXpTema(int xp)
    {
        _fireBabeAuh.dbReference.Child("users").Child(_fireBabeAuh.user.UserId).Child("Tema").Child(_tema).Child("Status").Child("Xp").SetValueAsync(xp);
    }
    private IEnumerator GetXpTema()
    {
        var task = _fireBabeAuh.dbReference.Child("users").Child(_fireBabeAuh.user.UserId).Child("Tema").Child(_tema).Child("Status").Child("Xp").GetValueAsync();
        yield return new WaitUntil(predicate: () => task.IsCompleted);

        if (task.Exception != null)
        {
            
            Debug.Log(" salva notas e tentativas");
        }
        else
        {
            if (task.Result.Value == null )
            {
                    
                SetXpTema(xpTema);
            }
            DataSnapshot snapshot = task.Result;
            xpTema = int.Parse(snapshot.Value.ToString());
          
        }
    }
    private IEnumerator GetLevelTema()
    {
        var task = _fireBabeAuh.dbReference.Child("users").Child(_fireBabeAuh.user.UserId).Child("Tema").Child(_tema).Child("Status").Child("Level").GetValueAsync();
        yield return new WaitUntil(predicate: () => task.IsCompleted);

        if (task.Exception != null)
        {
           
            SetLevelTema(levelTema);
            Debug.Log(" salva ");
        }
        else
        {
            if (task.Result.Value == null)
            {             
                SetLevelTema(levelTema);
            }
            DataSnapshot snapshot = task.Result;
            levelTema = int.Parse(snapshot.Value.ToString());
         
        }
    }

    private IEnumerator SetTentativas(int tentativa)
    {
      
        var task = _fireBabeAuh.dbReference.Child("users").Child(_fireBabeAuh.user.UserId).Child("Tema").Child(_tema).Child("Tentativas").SetValueAsync(tentativa);
        yield return new WaitUntil(predicate: () => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogError(" deu ruim");
        }
        else
        {

        }

    }
    private IEnumerator SetNotasTotais(int nota)
    {
       
        var task = _fireBabeAuh.dbReference.Child("users").Child(_fireBabeAuh.user.UserId).Child("Tema").Child(_tema).Child("TotalDasNota").SetValueAsync(nota);
        yield return new WaitUntil(predicate: () => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogError(" deu ruim");
        }
        else
        {

        }

    }
    private IEnumerator GetValuesTentativas()
    {
    
        var task = _fireBabeAuh.dbReference.Child("users").Child(_fireBabeAuh.user.UserId).Child("Tema").Child(_tema).Child("Tentativas").GetValueAsync();
        yield return new WaitUntil(predicate: () => task.IsCompleted);

        if (task.Exception != null)
        {


            StartCoroutine(SetTentativas(totalDetentativas));

            Debug.Log(" salva notas e tentativas");
        }
        else if (task.Result.Value == null)
        {
            StartCoroutine(SetTentativas(totalDetentativas));
        }
        else
        {
            
            DataSnapshot snapshot = task.Result;
            string x = snapshot.Value.ToString();
            totalDetentativas = int.Parse(x);

        }

    }
    private IEnumerator GetValuesNotas()
    {
        var task = _fireBabeAuh.dbReference.Child("users").Child(_fireBabeAuh.user.UserId).Child("Tema").Child(_tema).Child("TotalDasNota").GetValueAsync();
        yield return new WaitUntil(predicate: () => task.IsCompleted);

        if (task.Exception != null)
        {

            StartCoroutine(SetNotasTotais(totalDeNOtas));
          

            Debug.Log(" salva notas e tentativas");
        }
        else if(task.Result.Value == null)
        {
                StartCoroutine(SetNotasTotais(totalDeNOtas));
          
        }
        else
        {
           
            DataSnapshot snapshot = task.Result;
            string x = snapshot.Value.ToString();
            totalDeNOtas = int.Parse(x);
        }
    }
    private IEnumerator SetDataNota(int nota ,string data)
    {
     
        var task = _fireBabeAuh.dbReference.Child("users").Child(_fireBabeAuh.user.UserId).Child("Tema").Child(_tema).Child("DataNota").Child(data).SetValueAsync(nota);
        yield return new WaitUntil(predicate: () => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogError(" deu ruim");
        }
        else
        {

        }

    }
    private IEnumerator SetXp(int xptotal)
    {

        var task = _fireBabeAuh.dbReference.Child("users").Child(_fireBabeAuh.user.UserId).Child("Status").Child("Xp").SetValueAsync(xptotal);
        yield return new WaitUntil(predicate: () => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogError(" deu ruim");
        }
        else
        {

        }

    }
    private IEnumerator GetXp()
    {

        var task = _fireBabeAuh.dbReference.Child("users").Child(_fireBabeAuh.user.UserId).Child("Status").Child("Xp").GetValueAsync();
        yield return new WaitUntil(predicate: () => task.IsCompleted);

        if (task.Exception != null)
        {
          

            StartCoroutine(SetXp(xpAt));
            Debug.LogError(" deu ruim xp");
          

          
        }
        else
        {
            if (task.Result.Value == null)
            {
                

                StartCoroutine(SetXp(xpAt));
                Debug.LogError(" deu ruim xp");



            }
            DataSnapshot data = task.Result;
            string x = data.Value.ToString();
            xpAt = int.Parse(x);
        }

    }
    private IEnumerator SetLevel(int Leveltotal)
    {

        var task = _fireBabeAuh.dbReference.Child("users").Child(_fireBabeAuh.user.UserId).Child("Status").Child("Level").SetValueAsync(Leveltotal);
        yield return new WaitUntil(predicate: () => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogError(" deu ruim");
        }
        else
        {

        }

    }
    private IEnumerator GetLevel()
    {

        var task = _fireBabeAuh.dbReference.Child("users").Child(_fireBabeAuh.user.UserId).Child("Status").GetValueAsync();
        yield return new WaitUntil(predicate: () => task.IsCompleted);

        if (task.Exception != null)
        {
           
            StartCoroutine(SetLevel(levelAt));
            Debug.LogError(" deu ruim");


        }
        else if (task.Result.Child("Level").Value == null)
        {
           
            StartCoroutine(SetLevel(levelAt));
        }
        else
        {
            DataSnapshot data = task.Result;

            string l = data.Child("Level").Value.ToString();
            levelAt = int.Parse(l);

        }

    }
    public void SomaTentativaNotas( int notas)
    {
       

        NotaAtual = totalDeNOtas + notas;
        tentativaAtual = totalDetentativas + 1;
       
        string _DataTime = Timestamp.GetCurrentTimestamp().ToString().TrimStart().TrimEnd().Substring(11,16);
        Debug.LogWarning(" Data  =  " + _DataTime);
        StartCoroutine(SetDataNota(notas,_DataTime));


        StartCoroutine(SetNotasTotais(NotaAtual));
        StartCoroutine(SetTentativas(tentativaAtual));

        int limitXp = 100;
        int xp = xpAt + notas;
        if (xp >= limitXp)
        {          
            int level = (levelAt += 1);
            if (level >= levelAt)
            {
                StartCoroutine(SetLevel(level));
            }
            int ZERO = xp - limitXp;
            StartCoroutine(SetXp(ZERO));
           
        }
        else
        {

            StartCoroutine(SetXp(xp));
            
        }
    }
}
