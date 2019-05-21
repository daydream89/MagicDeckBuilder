using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeckBuilder
{
	public enum ManaType
	{
		COMMON = 0,
		WHITE,
		RED,
		BLUE,
		GREEN,
		BLACK,

		WHITE_RED,
		WHITE_BLUE,
		WHITE_GREEN,
		WHITE_BLACK,
		RED_BLUE,
		RED_GREEN,
		RED_BLACK,
		BLUE_GREEN,
		BLUE_BLACK,
		GREEN_BLACK,

		COMMON_X,

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
		private Dictionary<ManaType, int> manaCost;
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
			manaCost = new Dictionary<ManaType, int>();
			types = new List<String>();
			rarity = Rarity.RARITY_NONE;
			isSubCardExist = false;
		}

		~CardData()
		{
			
		}

		public void SetCardID(String id) { cardID = id; }
		public String GetCardID() { return cardID; }

		public void SetCardName(String name) { cardName = name; }
		public String GetCardName() { return cardName; }

		public void SetManaCost(List<String> costList) { ConvertStringListToManaCost(costList, ref manaCost); }
		public void SetManaCost(String costList) { ConvertStringToManaCost(costList, ref manaCost); }
		public Dictionary<ManaType, int> GetManaCost() { return manaCost; }

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
			foreach (var str in tokStr)
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

		private void ConvertStringListToManaCost(List<String> costList, ref Dictionary<ManaType, int> mana)
		{
			foreach(var str in costList)
			{
				String strLower = str.ToLower();
				int cost = 1;
				ManaType type = ManaType.MANA_TYPE_MAX;
				if (strLower == "white")
					type = ManaType.WHITE;
				else if (strLower == "red")
					type = ManaType.RED;
				else if (strLower == "blue")
					type = ManaType.BLUE;
				else if (strLower == "green")
					type = ManaType.GREEN;
				else if (strLower == "black")
					type = ManaType.BLACK;
				else
				{
					if (strLower == "white or red" || strLower == "red or white")
						type = ManaType.WHITE_RED;
					else if (strLower == "white or blue" || strLower == "blue or white")
						type = ManaType.WHITE_BLUE;
					else if (strLower == "white or green" || strLower == "green or white")
						type = ManaType.WHITE_GREEN;
					else if (strLower == "white or black" || strLower == "black or white")
						type = ManaType.WHITE_BLACK;
					else if (strLower == "blue or red" || strLower == "red or blue")
						type = ManaType.RED_BLUE;
					else if (strLower == "green or red" || strLower == "red or green")
						type = ManaType.RED_GREEN;
					else if (strLower == "red or black" || strLower == "black or red")
						type = ManaType.RED_BLACK;
					else if (strLower == "blue or green" || strLower == "green or blue")
						type = ManaType.BLUE_GREEN;
					else if (strLower == "blue or black" || strLower == "black or blue")
						type = ManaType.BLUE_BLACK;
					else if (strLower == "green or black" || strLower == "black or green")
						type = ManaType.GREEN_BLACK;
					else if (strLower == "variable colorless")
						type = ManaType.COMMON_X;
					else
					{
						type = ManaType.COMMON;
						cost = Int32.Parse(str);
					}
				}

				if (mana.ContainsKey(type) == true)
				{
					int value = mana[type];
					mana[type] = value + 1;
				}
				else
					mana.Add(type, cost);
			}
		}

		private void ConvertStringToManaCost(String strCost, ref Dictionary<ManaType, int> mana)
		{
			for (ManaType type = ManaType.COMMON; type < ManaType.MANA_TYPE_MAX; ++type)
			{
				if (mana.ContainsKey(type) == true)
				{
					int value = mana[type];
					mana[type] = value + 1;
				}
				else
				{
					int cost = Int32.Parse(strCost.Substring((int)type, 1));
					if(cost != 0)
						mana.Add(type, cost);
				}
			}
		}
	}
}
