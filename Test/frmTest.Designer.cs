namespace Test
{
    partial class frmTest
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnPublicTest = new System.Windows.Forms.Button();
            this.btnTradingTest = new System.Windows.Forms.Button();
            this.btnTradingHistory = new System.Windows.Forms.Button();
            this.btnAccount = new System.Windows.Forms.Button();
            this.btnMarketData = new System.Windows.Forms.Button();
            this.btnSocketTrading = new System.Windows.Forms.Button();
            this.gridviewReponse = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.gridviewReponse)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPublicTest
            // 
            this.btnPublicTest.Location = new System.Drawing.Point(12, 22);
            this.btnPublicTest.Name = "btnPublicTest";
            this.btnPublicTest.Size = new System.Drawing.Size(75, 23);
            this.btnPublicTest.TabIndex = 0;
            this.btnPublicTest.Text = "Public Test";
            this.btnPublicTest.UseVisualStyleBackColor = true;
            this.btnPublicTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnTradingTest
            // 
            this.btnTradingTest.Location = new System.Drawing.Point(93, 22);
            this.btnTradingTest.Name = "btnTradingTest";
            this.btnTradingTest.Size = new System.Drawing.Size(75, 23);
            this.btnTradingTest.TabIndex = 2;
            this.btnTradingTest.Text = "Trading Test";
            this.btnTradingTest.UseVisualStyleBackColor = true;
            this.btnTradingTest.Click += new System.EventHandler(this.btnTradingTest_Click);
            // 
            // btnTradingHistory
            // 
            this.btnTradingHistory.Location = new System.Drawing.Point(174, 22);
            this.btnTradingHistory.Name = "btnTradingHistory";
            this.btnTradingHistory.Size = new System.Drawing.Size(106, 23);
            this.btnTradingHistory.TabIndex = 3;
            this.btnTradingHistory.Text = "Trading History";
            this.btnTradingHistory.UseVisualStyleBackColor = true;
            // 
            // btnAccount
            // 
            this.btnAccount.Location = new System.Drawing.Point(286, 22);
            this.btnAccount.Name = "btnAccount";
            this.btnAccount.Size = new System.Drawing.Size(106, 23);
            this.btnAccount.TabIndex = 4;
            this.btnAccount.Text = "Acount";
            this.btnAccount.UseVisualStyleBackColor = true;
            this.btnAccount.Click += new System.EventHandler(this.btnAccount_Click);
            // 
            // btnMarketData
            // 
            this.btnMarketData.Location = new System.Drawing.Point(12, 51);
            this.btnMarketData.Name = "btnMarketData";
            this.btnMarketData.Size = new System.Drawing.Size(156, 23);
            this.btnMarketData.TabIndex = 5;
            this.btnMarketData.Text = "socket MarketData";
            this.btnMarketData.UseVisualStyleBackColor = true;
            this.btnMarketData.Click += new System.EventHandler(this.btnMarketData_Click);
            // 
            // btnSocketTrading
            // 
            this.btnSocketTrading.Location = new System.Drawing.Point(174, 51);
            this.btnSocketTrading.Name = "btnSocketTrading";
            this.btnSocketTrading.Size = new System.Drawing.Size(156, 23);
            this.btnSocketTrading.TabIndex = 6;
            this.btnSocketTrading.Text = "socket Trading";
            this.btnSocketTrading.UseVisualStyleBackColor = true;
            this.btnSocketTrading.Click += new System.EventHandler(this.btnSocketTrading_Click);
            // 
            // gridviewReponse
            // 
            this.gridviewReponse.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridviewReponse.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridviewReponse.Location = new System.Drawing.Point(12, 80);
            this.gridviewReponse.Name = "gridviewReponse";
            this.gridviewReponse.Size = new System.Drawing.Size(540, 175);
            this.gridviewReponse.TabIndex = 7;
            // 
            // frmTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 267);
            this.Controls.Add(this.gridviewReponse);
            this.Controls.Add(this.btnSocketTrading);
            this.Controls.Add(this.btnMarketData);
            this.Controls.Add(this.btnAccount);
            this.Controls.Add(this.btnTradingHistory);
            this.Controls.Add(this.btnTradingTest);
            this.Controls.Add(this.btnPublicTest);
            this.Name = "frmTest";
            this.Text = "Test From";
            ((System.ComponentModel.ISupportInitialize)(this.gridviewReponse)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPublicTest;
        private System.Windows.Forms.Button btnTradingTest;
        private System.Windows.Forms.Button btnTradingHistory;
        private System.Windows.Forms.Button btnAccount;
        private System.Windows.Forms.Button btnMarketData;
        private System.Windows.Forms.Button btnSocketTrading;
        private System.Windows.Forms.DataGridView gridviewReponse;
    }
}

