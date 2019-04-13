using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeckBuilder
{
	class WebLibrary : IDisposable
	{
		private enum eCardElement
		{
			ELEMENT_NONE = 0,
			ELEMENT_CARD_NAME,
			ELEMENT_COST,
			ELEMENT_CMC,
			ELEMENT_TYPE,
			ELEMENT_TEXT,
			ELEMENT_EXPANSION,
			ELEMENT_RARITY,
		}

		private const String m_imageDir = "CardData\\Images\\";

		public String MakeURL(string cardID)
		{
			StringBuilder urlStr = new StringBuilder();
			urlStr.Clear();
			urlStr.Append("http://gatherer.wizards.com/Pages/Card/Details.aspx?printed=true&multiverseid=");
			urlStr.Append(cardID);

			return urlStr.ToString();
		}

		public HtmlDocument GetHTMLDocumentByURL(String url)
		{
			HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(url);
			HttpWebResponse webResp = (HttpWebResponse)webReq.GetResponse();

			TextReader reader = (TextReader)new StreamReader(webResp.GetResponseStream());
			String html = reader.ReadToEnd();
			HtmlDocument doc = GetHTMLDocumentByHTML(html);

			reader.Close();
			webResp.Close();

			return doc;
		}

		public HtmlDocument GetHTMLDocumentByHTML(String html)
		{
			using (WebBrowser browser = new WebBrowser())
			{
				browser.ScriptErrorsSuppressed = true;
				browser.DocumentText = html;
				browser.Document.OpenNew(true);
				browser.Document.Write(html);
				browser.Refresh();

				return browser.Document;
			}
		}

		public CardData MakeCardData(HtmlDocument document, String cardID)
		{
			if (Directory.Exists(m_imageDir) == false)
				Directory.CreateDirectory(m_imageDir);

			CardData card = new CardData();
			card.SetCardID(cardID);

			GetCardText(in document, ref card);
			DownLoadCardImage(ref card);
			
			return card;
		}

		private bool GetCardText(in HtmlDocument document, ref CardData card)
		{
			eCardElement elementType = eCardElement.ELEMENT_NONE;

			HtmlElementCollection dataList = document.GetElementsByTagName("div");
			foreach (HtmlElement cardElement in dataList)
			{
				if (elementType == eCardElement.ELEMENT_CARD_NAME)
					card.SetCardName(cardElement.InnerText);
				else if (elementType == eCardElement.ELEMENT_CMC)
					card.SetCMC(cardElement.InnerText);
				else if (elementType == eCardElement.ELEMENT_TYPE)
					card.SetType(cardElement.InnerText);
				else if (elementType == eCardElement.ELEMENT_TEXT)
					card.SetText(cardElement.InnerText);
				else if (elementType == eCardElement.ELEMENT_EXPANSION)
					card.SetCardSet(cardElement.InnerText);
				else if (elementType == eCardElement.ELEMENT_RARITY)
				{
					card.SetRarity(cardElement.InnerText);
					break;
				}
				else if (elementType == eCardElement.ELEMENT_COST)
				{
					string text = cardElement.InnerHtml;
					string[] textList = text.Split('"');

					bool isCost = false;
					List<string> manaCostList = new List<string>();
					foreach (string temp in textList)
					{
						if (isCost == true)
						{
							manaCostList.Add(temp);
							isCost = false;
						}

						if (temp.ToLower() == " alt=")
							isCost = true;
					}

					if (0 < manaCostList.Count)
						card.SetManaCost(manaCostList);

					elementType = eCardElement.ELEMENT_NONE;
				}

				elementType = eCardElement.ELEMENT_NONE;
				if (cardElement.InnerText == "Card Name:")
					elementType = eCardElement.ELEMENT_CARD_NAME;
				else if (cardElement.InnerText == "Mana Cost:")
					elementType = eCardElement.ELEMENT_COST;
				else if (cardElement.InnerText == "Converted Mana Cost:")
					elementType = eCardElement.ELEMENT_CMC;
				else if (cardElement.InnerText == "Types:")
					elementType = eCardElement.ELEMENT_TYPE;
				else if (cardElement.InnerText == "Card Text:")
					elementType = eCardElement.ELEMENT_TEXT;
				else if (cardElement.InnerText == "Expansion:")
					elementType = eCardElement.ELEMENT_EXPANSION;
				else if (cardElement.InnerText == "Rarity:")
					elementType = eCardElement.ELEMENT_RARITY;
			}

			return true;
		}

		private bool GetCardImage(in HtmlDocument document, ref CardData card)
		{
			DownLoadCardImage(ref card);

			return true;
		}

		private void DownLoadCardImage(ref CardData card)
		{
			StringBuilder url = new StringBuilder("https://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=");
			url.Append(card.GetCardID());
			url.Append("&type=card");

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url.ToString());
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			bool bImage = response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase);
			if ((response.StatusCode == HttpStatusCode.OK ||
				response.StatusCode == HttpStatusCode.Moved ||
				response.StatusCode == HttpStatusCode.Redirect) &&
				bImage)
			{
				card.SetImagePath(m_imageDir + card.GetCardName() + ".jpeg");

				Stream inputStream = response.GetResponseStream();
				Stream outputStream = File.OpenWrite(card.GetImagePath());

				byte[] buffer = new byte[4096];
				int bytesRead;
				do
				{
					bytesRead = inputStream.Read(buffer, 0, buffer.Length);
					outputStream.Write(buffer, 0, bytesRead);
				} while (bytesRead != 0);

				inputStream.Close();
				outputStream.Close();
			}

			response.Close();
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
