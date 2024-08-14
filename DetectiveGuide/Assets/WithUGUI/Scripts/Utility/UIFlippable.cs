/// Credit ChoMPHi
/// Sourced from - https://discussions.unity.com/t/script-flippable-for-ui-graphics/563860/3
/// Modified 
using System.Collections.Generic;

namespace UnityEngine.UI.Extensions
{
    [RequireComponent(typeof(RectTransform)), RequireComponent(typeof(Graphic)), DisallowMultipleComponent]
    [AddComponentMenu("UI/UIFlippable")]
    public class UIFlippable : MonoBehaviour, IMeshModifier
    {
        [SerializeField] private bool m_Horizontal = false;
        [SerializeField] private bool m_Vertical = false;
        [SerializeField] private bool m_Angle90 = false;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="UnityEngine.UI.UIFlippable"/> should be flipped horizontally.
        /// </summary>
        /// <value><c>true</c> if horizontal; otherwise, <c>false</c>.</value>
        public bool Horizontal { 
            get { return this.m_Horizontal; } 
            set 
            { 
                this.m_Horizontal = value; 
                this.GetComponent<Graphic>().SetVerticesDirty(); 
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="UnityEngine.UI.UIFlippable"/> should be flipped vertically.
        /// </summary>
        /// <value><c>true</c> if vertical; otherwise, <c>false</c>.</value>
        public bool Vertical
        {
            get { return this.m_Vertical; }
            set 
            { 
                this.m_Vertical = value; 
                this.GetComponent<Graphic>().SetVerticesDirty(); 
            }
        }

        //
        public bool Angle90
        {
            get { return this.m_Angle90; }
            set
            {
                this.Angle90 = value;
                this.GetComponent<Graphic>().SetVerticesDirty();
            }
        }

        private void Awake()
        {
            OnValidate();
        }

        protected void OnValidate()
        {
            this.GetComponent<Graphic>().SetVerticesDirty();
        }

        public void ModifyVertices(List<UIVertex> inVerts)
        {
            RectTransform rt = this.transform as RectTransform;

            for (int i = 0, end = inVerts.Count; i < end; i++)
            {
                UIVertex v = inVerts[i];

                // Modify Positions
                v.position = new Vector3(
                    (this.m_Horizontal ? (v.position.x + (rt.rect.center.x - v.position.x) * 2) : v.position.x),
                    (this.m_Vertical ? (v.position.y + (rt.rect.center.y - v.position.y) * 2) : v.position.y),
                    v.position.z
                    );

                // Apply
                inVerts[i] = v;
            }
        }

        public void ModifyVertices(List<UIVertex> inVerts, bool isVert)
        {
            RectTransform rt = this.transform as RectTransform;

            for(int i = 0; i < inVerts.Count; i++)
            {
                UIVertex v = inVerts[i];
                //UIVertex v = new UIVertex();
                //inVerts.PopulateUIVertex(ref v, i);

                var x = (this.m_Horizontal ? (v.position.x + (rt.rect.center.x - v.position.x) * 2) : v.position.x);
                var y = (this.m_Vertical ? (v.position.y + (rt.rect.center.y - v.position.y) * 2) : v.position.y);

                if(isVert)
                {
                    if(Angle90)
                    {
                        if (i == 0)
                        {
                            if (this.m_Vertical)
                                y -= rt.rect.height;
                            else
                                y += rt.rect.height;
                        }
                        else if (i == 1)
                        {
                            if (this.m_Horizontal)
                                x -= rt.rect.width;
                            else
                                x += rt.rect.width;
                        }
                        else if (i == 2)
                        {
                            if (this.m_Vertical)
                                y += rt.rect.height;
                            else
                                y -= rt.rect.height;
                        }
                        else if (i == 3)
                        {
                            if (this.m_Horizontal)
                                x += rt.rect.width;
                            else
                                x -= rt.rect.width;
                        }
                    }
                }

                v.position = new Vector3(x, y, v.position.z);
                inVerts[i] = v;
                //inVerts.SetUIVertex(v, i);
            }
        }

        public void ModifyMesh(Mesh inMesh) { }

        public void ModifyMesh(VertexHelper inVerts)
        {
            List<UIVertex> buffer = new List<UIVertex>();
            inVerts.GetUIVertexStream(buffer);
            ModifyVertices(buffer, inVerts.currentVertCount == 4);
            inVerts.AddUIVertexTriangleStream(buffer);
        }
    }
}
