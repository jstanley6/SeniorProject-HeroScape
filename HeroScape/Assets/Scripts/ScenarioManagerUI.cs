using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioManagerUI : MonoBehaviour
{
    // Start is called before the first frame update

    public InputField titleInputField;
    //public Dropdown scenarioTitlesDropdown;
    public string currentScenarioTitle;

    private List<string> listOfscenarios = new List<string>();
    private int selectedScenarioIndex = -1;

    void Start()
    {
        currentScenarioTitle = "";
        titleInputField.text = currentScenarioTitle;
        titleInputField.onEndEdit.AddListener(OnEnterPressed);
        //scenarioTitlesDropdown.onValueChanged.AddListener(SelectScenario);
    }

    public void SelectScenario(int index)
    {
        if (index < 0 || index >= listOfscenarios.Count) return;

        selectedScenarioIndex = index;
        titleInputField.text = listOfscenarios[index]; // Load title for editing
    }

    void OnEnterPressed(string input)
    {
        string title = input.Trim();
        if (string.IsNullOrEmpty(title)) return; // ignore empty input

        if (selectedScenarioIndex == -1)
        {
            // Add new scenario
            if (!listOfscenarios.Contains(title))
            {
                listOfscenarios.Add(title);
                //RefreshDropdown();
            }
        }
        else
        {
            // Update selected scenario
            listOfscenarios[selectedScenarioIndex] = title;
            //RefreshDropdown();
            selectedScenarioIndex = -1; // Reset selection
        }

        titleInputField.text = ""; // Clear input field
    }

    //void RefreshDropdown()
    //{
    //    scenarioTitlesDropdown.ClearOptions();
    //    scenarioTitlesDropdown.AddOptions(listOfscenarios);
    //}

    // Update is called once per frame
    void Update()
    {
        
    }
}
