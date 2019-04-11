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
			this.CardOptionGB = new System.Windows.Forms.GroupBox();
			this.CardList = new System.Windows.Forms.ListBox();
			this.DeckBuilderGB = new System.Windows.Forms.GroupBox();
			this.DeckCardImgGB = new System.Windows.Forms.GroupBox();
			this.DeckOptionGB = new System.Windows.Forms.GroupBox();
			this.DeckList = new System.Windows.Forms.ListBox();
			this.CardDatabaseGB.SuspendLayout();
			this.DeckBuilderGB.SuspendLayout();
			this.SuspendLayout();
			// 
			// CardDatabaseGB
			// 
			this.CardDatabaseGB.Controls.Add(this.ListCardImgGB);
			this.CardDatabaseGB.Controls.Add(this.CardOptionGB);
			this.CardDatabaseGB.Controls.Add(this.CardList);
			this.CardDatabaseGB.Location = new System.Drawing.Point(12, 12);
			this.CardDatabaseGB.Name = "CardDatabaseGB";
			this.CardDatabaseGB.Size = new System.Drawing.Size(380, 426);
			this.CardDatabaseGB.TabIndex = 0;
			this.CardDatabaseGB.TabStop = false;
			this.CardDatabaseGB.Text = "Card Database";
			// 
			// ListCardImgGB
			// 
			this.ListCardImgGB.Location = new System.Drawing.Point(192, 136);
			this.ListCardImgGB.Name = "ListCardImgGB";
			this.ListCardImgGB.Size = new System.Drawing.Size(182, 282);
			this.ListCardImgGB.TabIndex = 2;
			this.ListCardImgGB.TabStop = false;
			this.ListCardImgGB.Text = "Card Image";
			// 
			// CardOptionGB
			// 
			this.CardOptionGB.Location = new System.Drawing.Point(192, 24);
			this.CardOptionGB.Name = "CardOptionGB";
			this.CardOptionGB.Size = new System.Drawing.Size(182, 106);
			this.CardOptionGB.TabIndex = 1;
			this.CardOptionGB.TabStop = false;
			this.CardOptionGB.Text = "Option";
			// 
			// CardList
			// 
			this.CardList.FormattingEnabled = true;
			this.CardList.ItemHeight = 15;
			this.CardList.Location = new System.Drawing.Point(6, 24);
			this.CardList.Name = "CardList";
			this.CardList.Size = new System.Drawing.Size(180, 394);
			this.CardList.TabIndex = 0;
			// 
			// DeckBuilderGB
			// 
			this.DeckBuilderGB.Controls.Add(this.DeckCardImgGB);
			this.DeckBuilderGB.Controls.Add(this.DeckOptionGB);
			this.DeckBuilderGB.Controls.Add(this.DeckList);
			this.DeckBuilderGB.Location = new System.Drawing.Point(409, 12);
			this.DeckBuilderGB.Name = "DeckBuilderGB";
			this.DeckBuilderGB.Size = new System.Drawing.Size(380, 426);
			this.DeckBuilderGB.TabIndex = 1;
			this.DeckBuilderGB.TabStop = false;
			this.DeckBuilderGB.Text = "Deck Builder";
			// 
			// DeckCardImgGB
			// 
			this.DeckCardImgGB.Location = new System.Drawing.Point(192, 136);
			this.DeckCardImgGB.Name = "DeckCardImgGB";
			this.DeckCardImgGB.Size = new System.Drawing.Size(182, 282);
			this.DeckCardImgGB.TabIndex = 2;
			this.DeckCardImgGB.TabStop = false;
			this.DeckCardImgGB.Text = "Card Image";
			// 
			// DeckOptionGB
			// 
			this.DeckOptionGB.Location = new System.Drawing.Point(192, 24);
			this.DeckOptionGB.Name = "DeckOptionGB";
			this.DeckOptionGB.Size = new System.Drawing.Size(182, 106);
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
			this.DeckList.Size = new System.Drawing.Size(180, 394);
			this.DeckList.TabIndex = 0;
			// 
			// DeckBuilder
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.DeckBuilderGB);
			this.Controls.Add(this.CardDatabaseGB);
			this.Name = "DeckBuilder";
			this.Text = "MTG Deck Builder";
			this.CardDatabaseGB.ResumeLayout(false);
			this.DeckBuilderGB.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox CardDatabaseGB;
		private System.Windows.Forms.GroupBox DeckBuilderGB;
		private System.Windows.Forms.GroupBox CardOptionGB;
		private System.Windows.Forms.ListBox CardList;
		private System.Windows.Forms.GroupBox DeckOptionGB;
		private System.Windows.Forms.ListBox DeckList;
		private System.Windows.Forms.GroupBox ListCardImgGB;
		private System.Windows.Forms.GroupBox DeckCardImgGB;
	}
}

