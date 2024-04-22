
using System.Collections.Generic;
using UnityEngine;
using UMA.CharacterSystem;
using UMA;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Characters;



namespace vwgamedev.gamecreator.uma{
    [RequireComponent(typeof(DynamicCharacterAvatar))]
    public class CCExpressionPlayer : MonoBehaviour
    {
        private DynamicCharacterAvatar DCA;
        private UMAData umaData;
        private Animator animator;

        [SerializeField] List<skinnedMeshRendererContainer> renderers = new List<skinnedMeshRendererContainer>();
        [SerializeField] protected PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();


        // ------- Character Expression Values -------
        [Range(0f,1f)]public float jawOpen;

        private void Awake(){
            DCA = GetComponent<DynamicCharacterAvatar>();
            animator = GetComponent<Animator>();
            umaData = DCA.umaData;

            UpdateRig();
        }
        /// <summary>
        ///  We call that method always when we have changed our rig and we need to reconfigure our CCExpressionPlayer
        /// </summary>
        public void UpdateRig(){
            Debug.Log("Update Rig");
            renderers.Clear();
            if(umaData == null) return;
            if(umaData.GetRenderers() == null) return;
            foreach(SkinnedMeshRenderer skinnedMeshRenderer in umaData.GetRenderers()){
                skinnedMeshRendererContainer container = new skinnedMeshRendererContainer();
                container.skinnedMesh = skinnedMeshRenderer;
                container.GetBlendShapeNames();
                renderers.Add(container);
            }
        }


        private void LateUpdate(){
            if(animator == null) return;
            Transform jawBone = animator.GetBoneTransform(HumanBodyBones.Jaw);
            if(jawBone != null){
                jawBone.localRotation = Quaternion.Euler(0,0,Mathf.Lerp(-90,-110,jawOpen));
            }
            foreach(skinnedMeshRendererContainer container in renderers){
                container.SetBlendShape("V_Open",jawOpen*100);
            }
            
            
        }

        [System.Serializable]
        public class skinnedMeshRendererContainer
        {
            public SkinnedMeshRenderer skinnedMesh;
            public Dictionary<string, int> blendShapeNames;

            public void SetBlendShape(string name, float value)
            {
                if (blendShapeNames.TryGetValue(name, out int id))
                {
                    skinnedMesh.SetBlendShapeWeight(id, value);
                }
            }
            public void GetBlendShapeNames()
            {
                Mesh m = skinnedMesh.sharedMesh;
                string[] arr;
                arr = new string[m.blendShapeCount];
                blendShapeNames = new Dictionary<string, int>();
                for (int i = 0; i < m.blendShapeCount; i++)
                {
                    string s = m.GetBlendShapeName(i);
                    arr[i] = s;
                    blendShapeNames.Add(s, i);
                }
            }
        }
    }

}
