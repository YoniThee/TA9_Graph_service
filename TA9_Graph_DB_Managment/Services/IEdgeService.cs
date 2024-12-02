using TA9_Graph_DB_Managment.Shaphes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA9_Graph_DB_Managment.Services
{
    public interface IEdgeService
    {
        public Task<IEnumerable<Edge>> GetAllEdgesAsync();
        public Task<Edge> GetEdgeByIdAsync(string edgeId);

        public Task<Edge> CreateEdgeAsync(Edge edge);
        public Task<Edge> UpdateEdgeAsync(Edge edge);
        public Task DeleteEdgeAsync(string edgeId);


    }
}
