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
	public partial class DeckBuilder : Form
	{
		private const String m_cardDataDir = "CardData\\";
		private Dictionary<String, int> m_setStartIdList;
		private Dictionary<String, int> m_cardSetNumList;

		private WebLibrary m_WebLibrary;
		private Dictionary<String, CardData> m_CardList;

		public DeckBuilder()
		{
			InitializeComponent();

			// need change this file read later.
			m_setStartIdList = new Dictionary<String, int>{ { "ixalan", 436647 }, { "rivals of ixalan", 440876 }, { "dominaria", 444503 },
														   { "core set 2019", 448823 }, { "guild of ravnica", 454305 }, { "ravnica allegiance", 458699 } };

			m_cardSetNumList = new Dictionary<String, int>{ { "ixalan", 284 }, { "rivals of ixalan", 212 }, { "dominaria", 265 },
														    { "core set 2019", 280 }, { "guild of ravnica", 288 }, { "ravnica allegiance", 270 } };

			if (Directory.Exists(m_cardDataDir) == false)
				Directory.CreateDirectory(m_cardDataDir);

			m_CardList = new Dictionary<String, CardData>();
			m_WebLibrary = new WebLibrary();
		}

		public void CrawlingCard()
		{
			int cardNum = m_cardSetNumList["core set 2019"];
			int startCardId = m_setStartIdList["core set 2019"];
			for (int i = 0; i < cardNum; ++i)
			{
				int cardID = startCardId + i;
				String strCardID = cardID.ToString();
				String url = m_WebLibrary.MakeURL(strCardID);

				HtmlDocument doc = m_WebLibrary.GetHTMLDocumentByURL(url);
				CardData card = m_WebLibrary.MakeCardData(doc, strCardID);

				if (m_CardList.ContainsKey(card.GetCardName()) == false)
					m_CardList.Add(card.GetCardName(), card);

				double progressRate = (double)m_CardList.Count / (double)m_cardSetNumList["core set 2019"];
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
	}
}
