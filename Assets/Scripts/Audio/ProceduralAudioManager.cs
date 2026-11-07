using UnityEngine;

public class ProceduralAudioManager : MonoBehaviour
{
    public static ProceduralAudioManager Instance { get; private set; }

    private AudioSource sfxSource;
    private AudioSource ambientSource;

    private AudioClip jumpClip;
    private AudioClip crystalClip;
    private AudioClip plateClip;
    private AudioClip gateClip;
    private AudioClip solveClip;
    private AudioClip errorClip;
    private AudioClip rockClip;
    private AudioClip relicClip;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;

        ambientSource = gameObject.AddComponent<AudioSource>();
        ambientSource.loop = true;
        ambientSource.volume = 0.25f;

        GenerateClips();
        StartAmbientLoop();
    }

    void GenerateClips()
    {
        jumpClip = CreateToneClip(320f, 480f, 0.2f, 0.35f);
        crystalClip = CreateArpeggioClip(new float[] { 523.25f, 659.25f, 783.99f, 1046.50f }, 0.4f, 0.4f);
        plateClip = CreateToneClip(220f, 180f, 0.25f, 0.4f);
        gateClip = CreateNoiseRumbleClip(1.8f, 0.5f);
        solveClip = CreateChordClip(new float[] { 440f, 554.37f, 659.25f, 880f }, 1.0f, 0.5f);
        errorClip = CreateBuzzerClip(130f, 0.35f, 0.45f);
        rockClip = CreateNoiseRumbleClip(0.6f, 0.6f);
        relicClip = CreateArpeggioClip(new float[] { 440f, 554.37f, 659.25f, 880f, 1108.73f, 1318.51f }, 1.8f, 0.6f);
    }

    void StartAmbientLoop()
    {
        AudioClip breeze = CreateBreezeLoopClip(3.0f, 0.15f);
        ambientSource.clip = breeze;
        ambientSource.Play();
    }

    public void PlayJumpSound() { if (jumpClip) sfxSource.PlayOneShot(jumpClip, 0.4f); }
    public void PlayCrystalPickupSound() { if (crystalClip) sfxSource.PlayOneShot(crystalClip, 0.6f); }
    public void PlayPlateClickSound() { if (plateClip) sfxSource.PlayOneShot(plateClip, 0.5f); }
    public void PlayGateGrindSound() { if (gateClip) sfxSource.PlayOneShot(gateClip, 0.65f); }
    public void PlayPuzzleSolvedSound() { if (solveClip) sfxSource.PlayOneShot(solveClip, 0.7f); }
    public void PlayPuzzleErrorSound() { if (errorClip) sfxSource.PlayOneShot(errorClip, 0.55f); }
    public void PlayRockImpactSound() { if (rockClip) sfxSource.PlayOneShot(rockClip, 0.6f); }
    public void PlayRelicClaimedSound() { if (relicClip) sfxSource.PlayOneShot(relicClip, 0.85f); }

    AudioClip CreateToneClip(float startFreq, float endFreq, float duration, float volume)
    {
        int sampleRate = 44100;
        int samplesCount = Mathf.CeilToInt(sampleRate * duration);
        float[] samples = new float[samplesCount];

        for (int i = 0; i < samplesCount; i++)
        {
            float t = (float)i / samplesCount;
            float freq = Mathf.Lerp(startFreq, endFreq, t);
            float envelope = Mathf.Sin(t * Mathf.PI); // Smooth attack/decay
            samples[i] = Mathf.Sin(2f * Mathf.PI * freq * ((float)i / sampleRate)) * envelope * volume;
        }

        AudioClip clip = AudioClip.Create("ToneClip", samplesCount, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }

    AudioClip CreateArpeggioClip(float[] freqs, float duration, float volume)
    {
        int sampleRate = 44100;
        int samplesCount = Mathf.CeilToInt(sampleRate * duration);
        float[] samples = new float[samplesCount];
        int noteSamples = samplesCount / freqs.Length;

        for (int note = 0; note < freqs.Length; note++)
        {
            float freq = freqs[note];
            int startIdx = note * noteSamples;
            int endIdx = Mathf.Min(startIdx + noteSamples, samplesCount);

            for (int i = startIdx; i < endIdx; i++)
            {
                float localT = (float)(i - startIdx) / noteSamples;
                float envelope = Mathf.Sin(localT * Mathf.PI);
                samples[i] += Mathf.Sin(2f * Mathf.PI * freq * ((float)i / sampleRate)) * envelope * volume;
            }
        }

        AudioClip clip = AudioClip.Create("ArpeggioClip", samplesCount, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }

    AudioClip CreateChordClip(float[] freqs, float duration, float volume)
    {
        int sampleRate = 44100;
        int samplesCount = Mathf.CeilToInt(sampleRate * duration);
        float[] samples = new float[samplesCount];

        for (int i = 0; i < samplesCount; i++)
        {
            float t = (float)i / samplesCount;
            float envelope = Mathf.Sin(t * Mathf.PI);
            float sum = 0f;
            foreach (float f in freqs)
            {
                sum += Mathf.Sin(2f * Mathf.PI * f * ((float)i / sampleRate));
            }
            samples[i] = (sum / freqs.Length) * envelope * volume;
        }

        AudioClip clip = AudioClip.Create("ChordClip", samplesCount, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }

    AudioClip CreateBuzzerClip(float freq, float duration, float volume)
    {
        int sampleRate = 44100;
        int samplesCount = Mathf.CeilToInt(sampleRate * duration);
        float[] samples = new float[samplesCount];

        for (int i = 0; i < samplesCount; i++)
        {
            float t = (float)i / samplesCount;
            float envelope = 1f - t;
            float val = Mathf.Sin(2f * Mathf.PI * freq * ((float)i / sampleRate)) > 0f ? 1f : -1f;
            samples[i] = val * envelope * volume;
        }

        AudioClip clip = AudioClip.Create("BuzzerClip", samplesCount, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }

    AudioClip CreateNoiseRumbleClip(float duration, float volume)
    {
        int sampleRate = 44100;
        int samplesCount = Mathf.CeilToInt(sampleRate * duration);
        float[] samples = new float[samplesCount];
        float lastVal = 0f;

        for (int i = 0; i < samplesCount; i++)
        {
            float t = (float)i / samplesCount;
            float envelope = Mathf.Sin(t * Mathf.PI);
            float white = Random.Range(-1f, 1f);
            // Low-pass Brownian filter for rumble
            lastVal = (lastVal + (0.05f * white)) / 1.05f;
            samples[i] = lastVal * envelope * volume * 3.5f;
        }

        AudioClip clip = AudioClip.Create("RumbleClip", samplesCount, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }

    AudioClip CreateBreezeLoopClip(float duration, float volume)
    {
        int sampleRate = 44100;
        int samplesCount = Mathf.CeilToInt(sampleRate * duration);
        float[] samples = new float[samplesCount];
        float filtered = 0f;

        for (int i = 0; i < samplesCount; i++)
        {
            float white = Random.Range(-1f, 1f);
            filtered = Mathf.Lerp(filtered, white, 0.03f);
            samples[i] = filtered * volume;
        }

        AudioClip clip = AudioClip.Create("BreezeLoop", samplesCount, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }
}
