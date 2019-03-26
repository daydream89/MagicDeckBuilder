using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeckBuilder
{
	class WebLibrary
	{
		static public Uri MakeURL(string cardID)
		{
			Uri uri = new Uri("http://gatherer.wizards.com/Pages/Card/Details.aspx?printed=true&multiverseid=" + cardID);
			return uri;
		}

		static public CardData MakeCardData(HtmlDocument document)
		{
			// todo. need refactoring with better way
			// why do i using enum?
			bool isCardName = false;
			bool isManaCost = false;
			bool isCMC = false;
			bool isType = false;
			bool isCardText = false;
			bool isExpansion = false;
			bool isRarity = false;

			CardData card = new CardData();

			HtmlElementCollection dataList = document.GetElementsByTagName("div");
			foreach (HtmlElement cardElement in dataList)
			{
				if (isCardName == true)
				{
					card.SetCardName(cardElement.InnerText);
					isCardName = false;
				}
				else if (isManaCost == true)
				{
					string text = cardElement.InnerHtml;
					string[] textList = text.Split('"');

					bool isCost = false;
					List<string> manaCostList = new List<string>();
					foreach(string temp in textList)
					{
						if(isCost == true)
						{
							manaCostList.Add(temp);
							isCost = false;
						}

						if (temp.ToLower() == " alt=")
							isCost = true;
					}

					if (0 < manaCostList.Count)
						card.SetManaCost(manaCostList);

					isManaCost = false;
				}
				else if(isCMC == true)
				{
					card.SetCMC(cardElement.InnerText);
					isCMC = false;
				}
				else if (isType == true)
				{
					card.SetType(cardElement.InnerText);
					isType = false;
				}
				else if (isCardText == true)
				{
					card.SetText(cardElement.InnerText);
					isCardText = false;
				}
				else if (isExpansion == true)
				{
					card.SetCardSet(cardElement.InnerText);
					isExpansion = false;
				}
				else if (isRarity == true)
				{
					card.SetRarity(cardElement.InnerText);
					isRarity = false;
				}

				if (cardElement.InnerText == "Card Name:")
					isCardName = true;
				else if (cardElement.InnerText == "Mana Cost:")
					isManaCost = true;
				else if (cardElement.InnerText == "Converted Mana Cost:")
					isCMC = true;
				else if (cardElement.InnerText == "Types:")
					isType = true;
				else if (cardElement.InnerText == "Card Text:")
					isCardText = true;
				else if (cardElement.InnerText == "Expansion:")
					isExpansion = true;
				else if (cardElement.InnerText == "Rarity:")
					isRarity = true;
			}

			return card;
		}
		/*
		static public void MakeMovieDB(HtmlElement cardHElement, out CardData cardData)
		{
			cardData = new CardData();

			int lineIndex = 0;

			//string text2 = movieHElement.InnerText;
			foreach(HtmlElement item in cardHElement.GetElementsByTagName("div"))
			{
				string innerHtml = item.InnerHtml;
				string innerText = item.InnerText;
			}
			
			foreach (HtmlElement item in movieHElement.GetElementsByTagName("dt"))
			{
				cardData.title = item.InnerText;
			}

			foreach (HtmlElement item2 in movieHElement.GetElementsByTagName("dd"))
			{
				if (lineIndex == 1)
				{
					foreach (HtmlElement item3 in item2.GetElementsByTagName("a"))
					{
						string text = item3.InnerText;
						string href = item3.GetAttribute("href");

						if (href.IndexOf("genre") > -1)
							cardData.genre.Add(text);
						else if (href.IndexOf("nation") > -1)
							cardData.nation.Add(text);
						else if (href.IndexOf("year") > -1)
							cardData.year = text;
					}
				}
				else if (lineIndex == 2)
				{
					string text = item2.InnerText;
					text = text.Replace("감독 : ", string.Empty);
					text = text.Replace("출연 : ", string.Empty);
					text = text.Replace(", ", ",");

					string[] directorAndActor = text.Split('|');
					string[] director = directorAndActor[0].Split(',');
					string[] actor = directorAndActor[1].Split(',');

					cardData.director.AddRange(director);
					cardData.actor.AddRange(actor);
				}
				lineIndex++;
			}
		}
	*/
	}
}
