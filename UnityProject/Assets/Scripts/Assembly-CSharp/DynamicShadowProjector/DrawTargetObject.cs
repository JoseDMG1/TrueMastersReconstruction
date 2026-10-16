using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace DynamicShadowProjector
{
	[ExecuteAlways]
	[RequireComponent(typeof(ShadowTextureRenderer))]
	public class DrawTargetObject : MonoBehaviour
	{
		[Serializable]
		public struct ReplaceShader
		{
			public string renderType;

			public Shader shader;
		}

		public enum TextureAlignment
		{
			None = 0,
			TargetAxisX = 1,
			TargetAxisY = 2,
			TargetAxisZ = 3
		}

		public enum UpdateFunction
		{
			OnPreCull = 0,
			LateUpdate = 1,
			UpdateTransform = 2
		}

		private struct CommandBufferHash
		{
			private ulong hash;

			public void Init()
			{
			}

			public void AddRenderer(Renderer renderer)
			{
			}

			public bool Equals(CommandBufferHash rhs)
			{
				return false;
			}
		}

		[SerializeField]
		private Transform m_target;

		[SerializeField]
		private Transform m_targetDirection;

		[SerializeField]
		private LayerMask m_layerMask;

		[SerializeField]
		private TextureAlignment m_textureAlignment;

		[SerializeField]
		private UpdateFunction m_updateFunction;

		[SerializeField]
		private Material m_shadowShader;

		[SerializeField]
		private ReplaceShader[] m_replacementShaders;

		[SerializeField]
		private bool m_renderChildren;

		[SerializeField]
		private bool m_autoDetectHierarchyChanges;

		[SerializeField]
		private bool m_followTarget;

		private CommandBufferHash m_commandBufferHash;

		private bool m_isCommandBufferDirty;

		private CommandBuffer m_commandBuffer;

		private ShadowTextureRenderer m_shadowRenderer;

		private Vector3 m_localTargetPosition;

		private Dictionary<Material, Material> m_replacedMaterialCache;

		public Transform target
		{ get { return m_target; } set { m_target=value; CacheOffset(); SetCommandBufferDirty(); } }

		public Transform targetDirection
		{ get { return m_targetDirection; } set { m_targetDirection=value; SetCommandBufferDirty(); } }

		public bool renderChildren
		{ get { return m_renderChildren; } set { m_renderChildren=value; SetCommandBufferDirty(); } }

		public bool autoDetectHierarchyChanges
		{ get { return m_autoDetectHierarchyChanges; } set { m_autoDetectHierarchyChanges=value; SetCommandBufferDirty(); } }

		public LayerMask layerMask
		{ get { return m_layerMask; } set { m_layerMask=value; SetCommandBufferDirty(); } }

		public TextureAlignment textureAlignment
		{ get { return m_textureAlignment; } set { m_textureAlignment=value; SetCommandBufferDirty(); } }

		public UpdateFunction updateFunction
		{ get { return m_updateFunction; } set { m_updateFunction=value; SetCommandBufferDirty(); } }

		public bool followTarget
		{ get { return m_followTarget; } set { m_followTarget=value; SetCommandBufferDirty(); } }

		public Material shadowShader
		{ get { return m_shadowShader; } set { m_shadowShader=value; SetCommandBufferDirty(); } }

		public ReplaceShader[] replacementShaders
		{ get { return m_replacementShaders; } set { m_replacementShaders=value; SetCommandBufferDirty(); } }

		internal CommandBuffer commandBuffer => m_commandBuffer;
        private void CacheOffset() { if(m_target)m_localTargetPosition=transform.InverseTransformPoint(m_target.position); }

		public void SetCommandBufferDirty()
		{
            m_isCommandBufferDirty=true;
		}

		public void UpdateCommandBuffer()
		{
            if(!m_target || !m_shadowShader)return;
            CreateCommandBuffer(); if(m_commandBuffer==null)return;
            m_commandBuffer.Clear(); m_commandBuffer.ClearRenderTarget(true,true,Color.white);
            var renderers=m_renderChildren ? m_target.GetComponentsInChildren<Renderer>() : m_target.GetComponents<Renderer>();
            foreach(var renderer in renderers) {
                if(!renderer.enabled || !renderer.gameObject.activeInHierarchy || ((1<<renderer.gameObject.layer)&m_layerMask.value)==0)continue;
                if(!(renderer is MeshRenderer) && !(renderer is SkinnedMeshRenderer))continue;
                var materials=renderer.sharedMaterials;
                var mesh=renderer is SkinnedMeshRenderer ? ((SkinnedMeshRenderer)renderer).sharedMesh : renderer.GetComponent<MeshFilter>()?.sharedMesh;
                if(!mesh)continue;
                for(int i=0;i<Mathf.Min(materials.Length,mesh.subMeshCount);i++) {
                    var source=materials[i];if(!source)continue;
                    Shader shader=m_shadowShader.shader;string renderType=source.GetTag("RenderType",false,"Opaque");
                    if(m_replacementShaders!=null)foreach(var replacement in m_replacementShaders)if(replacement.renderType==renderType && replacement.shader){shader=replacement.shader;break;}
                    if(!m_replacedMaterialCache.TryGetValue(source,out var mask)) {mask=new Material(shader) {hideFlags=HideFlags.HideAndDontSave};m_replacedMaterialCache.Add(source,mask);}
                    if(mask.shader!=shader)mask.shader=shader;
                    mask.SetColor("_Color",m_shadowRenderer.shadowColor);
                    if(source.HasProperty("_MainTex")) {mask.SetTexture("_MainTex",source.GetTexture("_MainTex"));mask.SetTextureScale("_MainTex",source.GetTextureScale("_MainTex"));mask.SetTextureOffset("_MainTex",source.GetTextureOffset("_MainTex"));}
                    else if(source.HasProperty("_BaseMap"))mask.SetTexture("_MainTex",source.GetTexture("_BaseMap"));
                    if(source.HasProperty("_Cutoff"))mask.SetFloat("_Cutoff",source.GetFloat("_Cutoff"));
                    mask.SetFloat("_Cull",source.HasProperty("_Cull")?source.GetFloat("_Cull"):2);
                    m_commandBuffer.DrawRenderer(renderer,mask,i,0);
                }
            }
            m_isCommandBufferDirty=false;
		}

		public void UpdateMaterial(Material mat)
		{
            if(mat && m_shadowRenderer) mat.SetColor("_Color",m_shadowRenderer.shadowColor);
		}

		public void UpdateTransform()
		{
            if(!m_target)return;
            if(m_textureAlignment!=TextureAlignment.None || m_targetDirection) {
                Vector3 up=m_textureAlignment==TextureAlignment.TargetAxisX ? m_target.right : m_textureAlignment==TextureAlignment.TargetAxisZ ? m_target.forward : m_target.up;
                Vector3 direction=m_targetDirection ? m_targetDirection.forward : transform.forward;
                transform.LookAt(transform.position+direction,up);
            }
            if(m_followTarget)transform.position+=m_target.position-transform.TransformPoint(m_localTargetPosition);
		}

		private void Awake()
		{
            m_shadowRenderer=GetComponent<ShadowTextureRenderer>(); CacheOffset();
		}

		private void OnValidate()
		{
            m_isCommandBufferDirty=true;
		}

		private void OnEnable()
		{
            if(!m_shadowRenderer)m_shadowRenderer=GetComponent<ShadowTextureRenderer>();
            if(m_replacedMaterialCache==null)m_replacedMaterialCache=new Dictionary<Material,Material>();
            CreateCommandBuffer();
		}

		private void OnDisable()
		{
            if(m_commandBuffer!=null) {if(m_shadowRenderer)m_shadowRenderer.RemoveCommandBuffer(m_commandBuffer);m_commandBuffer.Release();m_commandBuffer=null;}
		}

		private void OnDestroy()
		{
            OnDisable();if(m_replacedMaterialCache!=null)foreach(var material in m_replacedMaterialCache.Values) {if(Application.isPlaying)Destroy(material);else DestroyImmediate(material);}m_replacedMaterialCache?.Clear();
		}

		private void LateUpdate()
		{
            if(m_updateFunction==UpdateFunction.LateUpdate)UpdateTransform();
		}

		internal void OnPreCull()
		{
            if(m_updateFunction==UpdateFunction.OnPreCull)UpdateTransform();
            UpdateCommandBuffer();
		}

		private void OnVisibilityChanged(bool isVisible)
		{
		}

		private void CreateCommandBuffer()
		{
            if(m_commandBuffer!=null || !m_shadowRenderer)return;
            m_commandBuffer=new CommandBuffer {name="Reconstructed target shadow"};m_shadowRenderer.AddCommandBuffer(m_commandBuffer);
		}

		private bool IsTargetObjectTreeStructureChanged()
		{
			return false;
		}

		private void AddDrawCommandForGameObject(GameObject obj, bool recursive, ref CommandBufferHash commandBufferHash, bool hashOnly)
		{
		}

		private void AddDrawCommand(Renderer renderer, int renderTypeIndex)
		{
		}
	}
}
