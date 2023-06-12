using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour,IHasTrigger
{
    [SerializeField] private GameObject _wall_prefab;

    [SerializeField]
    private float x_scale;
    [SerializeField]
    private float y_scale;
    [SerializeField]
    private float width;
    //�_�̐�(�����قǊ��炩��)
    [SerializeField]
    private int _points = 64;
    public int Points
    {
        get => _points;
        set
        {
            _points = value;
            UpdateWalls();
        }
    }


    private void OnValidate()
    {
        //�ݒ�l���ύX���ꂽ��R���C�_�[�̌`���X�V
        UpdateWalls();
    }

    //=================================================================================
    //�X�V
    //=================================================================================

    /// <summary>
    /// ���݂̃X�P�[������R���C�_�[�̌`�X�V
    /// </summary>
    public void UpdateWalls()
    {

        var points = new Vector3[_points];
        var angle = 0f;
        var deltaAngle = 2f * Mathf.PI / _points;
        angle -= deltaAngle/2;

        for (var i = 0; i < _points; i++)
        {
            var x = Mathf.Cos(angle)*0.5f* x_scale;
            var z = Mathf.Sin(angle)*0.5f* y_scale;
            points[i] = new Vector3(x, transform.position.y, z);
            angle += deltaAngle;
        }

        for (var i = 0; i < _points; i++)
        {
            // ���݂̓_�̍��W�Ǝ��̓_�̍��W���擾���܂�
            var currentPoint = points[i];
            var nextPoint = points[(i + 1) % _points]; // �Ō�̓_����ŏ��̓_�ւ̐ڑ����������܂�

            // ���_�̍��W���v�Z���܂�
            var midPoint = (currentPoint + nextPoint)*0.5f;

            // �ǃI�u�W�F�N�g�̌����iQuaternion�j���v�Z���܂�
            var direction = nextPoint - currentPoint;
            var rotation = Quaternion.LookRotation(direction, Vector3.up);

            // �ǃI�u�W�F�N�g��Instantiate���܂�
            var wall = Instantiate(_wall_prefab, midPoint, rotation, transform);
            var distance = Vector3.Distance(currentPoint, nextPoint);
            wall.transform.Translate(new Vector3(_wall_prefab.transform.localScale.x / 2,0,0), Space.Self);
            // �ǃI�u�W�F�N�g��X�X�P�[����ݒ肵�܂�
            wall.transform.localScale = new Vector3(_wall_prefab.transform.localScale.x/transform.lossyScale.x, _wall_prefab.transform.lossyScale.y / transform.localScale.y, distance / transform.lossyScale.z);
        }
    }
    public void Triggerred(Transform transform)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }
}