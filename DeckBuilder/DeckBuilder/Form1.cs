using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace DeckBuilder
{
	using ExpansionMap = Dictionary<eExpansion, int>;
	using CardList = Dictionary<eExpansion, Dictionary<String, CardData>>;
	using DeckList = Dictionary<String, DeckCardData>;

	public enum eExpansion
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
		private ExpansionMap m_setStartIdList;
		private ExpansionMap m_cardSetNumList;

		private CardList m_CardList;
		private Dictionary<String, DeckCardData> m_DeckList;
		private CardData m_curSelectedCard;

		public String GetFullNameFromExpansionEnum(eExpansion expansion)
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

		public eExpansion GetExpansionEnumFromString(String expansionName)
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

		public ref readonly CardList GetCardList(eExpansion expansion)
		{
			return ref m_CardList;
		}

		public int GetStartCardID(eExpansion expansion)
		{
			return m_setStartIdList[expansion];
		}

		public int GetCardNum(eExpansion expansion)
		{
			return m_cardSetNumList[expansion];
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

		private void CrawlingCardBtn_Click(object sender, EventArgs e)
		{
			String expansionName = ExpansionComboBox.SelectedItem as String;

			BackgroundWorker backWorker = new BackgroundWorker();
			backWorker.WorkerReportsProgress = true;

			backWorker.DoWork += (_, args) =>
			{
				eExpansion expansion = GetExpansionEnumFromString(expansionName);
				StringBuilder imagePath = new StringBuilder(m_cardImageDir);
				imagePath.Append(expansionName);
				imagePath.Append("\\");

				var crawlingForm = new Form2(this, imagePath.ToString(), expansion, ref backWorker);
				crawlingForm.Opacity = 0;
				crawlingForm.ShowInTaskbar = false;
			
				Application.Run(crawlingForm);
			};

			backWorker.ProgressChanged += (_, args) =>
			{
				CardDataProgressBar.Value = (int)args.ProgressPercentage;
			};

			backWorker.RunWorkerAsync();
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

		private void DeckList_ItemSelected(object sender, EventArgs e)
		{
			String itemStr = DeckList.SelectedItem as String;
			String[] tok = itemStr.Split('\t');
			if (tok[0] == null)
				return;

			String name = tok[0];
			foreach (KeyValuePair<eExpansion, Dictionary<String, CardData>> cardList in m_CardList)
			{
				if (cardList.Value.ContainsKey(name))
				{
					DeckCardImg.Image = Image.FromFile(cardList.Value[name].GetImagePath());
					break;
				}
			}
		}

		private void DeckBuilder_FormClosed(object sender, FormClosedEventArgs e)
		{
			SaveCardData();
		}

		// todo. 휠로 카드 수 증감할 수 있도록 수정
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

		// todo. 휠로 카드 수 증감할 수 있도록 수정
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

		private void DeckListSaveBtn_Click(object sender, EventArgs e)
		{
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.InitialDirectory = Environment.CurrentDirectory;
			dialog.AddExtension = true;
			dialog.Filter = "xml 파일 (*.xml)|*.xml";

			if (dialog.ShowDialog(this) == DialogResult.OK)
			{
				SaveDeckList(dialog.FileName);
			}
		}

		private void LoadDeckListBtn_Click(object sender, EventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.InitialDirectory = Environment.CurrentDirectory;
			dialog.AddExtension = true;
			dialog.Filter = "xml 파일 (*.xml)|*.xml";

			if (dialog.ShowDialog(this) == DialogResult.OK)
			{
				LoadDeckList(dialog.FileName);
				RefreshDeckList();
			}
		}
	}
}
