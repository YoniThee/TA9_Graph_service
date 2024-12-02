
namespace TA9_Graph_service.ShapesData
{
    public class NodeDataResponse
    {
        public string NodeId { get; set; }

        public int Data { get; set; }
        public List<string> Neighbors { get; set; }
    }
}
