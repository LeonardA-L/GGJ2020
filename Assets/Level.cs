﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Level")]
public class Level : ScriptableObject
{
    [System.Serializable]
    public class Entry
    {
        [SerializeField]
        private TestObject m_itemPrefab = null;
        [SerializeField]
        private int m_amount = 1;

        public TestObject ItemPrefab => m_itemPrefab;
        public int Amount => m_amount;
    }

    [SerializeField]
    private List<Entry> m_libraryContent = null;
    [SerializeField]
    private float m_distanceToWin = 50;
    public float DistanceGoal => m_distanceToWin;

    public List<Entry> Library => m_libraryContent;

    public bool m_forceUseAll = false;
}
