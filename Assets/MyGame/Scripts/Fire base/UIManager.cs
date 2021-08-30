using System.Collections;
using UnityEngine;



public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    AudioManager audio;
    //Screen object variables
    [Header("Valores de ativar objetos")]
    public GameObject loginUI;
    public GameObject registerUI;
    public GameObject menuTemasUI;
    public GameObject menuInicialUI;
    public GameObject menuDasPerguntasUI;
    public GameObject menuDeNotasUI; 
    public GameObject menuConfig;
    public GameObject painelstatus;
    bool ativo = true;
    [Header("fad")]
    public GameObject fad;
    [Range(0, 10)]
    public float time = 1.38f;
  
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }
 
    //Functions to change the login screen UI
    public void LoginScreen() //Back button
    {
        StartCoroutine(Login());

    }
    public void MenuConfig(bool ad)
    {
        menuConfig.SetActive(ad);
    }
    public void painelStatus()
    {
       
        if(ativo == true)
        {
            painelstatus.SetActive(true);
            ativo = false;
            

        }else
        {
            painelstatus.SetActive(false);
            ativo = true;
        }
      
    }

    public void RegisterScreen() // Regester button
    {   
         StartCoroutine(Resgister());

    }
   
    public void MenuTemas()
    {

        StartCoroutine(Temas());

    }
   
    public void MenuInicial()
    {
        StartCoroutine(Inicial());
    }
 
    public void MenuPerguntas()
    {
        StartCoroutine(Perguntas());
    }
  
    public void MenuNota()
    {

        StartCoroutine(Nota());
     
    }
    private IEnumerator Inicial()
    {
        fad.GetComponent<Animator>().SetTrigger("Play");
        yield return new WaitForSeconds(time);
        menuInicialUI.SetActive(true);
        menuTemasUI.SetActive(false);
        loginUI.SetActive(false);
        registerUI.SetActive(false);
        menuDasPerguntasUI.SetActive(false);
        menuDeNotasUI.SetActive(false);
    }
    private IEnumerator Temas()
    {
        fad.GetComponent<Animator>().SetTrigger("Play");
        yield return new WaitForSeconds(time);
        menuTemasUI.SetActive(true);
        loginUI.SetActive(false);
        registerUI.SetActive(false);
        menuDasPerguntasUI.SetActive(false);
        menuDeNotasUI.SetActive(false);
        menuInicialUI.SetActive(false);
    }
    private IEnumerator Perguntas()
    {
        AudioManager.managerAudio.PlayAudioBlackground(1);
        fad.GetComponent<Animator>().SetTrigger("Play");
        yield return new WaitForSeconds(2);       
        menuDasPerguntasUI.SetActive(true);
        menuDeNotasUI.SetActive(false);
        menuTemasUI.SetActive(false);
        loginUI.SetActive(false);
        registerUI.SetActive(false);
        menuInicialUI.SetActive(false);
    }
    private IEnumerator Nota()
    {
        fad.GetComponent<Animator>().SetTrigger("Play");
        yield return new WaitForSeconds(time);
        menuDasPerguntasUI.SetActive(false);
        menuDeNotasUI.SetActive(true);
        menuTemasUI.SetActive(false);
        loginUI.SetActive(false);
        registerUI.SetActive(false);
        menuInicialUI.SetActive(false);
    }
    private IEnumerator Resgister()
    {
        fad.GetComponent<Animator>().SetTrigger("Play");
        yield return new WaitForSeconds(time);
        loginUI.SetActive(false);
        registerUI.SetActive(true);
        menuTemasUI.SetActive(false);
        menuDasPerguntasUI.SetActive(false);
        menuDeNotasUI.SetActive(false);
        menuInicialUI.SetActive(false);

    }
    private IEnumerator Login()
    {
        fad.GetComponent<Animator>().SetTrigger("Play");
        yield return new WaitForSeconds(time);
        loginUI.SetActive(true);
        registerUI.SetActive(false);
        menuTemasUI.SetActive(false);
        menuDasPerguntasUI.SetActive(false);
        menuDeNotasUI.SetActive(false);
        menuInicialUI.SetActive(false);
    }


}
