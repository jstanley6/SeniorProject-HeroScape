using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ImageScrollList : MonoBehaviour
{
    public GameObject imagePrefab;
    public Transform contentPanel;
    public InputField searchField;

    private List<GameObject> imageObjects = new List<GameObject>();
    private List<Sprite> allImages = new List<Sprite>();

    void Start()
    {
        LoadImages();
        searchField.onValueChanged.AddListener(FilterImages);
    }

    void LoadImages()
    {
        
        Sprite[] sprites = Resources.LoadAll<Sprite>("HeroscapePieces"); // Place images in Assets/Resources/Images
        allImages = sprites.ToList();

        foreach (Sprite sprite in allImages)
        {
            AddImageItem(sprite);
        }
    }

    void AddImageItem(Sprite sprite)
    {
        GameObject newItem = Instantiate(imagePrefab, contentPanel);
        newItem.GetComponent<Image>().sprite = sprite;
        imageObjects.Add(newItem);
    }

    void FilterImages(string query)
    {
        query = query.ToLower();

        for (int i = 0; i < allImages.Count; i++)
        {
            bool matchesSearch = allImages[i].name.ToLower().Contains(query);
            imageObjects[i].SetActive(matchesSearch);
        }
    }
}

