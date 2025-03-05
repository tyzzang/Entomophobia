using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.HID;

namespace NavKeypad
{
    public class Keypad : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private GrantEvent onGrantCard;
        [SerializeField] private UnityEvent onAccessGranted;
        [SerializeField] private UnityEvent onAccessDenied;
        [Header("Combination Code (9 Numbers Max)")]
        [SerializeField] private int keypadCombo = 12345;

        public UnityEvent OnAccessGranted => onAccessGranted;
        public UnityEvent OnAccessDenied => onAccessDenied;
        public GrantEvent OnGrantCard => onGrantCard;

        [Header("Settings")]
        [SerializeField] private string accessGrantedText = "Granted";
        [SerializeField] private string accessDeniedText = "Denied";

        [Header("Visuals")]
        [SerializeField] private float displayResultTime = 1f;
        [Range(0, 5)]
        [SerializeField] private float screenIntensity = 2.5f;
        [Header("Colors")]
        [SerializeField] private Color screenNormalColor = new Color(0.98f, 0.50f, 0.032f, 1f); //orangy
        [SerializeField] private Color screenDeniedColor = new Color(1f, 0f, 0f, 1f); //red
        [SerializeField] private Color screenGrantedColor = new Color(0f, 0.62f, 0.07f); //greenish
        [SerializeField] private Color screenCardColor = new Color(0f, 0.52f, 0.3f);
        [Header("SoundFx")]
        [SerializeField] private AudioClip buttonClickedSfx;
        [SerializeField] private AudioClip accessDeniedSfx;
        [SerializeField] private AudioClip accessGrantedSfx;
        [Header("Component References")]
        [SerializeField] private Renderer panelMesh;
        [SerializeField] private TMP_Text keypadDisplayText;
        [SerializeField] private AudioSource audioSource;

        [SerializeField]
        private string currentInput;
        private bool displayingResult = false;
        private bool accessWasGranted = false;

        [Header("What Open it")]
        [SerializeField] private GameObject m_doorObj;

        [Header("Tag Place For Card")]
        [SerializeField] private GameObject m_cardTagObj;

        private void Awake()
        {
            ClearInput();
            
            panelMesh.material.SetVector("_EmissionColor", screenNormalColor * screenIntensity);
        }


        //Gets value from pressedbutton
        public void AddInput(string input)
        {
            audioSource.PlayOneShot(buttonClickedSfx);
            if (displayingResult || accessWasGranted) return;
            switch (input)
            {
                case "enter":
                    CheckCombo();
                    break;
                default:
                    if (currentInput != null && currentInput.Length == 9) // 9 max passcode size 
                    {
                        return;
                    }
                    currentInput += input;
                    keypadDisplayText.text = currentInput;
                    break;
            }

        }
        public void CheckCombo()
        {
            if (int.TryParse(currentInput, out var currentKombo))
            {
                bool granted = currentKombo == keypadCombo;
                if (!displayingResult)
                {
                    StartCoroutine(DisplayResultRoutine(granted));
                }
            }
            else
            {
                Debug.LogWarning("Couldn't process input for some reason..");
            }

        }

        //mainly for animations 
        private IEnumerator DisplayResultRoutine(bool granted)
        {
            displayingResult = true;

            if (granted) AccessGrantedCard();
            else AccessDenied();

            yield return new WaitForSeconds(displayResultTime);
            displayingResult = false;
            //if (granted)yield break;
            ClearInput();
            panelMesh.material.SetVector("_EmissionColor", screenNormalColor * screenIntensity);

        }

        private void AccessDenied()
        {
            keypadDisplayText.text = accessDeniedText;
            onAccessDenied?.Invoke();
            panelMesh.material.SetVector("_EmissionColor", screenDeniedColor * screenIntensity);
            audioSource.PlayOneShot(accessDeniedSfx);
        }

        private void ClearInput()
        {
            

            currentInput = "";
            keypadDisplayText.text = currentInput;
            
        }

        private void AccessGranted()
        {
            //accessWasGranted = true;
            keypadDisplayText.text = accessGrantedText;
            onAccessGranted?.Invoke();

            panelMesh.material.SetVector("_EmissionColor", screenGrantedColor * screenIntensity);
            audioSource.PlayOneShot(accessGrantedSfx);
            
        }

        private void AccessGrantedCard()
        {
            keypadDisplayText.text = accessGrantedText;
            panelMesh.material.SetVector("_EmissionColor", screenCardColor * screenIntensity);
            onGrantCard?.Invoke(keypadCombo);
            audioSource.PlayOneShot(accessGrantedSfx);
        }

        public void SetCardInfoByTag(int argNum)
        {
            StartCoroutine(setCard(argNum));
        }

        public IEnumerator setCard(int argNum)
        {
            float _timer = 0.0f;
            

            while (_timer < displayResultTime)
            {
                Ray _ray = new Ray(m_cardTagObj.transform.position, m_cardTagObj.transform.forward);
                RaycastHit _hit;
                Debug.DrawRay(m_cardTagObj.transform.position, m_cardTagObj.transform.forward, Color.magenta);

                if (Physics.Raycast(_ray, out _hit, 1.0f) && _hit.transform.gameObject.layer == GManager.Instance.IsKeyMaskIndex)
                {
                    Debug.Log(_hit.transform.name);
                    _hit.transform.name = argNum.ToString();
                    panelMesh.material.SetVector("_EmissionColor", screenGrantedColor * screenIntensity);
                    yield break; // 히트가 발생하면 코루틴을 종료합니다.
                }

                _timer += Time.deltaTime;
                yield return null; // 다음 프레임까지 대기
            }
        }



    }
}