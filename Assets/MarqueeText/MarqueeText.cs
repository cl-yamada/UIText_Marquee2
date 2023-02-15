using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class MarqueeText : BaseMeshEffect
{
#region SerializeField
    [SerializeField, Header("間隔")]
    private float space;

    [SerializeField, Header("速度")]
    private float speed;

    [SerializeField, Header("タイムスケールの影響を受けたくない==true")]
    private bool useUnscaledDeltaTime = false;
#endregion

    private float offset;
    private List<UIVertex> uiVertexes = new List<UIVertex>();

    private Graphic cacheGraphic;    
    public Graphic CacheGraphic
    {
        get
        {
            if (cacheGraphic == null)
            {
                if (!TryGetComponent<Graphic>(out cacheGraphic))
                {
                    Debug.LogError("$[MarqueeText.cs]class GraphicのGetComponentに失敗しています");
                    return null;
                }
            }

            return cacheGraphic;
        }
    }

    private Vector2 getPivot
    {
        get
        {
            return (transform as RectTransform).pivot;
        }
    }

    private Rect getRect
    {
        get
        {
            return (transform as RectTransform).rect;
        }
    }

    protected override void Awake()
    {
        InitializeTextSetting();
    }

    /// <summary>
    /// Unityのフローで勝手に走る
    /// </summary>
    private void Update()
    {
        // テキスト移動
        float DeltaTime() { return useUnscaledDeltaTime ? Time.unscaledDeltaTime :Time.deltaTime; }
        offset += DeltaTime() * speed;

        // 頂点を更新
        CacheGraphic.SetVerticesDirty();
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        InitializeTextSetting();

        // 頂点を更新
        CacheGraphic.SetVerticesDirty();
    }
#endif

    /// <summary>
    /// Textコンポーネントの初期化処理
    /// </summary>
    private void InitializeTextSetting()
    {
        var getText = GetComponent<Text>();
        if (getText != null)
        {
            getText.horizontalOverflow = HorizontalWrapMode.Overflow;
        }
    }

#region interface
    // 1文字(1メッシュ)の頂点数
    private const int CHAR_VERTEX_COUNT = 6;
    public override void ModifyMesh(VertexHelper _vHelper)
    {
        uiVertexes.Clear();
        _vHelper.GetUIVertexStream(uiVertexes);
        var count = uiVertexes.Count;
        if (count >= CHAR_VERTEX_COUNT)
        {
            // テキスト全体の幅・文字数
            var textWidth = uiVertexes[count - 3].position.x - uiVertexes[0].position.x;
            var textCount = uiVertexes.Count / CHAR_VERTEX_COUNT;
            // １文字の幅
            var charWidth = textWidth / textCount;

            // ビューから溢れている時だけ以下の処理をする
            //if (textWidth - charWidth + space > getRect.width)
            {
                var moveValue = offset % (textWidth + space);
                var leftValue = Mathf.Lerp(0, -getRect.width, getPivot.x);
                for (var vertexCount = 0; vertexCount < count; vertexCount += CHAR_VERTEX_COUNT)
                {
                    // ビュー内から文字が溢れたチェック
                    var checkVert = uiVertexes[vertexCount + 3];
                    checkVert.position.x -= moveValue;
                    var outView = checkVert.position.x < leftValue;
                    if (outView)
                        checkVert.position.x += textWidth + space;

                    uiVertexes[vertexCount + 3] = checkVert;

                    // 各文字ポリゴンの移動
                    foreach(var index in new []{0, 1, 2, 4, 5})
                    {
                        var vert = uiVertexes[vertexCount + index];
                        vert.position.x -= moveValue;
                        if(outView)
                            vert.position.x += textWidth + space;
                        uiVertexes[vertexCount + index] = vert;
                    }
                }
            }
        }

        _vHelper.Clear();
        _vHelper.AddUIVertexTriangleStream(uiVertexes);
    }
#endregion
}
