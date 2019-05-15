﻿using System;
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
	using ExpansionMap = Dictionary<eExpansion, int>;
	using CardList = Dictionary<eExpansion, Dictionary<String, CardData>>;

	public partial class Form2 : Form
	{
		private DeckBuilder m_mainForm;
		private BackgroundWorker m_backWorker;
		private WebLibrary m_WebLibrary;
		private String m_imagePath = "";
		private eExpansion m_expansion = eExpansion.EXPANSION_MAX;

		private Form2()
		{
			Application.SetCompatibleTextRenderingDefault(false);

			InitializeComponent();
		}

		public Form2(DeckBuilder parent, String imagePath, eExpansion expansion, ref BackgroundWorker backWorker) 
		{
			InitializeComponent();

			m_mainForm = parent;
			m_backWorker = backWorker;

			m_imagePath = imagePath;
			m_expansion = expansion;

			m_WebLibrary = new WebLibrary();
		}

		private void Form2_Load(object sender, EventArgs e)
		{
			CardList cardList = m_mainForm.GetCardList(m_expansion);
			int startID = m_mainForm.GetStartCardID(m_expansion);
			int cardNum = m_mainForm.GetCardNum(m_expansion);

			for (int i = 0; i < cardNum; ++i)
			{
				int cardID = startID + i;
				CrawlingCard(cardID, ref cardList);

				double progressRate = (double)cardList[m_expansion].Count / (double)cardNum;
				m_backWorker.ReportProgress((int)(progressRate * 100));
			}
		}

		public void CrawlingCard(int cardID, ref CardList cardList)
		{
			String strCardID = cardID.ToString();
			String url = m_WebLibrary.MakeURL(strCardID);

			HtmlDocument doc = m_WebLibrary.GetHTMLDocumentByURL(url);
			CardData card = m_WebLibrary.MakeCardData(doc, m_imagePath, strCardID);

			if (cardList[m_expansion].ContainsKey(card.GetCardName()) == false)
			{
				if (File.Exists(m_imagePath) == false)
					m_WebLibrary.DownLoadCardImage(m_imagePath, ref card);

				cardList[m_expansion].Add(card.GetCardName(), card);
			}
		}
	}
}
