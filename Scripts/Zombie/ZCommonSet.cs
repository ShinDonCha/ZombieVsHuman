using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZombieAniClip
{
    public AnimationClip[] m_idleAni = null;
    public AnimationClip[] m_attackAni = null;
    public AnimationClip[] m_runAni = null;
    public AnimationClip[] m_dieAni = null;
}

[CreateAssetMenu(fileName = "ZCommon", menuName = "ScriptableObject/ZCommonSet", order = int.MaxValue)]
public class ZCommonSet : ScriptableObject
{
    public ZombieAniClip m_aniClip;              //����Ƽ���� ���� �ִϸ��̼� Ŭ�� �������� ����
    public GameObject m_bloodEff = null;        //���� �ǰ� �� �̹���
    public GameObject m_bloodDec = null;        //���� �ǰ� �� �ٴڿ� �������� ���� �̹���
    [HideInInspector] public float[] m_decPos = { -0.5f, 0.0f, 0.5f };
    public Texture[] m_decTex = null;

    [HideInInspector] public float m_maxHp = 100.0f;                     //������ �ִ� ü��

}
