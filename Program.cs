using System.ComponentModel;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

namespace HW9;

public class Program
{
  public class TempJSONtoXML
  {
    [XmlAnyElement]
    public XmlElement[] anyElements = new XmlElement[] {};

    public override string ToString()
    {
      foreach (XmlNode node in anyElements)
      {
        Console.WriteLine(node.Name + " : " + node.InnerText);
      }

      return "";
    }
  }

  static void Main(string[] args)
  {
    JSONtoXML("""{"query": "Виктор Иван","count": 7,"parts": ["NAME", "SURNAME", ["NAME", "SURNAME"], {"name": "dude", "age" : 23}]}""");
  }

  static public long count = 0;

  public static void JSONtoXML(string json)
  {
    var serializer = new XmlSerializer(typeof(TempJSONtoXML));

    var temp = (TempJSONtoXML?) serializer.Deserialize(new StringReader("<TempJSONtoXML>\n" + JSONtoString(json) + "</TempJSONtoXML>"));

    temp.ToString();
  }

  static string JSONtoString(string json)
  {
    string str = string.Empty;
    
    if (json != string.Empty && !json.StartsWith('['))
    {
      var temp = JsonDocument.Parse(json).RootElement;

      foreach (var item in temp.EnumerateObject())
      {
        if (item.Value.ToString().StartsWith("["))
        {
          foreach (var i in item.Value.EnumerateArray())
          {
            if (i.ToString().StartsWith("{"))
            {
              str += "\n<" + item.Name + ">" + JSONtoString(i.ToString()) + "</" + item.Name + ">" + Environment.NewLine;
            }
            else if (i.ToString().StartsWith("["))
            {
              str += "\n<" + item.Name + ">" +  JSONtoString("{\"" + item.Name + ++count + "\":" + i.ToString() + "}") + "</" + item.Name + ">" + Environment.NewLine;
            }
            else
            {
              str += ("\n<" + item.Name + ">"+ i + "</" + item.Name + ">") + Environment.NewLine;
            }
          }
        }
        else if (item.Value.ToString().StartsWith("{"))
        {
          str += ("\n<" + ++count + item.Name + ">") + Environment.NewLine;
          str += "\n<" + item.Name + ">" + JSONtoString(item.Value.ToString()) + "</" + item.Name + ">" + Environment.NewLine;
        }
        else
        {
          str += ("\n<" + item.Name + ">" + item.Value + "</" + item.Name + ">") + Environment.NewLine;
        }
      }
    }

    return str;
  }
}
