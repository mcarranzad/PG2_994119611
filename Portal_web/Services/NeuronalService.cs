using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Newtonsoft.Json;
using sgc.ml.Models;
using sgc.ml.Services.Interfaces;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace sgc.ml.Services;

public class NeuronalService : INeuronalService
{
    private readonly HttpClient httpClient;
    private readonly ILogger<NeuronalService> logger;

    private string path = "http://localhost:8080";

    public NeuronalService(HttpClient httpClient, ILogger<NeuronalService> logger)
    {
        this.httpClient = httpClient;
        this.httpClient.Timeout = TimeSpan.FromSeconds(45);
        this.logger = logger;
    }
    
    
    /// <summary>
    /// Este metodo se usa para crear un usuario de forma manual
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<EvaluationResponse> GetResponse(EvaluationItems model)
    {
        try
        {
            // var query = CsnFunctions.JsonDebug(model);
            string requestUrl = $"{path}/predictions";

            httpClient.DefaultRequestHeaders.Add("Authorization","admin2023$123456");     
            
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(requestUrl, model);
            string jsonContent = await response.Content.ReadAsStringAsync();

            var authenticationResponse = JsonConvert.DeserializeObject<EvaluationResponse>(jsonContent);

            // EvaluationResponse? authenticationResponse = JsonSerializer
            //     .Deserialize<EvaluationResponse>(jsonContent, new JsonSerializerOptions
            //     {
            //         PropertyNameCaseInsensitive = true
            //     });

            if (!response.IsSuccessStatusCode)
            {
                authenticationResponse.result = false;
            }

            return authenticationResponse;
        }

        catch (TimeoutException ex)
        {
            return new EvaluationResponse
                { result = false, msg = "Tiempo de espera prolongado del servidor de digifact" };
        }
        catch (HttpRequestException ex)
        {
            return new EvaluationResponse
                { result = false, msg = "Error durante la respuesta del servicio de digifact" };
        }
        catch (WebException ex)
        {
            return new EvaluationResponse
                { result = false, msg = "Error durante la consulta de informacion de digifact" };
        }
        catch (Exception ex)
        {
            return new EvaluationResponse { result = false, msg = $"Error general del sistema {ex.Message} " };
        }
    }
}