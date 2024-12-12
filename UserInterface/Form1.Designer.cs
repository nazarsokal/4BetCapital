namespace UserInterface
{
    partial class Form1
    {
        public Form1()
        {
            // Налаштування форми
            this.Text = "Спортивні Ставки";
            this.Size = new System.Drawing.Size(1200, 800);

            // Баланс панель
            Panel balancePanel = new Panel
            {
                Size = new System.Drawing.Size(1180, 50),
                Location = new System.Drawing.Point(10, 10),
                BorderStyle = BorderStyle.FixedSingle
            };
            Label balanceLabel = new Label
            {
                Text = "Баланс:",
                Location = new System.Drawing.Point(10, 15),
                AutoSize = true
            };
            balancePanel.Controls.Add(balanceLabel);
            this.Controls.Add(balancePanel);

            // Панель для вкладок (Футбол, Волейбол, Баскетбол, Кіберспорт)
            TabControl tabControl = new TabControl
            {
                Size = new System.Drawing.Size(800, 600),
                Location = new System.Drawing.Point(200, 70)
            };

            // Додавання вкладок
            tabControl.TabPages.Add(CreateSportTab("Футбол"));
            tabControl.TabPages.Add(CreateSportTab("Волейбол"));
            tabControl.TabPages.Add(CreateSportTab("Баскетбол"));
            tabControl.TabPages.Add(CreateSportTab("Кіберспорт"));
            this.Controls.Add(tabControl);

            // Реклама панель
            Panel adPanel = new Panel
        {
                Size = new System.Drawing.Size(180, 600),
                Location = new System.Drawing.Point(10, 70),
                BorderStyle = BorderStyle.FixedSingle
            };
            Label adLabel = new Label
            {
                Text = "Реклама",
                Location = new System.Drawing.Point(50, 290),
                AutoSize = true
            };
            adPanel.Controls.Add(adLabel);
            this.Controls.Add(adPanel);
        }

        private TabPage CreateSportTab(string sportName)
        {
            TabPage tabPage = new TabPage(sportName);

            // Створення подій для спорту
            FlowLayoutPanel eventPanel = new FlowLayoutPanel
        {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };

            for (int i = 0; i < 5; i++)
            {
                Panel matchPanel = new Panel
                {
                    Size = new System.Drawing.Size(750, 100),
                    BorderStyle = BorderStyle.FixedSingle
                };

                Label timeLabel = new Label
                {
                    Text = "Вт: 14:00",
                    Location = new System.Drawing.Point(10, 10),
                    AutoSize = true
                };

                Label team1Label = new Label
                {
                    Text = "Команда 1",
                    Location = new System.Drawing.Point(10, 40),
                    AutoSize = true
                };

                Label drawLabel = new Label
                {
                    Text = "X",
                    Location = new System.Drawing.Point(320, 40),
                    AutoSize = true
                };

                Label team2Label = new Label
            {
                    Text = "Команда 2",
                    Location = new System.Drawing.Point(600, 40),
                    AutoSize = true
                };

                Label odds1Label = new Label
                {
                    Text = "1.43",
                    Location = new System.Drawing.Point(10, 70),
                    AutoSize = true
                };
                Button bet1Button = new Button
                {
                    Text = "Ставка",
                    Location = new System.Drawing.Point(60, 65),
                    Size = new System.Drawing.Size(75, 25)
            };

            // Label for results
            Label lblResults = new Label();
            lblResults.Text = "Результати:";
            lblResults.Location = new Point(350, 10);
            lblResults.Size = new Size(200, 20);

                Label oddsDrawLabel = new Label
                {
                    Text = "1.77",
                    Location = new System.Drawing.Point(320, 70),
                    AutoSize = true
                };
                Button betDrawButton = new Button
                {
                    Text = "Ставка",
                    Location = new System.Drawing.Point(380, 65),
                    Size = new System.Drawing.Size(75, 25)
                };

                Label odds2Label = new Label
                {
                    Text = "2.20",
                    Location = new System.Drawing.Point(600, 70),
                    AutoSize = true
                };
                Button bet2Button = new Button
            {
                    Text = "Ставка",
                    Location = new System.Drawing.Point(660, 65),
                    Size = new System.Drawing.Size(75, 25)
            };

                matchPanel.Controls.Add(timeLabel);
                matchPanel.Controls.Add(team1Label);
                matchPanel.Controls.Add(drawLabel);
                matchPanel.Controls.Add(team2Label);
                matchPanel.Controls.Add(odds1Label);
                matchPanel.Controls.Add(bet1Button);
                matchPanel.Controls.Add(oddsDrawLabel);
                matchPanel.Controls.Add(betDrawButton);
                matchPanel.Controls.Add(odds2Label);
                matchPanel.Controls.Add(bet2Button);

                eventPanel.Controls.Add(matchPanel);
        }

            tabPage.Controls.Add(eventPanel);
            return tabPage;
        }
    }
}