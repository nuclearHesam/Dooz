namespace Dooz
{
    public partial class GamePage : Form
    {
        public GamePage()
        {
            InitializeComponent();
        }

        private readonly Dictionary<string, char> map = new()
        {
            {"r1c1",'0'},{"r1c2",'0'},{"r1c3",'0'},
            {"r2c1",'0'},{"r2c2",'0'},{"r2c3",'0'},
            {"r3c1",'0'},{"r3c2",'0'},{"r3c3",'0'}
        };

        private void Buttons_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string name = button.Name;

            if (map[name] == 'O' || map[name] == 'X')
                return;

            map[name] = 'X';
            button.BackColor = Color.FromArgb(255, 128, 255);
            button.Text = "X";

            if (CheckWin() == 'X')
                return;

            BlockTurn('O');

            if (CheckWin() == 'O')
            {
                // center
                BotTurn($"r2c2");
            }
            else 
            {
                BlockTurn('X');

                if (CheckWin() == 'O')
                    BlockTurn('X');
            }
        }

        private void BlockTurn(char turn)
        {
            for (int i = 1; i < 4; i++)
            {
                // r 
                if (map[$"r{i}c1"] == turn && map[$"r{i}c2"] == '0' && map[$"r{i}c3"] == turn)
                {
                    BotTurn($"r{i}c2");
                    return;
                }

                // r right
                if (map[$"r{i}c1"] == turn && map[$"r{i}c2"] == turn && map[$"r{i}c3"] == '0')
                {
                    BotTurn($"r{i}c3");
                    return;
                }

                // r left
                if (map[$"r{i}c1"] == '0' && map[$"r{i}c2"] == turn && map[$"r{i}c3"] == turn)
                {
                    BotTurn($"r{i}c1");
                    return;
                }

                // c
                if (map[$"r1c{i}"] == turn && map[$"r2c{i}"] == '0' && map[$"r3c{i}"] == turn)
                {
                    BotTurn($"r2c{i}");
                    return;
                }

                // c up
                if (map[$"r1c{i}"] == '0' && map[$"r2c{i}"] == turn && map[$"r3c{i}"] == turn)
                {
                    BotTurn($"r1c{i}");
                    return;
                }

                // c down
                if (map[$"r1c{i}"] == turn && map[$"r2c{i}"] == turn && map[$"r3c{i}"] == '0')
                {
                    BotTurn($"r3c{i}");
                    return;
                }
            }

            //  center
            if ((map["r1c1"] == turn && map["r3c3"] == turn) || (map["r1c3"] == turn && map["r3c1"] == turn))
            {
                if (map[$"r2c2"] == '0')
                {
                    BotTurn("r2c2");
                    return;
                }
            }

            // down right
            if (map["r1c1"] == turn && map["r2c2"] == turn && map["r3c3"] == '0')
            {
                BotTurn("r3c3");
                return;
            }
            // up left
            if (map["r1c1"] == '0' && map["r2c2"] == turn && map["r3c3"] == turn)
            {
                BotTurn("r1c1");
                return;
            }
            // down left
            if (map["r1c3"] == turn && map["r2c2"] == turn && map["r3c1"] == '0')
            {
                BotTurn("r3c1");
                return;
            }
            // up right
            if (map["r1c3"] == '0' && map["r2c2"] == turn && map["r3c1"] == turn)
            {
                BotTurn("r1c3");
                return;
            }

            if (turn == 'X')
                RandormTurn();
        }

        private void RandormTurn()
        {
            var nonZeroEntries = map.Where(kvp => kvp.Value == '0').ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            if (nonZeroEntries.Count != 0)
            {
                Random random = new();
                int turnIndex = random.Next(0, nonZeroEntries.Count);
                var turnKey = nonZeroEntries.Keys.ElementAt(turnIndex);

                if (this.Controls.Find(turnKey, true).FirstOrDefault() is Button selectedButton)
                {
                    BotTurn(turnKey);
                }
            }
        }

        private void BotTurn(string turnKey)
        {
            if (this.Controls.Find(turnKey, true).FirstOrDefault() is Button button)
            {
                button.Text = "O";
                button.BackColor = Color.FromArgb(128, 128, 255);
                map[turnKey] = 'O';
            }
        }

        private char CheckWin()
        {
            char win = '0';

            for (int i = 1; i < 4; i++)
            {
                // r
                if (map[$"r{i}c1"] != '0' &&
                    map[$"r{i}c3"] != '0' &&
                    map[$"r{i}c1"] == map[$"r{i}c2"] &&
                    map[$"r{i}c2"] == map[$"r{i}c3"])
                {
                    win = map[$"r{i}c2"];

                    MessageBox.Show(win + " Win!");
                    Restart();

                    return win;
                }

                // c
                if (map[$"r1c{i}"] != '0' &&
                    map[$"r3c{i}"] != '0' &&
                    map[$"r1c{i}"] == map[$"r2c{i}"] &&
                    map[$"r2c{i}"] == map[$"r3c{i}"])
                {
                    win = map[$"r2c{i}"];

                    MessageBox.Show(win + " Win!");
                    Restart();

                    return win;
                }
            }

            //  center
            if (map[$"r2c2"] != '0')
            {
                if ((map["r1c1"] == map["r2c2"] &&
                    map["r2c2"] == map["r3c3"])
                   ||
                    (map["r1c3"] == map["r2c2"] &&
                    map["r2c2"] == map["r3c1"]))
                {
                    win = map[$"r2c2"];

                    MessageBox.Show(win + " Win!");
                    Restart();

                    return win;
                }
            }

            //  equal
            if (!map.Where(kvp => kvp.Value == '0').Any())
            {
                MessageBox.Show("Equal!");
                Restart();
                return 'X';
            }

            return win;
        }

        private void Restart()
        {
            map.Clear();
            foreach (var key in new List<string> { "r1c1", "r1c2", "r1c3", "r2c1", "r2c2", "r2c3", "r3c1", "r3c2", "r3c3" })
            {
                if (this.Controls.Find(key, true).FirstOrDefault() is Button selectedButton)
                {
                    selectedButton.Text = "";
                    selectedButton.BackColor = Color.FromName("Control");
                }

                map[key] = '0';
            }
        }
    }
}
