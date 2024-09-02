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
            //����Ƽ ���������� ���� ������ �����͸� �������� �Լ�
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.responseCode != 200)
            {
                Debug.Log($"{sheetID} : �ҷ����� �� ���� �߻�");
                yield break;
            }
             
            string fileText = www.downloadHandler.text;
            Debug.Log($"{sheetID} : �ε� �Ϸ�. �ؽ�Ʈ ������ �Ľ� ����");
            yield return null; //�ؽ�Ʈ�� ui�� �׷��� �ð� Ȯ��

            string[] lines = fileText.Split("\n");

            //ù��° ���� ����ϱ� ���� �о�
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
                Debug.Log($"\n {sheetID} : �ؽ�Ʈ �Ľ��� ���� �߻� : �ùٸ��� ���� ��");
                Debug.Log($"\n {sheetID} : {lineNum} ���ο��� ����");
                Debug.Log($"\n{e.Message}");

                yield break;
            }
            //Try���� ������ ������ ��������
            finally
            {

            }

            Debug.Log($"\n {sheetID} �κ��� {lineNum - 1} ���� ������ ���������� �ۼ��Ϸ�");

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
            //������ �ֳ�Ȯ���ϰ�
            if (asset == null)
            { //������ ��������
                asset = ScriptableObject.CreateInstance<CardSO>();
                asset.curName = name;
                asset.description = description;
                string filename = UnityEditor.AssetDatabase.GenerateUniqueAssetPath($"Assets/09.SO/SheetLoader/{name}.asset");
                //Create���� ���� �־�� �� 2023.07.18
                AssetDatabase.DeleteAsset(filename);
                AssetDatabase.CreateAsset(asset, filename);
            }
            else
            { // �ִٸ� ���� ����
                asset.curName = name;
                asset.description = description;
                EditorUtility.SetDirty(asset);
            }

        }
    }


}