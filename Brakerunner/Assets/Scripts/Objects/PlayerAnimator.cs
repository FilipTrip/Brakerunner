using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private GameObject idle;
    [SerializeField] private GameObject run;
    [SerializeField] private GameObject jump;
    [SerializeField] private GameObject duck;
    [SerializeField] private GameObject shoot;
    [SerializeField] private GameObject fall;
    [SerializeField] private Animator runAnimator;
    
    private GameObject activeAnimation;
    private bool hideAll = false;

    public void Start()
    {
        activeAnimation = idle;
        idle.SetActive(true);

        run.SetActive(false);
        jump.SetActive(false);
        duck.SetActive(false);
        shoot.SetActive(false);
        fall.SetActive(false);
    }

    private void SetAnimation(GameObject animation)
    {
        if (hideAll)
            return;

        activeAnimation.SetActive(false);
        activeAnimation = animation;
        activeAnimation.SetActive(true);
    }

    public void UpdateRunSpeed(float movementSpeed)
    {
        runAnimator.speed = movementSpeed * 0.1f;
    }

    public void Idle()
    {
        SetAnimation(idle);
    }

    public void Run()
    {
        SetAnimation(run);
    }

    public void Jump()
    {
        SetAnimation(jump);
    }

    public void Duck()
    {
        SetAnimation(duck);
    }

    public void Shoot()
    {
        SetAnimation(shoot);
    }

    public void Fall()
    {
        SetAnimation(fall);
    }

    public void HideAll()
    {
        hideAll = true;
        activeAnimation = null;

        idle.SetActive(false);
        run.SetActive(false);
        jump.SetActive(false);
        duck.SetActive(false);
        shoot.SetActive(false);
        fall.SetActive(false);
    }

    public void StopJump()
    {
        if (activeAnimation == jump)
            Run();
    }

    public void StopDuck()
    {
        if (activeAnimation == duck)
            Run();
    }

    public void StopShoot()
    {
        if (activeAnimation == shoot)
            Run();
    }

}
