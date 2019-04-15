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
		private const String m_cardDataDir = "CardData\\";
		private Dictionary<eExpansion, int> m_setStartIdList;
		private Dictionary<eExpansion, int> m_cardSetNumList;

		private WebLibrary m_WebLibrary;
		private Dictionary<String, CardData> m_CardList;

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

			m_CardList = new Dictionary<String, CardData>();
			m_WebLibrary = new WebLibrary();

			//for test
			/*String name = "가시 장교";
			CardData data = new CardData();
			data.SetImagePath("CardData\\Images\\가시 장교.jpeg");
			m_CardList.Add(name, data);
			CardListBox.Items.Add(name);*/

			SetExpansionComboBox();
		}

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

		public void SetExpansionComboBox()
		{
			ExpansionComboBox.Items.Add("Ixalan");
			ExpansionComboBox.Items.Add("Rivals of Ixalan");
			ExpansionComboBox.Items.Add("Dominaria");
			ExpansionComboBox.Items.Add("Core Set 2019");
			ExpansionComboBox.Items.Add("Guild of Ravnica");
			ExpansionComboBox.Items.Add("Ravnica Allegiance");

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

				if (m_CardList.ContainsKey(card.GetCardName()) == false)
					m_CardList.Add(card.GetCardName(), card);

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
			foreach (KeyValuePair<String, CardData> card in m_CardList)
			{
				CardListBox.Items.Add(card.Key);
			}
		}

		private void CardListBox_ItemSelected(object sender, EventArgs e)
		{
			// C#에서 casting 방법이 있을듯?
			String name = CardListBox.SelectedItem as String;
			ListCardImg.Image = Image.FromFile(m_CardList[name].GetImagePath());
		}
	}
}
