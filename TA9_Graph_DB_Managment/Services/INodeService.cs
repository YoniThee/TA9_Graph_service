using TA9_Graph_DB_Managment.Shaphes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA9_Graph_DB_Managment.Services
{
    public interface INodeService
    {
        public Task<IEnumerable<Node>> GetAllNodesAsync();
        public Task<Node> GetNodeByIdAsync(string nodeId);

        public Task<Node> CreateNodeAsync(Node node);
        public Task<Node> UpdateNodeAsync(Node node);
        public Task DeleteNodeAsync(string nodeId);


    }
}
