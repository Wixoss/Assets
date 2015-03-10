using UnityEngine;
using System.Collections;

public class BackGround : MonoBehaviour
{

	public Vector3 Scale;
	public Vector3 Pos;
    public float RotaionSpeed;

    // Use this for initialization
    void Start()
    {
//        m.mainTexture = Texture2D;
//        renderer.material = m;
    }

    // Update is called once per frame
    void Update()
    {
        var rot = Quaternion.Euler(0, 0, Time.time * RotaionSpeed);
		var m = Matrix4x4.TRS(Pos, rot, Scale);
        renderer.material.SetMatrix("_Rotation", m);
    }
}
