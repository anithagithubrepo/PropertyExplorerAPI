using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using PropertyExplorerAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PropertyExplorerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public PropertyController(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }


        
        [HttpGet("properties")]
        public async Task<IActionResult> GetJsonFromBlob()
        {
            string? blobUri = _configuration["AzureBlob:BlobUri"];

            if (string.IsNullOrEmpty(blobUri))
            {
                return BadRequest("Blob URI is not configured properly.");
            }

            var blobClient = new BlobClient(new Uri(blobUri));

            try
            {
                var downloadInfo = await blobClient.DownloadAsync();
                var properties = await JsonSerializer.DeserializeAsync<List<Property>>(downloadInfo.Value.Content);

                if (properties == null)
                {
                    return BadRequest("Failed to parse property data.");
                }

                return Ok(properties);
            }
            catch (Azure.RequestFailedException ex) when (ex.Status == 404)
            {
                // Blob not found
                return NotFound("The requested property data file was not found in blob storage.");
            }
            catch (Azure.RequestFailedException ex) when (ex.Status == 403 || ex.Status == 401)
            {
                // Invalid SAS token or unauthorized
                return Unauthorized("Access to blob storage is denied. Please check your SAS token or permissions.");
            }
            catch (Azure.RequestFailedException ex)
            {
                // Other Azure-specific errors
                return StatusCode(502, $"Azure Blob Storage error: {ex.Message}");
            }
            catch (JsonException)
            {
                // JSON parsing error
                return BadRequest("The property data file contains invalid JSON.");
            }
            catch (Exception ex)
            {
                // General/unexpected errors
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
