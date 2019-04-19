﻿using System;
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

			foreach (KeyValuePair<eExpansion, Dictionary<String, CardData>> cardList in m_CardList)
			{
				String expansion = cardList.Key.ToString();
				StringBuilder filePath = new StringBuilder(m_cardDataDir);
				filePath.Append(expansion);
				filePath.Append(".xml");

				XmlTextWriter writer = new XmlTextWriter(filePath.ToString(), Encoding.UTF8);
				writer.Formatting = Formatting.Indented;
				writer.WriteStartDocument();
				writer.WriteStartElement("CardList");

				foreach (KeyValuePair<String, CardData> card in cardList.Value)
				{
					// todo. CardData 내부에서만 array로 관리하고 해당 array가 밖으로 나오지 않도록 하자.
					int[] cost = card.Value.GetManaCost();
					StringBuilder strMana = new StringBuilder();
					for (ManaType mana = 0; mana < ManaType.MANA_TYPE_MAX; mana++)
						strMana.Append(cost[(int)mana].ToString());

					List<CardType> typeList = card.Value.GetCardTypes();
					StringBuilder strTypes = new StringBuilder();
					foreach(CardType type in typeList)
						strTypes.Append(((int)type).ToString());

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

		public void LoadCardData()
		{
			if (Directory.Exists(m_cardDataDir) == false)
				Directory.CreateDirectory(m_cardDataDir);

			for (eExpansion expansion = eExpansion.XLN; expansion != eExpansion.EXPANSION_MAX; expansion++)
			{
				StringBuilder filePath = new StringBuilder(m_cardDataDir);
				filePath.Append(GetFullNameFromExpansionEnum(expansion));
				filePath.Append(".xml");

				if (File.Exists(filePath.ToString()) == false)
					continue;

				XmlDocument doc = new XmlDocument();
				doc.Load(filePath.ToString());

				foreach (XmlNode node in doc.DocumentElement.ChildNodes)
				{
					CardData cardData = new CardData();
					cardData.SetCardSet(expansion.ToString());
					StringBuilder value = new StringBuilder();

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
					List<String> typeList = new List<string>();
					for (int i = 0; i < value.Length; ++i)
						typeList.Add(value.ToString().Substring(i, 1));
					cardData.SetType(typeList);
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