using karin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Karin
{

    public class DialogSystem : MonoSingleton<DialogSystem>
    {
        Stack<string> textStack = new Stack<string>();
        delegate void TextEvent(Action action);

        public string playerName;

        public TextMeshProUGUI TextField;
        public TextMeshProUGUI NameField;
        public RectTransform character;
        public RectTransform npc;

        [SerializeField] private bool _playOnAwake;
        [SerializeField] private NPCdataSO _data;
        [SerializeField] private UnityEvent callbackEvent; 

        public const string MethodStart = "[", value = ":", MethodEnd = "]";
        private bool isMethod = false, isProperty = false;
        private string output, methodName, property, nowName;
        private float delay = 0;

        private void Start()
        {
            if (_playOnAwake)
            {
                TextInit(_data.text);
                TextSpawn(callbackEvent.Invoke);
            }
        }

        public void TextInit(string text)
        {
            text.Split('\n').ToList().ForEach(t => textStack.Push(t));
        }

        [ContextMenu("Text")]
        public void TextSpawn(Action callback = null)
        {
            if (textStack.Count > 0)
            {
                StartCoroutine(TextSpawnCoroutine(callback));
            }
        }

        public void SetText(string t)
        {
            TextField.text = t;
        }

        public IEnumerator TextSpawnCoroutine(Action callback = null)
        {
            while (true)
            {
                if (Input.GetKeyDown(KeyCode.L))
                    break;
                yield return null;

                if (Mouse.current.leftButton.wasReleasedThisFrame)
                {
                    string text = textStack.Pop();
                    string[] SplitText = text.Split(">");
                    NameSetting(SplitText[0]);

                    StringBuilder sb = new StringBuilder(SplitText[1]);
                    methodName = "";
                    property = "";
                    output = "";

                    while (sb.Length > 0)
                    {
                        if (Input.GetKeyDown(KeyCode.L))
                            break;

                        TextSpawn(sb[0].ToString());

                        yield return new WaitForSeconds(delay);
                        delay = 0;

                        TextField.text = output;
                        sb = sb.Remove(0, 1);
                    }

                    if (textStack.Count == 0)
                    {
                        callback?.Invoke();
                        break;
                    }
                }

            }
        }

        private void NameSetting(string name)
        {
            if (NameField != null)
                NameField.text = name;
            nowName = name;
        }

        private void TextSpawn(string sb)
        {
            if (sb == MethodStart)
            {
                isMethod = true;
                return;
            }
            else if (sb == value)
            {
                isProperty = true;
                return;
            }
            else if (sb == MethodEnd)
            {
                isMethod = false;
                isProperty = false;

                Debug.Log($"method 인식 : {methodName} , {property}");
                switch (methodName)
                {
                    case "Delay":
                        try
                        {
                            float value = float.Parse(property);
                            delay = value;
                        }
                        catch (Exception e)
                        {
                            Debug.Log(e.Message);
                        }
                        break;
                    case "Face":
                        try
                        {
                            float value = float.Parse(property);
                            Debug.Log($"얼굴을 {value}번쨰얼굴로 교체 합니다.");
                        }
                        catch (Exception e)
                        {
                            Debug.Log(e.Message);
                        }
                        break;
                    case "MoveX":
                        try
                        {
                            float value = float.Parse(property);
                            if (playerName == nowName)
                            {
                                character.anchoredPosition += new Vector2(value, 0);
                            }
                            else
                            {
                                npc.anchoredPosition += new Vector2(value, 0);
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.Log(e.Message);
                        }
                        break;
                    case "MoveY":
                        try
                        {
                            float value = float.Parse(property);
                            if (playerName == nowName)
                            {
                                character.anchoredPosition += new Vector2(0, value);
                            }
                            else
                            {
                                npc.anchoredPosition += new Vector2(0, value);
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.Log(e.Message);
                        }
                        break;
                }

                methodName = "";
                property = "";
                return;
            }

            if (!isMethod && !isProperty)
            {
                output += sb;
                delay = 0.1f;
            }
            else if (isMethod && !isProperty)
            {
                methodName += sb;
            }
            else if (isMethod && isProperty)
            {
                property += sb;
            }
        }
    }
}