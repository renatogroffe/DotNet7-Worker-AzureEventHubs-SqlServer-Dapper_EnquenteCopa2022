using Microsoft.Data.SqlClient;
using Dapper.Contrib.Extensions;
using WorkerEventHubsCopa2022.EventHubs;

namespace WorkerEventHubsCopa2022.Data;

public class VotacaoRepository
{
    private readonly IConfiguration _configuration;

    public VotacaoRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Save(QuestaoEventData voto)
    {
        using var conexao = new SqlConnection(
            _configuration.GetConnectionString("BaseVotacao"));
        conexao.Insert<VotoTecnologia>(new()
        {
            DataProcessamento = DateTime.UtcNow.AddHours(-3),
            EventHub = _configuration["AzureEventHubs:EventHub"],
            Producer = voto.Producer,
            Consumer = Environment.MachineName,
            ConsumerGroup = _configuration["AzureEventHubs:ConsumerGroup"],
            HorarioVoto = voto.Horario,
            IdVoto = voto.IdVoto,
            Escolha = voto.Pais
        });
    }
}