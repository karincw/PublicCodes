using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace CW
{
    public class SpreadSheetLoader : MonoBehaviour
    {
        private readonly string documentID = "1hxz8zOcXDzjCdvcK8zhihAWl85gQUqRsmOrN93Boaqs";

        IEnumerator GetDataFromSheet(string sheetID = "0", Action<string[]> Processs = null)
        {
            UnityWebRequest www = UnityWebRequest.Get($"https://docs.google.com/spreadsheets/d/{documentID}/export?format=tsv&gid={sheetID}");
            //유니티 내부적으로 웹과 연결해 데이터를 가져오는 함수
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.responseCode != 200)
            {
                Debug.Log($"{sheetID} : 불러오기 중 오류 발생");
                yield break;
            }
             
            string fileText = www.downloadHandler.text;
            Debug.Log($"{sheetID} : 로딩 완료. 텍스트 데이터 파싱 시작");
            yield return null; //텍스트가 ui에 그려질 시간 확보

            string[] lines = fileText.Split("\n");

            //첫번째 줄은 헤더니까 빼고 읽어
            int lineNum = 1;
            try
            {
                for (lineNum = 1; lineNum < lines.Length; lineNum++)
                {
                    string[] dataArr = lines[lineNum].Split("\t");
                    Processs.Invoke(dataArr);
                }
            }
            catch (Exception e)
            {
                Debug.Log($"\n {sheetID} : 텍스트 파싱중 오류 발생 : 올바르지 않은 값");
                Debug.Log($"\n {sheetID} : {lineNum} 라인에서 오류");
                Debug.Log($"\n{e.Message}");

                yield break;
            }
            //Try문을 나갈때 무조건 실행해줌
            finally
            {

            }

            Debug.Log($"\n {sheetID} 로부터 {lineNum - 1} 개의 파일이 성공적으로 작성완료");

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [ContextMenu("Create")]
        public void CreateTest()
        {
            StartCoroutine(GetDataFromSheet("0", (dataArr) => 
            {
                CreateScriptAbleObject(dataArr[0], 
                    dataArr[1]); 
            }));
        }

        private void CreateScriptAbleObject(string name, string description)
        {
            CardSO asset;

            asset = AssetDatabase.LoadAssetAtPath<CardSO>($"Assets/09.SO/SheetLoader/{name}.asset");
            //에셋이 있나확인하고
            if (asset == null)
            { //없으면 만들어줘라
                asset = ScriptableObject.CreateInstance<CardSO>();
                asset.curName = name;
                asset.description = description;
                string filename = UnityEditor.AssetDatabase.GenerateUniqueAssetPath($"Assets/09.SO/SheetLoader/{name}.asset");
                //Create전에 값을 넣어야 해 2023.07.18
                AssetDatabase.DeleteAsset(filename);
                AssetDatabase.CreateAsset(asset, filename);
            }
            else
            { // 있다면 값만 변경
                asset.curName = name;
                asset.description = description;
                EditorUtility.SetDirty(asset);
            }

        }
    }


}