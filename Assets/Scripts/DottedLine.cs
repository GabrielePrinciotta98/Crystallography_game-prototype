using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DottedLine : MonoBehaviour
{
    private Vector3 originPos;
    private Quaternion originRot;

    public bool Vertical { get; set; }

    private Renderer _renderer;
    private Atom atom;
    private bool atomFlag;

    private float timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position;
        transform.rotation *= Quaternion.Euler(90, 0, 90);
        originRot = transform.rotation;
        _renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Vertical)
        {
            if (atomFlag)
            {
                this.transform.parent = atom.transform;
                transform.localPosition = Vector3.zero;
                timer = 0;
            }
            else
            {
                timer += Time.deltaTime;
            }

            Vector3 curPosition = transform.position;
            Quaternion curRotation = originRot;
            curPosition.x = originPos.x;
            transform.position = curPosition;
            transform.rotation = curRotation;

            if (!(timer >= 0.01f)) return;
            _renderer.enabled = false;
        }
        else
        {
            if (atomFlag)
            {
                this.transform.parent = atom.transform;
                transform.localPosition = new Vector3(0, -10, 0);
                timer = 0;
            }
            else
            {
                timer += Time.deltaTime;
            }

            Vector3 curPosition = transform.position;
            Quaternion curRotation = originRot;
            //curPosition.x = originPos.x;
            transform.position = curPosition;
            transform.rotation = curRotation;

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
