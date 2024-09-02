using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainGameUI : MonoBehaviour
{
    private UIDocument _uiDocumunt;
    private VisualElement root;

    private VisualElement motherScreen;

    private VisualElement settingScreen;
    private VisualElement settings;
    private VisualElement anotherViewer;

    private CamMove _cammove;
    private GameInput _gameInput;

    [SerializeField] private RenderTexture _instrumentRenderTex;
    [SerializeField] private RenderTexture _fightRenderTex;

    private List<InstrumentManager> _instrumentManagers;

    private bool isStop = false;

    StyleBackground instrumentView;
    StyleBackground fightView;

    [SerializeField] private AudioMixer myAudioMixer;

    private enum NowViewer
    {
        INSTRUMENT,
        FIGHT
    }

    [SerializeField] NowViewer Viewer;

    private void Awake()
    {
        isStop = false;
        Viewer = NowViewer.FIGHT;
        _cammove = FindObjectOfType<CamMove>();
        _gameInput = FindObjectOfType<GameInput>();
        _uiDocumunt = GetComponent<UIDocument>();
        root = _uiDocumunt.rootVisualElement;
        _instrumentManagers = FindObjectsOfType<InstrumentManager>().ToList();
    }

    private void OnEnable()
    {
        motherScreen = root.Q<VisualElement>("MotherScreen");
        settingScreen = root.Q<VisualElement>("SettingScreen");
        settings = root.Q<VisualElement>("settings");

        instrumentView = new StyleBackground(Background.FromRenderTexture(_instrumentRenderTex));
        fightView = new StyleBackground(Background.FromRenderTexture(_fightRenderTex));
        anotherViewer = root.Q<VisualElement>("anotherviewer");
        var exitbuttons = root.Query<Button>(className: "exit-button").ToList(); // 22일에 배운걸로 교체\
        foreach (var button in exitbuttons)
        {
            button.RegisterCallback<ClickEvent>(evt =>
            {
                settingScreen.style.right = new Length(0, LengthUnit.Percent);
                motherScreen.style.bottom = new Length(100, LengthUnit.Percent);
                _gameInput.GameInputEnable();
            });

        }
        var returnbuttons = root.Query<Button>(className: "return-button").ToList(); // 22일에 배운걸로 교체
        foreach (var item in returnbuttons)
        {
            item.RegisterCallback<ClickEvent>
                (evt => settingScreen.style.right = new Length(0, LengthUnit.Percent));
        }
        var UtillitysButton = root.Q<Button>("utillitys-button");
        UtillitysButton.RegisterCallback<ClickEvent>(evt =>
        {
            // etc
            settings.style.bottom = new Length(-100, LengthUnit.Percent); // 1칸위
            settingScreen.style.right = new Length(100, LengthUnit.Percent);
        });
        var soundSettingbutton = root.Q<Button>("soundSetting-button");
        soundSettingbutton.RegisterCallback<ClickEvent>(evt =>
        {
            // etc
            settings.style.bottom = new Length(0, LengthUnit.Percent); // 기본
            settingScreen.style.right = new Length(100, LengthUnit.Percent);
        });
        var viewChangebutton = root.Q<Button>("viewChange-button");
        viewChangebutton.RegisterCallback<ClickEvent>(evt =>
        {
            _cammove.CameraMove();
            switch (Viewer)
            {
                case NowViewer.INSTRUMENT:
                    anotherViewer.style.backgroundImage = fightView;
                    Viewer = NowViewer.FIGHT;
                    _gameInput.MapScrollEvent(true);
                    break;

                case NowViewer.FIGHT:
                    anotherViewer.style.backgroundImage = instrumentView;
                    Viewer = NowViewer.INSTRUMENT;
                    _gameInput.MapScrollEvent(false);
                    break;

                default:
                    break;
            }
        });
        var speedChangeToggle = root.Q<Toggle>("speedChange-toggle");
        speedChangeToggle.RegisterValueChangedCallback<bool>(evt =>
        {
            _gameInput.SpeedToogle = evt.newValue;
        });
        var toneChangeToogle = root.Q<Toggle>("toneChange-toggle");
        toneChangeToogle.RegisterValueChangedCallback<bool>(evt =>
        {
            _gameInput.ToneToogle = evt.newValue;
        });
        var resetTickButton = root.Q<Button>("play-button");
        resetTickButton.RegisterCallback<ClickEvent>(Play);
        var stopToggle = root.Q<Toggle>("stop-toggle");
        stopToggle.RegisterValueChangedCallback<bool>(evt =>
        {
            if (evt.newValue == true)
            {
                isStop = true;
                foreach (var manager in _instrumentManagers)
                {
                    manager.TickStop();
                }
            }
            else if (evt.newValue == false)
            {
                isStop = false;
                foreach (var manager in _instrumentManagers)
                {
                    manager.TickStart();
                }
            }
        });
        var homeButton = root.Q<Button>("home-button");
        homeButton.RegisterCallback<ClickEvent>(evt =>
        {
            SceneManager.LoadScene("Lobby");
        });
        var settingButton = root.Q<Button>("setting-button");
        settingButton.RegisterCallback<ClickEvent>(evt =>
        {
            motherScreen.style.bottom = new Length(0, LengthUnit.Percent);
            _gameInput.UIInputEnable();
        });

        #region Sliders

        var MasterSlider = root.Q<Slider>("master-slider");
        MasterSlider.RegisterValueChangedCallback(evt =>
        {
            myAudioMixer.SetFloat("Master", evt.newValue);
        });
        var InstrumentSlider = root.Q<Slider>("instrument-slider");
        InstrumentSlider.RegisterValueChangedCallback(evt =>
        {
            myAudioMixer.SetFloat("Instrument", evt.newValue);
        });
        var DrumSlider = root.Q<Slider>("drum-slider");
        DrumSlider.RegisterValueChangedCallback(evt =>
        {
            myAudioMixer.SetFloat("Drum", evt.newValue);
        });
        var BaseSlider = root.Q<Slider>("base-slider");
        BaseSlider.RegisterValueChangedCallback(evt =>
        {
            myAudioMixer.SetFloat("Base", evt.newValue);
        });
        var GuitarSlider = root.Q<Slider>("guitar-slider");
        GuitarSlider.RegisterValueChangedCallback(evt =>
        {
            myAudioMixer.SetFloat("Guitar", evt.newValue);
        });
        var HarpSlider = root.Q<Slider>("harp-slider");
        HarpSlider.RegisterValueChangedCallback(evt =>
        {
            myAudioMixer.SetFloat("Harp", evt.newValue);
        });
        var MarimbaSlider = root.Q<Slider>("marimba-slider");
        MarimbaSlider.RegisterValueChangedCallback(evt =>
        {
            myAudioMixer.SetFloat("Marimba", evt.newValue);
        });
        var PianoSlider = root.Q<Slider>("piano-slider");
        PianoSlider.RegisterValueChangedCallback(evt =>
        {
            myAudioMixer.SetFloat("Piano", evt.newValue);
        });
        var FluteSlider = root.Q<Slider>("flute-slider");
        FluteSlider.RegisterValueChangedCallback(evt =>
        {
            myAudioMixer.SetFloat("Flute", evt.newValue);
        });
        #endregion

        #region Save&Load
        var LoadIndex = root.Q<TextField>("loadIndex-txt");
        LoadIndex.RegisterValueChangedCallback<string>(evt =>
        {
            try
            {
                GameManager.Instance.FileManager.LoadIndex = evt.newValue;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                GameManager.Instance.FileManager.LoadIndex = "";
                return;
            }
        });

        var SaveIndex = root.Q<TextField>("saveIndex-txt");
        SaveIndex.RegisterValueChangedCallback<string>(evt =>
        {
            try
            {
                GameManager.Instance.FileManager.SaveIndex = evt.newValue;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                GameManager.Instance.FileManager.SaveIndex = "";
                return;
            }
        });
        var LoadButton = root.Q<Button>("Load-button");
        LoadButton.RegisterCallback<ClickEvent>(evt =>
        {
            StartCoroutine(GameManager.Instance.FileManager.Load());
        });

        var SaveButton = root.Q<Button>("save-button");
        SaveButton.RegisterCallback<ClickEvent>(evt =>
        {
            StartCoroutine(GameManager.Instance.FileManager.Save());
        });
        #endregion
    }

    private async void Play(ClickEvent evt)
    {
        if (isStop == true || GameManager.Instance.PlayMode == true) return;

        GameManager.Instance.FileManager.Play();

        await Task.Delay(250);
        GameManager.Instance.PlayMode = true;
        foreach (var manager in _instrumentManagers)
        {
            manager.ResetTick();
        }

        _cammove.FightAreaView();
        anotherViewer.style.backgroundImage = instrumentView;
        Viewer = NowViewer.INSTRUMENT;
        _gameInput.MapScrollEvent(false);
    }

}
