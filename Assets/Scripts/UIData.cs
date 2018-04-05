using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIData : MonoBehaviour
{
    #region Attributes
    private static UIData instance;

    public Text currency;
    public Text lives;
    public Text actualWave;
    public Text waveCountdown;
    public Text messagePrefab;
    public Canvas gameLogger;

    private List<Text> messages;
    private Text messageHolder;
    private Color originalCurrencyColor;
    private Color originalLoggerColor;
    //private Color originalLivesColor;
    //private Color originalActualWaveColor;
    //private Color originalWaveCountdownColor;

    private bool shouldFade;
    private float fadeTime = 2f;
    private float timer = 0f;
    private Color fadeColor;
    #endregion

    #region Methods
    private UIData() { }

    #region UnityMethods
    private void Start()
    {
        originalCurrencyColor = currency.color;
        originalLoggerColor = messagePrefab
            .GetComponentInChildren<Text>().color;

        messages = new List<Text>();
        //originalLivesColor = lives.color;
        //originalActualWaveColor = actualWave.color;
        //originalWaveCountdownColor = waveCountdown.color;
    }

    private void Awake()
    {
        if (instance != null) {
            Debug.LogError(
                "More than one " +
                "UIData instance");
        } else {
            instance = this;
        }
    }

    private void Update()
    {
        if (shouldFade) {
            FadeLoggerMessage();
        }
    }
    #endregion

    #region UserDefinedMethods
    private void PrepareFading(Text holder)
    {
        fadeColor = new Color(
            holder.color.r,
            holder.color.g,
            holder.color.b,
            holder.color.a);

        fadeTime = 2f;
        timer = 0f;

        messageHolder = holder;
        messages.Add(messageHolder);
        shouldFade = true;
    }

    private void FadeLoggerMessage()
    {
        if (timer >= fadeTime)
        {
            float alpha = messageHolder.color.a;
            alpha -= Time.deltaTime / 5;
            //Debug.Log("Alpha: " + alpha);
            fadeColor.a = alpha;
            messageHolder.color = fadeColor;

        } else {
            timer += Time.deltaTime;
        }

        if (messageHolder.color.a <= 0)
        {
            messageHolder.text = "";
            messageHolder.color = 
                originalLoggerColor;

            messages.Remove(messageHolder);
            Destroy(messagePrefab.gameObject);

            if (AreAllMessagesEmpty())
                shouldFade = false;
        }
    }

    public IEnumerator LogMessage(string message)
    {
        //messages = gameLogger.GetComponentsInChildren<Text>();

        //Text holder = messages[0];
        //if (messages[0].text.Equals("")) {
        //    holder = messages[0];
        //} else if (messages[1].text.Equals("")) {
        //    holder = messages[1];
        //} else if (messages[2].text.Equals("")) {
        //    holder = messages[2];
        //}

        Text holder = Instantiate(messagePrefab,
            gameLogger.transform.position,
            gameLogger.transform.rotation);

        holder.text = message;
        PrepareFading(holder);

        yield return new WaitForSeconds(0.3f);
    }

    public bool AreAllMessagesEmpty()
    {
        bool result = true;
        foreach (Text message in messages)
            if (!message.text.Equals(""))
                result = false;

        return result;
    }

    public static UIData GetInstance()
    {
        return instance;
    }
    #endregion
    #endregion

    #region Properties
    public int Currency
    {
        get {
            return System.Convert.ToInt32(
                currency.text);
        }

        set {
            currency.text = "$" + value;
            if (value.Equals(PlayerStats.MaxMoney)) {
                currency.color = Color.red;
            } else {
                currency.color = originalCurrencyColor;
            }
        }
    }

    public int Lives
    {
        get {
            return System.Convert.ToInt32(
                actualWave.text);
        }

        set { lives.text = "Lives: " + value; }
    }

    public int ActualWave
    {
        get {
            return System.Convert.ToInt32(
                actualWave.text);
        }

        set { actualWave.text = "Wave: " + value; }
    }

    public float WaveCountdown
    {
        get {
            return System.Convert.ToSingle(
                waveCountdown.text);
        }

        set { 
            waveCountdown.text = "Next Wave: " +
                string.Format("{0:00.0}", value);
        }
    }

    public string GameLoggerText
    {
        //get { return gameLogger.text; }
        set { StartCoroutine(LogMessage(value)); }
    }
    #endregion
}