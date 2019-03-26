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

		RARITY_COMMON,
		RARITY_UNCOMMON,
		RARITY_RARE,
		RARITY_MITHIC_RARE,

		RARITY_MAX,
	}

	class CardData
	{
		private string cardName;
		private int[] manaCost;
		private int convertedManaCost;
		private bool[] type;
		private string text;
		private string cardSet;
		private Rarity rarity;

		public CardData()
		{
			cardName = "";
			manaCost = new int[(int)ManaType.MANA_TYPE_MAX];
			convertedManaCost = 0;
			type = new bool[(int)CardType.CARD_TYPE_MAX];
			text = "";
			cardSet = "";
			rarity = Rarity.RARITY_NONE;

			type = new bool[(int)CardType.CARD_TYPE_MAX];
			for (int i = (int)CardType.CARD_TYPE_NONE; i < (int)CardType.CARD_TYPE_MAX; ++i)
				type[i] = false;
		}

		~CardData()
		{
			
		}

		public void SetCardName(string name) { cardName = name; }
		public string GetCardName() { return cardName; }

		public void SetManaCost(List<string> costList) { manaCost = ConvertStringListToManaCost(costList); }
		public int[] GetManaCost() { return manaCost; }

		public void SetCMC(string cmc) { convertedManaCost = Int32.Parse(cmc); }
		public int GetCMC() { return convertedManaCost; }

		public void SetType(string cardType) { type = ConvertStringToCardType(cardType); }
		public void SetType(bool[] cardType) { type = cardType; }
		public bool[] GetCardType() { return type; }
		public bool IsCardType(CardType cardType) { return type[(int)cardType]; }

		public void SetText(string cardText) { text = cardText; }
		public string GetText() { return text; }

		public void SetCardSet(string expansion) { cardSet = expansion; }
		public string GetCardSet() { return cardSet; }
		public bool IsIncludeSet(string setName) { return setName == cardSet; }

		public void SetRarity(string cardRarity) { rarity = ConvertStringToRarity(cardRarity); }
		public Rarity GetRarity() { return rarity; }

		private bool[] ConvertStringToCardType(string typeStr)
		{
			bool[] cardType = new bool[(int)CardType.CARD_TYPE_MAX];
			for (int i = (int)CardType.CARD_TYPE_NONE; i < (int)CardType.CARD_TYPE_MAX; ++i)
				cardType[i] = false;

			// todo. 플커랑 대지는 타입이 이상하게 되어 있는 경우가 많음.
			// ' '공백문자와 -하이픈으로 tokenize해서 넣어주어야 함.
			// eg. 전설적 플레인즈워커-앙그라스
			//     기본 대지-숲

			if (typeStr.ToLower() == "생물")
				cardType[(int)CardType.CARD_TYPE_CREATURE] = true;
			else if (typeStr.ToLower() == "집중마법")
				cardType[(int)CardType.CARD_TYPE_SOCERY] = true;
			else if (typeStr.ToLower() == "순간마법")
				cardType[(int)CardType.CARD_TYPE_INSTANT] = true;
			else if (typeStr.ToLower() == "부여마법")
				cardType[(int)CardType.CARD_TYPE_ENCHANTMENT] = true;
			else if (typeStr.ToLower() == "플레인즈워커")
				cardType[(int)CardType.CARD_TYPE_PLAINSWALKER] = true;
			else if (typeStr.ToLower() == "대지")
				cardType[(int)CardType.CARD_TYPE_LAND] = true;
			else if (typeStr.ToLower() == "마법물체")
				cardType[(int)CardType.CARD_TYPE_ARTIFACT] = true;

			return cardType;
		}

		private Rarity ConvertStringToRarity(string cardRarity)
		{
			if (cardRarity.ToLower() == "common")
				return Rarity.RARITY_COMMON;
			else if (cardRarity.ToLower() == "uncommon")
				return Rarity.RARITY_UNCOMMON;
			else if (cardRarity.ToLower() == "rare")
				return Rarity.RARITY_RARE;
			else if (cardRarity.ToLower() == "mithic rare")
				return Rarity.RARITY_MITHIC_RARE;
			else
				return Rarity.RARITY_NONE;
		}

		private int[] ConvertStringListToManaCost(List<string> costList)
		{
			int[] manaCost = new int[(int)ManaType.MANA_TYPE_MAX];

			foreach(string cost in costList)
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
	}
}
