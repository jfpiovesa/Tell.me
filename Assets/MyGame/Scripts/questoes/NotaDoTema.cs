using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotaDoTema : MonoBehaviour
{
    public static NotaDoTema instace;
    AuthManager manager;
  

    
    public TMP_Text txtinfoTema;
    public TMP_Text txtnome;
    public int notaf;
    public  string _tema;

    private void Awake()
    {
        if (instace == null)
        {
            instace = this;
        }
        else if (instace != null)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }
    private void Start()
    {
        manager = FindObjectOfType<AuthManager>() as AuthManager;
    }
    public void NotaResultado()
    {
     

        _tema = PlayerPrefs.GetString("NomeTema");
        // pega valor da  salvos no metodo playerprefs mas  temporaria para exibir na nota final 
        notaf = PlayerPrefs.GetInt("Notatemp");
        txtnome.text = (" Parabéns," + manager._name + "!") ;
        txtinfoTema.text = " você ganhou " + notaf.ToString() + " pontos de experiência! ";

  
    }

    
}
