/*
   NiceJson is a simple library for JSON Data Interchange Standard
   Copyright (C) 2015 Ángel Quiroga Mendoza <me@angelqm.com>

   This program is free software; you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation; either version 3 of the License, or
   (at your option) any later version.

   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with this program; if not, write to the Free Software Foundation,
   Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301  USA
*/

using System;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;

namespace NiceJson
{
	public abstract class JsonNode
	{
        public const char IDENT_CHAR = ' ';
        public const int IDENT_COUNT = 1;

        public const char CHAR_CURLY_OPEN = '{';
        public const char CHAR_CURLY_CLOSED = '}';
        public const char CHAR_SQUARED_OPEN = '[';
        public const char CHAR_SQUARED_CLOSED = ']';

        public const char CHAR_COLON = ':';
        public const char CHAR_COMMA = ',';
        public const char CHAR_QUOTE = '"';

        public const char CHAR_NULL_LITERAL = 'n';
        public const char CHAR_TRUE_LITERAL = 't';
        public const char CHAR_FALSE_LITERAL = 'f';

        public const char CHAR_SPACE = ' ';
        public const char CHAR_RF = '\r';
        public const char CHAR_NL = '\n';
        public const char CHAR_TAB = '\t';
        public const char CHAR_SCAPE = '\\';

        public const string STRING_SPACE = " ";
        public const string STRING_LITERAL_NULL = "null";
        public const string STRING_LITERAL_TRUE = "true";
        public const string STRING_LITERAL_FALSE = "false";

        //setter implicit casting 

        public static implicit operator JsonNode(string value)
		{
			return new JsonBasic(value);
		}

		public static implicit operator JsonNode(int value)
		{
			return new JsonBasic(value);
		}

        public static implicit operator JsonNode(long value)
        {
            return new JsonBasic(value);
        }

        public static implicit operator JsonNode(float value)
		{
			return new JsonBasic(value);
		}

		public static implicit operator JsonNode(double value)
		{
			return new JsonBasic(value);
		}

        public static implicit operator JsonNode(decimal value)
        {
            return new JsonBasic(value);
        }

        public static implicit operator JsonNode(bool value)
		{
			return new JsonBasic(value);
		}

		//getter implicit casting 

		public static implicit operator string(JsonNode value)
		{
			if (value != null)
			{
				return value.ToString();
			}
			else
			{
				return null;
			}
		}

		public static implicit operator int(JsonNode value)
		{
			return int.Parse(((JsonBasic) value).ToString());
		}

        public static implicit operator long (JsonNode value)
        {
            return long.Parse(((JsonBasic)value).ToString());
        }

        public static implicit operator float(JsonNode value)
		{
			return float.Parse(((JsonBasic) value).ToString());
		}
		
		public static implicit operator double(JsonNode value)
		{
			return double.Parse(((JsonBasic) value).ToString());
		}

        public static implicit operator decimal (JsonNode value)
        {
            return decimal.Parse(((JsonBasic)value).ToString());
        }

        public static implicit operator bool(JsonNode value)
		{
			return bool.Parse(((JsonBasic) value).ToString());
		}

		public abstract string ToJsonString();

        public string ToJsonPrettyPrintString()
        {
            string jsonString = this.ToJsonString();

            string identStep = string.Empty;
            for (int i = 0; i < IDENT_COUNT; i++)
            {
                identStep += IDENT_CHAR;
            }

            bool inString = false;

            string currentIdent = string.Empty;
            for (int i = 0; i < jsonString.Length; i++)
            {
                switch (jsonString[i])
                {
                    case CHAR_COLON:
                        {
                            if (!inString)
                            {
                                jsonString = jsonString.Insert(i + 1, STRING_SPACE);
                            }
                        }
                        break;
                    case CHAR_QUOTE:
                        {
                            if (i == 0 || (jsonString[i - 1] != CHAR_SCAPE))
                            {
                                inString = !inString;
                            }
                        }
                        break;
                    case CHAR_COMMA:
                        {
                            if (!inString)
                            {
                                jsonString = jsonString.Insert(i + 1, CHAR_NL + currentIdent);
                            }
                        }
                        break;
                    case CHAR_CURLY_OPEN:
                    case CHAR_SQUARED_OPEN:
                        {
                            if (!inString)
                            {
                                currentIdent += identStep;
                                jsonString = jsonString.Insert(i + 1, CHAR_NL + currentIdent);
                            }
                        }
                        break;
                    case CHAR_CURLY_CLOSED:
                    case CHAR_SQUARED_CLOSED:
                        {
                            if (!inString)
                            {
                                currentIdent = currentIdent.Substring(0, currentIdent.Length - identStep.Length);
                                jsonString = jsonString.Insert(i, CHAR_NL + currentIdent);
                                i += currentIdent.Length + 1;
                            }
                        }
                        break;
                }
            }

            return jsonString;
        }
    }

	public class JsonBasic : JsonNode
	{
		private object m_value;

		public JsonBasic(object value)
		{
			m_value = value;
		}

		public override string ToString()
		{
			return m_value.ToString();
		}

		public override string ToJsonString ()
		{
			if (m_value == null)
			{
				return STRING_LITERAL_NULL;
			}
			else if (m_value is string)
			{
				return CHAR_QUOTE + m_value.ToString() + CHAR_QUOTE;
			}
			else if (m_value is bool)
			{
                if ((bool) m_value)
                {
                    return STRING_LITERAL_TRUE;
                }
                else
                {
                    return STRING_LITERAL_FALSE;
                }
			}
            else
            {
                return m_value.ToString();
            }
		}

    }

	public class JsonObject : JsonNode, IEnumerable
    {
		private Dictionary<string,JsonNode> m_dictionary = new Dictionary<string, JsonNode>();

        public Dictionary<string,JsonNode>.KeyCollection Keys
        {
            get
            {
                return m_dictionary.Keys;
            }
        }

        public Dictionary<string, JsonNode>.ValueCollection Values
        {
            get
            {
                return m_dictionary.Values;
            }
        }

        public JsonNode this[string key]
		{
			get
			{
				return m_dictionary[key];
			}

			set
			{
				m_dictionary[key] = value;
			}
		}

        public void Add(string key, JsonNode value)
        {
            m_dictionary.Add(key, value);
        }

        public bool Remove(string key)
        {
            return m_dictionary.Remove(key);
        }

        public bool ContainsKey(string key)
		{
			return m_dictionary.ContainsKey(key);
		}

		public bool ContainsValue(JsonNode value)
		{
			return m_dictionary.ContainsValue(value);
		}

        public void Clear()
        {
            m_dictionary.Clear();
        }

        public int Count
        {
            get
            {
                return m_dictionary.Count;
            }
        }

        public IEnumerator GetEnumerator()
        {
            foreach (KeyValuePair<string, JsonNode> jsonKeyValue in m_dictionary)
            {
                yield return jsonKeyValue;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToJsonString ()
		{
			if (m_dictionary == null)
			{
				return STRING_LITERAL_NULL;
			}
			else
			{
				string jsonString = string.Empty;
                jsonString += CHAR_CURLY_OPEN;
                foreach (string key in m_dictionary.Keys)
				{
					jsonString += CHAR_QUOTE+key+ CHAR_QUOTE+ CHAR_COLON;
					if (m_dictionary[key] != null)
					{
						jsonString += m_dictionary[key].ToJsonString();
					}
					else
					{
						jsonString += STRING_LITERAL_NULL;
					}

                    jsonString += CHAR_COMMA;
                }
				if (jsonString[jsonString.Length -1] == CHAR_COMMA)
				{
					jsonString = jsonString.Substring(0,jsonString.Length -1);//removing last ,
				}
				jsonString+= CHAR_CURLY_CLOSED;

				return jsonString;
			}

		}
	}

	public class JsonArray : JsonNode, IEnumerable<JsonNode>
    {
		private List<JsonNode> m_list = new List<JsonNode>();

		public int Count
		{
			get
			{
				return m_list.Count;
			}
		}
		
		public JsonNode this[int index]
		{
			get
			{
				return m_list[index];
			}
			
			set
			{
				m_list[index] = value;
			}
		}

        public IEnumerator<JsonNode> GetEnumerator()
        {
            foreach (JsonNode value in m_list)
            {
                yield return value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        //expose some methods of list extends with needs

        public void Add(JsonNode item)
		{
			m_list.Add(item);
		}

		public void AddRange(IEnumerable<JsonNode> collection)
		{
			m_list.AddRange(collection);
		}

        public void Insert(int index,JsonNode item)
        {
            m_list.Insert(index,item);
        }

        public void InsertRange(int index,IEnumerable<JsonNode> collection)
        {
            m_list.InsertRange(index,collection);
        }

        public void RemoveAt(int index)
        {
            m_list.RemoveAt(index);
        }

        public bool Remove(JsonNode item)
		{
			return m_list.Remove(item);
		}

		public void Clear()
		{
			m_list.Clear();
		}

		//end exposed methods

		
		public override string ToJsonString ()
		{
			if (m_list == null)
			{
				return STRING_LITERAL_NULL;
			}
			else
			{
				string jsonString = string.Empty;
                jsonString += CHAR_SQUARED_OPEN;
                foreach (JsonNode value in m_list)
				{
					if (value != null)
					{
						jsonString+= value.ToJsonString();
					}
					else
					{
						jsonString += STRING_LITERAL_NULL;
					}

                    jsonString += CHAR_COMMA;
                }
				if (jsonString[jsonString.Length-1] == CHAR_COMMA)
				{
					jsonString = jsonString.Substring(0,jsonString.Length -1);//removing last ,
				}
				jsonString+= CHAR_SQUARED_CLOSED;
				return jsonString;
			}
		}
	}

    public class JsonUtils
    {
        public static JsonNode ParseJsonString(string jsonString)
        {
            return ParseJsonPart(RemoveNonTokenChars(jsonString));
        }

        private static JsonNode ParseJsonPart(string jsonPart)
        {
            JsonNode jsonPartValue = null;
            switch (jsonPart[0])
            {
                case JsonNode.CHAR_CURLY_OPEN:
                    {
                        JsonObject jsonObject = new JsonObject();
                        List<string> splittedParts = SplitJsonParts(jsonPart.Substring(1, jsonPart.Length - 2));
                        string[] keyValueParts = new string[2];

                        foreach (string keyValuePart in splittedParts)
                        {
                            keyValueParts = SplitKeyValuePart(keyValuePart);
                            jsonObject[keyValueParts[0]] = ParseJsonPart(keyValueParts[1]);
                        }
                        jsonPartValue = jsonObject;
                    }
                    break;
                case JsonNode.CHAR_SQUARED_OPEN:
                    {
                        JsonArray jsonArray = new JsonArray();
                        List<string> splittedParts = SplitJsonParts(jsonPart.Substring(1, jsonPart.Length - 2));

                        foreach (string part in splittedParts)
                        {
                            jsonArray.Add(ParseJsonPart(part));
                        }
                        jsonPartValue = jsonArray;
                    }
                    break;
                case JsonNode.CHAR_QUOTE:
                    {
                        jsonPartValue = new JsonBasic(jsonPart.Substring(1, jsonPart.Length - 2));
                    }
                    break;
                case JsonNode.CHAR_FALSE_LITERAL://false
                    {
                        jsonPartValue = new JsonBasic(false);
                    }
                    break;
                case JsonNode.CHAR_TRUE_LITERAL://true
                    {
                        jsonPartValue = new JsonBasic(true);
                    }
                    break;
                case JsonNode.CHAR_NULL_LITERAL://null
                    {
                        jsonPartValue = null;
                    }
                    break;
                default://it must be a number or it will fail
                    {
                        long longValue = 0;
                        if (long.TryParse(jsonPart, NumberStyles.Any, CultureInfo.InvariantCulture, out longValue))
                        {
                            if (longValue > int.MaxValue || longValue < int.MinValue)
                            {
                                jsonPartValue = new JsonBasic(longValue);
                            }
                            else
                            {
                                jsonPartValue = new JsonBasic((int)longValue);
                            }
                        }
                        else
                        {
                            decimal decimalValue = 0;
                            if (decimal.TryParse(jsonPart, NumberStyles.Any, CultureInfo.InvariantCulture, out decimalValue))
                            {
                                jsonPartValue = new JsonBasic(decimalValue);
                            }
                        }
                    }
                    break;
            }

            return jsonPartValue;
        }

        private static List<string> SplitJsonParts(string json)
        {
            List<string> jsonParts = new List<string>();
            int identLevel = 0;
            int lastPartChar = 0;
            bool inString = false;

            for (int i = 0; i < json.Length; i++)
            {
                switch (json[i])
                {
                    case JsonNode.CHAR_COMMA:
                        {
                            if (!inString && identLevel == 0)
                            {
                                jsonParts.Add(json.Substring(lastPartChar, i - lastPartChar));
                                lastPartChar = i + 1;
                            }
                        }
                        break;
                    case JsonNode.CHAR_QUOTE:
                        {
                            if (i == 0 || (json[i - 1] != JsonNode.CHAR_SCAPE))
                            {
                                inString = !inString;
                            }
                        }
                        break;
                    case JsonNode.CHAR_CURLY_OPEN:
                    case JsonNode.CHAR_SQUARED_OPEN:
                        {
                            if (!inString)
                            {
                                identLevel++;
                            }
                        }
                        break;
                    case JsonNode.CHAR_CURLY_CLOSED:
                    case JsonNode.CHAR_SQUARED_CLOSED:
                        {
                            if (!inString)
                            {
                                identLevel--;
                            }
                        }
                        break;
                }
            }

            jsonParts.Add(json.Substring(lastPartChar));

            return jsonParts;
        }

        private static string[] SplitKeyValuePart(string json)
        {
            string[] parts = new string[2];
            bool inString = false;

            bool found = false;
            int index = 0;

            while (index < json.Length && !found)
            {
                if (json[index] == JsonNode.CHAR_QUOTE && (index == 0 || (json[index - 1] != JsonNode.CHAR_SCAPE)))
                {
                    if (!inString)
                    {
                        inString = true;
                        index++;
                    }
                    else
                    {
                        parts[0] = json.Substring(1, index - 1);
                        parts[1] = json.Substring(index + 2);//+2 because of the :
                        found = true;
                    }
                }
                else
                {
                    index++;
                }
            }

            return parts;
        }

        private static string RemoveNonTokenChars(string s)
        {
            int len = s.Length;
            char[] s2 = new char[len];
            int currentPos = 0;
            bool outString = true;
            for (int i = 0; i < len; i++)
            {
                char c = s[i];
                if (c == JsonNode.CHAR_QUOTE)
                {
                    if (i == 0 || (s[i - 1] != JsonNode.CHAR_SCAPE))
                    {
                        outString = !outString;
                    }
                }

                if (!(c == JsonNode.CHAR_SPACE && outString) && c != JsonNode.CHAR_RF && c != JsonNode.CHAR_NL && c != JsonNode.CHAR_TAB)
                {
                    s2[currentPos++] = c;
                }
            }
            return new String(s2, 0, currentPos);
        }
    }
}