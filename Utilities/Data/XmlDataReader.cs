using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using System.Xml.Linq;

namespace Utilities.Data
{
	/// <summary>
	/// Basic implementation of an XmlReader that acts as an adapter to the IDataReader interfaces.
	/// It uses streaming to maximize performance
	/// This class is mostly used with things like the SqlBulkCopy API so not all members are implemented
	/// </summary>
	public abstract class XmlDataReader : IDataReader
	{
		private readonly string rowElementName;

		private readonly XmlReader xmlReader;
		private readonly int fieldCount = -1;
		protected readonly int invalidField = -1;

		private bool m_disposed;

		protected IEnumerator<XElement> enumerator;

		public abstract object GetValue(int i);

		/// <summary>
		/// Initialize the XmlDataStreamer. After initialization call Read() to move the reader forward.
		/// </summary>
		/// <param name="xmlReader">XmlReader used to iterate the data. Will be disposed by when done.</param>
		/// <param name="fieldCount">IDataReader FiledCount.</param>
		/// <param name="rowElementName">Name of the XML element that contains row data</param>
		protected XmlDataReader(XmlReader xmlReader, int fieldCount, string rowElementName)
		{
			this.rowElementName = rowElementName;
			this.fieldCount = fieldCount;
			this.xmlReader = xmlReader;
			enumerator = GetXmlStream().GetEnumerator();
		}

		public bool Read()
		{
			return enumerator.MoveNext();
		}

		public int Depth { get; private set; }
		public bool IsClosed { get; private set; }

		public int FieldCount
		{
			get { return fieldCount; }
		}

		public XElement CurrentElement
		{
			get { return enumerator.Current; }
		}

		/// <summary>
		/// http://msdn.microsoft.com/en-us/library/system.xml.linq.xstreamingelement.aspx
		/// </summary>
		/// <returns></returns>
		private IEnumerable<XElement> GetXmlStream()
		{
			using (xmlReader)
			{
				xmlReader.MoveToContent();

				while (xmlReader.Read())
				{
					if (IsRowElement())
					{
						var rowElement = XNode.ReadFrom(xmlReader) as XElement;
						if (rowElement != null)
						{
							yield return rowElement;
						}
					}
				}
			}
		}

		private bool IsRowElement()
		{
			if (xmlReader.NodeType != XmlNodeType.Element)
				return false;

			return xmlReader.Name == rowElementName;
		}

		void IDisposable.Dispose()
		{
			Dispose();
		}

		protected virtual void Dispose()
		{
			if (m_disposed)
				return;

			enumerator.Dispose();
			m_disposed = true;
		}

		public abstract int GetOrdinal(string name);

		#region Not required by sql bulk copy
		object IDataRecord.this[int i]
		{
			get { throw new NotImplementedException(); }
		}

		object IDataRecord.this[string name]
		{
			get { throw new NotImplementedException(); }
		}

		public Type GetFieldType(int i)
		{
			throw new NotImplementedException();
		}

		public int GetValues(object[] values)
		{
			throw new NotImplementedException();
		}


		public bool GetBoolean(int i)
		{
			throw new NotImplementedException();
		}

		public byte GetByte(int i)
		{
			throw new NotImplementedException();
		}

		public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			throw new NotImplementedException();
		}

		public char GetChar(int i)
		{
			throw new NotImplementedException();
		}

		public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			throw new NotImplementedException();
		}

		public Guid GetGuid(int i)
		{
			throw new NotImplementedException();
		}

		public short GetInt16(int i)
		{
			throw new NotImplementedException();
		}

		public int GetInt32(int i)
		{
			throw new NotImplementedException();
		}

		public long GetInt64(int i)
		{
			throw new NotImplementedException();
		}

		public float GetFloat(int i)
		{
			throw new NotImplementedException();
		}

		public double GetDouble(int i)
		{
			throw new NotImplementedException();
		}

		public string GetString(int i)
		{
			throw new NotImplementedException();
		}

		public decimal GetDecimal(int i)
		{
			throw new NotImplementedException();
		}

		public DateTime GetDateTime(int i)
		{
			throw new NotImplementedException();
		}

		public IDataReader GetData(int i)
		{
			throw new NotImplementedException();
		}

		public bool IsDBNull(int i)
		{
			throw new NotImplementedException();
		}

		public void Close()
		{
			throw new NotImplementedException();
		}

		public DataTable GetSchemaTable()
		{
			throw new NotImplementedException();
		}

		public bool NextResult()
		{
			throw new NotImplementedException();
		}

		public int RecordsAffected
		{
			get { throw new NotImplementedException(); }
		}

		public string GetName(int i)
		{
			throw new NotImplementedException();
		}

		public string GetDataTypeName(int i)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}