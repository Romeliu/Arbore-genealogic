using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//Datele de intrare pot fi modificicate in fisierul "Informatii.txt", aflat in folderul
//"Arbore genealogic"

namespace Arbore_genealogic
{
    public partial class Form1 : Form
    {
        int numar_de_persoane;
        string[] text;
        int[] vector_de_tati=new int[101];
        int[] numar_copii=new int[101];
        Dictionary<string, int> nume = new Dictionary<string, int>();
        string[] name = new string[102];
        public Form1()
        {
            InitializeComponent();
            text = System.IO.File.ReadAllLines(@"../../Informatii.txt");
            numar_de_persoane = Int32.Parse(text[0].Trim());
            for (int i=1;i<=numar_de_persoane;i++)
            {
                vector_de_tati[i] = Int32.Parse((text[1].Split(' ')[i-1]).ToString().Trim());
                nume.Add(text[2].Split(' ')[i - 1].Trim(),i);
                name[i] = text[2].Split(' ')[i - 1].Trim();
                numar_copii[vector_de_tati[i]]++;
                if (vector_de_tati[i] == 0)
                    treeView1.Nodes.Add(name[i]);
                comboBox1.Items.Add(name[i]);
                comboBox2.Items.Add(name[i]);
                comboBox3.Items.Add(name[i]);
            }
            treeView1.SelectedNode = treeView1.Nodes[0];
            Nod_nou(1);
            treeView1.SelectedNode = null;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int value = 0;
            label3.Text = "";
            nume.TryGetValue(comboBox1.Text, out value);
            label3.Visible = true;
            switch (numar_copii[value])
            {
                case 0:
                    label3.Text = string.Format("{0} nu are copii", comboBox1.Text);
                    break;
                case 1:
                    label3.Text = string.Format("{0} are {1} copil: ", comboBox1.Text, numar_copii[value]);
                    for (int i = 1; i <= numar_de_persoane; i++)
                    {
                        if (vector_de_tati[i] == value)
                            label3.Text += name[i] + " ";
                    }
                    break;
                default:
                    label3.Text = string.Format("{0} are {1} copii: ", comboBox1.Text, numar_copii[value]);
                    int n = 0;
                    for (int i = 1; i <= numar_de_persoane; i++)
                    {
                        if (vector_de_tati[i] == value)
                        {
                            if (n != 0)
                                label3.Text += ", ";
                            label3.Text += name[i] + " ";
                            n++;
                        }
                    }
                    break;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label3.Visible = false;
            comboBox1.SelectedItem = null;
            label4.Visible = false;
            comboBox2.SelectedItem = null;
            comboBox3.SelectedItem = null;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            label4.Text = "";
            label4.Visible = true;
            if (comboBox2.Text != comboBox3.Text)
            {
                int value1, value2;
                nume.TryGetValue(comboBox2.Text, out value1);
                nume.TryGetValue(comboBox3.Text, out value2);
                if (Stramos_comun(value1, value2) != 0)
                {
                    label4.Text = string.Format("Stramosul comun al lui {0} si {1} este {2}", comboBox2.Text, comboBox3.Text, name[Stramos_comun(value1, value2)]);
                }
                else
                    label4.Text = string.Format("{0} si {1} nu au stramos comun", comboBox2.Text, comboBox3.Text);
            }
            else
                errorProvider2.SetError(comboBox3,"Va rugam selectati doua persoane diferite!");
        }

        int Stramos_comun(int a,int b)
        {
            int aux = b;
            while (vector_de_tati[a] != 0)
            {
                while (vector_de_tati[aux] != 0)
                {
                    if (vector_de_tati[a] == vector_de_tati[aux])
                        return vector_de_tati[a];
                    aux = vector_de_tati[aux];
                }
                aux = b;
                a = vector_de_tati[a];
            }
            return 0;
        }

        void Nod_nou(int j)
        {
            int ind = 1;
            int nr_nod = 0;
            while(ind<=numar_de_persoane)
            {
                int l;
                nume.TryGetValue(treeView1.SelectedNode.Text,out l);
                if(vector_de_tati[ind]==l)
                {
                    Console.WriteLine(name[ind]);
                    treeView1.SelectedNode.Nodes.Add(name[ind]);
                    treeView1.SelectedNode = treeView1.SelectedNode.Nodes[nr_nod];
                    Nod_nou(++j);
                    nr_nod++;
                    treeView1.ExpandAll();
                }
                ind++;
            }
            treeView1.SelectedNode = treeView1.SelectedNode.Parent;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try { errorProvider2.Dispose(); }
            catch { }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try { errorProvider2.Dispose(); }
            catch { }
        }
    }
}
