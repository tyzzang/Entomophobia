using BNG;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(tmpEvent))]
public class tmpEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // tmpEvent 스크립트의 참조 가져오기
        tmpEvent tmpEventScript = (tmpEvent)target;

        // 기본 인스펙터 드로잉 (m_eventType을 포함)
        tmpEventScript.m_eventType = (EventType.TYPE)EditorGUILayout.EnumPopup("Event Type", tmpEventScript.m_eventType);
        tmpEventScript.m_audio = (AudioSource)EditorGUILayout.ObjectField("Audio Source", tmpEventScript.m_audio, typeof(AudioSource), true);
        SerializedProperty audioClip = serializedObject.FindProperty("m_audioClip");
        EditorGUILayout.PropertyField(audioClip, new GUIContent("Audio Clips"), true);
        tmpEventScript.m_wall = (GameObject)EditorGUILayout.ObjectField("Wall Object", tmpEventScript.m_wall, typeof(GameObject), true);
        tmpEventScript.m_flashLightScript = (FlashLight)EditorGUILayout.ObjectField("Flash Object", tmpEventScript.m_flashLightScript, typeof(FlashLight), true);

        // 선택된 EventType에 따라 조건부로 변수를 표시
        switch (tmpEventScript.m_eventType)
        {
            case EventType.TYPE.Ani:
                tmpEventScript.m_animator = (Animator)EditorGUILayout.ObjectField("Animator", tmpEventScript.m_animator, typeof(Animator), true);
                break;

            case EventType.TYPE.Particle:
                //tmpEventScript.m_smokeParticle = (ParticleSystem)EditorGUILayout.ObjectField("Smoke Particle", tmpEventScript.m_smokeParticle, typeof(ParticleSystem), true);
                tmpEventScript.m_tmpWall = (GameObject)EditorGUILayout.ObjectField("tmpWall Object", tmpEventScript.m_tmpWall, typeof(GameObject), true);
                //tmpEventScript.m_boxCollider = (BoxCollider)EditorGUILayout.ObjectField("Box Collider", tmpEventScript.m_boxCollider, typeof(BoxCollider), true);
                break;

            case EventType.TYPE.Move:
                tmpEventScript.m_cart = (Cinemachine.CinemachineDollyCart)EditorGUILayout.ObjectField("Dolly Cart", tmpEventScript.m_cart, typeof(Cinemachine.CinemachineDollyCart), true);
                break;

            case EventType.TYPE.Sound:
                tmpEventScript.m_audio = (AudioSource)EditorGUILayout.ObjectField("Audio Source", tmpEventScript.m_audio, typeof(AudioSource), true);

                break;

            case EventType.TYPE.CardTag:
                tmpEventScript.m_passwordNum = EditorGUILayout.IntField("Password Number", tmpEventScript.m_passwordNum);
                tmpEventScript.m_doorObj = (GameObject)EditorGUILayout.ObjectField("Door Object", tmpEventScript.m_doorObj, typeof(GameObject), true);
                tmpEventScript.m_audio = (AudioSource)EditorGUILayout.ObjectField("Audio Source", tmpEventScript.m_audio, typeof(AudioSource), true);

                break;

            case EventType.TYPE.SlowMotion:
                tmpEventScript.m_timeController = (TimeController)EditorGUILayout.ObjectField("Time Controller", tmpEventScript.m_timeController, typeof(TimeController), true);
                break;
            case EventType.TYPE.FirstPuzzle:
                tmpEventScript.m_doorObj = (GameObject)EditorGUILayout.ObjectField("Door Object", tmpEventScript.m_doorObj, typeof(GameObject), true);
                tmpEventScript.m_slidePuzzleObj = (GameObject)EditorGUILayout.ObjectField("Slide Puzzle Object", tmpEventScript.m_slidePuzzleObj, typeof(GameObject), true);
                break;
            case EventType.TYPE.LaserOn:
                tmpEventScript.m_handLaser = (LaserPointer)EditorGUILayout.ObjectField("Hand Laser", tmpEventScript.m_handLaser, typeof(LaserPointer), true);
                tmpEventScript.m_selecter = (LaserSelecter)EditorGUILayout.ObjectField("Laser Selecter", tmpEventScript.m_selecter, typeof(LaserSelecter), true);
                break;
            case EventType.TYPE.LaserOff:
                tmpEventScript.m_handLaser = (LaserPointer)EditorGUILayout.ObjectField("Hand Laser", tmpEventScript.m_handLaser, typeof(LaserPointer), true);
                tmpEventScript.m_selecter = (LaserSelecter)EditorGUILayout.ObjectField("Laser Selecter", tmpEventScript.m_selecter, typeof(LaserSelecter), true);
                break;
            case EventType.TYPE.CreatureCheck:
                tmpEventScript.m_doorObj = (GameObject)EditorGUILayout.ObjectField("Door Object", tmpEventScript.m_doorObj, typeof(GameObject), true);
                break;
            case EventType.TYPE.CoCoon:
                tmpEventScript.m_cocoonObj = (GameObject)EditorGUILayout.ObjectField("CoCoon Object", tmpEventScript.m_cocoonObj, typeof(GameObject), true);
                break;
            case EventType.TYPE.FlashLight:
                tmpEventScript.m_flashLight = (Grabbable)EditorGUILayout.ObjectField("FlashLight Object", tmpEventScript.m_flashLight, typeof(Grabbable), true);
                break;
            case EventType.TYPE.SubCreature:
                SerializedProperty subCreatureObj = serializedObject.FindProperty("m_subCreatureObj");
                EditorGUILayout.PropertyField(subCreatureObj, new GUIContent("SubCreature"), true);
                tmpEventScript.m_ease = (Ease)EditorGUILayout.EnumPopup("Ease", tmpEventScript.m_ease);
                tmpEventScript.m_jumpForce = (float)EditorGUILayout.FloatField("Jump Force", tmpEventScript.m_jumpForce);
                break;
            case EventType.TYPE.BasementShutDoor:
                tmpEventScript.m_doorObj = (GameObject)EditorGUILayout.ObjectField("Door Object", tmpEventScript.m_doorObj, typeof(GameObject), true);
                break;
            default:
                EditorGUILayout.HelpBox("Select a valid event type to configure additional properties.", MessageType.Info);
                break;
        }

        tmpEventScript.m_rayST = (Transform)EditorGUILayout.ObjectField("Ray Start", tmpEventScript.m_rayST, typeof(Transform), true);
        tmpEventScript.m_raySTforCard = (Transform)EditorGUILayout.ObjectField("Ray Start for Card", tmpEventScript.m_raySTforCard, typeof(Transform), true);
        tmpEventScript.m_length = EditorGUILayout.FloatField("Length", tmpEventScript.m_length);
        tmpEventScript.m_rayLength = EditorGUILayout.FloatField("Ray Length", tmpEventScript.m_rayLength);
        tmpEventScript.m_viewInFlag = EditorGUILayout.Toggle("View In Flag", tmpEventScript.m_viewInFlag);
        tmpEventScript.m_checkDistance = EditorGUILayout.Toggle("Check Distance", tmpEventScript.m_checkDistance);
        tmpEventScript.m_userViewFlag = EditorGUILayout.Toggle("User View Flag", tmpEventScript.m_userViewFlag);
        tmpEventScript.m_SEPlayFlag = EditorGUILayout.Toggle("SE Play Flag", tmpEventScript.m_SEPlayFlag);
        tmpEventScript.m_tagFlag = EditorGUILayout.Toggle("Tag Flag", tmpEventScript.m_tagFlag);
        tmpEventScript.m_moveFlag = EditorGUILayout.Toggle("Move Flag", tmpEventScript.m_moveFlag);
        tmpEventScript.m_particleFlag = EditorGUILayout.Toggle("Particle Flag", tmpEventScript.m_particleFlag);
        // 변경사항 저장
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
        serializedObject.ApplyModifiedProperties();
    }
}
