using System;
using System.Collections;
using DG.Tweening;
using Interactable.Object;
using Items.Data;
using Items.Script;
using UnityEngine;
using Zlipacket.CoreZlipacket.Tools;
using Environment = Zlipacket.CoreZlipacket.Misc.Environment;
using Random = UnityEngine.Random;

namespace Items.Resources.Ingredients.Soda
{
    [RequireComponent(typeof(ImpactDetector), typeof(ItemGrab))]
    public class Soda : ItemScript
    {
        private ImpactDetector impactDetector;
        private ItemGrab itemGrab;
        [SerializeField] private SO_Item emptyCan;
        
        [Header("Configs")]
        public float impactThreshold = 450f;
        public float splashDuration = 7f;
        public float splashForce = 2f;
        public float splashAngle = 5f;
        
        private Coroutine co_Splash = null;
        public bool isSplashing => co_Splash != null;

        private void Start()
        {
            impactDetector = GetComponent<ImpactDetector>();
            itemGrab = GetComponent<ItemGrab>();
            
            impactDetector.onImpact.AddListener(OnImpact);
        }

        private void OnImpact(float impactMagnitude)
        {
            if (impactMagnitude < impactThreshold)
                return;

            //Debug.Log("Start Impact: " + impactMagnitude);
            PlayShakeAnimation();
            Splash();
        }

        public void Splash()
        {
            if (isSplashing)
                return;
            
            co_Splash = StartCoroutine(Splashing());
        }

        private IEnumerator Splashing()
        {
            itemGrab.allowInventoryStoring = false;
            float elapsedTime = 0f;
            
            while (elapsedTime < splashDuration)
            {
                yield return null;
                
                rb.AddForce(ZlipUtilities.GetRandomDirectionInCone(-transform.up, splashAngle) * splashForce, ForceMode.Force);
                
                elapsedTime += Time.fixedDeltaTime;
            }
            
            SplashDone();
            co_Splash = null;
        }

        private void SplashDone()
        {
            Item can = Instantiate(emptyCan.itemPrefab, transform.position, transform.rotation).GetComponent<Item>();

            can.transform.SetParent(Environment.Instance.root);
            can.Initialize();
            
            Destroy(gameObject);
        }

        public Tween PlayShakeAnimation()
        {
            if (isTweening)
                itemAnimation.Kill();
            
            itemAnimation = transform.DOShakeRotation(1f);
            
            return itemAnimation;
        }
    }
}