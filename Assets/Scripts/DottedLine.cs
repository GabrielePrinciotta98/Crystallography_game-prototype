using UnityEngine;

public class DottedLine : MonoBehaviour
{
    private Vector3 originPos;
    private Quaternion originRot;

    public bool Vertical { get; set; }

    private Renderer _renderer;
    private Atom atom;
    private bool atomFlag;
    private Transform dottedLineTransform;
    private float timer;
    
    void Start()
    {
        dottedLineTransform = transform;
        originPos = dottedLineTransform.position;
        var rotation = dottedLineTransform.rotation;
        rotation *= Quaternion.Euler(90, 0, 90);
        transform.rotation = rotation;
        originRot = rotation; 
        _renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (!Vertical)
        {
            if (atomFlag)
            {
                dottedLineTransform.parent = atom.transform;
                dottedLineTransform.localPosition = Vector3.zero;
                timer = 0;
            }
            else
            { 
                timer += Time.deltaTime;
            }

            Vector3 curPosition = transform.position;
            Quaternion curRotation = originRot;
            curPosition.x = originPos.x;
            dottedLineTransform.position = curPosition;
            dottedLineTransform.rotation = curRotation;

            if (!(timer >= 0.01f)) return;
            _renderer.enabled = false;
        }
        else
        {
            if (atomFlag)
            {
                dottedLineTransform.parent = atom.transform;
                dottedLineTransform.localPosition = new Vector3(0, -10, 0);
                timer = 0;
            }
            else
            {
                timer += Time.deltaTime;
            }

            Vector3 curPosition = dottedLineTransform.position;
            Quaternion curRotation = originRot;
            dottedLineTransform.position = curPosition;
            dottedLineTransform.rotation = curRotation;

            if (!(timer >= 0.5f)) return;
            _renderer.enabled = false;
        }
    }

    public void SetAtom(Atom atom, bool flag)
    {
        this.atom = atom;
        atomFlag = flag;
    }
}
