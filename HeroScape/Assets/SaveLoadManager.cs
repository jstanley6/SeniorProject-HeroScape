using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using static Piece;

public class SaveLoadManager : MonoBehaviour
{
    public EditorController editor;
    public Text nameText;
    public Text descriptionText;
    public Text goalText;
    public Text setupText;
    public Text victoryText;
    public Text specialRulesText;
    public List<GameObject> prefabs = new List<GameObject>();

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

    public void ExportJson()
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
        string path = Application.dataPath + "/" + fileName + ".json";

        string content = JsonConvert.SerializeObject(scenario);
        File.WriteAllText(path, content);
    }

    public void ImportJson()
    {
        editor.ClearAll();
        string fileName;
        StringBuilder sb = new StringBuilder();
        foreach (char c in nameText.text)
        {
            if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == ' ' || c == '_')
            {
                sb.Append(c);
            }
        }
        fileName = sb.ToString();
        string path = Application.dataPath + "/" + fileName + ".json";
        string content = File.ReadAllText(path);
        Scenario scenario = JsonConvert.DeserializeObject<Scenario>(content);
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
                    newPiece.transform.position = editor.grid.gridHexXZLayers[item.Key.y].GetWorldPosition(item.Key.x, item.Key.z) + new Vector3(0, (float) item.Key.y / 5f, 0);
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
}