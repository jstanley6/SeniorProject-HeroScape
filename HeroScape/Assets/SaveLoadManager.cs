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
    // Start is called before the first frame update
    void Start()
    {
        
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
            piece.size = item.Value.size;
            piece.rotations = item.Value.rotations;
            scenario.terrainPieces.Add(item.Key, piece);
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
        string path = Application.dataPath + "/" + fileName + ".txt";

        string content = JsonConvert.SerializeObject(scenario);
        File.WriteAllText(path, content);
    }

    public void ImportJson()
    {

    }

    private class Scenario
    {
        public string name;
        public string description;
        public string goal;
        public string setup;
        public string victory;
        public string specialRules;
        public Dictionary<Vector3Int, SimplePiece> terrainPieces = new Dictionary<Vector3Int, SimplePiece>();
    }
    private class SimplePiece
    {
        public TerrainType terrainType;
        public PieceSize size;
        public int rotations;
    }
}
