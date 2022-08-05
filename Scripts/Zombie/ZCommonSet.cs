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
    public ZombieAniClip m_aniClip;              //유니티에서 좀비 애니메이션 클립 지정해줄 변수
    public GameObject m_bloodEff = null;        //좀비 피격 시 이미지
    public GameObject m_bloodDec = null;        //좀비 피격 시 바닥에 떨어지는 혈흔 이미지
    [HideInInspector] public float[] m_decPos = { -0.5f, 0.0f, 0.5f };
    public Texture[] m_decTex = null;

    [HideInInspector] public float m_maxHp = 100.0f;                     //좀비의 최대 체력

}
