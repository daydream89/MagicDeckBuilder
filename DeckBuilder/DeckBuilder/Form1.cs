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
		XLN = 0,
		RIX,
		DOM,
		M19,
		GRN,
		RNA,

		EXPANSION_MAX,
	}

	public struct DeckCardData
	{
		private CardData cardData;
		private int num;

		public void SetCardData(CardData data) { cardData = data; }
		public CardData GetCardData() { return cardData; }

		public void SetCardNum(int n) { num = n; }
		public int GetCardNum() { return num; }
	}

	public partial class DeckBuilder : Form
	{
		const int MAX_CARD_NUM = 4;

		private String m_cardDataDir;
		private String m_cardImageDir;
		private Dictionary<eExpansion, int> m_setStartIdList;
		private Dictionary<eExpansion, int> m_cardSetNumList;

		private WebLibrary m_WebLibrary;
		private Dictionary<eExpansion, Dictionary<String, CardData>> m_CardList;
		private Dictionary<String, DeckCardData> m_DeckList;
		private CardData m_curSelectedCard;

		private String GetFullNameFromExpansionEnum(eExpansion expansion)
		{
			switch (expansion)
			{
				case eExpansion.XLN: return "Ixalan";
				case eExpansion.RIX: return "Rivals of Ixalan";
				case eExpansion.DOM: return "Dominaria";
				case eExpansion.M19: return "Core Set 2019";
				case eExpansion.GRN: return "Guild of Ravnica";
				case eExpansion.RNA: return "Ravnica Allegiance";
				default: return "";
			}
		}

		private eExpansion GetExpansionEnumFromString(String expansionName)
		{
			if (expansionName == "Ixalan" || expansionName == "XLN")
				return eExpansion.XLN;
			else if (expansionName == "Rivals of Ixalan" || expansionName == "RIX")
				return eExpansion.RIX;
			else if (expansionName == "Dominaria" || expansionName == "DOM")
				return eExpansion.DOM;
			else if (expansionName == "Core Set 2019" || expansionName == "M19")
				return eExpansion.M19;
			else if (expansionName == "Guild of Ravnica" || expansionName == "GRN")
				return eExpansion.GRN;
			else if (expansionName == "Ravnica Allegiance" || expansionName == "RNA")
				return eExpansion.RNA;

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
			for (eExpansion expansion = eExpansion.XLN; expansion < eExpansion.EXPANSION_MAX; ++expansion)
			{
				Dictionary<string, CardData> cardList = new Dictionary<string, CardData>();
				m_CardList.Add(expansion, cardList);
			}

			m_DeckList = new Dictionary<string, DeckCardData>();

			// load settings & card data
			LoadSettings();
			LoadCardData();
			RefreshCardList();

			SetExpansionComboBox();
			SetCardNumComboBox();
		}

		private void SetExpansionComboBox()
		{
			for(eExpansion expansion = eExpansion.XLN; expansion != eExpansion.EXPANSION_MAX; expansion++)
				ExpansionComboBox.Items.Add(GetFullNameFromExpansionEnum(expansion));
			
			ExpansionComboBox.SelectedIndex = 0;
		}

		private void SetCardNumComboBox()
		{
			AddCardNumComboBox.Items.Add("수량");
			RemoveCardNumComboBox.Items.Add("수량");
			for (int i = 0; i < MAX_CARD_NUM; ++i)
			{
				AddCardNumComboBox.Items.Add(i+1);
				RemoveCardNumComboBox.Items.Add(i+1);
			}

			AddCardNumComboBox.SelectedIndex = 0;
			RemoveCardNumComboBox.SelectedIndex = 0;
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
				{
					if(File.Exists(imagePath.ToString()) == false)
						m_WebLibrary.DownLoadCardImage(imagePath.ToString(), ref card);

					m_CardList[expansion].Add(card.GetCardName(), card);
				}

				double progressRate = (double)m_CardList[expansion].Count / (double)m_cardSetNumList[expansion];
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
			CardListBox.Items.Clear();

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
			String name = CardListBox.SelectedItem as String;
			foreach (KeyValuePair<eExpansion, Dictionary<String, CardData>> cardList in m_CardList)
			{
				if (cardList.Value.ContainsKey(name))
				{
					m_curSelectedCard = cardList.Value[name];
					break;
				}
			}

			ListCardImg.Image = Image.FromFile(m_curSelectedCard.GetImagePath());
		}

		private void DeckBuilder_FormClosed(object sender, FormClosedEventArgs e)
		{
			SaveCardData();
		}

		private void ApplyDeckListBtn_Click(object sender, EventArgs e)
		{
			if (AddCardNumComboBox.SelectedIndex == 0)
				return;

			String cardName = m_curSelectedCard.GetCardName();
			if (m_DeckList.ContainsKey(cardName) == true)
			{
				int cardNum = m_DeckList[cardName].GetCardNum();
				if (cardNum + AddCardNumComboBox.SelectedIndex <= MAX_CARD_NUM)
				{
					DeckCardData deckCard = m_DeckList[cardName];
					deckCard.SetCardNum(cardNum + AddCardNumComboBox.SelectedIndex);
					m_DeckList[cardName] = deckCard;
				}
				else
					return;
			}
			else
			{
				DeckCardData deckCardData = new DeckCardData();
				deckCardData.SetCardData(m_curSelectedCard);
				deckCardData.SetCardNum(AddCardNumComboBox.SelectedIndex);
				m_DeckList.Add(cardName, deckCardData);
			}
			
			RefreshDeckList();
		}

		private void RemoveDeckListCardBtn_Click(object sender, EventArgs e)
		{
			if (DeckList.SelectedItem == null || RemoveCardNumComboBox.SelectedIndex == 0)
				return;

			String text = DeckList.SelectedItem as String;
			String[] tok = text.Split('\t');
			if (tok[1] == null)
				return;

			String cardName = tok[0];
			int cardNum = Int32.Parse(tok[1]);
			if (RemoveCardNumComboBox.SelectedIndex < cardNum)
			{
				if (m_DeckList.ContainsKey(cardName) == true)
				{
					DeckCardData deckCard = m_DeckList[cardName];
					deckCard.SetCardNum(cardNum - RemoveCardNumComboBox.SelectedIndex);
					m_DeckList[cardName] = deckCard;
				}
			}
			else if (RemoveCardNumComboBox.SelectedIndex == cardNum)
				m_DeckList.Remove(cardName);
			else
				return;

			RefreshDeckList();
		}

		private void RefreshDeckList()
		{
			DeckList.Items.Clear();
			foreach (KeyValuePair<String, DeckCardData> cardData in m_DeckList)
				DeckList.Items.Add(cardData.Key + "\t" + cardData.Value.GetCardNum().ToString());
		}
	}
}
