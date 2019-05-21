using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace DeckBuilder
{
	public partial class DeckBuilder : Form
	{
		public void SaveCardData()
		{
			if (Directory.Exists(m_cardDataDir) == false)
				Directory.CreateDirectory(m_cardDataDir);

			foreach (var cardList in m_CardList)
			{
				String expansion = cardList.Key.ToString();
				StringBuilder filePath = new StringBuilder(m_cardDataDir);
				filePath.Append(expansion);
				filePath.Append(".xml");

				XmlTextWriter writer = new XmlTextWriter(filePath.ToString(), Encoding.UTF8);
				writer.Formatting = Formatting.Indented;
				writer.WriteStartDocument();
				writer.WriteStartElement("CardList");

				foreach (var card in cardList.Value)
				{
					Dictionary<ManaType, int> cost = card.Value.GetManaCost();
					StringBuilder strMana = new StringBuilder();
					for (ManaType type = ManaType.COMMON; type < ManaType.MANA_TYPE_MAX; ++type)
					{
						if (cost.ContainsKey(type))
							strMana.Append(cost[type].ToString());
						else
							strMana.Append("0");
					}

					List<String> typeList = card.Value.GetCardTypes();
					StringBuilder strTypes = new StringBuilder();
					foreach (var type in typeList)
					{
						strTypes.Append(type);
						if(typeList.Last() != type)
							strTypes.Append(" ");
					}

					writer.WriteStartElement("Card");
					writer.WriteAttributeString("ID", card.Value.GetCardID());
					writer.WriteAttributeString("Name", card.Value.GetCardName());
					writer.WriteAttributeString("ManaCost", strMana.ToString());
					writer.WriteAttributeString("CMC", card.Value.GetCMC().ToString());
					writer.WriteAttributeString("Type", strTypes.ToString());
					writer.WriteAttributeString("Text", card.Value.GetText());
					writer.WriteAttributeString("Expansion", card.Value.GetCardSet());
					writer.WriteAttributeString("Rarity", card.Value.GetRarity().ToString());
					writer.WriteAttributeString("ImagePath", card.Value.GetImagePath());
					writer.WriteEndElement();
				}

				writer.WriteEndElement();
				writer.WriteEndDocument();
				writer.Flush();
				writer.Close();
			}
		}

		public void SaveDeckList(String filePath)
		{
			XmlTextWriter writer = new XmlTextWriter(filePath.ToString(), Encoding.UTF8);
			writer.Formatting = Formatting.Indented;
			writer.WriteStartDocument();
			writer.WriteStartElement("DeckList");

			foreach (var cardData in m_DeckList)
			{
				writer.WriteStartElement("Card");
				writer.WriteAttributeString("Name", cardData.Key);
				writer.WriteAttributeString("Expansion", cardData.Value.GetCardData().GetCardSet());
				writer.WriteAttributeString("Num", cardData.Value.GetCardNum().ToString());
				writer.WriteEndElement();
			}

			writer.WriteEndElement();
			writer.WriteEndDocument();
			writer.Flush();
			writer.Close();
		}

		public void LoadCardData()
		{
			if (Directory.Exists(m_cardDataDir) == false)
				Directory.CreateDirectory(m_cardDataDir);

			for (eExpansion expansion = eExpansion.XLN; expansion != eExpansion.EXPANSION_MAX; expansion++)
			{
				StringBuilder filePath = new StringBuilder(m_cardDataDir);
				filePath.Append(expansion.ToString());
				filePath.Append(".xml");

				if (File.Exists(filePath.ToString()) == false)
					continue;

				XmlDocument doc = new XmlDocument();
				doc.Load(filePath.ToString());

				StringBuilder value = new StringBuilder();
				foreach (XmlNode node in doc.DocumentElement.ChildNodes)
				{
					CardData cardData = new CardData();
					cardData.SetCardSet(expansion.ToString());

					value.Append(node.Attributes["ID"].Value);
					cardData.SetCardID(value.ToString());
					value.Clear();

					value.Append(node.Attributes["Name"].Value);
					cardData.SetCardName(value.ToString());
					value.Clear();

					value.Append(node.Attributes["ManaCost"].Value);
					cardData.SetManaCost(value.ToString());
					value.Clear();

					value.Append(node.Attributes["CMC"].Value);
					cardData.SetCMC(value.ToString());
					value.Clear();

					value.Append(node.Attributes["Type"].Value);
					cardData.SetType(value.ToString());
					value.Clear();

					value.Append(node.Attributes["Text"].Value);
					cardData.SetText(value.ToString());
					value.Clear();

					value.Append(node.Attributes["Rarity"].Value);
					cardData.SetRarity(value.ToString());
					value.Clear();

					value.Append(node.Attributes["ImagePath"].Value);
					cardData.SetImagePath(value.ToString());
					value.Clear();

					m_CardList[expansion].Add(cardData.GetCardName(), cardData);
				}
			}
		}

		public void LoadDeckList(String filePath)
		{
			if (File.Exists(filePath) == false)
				return;

			XmlDocument doc = new XmlDocument();
			doc.Load(filePath.ToString());

			foreach (XmlNode node in doc.DocumentElement.ChildNodes)
			{
				String name = node.Attributes["Name"].Value;
				String expansionStr = node.Attributes["Expansion"].Value;
				String numStr = node.Attributes["Num"].Value;

				eExpansion expansion = GetExpansionEnumFromString(expansionStr);

				DeckCardData cardData = new DeckCardData();
				cardData.SetCardNum(Int32.Parse(numStr));
				cardData.SetCardData(m_CardList[expansion][name]);

				m_DeckList.Add(name, cardData);
			}
		}

		public bool LoadSettings()
		{
			StringBuilder path = new StringBuilder("CardData\\");
			if (Directory.Exists(path.ToString()) == false)
				Directory.CreateDirectory(path.ToString());

			path.Append("Settings.xml");
			if (File.Exists(path.ToString()) == false)
				return false;

			XmlDocument doc = new XmlDocument();
			doc.Load(path.ToString());

			foreach (XmlNode node in doc.DocumentElement.ChildNodes)
			{
				if(node.Name == "CardDataDir")
				{
					m_cardDataDir = node.InnerText;
				}
				else if(node.Name == "CardImageDir")
				{
					m_cardImageDir = node.InnerText;
				}
				else if (node.Name == "SetStartID")
				{
					for (eExpansion expansion = eExpansion.XLN; expansion != eExpansion.EXPANSION_MAX; expansion++)
					{
						String data = node.Attributes[expansion.ToString()].Value;
						m_setStartIdList.Add(expansion, Int32.Parse(data));
					}
				}
				else if (node.Name == "SetCardNum")
				{
					for (eExpansion expansion = eExpansion.XLN; expansion != eExpansion.EXPANSION_MAX; expansion++)
					{
						String data = node.Attributes[expansion.ToString()].Value;
						m_cardSetNumList.Add(expansion, Int32.Parse(data));
					}
				}
			}

			return true;
		}
	}
}
