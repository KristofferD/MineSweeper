using System;
using System.Windows.Forms;

class MainSweeperForm : Form
{
    private Game game;

    public MainSweeperForm()
    {
        this.game = new Game(10, 10, 10);
        this.InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.SuspendLayout();
        // TODO: Add controls to the form
        this.ResumeLayout(false);
    }

    static void Main()
    {
        Application.Run(new MainSweeperForm());
    }
}
