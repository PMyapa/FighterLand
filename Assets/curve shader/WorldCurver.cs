using UnityEngine;

[ExecuteInEditMode]
public class WorldCurver : MonoBehaviour
{
	[Range(-0.1f, 0.1f)]
	public float curveStrength = 0.0024f;

    int m_CurveStrengthID;

    private void OnEnable()
    {
        m_CurveStrengthID = Shader.PropertyToID("_CurveStrength");
    }

	void FixedUpdate()
	{
		Shader.SetGlobalFloat(m_CurveStrengthID, curveStrength);
	}
}
