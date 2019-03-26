using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeckBuilder
{
	public partial class DeckBuilder : Form
	{
		private Dictionary<string, int> m_cardSetIdPair;

		public DeckBuilder()
		{
			InitializeComponent();

			// need change this file read later.
			m_cardSetIdPair = new Dictionary<string, int>{ { "ixalan", 436 }, { "rivals of ixalan", 441 }, { "dominaria", 444 },
														   { "core set 2019", 448 }, { "guild of ravnica", 454 }, { "ravnica allegiance", 458 } };

			CrawlingCard();
		}

		public void CrawlingCard()
		{
			string cardID = m_cardSetIdPair["core set 2019"].ToString() + "907";
			Uri uri = WebLibrary.MakeURL(cardID);

			CrawlingBrowser.Navigate(uri.AbsoluteUri);
		}

		private void CrawlingBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			if (e.Url.AbsoluteUri == CrawlingBrowser.Url.AbsoluteUri)
			{
				CardData card = WebLibrary.MakeCardData(CrawlingBrowser.Document);

				//Output(db);
			}
		}
}
}
