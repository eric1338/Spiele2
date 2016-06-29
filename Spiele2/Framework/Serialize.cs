using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Framework
{
	public static class Serialize
	{
		/// <summary>
		/// Serializes the obj into SOAP format file.
		/// </summary>
		/// <param name="value_">The object to be serialized.</param>
		/// <param name="fileName_">The file name_.</param>
		public static void ObjIntoSoapFile(this object value_, string fileName_)
		{
			if (null == value_) new ArgumentNullException("Null object given!");
			SoapFormatter formatter = new SoapFormatter();
			using (FileStream fs = new FileStream(fileName_, FileMode.Create))
			{
				formatter.Serialize(fs, value_);
			}
		}

		/// <summary>
		/// Serializes the obj into XML file.
		/// </summary>
		/// <param name="value_">The object to be serialized.</param>
		/// <param name="fileName_">The file name_.</param>
		public static void ObjIntoXMLFile(this object value_, string fileName_)
		{
			if (null == value_) new ArgumentNullException("Null object given!");
			XmlSerializer formatter = new XmlSerializer(value_.GetType());
			using (StreamWriter outfile = new StreamWriter(fileName_))
			{
				formatter.Serialize(outfile, value_);
			}
		}

		/// <summary>
		/// Serializes the obj into XML string.
		/// </summary>
		/// <param name="value_">The object to be serialized.</param>
		/// <param name="fileName_">The file name_.</param>
		public static string ObjIntoXmlString(this object value_)
		{
			if (null == value_) new ArgumentNullException("Null object given!");
			XmlSerializer formatter = new XmlSerializer(value_.GetType());
			StringBuilder builder = new StringBuilder();
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Encoding = Encoding.Default;
			settings.Indent = false;
			settings.OmitXmlDeclaration = true;
			settings.NamespaceHandling = NamespaceHandling.OmitDuplicates;
			using (XmlWriter writer = XmlWriter.Create(builder, settings))
			{
				formatter.Serialize(writer, value_);
			}
			string output = builder.ToString();
			return output;
		}

		/// <summary>
		/// Serializes the obj into binary file.
		/// </summary>
		/// <param name="value_">The object to be serialized.</param>
		/// <param name="fileName_">The file name_.</param>
		public static void ObjIntoBinFile(this object value_, string fileName_)
		{
			if (null == value_) new ArgumentNullException("Null object given!");
			BinaryFormatter formatter = new BinaryFormatter();
			using (FileStream outfile = new FileStream(fileName_, FileMode.Create, FileAccess.Write))
			{
				formatter.Serialize(outfile, value_);
			}
		}

		/// <summary>
		/// Deserializes an new obj instance from XML file.
		/// </summary>
		/// <param name="fileName_">The file name.</param>
		/// <param name="type_">The type of the class that will be deserialized.</param>
		/// <returns>object if successfull</returns>
		public static object ObjFromXMLFile(string fileName_, Type type_)
		{
			using (StreamReader inFile = new StreamReader(fileName_))
			{
				XmlSerializer formatter = new XmlSerializer(type_);
				return formatter.Deserialize(inFile);
			}
		}

		public static object ObjFromXmlString(string xmlString_, Type type_)
		{
			using (StringReader input = new StringReader(xmlString_))
			{
				XmlSerializer formatter = new XmlSerializer(type_);
				return formatter.Deserialize(input);
			}
		}

		/// <summary>
		/// Deserializes an new obj instance from a binary file.
		/// </summary>
		/// <param name="fileName_">The file name.</param>
		/// <returns>object if successfull</returns>
		public static object ObjFromBinFile(string fileName_)
		{
			using (FileStream inFile = new FileStream(fileName_, FileMode.Open, FileAccess.Read))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				return formatter.Deserialize(inFile);
			}
		}
	}
}
