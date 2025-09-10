using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SwordVisual : MonoBehaviour
{
    [SerializeField] private Sword sword;
    private Vector3 _defaultLocalPosition;
    private Vector3 _defaultLocalRotation;
    
    private static readonly int Attack1 = Animator.StringToHash(Attack);
    private const string Attack = "Attack";
    
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();    
        _defaultLocalPosition = transform.localPosition;
        _defaultLocalRotation = transform.localEulerAngles;
    }

    private void Start()
    {
        if (sword != null)
            sword.OnSwordSwing += Sword_OnSwordSwing;
    }

    private void Sword_OnSwordSwing(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(Attack1);
    }
    
    public void ResetAnimation()
    {
        _animator.Rebind();
        _animator.Update(0f);
        transform.localPosition = _defaultLocalPosition;
        transform.localEulerAngles = _defaultLocalRotation;
    }

    public void TriggerEndAttackAnimation()
    {
        sword.AttackColliderTurnOff();
    }
    
    private void OnDisable()
    {
        transform.localPosition = _defaultLocalPosition;
        transform.localEulerAngles = _defaultLocalRotation;
        sword.AttackColliderTurnOff();
    }
    
    private void OnDestroy()
    {
        
        sword.OnSwordSwing -= Sword_OnSwordSwing;
    }
}
