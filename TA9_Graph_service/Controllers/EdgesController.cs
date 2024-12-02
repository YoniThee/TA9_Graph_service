using Microsoft.AspNetCore.Mvc;
using TA9_Graph_DB_Managment.Services;
using TA9_Graph_DB_Managment.Shaphes;
using Swashbuckle.AspNetCore.Annotations;
using TA9_Graph_service.ShapesData;

namespace TA9_Graph_service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EdgesController : ControllerBase
    {
        private readonly IEdgeService _edgeService;
        private readonly ILogger<EdgesController> _logger;


        public EdgesController(IEdgeService edgeService, ILogger<EdgesController> logger)
        {
            _edgeService = edgeService;
            _logger = logger;
        }

        /// <summary>
        /// Get All Edges
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, "Request is send seccessfully", typeof(EdgeDataResponse))]

        public ActionResult<IEnumerable<EdgeDataResponse>> GetAll()
        {
            var edges = _edgeService.GetAllEdgesAsync().Result;
            return Ok(edges);
        }
        /// <summary>
        /// Get specific Edge
        /// </summary>
        /// <param name="edgeId"> The Specific Edge to Get</param>
        /// <returns></returns>
        [HttpGet("{edgeId}")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request is send seccessfully", typeof(EdgeDataResponse))]
        public ActionResult<EdgeDataResponse> Get([FromRoute] string edgeId)
        {
            var edge =  _edgeService.GetEdgeByIdAsync(edgeId).Result;
            if (edge == null)
            {
                return NotFound();
            }
            return Ok(edge);
        }




        /// <summary>
        /// Crate new Edge
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK, "Request is send seccessfully", typeof(EdgeDataRequest))]
        public IActionResult Post([FromBody] EdgeDataRequest edgeData)
        {
            var edge = new Edge()
            {
                EdgeId = Guid.NewGuid().ToString(),
                Node1_id = edgeData.Node1_id,
                Node2_id = edgeData.Node2_id,
            };
            var createdEdge = _edgeService.CreateEdgeAsync(edge).Result;
            if (createdEdge == null)
            {
                return BadRequest("Failed to create Edge. Please check the neighbor node existence.");
            }
            return Ok(createdEdge);
        }
        /// <summary>
        /// Update exist Edge
        /// </summary>
        /// <param name="edgeId"> Update exist Edge</param>
        /// <returns></returns>
        [HttpPut("{edgeId}")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request is send seccessfully", typeof(EdgeDataRequest))]
        public IActionResult Put([FromRoute]string edgeId, [FromBody] EdgeDataRequest edgeData)

        {
            var updatedEdge = _edgeService.GetEdgeByIdAsync(edgeId).Result;
            if (updatedEdge == null)
            {
                return NotFound();
            }
            updatedEdge.Node1_id = edgeData.Node1_id;
            updatedEdge.Node2_id = edgeData.Node2_id;
            _edgeService.UpdateEdgeAsync(updatedEdge);
            return Ok(updatedEdge);
        }
        /// <summary>
        /// Delete specific Edge
        /// </summary>
        /// <param name="edgeId"> Delete Specific Edge</param>
        /// <returns></returns>
        [HttpDelete("{edgeId}")]
        [SwaggerResponse(StatusCodes.Status200OK, "Edge is deleted")]
        public IActionResult Delete([FromRoute] string edgeId)
        {
            try
            {
                _edgeService.DeleteEdgeAsync(edgeId);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
            return Ok();
        }

    }
 
}
