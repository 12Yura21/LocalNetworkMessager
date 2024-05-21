namespace LocalNetworkMessager
{
    public partial class Form1 : Form
    {
        public event Action Disconnect;
        public event Action<string> Connect;
        public event Action<string> SendMessage;
        public event Func<string> CreateServer;
        public event Action CloseServer;



        public Form1()
        {
            InitializeComponent();
            
        }

        public void AddTextToLIst(string message)
        {
            listBox1.Items.Add(message);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Connect?.Invoke(textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Disconnect?.Invoke();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string message = textBox2.Text;
            SendMessage?.Invoke(message);
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            CloseServer?.Invoke();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            CreateServer?.Invoke();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            
        }
    }
}