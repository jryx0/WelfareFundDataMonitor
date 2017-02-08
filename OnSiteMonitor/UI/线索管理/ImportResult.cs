using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OnSiteFundComparer.UI.线索管理
{
    public partial class ImportResult : Form
    {
        private DataSet _dataSource;
        public ImportResult(DataSet ds)
        {
            InitializeComponent();

            _dataSource = ds;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (_dataSource != null)
                this.dataGridView1.DataSource = _dataSource.Tables[0];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
