using UnityEngine;

public class EffectObject : MonoBehaviour
{
    public int _id;

    public void StartEffect(int id, float time)
    {
        _id = id;
        Invoke("DestoryEffect", time);
    }

    public void FreeObject()
    {
        CancelInvoke();

        EffectPoolMgr.In.FreeObject(this);
    }
    void DestoryEffect()
    {
        EffectPoolMgr.In.FreeObject(this);
    }
}