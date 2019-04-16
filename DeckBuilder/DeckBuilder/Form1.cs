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
	enum eExpansion
	{
		IXALAN = 0,
		RIVALS_OF_IXALAN,
		DOMINARIA,
		CORE_SET_2019,
		GUILD_OF_RAVNICA,
		RAVNICA_ALLEGIANCE,

		EXPANSION_MAX,
	}

	public partial class DeckBuilder : Form
	{
		private const String m_cardDataDir = "CardData\\Text\\";
		private const String m_cardImageDir = "CardData\\Image\\";
		private Dictionary<eExpansion, int> m_setStartIdList;
		private Dictionary<eExpansion, int> m_cardSetNumList;

		private WebLibrary m_WebLibrary;
		private Dictionary<eExpansion, Dictionary<String, CardData>> m_CardList;

		private String GetStringFromExpansionEnum(eExpansion expansion)
		{
			switch (expansion)
			{
				case eExpansion.IXALAN: return "Ixalan";
				case eExpansion.RIVALS_OF_IXALAN: return "Rivals of Ixalan";
				case eExpansion.DOMINARIA: return "Dominaria";
				case eExpansion.CORE_SET_2019: return "Core Set 2019";
				case eExpansion.GUILD_OF_RAVNICA: return "Guild of Ravnica";
				case eExpansion.RAVNICA_ALLEGIANCE: return "Ravnica Allegiance";
				default: return "";
			}
		}

		private eExpansion GetExpansionEnumFromString(String expansionName)
		{
			if (expansionName == "Ixalan")
				return eExpansion.IXALAN;
			else if (expansionName == "Rivals of Ixalan")
				return eExpansion.RIVALS_OF_IXALAN;
			else if (expansionName == "Dominaria")
				return eExpansion.DOMINARIA;
			else if (expansionName == "Core Set 2019")
				return eExpansion.CORE_SET_2019;
			else if (expansionName == "Guild of Ravnica")
				return eExpansion.GUILD_OF_RAVNICA;
			else if (expansionName == "Ravnica Allegiance")
				return eExpansion.RAVNICA_ALLEGIANCE;

			return eExpansion.EXPANSION_MAX;
		}

		public DeckBuilder()
		{
			InitializeComponent();

			// need change this file read later.
			m_setStartIdList = new Dictionary<eExpansion, int>{ { eExpansion.IXALAN, 436647 }, { eExpansion.RIVALS_OF_IXALAN, 440876 }, { eExpansion.DOMINARIA, 444503 },
														   { eExpansion.CORE_SET_2019, 448823 }, { eExpansion.GUILD_OF_RAVNICA, 454305 }, { eExpansion.RAVNICA_ALLEGIANCE, 458699 } };

			m_cardSetNumList = new Dictionary<eExpansion, int>{ { eExpansion.IXALAN, 284 }, { eExpansion.RIVALS_OF_IXALAN, 212 }, { eExpansion.DOMINARIA, 265 },
														    { eExpansion.CORE_SET_2019, 280 }, { eExpansion.GUILD_OF_RAVNICA, 288 }, { eExpansion.RAVNICA_ALLEGIANCE, 270 } };

			if (Directory.Exists(m_cardDataDir) == false)
				Directory.CreateDirectory(m_cardDataDir);

			m_WebLibrary = new WebLibrary();
			m_CardList = new Dictionary<eExpansion, Dictionary<string, CardData>>();
			for (eExpansion expansion = eExpansion.IXALAN; expansion < eExpansion.EXPANSION_MAX; ++expansion)
			{
				Dictionary<string, CardData> cardList = new Dictionary<string, CardData>();
				m_CardList.Add(expansion, cardList);
			}

			//for test
			String name = "아단토 선봉대";
			CardData data = new CardData();
			data.SetCardID("436647");
			data.SetCardName(name);
			data.SetCardSet("Ixalan");
			data.SetCMC("2");
			data.SetRarity("uncommon");
			data.SetText("아단토 선봉대가 공격 중인 한 아단토 선봉대는 +2/+0을 받는다. 생명 4점을 지불한다: 아단토 선봉대는 턴종료까지 무적을 얻는다.");
			data.SetType("creature");
			data.SetImagePath("CardData\\Images\\아단토 선봉대.jpeg");

			List<String> cost = new List<string>();
			cost.Add("1");
			cost.Add("white");
			data.SetManaCost(cost);

			m_CardList[eExpansion.IXALAN].Add(name, data);
			SaveCardData();

			//CardListBox.Items.Add(name);

			//LoadCardData();

			SetExpansionComboBox();
		}

		public void SaveCardData()
		{
			if (Directory.Exists(m_cardDataDir) == false)
				Directory.CreateDirectory(m_cardDataDir);

			foreach (KeyValuePair<eExpansion, Dictionary<String, CardData>> cardList in m_CardList)
			{
				String expansion = GetStringFromExpansionEnum(cardList.Key);
				StringBuilder filePath = new StringBuilder(m_cardDataDir);
				filePath.Append(expansion);
				filePath.Append(".xml");

				XmlTextWriter writer = new XmlTextWriter(filePath.ToString(), Encoding.UTF8);
				writer.Formatting = Formatting.Indented;
				writer.WriteStartDocument();
				writer.WriteStartElement("CardList");

				foreach (KeyValuePair<String, CardData> card in cardList.Value)
				{
					int[] cost = card.Value.GetManaCost();
					StringBuilder strMana = new StringBuilder();
					for (ManaType mana = 0; mana < ManaType.MANA_TYPE_MAX; mana++)
						strMana.Append(cost[(int)mana].ToString());

					bool[] types = card.Value.GetCardType();
					StringBuilder strType = new StringBuilder();
					for (CardType type = 0; type < CardType.CARD_TYPE_MAX; type++)
					{
						if (types[(int)type] == true)
							strType.Append("1");
						else
							strType.Append("0");
					}

					writer.WriteStartElement("Card");
					writer.WriteAttributeString("ID", card.Value.GetCardID());
					writer.WriteAttributeString("Name", card.Value.GetCardName());
					writer.WriteAttributeString("ManaCost", strMana.ToString());
					writer.WriteAttributeString("CMC", card.Value.GetCMC().ToString());
					writer.WriteAttributeString("Type", strType.ToString());
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
			StringBuilder dir = new StringBuilder(m_cardDataDir);
			dir.Append("Text\\");
			if (Directory.Exists(dir.ToString()) == false)
				Directory.CreateDirectory(dir.ToString());

			for (eExpansion expansion = eExpansion.IXALAN; expansion != eExpansion.EXPANSION_MAX; expansion++)
			{
				StringBuilder filePath = new StringBuilder(dir.ToString());
				filePath.Append(GetStringFromExpansionEnum(expansion));
				filePath.Append(".xml");

				if (File.Exists(filePath.ToString()) == false)
					continue;

				XmlDocument doc = new XmlDocument();
				doc.Load(filePath.ToString());

				foreach(XmlNode node in doc.DocumentElement.ChildNodes)
				{
					String data = node.InnerText;
				}
			}
		}

		public void SetExpansionComboBox()
		{
			for(eExpansion expansion = eExpansion.IXALAN; expansion != eExpansion.EXPANSION_MAX; expansion++)
				ExpansionComboBox.Items.Add(GetStringFromExpansionEnum(expansion));
			
			ExpansionComboBox.SelectedIndex = 0;
		}

		public void CrawlingCard()
		{
			String expansionName = ExpansionComboBox.SelectedText;
			eExpansion expansion = GetExpansionEnumFromString(expansionName);

			int cardNum = m_cardSetNumList[expansion];
			int startCardId = m_setStartIdList[expansion];
			for (int i = 0; i < cardNum; ++i)
			{
				int cardID = startCardId + i;
				String strCardID = cardID.ToString();
				String url = m_WebLibrary.MakeURL(strCardID);

				HtmlDocument doc = m_WebLibrary.GetHTMLDocumentByURL(url);
				CardData card = m_WebLibrary.MakeCardData(doc, strCardID);

				if (m_CardList[expansion].ContainsKey(card.GetCardName()) == false)
					m_CardList[expansion].Add(card.GetCardName(), card);

				double progressRate = (double)m_CardList.Count / (double)m_cardSetNumList[expansion];
				CardDataProgressBar.Value = (int)(progressRate * 100);
			}
		}

		private void CrawlingCardBtn_Click(object sender, EventArgs e)
		{
			CrawlingCard();
		}

		private void RefreshListBtn_Click(object sender, EventArgs e)
		{
			foreach (KeyValuePair<eExpansion, Dictionary<String, CardData>> cardList in m_CardList)
			{
				foreach (KeyValuePair<String, CardData> card in cardList.Value)
				{
					CardListBox.Items.Add(card.Key);
				}
			}
		}

		private void CardListBox_ItemSelected(object sender, EventArgs e)
		{
			String imagePath = "";
			String name = CardListBox.SelectedItem as String;
			foreach (KeyValuePair<eExpansion, Dictionary<String, CardData>> cardList in m_CardList)
				if(cardList.Value.ContainsKey(name))
					imagePath = cardList.Value[name].GetImagePath();

			ListCardImg.Image = Image.FromFile(imagePath);
		}
	}
}
