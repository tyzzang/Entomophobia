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
    private GameObject titleLogo;  // 타이틀 로고 오브젝트

    [SerializeField] 
    private GameObject languageSelection;  // 언어 선택 UI 오브젝트

    [SerializeField]
    public Button englishButton;          // 영어 버튼
    [SerializeField]
    public Button japaneseButton;
    [SerializeField]
    public Button koreanButton;// 일본어 버튼

    [SerializeField] 
    public GameObject warningMessage;  // 경고문 오브젝트

    [SerializeField]
    Image[] logoImage;

    [SerializeField]
    string[] warningText;

    [SerializeField]
    TMP_Text warningTextUI;

    private CanvasGroup warningMessageCanvasGroup;  // CanvasGroup 참조 변수

    [SerializeField] 
    private float titleDisplayDuration;  // 타이틀 로고 표시 시간

    [SerializeField] 
    private float warningDisplayDuration;  // 경고문 표시 시간

    [SerializeField]
    private float fadeDuration; // 페이드 효과 지속 시간

    private int selectedLanguageIndex = 0;  // 선택된 언어의 인덱스 (0: 영어, 1: 일본어)

    void Start()
    {
        // 경고문 오브젝트에서 CanvasGroup 컴포넌트를 가져옴
        warningMessageCanvasGroup = warningMessage.GetComponent<CanvasGroup>();

        if (warningMessageCanvasGroup == null)
        {
            Debug.LogError("WarningMessage에 CanvasGroup이 없습니다!");
            return;
        }

        // 초기 상태 설정: 타이틀 로고만 보이게 하고 나머지는 비활성화
        titleLogo.SetActive(true);
        languageSelection.SetActive(false);
        // warningMessage.SetActive(false);
        warningMessageCanvasGroup.alpha = 0; // 경고문은 처음에 보이지 않도록 설정

        // 2~3초 후 타이틀 로고를 숨기고 언어 선택 화면을 표시
        Invoke("ShowLanguageSelection", titleDisplayDuration);

        // 각 버튼에 대한 클릭 이벤트 설정
        englishButton.onClick.AddListener(() => SelectLanguage(LangType.Type.English)); // 0: 영어
        japaneseButton.onClick.AddListener(() => SelectLanguage(LangType.Type.Japanese)); // 1: 일본어
        koreanButton.onClick.AddListener(() => SelectLanguage(LangType.Type.Korean)); // 1: 일본어
    }

    void ShowLanguageSelection()
    {
        logoImage[0].DOFade(0, 2.0f).OnComplete(() =>
        {
            titleLogo.SetActive(false);       // 타이틀 로고 숨김
            languageSelection.SetActive(true); // 언어 선택 화면 표시
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

        // 로컬라이즈 설정을 해당 언어로 변경
        //LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[selectedLanguageIndex];

        // 언어 선택 화면을 숨기고 경고문 표시
        languageSelection.SetActive(false);
        //ShowWarningMessage();
        StartCoroutine(FadeInWarningMessage());
    }

    void ShowWarningMessage()
    {
        // 경고문 활성화
        warningMessage.SetActive(true);
        
        // 3~5초 후 게임 씬으로 전환
        Invoke("LoadGameScene", warningDisplayDuration);
    }

    public void LoadGameScene()
    {
        // 실제 게임 씬으로 전환
        SceneManager.LoadScene("Main");
    }
    IEnumerator FadeInWarningMessage()
    {
        // 경고문 페이드 인
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            warningMessageCanvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        // 경고문이 표시된 후 일정 시간 후에 페이드 아웃
        yield return new WaitForSeconds(warningDisplayDuration);
        StartCoroutine(FadeOutWarningMessage());
    }

    IEnumerator FadeOutWarningMessage()
    {
        // 경고문 페이드 아웃
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            warningMessageCanvasGroup.alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            yield return null;
        }

        // 경고문이 사라지면 게임 씬 로드
        LoadGameScene();
    }
}
