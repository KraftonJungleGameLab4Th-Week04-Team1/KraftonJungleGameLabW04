using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // 사운드매니저는 독자적인 싱글톤 패턴을 사용합니다.
    // 이유는 일반 클릭같은 경우는 액션이 따로 있지 않기 때문에, 뭐는 액션구독, 뭐는 따로 호출하는건 일관성이 없어서입니다.
    // GameManager 오브젝트에 스크립트 컴포넌트로 추가되어 있습니다.
    // 일단은 개발편의상 드래그앤드롭형식으로 오디오소스를 넣어주나, 이후 절대경로로 접근하게 리팩토링 가능합니다.
    // 따라서 누락되었을 수 있습니다. Resource/Sounds 에서 찾아주세요
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource actionAudioSource; // 단발 오디오 소스 (클릭, 버튼 클릭 등) bgm은 넣으면 안됨.

    [SerializeField] private AudioClip basicClickSound;
    [SerializeField] private float basicClickSoundVolume;
    [SerializeField] private AudioClip aircraftMoveSound;
    [SerializeField] private float aircraftMoveSoundVolume;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
        }
    }

    private void Start()
    {
        //actionAudioSource = GetComponent<AudioSource>();
        GameManager.Instance.OnArriveAction += _ => StopSound();
    }

    private void PlaySound(AudioClip clip, float volume)
    {
        if (clip != null && actionAudioSource != null)
        {
            actionAudioSource.PlayOneShot(clip, volume);
        }
    }

    private void StopSound()
    {
        Debug.Log("사운드 정지");
        actionAudioSource.Stop();
    }

    public void PlayBasicClickSound()
    {
        PlaySound(basicClickSound, basicClickSoundVolume);
    }

    public void PlayAircraftMoveSound()
    {
        PlaySound(aircraftMoveSound, aircraftMoveSoundVolume);
        Debug.Log("비행기 이동 사운드 재생");
    }
}
