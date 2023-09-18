using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveText : MonoBehaviour
{
    TMP_Text text;
    Mesh textMesh;

    Vector3[] vertices;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();

        Debug.Log(text.textInfo.characterCount);

    }

    // Update is called once per frame
    void Update()
    {
        text.ForceMeshUpdate();
        textMesh = text.mesh;
        vertices = textMesh.vertices;

        for (int i = 0; i < text.textInfo.characterCount; i++)
        {
            TMP_CharacterInfo c = text.textInfo.characterInfo[i];

            int index = c.index;

            Vector3 offset = Wobble(Time.time + i);
            vertices[index] += offset;
            vertices[index + 1] += offset;
            vertices[index + 2] += offset;
            vertices[index + 3] += offset;


            //Vector3 offset = Wobble(Time.time + i);
            //vertices[i] = vertices[i] + offset;
        }
        textMesh.vertices = vertices;
        text.canvasRenderer.SetMesh(textMesh);
    }

    Vector2 Wobble(float time)
    {
        return new Vector2(Mathf.Sin(time * 10f), Mathf.Cos(time * 8f));
    }
}
