using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ElementalType
{
    None,
    Fire,
    Water,
    Wind,
    Thunder
}



public class ElemantalStates 
{
    /// <summary>
    /// ���� ���� �Ӽ�
    /// </summary>
    ElementalType currentElemantal = ElementalType.None;
    public ElementalType CurrentElemantal
    {

        get => currentElemantal;
        private set
        {
            currentElemantal = value;
        }
    }

    /// <summary>
    /// ���� ���� ����
    /// </summary>
    public int currentElemantalLevel = 0;

    /// <summary>
    /// ��� ���ҿ� ���� ����
    /// ElementalType�� �ε��� ������ ����
    /// </summary>
    public int[] elemantalevels = new int[5] {0,1,1,1,1};

    /// <summary>
    /// ���Ұ� ����� �� ����Ǵ� ��������Ʈ
    /// </summary>
    public System.Action<ElementalType> onElemantaltypeChange;

    /// <summary>
    /// ���� ���Ҹ� �ٲܶ� �Ҹ��� �Լ�
    /// </summary>
    /// <param name="type">�ٲ� ����</param>
    public void ChangeType(ElementalType type)
    {
        if (type != CurrentElemantal)       // ���� ���ҿ� ������ ���� �ٲ��� �ʴ´�
        {
            if (type != 0 && elemantalevels[(int)type] == 0) return;        // ���Ӽ��� �ƴѵ� ������ 0�� ���Ҹ� �ҷ������� �� �� �ƹ��͵� ���� �ʴ´�.
            CurrentElemantal = type;
            currentElemantalLevel = elemantalevels[(int)type];
            onElemantaltypeChange?.Invoke(CurrentElemantal);
        }
    }

    /// <summary>
    /// Ư�� ������ ������ �ø��� �Լ�
    /// </summary>
    /// <param name="type">���� �ø� ����</param>
    public void LevelUpElemental(ElementalType type)
    {
        elemantalevels[(int)type]++;
    }
}
