using UnityEngine;
using System.Collections;
using System.IO;
using NiceJson;

public class JsonTest : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        string file = "sample.json";
        Debug.Log("Reading file...");

        string jsonString = File.ReadAllText("Assets/"+ file);

        Debug.Log("Init parse");

        JsonNode node = JsonUtils.ParseJsonString(jsonString);

        Debug.Log("Parsed completed");

        File.WriteAllText("Assets/" + "Copy"+file, node.ToJsonPrettyPrintString());
    }
}