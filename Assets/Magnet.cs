using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : TestObject
{
    public TestCollider _blueCollider1 = null;
    public TestCollider _blueCollider2 = null;

    public override void OnActivate()
    {
        var hitColliders = Physics2D.OverlapCircleAll(_blueCollider1.transform.position, 1f);

        for (var i = 0; i < hitColliders.Length; i++)
        {
            //Debug.Log(hitColliders[i].gameObject.name);
            var col = hitColliders[i].gameObject.GetComponent<TestCollider>();
            if (col != null)
            {
                var parent = col.Parent;
                TestCreate.Instance.RemoveLink(this, parent);
            }
        }
        var hitColliders2 = Physics2D.OverlapCircleAll(_blueCollider2.transform.position, 1f);

        for (var i = 0; i < hitColliders2.Length; i++)
        {
            Debug.Log(hitColliders2[i].gameObject.name);
            var col = hitColliders2[i].gameObject.GetComponent<TestCollider>();
            if (col != null)
            {
                var parent = col.Parent;
                TestCreate.Instance.RemoveLink(this, parent);
            }
        }


    }
}
