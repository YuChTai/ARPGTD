using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public abstract class CharacterBase : MonoBehaviour {

    protected CharacterController charCtrler;
    protected Animator anim;
    public float XBounds_CharacterController { get; private set; }
    public float YBounds_CharacterController { get; private set; }
    public Vector3 CenterBounds_CharacterController { get; private set; }

    protected virtual void Awake() {
        charCtrler = GetComponent<CharacterController>();
        anim = transform.Find("Model").GetComponent<Animator>();
    }

    protected virtual void Start() {
        XBounds_CharacterController = charCtrler.bounds.extents.x;
        XBounds_CharacterController = charCtrler.bounds.extents.y;
        CenterBounds_CharacterController = charCtrler.bounds.center;
    }

    protected virtual void Update() {
        
    }
}
