using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    // Reference to audio mixer to change the volume with slider
    public AudioMixer audioMixer;

    // Variable to save possible screen resolutions for PC
    Resolution[] resolutions;

    // Reference UI element to add resolutions to dropdown menu
    public Dropdown resolutionDropdown;

    // Figure out screen resolution when starting game
    void Start()
    {
        resolutions = Screen.resolutions;

        // Make resolutions blank in resolution dropdown
        resolutionDropdown.ClearOptions();

        // Add options to Dropdown menu
        List<string> options = new List<string>(); //Need to convert array of resolutions into array of strings for AddOptions()
        int currentResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if(resolutions[i].width == Screen.currentResolution.width
                && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
    }

    // Take value from slider to adjust volume
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    // Toggle fullscreen mode
    public void SetFullScreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
