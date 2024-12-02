using Microsoft.AspNetCore.Mvc;
using TA9_Graph_DB_Managment.Services;
using TA9_Graph_DB_Managment.Shaphes;
using TA9_Graph_DB_Managment;
using System.Xml.Linq;
using TA9_Graph_service.ShapesData;
using Swashbuckle.AspNetCore.Annotations;


namespace TA9_Graph_service.Controllers
{
    [ApiController]
    [Route("[controller]")]  
    public class NodesController : ControllerBase
    {
        private readonly INodeService _nodeService;
        private readonly ILogger<NodesController> _logger;


        public NodesController(INodeService nodeService, ILogger<NodesController> logger)
        {
            _nodeService = nodeService;
            _logger = logger;
        }

        /// <summary>
        /// Get All Nodes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, "Request is send seccessfully", typeof(NodeDataResponse))]

        public ActionResult<IEnumerable<NodeDataResponse>> GetAll()
        {
            var nodes = _nodeService.GetAllNodesAsync().Result;
            return Ok(nodes);
        }
        /// <summary>
        /// Get specific Node
        /// </summary>
        /// <param name="NodeId"> The Specific Node to Get</param>
        /// <returns></returns>
        [HttpGet("{nodeId}")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request is send seccessfully", typeof(NodeDataResponse))]
        public ActionResult<NodeDataResponse> Get([FromRoute] string nodeId)
        {
            var node =  _nodeService.GetNodeByIdAsync(nodeId).Result;
            if (node == null)
            {
                return NotFound();
            }
            return Ok(node);
        }




        /// <summary>
        /// Crate new Node
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status201Created, "Request is  send seccessfully", typeof(NodeDataRequest))]
        public IActionResult Post([FromBody] NodeDataRequest nodeData)
        {
            var node = new Node(nodeData.Data)
            {
                NodeId = Guid.NewGuid().ToString(),
            };
            var createdNode = _nodeService.CreateNodeAsync(node).Result;
            if (createdNode == null)
            {
                return BadRequest("Failed to create node. Please check the neighbor node existence.");
            }
            return Ok(createdNode);
        }
        /// <summary>
        /// Update exist Node
        /// </summary>
        /// <param name="NodeId"> Update exist Node</param>
        /// <returns></returns>
        [HttpPut("{nodeId}")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request is send seccessfully", typeof(NodeDataRequest))]
        public IActionResult Put(string nodeId, [FromBody] NodeDataRequest nodeData)

        {
            var updatedNode = _nodeService.GetNodeByIdAsync(nodeId).Result;
            if (updatedNode == null)
            {
                return NotFound();
            }
            updatedNode.Data = nodeData.Data;
            _nodeService.UpdateNodeAsync(updatedNode);
            return Ok(updatedNode);
        }
        /// <summary>
        /// Delete specific Node
        /// </summary>
        /// <param name="NodeId"> Delete Specific Node</param>
        /// <returns></returns>
        [HttpDelete("{nodeId}")]
        [SwaggerResponse(StatusCodes.Status200OK, "Node is deleted")]
        public IActionResult Delete([FromRoute] string nodeId)
        {
            try
            {
                _nodeService.DeleteNodeAsync(nodeId);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
            return Ok();
        }

    }
 
}
