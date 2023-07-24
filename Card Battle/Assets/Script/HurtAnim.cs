using UnityEngine;

public class HurtAnim : MonoBehaviour
{
    public void StartAnim(Character receiver)
    {
        receiver.GetComponent<Animator>().SetTrigger("Hurt");
    }
}
