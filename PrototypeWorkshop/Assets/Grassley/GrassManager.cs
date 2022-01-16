using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassManager : MonoBehaviour
{
    public Sprite FullGrassSprite;
    public Sprite cutGrassSprite;

    public GameObject cutParticles;
    public GameObject dirtParticles;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeParticleAtLocation(Vector3 pos)
    {
        cutParticles.transform.position = pos;

        ParticleSystem.EmitParams m = new ParticleSystem.EmitParams();

        cutParticles.GetComponent<ParticleSystem>().Emit(m, 12);

    }
    public void MakeDirtParticlesAtLocation(Vector3 pos, Vector3 velo)
    {
        dirtParticles.transform.position = pos;

        ParticleSystem.EmitParams m = new ParticleSystem.EmitParams();
        m.velocity = velo;
        dirtParticles.GetComponent<ParticleSystem>().Emit(m, 6);

    }

}
