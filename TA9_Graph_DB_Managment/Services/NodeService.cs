using Microsoft.EntityFrameworkCore;
using TA9_Graph_DB_Managment.Shaphes;
using TA9_Graph_DB_Managment.SharedDb;

namespace TA9_Graph_DB_Managment.Services
{
    public class NodeService : INodeService
    {
        private readonly GraphDbContext _dbContext;

        public NodeService(GraphDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Node> CreateNodeAsync(Node node)
        {
            try
            {
                var existingNode = isNodeExsit(node.NodeId);
                if (existingNode != null)
                {
                    throw new ArgumentException($"Node with Id {node.NodeId} is alreay exist");
                }
                _dbContext.Nodes.Add(node);
                await _dbContext.SaveChangesAsync();
                return node;
            }
            catch (Exception ex)
            {
                return null;

            }
        }
        public async Task DeleteNodeAsync(string nodeId)
        {
            var existingNode = isNodeExsit(nodeId);
            if (existingNode != null)
            {
                var edges = _dbContext.Edges.ToListAsync().Result;
                foreach (var neighbor in existingNode.Neighbors)
                {
                    _dbContext.Nodes.FindAsync(neighbor).Result.Neighbors.Remove(nodeId);
                    foreach (var edge in edges)
                    {
                        if ((edge.Node2_id == nodeId && edge.Node1_id == neighbor)
                            || (edge.Node1_id == nodeId && edge.Node2_id == neighbor))
                        {
                            _dbContext.Edges.Remove(edge);
                        }
                    }
                }

                _dbContext.Nodes.Remove(existingNode);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException($"Node with Id {nodeId} does not exist");
            }
        }

        public async Task<IEnumerable<Node>> GetAllNodesAsync() => await _dbContext.Nodes.ToListAsync();

        public async Task<Node> GetNodeByIdAsync(string nodeId) => await _dbContext.Nodes.FirstOrDefaultAsync(n => n.NodeId == nodeId);

        public async Task<Node> UpdateNodeAsync(Node node)
        {
            var existingNode = await _dbContext.Nodes.FindAsync(node.NodeId);

            if (existingNode == null)
            {
                return null;
            }
            existingNode.Data = node.Data;
            await _dbContext.SaveChangesAsync();

            return existingNode;
        }

        private Node? isNodeExsit(string nodeId)
        {
            return _dbContext.Nodes.FirstOrDefaultAsync(n => n.NodeId == nodeId).Result;
        }
    }
}
