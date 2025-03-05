using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private GameObject titleLogo;  // Ÿ��Ʋ �ΰ� ������Ʈ

    [SerializeField] 
    private GameObject languageSelection;  // ��� ���� UI ������Ʈ

    [SerializeField]
    public Button englishButton;          // ���� ��ư
    [SerializeField]
    public Button japaneseButton;
    [SerializeField]
    public Button koreanButton;// �Ϻ��� ��ư

    [SerializeField] 
    public GameObject warningMessage;  // ��� ������Ʈ

    [SerializeField]
    Image[] logoImage;

    [SerializeField]
    string[] warningText;

    [SerializeField]
    TMP_Text warningTextUI;

    private CanvasGroup warningMessageCanvasGroup;  // CanvasGroup ���� ����

    [SerializeField] 
    private float titleDisplayDuration;  // Ÿ��Ʋ �ΰ� ǥ�� �ð�

    [SerializeField] 
    private float warningDisplayDuration;  // ��� ǥ�� �ð�

    [SerializeField]
    private float fadeDuration; // ���̵� ȿ�� ���� �ð�

    private int selectedLanguageIndex = 0;  // ���õ� ����� �ε��� (0: ����, 1: �Ϻ���)

    void Start()
    {
        // ��� ������Ʈ���� CanvasGroup ������Ʈ�� ������
        warningMessageCanvasGroup = warningMessage.GetComponent<CanvasGroup>();

        if (warningMessageCanvasGroup == null)
        {
            Debug.LogError("WarningMessage�� CanvasGroup�� �����ϴ�!");
            return;
        }

        // �ʱ� ���� ����: Ÿ��Ʋ �ΰ� ���̰� �ϰ� �������� ��Ȱ��ȭ
        titleLogo.SetActive(true);
        languageSelection.SetActive(false);
        // warningMessage.SetActive(false);
        warningMessageCanvasGroup.alpha = 0; // ����� ó���� ������ �ʵ��� ����

        // 2~3�� �� Ÿ��Ʋ �ΰ� ����� ��� ���� ȭ���� ǥ��
        Invoke("ShowLanguageSelection", titleDisplayDuration);

        // �� ��ư�� ���� Ŭ�� �̺�Ʈ ����
        englishButton.onClick.AddListener(() => SelectLanguage(LangType.Type.English)); // 0: ����
        japaneseButton.onClick.AddListener(() => SelectLanguage(LangType.Type.Japanese)); // 1: �Ϻ���
        koreanButton.onClick.AddListener(() => SelectLanguage(LangType.Type.Korean)); // 1: �Ϻ���
    }

    void ShowLanguageSelection()
    {
        logoImage[0].DOFade(0, 2.0f).OnComplete(() =>
        {
            titleLogo.SetActive(false);       // Ÿ��Ʋ �ΰ� ����
            languageSelection.SetActive(true); // ��� ���� ȭ�� ǥ��
            logoImage[1].DOFade(100, 2f);
            logoImage[2].DOFade(100, 2f).OnComplete(() =>
            {

            });
        });        
        
    }

    void SelectLanguage(LangType.Type argType)
    {
        LanguageManager.Instance.m_langType = argType;

        if (LanguageManager.Instance.m_langType == LangType.Type.English)
        {
            warningTextUI.text = warningTextUI.text + warningText[0];
        }
        else if (LanguageManager.Instance.m_langType == LangType.Type.Japanese)
        {
            warningTextUI.text = warningTextUI.text + warningText[1];
        }
        else if (LanguageManager.Instance.m_langType == LangType.Type.Korean)
        {
            warningTextUI.text = warningTextUI.text + warningText[2];
        }

        //selectedLanguageIndex = languageIndex;

        // ���ö����� ������ �ش� ���� ����
        //LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[selectedLanguageIndex];

        // ��� ���� ȭ���� ����� ��� ǥ��
        languageSelection.SetActive(false);
        //ShowWarningMessage();
        StartCoroutine(FadeInWarningMessage());
    }

    void ShowWarningMessage()
    {
        // ��� Ȱ��ȭ
        warningMessage.SetActive(true);
        
        // 3~5�� �� ���� ������ ��ȯ
        Invoke("LoadGameScene", warningDisplayDuration);
    }

    public void LoadGameScene()
    {
        // ���� ���� ������ ��ȯ
        SceneManager.LoadScene("Main");
    }
    IEnumerator FadeInWarningMessage()
    {
        // ��� ���̵� ��
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            warningMessageCanvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        // ����� ǥ�õ� �� ���� �ð� �Ŀ� ���̵� �ƿ�
        yield return new WaitForSeconds(warningDisplayDuration);
        StartCoroutine(FadeOutWarningMessage());
    }

    IEnumerator FadeOutWarningMessage()
    {
        // ��� ���̵� �ƿ�
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            warningMessageCanvasGroup.alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            yield return null;
        }

        // ����� ������� ���� �� �ε�
        LoadGameScene();
    }
}
