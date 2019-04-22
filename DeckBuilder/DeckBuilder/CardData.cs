using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeckBuilder
{
	enum CardType
	{
		CARD_TYPE_NONE = 0,

		CARD_TYPE_CREATURE,
		CARD_TYPE_SOCERY,
		CARD_TYPE_INSTANT,
		CARD_TYPE_ENCHANTMENT,
		CARD_TYPE_ARTIFACT,
		CARD_TYPE_PLAINSWALKER,
		CARD_TYPE_LAND,
		
		CARD_TYPE_MAX,
	}

	enum ManaType
	{
		MANA_TYPE_COMMON = 0,
		MANA_TYPE_WHITE,
		MANA_TYPE_RED,
		MANA_TYPE_BLUE,
		MANA_TYPE_GREEN,
		MANA_TYPE_BLACK,

		MANA_TYPE_MAX,
	}

	enum Rarity
	{
		RARITY_NONE = 0,

		COMMON,
		UNCOMMON,
		RARE,
		MITHIC_RARE,

		RARITY_MAX,
	}

	class CardData
	{
		private String cardID;
		private String cardName;
		private int[] manaCost;
		private int convertedManaCost;
		private List<CardType> types;
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
			types = new List<CardType>();
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

		public void AddType(String cardType) { types.Add(ConvertStringToCardType(cardType)); }
		public void SetType(List<String> typeList) { types = ConvertStringToCardType(typeList); }
		public void SetType(List<CardType> cardTypes) { types = cardTypes; }
		public List<CardType> GetCardTypes() { return types; }
		public bool IsCardType(CardType cardType) { return types.Contains(cardType); }

		public void SetText(String cardText) { text = cardText; }
		public String GetText() { return text; }

		public void SetCardSet(String expansion) { cardSet = expansion; }
		public String GetCardSet() { return cardSet; }
		public bool IsIncludeSet(String setName) { return setName == cardSet; }

		public void SetRarity(String cardRarity) { rarity = ConvertStringToRarity(cardRarity); }
		public Rarity GetRarity() { return rarity; }

		public void SetImagePath(String path) { imagePath = path; }
		public String GetImagePath() { return imagePath; }

		private CardType ConvertStringToCardType(String typeStr)
		{
			// todo. 플커랑 대지는 타입이 이상하게 되어 있는 경우가 많음.
			// ' '공백문자와 -하이픈으로 tokenize해서 넣어주어야 함.
			// eg. 전설적 플레인즈워커-앙그라스
			//     기본 대지-숲
			CardType type = CardType.CARD_TYPE_NONE;
			if (typeStr.ToLower() == "생물")
				type = CardType.CARD_TYPE_CREATURE;
			else if (typeStr.ToLower() == "집중마법")
				type = CardType.CARD_TYPE_SOCERY;
			else if (typeStr.ToLower() == "순간마법")
				type = CardType.CARD_TYPE_INSTANT;
			else if (typeStr.ToLower() == "부여마법")
				type = CardType.CARD_TYPE_ENCHANTMENT;
			else if (typeStr.ToLower() == "플레인즈워커")
				type = CardType.CARD_TYPE_PLAINSWALKER;
			else if (typeStr.ToLower() == "대지")
				type = CardType.CARD_TYPE_LAND;
			else if (typeStr.ToLower() == "마법물체")
				type = CardType.CARD_TYPE_ARTIFACT;

			return type;
		}

		private List<CardType> ConvertStringToCardType(List<String> typeStrList)
		{
			List<CardType> typeList = new List<CardType>();
			
			foreach (String strType in typeStrList)
			{
				int num = Int32.Parse(strType);
				typeList.Add((CardType)num);
			}

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
