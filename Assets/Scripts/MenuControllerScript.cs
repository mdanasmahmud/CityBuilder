using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button cityButton;  // Reference to City button
    public Button populationButton;  // Reference to Population button
    public GameObject[] cityButtons;  // Array of buttons for City
    public GameObject[] populationButtons;  // Array of buttons for Population

    void Start()
    {
        // Add listeners to the buttons
        cityButton.onClick.AddListener(OnCityClicked);
        populationButton.onClick.AddListener(OnPopulationClicked);

        // Set the default state
        SetDefaultColors();
        ShowCityMenu();
    }

    void OnCityClicked()
    {
        // Change colors when "City" is clicked
        cityButton.GetComponent<Image>().color = new Color32(170, 0, 0, 255); // #AA0000
        populationButton.GetComponent<Image>().color = new Color32(128, 128, 128, 153); // Gray with 60% transparency

        // Show city-related buttons and hide population-related buttons
        ShowCityMenu();
    }

    void OnPopulationClicked()
    {
        // Change colors when "Population" is clicked
        populationButton.GetComponent<Image>().color = new Color32(124, 185, 232, 255); // #7CB9E8
        cityButton.GetComponent<Image>().color = new Color32(128, 128, 128, 153); // Gray with 60% transparency

        // Show population-related buttons and hide city-related buttons
        ShowPopulationMenu();
    }

    void ShowCityMenu()
    {
        // Enable city buttons and disable population buttons
        foreach (GameObject btn in cityButtons)
        {
            btn.SetActive(true);
        }
        foreach (GameObject btn in populationButtons)
        {
            btn.SetActive(false);
        }
    }

    void ShowPopulationMenu()
    {
        // Enable population buttons and disable city buttons
        foreach (GameObject btn in populationButtons)
        {
            btn.SetActive(true);
        }
        foreach (GameObject btn in cityButtons)
        {
            btn.SetActive(false);
        }
    }

    void SetDefaultColors()
    {
        // Set default colors for the buttons
        cityButton.GetComponent<Image>().color = new Color32(170, 0, 0, 255); // #AA0000
        populationButton.GetComponent<Image>().color = new Color32(128, 128, 128, 153); // Gray with 60% transparency
    }
}
