using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using HL7ResultsGateway.Domain.Services;
using HL7ResultsGateway.Domain.Exceptions;

namespace HL7ResultsGateway.Application.UseCases.ProcessHL7Message
{
    public class ProcessHL7MessageHandler : IProcessHL7MessageHandler
    {
        private readonly IHL7MessageParser _parser;
        private readonly ILogger<ProcessHL7MessageHandler> _logger;

        public ProcessHL7MessageHandler(
            IHL7MessageParser parser,
            ILogger<ProcessHL7MessageHandler> logger)
        {
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ProcessHL7MessageResult> Handle(
            ProcessHL7MessageCommand command,
            CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            cancellationToken.ThrowIfCancellationRequested();

            _logger.LogInformation("Processing HL7 message from source: {Source}", command.Source);

            try
            {
                var parsedResult = _parser.ParseMessage(command.HL7Message);

                _logger.LogInformation(
                    "Successfully parsed HL7 message. MessageType: {MessageType}, PatientId: {PatientId}",
                    parsedResult.MessageType,
                    parsedResult.Patient?.PatientId);

                return new ProcessHL7MessageResult(
                    Success: true,
                    ParsedMessage: parsedResult,
                    ErrorMessage: null,
                    ProcessedAt: DateTime.UtcNow);
            }
            catch (HL7ParseException ex)
            {
                _logger.LogWarning(ex, "Failed to parse HL7 message from source: {Source}", command.Source);

                return new ProcessHL7MessageResult(
                    Success: false,
                    ParsedMessage: null,
                    ErrorMessage: ex.Message,
                    ProcessedAt: DateTime.UtcNow);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid HL7 message from source: {Source}", command.Source);

                return new ProcessHL7MessageResult(
                    Success: false,
                    ParsedMessage: null,
                    ErrorMessage: ex.Message,
                    ProcessedAt: DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error processing HL7 message from source: {Source}", command.Source);

                return new ProcessHL7MessageResult(
                    Success: false,
                    ParsedMessage: null,
                    ErrorMessage: "An unexpected error occurred while processing the HL7 message",
                    ProcessedAt: DateTime.UtcNow);
            }
        }
    }
}