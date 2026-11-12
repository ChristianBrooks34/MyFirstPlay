public class BaseSoundController
{

}

public class SoundUIController : BaseSoundController
{
    private readonly SoundUIProfile _soundUIProfile;

    public SoundUIController(SoundUIProfile soundUIProfile)
    {
        _soundUIProfile = soundUIProfile;
    }


}


