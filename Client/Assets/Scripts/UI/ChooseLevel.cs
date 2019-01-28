using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChooseLevel : MonoBehaviour
{
    public Transform optionBtnParent;
    public Transform optionBtn;
    public Transform SettingPanel;

    //Setting
    public Slider soundSlider;
    public Slider voiceSlider;
    public Button closeSetting;
    public Dropdown languagesDropdown;
    public Button confirmSetting;

    void Start()
    {
        Init();
        GameSoundManager.Instance.PlayCustomBGMConnnection("Town");
    }

    public void Init()
    {
        foreach (var option in GameController.config.GetOptions())
        {
            Transform optionTrans = GameObject.Instantiate<GameObject>(optionBtn.gameObject).transform;
            optionTrans.parent = optionBtnParent;
            optionTrans.localPosition = Vector3.zero;
            optionTrans.localScale = Vector3.one;
            optionTrans.gameObject.SetActive(true);
            optionTrans.name = option;
            Text text = optionTrans.Find("Text").GetComponent<Text>();
            text.text = Localization.Get(option);

            switch (option)
            {
                case "NewGame":
                    UIEventListener.Get(optionTrans.gameObject).onPointerClick = OnNewGameClick;
                    break;
                case "Loading":
                    if (UserData.Instance.records.Count > 0)
                    {
                        optionTrans.GetComponent<Button>().interactable = true;
                        UIEventListener.Get(optionTrans.gameObject).onPointerClick = OnLoadingClick;
                    }
                    else
                        optionTrans.GetComponent<Button>().interactable = false;
                    break;
                case "Setting":
                    UIEventListener.Get(optionTrans.gameObject).onPointerClick = OnSettingClick;
                    break;
                case "Exit":
                    UIEventListener.Get(optionTrans.gameObject).onPointerClick = OnExitClick;
                    break;
            }
        }

        //Setting
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume", 1f);
        voiceSlider.value = PlayerPrefs.GetFloat("VoiceVolume", 1f);
        UIEventListener.Get(closeSetting.gameObject).onPointerClick = OnCloseSettingClick;
        UpdateLanguageDropdown();
        UIEventListener.Get(confirmSetting.gameObject).onPointerClick = OnConfirmSettingClick;
    }

    private void OnNewGameClick(PointerEventData ped)
    {
        WindowManager.Close(UIMenu.ChooseLevelWnd);
        WindowManager.Open(UIMenu.TicketWnd);
    }

    private void OnLoadingClick(PointerEventData ped)
    {
        Level.recordId = 0;
        WindowManager.Close(UIMenu.ChooseLevelWnd);
        Application.LoadLevelAsync("Level");
    }

    private void OnSettingClick(PointerEventData ped)
    {
        SettingPanel.gameObject.SetActive(true);
    }

    private void OnExitClick(PointerEventData ped)
    {
        Application.Quit();
    }

    private void OnCloseSettingClick(PointerEventData ped)
    {
        SettingPanel.gameObject.SetActive(false);
    }

    private void UpdateLanguageDropdown()
    {
        languagesDropdown.options.Clear();
        foreach (var language in GameController.config.GetLanguages())
        {
            var tempData = new Dropdown.OptionData();
            tempData.text = Localization.Get(language);
            languagesDropdown.options.Add(tempData);
        }
        var curLanguage = PlayerPrefs.GetString("Language", "English");
        languagesDropdown.value = languagesDropdown.options.FindIndex(option => option.text == curLanguage);
    }

    private void OnConfirmSettingClick(PointerEventData ped)
    {
        PlayerPrefs.SetString("Language", GameController.config.GetLanguages()[languagesDropdown.value]);
        Localization.language = PlayerPrefs.GetString("Language", "English");
        SettingPanel.gameObject.SetActive(false);
        WindowManager.Close(UIMenu.ChooseLevelWnd);
        WindowManager.Open(UIMenu.ChooseLevelWnd);
    }

    void Update()
    {
        if (soundSlider.value != PlayerPrefs.GetFloat("SoundVolume"))
        {
            PlayerPrefs.SetFloat("SoundVolume", soundSlider.value);
            SoundManager.SetVolumeSFX(soundSlider.value);
        }

        if (voiceSlider.value != PlayerPrefs.GetFloat("VoiceVolume"))
        {
            PlayerPrefs.SetFloat("VoiceVolume", voiceSlider.value);
            SoundManager.SetVolumeMusic(voiceSlider.value);
        }
    }
}
