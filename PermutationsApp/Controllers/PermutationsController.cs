using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PermutationsApp.DataModels;
using PermutationsApp.Services;
using PermutationsApp.Singletons;

namespace PermutationsApp.Controllers;

[ApiController]
[Route("api/v1")]
public class PermutationsController
{
    
    [HttpGet]
    [Route("stats")]
    public JsonResult GetStats()
    {
        try
        {
            var service = new PermutationsService();
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
    public JsonResult GetSimilarWords([FromRoute] ApiWordObj apiWordObj)
    {
        try
        {
            //Get start time of the request to measuring processing time
            var start = GetNanoseconds();
            
            //Count the number of requests
            StatsSingleton.Instance.TotalRequests++;
            
            var service = new PermutationsService();
            var result = service.SimilarWords(apiWordObj.Word);

            var end = GetNanoseconds();
            
            //Calculate processing time and sum it
            StatsSingleton.Instance.SumProcessingTimeNs += (int)(end - start);
        
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