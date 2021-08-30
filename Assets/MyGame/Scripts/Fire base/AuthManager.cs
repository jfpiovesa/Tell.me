 using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Linq;
using UnityEngine.UI;

public class AuthManager : MonoBehaviour
{
    FireBabeAuh _fireBabeAuh;
    public string _name;
    //Login variables
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;
    private  string autologin;

    //Register variables
    [Header("Register")]
    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField phoneNumberField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;
    public Text tagsArea;
    public TMP_Text warningRegisterText;

    // User data  variables

    [Header(" UserData ")]
    private TMP_Text nameuserTmeafield;
    public GameObject MenuElement;
    public Transform scoredboardContent;

    //valores salvos no playerprefs
    [Header("salvos no playerprefs")]
    public string userId;

    [Header("menu avatar")]
    public Image barraXP;
    public TMP_Text nome;
    public TMP_Text nomeNaPergunta;
    public Text levelPlayer;
    public string level;
    public Toggle bnt_Toggle;

    [Header("Pefil User")]
    public TMP_Text nomePerfil;
    public TMP_Text numberPhonePerfil;
    public TMP_Text TagPerfil;
    public TMP_Text imputfildTagPerfil;
    public TMP_InputField inputfildNomePerfil;
    public TMP_InputField inputfildNumberPhonePerfil;
   

    private void Start()
    {
        _fireBabeAuh = FindObjectOfType<FireBabeAuh>() as FireBabeAuh;
        PlayerPrefs.SetString("useride", "");
        autologin =  PlayerPrefs.GetString("AutoLogin");
        string login = PlayerPrefs.GetString("Login");
        string password = PlayerPrefs.GetString("Password");
        if (autologin == "true")
        {
            bnt_Toggle.isOn = true;
            StartCoroutine(LoginAuto(login, password));
        }else if(autologin == "false" || autologin == string.Empty)
        {
            bnt_Toggle.isOn = false;
        }
    }
  
    // fuction login auto
    public IEnumerator LoginAuto(string login, string password)
    {
     

        yield return new WaitForSeconds(1);
      
            if (login == string.Empty && password == string.Empty)
            {
              
            }
            else
            {
                emailLoginField.text = login.ToString();
                passwordLoginField.text = password.ToString();
                LoginButton();

            } 
        
    }
    public void UpdatePerfil()
    {
        StartCoroutine(UpdateUserNameAuth(inputfildNomePerfil.text));
        StartCoroutine(NumberPhone(inputfildNumberPhonePerfil.text));
        TagsArea(imputfildTagPerfil.text);
    }
    public void LoadPerfil()
    {
        StartCoroutine(GetNumbePhone());
        StartCoroutine(GetTag());
        StartCoroutine(LoadUserName());
    }
    public void AutoButton()
    {

        if (bnt_Toggle.isOn == true )
        {         
            Debug.Log("true");
            PlayerPrefs.SetString("AutoLogin", "true");                 

        }
        else if( bnt_Toggle.isOn == false || autologin == string.Empty)
        {         
            Debug.Log("false");
            PlayerPrefs.SetString("AutoLogin", "false");         
          
        }
       
    }
    // function clear register
    public void ClearRegisterFields()
    {
        emailRegisterField.text = "";
        usernameRegisterField.text = "";
        passwordRegisterField.text = "";
        phoneNumberField.text = "";
        passwordRegisterVerifyField.text = "";
    }
    public void ClearLoginFields()
    {
        emailLoginField.text = "";
        passwordLoginField.text = "";

    }

    //Function for the login button
    public void LoginButton()
    {

        //Call the login coroutine passing the email and password
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
        



    }
    //Function for the register button
    public void RegisterButton()
    {
        //Call the register coroutine passing the email, password, and username
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text, phoneNumberField.text, tagsArea.text));
    }
    public void SingOutButton()
    {
        _fireBabeAuh.auth.SignOut();
        PlayerPrefs.SetString("AutoLogin", "false");
        bnt_Toggle.isOn = false;
        UIManager.instance.LoginScreen();
        ClearRegisterFields();
        ClearLoginFields();
    }

    //login
    private IEnumerator Login(string _email, string _password)
    {
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = _fireBabeAuh.auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            warningLoginText.text = message;
        }
        else
        {
            //User is now logged in
            //Now get the result
            _fireBabeAuh.user = LoginTask.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", _fireBabeAuh.user.DisplayName, _fireBabeAuh.user.Email);
            warningLoginText.text = "";
            PlayerPrefs.SetString("Login", _email);
            PlayerPrefs.SetString("Password", _password);
            confirmLoginText.text = "Logged In";
            confirmLoginText.text = "";
            Time();
            LoadPerfil();

        }
    }
    private void Time()
    {
        StartCoroutine(GetIdUse());
        Loaded();      
        XpAtualizar();
      
        UIManager.instance.MenuInicial();

    }
    public void XpAtualizar()
    {
        StartCoroutine(GetLevel());
        StartCoroutine(GetXp());
    }
    public void Loaded()
    {
        StartCoroutine(LoadScoreboardDAta());
    }

    // register
    private IEnumerator Register(string _email, string _password, string _username, string _numberPhone,string _tags)
    {
        if (_username == "")
        {
            //If the username field is blank show a warning
            warningRegisterText.text = "Missing Username";
        }
        else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            //If the password does not match show a warning
            warningRegisterText.text = "Password Does Not Match!";
        }
        else
        {
            //Call the Firebase auth signin function passing the email and password
            var RegisterTask = _fireBabeAuh.auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                }
                warningRegisterText.text = message;
            }
            else
            {
                //User has now been created
                //Now get the result
                _fireBabeAuh.user = RegisterTask.Result;

                if (_fireBabeAuh.user != null)
                {
                    //Create a user profile and set the username
                    UserProfile profile = new UserProfile { DisplayName = _username};

                    //Call the Firebase auth update user profile function passing the profile with the username
                    var ProfileTask = _fireBabeAuh.user.UpdateUserProfileAsync(profile);
                    //Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        warningRegisterText.text = "Username Set Failed!";
                    }
                    else
                    {
                        warningRegisterText.text = "";
                        ClearLoginFields();
                        ClearRegisterFields();
                        StartCoroutine(NumberPhone(_numberPhone));
                        StartCoroutine(UpdateDAtaBaseAuth(_username));                    
                        LvXP();
                        TagsArea(_tags);
                        UIManager.instance.LoginScreen();
                    }
                }
            }
        }
    }

    public void TagsArea(string tags)
    {
          _fireBabeAuh.dbReference.Child("users").Child(_fireBabeAuh.user.UserId).Child("User").Child("Tags").SetValueAsync(tags);
    }
    public void LvXP()
    {
        _fireBabeAuh.dbReference.Child("users").Child(_fireBabeAuh.user.UserId).Child("Status").Child("Xp").SetValueAsync(0);
        _fireBabeAuh.dbReference.Child("users").Child(_fireBabeAuh.user.UserId).Child("Status").Child("Level").SetValueAsync(0);

    }
    public IEnumerator NumberPhone(string number)
    {
        var Task = _fireBabeAuh.dbReference.Child("users").Child(_fireBabeAuh.user.UserId).Child("User").Child("NumberPhone").SetValueAsync(number);
        yield return new WaitUntil(predicate: () => Task.IsCompleted);

        if (Task.Exception != null)
        {
            Debug.LogWarning(message: $" faild  to register  task with {Task.Exception}");
        }
        else
        {
            // Auth  username  is now  update
        }
    }
    private IEnumerator UpdateUserNameAuth(string _username)
    {
        // creator  um user profile and set the username
        //UserProfile profile = new UserProfile { DisplayName = _username };

        // call the firebase  auth  update  user profile fuction  passing  the profile  with the username 
       // var ProfileTask = _fireBabeAuh.user.UpdateUserProfileAsync(profile);
       var ProfileTask = _fireBabeAuh.dbReference.Child("users").Child(_fireBabeAuh.user.UserId).Child("User").Child("username").SetValueAsync(_username);
        // wait  until  the task  completes 
        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

        if (ProfileTask.Exception != null)
        {
            Debug.LogWarning(message: $" faild  to register  task with {ProfileTask.Exception}");
        }
        else
        {
            // Auth  username  is now  update
        }

    }
    private IEnumerator UpdateDAtaBaseAuth(string _username)
    {

        var DbTask = _fireBabeAuh.dbReference.Child("users").Child(_fireBabeAuh.user.UserId).Child("User").Child("username").SetValueAsync(_username);

        // wait  until  the task  completes 
        yield return new WaitUntil(predicate: () => DbTask.IsCompleted);

        if (DbTask.Exception != null)
        {
            Debug.LogWarning(message: $" faild  to register  task with {DbTask.Exception}");
        }
        else
        {
            // database  username  is now  update
        }
    }

    private IEnumerator GetIdUse()
    {
        var task = _fireBabeAuh.dbReference.Child("users").GetValueAsync();
        yield return new WaitUntil(predicate: () => task.IsCompleted);
        if(task.Exception != null)
        {
            Debug.LogError(" deu ruim em " + task.Exception);

        }
        else
        {
           
            DataSnapshot dataSnapshot = task.Result;
            string nome = dataSnapshot.Child(_fireBabeAuh.user.UserId).Key.ToString();
            PlayerPrefs.SetString("useride",nome);          
            Debug.Log(" userId" + nome);
        }
    }
    private IEnumerator LoadScoreboardDAta()
    {

        var dbTask = _fireBabeAuh.dbReference.Child("temas").OrderByValue().GetValueAsync();

        yield return new WaitUntil(predicate: () => dbTask.IsCompleted);

        if (dbTask.Exception != null)
        {
            Debug.LogWarning(message: $" faileda to register task with{dbTask.Exception}");
        }
        else
        {
            // data has  been retrieved
            DataSnapshot snapshot = dbTask.Result;

            foreach (Transform child in scoredboardContent.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (DataSnapshot childdataSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                string _nometemas = childdataSnapshot.Child("Tema").Value.ToString();
                string _valor = childdataSnapshot.Key.ToString();
                GameObject scoreboardElement = Instantiate(MenuElement, scoredboardContent);
                scoreboardElement.GetComponent<ScoreElement>().NewTemaElement(_nometemas);
                scoreboardElement.GetComponent<ScoreElement>().NewNotasEstrelas(_valor);


            }
        }


    }
    private IEnumerator LoadUserName()
    {
        var dbTask = _fireBabeAuh.dbReference.Child("users").Child(_fireBabeAuh.user.UserId).Child("User").GetValueAsync();

        yield return new WaitUntil(predicate: () => dbTask.IsCompleted);

        if (dbTask.Exception != null)
        {
            Debug.LogWarning(message: $" failed   to register task with {dbTask.Exception}");
        }
        else if (dbTask.Result.Value == null)
        {
           _name = "";
        }
        else
        {
            DataSnapshot snapshot = dbTask.Result;
            _name = snapshot.Child("username").Value.ToString();
            nome.text = " Bem-vindo, " + _name.ToString() + "!";
            nomeNaPergunta.text = _name;
            nomePerfil.text = _name;
            inputfildNomePerfil.text = _name;

        }
    }
    
    private IEnumerator GetXp()
    {

        var task = _fireBabeAuh.dbReference.Child("users").Child(_fireBabeAuh.user.UserId).Child("Status").Child("Xp").GetValueAsync();
        yield return new WaitUntil(predicate: () => task.IsCompleted);

        if (task.Exception != null)
        {                   
            Debug.LogError(" deu ruim xp");
        }
        else
        {         
            DataSnapshot data = task.Result;
            string x = data.Value.ToString();
           int xp = int.Parse(x);
            float valor = 100;
            barraXP.fillAmount = xp / valor;         

           

        }

    }
    private IEnumerator GetLevel()
    {

        var task = _fireBabeAuh.dbReference.Child("users").Child(_fireBabeAuh.user.UserId).Child("Status").Child("Level").GetValueAsync();
        yield return new WaitUntil(predicate: () => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogError(" deu ruim xp");
        }
        else
        {
            DataSnapshot snapshot = task.Result;

            level = snapshot.Value.ToString();
            levelPlayer.text = level;
        }

    }
    private IEnumerator GetNumbePhone()
    {

        var task = _fireBabeAuh.dbReference.Child("users").Child(_fireBabeAuh.user.UserId).Child("User").Child("NumberPhone").GetValueAsync();
        yield return new WaitUntil(predicate: () => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogError("erro");
        }
        else
        {
            DataSnapshot snapshot = task.Result;

            string x = snapshot.Value.ToString();
            numberPhonePerfil.text = x;
            inputfildNumberPhonePerfil.text = x;
        }

    }
    private IEnumerator GetTag()
    {

        var task = _fireBabeAuh.dbReference.Child("users").Child(_fireBabeAuh.user.UserId).Child("User").Child("Tags").GetValueAsync();
        yield return new WaitUntil(predicate: () => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogError("erro");
        }
        else
        {
            DataSnapshot snapshot = task.Result;

            string x = snapshot.Value.ToString();
            TagPerfil.text = x;
            

        }

    }
}

