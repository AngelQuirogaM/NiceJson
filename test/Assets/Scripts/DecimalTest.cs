using UnityEngine;
using System.Collections;
using System.Globalization;

public class DecimalTest : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        //string decimalString = "-1.30142114406914976E17";
        ParseStringNumber("-1.30142114406914976E17");
        ParseStringNumber("-1.7555215491128452E-19");
        ParseStringNumber("0.1666666666666666666");
        ParseStringNumber("12.0");
    }
	
	private void ParseStringNumber(string numberString)
    {
        int intOutput = 0;
        long longOutput = 0;
        float floatOutput = 0;
        double doubleOutput = 0;
        decimal decimalOutput = 0;

        string outPut = "String = \"" + numberString+"\"\n";

        if (int.TryParse(numberString, NumberStyles.Any, CultureInfo.InvariantCulture, out intOutput))
        {
            outPut += ("Int Parsed : " + intOutput.ToString()) + "\n";
        }
        else
        {
            outPut += ("Int parsed FAIL") + "\n";
        }

        if (long.TryParse(numberString, NumberStyles.Any, CultureInfo.InvariantCulture, out longOutput))
        {
            outPut += ("Long Parsed : " + longOutput.ToString()) + "\n";
        }
        else
        {
            outPut += ("Long parsed FAIL") + "\n";
        }

        if (float.TryParse(numberString, NumberStyles.Any, CultureInfo.InvariantCulture, out floatOutput))
        {
            outPut += ("Float Parsed : " + floatOutput.ToString()) + "\n";
        }
        else
        {
            outPut += ("Float parsed FAIL") + "\n";
        }

        if (double.TryParse(numberString, NumberStyles.Any, CultureInfo.InvariantCulture, out doubleOutput))
        {
            outPut += ("Double parsed : " + doubleOutput.ToString()) + "\n";
        }
        else
        {
            outPut += ("Double parsed FAIL") + "\n";
        }

        if (decimal.TryParse(numberString, NumberStyles.Any, CultureInfo.InvariantCulture, out decimalOutput))
        {
            outPut += ("Decimal Parsed : " + decimalOutput.ToString()) + "\n";
        }
        else
        {
            outPut += ("Parsed FAIL") + "\n";
        }

        Debug.Log(outPut);
    }
}
