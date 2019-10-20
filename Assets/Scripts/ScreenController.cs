using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    public GameEvent onWindowOpen;
    public GameEvent onWindowClose;
    private Animator m_animator;

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetWindowState(bool windowState)
    {
        m_animator.SetBool("Open", windowState);
    }

    public void StartNotificationFade()
    {
        m_animator.SetTrigger("Fade");
    }

    public void OnUIWindowOpen()
    {
        if (onWindowOpen != null)
        {
            onWindowOpen.Raise(this);
        }
    }

    public void OnUIWindowClose()
    {
        if (onWindowClose != null)
        {
            onWindowClose.Raise(this);
        }
    }
}
