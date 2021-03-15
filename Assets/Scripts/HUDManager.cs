using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    private GameObject canvas;
    private Detector detector;
    private SolutionDetector solutionDetector;
    private EmitterCone emitterCone;
    private EmitterConeSol emitterConeSol;
    private AtomsManager atomsManager;
    private SolutionManager solutionManager;
    private Wave wave;
    [SerializeReference] private Button zoom; 
    [SerializeReference] private Button lambda; 
    [SerializeReference] private Button power; 
    [SerializeReference] private Button rotation;
    [SerializeReference] private TextManager textManager;

    private bool mapDisplayed;
    private GameObject minimapButton;
    private GameObject zoomLocked;
    private GameObject lambdaLocked;
    private GameObject powerLocked;
    private GameObject rotationLocked;

    private static List<Button> buttons;
    //[SerializeReference] private Button rotateButton; 
    //[SerializeReference] private Button stopRotationButton;

    private void Awake()
    {
        minimapButton = GameObject.Find("MimimapButton");
        zoomLocked = GameObject.Find("ZoomButtonLocked");
        lambdaLocked = GameObject.Find("LambdaButtonLocked");
        powerLocked = GameObject.Find("PowerButtonLocked");
        rotationLocked = GameObject.Find("RotationButtonLocked");
        
        canvas = GameObject.Find("Canvas");

        detector = FindObjectOfType<Detector>();
        solutionDetector = FindObjectOfType<SolutionDetector>();
        emitterCone = FindObjectOfType<EmitterCone>();
        emitterConeSol = FindObjectOfType<EmitterConeSol>();
        atomsManager = FindObjectOfType<AtomsManager>();
        solutionManager = FindObjectOfType<SolutionManager>();
        wave = FindObjectOfType<Wave>();
    }

    void Start()
    {
        buttons = new List<Button>();
        
        textManager = Instantiate(textManager);

        //INSTANZIO I BOTTONI E I RELATIVI LISTENER
        //minimapButton.GetComponent<Button>().onClick.AddListener(DisplayMap);
        
        
        if (PowerUpsManger.ZoomUnlocked)
        {
            zoomLocked.SetActive(false);
            zoom = Instantiate(zoom, canvas.transform);
            buttons.Add(zoom);
            
            zoom.onClick.RemoveAllListeners();
            zoom.onClick.AddListener(delegate { DisplaySlider(zoom); });
            
            Slider zoomSlider = zoom.transform.GetChild(0).gameObject.GetComponent<Slider>();
            zoomSlider.onValueChanged.RemoveAllListeners();
            zoomSlider.onValueChanged.AddListener(delegate { detector.SetZoom(zoomSlider.value); });
            zoomSlider.onValueChanged.AddListener(delegate { textManager.SetZoomText(zoomSlider.value); });
            zoomSlider.onValueChanged.AddListener(delegate { solutionDetector.SetZoom(zoomSlider.value); });
            zoomSlider.onValueChanged.AddListener(delegate { emitterCone.SetZoom(zoomSlider.value); });
            zoomSlider.onValueChanged.AddListener(delegate { emitterConeSol.SetZoom(zoomSlider.value); });
            
            textManager.SetZoomTextReference(zoom.transform.GetChild(1).gameObject.GetComponent<Text>());

        }
        else
        {
            zoomLocked.GetComponent<Button>().onClick.AddListener(LevelLoader.LoadShop);
        }
        
        if (PowerUpsManger.LambdaUnlocked)
        {
            lambdaLocked.SetActive(false);
            lambda = Instantiate(lambda, canvas.transform);
            buttons.Add(lambda);
            
            lambda.onClick.RemoveAllListeners();
            lambda.onClick.AddListener(delegate { DisplaySlider(lambda); });
            
            Slider lambdaSlider = lambda.transform.GetChild(0).gameObject.GetComponent<Slider>();
            lambdaSlider.onValueChanged.RemoveAllListeners();
            lambdaSlider.onValueChanged.AddListener(delegate { detector.SetLambda(lambdaSlider.value); });
            lambdaSlider.onValueChanged.AddListener(delegate { textManager.SetLambdaText(lambdaSlider.value); });
            lambdaSlider.onValueChanged.AddListener(delegate { solutionDetector.SetLambda(lambdaSlider.value); });
            lambdaSlider.onValueChanged.AddListener(delegate { emitterCone.SetLambda(lambdaSlider.value); });
            lambdaSlider.onValueChanged.AddListener(delegate { emitterConeSol.SetLambda(lambdaSlider.value); });
            lambdaSlider.onValueChanged.AddListener(delegate { wave.SetLambda(lambdaSlider.value); });
            textManager.SetLambdaTextReference(lambda.transform.GetChild(1).gameObject.GetComponent<Text>());

        }
        else
        {
            lambdaLocked.GetComponent<Button>().onClick.AddListener(LevelLoader.LoadShop);
        }
        
        if (PowerUpsManger.PowerUnlocked)
        {
            powerLocked.SetActive(false);
            power = Instantiate(power, canvas.transform);
            buttons.Add(power);
            
            power.onClick.RemoveAllListeners();
            power.onClick.AddListener(delegate { DisplaySlider(power); });
            
            Slider powerSlider = power.transform.GetChild(0).gameObject.GetComponent<Slider>();
            powerSlider.onValueChanged.RemoveAllListeners();
            powerSlider.onValueChanged.AddListener(delegate { detector.SetPwr(powerSlider.value); });
            powerSlider.onValueChanged.AddListener(delegate { textManager.SetPowerText(powerSlider.value); });
            powerSlider.onValueChanged.AddListener(delegate { solutionDetector.SetPwr(powerSlider.value); });
            powerSlider.onValueChanged.AddListener(delegate { emitterCone.SetPwr(powerSlider.value); });
            powerSlider.onValueChanged.AddListener(delegate { emitterConeSol.SetPwr(powerSlider.value); });
            powerSlider.onValueChanged.AddListener(delegate { wave.SetPwr(powerSlider.value); });
            textManager.SetPowerTextReference(power.transform.GetChild(1).gameObject.GetComponent<Text>());

        }
        else
        {
            powerLocked.GetComponent<Button>().onClick.AddListener(LevelLoader.LoadShop);
        }
        
        if (PowerUpsManger.RotationUnlocked)
        {
            rotationLocked.SetActive(false);
            rotation = Instantiate(rotation, canvas.transform);
            buttons.Add(rotation);
            
            rotation.onClick.RemoveAllListeners();
            rotation.onClick.AddListener(delegate { DisplaySlider(rotation); });
            
            Slider rotationSlider = rotation.transform.GetChild(0).gameObject.GetComponent<Slider>();
            rotationSlider.onValueChanged.RemoveAllListeners();
            //rotationSlider.onValueChanged.AddListener(delegate {atomsManager.Rotate(1f);});
            //rotationSlider.onValueChanged.AddListener(delegate {solutionManager.Rotate(1f);});
            rotationSlider.onValueChanged.AddListener(delegate {atomsManager.Rotate(rotationSlider.value);  });
            rotationSlider.onValueChanged.AddListener(delegate {solutionManager.Rotate(rotationSlider.value);  });

            rotationSlider.onValueChanged.AddListener(delegate {textManager.SetRotationText(rotationSlider.value);});
            rotationSlider.onValueChanged.AddListener(delegate {rotationSlider.GetComponent<RotationSlider>().ChangeHandleColor45();});
            textManager.SetRotationTextReference(rotation.transform.GetChild(1).gameObject.GetComponent<Text>());

        }
        else
        {
            rotationLocked.GetComponent<Button>().onClick.AddListener(LevelLoader.LoadShop);
        }
    }
        
    public void DisplaySlider(Button powerUp)
    {
        powerUp.transform.GetChild(0).gameObject.SetActive(true);
        if (mapDisplayed && powerUp != rotation)
        {
            StartCoroutine(CloseMinimap());
            mapDisplayed = false;
        }


        const float textPosXMax = 453;
        const float textPosXMin = 160;
        
        Vector3 textPos = powerUp.transform.GetChild(1).localPosition;
        textPos = new Vector3(textPosXMax, textPos.y, textPos.z);
        powerUp.transform.GetChild(1).localPosition = textPos;
        foreach (var b in buttons)
        {
            if (b == powerUp) continue;
            b.transform.GetChild(0).gameObject.SetActive(false);
            textPos = b.transform.GetChild(1).localPosition;
            textPos = new Vector3(textPosXMin, textPos.y, textPos.z);
            b.transform.GetChild(1).localPosition = textPos;
        }
    }

    private void DisplayMap()
    {
        Debug.Log("map");
        
        if (mapDisplayed)
        {
            StartCoroutine(CloseMinimap());
            mapDisplayed = false;
        }
        else
        {
            foreach (var b in buttons)
            {
                if (b == rotation) continue;
                if (!b.transform.GetChild(0).gameObject.activeSelf) continue;
                b.transform.GetChild(0).gameObject.SetActive(false);
                Vector3 textPos = b.transform.GetChild(1).localPosition;
                textPos = new Vector3(160, textPos.y, textPos.z);
                b.transform.GetChild(1).localPosition = textPos;

            }
            StartCoroutine(OpenMinimap());
            mapDisplayed = true;
        }
    }

    IEnumerator OpenMinimap()
    {
        float t = 0;
        float ease;
        float newPosY;
        Vector3 minimapPos = minimapButton.transform.localPosition;
        float posY = minimapPos.y;
        while (t <= 10)
        {

            ease = EaseOutQuartic(t / 10);
            newPosY = Mathf.Lerp(posY, 240, ease);
            
            minimapButton.transform.localPosition = new Vector3(minimapPos.x, newPosY, minimapPos.z);
            t += 1;
            yield return new WaitForFixedUpdate();

        }
    }

    IEnumerator CloseMinimap()
    {
        float t = 0;
        float ease;
        float newPosY;
        Vector3 minimapPos = minimapButton.transform.localPosition;

        float posY = minimapPos.y;
        while (t <= 10)
        {
            ease = EaseInQuartic(t / 10);
            newPosY = Mathf.Lerp(posY, 518, ease);
            minimapButton.transform.localPosition = new Vector3(minimapPos.x, newPosY, minimapPos.z);
            t += 1;
            yield return new WaitForFixedUpdate();

        }
    }
    
    private float EaseOutQuartic(float x)
    {
        return 1 - Mathf.Pow(1 - x, 4);
    }
    
    private float EaseInQuartic(float x)
    {
        return x * x * x * x;
    }

    
   
}
