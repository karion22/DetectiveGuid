using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class eLoopScroll : eElement
{
    [System.Serializable] public class ItemChanged : UnityEvent<int, bool, GameObject> { }
    [System.Serializable] public class SelChanged : UnityEvent<int, bool, GameObject> { }
    [System.Serializable] public class LongPressed : UnityEvent<int, bool, GameObject> { }
    [System.Serializable] public class Started : UnityEvent { }
    [System.Serializable] public class Reached : UnityEvent { }

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

    // 방향 설정 : 가로 / 세로
    private enum eOrientation { Vettical, Horizontal }

    // 선택 모드 : 선택 불가 / 하나만 선택 / 여러개 선택
    private enum eSelelctionMode { Block, Single, Multiple }

    // 마지막에 도달했을 때 처리
    private enum eLoopMode { Default, Continue, Return }

    // 스크롤 방향을 나타낸다.
    [SerializeField] private eOrientation m_Orientation = eOrientation.Vettical;

    [SerializeField] private eSelelctionMode m_Selection = eSelelctionMode.Block;

    [SerializeField] private bool m_IsDeselectable = false;

    [SerializeField] private int m_MultipleSelect = 1;
    private List<int> m_SelelctedItems = new List<int>();

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

    [SerializeField] private Text m_EmptyText = null;
    [SerializeField] private bool m_IsShowEmptyText = true;
    public string EmptyText 
    { 
        set 
        { 
            if (m_EmptyText != null) 
                m_EmptyText.text = value; 
        } 
    }

    public ItemChanged OnItemChanged;
    public SelChanged OnSelChanged;
    public LongPressed OnLongPressed;
    public Started OnStarted;
    public Reached OnReached;

    private RowCollection m_Rows = new RowCollection();

    private RectTransform m_RectTransform;
    private RectTransform RectTransform { 
        get 
        {
            if (m_RectTransform == null) 
            { 
                m_RectTransform = GetComponent<RectTransform>(); 
            } 
            return m_RectTransform; 
        } 
    }

    private RectTransform m_ContentRectTransform;
    private ScrollRect m_ScrollRect;
    private RectTransform ContentRectTransform
    {
        get
        {
            if(m_ContentRectTransform == null)
            {
                if (m_ScrollRect == null)
                    m_ScrollRect = GetComponent<ScrollRect>();
                m_ContentRectTransform = m_ScrollRect.content.GetComponent<RectTransform>();
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

    protected override void Awake()
    {
        base.Awake();

        if(m_ScrollRect == null) m_ScrollRect= GetComponent<ScrollRect>();


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

        m_ScrollRect = GetComponent<ScrollRect>();

        if(m_IsLooping)
        {
            m_ScrollRect.verticalScrollbar = null;
            m_ScrollRect.horizontalScrollbar = null;
        }

        m_ScrollRect.horizontal = (m_Orientation != eOrientation.Vettical);
        m_ScrollRect.vertical = (m_Orientation == eOrientation.Vettical);
        m_ScrollRect.movementType = (m_IsLooping ? ScrollRect.MovementType.Unrestricted : m_ScrollRect.movementType);
        m_ScrollRect.horizontalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
        m_ScrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
        //

        var sb = new StringBuilder();
        for(int i = 0; i < m_PoolCapacity; i++)
        {
            if (i % m_ColumnCount == 0)
                m_Rows.Insert();

            var newItem = GameObject.Instantiate(m_Item) as RectTransform;
            newItem.SetParent(m_ScrollRect.content.transform, false);
            newItem.anchorMin = Vector2.up;
            newItem.anchorMax = Vector2.up;
            newItem.pivot = Vector2.up;

            sb.Append(m_Item.name);
            sb.Append(i + 1);
            newItem.name = sb.ToString();

            sb.Clear();

            eElement uiEvent = newItem.GetComponent<eElement>();
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

    }

    private int GetRealIndex(RectTransform inItem)
    {

        return 0;
    }

    private void OnItemClicked(PointerEventData inEventData)
    {
        if (inEventData.pointerEnter == null) return;

        RectTransform rt = inEventData.pointerEnter.GetComponent<RectTransform>();
        int index = GetRealIndex(rt);

    }

    private void OnItemLongPressed(PointerEventData inEventData)
    {        
        //RectTransform rt = inEventData.pointerEnter
    }
}
