namespace TA9_Graph_DB_Managment.Shaphes
{
    public class Node
    {
        public string NodeId { get; set; }
        public int Data { get; set; }
        public List<string> Neighbors { get; set; }

        public Node(int data) 
        {
            Data = data;
            Neighbors = new List<string>();
        }
    }
}
