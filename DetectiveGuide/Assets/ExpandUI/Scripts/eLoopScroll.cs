using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class eLoopScroll : eElement
{
    #region Custom Events
    [System.Serializable] public class TargetItemChanged : UnityEvent { }
    [System.Serializable] public class QueueItemChanged : UnityEvent<int, bool, GameObject> { }
    [System.Serializable] public class SelChanged : UnityEvent<int, bool, GameObject> { }
    [System.Serializable] public class LongPressed : UnityEvent<int, bool, GameObject> { }
    [System.Serializable] public class Started : UnityEvent { }
    [System.Serializable] public class Reached : UnityEvent { }
    #endregion

    #region Basic Element
    private class ColumnCollection : List<RectTransform>
    {
        public RectTransform Insert(RectTransform inIteam) { Add(inIteam); return inIteam; }
    }

    private class Row : IDisposable
    {
        public int QueueIndex { get; private set; }
        public ColumnCollection Column = new ColumnCollection();

        public RectTransform this[int index] { get { return Column[index]; } }
        public void Dispose() { Column.Clear(); }
    }

    private class RowCollection : List<Row>
    {
        public new void Clear()
        {
            for (int i = 0; i < Count; i++)
                this[i].Dispose();
            base.Clear();
        }

        public Row Insert()
        {
            Row row = new Row();
            Add(row);
            return row;
        }
    }
    #endregion

    #region Enum Definition
    // 방향 설정 : 가로 / 세로
    private enum eOrientation { Vettical, Horizontal }

    // 선택 모드 : 선택 불가 / 하나만 선택 / 여러개 선택
    private enum eSelelctionMode { Block, Single, Multiple }

    // 마지막에 도달했을 때 처리
    private enum eLoopMode { Default, Continue, Return }

    // 선택할 때 옵션 
    public enum eSelelctOption { Block, Select, Deselect }
    #endregion

    #region Properties
    // 스크롤 방향을 나타낸다.
    [SerializeField] private eOrientation m_Orientation = eOrientation.Vettical;

    [SerializeField] private eSelelctionMode m_Selection = eSelelctionMode.Block;

    [SerializeField] private bool m_IsDeselectable = false;

    [SerializeField] private int m_MultipleSelect = 1;
    private List<int> m_SelectedItems = new List<int>();

    // 스크롤 안에 들어갈 아이템 프리팹 (RectTransform)
    [SerializeField] private RectTransform m_Item = null;

    // 자동으로 사이즈를 키울 것인지
    [SerializeField] private bool m_IsAutoScaling = false;

    // 끝에 도달했을 때 다시 처음으로 돌아가게 할 것인지
    [SerializeField] private bool m_IsLooping = false;

    // 풀 사이즈
    [SerializeField, Range(1, 256)]
    private int m_PoolCapacity = 16;

    public int PoolCapacity { get { return m_PoolCapacity; } }

    // 아이템 개수
    [SerializeField, Range(1, 256)]
    private int m_ItemCount = 1;

    public int ItemCount { get { return m_ItemCount; } }

    // 열 개수
    [Range(1, 128)] public int m_ColumnCount = 1;

    // 행 개수
    [SerializeField, Range(1, 256)] private int m_RowCount = 4;
    public int RowCount 
    { 
        get
        {
            int value = ItemCount / m_ColumnCount;
            // 0이 아닐 경우 개수가 하나 더 있음.
            if (ItemCount % m_ColumnCount != 0)
                value++;
            return value;
        }
    }

    [SerializeField] private float m_HorizontalSpace = 0f;
    public float HorizontalSpace
    {
        set
        {
            m_HorizontalSpace = value;
            UpdateItemSize();
            //UpdateHorizontal();
        }
        get { return m_HorizontalSpace; }
    }

    [SerializeField] private float m_VerticalSpace = 0f;
    public float VerticalSpace
    {
        set
        {
            m_VerticalSpace = value;
            UpdateItemSize();
            //UpdateVertical();
        }
        get { return m_VerticalSpace; }
    }

    [SerializeField] private eText m_EmptyText = null;
    [SerializeField] private bool m_IsShowEmptyText = true;
    public string EmptyText 
    { 
        set 
        {
            m_EmptyText?.SetText(value); 
        } 
    }

    private RowCollection m_Rows = new RowCollection();

    private RectTransform m_RectTransform;
    private RectTransform RectTransform { 
        get 
        {
            if (m_RectTransform == null) 
                m_RectTransform = GetComponent<RectTransform>(); 
            return m_RectTransform; 
        } 
    }

    private RectTransform m_ContentRectTransform;
    private ScrollRect m_ContentScrollRect;
    private RectTransform ContentRectTransform
    {
        get
        {
            if(m_ContentRectTransform == null)
            {
                if (m_ContentScrollRect == null)
                    m_ContentScrollRect = GetComponent<ScrollRect>();
                m_ContentRectTransform = m_ContentScrollRect.content.GetComponent<RectTransform>();
            }
            return m_ContentRectTransform;
        }
    }

    private Vector2 ContentAnchoredPoistion { get { return ContentRectTransform.anchoredPosition; } }

    private Vector2 m_ItemSize = Vector2.zero;
    public Vector2 ItemSize
    {
        get
        {
            if (m_Item != null)
                UpdateItemSize();
            return m_ItemSize;
        }
    }

    [SerializeField] private int m_StartIndex = 0;
    [SerializeField] private bool m_bStartCenter = false;
    [SerializeField] private bool m_bStartAnimation = false;
    #endregion

    #region Events
    // Target Item이 변경되었을 때
    public TargetItemChanged OnTargetItemChanged;

    // 아이템이 변경되었을 때 (삭제, 추가 등)
    public QueueItemChanged OnQueueChanged;

    // 아이템을 선택했을 때
    public SelChanged OnSelChanged;

    // 길게 눌렀을 때
    public LongPressed OnLongPressed;
    
    // Start 함수에서 마지막에 호출될 때
    public Started OnStarted;
    public Reached OnReached;
    #endregion

    protected override void Awake()
    {
        base.Awake();

        if(m_ContentScrollRect == null) m_ContentScrollRect = GetComponent<ScrollRect>();
    }

    protected override void Start()
    {
        base.Start();

        if(m_Item == null)
        {
            DebugLog.Error("Item is empty");
            return;
        }

        WindowEvent.Instance.onScreenSizeChanged += OnResize;
        OnResize();

        m_ContentScrollRect = GetComponent<ScrollRect>();

        if(m_IsLooping)
        {
            m_ContentScrollRect.verticalScrollbar = null;
            m_ContentScrollRect.horizontalScrollbar = null;
        }

        m_ContentScrollRect.horizontal = (m_Orientation != eOrientation.Vettical);
        m_ContentScrollRect.vertical = (m_Orientation == eOrientation.Vettical);
        m_ContentScrollRect.movementType = (m_IsLooping ? ScrollRect.MovementType.Unrestricted : m_ContentScrollRect.movementType);
        m_ContentScrollRect.horizontalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
        m_ContentScrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
        //

        var sb = new StringBuilder();
        for(int i = 0; i < m_PoolCapacity; i++)
        {
            if (i % m_ColumnCount == 0)
                m_Rows.Insert();

            var newItem = GameObject.Instantiate(m_Item) as RectTransform;
            newItem.SetParent(m_ContentScrollRect.content.transform, false);
            newItem.anchorMin = Vector2.up;
            newItem.anchorMax = Vector2.up;
            newItem.pivot = Vector2.up;

            sb.Append(m_Item.name);
            sb.Append(i + 1);
            newItem.name = sb.ToString();

            sb.Clear();

            eEventElement uiEvent = newItem.GetComponent<eEventElement>();
            if ( uiEvent != null )
            {
                uiEvent.onClickEvent.AddListener(OnItemClicked);
                uiEvent.onLongPressed.AddListener(OnItemLongPressed);
            }
        }

        UpdateItemSize();
        UpdateContents();


        //
        if(OnStarted == null)
        {
            if (m_StartIndex > 0)
                MoveToItem(m_StartIndex, m_bStartCenter, m_bStartAnimation);
        }
        else
            OnStarted.Invoke();
    }

    protected override void OnDestroy()
    {
        if (WindowEvent.IsInstantiate)
            WindowEvent.Instance.onScreenSizeChanged -= OnResize;

        base.OnDestroy();
    }

    public void OnResize()
    {
        if (m_IsAutoScaling)
        {
            UpdateItemSize();

            m_RowCount = (int)(RectTransform.rect.width / ItemSize.x);
            m_PoolCapacity = (int)(RectTransform.rect.height / ItemSize.y + 3) * m_RowCount;
        }
    }

    // 아이템 하나의 사이즈
    private void UpdateItemSize()
    {
        if(m_Item != null)
            m_ItemSize = new Vector2(m_Item.sizeDelta.x + m_HorizontalSpace, m_Item.sizeDelta.y + m_VerticalSpace);
    }

    private void UpdateContents()
    {

    }

    private void MoveToItem(int inIndex, bool isCenter = false, bool useAnimation = false)
    {
        float targetPos = 0f, centerPos = 0f;

        if(m_Orientation == eOrientation.Horizontal)
        {
            targetPos = ItemSize.x * (inIndex / m_ColumnCount);

            if(isCenter)
                centerPos = (RectTransform.rect.size.x * 0.5f) - (m_ItemSize.x * 0.5f);
        }
        else if(m_Orientation == eOrientation.Vettical)
        {
            targetPos = ItemSize.y * (inIndex / m_ColumnCount);

            if (isCenter)
                centerPos = (RectTransform.rect.size.y * 0.5f) - (m_ItemSize.y * 0.5f);
        }
    }

    private GameObject GetItem(int inIndex)
    {
        int row = inIndex / m_ColumnCount;
        int column = inIndex % m_ColumnCount;

        for(int r = 0, endRow = m_Rows.Count; r < endRow; r++)
        {
            if (m_Rows[r].QueueIndex == inIndex)
                return m_Rows[r][column].gameObject;
        }

        return null;
    }

    private int GetDataIndex(RectTransform inRectTransform)
    {
        if(inRectTransform != null)
        {
            Row row = null;
            for(int r = 0, endRow = m_Rows.Count; r < endRow; r++)
            {
                row = m_Rows[r];
                for(int c = 0, endColumn = row.Column.Count; c < endColumn; c++)
                {
                    if (row[c] == inRectTransform)
                        return row.QueueIndex * m_ColumnCount + c;
                }
            }
        }

        return 0;
    }

    private int GetListIndex(RectTransform inRectTransform)
    {
        return 0;
    }

    public void SetItem(RectTransform inItem)
    {
        m_Item = inItem;

        OnTargetItemChanged?.Invoke();
    }

    public void SetItemCount(int inCount)
    {
        ReleaseAllSelectedItems();
    }

    public void SelectItem(int inIndex, eSelelctOption inOption = eSelelctOption.Block)
    {
        if (m_Rows.Count == 0)
        {
            Debug.Log("Row count is empty");
            return;
        }

        switch (inOption)
        {
            case eSelelctOption.Block:
                SelectItem_Impl(inIndex, GetItem(inIndex));
                break;

            case eSelelctOption.Select:
                {
                    if (m_SelectedItems.Contains(inIndex) == false)
                    {
                        m_SelectedItems.Add(inIndex);
                        OnSelChanged?.Invoke(inIndex, true, GetItem(inIndex));
                    }
                }
                break;

            case eSelelctOption.Deselect:
                {
                    if (m_SelectedItems.Contains(inIndex) == true)
                    {
                        m_SelectedItems.Remove(inIndex);
                        OnSelChanged?.Invoke(inIndex, false, GetItem(inIndex));
                    }
                }
                break;
        }
    }

    private void SelectItem_Impl(int inIndex, GameObject inGo)
    {
        if(inIndex != -1)
        {
            switch(m_Selection)
            {
                case eSelelctionMode.Block:
                    OnSelChanged?.Invoke(inIndex, true, inGo);
                    break;

                case eSelelctionMode.Single:
                    {
                        if(m_SelectedItems.Contains(inIndex) == true)
                        {
                            if (m_IsDeselectable)
                                ReleaseAllSelectedItems();
                        }
                        else
                        {
                            m_SelectedItems.Add(inIndex);
                            OnSelChanged?.Invoke(inIndex, true, inGo);
                        }
                    }
                    break;

                case eSelelctionMode.Multiple:
                    {
                        if(m_SelectedItems.Contains(inIndex) == true)
                        {
                            if(m_IsDeselectable)
                            {
                                m_SelectedItems.Remove(inIndex);
                                OnSelChanged?.Invoke(inIndex, false, inGo);
                            }
                        }
                        else
                        {
                            if(m_SelectedItems.Count < m_MultipleSelect)
                            {
                                m_SelectedItems.Add(inIndex);
                                OnSelChanged?.Invoke(inIndex, true, inGo);
                            }
                        }
                    }
                    break;
            }
        }
    }

    public bool IsSelected(int inIndex)
    {
        return (m_SelectedItems.Contains(inIndex));
    }

    public void ReleaseAllSelectedItems(bool bSendEvent = true)
    {
        if(bSendEvent)
        {
            for (int i = 0, end = m_SelectedItems.Count; i < end; i++)
                OnSelChanged?.Invoke(m_SelectedItems[i], false, GetItem(m_SelectedItems[i]));
        }
        m_SelectedItems.Clear();
    }

    private void OnItemClicked(PointerEventData inEventData)
    {
        if (inEventData.pointerEnter == null) return;

        RectTransform rt = inEventData.pointerEnter.GetComponent<RectTransform>();
        int index = GetDataIndex(rt);

        if (index != -1)
            SelectItem_Impl(index, rt.gameObject);

    }

    private void OnItemLongPressed(PointerEventData inEventData)
    {        
        if(inEventData.pointerEnter == null) return;

        RectTransform rt = inEventData.pointerEnter.GetComponent<RectTransform>();
        int index = GetDataIndex(rt);

        if (index != -1)
            OnLongPressed?.Invoke(index, false, rt.gameObject);
    }
}
