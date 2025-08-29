using System.Threading;
using System.Threading.Tasks;

namespace HL7ResultsGateway.Application.UseCases.ProcessHL7Message;

public interface IProcessHL7MessageHandler
{
    Task<ProcessHL7MessageResult> Handle(ProcessHL7MessageCommand command, CancellationToken cancellationToken);
}
