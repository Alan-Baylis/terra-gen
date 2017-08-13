using System;
using System.Xml;

namespace TerrainBrowser
{
	class TerrainData
	{
		public TerrainData()
		{
		}
	}

	class TerrainDataItem
	{
		public TerrainDataItem(XmlNode node)
		{
			_node = node;
		}



		private XmlNode _node;
	}
}
