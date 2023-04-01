using System;
using System.Drawing;
using System.Windows.Forms;




namespace lab2_2
{
    public partial class Form1 : Form
    {
        public int COUNT = 10;

        public class Node
        {
            public int Key;
            public int height;
            public Node left;
            public Node right;
            public int R = 15;

            public Node(int key)
            {
                Key= key;
                left = null; 
                right = null;
                height= 1;
            }
            internal void Risovat(Graphics g, int x, int y, int dx, int dy)
            {
                if (left != null)
                    g.DrawLine(Pens.Black, x, y, x - dx / 2, y + dy);
                if (right != null)
                    g.DrawLine(Pens.Black, x, y, x + dx / 2, y + dy);

                g.FillEllipse(Brushes.White, x - R, y - R, 2 * R, 2 * R);
                g.DrawEllipse(Pens.Black, x - R, y - R, 2 * R, 2 * R);
                g.DrawString(Key.ToString(), new Font("Calibri", 10), Brushes.Black, x - 8, y - 8);
                g.DrawString(height.ToString(), new Font("Calibri", 10), Brushes.Red, x - 20, y - 30);

                if (left != null)
                    left.Risovat(g, x - dx / 2, y + dy, dx / 2, dy);
                if (right != null)
                    right.Risovat(g, x + dx / 2, y + dy, dx / 2, dy);
            }
        }

        class Tree
        {
            public string txtnodes;
            public int comp = 1;
            public Node root;

            public void PreOrder(Node localroot)
            {
                if (localroot != null)
                {
                    txtnodes += (Convert.ToString(localroot.Key) + " ");
                    PreOrder(localroot.left);
                    PreOrder(localroot.right);
                }
            }


            public Node Insert(Node node, int key)
            {
                // Если дерево пустое, создаем новый узел с ключом
                if (node == null)
                    return new Node(key);

                // Если ключ меньше ключа узла, вставляем его в левое поддерево
                if (key < node.Key)
                    node.left = Insert(node.left, key);

                // Если ключ больше ключа узла, вставляем его в правое поддерево
                else if (key > node.Key)
                    node.right = Insert(node.right, key);

                // Если ключ равен ключу узла, ничего не делаем (предполагаем, что дерево не содержит дубликатов)
                else
                    return node;

                // Обновляем высоту узла после вставки
                UpdateHeight(node);
                return node;
            }

            public void Delete(Node localroot)
            {
                if (localroot != null)
                {
                    txtnodes += (Convert.ToString(localroot.Key) + " ");
                    if (localroot.left != null)
                    {
                        if (localroot.left.left == null && localroot.left.right == null)
                        {
                            localroot.left = null;
                        }
                        else { Delete(localroot.left); }
                    }
                    if (localroot.right != null)
                    {
                        if (localroot.right.left == null && localroot.right.right == null)
                        {
                            localroot.right = null;
                        }
                        else { Delete(localroot.right); }
                    }
                }
            }

            public int GetHeight(Node node)
            {
                if (node == null)
                    return 0;
                return node.height;
            }

            public int GetBalance(Node node)
            {
                if (node == null)
                    return 0;
                return GetHeight(node.left) - GetHeight(node.right);
            }

            public void UpdateHeight(Node node)
            {
                if (node == null)
                    return;
                node.height = Math.Max(GetHeight(node.left), GetHeight(node.right)) + 1;
            }

            public Node RightRotate(Node y)
            {
                Node x = y.left;
                Node z = x.right;

                // Поворот
                x.right = y;
                y.left = z;

                // Обновление высот
                UpdateHeight(y);
                UpdateHeight(x);

                // Возвращаем новый корень поддерева
                return x;
            }

            public Node LeftRotate(Node x)
            {
                Node y = x.right;
                Node z = y.left;

                // Поворот
                y.left = x;
                x.right = z;

                // Обновление высот
                UpdateHeight(x);
                UpdateHeight(y);

                // Возвращаем новый корень поддерева
                return y;
            }

            public void Balance(Node node)
            {
                int balance = GetBalance(node);

                // Если узел несбалансирован, то есть 4 случая

                // Левый левый случай
                if (balance > 1 && node.Key < node.left.Key)
                    RightRotate(node);

                // Правый правый случай
                if (balance < -1 && node.Key > node.right.Key)
                    LeftRotate(node);

                // Левый правый случай
                if (balance > 1 && node.Key > node.left.Key)
                {
                    node.left = LeftRotate(node.left);
                    RightRotate(node);
                }

                // Правый левый случай
                if (balance < -1 && node.Key < node.right.Key)
                {
                    node.right = RightRotate(node.right);
                    LeftRotate(node);
                }
            }

            public bool Find(int key) // Поиск узла с заданным ключом
            { // (предполагается, что дерево не пустое)
                Node current = root; // Начать с корневого узла
                while (current.Key != key) // Пока не найдено совпадение
                {
                    if (key < current.Key) // Двигаться налево?
                        current = current.left;
                    else
                        current = current.right; // Или направо?
                    comp++;
                    if (current == null) // Если потомка нет,
                        return false; // поиск завершился неудачей
                }
                return true; // Элемент найден
            }


            public int Maxdepth(Node root)
            {
                if (root == null) return 0;
                else return Math.Max(Maxdepth(root.left), Maxdepth(root.right)) + 1;
            }


        }



        //public void getout(Node root, int space)
        //{
        //    if (root == null)
        //        return;

        //    // Increase distance between levels
        //    space += COUNT;

        //    // Process right child first
        //    getout(root.right, space);

        //    // Print current node after space
        //    // count
        //    textBox5.Text += "\r\n";
        //    for (int i = COUNT; i < space; i++)
        //        textBox5.Text += " ";
        //    textBox5.Text += Convert.ToString(root.Key) + "\r\n";


        //    // Process left child
        //    getout(root.left, space);
        //}

        Tree theTree = new Tree();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            theTree.root = null;
            int[] arr = Array.ConvertAll(textBox1.Text.Split(), s => int.Parse(s));
            foreach (var VARIABLE in arr)
            {
                theTree.root=theTree.Insert(theTree.root, VARIABLE);
            }

            //Graphics g = panel1.CreateGraphics();
            //theTree.root.Risovat(g, panel1.Width / 2, 50, panel1.Height / 2, 50);
            button6_Click(sender, e);
            //textBox5.Text = "";
            //getout(theTree.root, 0);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            theTree.txtnodes = "";
            textBox2.Text = "";
            theTree.PreOrder(theTree.root);
            textBox2.Text = theTree.txtnodes;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            theTree.Delete(theTree.root);

            button6_Click(sender, e);
            //textBox5.Text = "";
            //getout(theTree.root, 0);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Graphics g = panel1.CreateGraphics();
            g.Clear(Color.White);
            theTree.root.Risovat(g, panel1.Width / 2, 50, panel1.Height / 2, 50);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            theTree.Balance(theTree.root);
            button6_Click(sender, e);
        }
    }
}
