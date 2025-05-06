using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Kit> kits = new List<Kit>();
    public Text feedback;

    void Start()
    {
        LoadKits("kits");
        StartCoroutine(CheckKitMatch());
    }

    IEnumerator CheckKitMatch()
    {
        while (true)
        {
            FindClosestKitMatch();
            yield return new WaitForSeconds(5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(kits.Count > 0 && Input.GetKeyDown(KeyCode.C))
        {
            FindClosestKitMatch();
        }
    }

    void LoadKits(string fileName)
    {
        TextAsset file = Resources.Load<TextAsset>(fileName);
        if (file == null)
        {
            Debug.LogError("Kit file not found!");
            return;
        }

        string[] lines = file.text.Split('\n');
        Debug.Log("Kit file successfully loaded.");

        Kit currentKit = null;

        foreach (string line in lines)
        {
            string trimmed = line.Trim();

            if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith("#"))
            {
                continue;
            }
            if (trimmed.StartsWith("Kit:"))
            {
                if (currentKit != null)
                {
                    kits.Add(currentKit);
                    Debug.Log($"Kit loaded: {currentKit.kitName} with {currentKit.kitPieces.Count} pieces.");
                }
                currentKit = new Kit(trimmed.Substring(4).Trim());
            }
            else if (currentKit != null && line.Contains(":"))
            {
                string[] parts = line.Split(':');
                string pieceName = parts[0].Trim();
                int quantity = int.Parse(parts[1].Trim());
                currentKit.AddPiece(pieceName, quantity);
            }
        }
        if (currentKit != null)
        {
            kits.Add(currentKit);
            Debug.Log($"Kit loaded: {currentKit.kitName} | Total Pieces: {currentKit.TotalQuantity()}");
        }

        Debug.Log($"Total kits loaded: {kits.Count}");
    }

    void compareSceneToKit(Kit kit)
    {
        //    foreach (Kit currentKit in kits)
        //    {
        //        Debug.Log($"--- Comparing scene to kit: {kit.kitName} ---");

        //        Dictionary<string, int> sceneCounts = new Dictionary<string, int>();

        //        // Find all GameObjects in the scene
        //        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        //        // Count the occurrences of pieces in the scene
        //        foreach (GameObject obj in allObjects)
        //        {
        //            if (!obj.scene.IsValid()) continue;

        //            string objName = obj.name.Replace("(Clone)", "").Trim(); // Clean up the clone name suffix

        //            if (kit.kitPieces.ContainsKey(objName)) // Only count pieces that belong to the current kit
        //            {
        //                if (sceneCounts.ContainsKey(objName))
        //                {
        //                    sceneCounts[objName]++;
        //                }
        //                else
        //                {
        //                    sceneCounts[objName] = 1;
        //                }
        //            }
        //        }

        //        // Compare the scene counts with the expected counts in the kit
        //        foreach (var piece in kit.kitPieces)
        //        {
        //            int sceneCount = sceneCounts.ContainsKey(piece.Key) ? sceneCounts[piece.Key] : 0;
        //            string message = $"Piece: {piece.Key} | Expected: {piece.Value} | In Scene: {sceneCount}";

        //            if (sceneCount == piece.Value)
        //            {
        //                Debug.Log($"✔ {message}");
        //            }
        //            else
        //            {
        //                Debug.LogWarning($"❌ {message}");
        //            }
        //        }

        //        // Check for extra pieces in the scene (pieces not in the kit)
        //        foreach (var kvp in sceneCounts)
        //        {
        //            if (!kit.kitPieces.ContainsKey(kvp.Key))
        //            {
        //                Debug.LogWarning($"Extra piece in scene not in kit: {kvp.Key} x{kvp.Value}");
        //            }
        //        }
        //    }
        //}
        Debug.Log($"--- Comparing scene to kit: {kit.kitName} ---");

        Dictionary<string, int> sceneCounts = new Dictionary<string, int>();

        // Find all GameObjects in the scene
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        // Count the occurrences of pieces in the scene
        foreach (GameObject obj in allObjects)
        {
            if (!obj.scene.IsValid()) continue;

            string objName = obj.name.Replace("(Clone)", "").Trim();

            if (kit.kitPieces.ContainsKey(objName))
            {
                if (sceneCounts.ContainsKey(objName))
                    sceneCounts[objName]++;
                else
                    sceneCounts[objName] = 1;
            }
        }

        int matchedPieces = 0;
        int totalExpected = 0;

        // Compare the scene counts with expected counts in the kit
        foreach (var piece in kit.kitPieces)
        {
            int expected = piece.Value;
            int inScene = sceneCounts.ContainsKey(piece.Key) ? sceneCounts[piece.Key] : 0;

            totalExpected += expected;
            matchedPieces += Mathf.Min(expected, inScene);

            string message = $"Piece: {piece.Key} | Expected: {expected} | In Scene: {inScene}";

            if (inScene == expected)
                Debug.Log($"✔ {message}");
            else
                Debug.LogWarning($"❌ {message}");
        }

        // Check for extra pieces
        foreach (var kvp in sceneCounts)
        {
            if (!kit.kitPieces.ContainsKey(kvp.Key))
            {
                Debug.LogWarning($"Extra piece in scene not in kit: {kvp.Key} x{kvp.Value}");
            }
        }

        float similarity = (totalExpected > 0) ? (matchedPieces / (float)totalExpected) * 100f : 0f;
        Debug.Log($"🔍 Similarity to kit '{kit.kitName}': {similarity:F1}%");
    }

    void FindClosestKitMatch()
    {
        if (kits.Count == 0) return;

        Dictionary<string, int> sceneCounts = new Dictionary<string, int>();
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (!obj.scene.IsValid()) continue;

            //string objName = obj.name.Replace("(Clone)", "").Trim();
            string objName = System.Text.RegularExpressions.Regex.Replace(obj.name, @"\s*\(\d+\)|\(Clone\)", "").Trim();

            if (sceneCounts.ContainsKey(objName))
                sceneCounts[objName]++;
            else
                sceneCounts[objName] = 1;
        }

        Kit bestMatch = null;
        float bestSimilarity = 0f;

        foreach (Kit kit in kits)
        {
            int matchedPieces = 0;
            int totalExpected = 0;

            foreach (var piece in kit.kitPieces)
            {
                int expected = piece.Value;
                int inScene = sceneCounts.ContainsKey(piece.Key) ? sceneCounts[piece.Key] : 0;

                matchedPieces += Mathf.Min(expected, inScene);
                totalExpected += expected;
            }

            float similarity = (totalExpected > 0) ? (matchedPieces / (float)totalExpected) * 100f : 0f;

            Debug.Log($"Kit '{kit.kitName}' similarity: {similarity:F1}%");

            if (similarity > bestSimilarity)
            {
                bestSimilarity = similarity;
                bestMatch = kit;
            }
        }

        if (bestMatch != null)
        {
            Debug.Log($"🏆 Best matching kit: {bestMatch.kitName} ({bestSimilarity:F1}% match)");
            compareSceneToKit(bestMatch); // Optionally show detailed comparison
        }
        else
        {
            Debug.Log("⚠ No matching kits found.");
        }

        if (bestSimilarity < 60f)
        {
            Debug.Log("No suitable kit.");
        } else
        {
            Debug.Log($"✅ Recommended Kit: <b>{bestMatch.kitName}</b> — Match Score: <b>{bestSimilarity:F1}%</b>");
            feedback.text = "";
            feedback.text += $"Recommended Kit: '{bestMatch.kitName}' -- similarity: {bestSimilarity:F1}%\n";
        }
    }




    public class Kit
    {
        public string kitName;
        public Dictionary<string, int> kitPieces = new Dictionary<string, int>();
        //public float price;
        public Kit(string name)
        {
            kitName = name;
            kitPieces = new Dictionary<string, int>();
        }

        public void AddPiece(string pieceName, int quantity)
        {
            if (kitPieces.ContainsKey(pieceName))
            {
                kitPieces[pieceName] += quantity;
            }
            else
            {
                kitPieces[pieceName] = quantity;
            }
        }

        public int TotalQuantity()
        {
            int total = 0;
            foreach (var entry in kitPieces)
            {
                total += entry.Value;
            }
            return total;
        }
    }
}

