using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    
    private enum VolumeType
    {
        MASTER,

        MUSIC,

        AMBIENCE,

        SFX,

    }

    [Header("Type")]
    [SerializeField] private VolumeType volumeType;

    private Slider volumeSlider;

    private void Awake()
    {
        volumeSlider = this.GetComponentInChildren<Slider>();
    }

    private void Update()
    {
        switch(volumeType)
        {
            case VolumeType.MASTER:
                volumeSlider.value = MenuVolumeControls.instance.masterVolume;
                break;

            case VolumeType.MUSIC:
                volumeSlider.value = MenuVolumeControls.instance.musicVolume;
                break;

            case VolumeType.AMBIENCE:
                volumeSlider.value = MenuVolumeControls.instance.ambienceVolume;
                break;

            case VolumeType.SFX:
                volumeSlider.value = MenuVolumeControls.instance.sfxVolume;
                break;

                default:
                Debug.LogWarning("Volume Type not supported: " + volumeType);
                break;  
        }

    }

    public void OnSliderValueChanged()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER:
                MenuVolumeControls.instance.masterVolume = volumeSlider.value;
                break;

            case VolumeType.MUSIC:
                MenuVolumeControls.instance.musicVolume = volumeSlider.value;
                break;

            case VolumeType.AMBIENCE:
                MenuVolumeControls.instance.ambienceVolume = volumeSlider.value;
                break;

            case VolumeType.SFX:
                MenuVolumeControls.instance.sfxVolume = volumeSlider.value;
                break;

            default:
                Debug.LogWarning("Volume Type not supported: " + volumeType);
                break;
        }
    }

}
