using Microsoft.EntityFrameworkCore;
using TA9_Graph_DB_Managment.Shaphes;
using TA9_Graph_DB_Managment.SharedDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;

namespace TA9_Graph_DB_Managment.Services
{
    public class EdgeService : IEdgeService
    {
        private readonly GraphDbContext _dbContext;

        public EdgeService(GraphDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Edge> CreateEdgeAsync(Edge edge)
        {
            try
            {
                var existingEdge = isEdgeExsit(edge.EdgeId);
                if (existingEdge != null)
                {
                    throw new ArgumentException($"Edge with Id {edge.EdgeId} is alreay exist");
                }

                var existingEdgeBetweenNodes = _dbContext.Edges
                        .Any(e => (e.Node1_id == edge.Node1_id && e.Node2_id == edge.Node2_id) ||
                     (e.Node2_id == edge.Node1_id && e.Node1_id == edge.Node2_id));

                if (existingEdgeBetweenNodes)
                {
                    throw new ArgumentException($"Edge between nodes {edge.Node2_id} and {edge.Node1_id} already exists.");
                }
                _dbContext.Edges.Add(edge);
                // update the neighbors list of each node
                try
                {
                    _dbContext.Nodes.FindAsync(edge.Node1_id).Result.Neighbors.Add(edge.Node2_id);
                    _dbContext.Nodes.FindAsync(edge.Node2_id).Result.Neighbors.Add(edge.Node1_id);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Node dosent exist!");

                }
                await _dbContext.SaveChangesAsync();
                return edge;
            }
            catch (Exception ex)
            {
                return null;

            }
        }



        public async Task DeleteEdgeAsync(string edgeId)
        {
            var existingEdge = isEdgeExsit(edgeId);
            if (existingEdge == null)
            {
                throw new ArgumentException($"Edge with ID {edgeId} does not exist");
            }

            try
            {
                _dbContext.Nodes.FindAsync(existingEdge.Node1_id).Result.Neighbors.Remove(existingEdge.Node2_id);
                _dbContext.Nodes.FindAsync(existingEdge.Node2_id).Result.Neighbors.Remove(existingEdge.Node1_id);
                _dbContext.Edges.Remove(existingEdge);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete edge: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Edge>> GetAllEdgesAsync() => await _dbContext.Edges.ToListAsync();

        public async Task<Edge> GetEdgeByIdAsync(string edgeId) => await _dbContext.Edges.FirstOrDefaultAsync(e => e.EdgeId == edgeId);

        public async Task<Edge> UpdateEdgeAsync(Edge edge)
        {
            var existingEdge = await _dbContext.Edges.FindAsync(edge.EdgeId);

            if (existingEdge == null)
            {
                return null;
            }

            var existingEdgeBetweenNodes = _dbContext.Edges.Any(e => (e.Node1_id == edge.Node1_id && e.Node2_id == edge.Node2_id)
                                                || (e.Node2_id == edge.Node1_id && e.Node1_id == edge.Node2_id));

            if (existingEdgeBetweenNodes)
            {
                throw new ArgumentException($"Edge between nodes {edge.Node2_id} and {edge.Node1_id} already exists.");
            }

            if (_dbContext.Nodes.FindAsync(edge.Node1_id).Result != null
                && _dbContext.Nodes.FindAsync(edge.Node2_id).Result != null)
            {
                // update the nodes at the edge
                existingEdge.Node1_id = edge.Node1_id;
                existingEdge.Node2_id = edge.Node2_id;

                // update the neighbors at the nodes
                _dbContext.Nodes.FindAsync(edge.Node1_id).Result.Neighbors.Add(edge.Node2_id);
                _dbContext.Nodes.FindAsync(edge.Node2_id).Result.Neighbors.Add(edge.Node1_id);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException($"Node dosent exist!");
            }

            return existingEdge;
        }

        private Edge? isEdgeExsit(string edgeId)
        {
            return _dbContext.Edges.FirstOrDefaultAsync(e => e.EdgeId == edgeId).Result;
        }
    }
}
