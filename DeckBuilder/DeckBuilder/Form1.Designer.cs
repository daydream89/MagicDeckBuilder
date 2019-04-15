namespace DeckBuilder
{
	partial class DeckBuilder
	{
		/// <summary>
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		/// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form 디자이너에서 생성한 코드

		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다. 
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
		/// </summary>
		private void InitializeComponent()
		{
			this.CardDatabaseGB = new System.Windows.Forms.GroupBox();
			this.ListCardImgGB = new System.Windows.Forms.GroupBox();
			this.ListCardImg = new System.Windows.Forms.PictureBox();
			this.CardOptionGB = new System.Windows.Forms.GroupBox();
			this.ExpansionComboBox = new System.Windows.Forms.ComboBox();
			this.CardDataProgressBar = new System.Windows.Forms.ProgressBar();
			this.RefreshListBtn = new System.Windows.Forms.Button();
			this.CrawlingCardBtn = new System.Windows.Forms.Button();
			this.CardListBox = new System.Windows.Forms.ListBox();
			this.DeckBuilderGB = new System.Windows.Forms.GroupBox();
			this.DeckCardImgGB = new System.Windows.Forms.GroupBox();
			this.DeckCardImg = new System.Windows.Forms.PictureBox();
			this.DeckOptionGB = new System.Windows.Forms.GroupBox();
			this.DeckList = new System.Windows.Forms.ListBox();
			this.CardDatabaseGB.SuspendLayout();
			this.ListCardImgGB.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ListCardImg)).BeginInit();
			this.CardOptionGB.SuspendLayout();
			this.DeckBuilderGB.SuspendLayout();
			this.DeckCardImgGB.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.DeckCardImg)).BeginInit();
			this.SuspendLayout();
			// 
			// CardDatabaseGB
			// 
			this.CardDatabaseGB.Controls.Add(this.ListCardImgGB);
			this.CardDatabaseGB.Controls.Add(this.CardOptionGB);
			this.CardDatabaseGB.Controls.Add(this.CardListBox);
			this.CardDatabaseGB.Location = new System.Drawing.Point(12, 12);
			this.CardDatabaseGB.Name = "CardDatabaseGB";
			this.CardDatabaseGB.Size = new System.Drawing.Size(441, 530);
			this.CardDatabaseGB.TabIndex = 0;
			this.CardDatabaseGB.TabStop = false;
			this.CardDatabaseGB.Text = "Card Database";
			// 
			// ListCardImgGB
			// 
			this.ListCardImgGB.Controls.Add(this.ListCardImg);
			this.ListCardImgGB.Location = new System.Drawing.Point(192, 175);
			this.ListCardImgGB.Name = "ListCardImgGB";
			this.ListCardImgGB.Size = new System.Drawing.Size(238, 349);
			this.ListCardImgGB.TabIndex = 2;
			this.ListCardImgGB.TabStop = false;
			this.ListCardImgGB.Text = "Card Image";
			// 
			// ListCardImg
			// 
			this.ListCardImg.Location = new System.Drawing.Point(7, 25);
			this.ListCardImg.Margin = new System.Windows.Forms.Padding(0);
			this.ListCardImg.Name = "ListCardImg";
			this.ListCardImg.Size = new System.Drawing.Size(223, 321);
			this.ListCardImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.ListCardImg.TabIndex = 0;
			this.ListCardImg.TabStop = false;
			// 
			// CardOptionGB
			// 
			this.CardOptionGB.Controls.Add(this.ExpansionComboBox);
			this.CardOptionGB.Controls.Add(this.CardDataProgressBar);
			this.CardOptionGB.Controls.Add(this.RefreshListBtn);
			this.CardOptionGB.Controls.Add(this.CrawlingCardBtn);
			this.CardOptionGB.Location = new System.Drawing.Point(192, 24);
			this.CardOptionGB.Name = "CardOptionGB";
			this.CardOptionGB.Size = new System.Drawing.Size(238, 145);
			this.CardOptionGB.TabIndex = 1;
			this.CardOptionGB.TabStop = false;
			this.CardOptionGB.Text = "Option";
			// 
			// ExpansionComboBox
			// 
			this.ExpansionComboBox.FormattingEnabled = true;
			this.ExpansionComboBox.Location = new System.Drawing.Point(7, 48);
			this.ExpansionComboBox.Name = "ExpansionComboBox";
			this.ExpansionComboBox.Size = new System.Drawing.Size(223, 23);
			this.ExpansionComboBox.TabIndex = 3;
			// 
			// CardDataProgressBar
			// 
			this.CardDataProgressBar.Location = new System.Drawing.Point(6, 116);
			this.CardDataProgressBar.Name = "CardDataProgressBar";
			this.CardDataProgressBar.Size = new System.Drawing.Size(224, 23);
			this.CardDataProgressBar.TabIndex = 2;
			// 
			// RefreshListBtn
			// 
			this.RefreshListBtn.Font = new System.Drawing.Font("굴림", 8F);
			this.RefreshListBtn.Location = new System.Drawing.Point(130, 77);
			this.RefreshListBtn.Name = "RefreshListBtn";
			this.RefreshListBtn.Size = new System.Drawing.Size(102, 33);
			this.RefreshListBtn.TabIndex = 1;
			this.RefreshListBtn.Text = "리스트 갱신";
			this.RefreshListBtn.UseVisualStyleBackColor = true;
			this.RefreshListBtn.Click += new System.EventHandler(this.RefreshListBtn_Click);
			// 
			// CrawlingCardBtn
			// 
			this.CrawlingCardBtn.Font = new System.Drawing.Font("굴림", 8F);
			this.CrawlingCardBtn.Location = new System.Drawing.Point(7, 77);
			this.CrawlingCardBtn.Name = "CrawlingCardBtn";
			this.CrawlingCardBtn.Size = new System.Drawing.Size(117, 33);
			this.CrawlingCardBtn.TabIndex = 0;
			this.CrawlingCardBtn.Text = "카드 가져오기";
			this.CrawlingCardBtn.UseVisualStyleBackColor = true;
			this.CrawlingCardBtn.Click += new System.EventHandler(this.CrawlingCardBtn_Click);
			// 
			// CardListBox
			// 
			this.CardListBox.FormattingEnabled = true;
			this.CardListBox.ItemHeight = 15;
			this.CardListBox.Location = new System.Drawing.Point(6, 24);
			this.CardListBox.Name = "CardListBox";
			this.CardListBox.Size = new System.Drawing.Size(180, 499);
			this.CardListBox.TabIndex = 0;
			this.CardListBox.SelectedIndexChanged += new System.EventHandler(this.CardListBox_ItemSelected);
			// 
			// DeckBuilderGB
			// 
			this.DeckBuilderGB.Controls.Add(this.DeckCardImgGB);
			this.DeckBuilderGB.Controls.Add(this.DeckOptionGB);
			this.DeckBuilderGB.Controls.Add(this.DeckList);
			this.DeckBuilderGB.Location = new System.Drawing.Point(459, 12);
			this.DeckBuilderGB.Name = "DeckBuilderGB";
			this.DeckBuilderGB.Size = new System.Drawing.Size(439, 530);
			this.DeckBuilderGB.TabIndex = 1;
			this.DeckBuilderGB.TabStop = false;
			this.DeckBuilderGB.Text = "Deck Builder";
			// 
			// DeckCardImgGB
			// 
			this.DeckCardImgGB.Controls.Add(this.DeckCardImg);
			this.DeckCardImgGB.Location = new System.Drawing.Point(192, 174);
			this.DeckCardImgGB.Name = "DeckCardImgGB";
			this.DeckCardImgGB.Size = new System.Drawing.Size(238, 350);
			this.DeckCardImgGB.TabIndex = 2;
			this.DeckCardImgGB.TabStop = false;
			this.DeckCardImgGB.Text = "Card Image";
			// 
			// DeckCardImg
			// 
			this.DeckCardImg.Location = new System.Drawing.Point(7, 25);
			this.DeckCardImg.Name = "DeckCardImg";
			this.DeckCardImg.Size = new System.Drawing.Size(223, 324);
			this.DeckCardImg.TabIndex = 0;
			this.DeckCardImg.TabStop = false;
			// 
			// DeckOptionGB
			// 
			this.DeckOptionGB.Location = new System.Drawing.Point(192, 24);
			this.DeckOptionGB.Name = "DeckOptionGB";
			this.DeckOptionGB.Size = new System.Drawing.Size(238, 144);
			this.DeckOptionGB.TabIndex = 1;
			this.DeckOptionGB.TabStop = false;
			this.DeckOptionGB.Text = "Option";
			// 
			// DeckList
			// 
			this.DeckList.FormattingEnabled = true;
			this.DeckList.ItemHeight = 15;
			this.DeckList.Location = new System.Drawing.Point(6, 24);
			this.DeckList.Name = "DeckList";
			this.DeckList.Size = new System.Drawing.Size(180, 499);
			this.DeckList.TabIndex = 0;
			// 
			// DeckBuilder
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(907, 554);
			this.Controls.Add(this.DeckBuilderGB);
			this.Controls.Add(this.CardDatabaseGB);
			this.Name = "DeckBuilder";
			this.Text = "MTG Deck Builder";
			this.CardDatabaseGB.ResumeLayout(false);
			this.ListCardImgGB.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.ListCardImg)).EndInit();
			this.CardOptionGB.ResumeLayout(false);
			this.DeckBuilderGB.ResumeLayout(false);
			this.DeckCardImgGB.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.DeckCardImg)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox CardDatabaseGB;
		private System.Windows.Forms.GroupBox DeckBuilderGB;
		private System.Windows.Forms.GroupBox CardOptionGB;
		private System.Windows.Forms.ListBox CardListBox;
		private System.Windows.Forms.GroupBox DeckOptionGB;
		private System.Windows.Forms.ListBox DeckList;
		private System.Windows.Forms.GroupBox ListCardImgGB;
		private System.Windows.Forms.GroupBox DeckCardImgGB;
		private System.Windows.Forms.Button CrawlingCardBtn;
		private System.Windows.Forms.Button RefreshListBtn;
		private System.Windows.Forms.ProgressBar CardDataProgressBar;
		private System.Windows.Forms.PictureBox ListCardImg;
		private System.Windows.Forms.PictureBox DeckCardImg;
		private System.Windows.Forms.ComboBox ExpansionComboBox;
	}
}

