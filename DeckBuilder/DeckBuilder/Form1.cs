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
		private String m_cardDataDir;
		private String m_cardImageDir;
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

			// initialize member variables
			m_setStartIdList = new Dictionary<eExpansion, int>();
			m_cardSetNumList = new Dictionary<eExpansion, int>();
			m_WebLibrary = new WebLibrary();
			m_CardList = new Dictionary<eExpansion, Dictionary<string, CardData>>();
			for (eExpansion expansion = eExpansion.IXALAN; expansion < eExpansion.EXPANSION_MAX; ++expansion)
			{
				Dictionary<string, CardData> cardList = new Dictionary<string, CardData>();
				m_CardList.Add(expansion, cardList);
			}

			// load settings & card data
			LoadSettings();
			LoadCardData();
			RefreshCardList();

			SetExpansionComboBox();
		}

		public void SetExpansionComboBox()
		{
			for(eExpansion expansion = eExpansion.IXALAN; expansion != eExpansion.EXPANSION_MAX; expansion++)
				ExpansionComboBox.Items.Add(GetStringFromExpansionEnum(expansion));
			
			ExpansionComboBox.SelectedIndex = 0;
		}

		public void CrawlingCard()
		{
			String expansionName = ExpansionComboBox.SelectedItem as String;
			eExpansion expansion = GetExpansionEnumFromString(expansionName);
			StringBuilder imagePath = new StringBuilder(m_cardImageDir);
			imagePath.Append(expansionName);
			imagePath.Append("\\");

			int startCardId = m_setStartIdList[expansion];
			for (int i = 0; i < m_cardSetNumList[expansion]; ++i)
			{
				int cardID = startCardId + i;
				String strCardID = cardID.ToString();
				String url = m_WebLibrary.MakeURL(strCardID);

				HtmlDocument doc = m_WebLibrary.GetHTMLDocumentByURL(url);
				CardData card = m_WebLibrary.MakeCardData(doc, imagePath.ToString(), strCardID);

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
			RefreshCardList();
		}

		private void RefreshCardList()
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

		private void DeckBuilder_FormClosed(object sender, FormClosedEventArgs e)
		{
			SaveCardData();
		}
	}
}
