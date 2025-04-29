using System.Collections.Generic;
using QuickJam.Core;
using Unity.VisualScripting;
using UnityEngine;

namespace QuickJam.UI
{
    public enum UILayer
    {
        Background,
        Screen,
        Popup,
        Overlay
    }
    
    public class UIManager : MonoSingleton<UIManager>
    {
        protected override bool ShouldDontDestroyOnLoad => true;

        [SerializeField] private Canvas rootCanvas;
        private readonly Dictionary<UILayer, Transform> _layerParents = new();
        
        private readonly Dictionary<UILayer, List<UIBase>> _activeUIs = new();

        
        protected override void Awake()
        {
            base.Awake();
            SetupLayers();
            
        }

        private void SetupLayers()
        {
            foreach (UILayer layer in System.Enum.GetValues(typeof(UILayer)))
            {
                GameObject layerObj = new GameObject(layer.ToString());
                layerObj.transform.SetParent(rootCanvas.transform, false);

                var rect = layerObj.AddComponent<RectTransform>();
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.one;
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;

                _layerParents[layer] = layerObj.transform;
                _activeUIs[layer] = new List<UIBase>();
            }
            
            _layerParents[UILayer.Background].SetSiblingIndex(0);
            _layerParents[UILayer.Screen].SetSiblingIndex(1);
            _layerParents[UILayer.Popup].SetSiblingIndex(2);
            _layerParents[UILayer.Overlay].SetSiblingIndex(3);
        }

        /// <summary>
        /// Create a UI from prefab
        /// </summary>
        public T OpenUI<T>(T uiPrefab, UILayer layer) where T : UIBase
        {
            if (!_layerParents.TryGetValue(layer, out var parent))
            {
                Debug.LogError($"[UIManager] No layer parent found for {layer}");
                return null;
            }

            T uiInstance = Instantiate(uiPrefab, parent);
            uiInstance.OnOpen();
            
            _activeUIs[layer].Add(uiInstance);
            
            return uiInstance;
        }

        /// <summary>
        /// Close a specific UI instance
        /// </summary>
        public void CloseUI(UIBase ui)
        {
            if (ui == null) return;

            foreach (var layer in _activeUIs.Keys)
            {
                if (_activeUIs[layer].Contains(ui))
                    _activeUIs[layer].Remove(ui);
            }

            ui.OnClose();
        }

        /// <summary>
        /// Close all UI on a specific layer
        /// </summary>
        public void CloseAllUIOnLayer(UILayer layer)
        {
            if (!_activeUIs.TryGetValue(layer, out var activeUI))
                return;

            foreach (var ui in activeUI)
            {
                if (ui != null)
                {
                    ui.OnClose();
                }
            }

            _activeUIs[layer].Clear();
        }

        /// <summary>
        /// Close all active UI on all layers
        /// </summary>
        public void CloseAllUI()
        {
            foreach (var layer in _activeUIs.Keys)
            {
                CloseAllUIOnLayer(layer);
            }
        }

        /// <summary>
        /// Query if any UI is active on a specific layer
        /// </summary>
        public bool IsLayerActive(UILayer layer)
        {
            return _activeUIs.ContainsKey(layer) && _activeUIs[layer].Count > 0;
        }
    }
}