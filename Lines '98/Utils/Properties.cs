using System;
using System.Collections;
using System.IO;
using System.Xml;

namespace Lines98.Utils
{
    class Properties
    {
        public const string TypeInteger = "int";
        public const string TypeDate = "date";
        public const string TypeBoolean = "bool";
        public const string TypeString = "str";

        private readonly Hashtable _properties = new Hashtable();

        public void Load(string filename)
        {
            if (!File.Exists(filename)) return;
            var doc = new XmlDocument();
            doc.Load(filename);
            XmlNode root = doc.DocumentElement;
            if (root == null) return;
            var node = root.FirstChild.FirstChild;
            while (node != null)
            {
                if (node.Attributes != null)
                {
                    var name = node.Attributes["name"].Value;
                    var type = node.Attributes["type"].Value;
                    var stringValue = node.Attributes["value"].Value;
                    object objectValue = null;
                    switch (type)
                    {
                        case TypeInteger:
                            objectValue = int.Parse(stringValue);
                            break;
                        case TypeDate:
                            objectValue = DateTime.Parse(stringValue);
                            break;
                        case TypeBoolean:
                            objectValue = bool.Parse(stringValue);
                            break;
                        default:
                            objectValue = stringValue;
                            break;
                    }

                    _properties.Add(name, objectValue);
                }

                node = node.NextSibling;
            }
        }

        public void Save(string filename)
        {
            var doc = new XmlDocument();
            var root = doc.CreateNode(XmlNodeType.Element, "configuration", "");

            doc.AppendChild(root);
            var node = doc.CreateNode(XmlNodeType.Element, "properties", "");
            root.AppendChild(node);
            root = node;

            foreach (var key in _properties.Keys)
            {
                var objValue = _properties[key];
                node = doc.CreateNode(XmlNodeType.Element, "property", "");

                // Name
                var attribute = doc.CreateAttribute("name");
                attribute.Value = key.ToString();
                if (node.Attributes != null)
                {
                    node.Attributes.Append(attribute);

                    // Value
                    attribute = doc.CreateAttribute("value");
                    attribute.Value = objValue.ToString();
                    node.Attributes.Append(attribute);

                    // Type
                    attribute = doc.CreateAttribute("type");
                    if (objValue is int)
                    {
                        attribute.Value = TypeInteger;
                    }
                    else if (objValue is DateTime)
                    {
                        attribute.Value = TypeDate;
                    }
                    else if (objValue is bool)
                    {
                        attribute.Value = TypeBoolean;
                    }
                    else if (objValue is string)
                    {
                        attribute.Value = TypeString;
                    }
                    else
                    {
                        attribute.Value = TypeString;
                    }
                    node.Attributes.Append(attribute);
                }

                root.AppendChild(node);
            }

            doc.Save(filename);
        }

        public object GetProperty(string name)
        {
            return _properties[name];
        }

        public int GetProperty(string name, int defaultValue)
        {
            var result = defaultValue;
            var obj = GetProperty(name);

            if (obj is int)
            {
                result = (int)obj;
            }
            else
            {
                _properties[name] = defaultValue;
            }

            return result;
        }

        public DateTime GetProperty(string name, DateTime defaultValue)
        {
            var result = defaultValue;
            var obj = GetProperty(name);

            if (obj is DateTime)
            {
                result = (DateTime)obj;
            }
            else
            {
                _properties[name] = defaultValue;
            }

            return result;
        }

        public bool GetProperty(string name, bool defaultValue)
        {
            var result = defaultValue;
            var obj = GetProperty(name);

            if (obj is bool)
            {
                result = (bool)obj;
            }
            else
            {
                _properties[name] = defaultValue;
            }

            return result;
        }

        public string GetProperty(string name, string defaultValue)
        {
            var result = defaultValue;
            var obj = GetProperty(name);

            if (obj != null)
            {
                result = obj.ToString();
            }
            else
            {
                _properties[name] = defaultValue;
            }

            return result;
        }

        public void SetProperty(string name, object propValue)
        {
            _properties[name] = propValue;
        }
    }
}
