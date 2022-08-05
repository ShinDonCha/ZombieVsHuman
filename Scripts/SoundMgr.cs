using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundList
{
    Weapon,
    Change,
    Reload
}

public class SoundMgr : MonoBehaviour
{
    public static SoundMgr inst;
    [HideInInspector] public AudioSource m_audioSource = null;

    [Header("----- AudioClip -----")]
    //------ ��� ����
    public AudioClip[] m_weaponSound = null;
    public AudioClip m_changeSound = null;
    public AudioClip m_reloadSound = null;
    //------ ��� ����

    [Header("----- DefaultVolume -----")]
    //------ ���� �⺻��
    public float[] m_weaponDefault;
    public float m_changeDefault;
    public float m_reloadDefault;
    //------ ���� �⺻��

    [HideInInspector] public float m_curDefault = 0.0f;         //���� ����Ǵ� Ŭ���� Default ����

    private void Awake()
    {
        inst = this;
        m_audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        AudioChange(SoundList.Weapon);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AudioChange(SoundList selectSound)
    {
        switch(selectSound)
        {
            case SoundList.Weapon:
                m_audioSource.clip = m_weaponSound[(int)PlayerCtrl.inst.m_nowWeapon.m_itemInfo.m_itName];
                m_curDefault = m_weaponDefault[(int)PlayerCtrl.inst.m_nowWeapon.m_itemInfo.m_itName];
                break;
            case SoundList.Change:
                m_audioSource.clip = m_changeSound;
                m_curDefault = m_changeDefault;
                break;
            case SoundList.Reload:
                m_audioSource.clip = m_reloadSound;
                m_curDefault = m_reloadDefault;
                break;
        }

        m_audioSource.volume = m_curDefault * GlobalValue.g_cfEffValue;             //ȿ���� ����
    }

}
