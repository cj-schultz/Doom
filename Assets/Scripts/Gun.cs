using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
public class Gun : MonoBehaviour
{
    public Transform muzzleTip;
    public ParticleSystem muzzleFlash;
    public GameObject sparksPrefab;
    public Animator gunAnim;
    public AudioClip gunshotSound;
    public ushort hapticLevel = 1000;

    private Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.ParentToHand | Hand.AttachmentFlags.SnapOnAttach;

    private Hand holdingHand;
    
    private void HandHoverUpdate(Hand hand)
    {

        if ((hand.controller != null && hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)) || hand.GetStandardInteractionButtonDown())
        {
            if(hand.currentAttachedObject == gameObject)
            {
                StartCoroutine(Shoot());
            }            
            else
            {
                hand.HoverLock(GetComponent<Interactable>());
                hand.AttachObject(gameObject, attachmentFlags, "Attach_Gun");
                holdingHand = hand;
            }
        }        
    }

    private IEnumerator Shoot()
    {
        // Asthetics
        muzzleFlash.Play();
        gunAnim.SetTrigger("Shoot");
        if(holdingHand.controller != null)
        {
            holdingHand.controller.TriggerHapticPulse(hapticLevel);
        }

        AudioSource.PlayClipAtPoint(gunshotSound, transform.position, 0.25f);

        // Functionality
        RaycastHit hitInfo;                          
        if(Physics.Raycast(muzzleTip.transform.position, muzzleTip.transform.forward, out hitInfo, 50f))
        {
            if(!hitInfo.collider.isTrigger)
            {
                GameObject sparks = Instantiate(sparksPrefab, hitInfo.point, Quaternion.Euler(hitInfo.normal));
                Destroy(sparks, .5f);
            }            

            if (hitInfo.transform.tag == "Enemy")
            {                
                Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
                enemy.GetShot(transform, hitInfo);
            }
            else if(hitInfo.transform.tag == "EnemyBullet")
            {
                hitInfo.transform.gameObject.SetActive(false);
                Destroy(hitInfo.transform.gameObject);
            }
        }

        yield return null;
    }
}
