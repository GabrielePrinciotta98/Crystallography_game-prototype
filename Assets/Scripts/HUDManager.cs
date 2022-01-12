using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    private AudioManager audioManager;
    private GameObject canvas;
    private Detector detector;
    private SolutionDetector solutionDetector;
    private EmitterCone emitterCone;
    private EmitterConeSol emitterConeSol;
    private TripodPosition tripodPosition;
    private TripodPositionSol tripodPositionSol;
    private Wall wall;
    private WallSol wallSol;
    private AtomsManager atomsManager;
    private SolutionManager solutionManager;
    private MoleculeManager moleculeManager;
    private Wave wave;
    private HintArrow hintArrow;
    private Button hint;
    private Button hintForbidden;
    [SerializeReference] private GameObject zoom; 
    [SerializeReference] private GameObject lambda; 
    [SerializeReference] private GameObject power; 
    [SerializeReference] private GameObject rotation;
    [SerializeReference] private Button molecule;
    [SerializeReference] private Button moleculeDisabled;
    [SerializeReference] private Button moleculeForbidden;
    [SerializeReference] private Button swap;
    [SerializeReference] private Button crystal;
    [SerializeReference] private TextManager textManager;
    

    private static List<GameObject> buttonsWithSlider;

    private void Awake()
    {
        hint = GameObject.Find("HintButton").GetComponent<Button>();
        hintForbidden = GameObject.Find("HintButtonForbidden").GetComponent<Button>();
        canvas = GameObject.Find("Canvas");

        audioManager = FindObjectOfType<AudioManager>();
        detector = FindObjectOfType<Detector>();
        solutionDetector = FindObjectOfType<SolutionDetector>();
        emitterCone = FindObjectOfType<EmitterCone>();
        emitterConeSol = FindObjectOfType<EmitterConeSol>();
        tripodPosition = FindObjectOfType<TripodPosition>();
        tripodPositionSol = FindObjectOfType<TripodPositionSol>();
        wall = FindObjectOfType<Wall>();
        wallSol = FindObjectOfType<WallSol>();
        atomsManager = FindObjectOfType<AtomsManager>();
        solutionManager = FindObjectOfType<SolutionManager>();
        moleculeManager = FindObjectOfType<MoleculeManager>();
        wave = FindObjectOfType<Wave>();
        hintArrow = FindObjectOfType<HintArrow>();
    }

    void Start()
    {
        buttonsWithSlider = new List<GameObject>();
        textManager = Instantiate(textManager);

        if (atomsManager.isCrystal)
        {
            hint.gameObject.SetActive(false);
            hintForbidden.gameObject.SetActive(true);
            //TODO hintForbidden.addListener.DisplayMessage
        }
        else
        {
            hint.gameObject.SetActive(true);
            hintForbidden.gameObject.SetActive(false);
            hint.onClick.AddListener(delegate { hintArrow.Activate(); });
            
        }
        
        
        //INSTANZIO I BOTTONI E I RELATIVI LISTENER
        if (PowerUpsManger.ZoomUnlocked)
        {
            zoom = Instantiate(zoom, canvas.transform);
            buttonsWithSlider.Add(zoom);
            Button zoomButton = zoom.GetComponentInChildren<Button>();
            
            zoomButton.onClick.RemoveAllListeners();
            zoomButton.onClick.AddListener(delegate { DisplaySlider(zoom); });
            zoomButton.onClick.AddListener(delegate { audioManager.Play("MenuButtonSelection"); });
            
            
            Slider zoomSlider = zoom.transform.GetChild(0).gameObject.GetComponent<Slider>();
            zoomSlider.onValueChanged.RemoveAllListeners();
            zoomSlider.onValueChanged.AddListener(delegate { detector.SetZoom(zoomSlider.value); });
            zoomSlider.onValueChanged.AddListener(delegate { textManager.SetZoomText(zoomSlider.value); });
            zoomSlider.onValueChanged.AddListener(delegate { solutionDetector.SetZoom(zoomSlider.value); });
            zoomSlider.onValueChanged.AddListener(delegate { emitterCone.SetZoom(zoomSlider.value); });
            zoomSlider.onValueChanged.AddListener(delegate { emitterConeSol.SetZoom(zoomSlider.value); });
            zoomSlider.onValueChanged.AddListener(delegate { tripodPosition.SetZoom(zoomSlider.value); });
            zoomSlider.onValueChanged.AddListener(delegate { tripodPositionSol.SetZoom(zoomSlider.value); });
            zoomSlider.onValueChanged.AddListener(delegate { wall.SetZoom(zoomSlider.value); });
            zoomSlider.onValueChanged.AddListener(delegate { wallSol.SetZoom(zoomSlider.value); });
            
            
            textManager.SetZoomTextReference(zoom.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>());

        }

        if (PowerUpsManger.LambdaUnlocked)
        {
            lambda = Instantiate(lambda, canvas.transform);
            buttonsWithSlider.Add(lambda);
            Button lambdaButton = lambda.GetComponentInChildren<Button>();
            
            lambdaButton.onClick.RemoveAllListeners();
            lambdaButton.onClick.AddListener(delegate { DisplaySlider(lambda); });
            lambdaButton.onClick.AddListener(delegate { audioManager.Play("MenuButtonSelection"); });

            
            Slider lambdaSlider = lambda.transform.GetChild(0).gameObject.GetComponent<Slider>();
            lambdaSlider.onValueChanged.RemoveAllListeners();
            lambdaSlider.onValueChanged.AddListener(delegate { detector.SetLambda(lambdaSlider.value); });
            lambdaSlider.onValueChanged.AddListener(delegate { textManager.SetLambdaText(lambdaSlider.value); });
            lambdaSlider.onValueChanged.AddListener(delegate { solutionDetector.SetLambda(lambdaSlider.value); });
            lambdaSlider.onValueChanged.AddListener(delegate { emitterCone.SetLambda(lambdaSlider.value); });
            lambdaSlider.onValueChanged.AddListener(delegate { emitterConeSol.SetLambda(lambdaSlider.value); });
            lambdaSlider.onValueChanged.AddListener(delegate { wave.SetLambda(lambdaSlider.value); });
            textManager.SetLambdaTextReference(lambda.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>());

        }
       
        if (PowerUpsManger.PowerUnlocked)
        {
            power = Instantiate(power, canvas.transform);
            buttonsWithSlider.Add(power);
            Button powerButton = power.GetComponentInChildren<Button>();

            
            powerButton.onClick.RemoveAllListeners();
            powerButton.onClick.AddListener(delegate { DisplaySlider(power); });
            powerButton.onClick.AddListener(delegate { audioManager.Play("MenuButtonSelection"); });

            
            Slider powerSlider = power.transform.GetChild(0).gameObject.GetComponent<Slider>();
            powerSlider.onValueChanged.RemoveAllListeners();
            powerSlider.onValueChanged.AddListener(delegate { detector.SetPwr(powerSlider.value); });
            powerSlider.onValueChanged.AddListener(delegate { textManager.SetPowerText(powerSlider.value); });
            powerSlider.onValueChanged.AddListener(delegate { solutionDetector.SetPwr(powerSlider.value); });
            powerSlider.onValueChanged.AddListener(delegate { emitterCone.SetPwr(powerSlider.value); });
            powerSlider.onValueChanged.AddListener(delegate { emitterConeSol.SetPwr(powerSlider.value); });
            powerSlider.onValueChanged.AddListener(delegate { wave.SetPwr(powerSlider.value); });
            textManager.SetPowerTextReference(power.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>());

        }

        if (PowerUpsManger.RotationUnlocked)
        {
            rotation = Instantiate(rotation, canvas.transform);
            buttonsWithSlider.Add(rotation);
            Button rotationButton = rotation.GetComponentInChildren<Button>();

            
            rotationButton.onClick.RemoveAllListeners();
            rotationButton.onClick.AddListener(delegate { DisplaySlider(rotation); });
            rotationButton.onClick.AddListener(delegate { audioManager.Play("MenuButtonSelection"); });

            
            Slider rotationSlider = rotation.transform.GetChild(0).gameObject.GetComponent<Slider>();
            rotationSlider.onValueChanged.RemoveAllListeners();
            
            rotationSlider.onValueChanged.AddListener(delegate {atomsManager.Rotate(rotationSlider.value);  });
            rotationSlider.onValueChanged.AddListener(delegate {solutionManager.Rotate(rotationSlider.value);  });

            rotationSlider.onValueChanged.AddListener(delegate {textManager.SetRotationText(rotationSlider.value);});
            rotationSlider.onValueChanged.AddListener(delegate {rotationSlider.GetComponent<RotationSlider>().ChangeHandleColor45();});
            textManager.SetRotationTextReference(rotation.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>());

        }
        
        if (PowerUpsManger.SwapUnlocked)
        {
            swap = Instantiate(swap, canvas.transform);
            swap.onClick.RemoveAllListeners();
            swap.onClick.AddListener(delegate { audioManager.Play("MenuButtonSelection"); });
            
        }

        if (PowerUpsManger.MoleculeUnlocked)
        {
            molecule = Instantiate(molecule, canvas.transform);
            moleculeDisabled = Instantiate(moleculeDisabled, canvas.transform);
            moleculeForbidden = Instantiate(moleculeForbidden, canvas.transform);

            if (atomsManager.isCrystal)
            {
                moleculeForbidden.gameObject.SetActive(true);
                //TODO moleculeForbidden.addListener.DisplayMessage
                
                molecule.gameObject.SetActive(false);
                moleculeDisabled.gameObject.SetActive(false);
            }
            else
            {
                moleculeForbidden.gameObject.SetActive(false);
                
                molecule.onClick.RemoveAllListeners();
                molecule.onClick.AddListener(delegate
                {
                    moleculeManager.ActivateMolecule(molecule, moleculeDisabled);
                });
                molecule.onClick.AddListener(delegate { audioManager.Play("MenuButtonSelection"); });

                moleculeDisabled.onClick.RemoveAllListeners();
                moleculeDisabled.onClick.AddListener(delegate
                {
                    moleculeManager.DeactivateMolecule(molecule, moleculeDisabled);
                });
                moleculeDisabled.onClick.AddListener(delegate { audioManager.Play("MenuButtonSelection"); });
                moleculeDisabled.gameObject.SetActive(false);
            }

        }
        
        
        if (atomsManager.isCrystal)
        {
            crystal = Instantiate(crystal, canvas.transform);
            crystal.onClick.RemoveAllListeners();

            crystal.onClick.AddListener(delegate { atomsManager.TriggerWhistle(); });
        }
    }
    

    public void DisplaySlider(GameObject powerUp)
    {
        GameObject slider = powerUp.transform.GetChild(0).gameObject;
        slider.SetActive(true);
        
        
        const float textPosXMax = 453;
        const float textPosXMin = 160;
        
        Vector3 textPos = powerUp.transform.GetChild(1).transform.GetChild(0).localPosition;
        textPos = new Vector3(textPosXMax, textPos.y, textPos.z);
        powerUp.transform.GetChild(1).transform.GetChild(0).localPosition = textPos;
        foreach (var b in buttonsWithSlider)
        {
            if (b == powerUp) continue;
            b.transform.GetChild(0).gameObject.SetActive(false);
            textPos = b.transform.GetChild(1).transform.GetChild(0).localPosition;
            textPos = new Vector3(textPosXMin, textPos.y, textPos.z);
            b.transform.GetChild(1).transform.GetChild(0).localPosition = textPos;
        }
    }

    

    
    
   
}
