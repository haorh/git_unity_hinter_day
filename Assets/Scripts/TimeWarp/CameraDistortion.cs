using System.Collections;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class CameraDistortion : MonoBehaviour {

    enum State
    {
        idle, start, revert
    }

    public GameObject character;
    public Camera _mainCamera; // FOV 60 - 100
    public AudioSource _warpSound;
    public float _startduration = 2f;
    public float _endDuration = 0.2f;
    public float _iFov = 60;
    public float _mFov = 100;
    public float _iVignette = 0.12f;
    public float _mVignette = 0.30f;
    public float _iChrome = 0f;
    public float _mChrome = 80f;
    public float _iFishEyeX = 0.117f;
    public float _mFishEyeX = 1.5f;
    public float _iFishEyeY = 0.117f;
    public float _mFishEyeY = 1.5f;

    public ColorCorrectionCurves _colorSaturation; // 0 -> 1
    public VignetteAndChromaticAberration _vignette; //0.12 -> 0.3
    public Fisheye _fishEye; // 0.117 -> 1
    State _state = State.idle;
    float _startTime = 0;

	void Update () {
		if(Input.GetMouseButtonDown(0) && _state == State.idle)
        {
            _startTime = Time.time;
            _state = State.start;
            _warpSound.Play();
        }

        if (_state == State.start)
            StartWarpEffect();
        else if (_state == State.revert)
            RevertWarpEffect();
	}

    void StartWarpEffect()
    {        
        if (_mainCamera.fieldOfView < _mFov - 5)
        {
            float t = (Time.time - _startTime) / _startduration;            
            //_colorSaturation.saturation = Mathf.SmoothStep(_colorSaturation.saturation, 0, t);
            _vignette.intensity = Mathf.SmoothStep(_vignette.intensity, _mVignette, t);
            _vignette.chromaticAberration = Mathf.SmoothStep(_vignette.chromaticAberration, _mChrome, t);
            //_fishEye.strengthX = Mathf.SmoothStep(_fishEye.strengthX, _mFishEyeX, t);
            _fishEye.strengthY = Mathf.SmoothStep(_fishEye.strengthY, _mFishEyeY, t);
            _mainCamera.fieldOfView = Mathf.SmoothStep(_mainCamera.fieldOfView, _mFov, t);

        }
        else
        {
            StartCoroutine(TravelComplete());
            _startTime = Time.time;
            _state = State.revert;
        }
    }

    void RevertWarpEffect()
    {
        if (_mainCamera.fieldOfView > _iFov)
        {
            float t = (Time.time - _startTime) / _endDuration;
            //_colorSaturation.saturation = Mathf.SmoothStep(_colorSaturation.saturation, 1, t);
            _vignette.intensity = Mathf.SmoothStep(_vignette.intensity, _iVignette, t);
            _vignette.chromaticAberration = Mathf.SmoothStep(_vignette.chromaticAberration, _iChrome, t);
            //_fishEye.strengthX = Mathf.SmoothStep(_fishEye.strengthX, _iFishEyeX, t);
            _fishEye.strengthY = Mathf.SmoothStep(_fishEye.strengthY, _iFishEyeY, t);
            _mainCamera.fieldOfView = Mathf.SmoothStep(_mainCamera.fieldOfView, _iFov, t);            
        }
        else                    
            _state = State.idle;        
    }
    
    IEnumerator TravelComplete()
    {
        yield return new WaitForSeconds(_endDuration / 8);
        GameObject.FindObjectOfType<SceneSwitcher>().TeleportPlayer(character);
    }
}
