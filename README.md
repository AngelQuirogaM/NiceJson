## Description
NiceJson is a simple C# library for JSON. You can use NiceJson to encode or decode JSON text. No binary saves, no Output/InputStreams, no bullshit.
**Just input/output string with optional pretty print**.

## Features
* Full compliance with JSON specification (RFC4627) (needs testing)
* With implicit operators.
* Simple string input output [JsonNode.ToJsonString(), JsonNode.ToJsonPrettyPrintString(),JsonNode.ParseJsonString(string json)]

## Sample
All you have to do is read the Sample to know how all the library works;)

```csharp
using NiceJson;

public class JsonExample
{
    public JsonExample()
    {
        //Object creation, all types extends from JsonNode, 
        JsonArray arrayExample = new JsonArray();
        JsonObject objectExample = new JsonObject();
        JsonObject objectExample2 = new JsonObject();

        string outPutString;
        string outPutPrettyPrintString;

        //Basic types can be created with JsonBasic or directly the type you want.
        int basicIntExample = 17;

        //Adding or removing components, it's the same like you were using a Dictionary (JsonObject) or a List (JsonArray)
        arrayExample.Add(objectExample);
        arrayExample.Add(objectExample2);

        objectExample["name"] = "Ángel";
        objectExample["age"] = basicIntExample;
        objectExample["programmer"] = true;
        objectExample["glasses"] = null; //Yes it has all basic types of Json including null :)

        //You can do operations with JsonNodes like if they were the primitive types without casting them
        objectExample["age"] = objectExample["age"] + 10;

        objectExample2["name"] = "Manolo";
        objectExample2["age"] = 54.4f;
        objectExample2["programmer"] = false;
        objectExample2["glasses"] = new JsonArray() { "sunglases", 3, null};

        //Also you can iterate through JsonArray and JsonObject(also using .Keys, .Values like dictionary)
        foreach(JsonObject personObject in arrayExample)
        {
            personObject["surname"] = "Surname";
        }

        //yes it has basic string, no spaces, no tabs, not end lines
        outPutString = arrayExample.ToJsonString();
        /* 
            outPutString : 
            
            [{"name":"Ángel","age":27,"programmer":true,"glasses":null,"surname":"Surname"},{"name":"Manolo","age":54.4,"programmer":false,"glasses":["sunglases",3,null],"surname":"Surname"}]
        */

        //yes it has pretty pring string, with spaces, tabs, endlines):
        outPutPrettyPrintString = arrayExample.ToJsonPrettyPrintString();
        /*
            outPutPrettyPrintString :

            [
                {
                    "name": "Ángel",
                    "age": 27,
                    "programmer": true,
                    "glasses": null,
                    "surname": "Surname"
                },
                {
                    "name": "Manolo",
                    "age": 54.4,
                    "programmer": false,
                    "glasses": [
                        "sunglases",
                        3,
                        null
                    ],
                    "surname": "Surname"
                }
            ]
        */

        //Also you can configurate the Json.IDENT_CHAR (\t,' ',whatever you want) and Json.PRETTYPRINT_IDENT_COUNT (for number of times you want to repeat char per ident)

        //Parsing json it's easy :)

        arrayExample = (JsonArray) JsonNode.ParseJsonString(outPutString);

    }
}


```
