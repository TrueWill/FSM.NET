using System;
using System.Windows.Forms;

namespace SampleWinFormsApp
{
    public partial class MainForm : Form
    {
        private readonly IModel _model;

        public MainForm(IModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            _model = model;

            InitializeComponent();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            _model.Initialize();

            textCurrentState.DataBindings.Add("Text", _model, "CurrentState");
            comboAvailableEvents.DataSource = _model.AvailableEvents;
            buttonAct.DataBindings.Add("Enabled", _model, "IsAnyEventAvailable");
        }

        private void buttonAct_Click(object sender, EventArgs e)
        {
            _model.Transition(comboAvailableEvents.Text);
        }
    }
}
