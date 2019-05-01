using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeckBuilder
{
	public enum ManaType
	{
		MANA_TYPE_COMMON = 0,
		MANA_TYPE_WHITE,
		MANA_TYPE_RED,
		MANA_TYPE_BLUE,
		MANA_TYPE_GREEN,
		MANA_TYPE_BLACK,

		MANA_TYPE_MAX,
	}

	public enum Rarity
	{
		RARITY_NONE = 0,

		COMMON,
		UNCOMMON,
		RARE,
		MITHIC_RARE,

		RARITY_MAX,
	}

	public class CardData
	{
		private String cardID;
		private String cardName;
		// todo. split mana 고려 필요.
		private int[] manaCost;
		private int convertedManaCost;
		private List<String> types;
		private String text;
		private String cardSet;
		private Rarity rarity;
		private String imagePath;

		// todo. 양면 카드 고려 필요.
		// for 양면카드 or 스플릿 카드
		private bool isSubCardExist;
		private CardData subCardData;

		public CardData()
		{
			cardID = "";
			cardName = "";
			manaCost = new int[(int)ManaType.MANA_TYPE_MAX];
			convertedManaCost = 0;
			types = new List<String>();
			text = "";
			cardSet = "";
			rarity = Rarity.RARITY_NONE;
			imagePath = "";
			isSubCardExist = false;
		}

		~CardData()
		{
			
		}

		public void SetCardID(String id) { cardID = id; }
		public String GetCardID() { return cardID; }

		public void SetCardName(String name) { cardName = name; }
		public String GetCardName() { return cardName; }

		public void SetManaCost(List<String> costList) { manaCost = ConvertStringListToManaCost(costList); }
		public void SetManaCost(String costList) { manaCost = ConvertStringListToManaCost(costList); }
		public int[] GetManaCost() { return manaCost; }

		public void SetCMC(String cmc) { convertedManaCost = Int32.Parse(cmc); }
		public int GetCMC() { return convertedManaCost; }

		public void SetType(String cardType) { types = ConvertStringToCardType(cardType); }
		public void SetType(List<String> cardTypes) { types = cardTypes; }
		public List<String> GetCardTypes() { return types; }
		public bool IsCardType(String cardType) { return types.Contains(cardType); }

		public void SetText(String cardText) { text = cardText; }
		public String GetText() { return text; }

		public void SetCardSet(String expansion) { cardSet = expansion; }
		public String GetCardSet() { return cardSet; }
		public bool IsIncludeSet(String setName) { return setName == cardSet; }

		public void SetRarity(String cardRarity) { rarity = ConvertStringToRarity(cardRarity); }
		public Rarity GetRarity() { return rarity; }

		public void SetImagePath(String path) { imagePath = path; }
		public String GetImagePath() { return imagePath; }

		// todo. 각종 convert함수들 class외부로 빼면 좋겠다
		private List<String> ConvertStringToCardType(String typeStr)
		{
			List<String> typeList = new List<string>();
			String seperator = " -";
			String[] tokStr = typeStr.Split(seperator.ToCharArray());
			foreach (String str in tokStr)
				typeList.Add(str);
			
			return typeList;
		}

		private Rarity ConvertStringToRarity(String cardRarity)
		{
			if (cardRarity.ToLower() == "common")
				return Rarity.COMMON;
			else if (cardRarity.ToLower() == "uncommon")
				return Rarity.UNCOMMON;
			else if (cardRarity.ToLower() == "rare")
				return Rarity.RARE;
			else if (cardRarity.ToLower() == "mithic rare")
				return Rarity.MITHIC_RARE;
			else
				return Rarity.RARITY_NONE;
		}

		private int[] ConvertStringListToManaCost(List<String> costList)
		{
			int[] manaCost = new int[(int)ManaType.MANA_TYPE_MAX];

			foreach(String cost in costList)
			{
				if (cost.ToLower() == "white")
					manaCost[(int)ManaType.MANA_TYPE_WHITE]++;
				else if (cost.ToLower() == "red")
					manaCost[(int)ManaType.MANA_TYPE_RED]++;
				else if (cost.ToLower() == "blue")
					manaCost[(int)ManaType.MANA_TYPE_BLUE]++;
				else if (cost.ToLower() == "green")
					manaCost[(int)ManaType.MANA_TYPE_GREEN]++;
				else if (cost.ToLower() == "black")
					manaCost[(int)ManaType.MANA_TYPE_BLACK]++;
				else
					manaCost[(int)ManaType.MANA_TYPE_COMMON] = Int32.Parse(cost);
			}

			return manaCost;
		}

		private int[] ConvertStringListToManaCost(String strCost)
		{
			int[] manaCost = new int[(int)ManaType.MANA_TYPE_MAX];

			for(ManaType mana = ManaType.MANA_TYPE_COMMON; mana < ManaType.MANA_TYPE_MAX; mana++)
				manaCost[(int)mana] = Int32.Parse(strCost.Substring((int)mana, 1));

			return manaCost;
		}
	}
}
