using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using System.Runtime.InteropServices;
using static Piece;

public class SaveLoadManager : MonoBehaviour
{
    private class Scenario
    {
        public string name;
        public string description;
        public string goal;
        public string setup;
        public string victory;
        public string specialRules;
        public List<KeyValuePair<Vector3Int, SimplePiece>> terrainPieces = new List<KeyValuePair<Vector3Int, SimplePiece>>();
        //Dictionary<Vector3Int, SimplePiece> terrainPieces = new Dictionary<Vector3Int, SimplePiece>();
    }
    private class SimplePiece
    {
        public TerrainType terrainType;
        public PieceSize pieceSize;
        public int rotations;
    }


    public EditorController editor;
    public InputField nameText;
    public InputField descriptionText;
    public InputField goalText;
    public InputField setupText;
    public InputField victoryText;
    public InputField specialRulesText;
    public List<GameObject> prefabs = new List<GameObject>();
    private string uploadedContent;


    // Start is called before the first frame update
    void Start()
    {
        GameObject[] tileObjects = Resources.LoadAll<GameObject>("HexTilePrefabs");
        prefabs = tileObjects.ToList();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void NewScenario()
    {
        nameText.text = "";
        descriptionText.text = "";
        goalText.text = "";
        setupText.text = "";
        victoryText.text = "";
        specialRulesText.text = "";
        editor.ClearAll();
    }

    public void BuildJson()
    {
        Scenario scenario = new Scenario();
        scenario.name = nameText.text;
        scenario.description = descriptionText.text;
        scenario.goal = goalText.text;
        scenario.setup = setupText.text;
        scenario.victory = victoryText.text;
        scenario.specialRules = specialRulesText.text;
        //scenario.terrainPieces = editor.terrainPieces;
        foreach(var item in editor.terrainPieces)
        {
            //scenario.terrainPieces.ElementAt<Vector3Int>(item.Key)
            SimplePiece piece = new SimplePiece();
            piece.terrainType = item.Value.terrainType;
            piece.pieceSize = item.Value.size;
            piece.rotations = item.Value.rotations;
            scenario.terrainPieces.Add(new KeyValuePair<Vector3Int, SimplePiece>(item.Key, piece));
        }

        string fileName;
        StringBuilder sb = new StringBuilder();
        foreach (char c in nameText.text)
        {
            if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == ' '  || c == '_')
            {
                sb.Append(c);
            }
        }
        fileName = sb.ToString();
        //string path = Application.dataPath + "/" + fileName + ".json";

        string content = JsonConvert.SerializeObject(scenario, Newtonsoft.Json.Formatting.Indented);
        //File.WriteAllText(path, content);
        DownloadJsonToFile(fileName, content);
    }

    public void LoadFromJson()
    {
        editor.ClearAll();
        //string fileName;
        StringBuilder sb = new StringBuilder();
        foreach (char c in nameText.text)
        {
            if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == ' ' || c == '_')
            {
                sb.Append(c);
            }
        }
        //fileName = sb.ToString();
        //string path = Application.dataPath + "/" + fileName + ".json";
        //string content = File.ReadAllText(path);
        UploadFileToJson();
        Scenario scenario = JsonConvert.DeserializeObject<Scenario>(uploadedContent);
        nameText.text = scenario.name;
        descriptionText.text = scenario.description;
        goalText.text = scenario.goal;
        setupText.text = scenario.setup;
        victoryText.text = scenario.victory;
        specialRulesText.text = scenario.specialRules;

        foreach (var item in scenario.terrainPieces)
        {
            foreach (GameObject prefab in prefabs)
            {
                if (prefab.name.Equals(item.Value.pieceSize.ToString() + item.Value.terrainType.ToString()))
                {
                    GameObject newPiece = Instantiate(prefab);
                    while (editor.grid.gridHexXZLayers.Count < item.Key.y)
                    {
                        editor.grid.AddLayer();
                    }
                    newPiece.transform.position = editor.grid.gridHexXZLayers[item.Key.y].GetWorldPosition(item.Key.x, item.Key.z) + new Vector3(0, item.Key.y / 5f, 0);
                    //item.Key;
                    newPiece.GetComponent<Piece>().rotations = item.Value.rotations;
                    newPiece.transform.eulerAngles = new Vector3(0, 60 * item.Value.rotations, 0);
                    editor.ClickedOnPiece(newPiece.GetComponent<Piece>());
                    editor.ClickedOnPiece(newPiece.GetComponent<Piece>());
                    editor.LetGoOfPiece();
                    break;
                }
            }
        }
    }
#if UNITY_WEBGL && !UNITY_EDITOR
    //
    // WebGL
    //
    [DllImport("__Internal")]
    private static extern void DownloadFile(string gameObjectName, string methodName, string filename, byte[] byteArray, int byteArraySize);
    
    [DllImport("__Internal")]
    private static extern void UploadFile(string gameObjectName, string methodName, string filter, bool multiple);

    public void UploadFileToJson() {
        UploadFile(gameObject.name, "OnFileUpload", ".json", false);
    }

    // Called from browser
    public void OnFileUpload(string url) {
        OutputRoutine(url);
    }

    // Broser plugin should be called in OnPointerDown.
    public void DownloadJsonToFile(string name, string content) {
        var bytes = Encoding.UTF8.GetBytes(content);
        DownloadFile(gameObject.name, "OnFileDownload", name + ".json", bytes, bytes.Length);
    }

    // Called from browser
    public void OnFileDownload() {
        print("File Successfully Downloaded");
    }
#else
    //
    // Standalone platforms & editor
    //

    public void DownloadJsonToFile(string name, string content)
    {
        var path = StandaloneFileBrowser.SaveFilePanel("Save Scenario", "", name, "json");
        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllText(path, content);
        }
    }
    public void UploadFileToJson()
    {
        var paths = StandaloneFileBrowser.OpenFilePanel("Open Scenario", "", "json", false);
        if (paths.Length > 0)
        {
            OutputRoutine(new System.Uri(paths[0]).AbsoluteUri);
        }
    }
#endif
    private void OutputRoutine(string url)
    {
        var loader = new WWW(url);
        print("File Successfully Uploaded");
        uploadedContent = loader.text;
    }
}