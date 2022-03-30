using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    private AudioManager audioManager;
    private DialogueBox dialogueBox;
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
    [SerializeReference] private Button crystalDisabled;
    [SerializeReference] private TextManager textManager;
    

    private static List<GameObject> buttonsWithSlider;
    private List<Button> allButtons = new List<Button>();

    private int clicksOnHint = 0;

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
        dialogueBox = FindObjectOfType<DialogueBox>();
    }

    void Start()
    {
        buttonsWithSlider = new List<GameObject>();
        textManager = Instantiate(textManager);

        if (atomsManager.isCrystal)
        {
            hint.gameObject.SetActive(false);
            hintForbidden.gameObject.SetActive(true);
            hintForbidden.onClick.AddListener(delegate { dialogueBox.StartFinalLevelInterference(); });
            hintForbidden.onClick.AddListener(delegate { audioManager.Play("MenuError"); });

        }
        else
        {
            hint.gameObject.SetActive(true);
            hintForbidden.gameObject.SetActive(false);
            if (PowerUpsManger.MoleculeUnlocked)
            {
                hint.onClick.AddListener(delegate { dialogueBox.StartAlertMoleculeModeImpossibleAfterHint(); });
                hint.onClick.AddListener(ActivateWithMoleculeUnlocked);
                
            }
            else
            {
                hint.onClick.AddListener(delegate { hintArrow.Activate(); });
                hint.onClick.AddListener(delegate { audioManager.Play("HintArrowActivation"); });
            }
            hint.onClick.AddListener(delegate { audioManager.Play("MenuButtonSelection"); });
            
        }
        
        allButtons.Add(hint);
        allButtons.Add(hintForbidden);
        //INSTANZIO I BOTTONI E I RELATIVI LISTENER
        if (PowerUpsManger.ZoomUnlocked)
        {
            //zoom = Instantiate(zoom, Position(), Quaternion.identity, canvas.transform);
            zoom = Instantiate(zoom, canvas.transform);

            buttonsWithSlider.Add(zoom);
            Button zoomButton = zoom.GetComponentInChildren<Button>();
            allButtons.Add(zoomButton);
            
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
            //lambda = Instantiate(lambda, Position(), Quaternion.identity, canvas.transform);
            buttonsWithSlider.Add(lambda);
            Button lambdaButton = lambda.GetComponentInChildren<Button>();
            allButtons.Add(lambdaButton);
            
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
            //power = Instantiate(power, Position(), Quaternion.identity, canvas.transform);
            buttonsWithSlider.Add(power);
            Button powerButton = power.GetComponentInChildren<Button>();
            allButtons.Add(powerButton);
            
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
            //rotation = Instantiate(rotation, Position(), Quaternion.identity, canvas.transform);
            buttonsWithSlider.Add(rotation);
            Button rotationButton = rotation.GetComponentInChildren<Button>();
            allButtons.Add(rotationButton);
            
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
            //swap = Instantiate(swap, Position(), Quaternion.identity, canvas.transform);
            allButtons.Add(swap);
            swap.onClick.RemoveAllListeners();
            swap.onClick.AddListener(delegate { audioManager.Play("MenuButtonSelection"); });
            
        }

        if (PowerUpsManger.MoleculeUnlocked)
        {
            //Vector3 moleculeButtonPos = Position();
            //molecule = Instantiate(molecule, moleculeButtonPos, Quaternion.identity, canvas.transform);
            molecule = Instantiate(molecule, canvas.transform);
            allButtons.Add(molecule);
            //moleculeDisabled = Instantiate(moleculeDisabled, moleculeButtonPos, Quaternion.identity, canvas.transform);
            moleculeDisabled = Instantiate(moleculeDisabled, canvas.transform);
            allButtons.Add(moleculeDisabled);
            //moleculeForbidden = Instantiate(moleculeForbidden, moleculeButtonPos, Quaternion.identity, canvas.transform);
            moleculeForbidden = Instantiate(moleculeForbidden, canvas.transform);
            allButtons.Add(moleculeForbidden);

            if (atomsManager.isCrystal)
            {
                moleculeForbidden.gameObject.SetActive(true);
                moleculeForbidden.onClick.AddListener(delegate { dialogueBox.StartFinalLevelInterference(); });
                moleculeForbidden.onClick.AddListener(delegate { audioManager.Play("MenuError"); });

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
                molecule.onClick.AddListener(ActivateWithMoleculeUnlocked);
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
            allButtons.Add(crystal);
            crystalDisabled = Instantiate(crystalDisabled, canvas.transform);
            allButtons.Add(crystalDisabled);
            crystal.onClick.RemoveAllListeners();

            crystal.onClick.AddListener(delegate { atomsManager.TriggerCrystalActivation(crystal, crystalDisabled); });
            
            crystalDisabled.onClick.RemoveAllListeners();
            crystalDisabled.onClick.AddListener(delegate { atomsManager.TriggerCrystalDeactivation(crystal, crystalDisabled); });
            crystalDisabled.gameObject.SetActive(false);
        }
        
        DisablePowerUps();
    }

    private void ActivateWithMoleculeUnlocked()
    {
        hint.onClick.AddListener(IncreaseClicksOnHintCounter);
        if (clicksOnHint < 0) return;
        hint.onClick.RemoveAllListeners();
        hint.onClick.AddListener(ForbidMoleculeMode);
        hint.onClick.AddListener(delegate { hintArrow.Activate(); });
        hint.onClick.AddListener(delegate { audioManager.Play("HintArrowActivation"); });
    }
    
    private void IncreaseClicksOnHintCounter()
    {
        clicksOnHint++;
    }

    private void ForbidMoleculeMode()
    {
        moleculeForbidden.gameObject.SetActive(true);
        moleculeForbidden.onClick.AddListener(delegate { dialogueBox.StartMoleculeModeImpossibleAfterHint(); });
        moleculeForbidden.onClick.AddListener(delegate { audioManager.Play("MenuError"); });

        molecule.gameObject.SetActive(false);
        moleculeDisabled.gameObject.SetActive(false);
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

    public void DisablePowerUps()
    {
        foreach (var button in allButtons)
        {
            button.interactable = false;
        }
        
        const float textPosXMin = 160;
        foreach (var b in buttonsWithSlider)
        {
            b.transform.GetChild(0).gameObject.SetActive(false);
            var textPos = b.transform.GetChild(1).transform.GetChild(0).localPosition;
            textPos = new Vector3(textPosXMin, textPos.y, textPos.z);
            b.transform.GetChild(1).transform.GetChild(0).localPosition = textPos;
        }
    }
    
    public void EnablePowerUps()
    {
        foreach (var button in allButtons)
        {
            if (button == hint && hintArrow.activated) continue;
            button.interactable = true;
        }
    }

    public void CheckEmittersOn()
    {
        if (emitterCone.GetPowerOn() && emitterConeSol.GetPowerOn())
        {
            EnablePowerUps();
        }
    }



}
