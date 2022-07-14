using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PermutationsApp.DataModels;
using PermutationsApp.Services;

namespace PermutationsApp.Controllers;

[ApiController]
[Route("api/v1")]
public class PermutationsController
{
    
    [HttpGet]
    [Route("stats")]
    public JsonResult GetStats([FromServices] PermutationsService service)
    {
        try
        {
            var stats = service.GetStats();
        
            //Return the stats in json format
            return new JsonResult(new {totalWords = stats.TotalWords, 
                totalRequests = stats.TotalRequests,
                avgProcessingTimeNs = stats.AvgProcessingTimeNs
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpGet]
    [Route("similar")]
    public async Task<JsonResult> GetSimilarWords([FromRoute] ApiWordObj apiWordObj, 
        [FromServices] StatsService statsService,
        [FromServices] PermutationsService permutationsService)
    {
        try
        {
            //Get start time of the request to measuring processing time
            var start = GetNanoseconds();
            var result = await permutationsService.SimilarWords(apiWordObj.Word);
            
            var end = GetNanoseconds();
            var processingTime = end - start;
            
            //Updating the stats
            statsService.UpdateStats(processingTime);
            
            return new JsonResult(new {similar = result});
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private static long GetNanoseconds()
    {
        double timestamp = Stopwatch.GetTimestamp();
        double nanoseconds = 1_000_000_000.0 * timestamp / Stopwatch.Frequency;

        return (long)nanoseconds;
    }
}