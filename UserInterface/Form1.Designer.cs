namespace UserInterface
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Label for available bets
            Label lblAvailableBets = new Label();
            lblAvailableBets.Text = "Доступні ставки:";
            lblAvailableBets.Location = new Point(10, 10);
            lblAvailableBets.Size = new Size(200, 20);

            // ListBox for displaying available bets
            ListBox lstAvailableBets = new ListBox();
            lstAvailableBets.Location = new Point(10, 40);
            lstAvailableBets.Size = new Size(300, 200);

            // Label for inputting bets
            Label lblPlaceBet = new Label();
            lblPlaceBet.Text = "Ваша ставка:";
            lblPlaceBet.Location = new Point(10, 260);
            lblPlaceBet.Size = new Size(200, 20);

            // TextBox for entering bet amount
            TextBox txtBetAmount = new TextBox();
            txtBetAmount.Location = new Point(10, 290);
            txtBetAmount.Size = new Size(200, 25);

            // Button to place bet
            Button btnPlaceBet = new Button();
            btnPlaceBet.Text = "Зробити ставку";
            btnPlaceBet.Location = new Point(220, 290);
            btnPlaceBet.Size = new Size(150, 30);
            btnPlaceBet.Click += (sender, e) =>
            {
                // TODO: Handle placing a bet

                MessageBox.Show("Ставку зроблено!");
            };

            // Label for results
            Label lblResults = new Label();
            lblResults.Text = "Результати:";
            lblResults.Location = new Point(350, 10);
            lblResults.Size = new Size(200, 20);

            // ListBox for displaying results
            ListBox lstResults = new ListBox();
            lstResults.Location = new Point(350, 40);
            lstResults.Size = new Size(300, 200);

            // Button to refresh results
            Button btnRefreshResults = new Button();
            btnRefreshResults.Text = "Оновити результати";
            btnRefreshResults.Location = new Point(350, 260);
            btnRefreshResults.Size = new Size(150, 30);
            btnRefreshResults.Click += (sender, e) =>
            {
                // TODO: Handle refreshing results
                MessageBox.Show("Результати оновлено!");
            };

            // Form1
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(700, 400);
            this.Controls.Add(lblAvailableBets);
            this.Controls.Add(lstAvailableBets);
            this.Controls.Add(lblPlaceBet);
            this.Controls.Add(txtBetAmount);
            this.Controls.Add(btnPlaceBet);
            this.Controls.Add(lblResults);
            this.Controls.Add(lstResults);
            this.Controls.Add(btnRefreshResults);
            this.Name = "Form1";
            this.Text = "Букмекерська контора";
            this.ResumeLayout(false);
        }

        #endregion
    }
}