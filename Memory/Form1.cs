using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace Memory
{
    public partial class Form1 : Form
    {
        Random random = new Random();

        // Die Icons
        List<string> icons = new List<string>()
        {
            "!", "!", "N", "N", ",", ",", "k", "k",
            "b", "b", "v", "v", "w", "w", "z", "z"
        };
        public Form1()
        {
            InitializeComponent();
            AssignIconsToSquares();

        }
        /// <summary>
        /// Jedes Symbol aus der obigen Liste wird einem Quadrat zugefügt
        /// </summary>
        private void AssignIconsToSquares()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                iconLabel.ForeColor = iconLabel.BackColor;
                if (iconLabel != null)
                {
                    int randomNumber = random.Next(icons.Count);
                    iconLabel.Text = icons[randomNumber];
                    icons.RemoveAt(randomNumber);
                }
            }
        }

        // firstClicked zeigt auf das erste Label-Steuerelement 
        // dass der Spieler klickt, aber es wird null sein 
        // wenn der Spieler noch nicht auf ein Label geklickt hat.
        Label firstClicked = null;

        // secondClicked zeigt auf das zweite Label-Steuerelement 
        // auf die der Spieler klickt
        Label secondClicked = null;

        private void label_click(object sender, EventArgs e)
        {
            // Der Timer ist erst nach zwei nicht übereinstimmenden Symbole an.
            // dieser Timer macht dann das die klicks wieder von vorne beginnen und die Symbole wieder verschwinden.
            if (timer1.Enabled == true)
                return;

            Label clickedLabel = sender as Label;

            // Wenn secondClicked nicht null ist, hat der Spieler bereits zweimal angeklickt und das Spiel wurde noch nicht zurückgesetzt = den Klick ignorieren
            if (secondClicked != null)
                return;

            if (clickedLabel != null)
            {
                // Wenn der Benutzer ein Symbol klickt das schon aufgedeckt ist wird es ignoriert.
                if (clickedLabel.ForeColor == Color.White)
                    return;

                // Wenn firstClicked null ist, ist dies das erste Symbol in dem Paar, auf das der Spieler geklickt hat, 
                // so setzt firstClickted auf das Label, dass der Spieler
                // angeklickt hat und ändert die Farbe in weiss und return;
                if (firstClicked == null)
                {
                    firstClicked = clickedLabel;
                    firstClicked.ForeColor = Color.White;
                    return;
                }

                //Wenn der Spieler bis hier kommt muss es das 2 Symbol sein daher dieses Symbol auf weiss stellen.

                secondClicked = clickedLabel;
                secondClicked.ForeColor = Color.White;

                // Überprüfung ob der Spieler gewonnen hat.
                CheckForWinner();


                // Wenn der Spieler zwei identische Symbole gegklickt hat, müssen diese Weiss bleiben aber firstClicked und secondClicked muss resettet werden.
                if (firstClicked.Text == secondClicked.Text)
                {
                    firstClicked = null;
                    secondClicked = null;
                    return;
                }

                // Wenn der Spieler so weit kommt, wird der Spieler 
                // zwei verschiedene Symbole angeklickt haben, also starten Sie den 
                // Timer (dieser drei Viertel  
                //  Sekunden wartet und dann die Symbole ausblendet)
                timer1.Start();
            }


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Stoppt den Timer
            timer1.Stop();

            // Beide Bilder verstecken bei klick.
            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            // Reset, so dass alles neu beginnt.
            firstClicked = null;
            secondClicked = null;

        }
        /// <summary>
        /// Prüft jedes Symbol auf Übereinstimmung, indem Sie 
        /// seine Vordergrundfarbe mit seiner Hintergrundfarbe vergleicht. 
        /// Wenn alle Symbole übereinstimmen, gewinnt der Spieler
        /// </summary>
        private void CheckForWinner()
        {
            // Alle labels im TableLayoutPanel werden überprüft, 
            // um zu sehen, ob sein Symbol übereinstimmt
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;

                if (iconLabel != null)
                {
                    if (iconLabel.ForeColor == iconLabel.BackColor)
                        return;
                }
            }
            SoundPlayer splayer = new SoundPlayer(@"C:\Windows\Media\Ring06.wav");
            // Wenn die Schleife nicht zurückkehrt hat der Benutzer gewonnen. Eine Nachricht und Sound wird ausgegeben
            splayer.Play();
            MessageBox.Show("Wuhuuuu!", "Gwonne!");

            Close();
        }
    }
}
