// Authored Built-In profile reconstruction, not full vendor source recovery.
const fs=require('fs');
const root='UnityProject/Assets/Scripts/Assembly-CSharp/DynamicShadowProjector/';
fs.mkdirSync('Evidence/shadow-core-backup',{recursive:true});
function edit(name,changes,extra='') {
 const path=root+name+'.cs';
 const backup='Evidence/shadow-core-backup/'+name+'.cs'; if(!fs.existsSync(backup))fs.copyFileSync(path,backup);
 let source=fs.readFileSync(backup,'utf8');
 for(const [signature,body] of Object.entries(changes)) {
  const pos=source.indexOf(signature); if(pos<0)throw Error('Missing signature '+signature);
  const start=source.indexOf('{',pos);let end=start,depth=0;
  do {if(source[end]==='{')depth++;if(source[end]==='}')depth--;end++;}while(depth&&end<source.length);
  source=source.slice(0,start)+'{\n'+body+'\n\t\t}'+source.slice(end);
 }
 if(extra)source=source.replace('\t\tpublic enum ',extra+'\n\t\tpublic enum ');
 return {path,source};
}
let renderer=edit('ShadowTextureRenderer',{
 'public void SetReplacementShader(Shader shader, string replacementTag)':'            if (Initialize()) m_camera.SetReplacementShader(shader,replacementTag);',
 'public void SetTexturePropertyDirty()':'            m_isTexturePropertyChanged=true;',
 'public void CreateRenderTexture()':`            if (!m_camera || !m_projector || !supportsAuthoredProfile) return;
            if (shadowTexture) { m_camera.targetTexture=null; shadowTexture.Release(); DestroyOwned(shadowTexture); }
            var format=RenderTextureFormat.ARGB32;
            if(m_preferredTextureFormats!=null) foreach(var candidate in m_preferredTextureFormats) if(SystemInfo.SupportsRenderTextureFormat(candidate)) {format=candidate;break;}
            var descriptor=new RenderTextureDescriptor(Mathf.Max(1,m_textureWidth),Mathf.Max(1,m_textureHeight),format,16);
            int requested=Mathf.Clamp((int)m_multiSampling,1,8);
            descriptor.msaaSamples=requested==2||requested==4||requested==8 ? requested : 1;
            descriptor.msaaSamples=SystemInfo.GetRenderTextureSupportedMSAASampleCount(descriptor);
            shadowTexture=new RenderTexture(descriptor) {name="Reconstructed dynamic shadow",hideFlags=HideFlags.HideAndDontSave,wrapMode=TextureWrapMode.Clamp,filterMode=FilterMode.Bilinear};
            shadowTexture.Create(); m_camera.targetTexture=shadowTexture;
            m_projectorMaterial.SetTexture("_ShadowTex",shadowTexture);
            m_isTexturePropertyChanged=false; m_shadowTextureValid=false;`,
 'internal void AddCommandBuffer(CommandBuffer commandBuffer)':`            if(!Initialize())return;
            m_camera.RemoveCommandBuffer(CameraEvent.BeforeImageEffectsOpaque,commandBuffer);
            m_camera.AddCommandBuffer(CameraEvent.BeforeImageEffectsOpaque,commandBuffer);`,
 'internal void RemoveCommandBuffer(CommandBuffer commandBuffer)':'            if(m_camera) m_camera.RemoveCommandBuffer(CameraEvent.BeforeImageEffectsOpaque,commandBuffer);',
 'private bool Initialize()':`            if(!gameObject.scene.IsValid()) return false;
            if(!m_projector)m_projector=GetComponent<Projector>();
            if(!m_projector || !m_projector.material) return false;
            if(!m_camera)m_camera=GetComponent<Camera>();
            if(!m_camera)m_camera=gameObject.AddComponent<Camera>();
            if(!m_projectorMaterial) { originalProjectorMaterial=m_projector.material; m_projectorMaterial=new Material(originalProjectorMaterial) {name=originalProjectorMaterial.name+" (owned shadow)",hideFlags=HideFlags.HideAndDontSave};m_projector.material=m_projectorMaterial; }
            m_camera.cullingMask=0; m_camera.clearFlags=CameraClearFlags.SolidColor; m_camera.backgroundColor=Color.white;
            m_camera.depth=-100; m_camera.allowHDR=false; m_camera.allowMSAA=true;
            m_camera.stereoTargetEye=StereoTargetEyeMask.None;
            return true;`,
 'private bool IsInitialized()':'            return m_camera && m_projectorMaterial && shadowTexture && shadowTexture.IsCreated();',
 'private void OnEnable()':`            if(Initialize()) {CreateRenderTexture(); Update();}`, 
 'private void OnDisable()':`            if(m_camera) {m_camera.enabled=false;m_camera.targetTexture=null;}
            if(m_projectorMaterial)m_projectorMaterial.SetTexture("_ShadowTex",Texture2D.whiteTexture);
            if(shadowTexture) {shadowTexture.Release();DestroyOwned(shadowTexture);shadowTexture=null;}
            m_isVisible=false;m_shadowTextureValid=false;`,
 'private void OnValidate()':'            m_isTexturePropertyChanged=true;',
 'private void OnDestroy()':`            OnDisable();
            if(m_projector && m_projector.material==m_projectorMaterial)m_projector.material=originalProjectorMaterial;
            if(m_projectorMaterial)DestroyOwned(m_projectorMaterial);`,
 'private void Update()':`            if(!Initialize())return;
            if(supportsAuthoredProfile && (m_isTexturePropertyChanged || !shadowTexture || !shadowTexture.IsCreated()))CreateRenderTexture();
            m_camera.orthographic=m_projector.orthographic; m_camera.orthographicSize=m_projector.orthographicSize;
            m_camera.fieldOfView=m_projector.fieldOfView; m_camera.aspect=m_projector.aspectRatio;
            m_camera.nearClipPlane=m_projector.nearClipPlane; m_camera.farClipPlane=m_projector.farClipPlane;
            m_isVisible=m_projector.enabled && supportsAuthoredProfile && shadowTexture;
            m_camera.enabled=m_isVisible;`,
 'private void OnPreCull()':'            Update();',
 'private void OnPostRender()':'            m_shadowTextureValid=shadowTexture && shadowTexture.IsCreated();',
 'public void OnAfterDeserialize()':'            m_isTexturePropertyChanged=true;'
 },`        // Only the inventoried zero-blur/zero-mip Built-In profile is restored.
        private Material originalProjectorMaterial;
        public bool supportsAuthoredProfile => GraphicsSettings.currentRenderPipeline==null && m_blurLevel==0 && m_mipLevel==0 && (int)m_superSampling<=1 && !m_testViewClip;
        public bool shadowTextureValid => m_shadowTextureValid;
        private static void DestroyOwned(Object value) { if(Application.isPlaying)Destroy(value);else DestroyImmediate(value); }
`);
const rProps={multiSampling:'m_multiSampling',superSampling:'m_superSampling',textureWidth:'m_textureWidth',textureHeight:'m_textureHeight',preferredTextureFormats:'m_preferredTextureFormats',mipLevel:'m_mipLevel',blurLevel:'m_blurLevel',blurSize:'m_blurSize',blurFilter:'m_blurFilter',mipmapBlurSize:'m_mipmapBlurSize',singlePassMipmapBlur:'m_singlePassMipmapBlur',mipmapFalloff:'m_mipmapFalloff',customMipmapFalloff:'m_customMipmapFalloff',shadowColor:'m_shadowColor',blurShader:'m_blurShader',downsampleShader:'m_downsampleShader',copyMipmapShader:'m_copyMipmapShader',eraseShadowShader:'m_eraseShadowShader',testViewClip:'m_testViewClip',camerasForViewClipTest:'m_camerasForViewClipTest'};
function properties(source,props,dirty) {
 for(const [name,field] of Object.entries(props)) {
  const regex=new RegExp('(public\\s+[^\\n{]+?\\s+'+name+'\\s*)\\{\\s*get\\s*\\{[^}]*\\}\\s*set\\s*\\{[^}]*\\}\\s*\\}');
  if(!regex.test(source))throw Error('Missing property '+name);
  source=source.replace(regex,`$1{ get { return ${field}; } set { ${field}=value; ${dirty} } }`);
 }return source;
}
renderer.source=properties(renderer.source,rProps,'SetTexturePropertyDirty();');
renderer.source=renderer.source.replace('public Camera projectorCamera => null;','public Camera projectorCamera { get { Initialize(); return m_camera; } }').replace('public bool isProjectorVisible => false;','public bool isProjectorVisible => m_isVisible;');
renderer.source=renderer.source.replace(/public float cameraNearClipPlane\s*\{\s*get\s*\{[^}]*\}\s*set\s*\{[^}]*\}\s*\}/,'public float cameraNearClipPlane { get { return projectorCamera.nearClipPlane; } set { projectorCamera.nearClipPlane=value; } }').replace(/public LayerMask cameraCullingMask\s*\{\s*get\s*\{[^}]*\}\s*set\s*\{[^}]*\}\s*\}/,'public LayerMask cameraCullingMask { get { return projectorCamera.cullingMask; } set { projectorCamera.cullingMask=value; } }');
fs.writeFileSync(renderer.path,renderer.source);
let draw=edit('DrawTargetObject',{
 'public void SetCommandBufferDirty()':'            m_isCommandBufferDirty=true;',
 'public void UpdateCommandBuffer()':`            if(!m_target || !m_shadowShader)return;
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
            m_isCommandBufferDirty=false;`,
 'public void UpdateMaterial(Material mat)':'            if(mat && m_shadowRenderer) mat.SetColor("_Color",m_shadowRenderer.shadowColor);',
 'public void UpdateTransform()':`            if(!m_target)return;
            if(m_textureAlignment!=TextureAlignment.None || m_targetDirection) {
                Vector3 up=m_textureAlignment==TextureAlignment.TargetAxisX ? m_target.right : m_textureAlignment==TextureAlignment.TargetAxisZ ? m_target.forward : m_target.up;
                Vector3 direction=m_targetDirection ? m_targetDirection.forward : transform.forward;
                transform.LookAt(transform.position+direction,up);
            }
            if(m_followTarget)transform.position+=m_target.position-transform.TransformPoint(m_localTargetPosition);`,
 'private void Awake()':`            m_shadowRenderer=GetComponent<ShadowTextureRenderer>(); CacheOffset();`,
 'private void OnEnable()':`            if(!m_shadowRenderer)m_shadowRenderer=GetComponent<ShadowTextureRenderer>();
            if(m_replacedMaterialCache==null)m_replacedMaterialCache=new Dictionary<Material,Material>();
            CreateCommandBuffer();`,
 'private void OnDisable()':`            if(m_commandBuffer!=null) {if(m_shadowRenderer)m_shadowRenderer.RemoveCommandBuffer(m_commandBuffer);m_commandBuffer.Release();m_commandBuffer=null;}`, 
 'private void OnDestroy()':`            OnDisable();if(m_replacedMaterialCache!=null)foreach(var material in m_replacedMaterialCache.Values) {if(Application.isPlaying)Destroy(material);else DestroyImmediate(material);}m_replacedMaterialCache?.Clear();`,
 'private void LateUpdate()':'            if(m_updateFunction==UpdateFunction.LateUpdate)UpdateTransform();',
 'internal void OnPreCull()':`            if(m_updateFunction==UpdateFunction.OnPreCull)UpdateTransform();
            UpdateCommandBuffer();`,
 'private void CreateCommandBuffer()':`            if(m_commandBuffer!=null || !m_shadowRenderer)return;
            m_commandBuffer=new CommandBuffer {name="Reconstructed target shadow"};m_shadowRenderer.AddCommandBuffer(m_commandBuffer);`,
 'private void OnValidate()':'            m_isCommandBufferDirty=true;'
 });
draw.source=properties(draw.source,{target:'m_target',targetDirection:'m_targetDirection',renderChildren:'m_renderChildren',autoDetectHierarchyChanges:'m_autoDetectHierarchyChanges',layerMask:'m_layerMask',textureAlignment:'m_textureAlignment',updateFunction:'m_updateFunction',followTarget:'m_followTarget',shadowShader:'m_shadowShader',replacementShaders:'m_replacementShaders'},'SetCommandBufferDirty();');
draw.source=draw.source.replace('m_target=value; SetCommandBufferDirty();','m_target=value; CacheOffset(); SetCommandBufferDirty();');
draw.source=draw.source.replace('internal CommandBuffer commandBuffer => null;','internal CommandBuffer commandBuffer => m_commandBuffer;\n        private void CacheOffset() { if(m_target)m_localTargetPosition=transform.InverseTransformPoint(m_target.position); }');
fs.writeFileSync(draw.path,draw.source);
for(const kind of ['Opaque','Transparent','Transparent Cutout']) {
 const path=`UnityProject/Assets/Shader/DynamicShadowProjector_Shadow_${kind}.shader`;
 const backup='Evidence/shadow-core-backup/'+kind.replaceAll(' ','-')+'.shader';if(!fs.existsSync(backup))fs.copyFileSync(path,backup);
 fs.writeFileSync(path,`// Approximate capture mask; native shader source was not recovered. See Knowledge/dynamic-shadow-core.md.
Shader "DynamicShadowProjector/Shadow/${kind}" {
 Properties { [HideInInspector] _Cull ("Cull",Float)=2 _Color ("Shadow",Color)=(0,0,0,1) _MainTex ("Alpha",2D)="white" {} _Cutoff ("Cutoff",Range(0,1))=.5 }
 SubShader { Tags { "RenderType"="${kind.replaceAll(' ','')}" } Pass { Cull [_Cull] ZWrite Off Blend DstColor Zero
 HLSLPROGRAM
 #pragma vertex vert
 #pragma fragment frag
 #include "UnityCG.cginc"
 sampler2D _MainTex;float4 _MainTex_ST;float4 _Color;float _Cutoff;
 struct appdata {float4 vertex:POSITION;float2 uv:TEXCOORD0;};
 struct v2f {float4 pos:SV_POSITION;float2 uv:TEXCOORD0;};
 v2f vert(appdata v) {v2f o;o.pos=UnityObjectToClipPos(v.vertex);o.uv=TRANSFORM_TEX(v.uv,_MainTex);return o;}
 float4 frag(v2f i):SV_Target {float alpha=${kind==='Opaque'?'1':'tex2D(_MainTex,i.uv).a'};${kind==='Transparent Cutout'?'clip(alpha-_Cutoff);alpha=1;':''} return float4(lerp(float3(1,1,1),_Color.rgb,saturate(alpha*_Color.a)),1);}
 ENDHLSL
 } } Fallback Off
}
`);
}
console.log('Restored authored core; advanced blur/mips/supersampling/view clipping remain unsupported. Backups under Evidence/shadow-core-backup.');
